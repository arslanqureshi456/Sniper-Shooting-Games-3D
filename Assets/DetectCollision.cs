using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip landingSound;
    public GameObject _playerModel, closeUpCam;
    public Fade _fade;
    public SceneStart scene;
    private bool isHere;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isHere && this.gameObject.CompareTag("Swing"))
        {
            audioSource.Stop();
            isHere = true;
            _playerModel.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            _playerModel.GetComponent<Rigidbody>().freezeRotation = true;
            _playerModel.GetComponent<Animator>().SetTrigger("CanJump");
        }

        if (other.CompareTag("Player") && !isHere && this.gameObject.CompareTag("SwitchCam"))
        {
            isHere = true;
            closeUpCam.SetActive(true);
            audioSource.Stop();
            audioSource.PlayOneShot(landingSound);
        }

        if (other.CompareTag("Player") && !isHere && this.gameObject.CompareTag("Dirt"))
        {
            isHere = true;
            _fade.FadeIn();
            
            Invoke("Stop" , 1);
        }
    }

    void Stop()
    {
        scene.OnSceneEnd();
    }

    private void OnDisable()
    {
        CancelInvoke("Stop");
    }
}
