using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyFootsteps : MonoBehaviour
{
    AudioSource source;
    public AudioClip[] footsteps;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 0.5f;
        source.spatialBlend = 0.6f;
        source.minDistance = 1.0f;
        source.maxDistance = 300.0f;
    }
    public void EnemyFootstep()
    {
        if (SceneManager.GetActiveScene().name == "Game" && (GameManager.Instance.isLevelCompleted || GameManager.Instance.isLevelFailed))
        {
            source.volume = 0;
        }
        else
        {
            source.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
        }
    }

   
}
