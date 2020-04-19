using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnPoint : MonoBehaviour
{
    public PickupType m_pickupType;
    public float m_pickupSpawnFrequency = 30;

    private bool m_pickupCollected = true;
    private float m_tSinceCollected = 0;


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    public void OnPickupCollected(object sender, EventArgs eventArgs)
    {
        m_pickupCollected = true;
        ((Pickup)sender).m_pickupCollectedEventHandler -= OnPickupCollected;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        m_tSinceCollected = m_pickupSpawnFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_pickupCollected)
        {
            m_tSinceCollected += Time.deltaTime;


            if(m_tSinceCollected >= m_pickupSpawnFrequency)
            {
                //Spawn a pickup
                Pickup pickup = PickupPoolManager.instance.GetPickup(m_pickupType);
                pickup.transform.position = transform.position;
                pickup.m_pickupCollectedEventHandler += OnPickupCollected;

                m_pickupCollected = false;
                m_tSinceCollected = 0;
            }
        }
       
    }

}
