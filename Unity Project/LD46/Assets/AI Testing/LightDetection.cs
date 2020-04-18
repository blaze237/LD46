using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public float m_MinLightIntensity;

    private HashSet<Light> m_collidingLights = new HashSet<Light>();
    private bool m_seen = false;

    private void Update()
    {
        //Check if we are lit up by any of the lights we are colliding with
        foreach(Light lightSource in m_collidingLights)
        {
            //Check if light intensity is sufficient 
            bool litUp = CheckLightIntensity(lightSource);

            //If light is spotlight, check if within light cone
            litUp = litUp && CheckInLightCone(lightSource);
            //Check if were in shadow?

            if(litUp)
            {
                m_seen = true;
                Debug.Log("seen");
                break;
            }
        }
       
    }

    bool CheckInLightCone(Light i_light)
    {    

        if(i_light.type != LightType.Spot)
        {
            return true;
        }

        //Get max radius of light cone
        float maxSpotRad = i_light.range * Mathf.Tan(Mathf.Deg2Rad * i_light.spotAngle);

        //Check we are on the right side of the cone
        float dist = Vector3.Dot(transform.position - i_light.transform.position, i_light.transform.forward);
        if(dist < 0)
        {
            return false;
        }
        if(dist > i_light.range)
        {
            return false;
        }

        //What is the radius of the cone at this distance
        float radius = (dist / i_light.range) * maxSpotRad;


        //Get orthogonal distance of our position from cone center to see if we lie within cone radius at this distance
        float orthDist = ((transform.position - i_light.transform.position) - dist * i_light.transform.forward).magnitude;

        return orthDist < radius;
    }



    float Saturate(float i_x)
    {
        if(i_x < 0)
        {
            return 0;
        }
        if(i_x > 1)
        {
            return 1;
        }

        return i_x;
    }

    //Determine the intensity of light hitting us
    bool CheckLightIntensity(Light i_light)
    {
        float dist = (transform.position - i_light.transform.position).magnitude;
        float normalizedDist = dist / i_light.range;
        float atten = Saturate(1.0f / (1.0f + 25.0f * normalizedDist * normalizedDist) * Saturate((1.0f - normalizedDist) * 5.0f));
        float maxIntensity = i_light.intensity;
        float actualIntensity = maxIntensity * atten;

        return actualIntensity >= m_MinLightIntensity;
    }


    private void UpdateSeenState()
    {
        m_seen = m_collidingLights.Count != 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LightSource"))
        {
            m_collidingLights.Add(other.gameObject.GetComponent<Light>());
            UpdateSeenState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LightSource"))
        {
            m_collidingLights.Remove(other.gameObject.GetComponent<Light>());
            UpdateSeenState();
        }
    }


}
