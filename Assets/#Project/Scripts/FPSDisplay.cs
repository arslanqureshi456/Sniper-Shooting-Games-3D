using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public float updateInterval = 0.5f;

    private float accum = 0.0f; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    private Text text;
 
    void Start()
    {
        text = GetComponent<Text>();

        if (!text)
        {
#if UNITY_EDITOR
            print("FramesPerSecond needs a GUIText component!");
#endif
            enabled = false;
            return;
        }
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            if (AdsManager.instance.isTestMode)
            {
                text.text = "" + (accum / frames).ToString("f2");
            }
            else
            {
                text.text = "";
            }
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}
