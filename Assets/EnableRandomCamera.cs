using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRandomCamera : MonoBehaviour
{
    public GameObject[] cameras;

    private void OnEnable()
    {
        cameras[Random.Range(0, cameras.Length)].SetActive(true);
    }
}
