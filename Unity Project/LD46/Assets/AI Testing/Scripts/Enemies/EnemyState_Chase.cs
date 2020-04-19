using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Chase : NavAgentState
{
    private EnemyAI m_owner;
    private Animator animator;
    private LightDetection m_lightDetectionSystem;

    public EnemyState_Chase(StateMachine sMachine, EnemyAI owner) : base(sMachine, owner.GetComponent<NavMeshAgent>())
    {
        this.m_owner = owner;
        animator = owner.GetComponent<Animator>();
        m_lightDetectionSystem = m_owner.GetComponent<LightDetection>();
    }

    private Vector2 GetXZPos(Vector3 i_pos)
    {
        return new Vector2(i_pos.x, i_pos.z);
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
        //Check for the presence of light
        if(m_lightDetectionSystem.IsIlluminated())
        {
            //If player light, then move into frozen state
            if(m_lightDetectionSystem.QueryFlags(LightSourceType.Player))
            {
                Debug.Log("PLAYER IS SHINING ON ME");
                m_sMachine.SetState(new EnemyState_InPlayerLight(m_sMachine, m_owner));
            }
            //If enviromental light, just talk but carry on trying to navigate(wont be able to nav into light which will be marked as an obstacle)
            if (m_lightDetectionSystem.QueryFlags(LightSourceType.Enviromental))
            {
                Debug.Log("Enviroment IS SHINING ON ME");
            }
        }

        m_agent.SetDestination(Player.instance.transform.position);

        //Face model towards the player
        var modelDir = GetXZPos(Player.instance.transform.position) - GetXZPos(m_owner.transform.position);
        m_owner.transform.forward = Utils.Project3D(modelDir);

        //If player leaves wake radius, then stop following and attacking
        if (Vector3.Distance(Player.instance.transform.position, m_owner.transform.position) >= m_owner.m_seekRadius)
        {
            m_sMachine.SetState(new EnemyState_Idle(m_sMachine, m_owner));
        }
        //If reach min distance to player, begin to talk (maybe they should slow down and reach their arms out or something?)
        else if (Vector3.Distance(Player.instance.transform.position, m_owner.transform.position) <= m_owner.m_seekRadius)
        {
            
        }
    }

    public override void Exit()
    {

    }
}
