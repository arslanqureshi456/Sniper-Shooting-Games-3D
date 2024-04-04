using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingInterstitialAd : MonoBehaviour
{
    float remainingTime = 4f;
    bool isAdShown = false;
    [SerializeField] UnityEngine.UI.Text txt;
    int i = 0;
    
    void OnEnable()
    {
        // Initialize Admob Interstitial , If Not Initialized
        
        if(GameManagerStatic.Instance.interstitial == "StaticInterstitial")
        {
            if (AdsManager_AdmobMediation.Instance != null && !AdsManager_AdmobMediation.Instance.IsInterstitialLoaded(AdsManager_AdmobMediation.InterstitialType.interstitialStatic))
            {
                AdsManager_AdmobMediation.Instance.LoadInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitialStatic);
            }
        }
        else if (GameManagerStatic.Instance.interstitial == "Interstitial")
        {
            if (AdsManager_AdmobMediation.Instance != null && !AdsManager_AdmobMediation.Instance.IsInterstitialLoaded(AdsManager_AdmobMediation.InterstitialType.interstitial))
            {
                AdsManager_AdmobMediation.Instance.LoadInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial);
            }
        }
        else
        {
            if (AdsManager_AdmobMediation.Instance != null && !AdsManager_AdmobMediation.Instance.IsInterstitialLoaded(AdsManager_AdmobMediation.InterstitialType.interstitial))
            {
                AdsManager_AdmobMediation.Instance.LoadInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial);
            }
        }
        AdsManager_AdmobMediation.Instance.SaveLastBannerState();
        remainingTime = 4f;
        isAdShown = false;
        txt.text = "Loading AD . . ."; /* + "(" + Mathf.FloorToInt(remainingTime) + ")";*/
        Time.timeScale = 0;
    }
    void Update()
    {
        if (i == 0)
        {
            remainingTime -= Time.unscaledDeltaTime;
            txt.text = "Loading AD . . ."; /* + "(" + Mathf.FloorToInt(remainingTime) + ")";*/
            if (remainingTime < 0)
            {
                Disable();
            }
            else
            {
                CheckIsLoaded(Mathf.FloorToInt(remainingTime));
            }
        }
    }
    int everySecond = 4;
    void CheckIsLoaded(int currentTime)
    {
        if (currentTime < everySecond)
        {
            if (AdsManager_AdmobMediation.Instance.IsInterstitialLoaded(AdsManager_AdmobMediation.InterstitialType.interstitial) ||
                AdsManager_AdmobMediation.Instance.IsInterstitialLoaded(AdsManager_AdmobMediation.InterstitialType.interstitialStatic))
            {
                Disable();
            }
            everySecond = currentTime;
        }
    }
    void Disable()
    {
        if (!isAdShown)
        {
            isAdShown = true;

            if(GameManagerStatic.Instance.interstitial == "StaticInterstitial")
            {
                if(AdsManager_AdmobMediation.Instance != null && AdsManager_AdmobMediation.Instance.IsInterstitialLoaded(AdsManager_AdmobMediation.InterstitialType.interstitialStatic))
                {
                    i = 0;
                    gameObject.SetActive(false);
                    AdsManager_AdmobMediation.Instance.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitialStatic);
                }
                else
                {
                    txt.text = "Sorry ! AD Not Available. Try Again !";
                    Time.timeScale = 1;
                    i = 1;
                    Invoke("DelayDisable", 0.2f);
                }
            }
            else if (GameManagerStatic.Instance.interstitial == "Interstitial")
            {
                if (AdsManager_AdmobMediation.Instance != null && AdsManager_AdmobMediation.Instance.IsInterstitialLoaded(AdsManager_AdmobMediation.InterstitialType.interstitial))
                {
                    i = 0;
                    gameObject.SetActive(false);
                    AdsManager_AdmobMediation.Instance.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial);
                }
                else if (AdsManager_Unity.Instance.IsInterstitialLoaded)
                {
                    i = 0;
                    gameObject.SetActive(false);
                    AdsManager_Unity.Instance.ShowInterstitial();
                }
                else
                {
                    Time.timeScale = 1;
                    txt.text = "Sorry ! AD Not Available. Try Again !";
                    i = 1;
                    Invoke("DelayDisable", 0.2f);
                }
            }
            else
            {
                if (AdsManager_AdmobMediation.Instance != null && AdsManager_AdmobMediation.Instance.IsInterstitialLoaded(AdsManager_AdmobMediation.InterstitialType.interstitial))
                {
                    i = 0;
                    if (GameManager.Instance != null && GameManager.Instance.pauseButtons.activeInHierarchy)
                    {
                        AdsManager_AdmobMediation.Instance.lastTimeScale = 0;
                        gameObject.SetActive(false);
                        AdsManager_AdmobMediation.Instance.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial, 0);
                        Time.timeScale = 0;
                        Invoke("DelayDisable", 0.2f);
                        Time.timeScale = AdsManager_AdmobMediation.Instance.lastTimeScale;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                        AdsManager_AdmobMediation.Instance.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial);
                    }
                }
                else if (AdsManager_Unity.Instance.IsInterstitialLoaded)
                {
                    i = 0;

                    if (GameManager.Instance != null && GameManager.Instance.pauseButtons.activeInHierarchy)
                    {
                        AdsManager_AdmobMediation.Instance.lastTimeScale = 0;
                        gameObject.SetActive(false);
                        AdsManager_Unity.Instance.ShowInterstitial();
                        Time.timeScale = 0;
                        Invoke("DelayDisable", 0.2f);
                        Time.timeScale = AdsManager_AdmobMediation.Instance.lastTimeScale;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                        AdsManager_Unity.Instance.ShowInterstitial();
                    }
                }
                else
                {
                    Time.timeScale = 1;
                    txt.text = "Sorry ! AD Not Available. Try Again !";
                    i = 1;
                    Invoke("DelayDisable", 0.2f);
                }
            }
        }
    }
    void DelayDisable()
    {
        GameManagerStatic.Instance.interstitial = "";
        gameObject.SetActive(false);
        i = 0;
        if (GameManager.Instance != null && GameManager.Instance.pauseButtons.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
    }

}
