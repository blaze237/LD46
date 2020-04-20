﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyGoalType
{
    Player,
    Tower
}
public class EnemyAI : MonoBehaviour
{
    //How close can player get before the enemy will begin to seek out the player
    public float m_seekRadius = 10;
    public float m_playerAttackAnimRadius = 1.5f;
    //How close should they get before starting their voice lines
    public float m_speechRadius = 5;
    public float m_tTillDeathFromLight = 5f;
    public AnimationCurve m_lightSlowdownCurve;
    public bool m_seeksTower = false;
    public int m_towerDamage = 5;
    public int m_playerDamage = 10;
    public float m_attackRate = 1;
    public float pickupSpawnChance = 0.5f;

    private int health = 100;
    public bool m_useLosCheck = false;
    //Y offset to apply before doing los checks to player. Needed for enemy avatars whos root is at the base of the model
    public float m_losVerticalOffset = 0;
    private StateMachine m_sMachine = new StateMachine();
    private float m_baseNavSpeed;
    private NavMeshAgent m_agent;
    private LightDetection m_lightDetectionSystem;
    private bool m_reachedTower = false;

    public event EventHandler m_enemyDeathEventHandler;



    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_seekRadius);
        Gizmos.DrawWireSphere(transform.position, m_playerAttackAnimRadius);
    }


    public virtual void Spawn(Vector3 pos)
    {

        //In order to set the enemies position freely, we must temporarily disable the object to prevent its position snapping according to navmesh
        gameObject.SetActive(false);
        transform.position = pos;
        health = 100;
        gameObject.SetActive(true);

    }

    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_baseNavSpeed = m_agent.speed;

        if (m_seeksTower)
        {
            m_sMachine.SetState(new EnemyState_Chase(m_sMachine, this));
        }
        else
        {
            m_sMachine.SetState(new EnemyState_Idle(m_sMachine, this));
        }

        m_lightDetectionSystem = GetComponent<LightDetection>();

    }

    void OnDeath()
    {
        float spwnChance = UnityEngine.Random.Range(0.0f, 1.0f);
        if (spwnChance <= pickupSpawnChance)
        {
            //Chance to spawn a random pickup on enemy death
            PickupType type = PickupType.Coffee;

            int ind = UnityEngine.Random.Range(0, (int)PickupType.s_SIZE);
            switch (ind)
            {
                case 0:
                    type = PickupType.Coffee;
                    break;
                case 1:
                    type = PickupType.Fuel;
                    break;
                case 2:
                    type = PickupType.Battery;
                    break;
                case 3:
                    type = PickupType.Tool;
                    break;
            }


            Pickup pickup = PickupPoolManager.instance.GetPickup(type);
            pickup.transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_agent.speed = m_baseNavSpeed;

        if(Input.GetKeyDown(KeyCode.P))
        {
            OnDeath();
            m_enemyDeathEventHandler?.Invoke(this, new EventArgs());
            gameObject.SetActive(false);
        }

        //Check for the presence of light
        if (m_lightDetectionSystem.IsIlluminated())
        {
            //If player light, then move into frozen state
            if (m_lightDetectionSystem.QueryFlags(LightEffectType.Burn))
            {
                Debug.Log("PLAYER is burning me");
                m_sMachine.SetState(new EnemyState_InPlayerLight(m_sMachine, this));
            }
            else if (m_lightDetectionSystem.QueryFlags(LightEffectType.Stop))
            {
                Debug.Log("Hit stop light, start talking but stay in this state");
            }
            else if (m_lightDetectionSystem.QueryFlags(LightEffectType.SlowDown))
            {
                Debug.Log("In slow down light. Start talkling.");
                var nearest = m_lightDetectionSystem.GetNearestLightSource(LightEffectType.SlowDown, Utils.Project2D(transform.position));

                float distRatio = nearest.Item2 / nearest.Item1.m_light.range;

                m_agent.speed = m_lightSlowdownCurve.Evaluate(distRatio) * m_baseNavSpeed;
                //Debug.Log("ratio: " + distRatio + "   curve: " + m_owner.m_lightSlowdownCurve.Evaluate(distRatio) + "speed" +m_owner.m_lightSlowdownCurve.Evaluate(distRatio) + m_agent.speed);

            }
        }


        m_sMachine.Update(Time.deltaTime);
    }

    public float GetBaseNavSpeed()
    {
        return m_baseNavSpeed;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return m_agent;
    }

    public bool HasReachedTower()
    {
        return m_reachedTower;
    }

    public bool losCheck()
    {
        if(!m_useLosCheck)
        {
            return true;
        }
        //Check for walls in front of player
        RaycastHit hit;
        int layerMask = ~(1 << this.gameObject.layer); //Dont care if we hit ourselves or another enemy 
        if (Physics.Raycast(transform.position + Vector3.up * m_losVerticalOffset, (Player.instance.transform.position - transform.position), out hit, Mathf.Infinity, layerMask))
            return hit.collider.gameObject.CompareTag("Player");

        return false;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TowerAttatch"))
        {
            m_reachedTower = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TowerAttatch"))
        {
            m_reachedTower = false;
        }
    }

}
