using System;
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

    public AudioClip batteryAudio;
    public AudioClip coffeeAudio;
    public AudioClip toolAudio;
    public AudioClip fuelAudio;
    private AudioSource audioSource;


    public void Start()
    {
        audioSource = PickupAudioSingleton.instance.audioSource;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (Player.instance.AddToInventory(pickupType))
            {
                //Let our spawner know weve been picked up
                m_pickupCollectedEventHandler?.Invoke(this, new EventArgs());

                switch(pickupType)
                {
                    case  PickupType.Battery:
                        audioSource.PlayOneShot(batteryAudio);
                        break;
                    case PickupType.Coffee:
                        audioSource.PlayOneShot(coffeeAudio);
                        break;
                    case PickupType.Fuel:
                        audioSource.PlayOneShot(fuelAudio);
                        break;
                    case PickupType.Tool:
                        audioSource.PlayOneShot(toolAudio);
                        break;
                }

                gameObject.SetActive(false);
            }
        }
    }
}
