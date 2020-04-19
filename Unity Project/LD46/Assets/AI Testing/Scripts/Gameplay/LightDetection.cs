using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public float m_MinLightIntensity;

    private HashSet<LightSource> m_collidingLights = new HashSet<LightSource>();
    //private bool m_illuminated = false;
    private Renderer m_rend;
    private int m_lightFlags = 0;



    public bool QueryFlags(LightEffectType i_mask)
    {
        return System.Convert.ToBoolean(m_lightFlags & ( 1 << (int)i_mask));
    }

    private void Start()
    {
        m_rend = GetComponent<Renderer>();
    }

    public bool IsIlluminated()
    {
        return m_lightFlags != 0;
    }

    public void OnLightSourceEnableEvent(LightEffectType i_type)
    {
        switch(i_type)
        {
            case LightEffectType.SlowDown:
                RefreshSlowdownFlag();
                break;
            case LightEffectType.Stop:
                RefreshStopFlag();
                break;
            case LightEffectType.Burn:
                RefreshBurnFlag();
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


    //Gross, hacky and doesnt really account for overlapping inteligentley. Oh well :/
    public Tuple<LightSource, float> GetNearestLightSource(LightEffectType i_type, Vector2 i_xzPos)
    {       
        LightSource closest = null;
        float minDist = Mathf.Infinity; 
        foreach (LightSource light in m_collidingLights)
        {
            if(light.m_lightSourceType == i_type && light.IsEnabled())
            {
                float dist = (Utils.Project2D(light.m_light.transform.position) - i_xzPos).magnitude;
                if(dist <= minDist)
                {
                    minDist = dist;
                    closest = light;
                }
            }
        }

        return new Tuple<LightSource, float>(closest, minDist);
    }



    private void RefreshAllFlags()
    {
        //Check through the list of lights we're illuminated by to see if we need to update the mask (might not need this for player lights if only ever one anyway)
        bool slow = false;
        bool stop = false;
        bool burn = false;
        m_lightFlags = 0;
        foreach (LightSource light in m_collidingLights)
        {
            if(light.m_lightSourceType == LightEffectType.SlowDown && light.IsEnabled())
            {
                m_lightFlags |= 1 << (int)LightEffectType.SlowDown;
                slow = true;
            }
            else if (light.m_lightSourceType == LightEffectType.Stop && light.IsEnabled())
            {
                m_lightFlags |= 1 << (int)LightEffectType.Stop;
                stop = true;
            }
            else if (light.m_lightSourceType == LightEffectType.Burn && light.IsEnabled())
            {
                m_lightFlags |= 1 << (int)LightEffectType.Burn;
                burn = true;
            }
            if (slow && stop && burn)
            {
                return;
            }
        }
    }

    private void RefreshStopFlag()
    {
        m_lightFlags &= ~(1 << ((int)LightEffectType.Stop));
        foreach (LightSource light in m_collidingLights)
        {
            if (light.m_lightSourceType == LightEffectType.Stop && light.IsEnabled())
            {
                m_lightFlags |= 1 << (int)LightEffectType.Stop;
                return;
            }
        } 
    }


    private void RefreshSlowdownFlag()
    {
        m_lightFlags &= ~ (1 << ((int)LightEffectType.SlowDown));
        foreach (LightSource light in m_collidingLights)
        {
            if (light.m_lightSourceType == LightEffectType.SlowDown && light.IsEnabled())
            {
                m_lightFlags |= 1 << (int)LightEffectType.SlowDown;
                return;
            }
        }
    }

    private void RefreshBurnFlag()
    {
        m_lightFlags &= ~(1 << ((int)LightEffectType.Burn));
        foreach (LightSource light in m_collidingLights)
        {
            if (light.m_lightSourceType == LightEffectType.Burn && light.IsEnabled())
            {
                m_lightFlags |=  1 << (int)LightEffectType.Burn;
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
            if (lightSource.IsEnabled())
            {
                m_lightFlags |=  1 << (int)lightSource.m_lightSourceType;
            }

        
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
            OnLightSourceEnableEvent(lightSource.m_lightSourceType);
            //Unubscribe to enable events on the light source
            lightSource.m_lightEnableEventHandler -= OnLightSourceEnableEvent;
        }
    }


}
