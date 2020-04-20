﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PickupType
{
    Coffee,
    Battery,
    Fuel,
    Tool,

    s_SIZE
}

public class Pickup : MonoBehaviour
{
    public PickupType pickupType;
    public event EventHandler m_pickupCollectedEventHandler;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (Player.instance.AddToInventory(pickupType))
            {
                //Let our spawner know weve been picked up
                m_pickupCollectedEventHandler?.Invoke(this, new EventArgs());

                gameObject.SetActive(false);
            }
        }
    }
}
