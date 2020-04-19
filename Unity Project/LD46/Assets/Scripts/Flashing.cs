using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashing : MonoBehaviour
{
    public float f_RandTime, f_MinTime, f_MaxTime;//, f_FlashTime, f_FlashTimer;
    public bool b_RandTime, b_Switch;//, b_Flashed, b_Flashing;
    //public int i_NumOfFlashes, i_CurNumOfFlashes;
    private Light LightScript;

    // Start is called before the first frame update
    void Start()
    {
        if(b_RandTime == true)
        {
            f_RandTime = 0f;
        }
        else
        {
            f_MinTime = 0f;
        }

        LightScript = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!b_Flashed && b_Flashing)
        {
            Flashed();
        }*/
        if(!b_RandTime)
        {
            f_MinTime -= Time.deltaTime;
            if (f_MinTime <= 0)
            {
                /*if (b_Switch)
                {*/
                    Switch();
                /*}
                else
                {
                    Flashed();
                }*/
            }
        }
        else
        {
            if(f_RandTime <= 0f)
            {
                Switch();
                f_RandTime = RandTime(f_MinTime,f_MaxTime);
            }
            else
            {
                f_RandTime -= Time.deltaTime;
            }
        }
    }

    float RandTime(float Min, float Max)
    {
        float NewTime = Random.Range(Min, Max);
        return NewTime;
    }

    void Switch()
    {
        if(LightScript.enabled == true)
        {
            LightScript.enabled = false;
        }
        else
        {
            LightScript.enabled = true;
        }
        f_MinTime = f_MaxTime;
    }

    /*void Flashed()
    {
        if (!b_Flashed)
        {
            if (i_CurNumOfFlashes < i_NumOfFlashes && f_FlashTime <= 0f)
            {
                if (LightScript == false)
                {
                    LightScript.enabled = true;
                    f_FlashTimer = f_FlashTime;
                }
                if (LightScript == true)
                {
                    LightScript.enabled = false;
                    i_CurNumOfFlashes++;
                }
            }
        }
    }*/
}
