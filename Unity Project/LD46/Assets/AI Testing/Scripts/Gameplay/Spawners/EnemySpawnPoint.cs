using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public EnemyType m_enemyType;
    public float m_spawnFrequency = 30;

    private bool m_enenmyKilled = true;
    private float m_tSinceKilled = 0;


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    public void OnEnemyKilled(object sender, EventArgs eventArgs)
    {
        m_enenmyKilled = true;
        ((Pickup)sender).m_pickupCollectedEventHandler -= OnEnemyKilled;
    }


    //// Start is called before the first frame update
    //void Start()
    //{
    //    m_tSinceCollected = m_pickupSpawnFrequency;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (m_pickupCollected)
    //    {
    //        m_tSinceCollected += Time.deltaTime;


    //        if (m_tSinceCollected >= m_pickupSpawnFrequency)
    //        {
    //            //Spawn a pickup
    //            Pickup pickup = PickupPoolManager.instance.GetPickup(m_pickupType);
    //            pickup.transform.position = transform.position;
    //            pickup.m_pickupCollectedEventHandler += OnPickupCollected;

    //            m_pickupCollected = false;
    //            m_tSinceCollected = 0;
    //        }
    //    }

    //}

}
