using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightColliderScaler : MonoBehaviour
{
    public bool m_realTimeUpdate = true;
    [Range(0.1f, 1.0f)]
    public float m_rangeToleranceScale = 0.9f;

    private const float REF_RANGE = 10;

    private UnityEngine.AI.NavMeshObstacle navmeshObstabcle;


    void UpdateScaling()
    {
        Light light = GetComponent<Light>();
        float scale = m_rangeToleranceScale * light.range / REF_RANGE;
        transform.localScale = new Vector3(scale, scale, scale);
        navmeshObstabcle.radius = scale * REF_RANGE;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.localScale != new Vector3(1, 1, 1))
        {
            Debug.Log("Scale must be unity");
        }

        navmeshObstabcle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        UpdateScaling();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_realTimeUpdate)
        {
            UpdateScaling();
        }
    }
}
