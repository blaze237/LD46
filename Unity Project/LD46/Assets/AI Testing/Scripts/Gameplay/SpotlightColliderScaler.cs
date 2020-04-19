using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpotlightColliderScaler : MonoBehaviour
{
   
    public bool m_realTimeUpdate = true;
    [Range(0.1f, 1.0f) ]
    public float m_radialToleranceScale = 0.75f;
    [Range(0.1f, 1.0f)]
    public float m_rangeToleranceScale = 0.9f;

    private const float REF_RANGE = 10;
    private const float REF_RADIUS = 2.679491924311f;

    void UpdateScaling()
    {
        Light light = GetComponent<Light>();   

        float lengthScale = m_rangeToleranceScale * light.range / REF_RANGE;
        float widthScale = m_radialToleranceScale * light.range * Mathf.Tan(Mathf.Deg2Rad * light.spotAngle / 2.0f) / REF_RADIUS;

        transform.localScale = new Vector3(widthScale, widthScale, lengthScale);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.localScale != new Vector3(1, 1, 1))
        {
            Debug.Log("Scale must be unity");
        }

        UpdateScaling();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_realTimeUpdate)
        {
            UpdateScaling();
        }
    }
}
