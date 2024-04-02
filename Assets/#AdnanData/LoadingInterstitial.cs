using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingInterstitial : MonoBehaviour
{
    public float remainingTime = 4f;
    bool isAdShown = false;
    [SerializeField] UnityEngine.UI.Text txt;

    void OnEnable()
    {
        remainingTime = 4f;
        isAdShown = false;
        txt.text = "Ad  Loading . . . (" + Mathf.FloorToInt(remainingTime) + ")";
    }
    void Update()
    {
        remainingTime -= Time.unscaledDeltaTime;
        txt.text = "Ad  Loading . . . (" + Mathf.FloorToInt(remainingTime) + ")";
        if (remainingTime < 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        //if (!isAdShown)
        //{
        //    isAdShown = true;
        //    if (_ads_manager.Instance != null && _ads_manager.Instance.interstitial.IsLoaded())
        //    {
        //        _ads_manager.Instance.AdsShowing = true;
        //        _ads_manager.Instance.interstitial.Show();
        //    }
        //    else
        //    {
        //        _ads_manager.Instance.SpecialInterstitialNotAvailable();
        //    }
        //}

    }
}
