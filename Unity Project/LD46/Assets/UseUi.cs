using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseUi : MonoBehaviour
{
    public TMPro.TextMeshProUGUI txt;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            txt.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            txt.enabled = false;

        }
    }
}
