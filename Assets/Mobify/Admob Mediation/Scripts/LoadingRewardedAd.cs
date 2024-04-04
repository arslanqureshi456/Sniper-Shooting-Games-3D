using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingRewardedAd : MonoBehaviour
{
    float remainingTime = 8f;
    bool isAdShown = false;
    [SerializeField] UnityEngine.UI.Text txt;
    int a = 0;

    void OnEnable()
    {
       
        // Initialize Admob Rewarded , If Not Initialized
            if (!AdsManager_AdmobMediation.Instance.IsRewardedLoaded())
            {
                AdsManager_AdmobMediation.Instance.LoadRewardedAd(AdsManager_AdmobMediation.RewardedType.rewardedVideo);
            }
            if (!AdsManager_Unity.Instance.IsRewardedLoaded)
            {
                AdsManager_Unity.Instance.LoadRewardedAd();
            }
        AdsManager_AdmobMediation.Instance.SaveLastBannerState();
        everySecond = 8;
        remainingTime = 8;
        isAdShown = false;
        txt.text = "Loading Reward . . . ";/* + "(" + Mathf.FloorToInt(remainingTime) + ")";*/
    }
    void Update()
    {
        if (a == 0)
        {
            remainingTime -= Time.unscaledDeltaTime;
            txt.text = "Loading Reward . . . ";/* + "(" + Mathf.FloorToInt(remainingTime) + ")";*/
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

    int everySecond = 8;
    void CheckIsLoaded(int currentTime)
    {
        if (currentTime < everySecond)
        {
            if (AdsManager_AdmobMediation.Instance.IsRewardedLoaded()) Disable();
            everySecond = currentTime;
        }
    }
    void Disable()
    {
        if (!isAdShown)
        {
            isAdShown = true;
                if (AdsManager_AdmobMediation.Instance.IsRewardedLoaded())
                {
                    a = 0;
                    gameObject.SetActive(false);
                    AdsManager_AdmobMediation.Instance.ShowRewardedAd();
                }
                else if (AdsManager_Unity.Instance.IsRewardedLoaded)
                {
                    a = 0;
                    gameObject.SetActive(false);
                    AdsManager_Unity.Instance.ShowRewardedAd();
                }
                else
                {
                    txt.text = "Sorry ! AD Not Available. Try Again !";
                    a = 1;
                    AdsManager_AdmobMediation.Instance.rewardedAdName = "";
                    AdsManager_Unity.Instance.rewardedAdName = "";
                Invoke("DelayDisable", 3);
                }
        }
    }
    void DelayDisable()
    {
        gameObject.SetActive(false);
        AdsManager_AdmobMediation.Instance.AfterLoadingBGBannerToShow();
        a = 0;
    }

}
