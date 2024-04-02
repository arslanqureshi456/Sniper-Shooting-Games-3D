using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffersScalingAnim : MonoBehaviour
{
    float time = 0;
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        time = 0;
        enabled = true;
    }
    private void Update()
    {
        time += 0.12f;
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
            
    }
}
