using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCutScene : MonoBehaviour
{
    public Transform target;
    public AudioClip fireSnd,mineExplosion;
    public LayerMask layerMask;

    private RaycastHit hit;
    bool isBulletAllowed = true, once = true;
    int countBullet = 0;
    private Quaternion fireRotation;

    private float fireRate = 12f;
    private float inaccuracy = 5;//3
    private float nextTimeToFire = 0f;

    private void Start()
    {
        StartCoroutine(GameManager.Instance.fpsPlayer.ActivateBulletTime(0.9f));
    }

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
        if (target == null)
        {
            this.enabled = false;
            return;
        }

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
            if(hit.collider.gameObject.GetComponent<ExplosiveObject>() != null && hit.collider.gameObject.GetComponent<ExplosiveObject>().hitPoints <= 0 && once)
            {
                once = false;
                isBulletAllowed = false;
                AudioManager.instance.PlayNadeSoundForScene();
            }
            if(isBulletAllowed)
                AudioManager.instance.otherAudioSource.PlayOneShot(fireSnd);
            if (hit.collider.gameObject.layer.Equals(11))
            {
                hit.collider.gameObject.GetComponent<ExplosiveObject>().ApplyDamage(4f);
            }
        }
    }
    public void StopBulets()
    {
        isBulletAllowed = false;
    }
}
