using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAudioSingleton : MonoBehaviour
{
    //Singleton pattern to ensure only ever one pool for each type
    public static PickupAudioSingleton instance;

    public AudioSource audioSource;


    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        //Enforce singleton pattern in the case that a second instance of the manager has been made
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }


        //ewwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww
        audioSource = GetComponent<AudioSource>();



    }
}
