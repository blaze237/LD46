using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnPoint : MonoBehaviour
{
    public PickupType m_pickupType;
    public float m_pickupSpawnFrequency = 30;
    public bool randomType = false;

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
                PickupType type = m_pickupType;

                if(randomType)
                {
                    int ind = UnityEngine.Random.Range(0, (int)PickupType.s_SIZE);

                    switch(ind)
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
                }
                               
                Pickup pickup = PickupPoolManager.instance.GetPickup(type);
                pickup.transform.position = transform.position;
                pickup.m_pickupCollectedEventHandler += OnPickupCollected;

                m_pickupCollected = false;
                m_tSinceCollected = 0;
            }
        }
       
    }

}
