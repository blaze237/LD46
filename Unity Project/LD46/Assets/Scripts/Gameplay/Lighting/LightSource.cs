using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LightEffectType : int
{
    SlowDown,
    Stop,
    Burn,
}

public class LightSource : MonoBehaviour
{
    public Light m_light;
    public LightEffectType m_lightSourceType = LightEffectType.Stop;

    public bool control = false;


    public delegate void LightEnableEvent(LightEffectType i_type);
    public event LightEnableEvent m_lightEnableEventHandler;

    //public event EventHandler m_lightEnableEventHandler;

    private bool m_enabled = true;
    private UnityEngine.AI.NavMeshObstacle navmeshObstabcle = null;

    public void SetEnabled(bool i_enabled)
    {
        if(i_enabled)
        {
            if(navmeshObstabcle != null) navmeshObstabcle.enabled = true;           
        }
        else
        {
            if (navmeshObstabcle != null) navmeshObstabcle.enabled = false;
        }

        m_light.enabled = i_enabled;
        m_enabled = i_enabled;
        m_lightEnableEventHandler?.Invoke(m_lightSourceType);
    }


    public bool IsEnabled()
    {
        return m_enabled;
    }

    // Start is called before the first frame update
    void Start()
    {
        enabled = m_light.enabled;
        navmeshObstabcle = m_light.GetComponent<UnityEngine.AI.NavMeshObstacle>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && control)
        {
            SetEnabled(!m_enabled);
        }
    }
}
