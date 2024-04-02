using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Bullet : MonoBehaviour
{
    public float speed = 1000;
    public float maxLifeTime = 3;

    private void Update()
    {
        Move();
        Invoke("SetTimeToDestroy",maxLifeTime);
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    void SetTimeToDestroy()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        CancelInvoke("SetTimeToDestroy");
    }
}
