using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Animator _animator, _playerAnimator;
    public GameObject _ropeModel, _camera, _playerModel;
    public Transform _playerTransform;
    public AudioSource audioSource;
    private void Awake()
    {
        _ropeModel.SetActive(false);
    }

    public void PlayHangingAnim()
    {
        audioSource.Play();
        _playerAnimator.SetTrigger("CanHang");
        _playerModel.transform.position = _playerTransform.position;
        _playerModel.transform.rotation = _playerTransform.rotation;
        _ropeModel.SetActive(true);
        _camera.SetActive(true);
        _playerModel.AddComponent<Rigidbody>();
        
    }

    public void FadeIn()
    {
        _animator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        _animator.SetTrigger("FadeOut");
    }
}
