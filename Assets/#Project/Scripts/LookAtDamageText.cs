using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtDamageText : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.forward = -transform.forward;
    }
}
