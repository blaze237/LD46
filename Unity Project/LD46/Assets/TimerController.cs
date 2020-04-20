using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    TextMeshProUGUI timerText;
    float startingTime;
    int timer;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        startingTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        timer = (int)Time.time - (int)startingTime;
        timerText.SetText(timer.ToString() + "s");
    }
}
