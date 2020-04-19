using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Attack : NavAgentState
{
    private EnemyAI m_owner;
    private EnemyGoalType m_goalType;
    private float m_tSinceDmgDealt;

    public EnemyState_Attack(StateMachine sMachine, EnemyAI owner, EnemyGoalType i_goalType) : base(sMachine, owner.GetNavMeshAgent())
    {
        this.m_owner = owner;
        m_goalType = i_goalType;
        m_tSinceDmgDealt = m_owner.m_attackRate;
    }

    public override void Enter()
    {

    }

    public override void Execute(float i_dt)
    {
        //If goal is tower, do some damage to it and keep walking till reach min dist
        if(m_goalType == EnemyGoalType.Tower)
        {
            m_tSinceDmgDealt += i_dt;

            if (Mathf.Abs((Utils.Project2D(Tower.instance.transform.position) - Utils.Project2D(m_owner.transform.position)).magnitude) > Tower.instance.m_minDistToAttack)
            {
                m_agent.SetDestination(Tower.instance.transform.position);
            }
            else
            {
                m_agent.isStopped = true;
            }

            if (m_tSinceDmgDealt > m_owner.m_attackRate)
            {
                Tower.instance.AddDamage(m_owner.m_towerDamage);
                m_tSinceDmgDealt = 0;
            }
                       
            //If player is close, switch to searching for them
            if(Player.instance.Get2DDistToPlayer(m_owner.transform.position) <= m_owner.m_seekRadius)
            {
                m_sMachine.SetState(new EnemyState_Chase(m_sMachine, m_owner));
            }

        }
        //If goal is player, just play anim and keep walking towards the player (make enemies deal damage, its not instant death)
        else
        {
            m_agent.SetDestination(Player.instance.transform.position);


            //Player has escaped attack radius
            if (Player.instance.Get2DDistToPlayer(m_owner.transform.position) > m_owner.m_playerAttackAnimRadius)
            {
                m_sMachine.SetState(new EnemyState_Chase(m_sMachine, m_owner));
            }


        }






        //If leave trigger or player gets close, go back to chase state
    }

    public override void Exit()
    {

    }
}
