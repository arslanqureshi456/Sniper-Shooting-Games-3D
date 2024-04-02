using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFire : MonoBehaviour
{
    [HideInInspector] public Transform target;
    public LayerMask layerMask;

    private RaycastHit hit;

    private Quaternion fireRotation;

    private float fireRate = 20f;//12
    private float inaccuracy = 3.5f;//20
    private float nextTimeToFire = 0f;

    private void FixedUpdate()
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }
    }

    private void Fire()
    {
        bool amAtTarget = Vector3.Angle(transform.forward, target.position - transform.position) < 10;
        if (amAtTarget)
        {
            fireRotation.SetLookRotation(target.position - transform.position);
        }
        else
        {
            fireRotation = Quaternion.LookRotation(transform.forward);
        }

        fireRotation *= Quaternion.Euler(Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), 0);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100f, layerMask))
        {
            this.transform.rotation = fireRotation;
            GameManager.Instance.fpsPlayer.WeaponEffectsComponent.ImpactEffects(hit.collider, hit.point, false, false, hit.normal);
        }
    }
}
