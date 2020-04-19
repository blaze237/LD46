using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int m_health = 100;
    public int fuel = 100;
    public float m_minDistToAttack = 0.3f;
    public Transform[] m_attackAttatchPoints;
    public delegate void damageEvent();
    public event damageEvent damageEventHandler;
    
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
    }

}
