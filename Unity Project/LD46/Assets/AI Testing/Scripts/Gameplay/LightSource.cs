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




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
