using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_InPlayerLight : NavAgentState
{
    private EnemyAI m_owner;
    private float m_tSinceStateStart = 0;
    private LightDetection m_lightDetectionSystem;

    public EnemyState_InPlayerLight(StateMachine sMachine, EnemyAI owner) : base(sMachine, owner.GetNavMeshAgent())
    {
        this.m_owner = owner;
        m_lightDetectionSystem = m_owner.GetComponent<LightDetection>();
        m_agent.isStopped = true;

    }

    public override void Enter()
    {

    }

    public override void Execute(float i_dt)
    {
        m_tSinceStateStart += i_dt;

        //if(m_tSinceStateStart >= m_owner.m_tTillDeathFromLight)
        //{
        //    Debug.Log("TIME TO DIE");
        //}

        //If player light, then move into frozen state
        if (!m_lightDetectionSystem.QueryFlags(LightEffectType.Burn))
        {
            Debug.Log("Im FREE");
            m_sMachine.SetState(new EnemyState_Chase(m_sMachine, m_owner));

        }
    }

    public override void Exit()
    {

    }
}
