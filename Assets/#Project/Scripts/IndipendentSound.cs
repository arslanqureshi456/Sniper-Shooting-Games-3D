using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndipendentSound : MonoBehaviour
{
    public float delay;
    void OnEnable()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        GetComponent<AudioSource>().enabled = true;
        //GetComponent<AudioSource>().volume = SaveManager.Instance.state.soundFxVolume;
        GetComponent<AudioSource>().volume = 0.25f;
        yield return new WaitForSecondsRealtime(delay);
        GetComponent<AudioSource>().enabled = true;
        GetComponent<AudioSource>().Play();
    }
}
