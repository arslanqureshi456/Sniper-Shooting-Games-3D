using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heli_Health : MonoBehaviour
{
    public float health = 100f;
    public float radius = 5f;
    public float force = 700f;

    public GameObject explosionEffect;
    public AudioClip dieSound;

    public static Heli_Health instance;

    private void OnEnable()
    {
        instance = this;
    }

    public void Damage(float damage)
    {
        if (health <= 0f)
        {
            Die();
            return;
        }
        health -= damage;
    }

    private void Die()
    {
        if (LevelSelectionNew.modeSelection != LevelSelectionNew.modeType.MULTIPLAYER)
            GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isDestroyChopper]++;
        AudioManager.instance.otherAudioSource.PlayOneShot(dieSound);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        GetComponent<Destructible>().Destroy();
    }

    public void DisableHeliSound()
    {
        GetComponent<AudioSource>().enabled = false;
    }
}
