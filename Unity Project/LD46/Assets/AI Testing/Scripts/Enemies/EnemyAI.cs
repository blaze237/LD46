using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //How close can player get before the enemy will begin to seek out the player
    public float m_seekRadius = 10;
    //How close should they get before starting their voice lines
    public float m_speechRadius = 5;

    public float m_tTillDeathFromLight = 5f;


    public bool m_useLosCheck = false;
    //Y offset to apply before doing los checks to player. Needed for enemy avatars whos root is at the base of the model
    public float m_losVerticalOffset = 0;
    private StateMachine m_sMachine = new StateMachine();


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_seekRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_sMachine.SetState(new EnemyState_Idle(m_sMachine, this));
    }

    // Update is called once per frame
    void Update()
    {
        m_sMachine.Update(Time.deltaTime);
    }


    public bool losCheck()
    {
        if(!m_useLosCheck)
        {
            return true;
        }
        //Check for walls in front of player
        RaycastHit hit;
        int layerMask = ~(1 << this.gameObject.layer); //Dont care if we hit ourselves or another enemy 
        if (Physics.Raycast(transform.position + Vector3.up * m_losVerticalOffset, (Player.instance.transform.position - transform.position), out hit, Mathf.Infinity, layerMask))
            return hit.collider.gameObject.CompareTag("Player");

        return false;

    }
}
