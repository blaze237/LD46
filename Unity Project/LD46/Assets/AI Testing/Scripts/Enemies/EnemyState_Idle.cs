﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Idle : NavAgentState
{
    private EnemyAI m_owner;

    public EnemyState_Idle(StateMachine sMachine, EnemyAI owner) : base(sMachine, owner.GetNavMeshAgent())
    {
        this.m_owner = owner;
    }

    public override void Enter()
    {
      
    }

    public override void Execute(float i_dt)
    {
        if (Vector2.Distance(Utils.Project2D(Player.instance.transform.position), Utils.Project2D(m_agent.transform.position)) <= m_owner.m_seekRadius)
        {
            m_sMachine.SetState(new EnemyState_Chase(m_sMachine, m_owner));
        }
    }

    public override void Exit()
    {
       
    }
}
