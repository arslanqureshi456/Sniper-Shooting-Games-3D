using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCulling : MonoBehaviour
{
    public static int defaultCam, nearCam, farCam, skinCam, skinFar,skyDome;

	public void OnEnable () {
        Invoke("Delayed", 0.32f);
    }
    void Delayed()
    {
        Camera camera = GetComponent<Camera>();
        float[] distances = new float[32];

        distances[0] = defaultCam;
        distances[23] = nearCam;
        distances[24] = farCam;
        distances[27] = skinCam;
        distances[28] = skinFar;
        distances[26] = skyDome;

        camera.layerCullDistances = distances;
        camera.layerCullSpherical = true;
    }

    private void OnDisable()
    {
        CancelInvoke("Delayed");
    }
}
