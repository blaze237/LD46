using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public float m_MinLightIntensity;

    private HashSet<LightSource> m_collidingLights = new HashSet<LightSource>();
    //private bool m_illuminated = false;
    private Renderer m_rend;
    private uint m_lightFlags = 0;



    public bool QueryFlags(LightSourceType i_mask)
    {
        return System.Convert.ToBoolean(m_lightFlags & ((uint)i_mask + 1));
    }

    private void Start()
    {
        m_rend = GetComponent<Renderer>();
    }

    public bool IsIlluminated()
    {
        return m_lightFlags != 0;
    }

    public void OnLightSourceEnableEvent(LightSourceType i_type)
    {
        switch(i_type)
        {
            case LightSourceType.Player:
                RefreshPlayerFlag();
                break;
            case LightSourceType.Enviromental:
                RefreshEnvFlag();
                break;
            default:
                RefreshAllFlags();
                break;
        }
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



    private void RefreshAllFlags()
    {
        //Check through the list of lights we're illuminated by to see if we need to update the mask (might not need this for player lights if only ever one anyway)
        bool env = false;
        bool player = false;
        m_lightFlags = 0;
        foreach (LightSource light in m_collidingLights)
        {
            if(light.m_lightSourceType == LightSourceType.Enviromental && light.IsEnabled())
            {
                m_lightFlags |= ((uint)LightSourceType.Enviromental + 1);
                env = true;
            }
            else if (light.m_lightSourceType == LightSourceType.Player && light.IsEnabled())
            {
                m_lightFlags |= ((uint)LightSourceType.Player + 1);
                player = true;
            }

            if(player && env)
            {
                return;
            }
        }
    }

    private void RefreshEnvFlag()
    {
        m_lightFlags &= ~((uint)LightSourceType.Enviromental + 1);
        foreach (LightSource light in m_collidingLights)
        {
            if (light.m_lightSourceType == LightSourceType.Enviromental && light.IsEnabled())
            {
                m_lightFlags |= ((uint)LightSourceType.Enviromental + 1);
                return;
            }
        } 
    }


    private void RefreshPlayerFlag()
    {
        m_lightFlags &= ~((uint)LightSourceType.Player + 1);
        foreach (LightSource light in m_collidingLights)
        {
            if (light.m_lightSourceType == LightSourceType.Player && light.IsEnabled())
            {
                m_lightFlags |= ((uint)LightSourceType.Player + 1);
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LightSource"))
        {
            LightSource lightSource = other.gameObject.GetComponent<LightSource>();
            m_collidingLights.Add(lightSource);
            //Update our mask
            m_lightFlags |= ((uint)lightSource.m_lightSourceType + 1);
            //Subscribe to enable events on the light source
            lightSource.m_lightEnableEventHandler += OnLightSourceEnableEvent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LightSource"))
        {
            LightSource lightSource = other.gameObject.GetComponent<LightSource>();
            m_collidingLights.Remove(lightSource);
            RefreshAllFlags();
            //Unubscribe to enable events on the light source
            lightSource.m_lightEnableEventHandler -= OnLightSourceEnableEvent;
        }
    }


}
