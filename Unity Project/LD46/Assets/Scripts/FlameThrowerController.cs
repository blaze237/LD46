using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerController : MonoBehaviour
{
    [SerializeField] GameObject flamePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HELLO");
        if (other.name == "Karen")
        {
            GameObject fire = Instantiate(flamePrefab, other.transform.position, Quaternion.identity);
            Destroy(fire, 2f);
        }
    }
}
