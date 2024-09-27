using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoading : MonoBehaviour
{
    public GameObject countDown;

    void CountDown()
    {
        countDown.SetActive(true);
        this.gameObject.SetActive(false);
        AdsManager.instance.ShowTopSmallBanner();
    }

    void Start()
    {
    
        Invoke("CountDown", 4f);        
    }

    private void OnDisable()
    {
        CancelInvoke("CountDown");
    }
}
