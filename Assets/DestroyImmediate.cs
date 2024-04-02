using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyImmediate : MonoBehaviour
{
    public float time;
    public GameObject[] cameraObjects;
    public bool cameraEnable = false;
    public Animator deadAnimator;
    private void OnEnable()
    {
        Destroy(this.gameObject, time);
    }

    private void Start()
    {
        if (cameraEnable)
        {
            int cameraNumber = Random.Range(0, 5);
            cameraObjects[cameraNumber].SetActive(true);
            deadAnimator.SetInteger("type", Random.Range(1,6));
        }
    }

}
