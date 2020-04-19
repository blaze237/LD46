using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Idle : NavAgentState
{
    private EnemyAI m_owner;

    public EnemyState_Idle(StateMachine sMachine, EnemyAI owner) : base(sMachine, owner.GetComponent<NavMeshAgent>())
    {
        this.m_owner = owner;
    }

    public override void Enter()
    {
      
    }

    public override void Execute(float i_dt)
    {
        if (Vector3.Distance(Player.instance.transform.position, m_agent.transform.position) <= m_owner.m_seekRadius)
        {
            m_sMachine.SetState(new EnemyState_Chase(m_sMachine, m_owner));
        }
    }

    public override void Exit()
    {
       
    }
}
