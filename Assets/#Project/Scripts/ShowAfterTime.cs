using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAfterTime : MonoBehaviour
{
    public float delay = 0.3f;
    public GameObject _object;

    private void OnEnable()
    {
        _object.SetActive(false);
        Invoke("Show",delay);
    }

    void Show()
    {
        _object.SetActive(true);
    }

    private void OnDisable()
    {
        CancelInvoke("Show");
    }
}
