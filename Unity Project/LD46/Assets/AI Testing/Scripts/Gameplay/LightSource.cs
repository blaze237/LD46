using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightSourceType : uint
{
    Player,
    Enviromental
}

public class LightSource : MonoBehaviour
{
    public Light m_light;
    public LightSourceType m_lightSourceType = LightSourceType.Enviromental;
    

    private bool m_enabled = true;
    private UnityEngine.AI.NavMeshObstacle navmeshObstabcle;

    public void SetEnabled(bool i_enabled)
    {
        if(i_enabled)
        {
            navmeshObstabcle.carving = true;
            m_light.enabled = i_enabled;
        }
        else
        {
            navmeshObstabcle.carving = false;
            m_light.enabled = i_enabled;
        }
        m_enabled = i_enabled;
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
        
    }
}
