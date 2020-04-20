using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public bool followRotation = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = Player.instance.transform.position;
        playerPos.y = transform.position.y;// hOffset;
        transform.position = playerPos;

        if(followRotation)
        {
            transform.rotation = Player.instance.transform.rotation;
            transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);

        }
    }
}
