using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FuelCanText : MonoBehaviour
{
    TextMeshProUGUI txt;


    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        Inventory inventory = Player.instance.GetInventory();

        txt.SetText(inventory.m_fuelCans.ToString());

        //   txt.
    }



}
