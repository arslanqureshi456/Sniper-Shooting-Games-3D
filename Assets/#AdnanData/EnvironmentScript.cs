using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScript : MonoBehaviour
{
    [Header("Environment Props")]
    public GameObject EnvironmentEffects;


    // Start is called before the first frame update
    void Start()
    {
        // Enabling Environment Particles , If Device RAM is greater tha 3GB
        if(SystemInfo.systemMemorySize > 3000)
        {
            EnvironmentEffects.SetActive(true);
        }
        else if(SystemInfo.systemMemorySize <= 3000)
        {
            EnvironmentEffects.SetActive(false);
        }

    }

}
