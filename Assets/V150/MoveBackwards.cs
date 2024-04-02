using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackwards : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime);
    }
}
