﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHide : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
