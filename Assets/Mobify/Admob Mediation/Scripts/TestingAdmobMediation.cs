using UnityEditor;
using UnityEngine;

public class TestingAdmobMediation : MonoBehaviour
{
    AdsManager_AdmobMediation adsManager = null;
    AdsManager_AdmobMediation AdsManager
    {
        get
        {
            if (adsManager == null)
                adsManager = GetComponent<AdsManager_AdmobMediation>();
            return adsManager;
        }
    }

    #region Banner
    public void ShowSmallBanner()
    {
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.SmallBannerType, GoogleMobileAds.Api.AdPosition.Top);
    }
    public void HideSmallBanner()
    {
        AdsManager.HideBanners();
    }

    public void ShowLargeBanner()
    {
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.LargeBannerType, GoogleMobileAds.Api.AdPosition.TopLeft);
    }
    public void HideLargeBanner()
    {
        AdsManager.HideBanners();
    }

    public void ShowAdaptiveBanner()
    {
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.AdaptiveBannerType, GoogleMobileAds.Api.AdPosition.Bottom);
    }
    public void HideAdaptiveBanner()
    {
        AdsManager.HideBanners();
    }

    public void ShowSmartBanner()
    {
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.SmartBannerType, GoogleMobileAds.Api.AdPosition.Top);
    }
    public void HideSmartBanner()
    {
        AdsManager.HideBanners();
    }

    #endregion
    
    #region Interstitial

    public void LoadInterstitial()
    {
        AdsManager.LoadInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial);
    }
    public void ShowInterstitial()
    {
        AdsManager.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial);
    }

    public void LoadInterstitialStatic()
    {
        AdsManager.LoadInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitialStatic);
    }
    public void ShowInterstitialStatic()
    {
        AdsManager.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitialStatic);
    }
    public void LoadInterstitial_WithoutFB()
    {
        AdsManager.LoadInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial_withoutFB);
    }
    public void ShowInterstitial_WithoutFB()
    {
        AdsManager.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial_withoutFB);
    }

    #endregion

    #region Rewarded

    public void LoadRewarded()
    {
        AdsManager.LoadRewardedAd(AdsManager_AdmobMediation.RewardedType.rewardedVideo);
    }
    public void ShowRewarded()
    {
        AdsManager.ShowRewardedAd();
    }
    public void LoadRewardedInterstitial()
    {
        AdsManager.LoadRewardedAd(AdsManager_AdmobMediation.RewardedType.rewardedInterstitial);
    }
    public void ShowRewardedInterstitial()
    {
        AdsManager.ShowRewardedAd();
    }
    #endregion

    #region App-Open

    public void LoadAppOpenAd()
    {
        AdsManager.LoadAppOpenAd();
    }
    public void ShowAppOpenAd()
    {
        AdsManager.ShowAppOpenAd();
    }

    #endregion

    public void ShowAdInspector()
    {
        AdsManager.ShowAdInspector();
    }
}