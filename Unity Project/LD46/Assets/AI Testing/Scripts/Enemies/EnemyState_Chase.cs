using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Chase : NavAgentState
{
    private EnemyAI m_owner;
    private Animator animator;
    private LightDetection m_lightDetectionSystem;

    public EnemyState_Chase(StateMachine sMachine, EnemyAI owner) : base(sMachine, owner.GetNavMeshAgent())
    {
        this.m_owner = owner;
        animator = owner.GetComponent<Animator>();
        m_lightDetectionSystem = m_owner.GetComponent<LightDetection>();
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
        m_agent.speed = m_owner.GetBaseNavSpeed();
        //Check for the presence of light
        if (m_lightDetectionSystem.IsIlluminated())
        {          
            //If player light, then move into frozen state
            if(m_lightDetectionSystem.QueryFlags(LightEffectType.Burn))
            {
                Debug.Log("PLAYER is burning me");
                m_sMachine.SetState(new EnemyState_InPlayerLight(m_sMachine, m_owner));
            }
            else if (m_lightDetectionSystem.QueryFlags(LightEffectType.Stop))
            {
                Debug.Log("Hit stop light, start talking but stay in this state");
            }
            else if (m_lightDetectionSystem.QueryFlags(LightEffectType.SlowDown))
            {
                Debug.Log("In slow down light. Start talkling.");
                var nearest = m_lightDetectionSystem.GetNearestLightSource(LightEffectType.SlowDown, Utils.Project2D(m_owner.transform.position));

                float distRatio = nearest.Item2 / nearest.Item1.m_light.range;

                m_agent.speed = m_owner.m_lightSlowdownCurve.Evaluate(distRatio) * m_owner.GetBaseNavSpeed(); //Be clever, ramp up with distance
                //Debug.Log("ratio: " + distRatio + "   curve: " + m_owner.m_lightSlowdownCurve.Evaluate(distRatio) + "speed" +m_owner.m_lightSlowdownCurve.Evaluate(distRatio) + m_agent.speed);

            }
        }

        m_agent.SetDestination(Player.instance.transform.position);

        //Face model towards the player
        var modelDir = Utils.Project2D(Player.instance.transform.position) - Utils.Project2D(m_owner.transform.position);
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
        m_agent.speed = m_owner.GetBaseNavSpeed();
    }
}
