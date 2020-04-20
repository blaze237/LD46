using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUi : MonoBehaviour
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

        txt.SetText("Tools: " + inventory.m_tools + "\nFuel: " + inventory.m_fuelCans + "\nSpareBattery: " + (inventory.m_spareBattery ? "Y" : "N") + "\nHealth: " + Player.instance.GetHealth() + "\nTowerH: " + Tower.instance.m_health + "\nTowerF: " + Tower.instance.m_fuel);

     //   txt.
    }
}
