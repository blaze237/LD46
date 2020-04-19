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
    public LayerMask m_layerMask;
    public bool m_useNavmeshObstacle = true;

    public GameObject obstaclePrefab;
    private UnityEngine.AI.NavMeshObstacle navmeshObstabcle= null;

    private const float REF_RANGE = 10;
    private const float REF_RADIUS = 2.679491924311f;
    private Transform m_prevTrans;
    private Light m_light;

    void UpdateScaling()
    {

        float lengthScale = m_rangeToleranceScale * m_light.range / REF_RANGE;
        float widthScale = m_radialToleranceScale * m_light.range * Mathf.Tan(Mathf.Deg2Rad * m_light.spotAngle / 2.0f) / REF_RADIUS;

        transform.localScale = new Vector3(widthScale, widthScale, lengthScale);

        //Raycast out in the direction of the m_light to see if we hit the ground, if we do, then place our navmesh obstacle here with sufficient radius
        if(navmeshObstabcle != null)
        {
            UpdateObstacle();
        }

    }


    void UpdateObstacle()
    {
        if(!m_useNavmeshObstacle)
        {
            return;
        }
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, m_light.range, m_layerMask))
        {
            navmeshObstabcle.carving = true;
            float obstacleRadius = (hit.distance * Mathf.Tan(Mathf.Deg2Rad * m_light.spotAngle / 2.0f));
            Vector3 obstaclePos = transform.InverseTransformPoint(hit.point);
            navmeshObstabcle.radius = obstacleRadius;
            navmeshObstabcle.center = obstaclePos;
            navmeshObstabcle.transform.rotation = transform.rotation;
            navmeshObstabcle.transform.position = transform.position;
        }
        else
        {
            navmeshObstabcle.carving = false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.localScale != new Vector3(1, 1, 1))
        {
            Debug.Log("Scale must be unity");
        }

        if(m_useNavmeshObstacle)
        {
            navmeshObstabcle = Instantiate(obstaclePrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<UnityEngine.AI.NavMeshObstacle>();
        }

        m_light = GetComponent<Light>();
        m_prevTrans = transform;
        UpdateScaling();

    }

    // Update is called once per frame
    void Update()
    {
        if(m_realTimeUpdate)
        {
            UpdateScaling();
        }

        if(m_useNavmeshObstacle && navmeshObstabcle != null && Utils.TransformEqualityCheck(transform , m_prevTrans))
        {
            UpdateObstacle();
        }
        m_prevTrans = transform;
    }
}
