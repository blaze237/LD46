using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int m_health = 100;
    public float m_fuel = 100;
    public float fuelPerSec = 0.5f;
    public float lowFuelLevel = 30;
    public float lowHealthLevel = 10;
    public float m_minDistToAttack = 0.3f;
    public Transform[] m_attackAttatchPoints;
    public int healthPackPower = 25;
    public float fuelPackPower = 25;

    //Events
    public delegate void damageEvent();
    public event damageEvent damageEventHandler;
    public delegate void NoFuelEvent();
    public event NoFuelEvent m_noFuelEventHandler;
    public delegate void LowFuelEvent();
    public event LowFuelEvent m_lowFuelEventHandler;
    public delegate void LowHealthEvent();
    public event LowHealthEvent m_lowHealthEventHandler;
    public delegate void FuelStabilisedEvent();
    public event FuelStabilisedEvent m_fuelStabilisedEventHandler;
    public delegate void HealthStabilisedEvent();
    public event HealthStabilisedEvent m_healthStabilisedEventHandler;



    private float m_tSinceFuelTick = 1.0f;
    //Only one tower per level also im tired and this makes it easier
    public static Tower instance = null;

    public void Awake()
    {

        //Check if instance already exists
        if (instance == null)
            instance = this;
        //Enforce singleton pattern in the case that a second instance of the tower has been made
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }


    }

    public void Update()
    {
        m_tSinceFuelTick += Time.deltaTime;

        if(m_tSinceFuelTick > 1.0f)
        {
            m_fuel -= fuelPerSec;
            m_tSinceFuelTick = 0;
            if (m_fuel <= 0)
            {
                m_noFuelEventHandler?.Invoke();
            }
            else if(m_fuel <= lowFuelLevel)
            {
                m_lowFuelEventHandler?.Invoke();
            }
        }

    }

    public float Get2DDistToTower(Vector3 pos)
    {
        return Mathf.Abs((Utils.Project2D(transform.position) - Utils.Project2D(pos)).magnitude);
    }


    public Vector3 GetAttatchPoint()
    {
        return m_attackAttatchPoints[Random.Range(0, m_attackAttatchPoints.Length)].position;
    }

    public void AddDamage(int dmg)
    {
        m_health -= dmg;
        //Update the count and probability in the 4 particle systems
        damageEventHandler?.Invoke();
        if (m_health <= 0)
        {
            Debug.Log("GAME OVER!");
        }
        else if(m_health <= lowHealthLevel)
        {
            m_lowHealthEventHandler?.Invoke();
        }
    }

    public bool AddHealth()
    {
        if(m_health == 100)
        {
            return false;
        }

        bool lowHealth = (m_health <= lowHealthLevel);

        m_health += healthPackPower;
        m_health = Mathf.Min(m_health, 100);

        if (lowHealth && (m_health > lowHealthLevel))
        {
            m_healthStabilisedEventHandler?.Invoke();
        }
        return true;
    }

    public bool AddFuel()
    {
        if (m_fuel == 100)
        {
            return false;
        }

        bool lowFuel = (m_fuel <= lowFuelLevel);

        m_fuel += fuelPackPower;
        m_fuel = Mathf.Min(m_fuel, 100);

        if (lowFuel && (m_fuel > lowFuelLevel))
        {
            m_fuelStabilisedEventHandler?.Invoke();
        }
        return true;
    }
}
