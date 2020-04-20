using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.rotation = Player.instance.transform.rotation;
        transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);      
        
    }
}
