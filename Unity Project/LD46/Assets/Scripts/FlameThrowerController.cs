using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerController : MonoBehaviour
{

    [SerializeField] GameObject flamesPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Karen")
        {
            GameObject flames = Instantiate(flamesPrefab, other.transform.position, Quaternion.identity, other.transform);
            Destroy(flames, 3f);
            other.GetComponent<HealthController>().DealDamage(100);
        }
    }


}
