using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public LayerMask groundLayer;

    private void OnEnable()
    {
        bool isHit = Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit ray, 100f, groundLayer);
        //if (isHit)
        //    this.transform.position = ray.point;
        if (isHit)
            this.transform.position = new Vector3(transform.position.x, ray.point.y, transform.position.z);
    }
}
