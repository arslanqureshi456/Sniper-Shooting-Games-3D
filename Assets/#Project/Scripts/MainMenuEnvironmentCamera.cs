using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEnvironmentCamera : MonoBehaviour
{
    public GameObject cam;

    private void OnEnable()
    {
        cam.SetActive(true);
    }
    private void OnDisable()
    {
        if(cam != null)
            cam.SetActive(false);
    }
}
