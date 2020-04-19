using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    PlayerOnly, 
    PlayerAndTower
}

public class EnemyPoolManager : MonoBehaviour
{
    //Singleton pattern to ensure only ever one pool for each type
    public static EnemyPoolManager instance;

    public GameObject karenPrefab_playerOnly;
    public GameObject karenPrefab_playerAndTower;


    public int initialPoolSize = 25;
    //Each type stored in its own pool
    ObjectPool<EnemyAI> pool_playerOnly;
    ObjectPool<EnemyAI> pool_playerAndTower;



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
        pool_playerOnly = new ObjectPool<EnemyAI>(karenPrefab_playerOnly, initialPoolSize);
        pool_playerAndTower = new ObjectPool<EnemyAI>(karenPrefab_playerAndTower, initialPoolSize);



    }

    //Determine the correct pool to get object of requested type from
    public EnemyAI GetEnemy(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.PlayerOnly:
                return pool_playerOnly.GetObject();
            case EnemyType.PlayerAndTower:
                return pool_playerAndTower.GetObject();
        }
        return null;
    }



}
