using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool<T> where T : MonoBehaviour
{
    GameObject objectPrefab;
    List<T> objects = new List<T>();


    //Initialise pool with some amount of instances
    public ObjectPool(GameObject prefab, int initialSize)
    {
        this.objectPrefab = prefab;

        for (int i = 0; i < initialSize; ++i)
        {
            GameObject o = GameObject.Instantiate(prefab);
            o.SetActive(false);
            objects.Add(o.GetComponent<T>());
        }
    }
    

    //Get a free object from the pool, or if none exist, create a new one and insert it into the pool
    public T GetObject()
    {
        //First see if there is a object in the pool not being used right now
        foreach (T o in objects)
        {
            if (!o.gameObject.activeInHierarchy)
            {
                o.gameObject.SetActive(true);
                return o;
            }
        }


        //If not, create one and expand the pool
        ExpandPool(objects.Count / 2);
        return objects[objects.Count - 1];

        
    }

    

    public int GetFreeCount()
    {
        int count = 0;
        //First see if there is a object in the pool not being used right now
        foreach (T o in objects)
        {
            if (!o.gameObject.activeInHierarchy)
            {
                ++count;
            }
        }

        return count;
    }

    public void ExpandPool(int expandBy)
    {
        for (int i = 0; i < expandBy; ++i)
        {
            GameObject go = GameObject.Instantiate(objectPrefab);
            go.SetActive(true);
            T o = go.GetComponent<T>();
            objects.Add(o);
        }
    }
}

