using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //We use a singleton for player as know there will only ever be one player present in any given scene for this game
    public static Player instance = null;

    public void Awake()
    {

        //Check if instance already exists
        if (instance == null)
            instance = this;
        //Enforce singleton pattern in the case that a second instance of the player has been made
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
              

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
