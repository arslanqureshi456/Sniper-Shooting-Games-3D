using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{

    // Average FPS
    int framesPassed = 0;
    float fpsTotal = 0f;
    public  float avgFPS = 0;

    // Min & Max FPS
    public float minFPS = Mathf.Infinity;
    public float maxFPS = 0f;

    public static FPSCounter instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        float fps = 1 / Time.unscaledDeltaTime;
       // print("FPS : " + fps);

        // Average FPS
        fpsTotal += fps;
        framesPassed++;
        avgFPS = fpsTotal / framesPassed;
       // print("Average FPS : " + avgFPS);

        // Max FPS
        if (fps > maxFPS && framesPassed > 10)
        {
            maxFPS = fps;
         //   print("Max FPS : " + maxFPS);
        }

        // Min FPS
        if (fps < minFPS && framesPassed > 10)
        {
            minFPS = fps;
          //  print("Min FPS : " + minFPS);
        }
    }
}
