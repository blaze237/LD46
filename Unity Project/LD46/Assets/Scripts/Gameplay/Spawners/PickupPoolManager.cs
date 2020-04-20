using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPoolManager : MonoBehaviour
{
    //Singleton pattern to ensure only ever one pool for each type
    public static PickupPoolManager instance;

    public GameObject coffeePrefab;
    public GameObject batteryPrefab;
    public GameObject toolPrefab;
    public GameObject fuelPrefab;


    public int initialPoolSize = 25;
    //Each projectile type stored in its own pool
    ObjectPool<Pickup> pickupPool_coffee;
    ObjectPool<Pickup> pickupPool_battery;
    ObjectPool<Pickup> pickupPool_tool;
    ObjectPool<Pickup> pickupPool_fuel;


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


        //Create our pools
        pickupPool_coffee = new ObjectPool<Pickup>(coffeePrefab, initialPoolSize);
        pickupPool_battery = new ObjectPool<Pickup>(batteryPrefab, initialPoolSize);
        pickupPool_tool = new ObjectPool<Pickup>(toolPrefab, initialPoolSize);
        pickupPool_fuel = new ObjectPool<Pickup>(fuelPrefab, initialPoolSize);


    }

        //Determine the correct pool to get object of requested type from
        public Pickup GetPickup(PickupType type)
        {
            switch (type)
            {
                case PickupType.Coffee:
                    return pickupPool_coffee.GetObject();
                case PickupType.Battery:
                    return pickupPool_battery.GetObject();
                case PickupType.Fuel:
                    return pickupPool_fuel.GetObject();
                case PickupType.Tool:
                    return pickupPool_tool.GetObject();
            }
            return null;
        }



}
