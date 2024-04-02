using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinMover : MonoBehaviour
{
    public Transform Target;
    float Dis, temp;
    float fac = 0;
    Vector3 localScale, startPos;
    void Start()
    {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<MeshCollider>());
        Dis = (Target.position - transform.position).sqrMagnitude;
        localScale = transform.localScale;
        startPos = transform.position;
    }
    void LateUpdate()
    {
        fac += 0.0145f;
        transform.position = Vector3.Slerp(startPos, Target.position, fac);
        transform.Rotate(2f, 2f, 0);
        transform.localScale = Vector3.Slerp(localScale, Vector3.zero, fac / 5);
        if (fac >= 1)
        {
            transform.localScale = Vector3.zero;
            enabled = false;
            //DestroyImmediate(gameObject, true);
        }
    }
}
