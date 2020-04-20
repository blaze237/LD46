using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpinController : MonoBehaviour
{
    [SerializeField] Vector3 spinAxis;
    float spinSpeed = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation *= Quaternion.Euler(spinAxis * Time.deltaTime * spinSpeed);
    }
}
