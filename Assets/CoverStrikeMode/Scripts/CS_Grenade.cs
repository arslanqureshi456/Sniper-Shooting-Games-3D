using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Grenade : MonoBehaviour
{
    public float throwForce = 100f;
    public float timeTilExplode = 3f;
    public GameObject explosion;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    public void SetTarget(Vector3 targetPos)
    {
        this.transform.LookAt(targetPos);
        _rb.useGravity = true;

        float xDist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPos.x, targetPos.z));
        float yDist = -(transform.position.y - targetPos.y);

        //Calculate force required
        throwForce = xDist / (Mathf.Sqrt(Mathf.Abs((yDist - xDist) / (0.5f * (-Physics.gravity.y)))));
        throwForce = 1.414f * throwForce * _rb.mass;

        transform.Rotate(-45, 0, 0);

        _rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        Invoke("DetonateGrenade" , timeTilExplode);
    }

    //IEnumerator StartDetonationTimer()
    //{
    //    yield return new WaitForSeconds(timeTilExplode);
    //    DetonateGrenade();
    //}

    void DetonateGrenade()
    {
        if (explosion)
            Instantiate(explosion, transform.position, transform.rotation);
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("No explosion object assigned to grenade!");
#endif
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        CancelInvoke("DetonateGrenade");
    }
}
