using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory
{
    public int m_fuelCans = 0;
    public bool m_spareBattery = false;
    public int m_tools = 0;
}
public class InventoryUseEvent : EventArgs
{
    public InventoryUseEvent(PickupType i_type)
    {
        this.m_type = i_type;
    }

    public PickupType m_type;
}


public class Player : MonoBehaviour
{
   
    //We use a singleton for player as know there will only ever be one player present in any given scene for this game
    public static Player instance = null;

    //Event handlers
    public delegate void DeathEvent();
    public event DeathEvent m_deathEventHandler;
    public delegate void InventoryAddEvent();
    public event InventoryAddEvent m_inventoryAddEvent;
    public event EventHandler<InventoryUseEvent> m_inventoryUseEvent;
    public PhoneController phone;

    public KeyCode m_toolsKey;
    public KeyCode m_fuelKey;
    public KeyCode m_batteryKey;

    public int m_healthPackValue = 25;
    public int m_maxTools = 3;
    public int m_maxFuels = 3;
    private int m_health = 100;
    private Inventory m_inventory = new Inventory();
    private bool m_nearTower = false;

    public AudioClip towerRepair;
    public AudioClip towerRefuel;
    public AudioSource audioSource;

    public int GetHealth()
    {
        return m_health;
    }

    public Inventory GetInventory()
    {
        return m_inventory;
    }

    public bool AddToInventory(PickupType i_type)
    {
        switch(i_type)
        {
            case PickupType.Battery:
                if(phone.batteryPercentage == 100)
                {
                    return false;
                }
                else
                {
                    phone.batteryPercentage += 66;
                    return true;
                }
                break;
            case PickupType.Coffee:
                if(m_health != 100)
                {
                    m_health += m_healthPackValue;
                    m_health = Mathf.Min(m_health, 100);
                    m_inventoryAddEvent?.Invoke();
                    return true;
                }
                else
                {
                    return false;
                }
                break;
            case PickupType.Fuel:
                if(m_inventory.m_fuelCans < m_maxFuels)
                {
                    ++m_inventory.m_fuelCans;
                    m_inventoryAddEvent?.Invoke();
                    return true;
                }
                else
                {
                    Debug.Log("Cant hold anymore");
                    return false;
                }
                break;
            case PickupType.Tool:
                if (m_inventory.m_tools < m_maxTools)
                {
                    ++m_inventory.m_tools;
                    m_inventoryAddEvent?.Invoke();
                    return true;
                }
                else
                {
                    Debug.Log("Cant hold anymore");
                    return false;
                }
                break;
        }

        return false;
    }

    public void DoDamage(int _dmg)
    {
        m_health -= _dmg;

        if(m_health <= 0)
        {
            m_deathEventHandler?.Invoke();

            SceneManager.LoadScene(1);

        }
    }

    public void Awake()
    {

        //Check if instance already exists
        if (instance == null)
            instance = this;
        //Enforce singleton pattern in the case that a second instance of the player has been made
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
              

    }

    public float Get2DDistToPlayer( Vector3 pos)
    {
        return Mathf.Abs((Utils.Project2D(transform.position) - Utils.Project2D(pos)).magnitude);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_nearTower)
        {
            if (Input.GetKeyDown(m_toolsKey) && m_inventory.m_tools > 0)
            {
                if (Tower.instance.AddHealth())
                {
                    --m_inventory.m_tools;
                    m_inventoryUseEvent?.Invoke(this, new InventoryUseEvent(PickupType.Tool));
                    audioSource.PlayOneShot(towerRepair);
                }
                else
                {
                    Debug.Log("Tower at max health");
                }
            }
            if (Input.GetKeyDown(m_fuelKey) & m_inventory.m_fuelCans > 0)
            {
                if (Tower.instance.AddFuel())
                {
                    --m_inventory.m_fuelCans;
                    m_inventoryUseEvent?.Invoke(this, new InventoryUseEvent(PickupType.Fuel));
                    audioSource.PlayOneShot(towerRefuel);
                }
                else
                {
                    Debug.Log("Tower at max fuel");
                }
            }
        }
        if (Input.GetKeyDown(m_batteryKey) && m_inventory.m_spareBattery)
        {
            m_inventory.m_spareBattery = false;
            m_inventoryUseEvent?.Invoke(this, new InventoryUseEvent(PickupType.Battery));
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TowerUseRange"))
        {
            m_nearTower = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TowerUseRange"))
        {
            m_nearTower = false;
        }
    }
}
