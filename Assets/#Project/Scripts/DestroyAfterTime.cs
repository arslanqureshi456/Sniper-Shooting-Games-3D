using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float delay = 0.3f;

    private void OnEnable()
    {
        Invoke("Close" , delay);
    }

    void Close()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke("Close");
    }
}
