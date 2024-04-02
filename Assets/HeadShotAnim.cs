using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadShotAnim : MonoBehaviour
{
    public float _time;
    public AudioClip _badgeSound;

    private Vector3 newScale;
    private Coroutine coroutine;
    private bool once = false;

    private void OnEnable()
    {
        Invoke("PlaySound", 0.15f);
        once = false;
        newScale = new Vector3(0.5f, 0.5f, 1);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(ScaleUp(_time));
    }

    IEnumerator ScaleUp(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, lerpValue);
            yield return null;
        }

        if(!once)
        {
            once = true;
            yield return new WaitForSeconds(1f);
            newScale = new Vector3(0f, 0.5f, 1);
            if (coroutine != null)
                StopCoroutine(coroutine);
            StartCoroutine(ScaleDown(_time));
        }
    }

    IEnumerator ScaleDown(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, lerpValue);
            yield return null;
        }

        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        newScale = new Vector3(0f, 0.5f, 1);
        StopCoroutine("ScaleUp");
        StopCoroutine("ScaleDown");
        CancelInvoke("PlaySound");
    }

    private void PlaySound()
    {
        AudioManager.instance.otherAudioSource.PlayOneShot(_badgeSound);
    }

}
