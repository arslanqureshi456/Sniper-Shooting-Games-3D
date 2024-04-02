using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon : MonoBehaviour
{
    public LayerMask groundLayer;
    public AudioClip impactClip, enemyDownClip, fallDownClip;

    private bool isHit = false;

    private void Awake()
    {
        this.enabled = false;
    }

    private void OnEnable()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.otherAudioSource.PlayOneShot(impactClip);

        if (AudioManager.instance != null)
            AudioManager.instance.otherAudioSource.PlayOneShot(enemyDownClip);

        this.transform.parent = null; // Detach Weapon
        this.transform.GetChild(1).gameObject.AddComponent<BoxCollider>();
        this.transform.gameObject.AddComponent<Rigidbody>();

        this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        float randomFactor = Random.Range(1f, 3f);
        this.transform.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(randomFactor, randomFactor, randomFactor), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            if (!isHit)
            {
                isHit = true;
                if (AudioManager.instance != null)
                    AudioManager.instance.otherAudioSource.PlayOneShot(fallDownClip);
            }
        }
    }
}