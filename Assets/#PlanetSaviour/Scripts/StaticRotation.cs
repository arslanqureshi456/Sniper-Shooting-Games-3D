using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRotation : MonoBehaviour
{
    public Vector3 Rotation;
    void Update()
    {
        transform.Rotate(Rotation);
    }
}
