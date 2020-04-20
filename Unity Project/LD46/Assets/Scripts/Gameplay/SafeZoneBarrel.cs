using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneBarrel : MonoBehaviour
{
    public LightSource m_lightSource;
    public float lightTime = 30;
    public float rechargeTime = 30;
    public ParticleSystem particleSystem;

    bool lit = false;
    float tSinceLit = 0;
    float tSinceWentOut;

    // Start is called before the first frame update
    void Start()
    {
        m_lightSource.SetEnabled(false);
        tSinceWentOut = rechargeTime;
        particleSystem.emissionRate = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(lit)
        {
            tSinceLit += Time.deltaTime; 

            if(tSinceLit > lightTime)
            {
                lit = false;
                m_lightSource.SetEnabled(false);
                particleSystem.emissionRate = 1;
            }
        }
        else
        {
            tSinceWentOut += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!lit && tSinceWentOut >= rechargeTime)
            {
                tSinceLit = 0;
                lit = true;
                m_lightSource.SetEnabled(true);
                particleSystem.emissionRate = 17;
            }

        }
    }

    

}
