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
        ((EnemyAI)sender).m_enemyDeathEventHandler -= OnEnemyKilled;
    }


    // Start is called before the first frame update
    void Start()
    {
        m_tSinceKilled = m_spawnFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_enenmyKilled)
        {
            m_tSinceKilled += Time.deltaTime;

            if (m_tSinceKilled >= m_spawnFrequency)
            {
                //Spawn a pickup
                EnemyAI enemy = EnemyPoolManager.instance.GetEnemy(m_enemyType);
                enemy.Spawn(transform.position);

                enemy.m_enemyDeathEventHandler += OnEnemyKilled;

                m_enenmyKilled = false;
                m_tSinceKilled = 0;
            }
        }

    }

}
