using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Chase : NavAgentState
{
    private EnemyAI m_owner;
    private Animator animator;
    EnemyGoalType m_curGoalType = EnemyGoalType.Player;
    private Vector3 m_cachedTowerGoalPoint;


    public EnemyState_Chase(StateMachine sMachine, EnemyAI owner) : base(sMachine, owner.GetNavMeshAgent())
    {
        this.m_owner = owner;
        animator = owner.GetComponent<Animator>();
    }


    private Vector3 GetGoalPos()
    {
        float dist2Player = Player.instance.Get2DDistToPlayer(m_owner.transform.position);// Mathf.Abs((Utils.Project2D(Player.instance.transform.position) - Utils.Project2D(m_owner.transform.position)).magnitude);
        //float dist2Tower = Tower.instance.Get2DDistToTower(m_owner.transform.position); //Mathf.Abs((Utils.Project2D(Tower.instance.transform.position) - Utils.Project2D(m_owner.transform.position)).magnitude);

        if (!m_owner.m_seeksTower || dist2Player <= m_owner.m_seekRadius) //Allways prefer the player to the tower
        {
            m_curGoalType = EnemyGoalType.Player;
            return Player.instance.transform.position;
        }

        //If goal type has just been set to tower, then need to find an attatch pos on the tower else just use the one allready found
        if (m_curGoalType != EnemyGoalType.Tower)
        {
            m_cachedTowerGoalPoint = Tower.instance.GetAttatchPoint();
        }
        m_curGoalType = EnemyGoalType.Tower;
        return m_cachedTowerGoalPoint;
    }


    public override void Enter()
    {
        m_agent.SetDestination(Player.instance.transform.position);
        m_agent.isStopped = false;
        if (animator != null)
        {
            animator.SetBool("moving", true);
        }
    }

    public override void Execute(float i_dt)
    {
        

        //Who is closer, the player or the tower?
        Vector3 goal = GetGoalPos();
                             
        m_agent.SetDestination(goal);

        //Face model towards the target
        Vector3 viewTarget = m_curGoalType == EnemyGoalType.Player ? Player.instance.transform.position : Tower.instance.transform.position;
        var modelDir = Utils.Project2D(viewTarget) - Utils.Project2D(m_owner.transform.position);
        if (modelDir != Vector2.zero)
        {
            m_owner.transform.forward = Utils.Project3D(modelDir);
        }
        //If player leaves wake radius, then stop following and attacking if we arent a tower seeker
        if (Vector3.Distance(Player.instance.transform.position, m_owner.transform.position) >= m_owner.m_seekRadius && !m_owner.m_seeksTower)
        {
            m_sMachine.SetState(new EnemyState_Idle(m_sMachine, m_owner));
        }

        //If goal type is player, move to attack state when reach min dist to player
        if (m_curGoalType == EnemyGoalType.Player && Vector3.Distance(Player.instance.transform.position, m_owner.transform.position) <= m_owner.m_playerAttackAnimRadius)
        {
            m_sMachine.SetState(new EnemyState_Attack(m_sMachine, m_owner, m_curGoalType));
        }
        //Similar for tower
        else if(m_curGoalType == EnemyGoalType.Tower && m_owner.HasReachedTower())
        {
            m_sMachine.SetState(new EnemyState_Attack(m_sMachine, m_owner, m_curGoalType));
        }
    }

    public override void Exit()
    {
        m_agent.speed = m_owner.GetBaseNavSpeed();
    }
}
