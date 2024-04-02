using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTime : MonoBehaviour
{
    public float delay = 0.3f;
    public GameObject[] items;
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSecondsRealtime(delay);
        foreach (GameObject g in items)
            g.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine("Close");
    }
}
