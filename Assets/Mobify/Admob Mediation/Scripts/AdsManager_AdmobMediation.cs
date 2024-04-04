using System;
using UnityEngine;
using GoogleMobileAds.Api;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class AdsManager_AdmobMediation : MonoBehaviour
{
    #region ID's - variables

    [SerializeField] string smallBannerID, smallBannerGamePlayID, largeBannerID, adaptiveBannerID, smartBannerID;
    [SerializeField] string nativeID;
    [SerializeField] string interstitialID, interstitialStaticID, interstitialWithoutFBID;
    [SerializeField] string rewardedID, rewardedInterstitialID;
    [SerializeField] string appOpenID;

    BannerView smallBannerView = null;
    BannerView smallBannerGamePlayView = null;
    BannerView largeBannerView = null;
    BannerView adaptiveBannerView = null;
    BannerView smartBannerView = null;

#if GoogleMobileAdsNative
    NativeAd nativeAdView = null;
#endif

    InterstitialAd interstitial = null;
    InterstitialAd interstitialStatic = null;
    InterstitialAd interstitial_withoutFB = null;

    RewardedAd rewardedAd = null;
    RewardedInterstitialAd rewardedInterstitialAd = null;

    AppOpenAd appOpenAd = null;

    #region For Editor Variable
    [SerializeField] bool isTestMode = false;
    [SerializeField] bool isSmallBannerIdAvailable = false;
    [SerializeField] bool isSmallBannerGamePlayIdAvailable = false;
    [SerializeField] bool isLargeBannerIdAvailable = false;
    [SerializeField] bool isAdaptiveBannerIdAvailable = false;
    [SerializeField] bool isSmartBannerIdAvailable = false;
    [SerializeField] bool isNativeIdAvailable = false;
    [SerializeField] bool isInterstitialIdAvailable = false;
    [SerializeField] bool isInterstitialStaticIdAvailable = false;
    [SerializeField] bool isInterstitial_withoutFB_IdAvailable = false;
    [SerializeField] bool isRewardedVideoIdAvailable = false;
    [SerializeField] bool isRewardedInterstitialIdAvailable = false;
    [SerializeField] bool isAppOpenIdAvailable = false;

    [SerializeField] bool isDebugLog = false;
    [SerializeField] bool isBannerBGEnable = false;

    [SerializeField] bool isMemoryThreshold = false;
    [SerializeField] int NoInitilizationBelow = 200;
    [SerializeField] int NoSmallBannerBelow = 250;
    [SerializeField] int NoLargeBannerBelow = 250;
    [SerializeField] int NoNativeBelow = 250;
    [SerializeField] int NoAnyInterstitialBelow = 300;
    [SerializeField] int NoInterstitialStaticAbove = 400;
    [SerializeField] int NoAnyRewardedBelow = 350;
    [SerializeField] int NoRewardedInterstitialAbove = 450;
    [SerializeField] int NoAppOpenBelow = 300;
    [SerializeField] bool isLowMemoryAnalytics = true;
    #endregion

    #endregion

    #region Instance

    static AdsManager_AdmobMediation _instance = null;
    public static AdsManager_AdmobMediation Instance
    {
        get
        {
            if (_instance == null)
            {
                if (FindObjectOfType<AdsManager_AdmobMediation>())
                {
                    _instance = FindObjectOfType<AdsManager_AdmobMediation>();
                }
                if (_instance != null)
                    DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
            return;
        }
    }

    #endregion

    #region Initilization

    bool isInitilizingFromStart = false;
    System.Collections.IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        if (isTestMode)
        {
            smallBannerID = smallBannerGamePlayID = largeBannerID = adaptiveBannerID = smartBannerID = "ca-app-pub-3940256099942544/6300978111";
            nativeID = "ca-app-pub-3940256099942544/2247696110";
            interstitialID = interstitialWithoutFBID = "ca-app-pub-3940256099942544/8691691433";
            interstitialStaticID = "ca-app-pub-3940256099942544/1033173712";
            rewardedID = "ca-app-pub-3940256099942544/5224354917";
            rewardedInterstitialID = "ca-app-pub-3940256099942544/5354046379";
            appOpenID = "ca-app-pub-3940256099942544/3419835294";
        }
        CheckInitialization(true);

#if unity_release
        ANRHandlerThreadStart();
#endif
    }
    bool CheckInitialization(bool isCallingFromStartFunc = false)
    {
        bool returnValue = false;
        switch (initilizeStatus)
        {
            case InitilizeStatus.None:
                {
                    if (IsInternetConnection())
                        if (!IsLowMemory(NoInitilizationBelow))
                            InitializeAds(isCallingFromStartFunc);
                    returnValue = false;
                    break;
                }
            case InitilizeStatus.Initilizing:
                {
                    returnValue = false;
                    break;
                }
            case InitilizeStatus.Failed:
                {
                    if (IsInternetConnection())
                        if (!IsLowMemory(NoInitilizationBelow))
                            InitializeAds(isCallingFromStartFunc);
                    returnValue = false;
                    break;
                }
            case InitilizeStatus.Initilized:
                {
                    returnValue = true;
                    break;
                }
        }
        return returnValue;
    }
    void InitializeAds(bool isCallingFromStartFunc = false)
    {
        try
        {
            if (!isInitilizingFromStart && !isCallingFromStartFunc) return;
            isInitilizingFromStart = true;
            initilizeStatus = InitilizeStatus.Initilizing;

            #region IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor)
                MobileAds.SetiOSAppPauseOnBackground(true);
            #endregion

            MobileAds.Initialize((initStatus) =>
            {
                if (initStatus == null)
                {
                    initilizeStatus = InitilizeStatus.Failed;
                    return;
                }
                System.Collections.Generic.Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
                foreach (System.Collections.Generic.KeyValuePair<string, AdapterStatus> keyValuePair in map)
                {
                    string className = keyValuePair.Key;
                    AdapterStatus status = keyValuePair.Value;
                    switch (status.InitializationState)
                    {
                        case AdapterState.NotReady:
                            ShowDebugLog("Adapter: " + className + " not ready.");
                            break;
                        case AdapterState.Ready:
                            ShowDebugLog("Adapter: " + className + " initialized.");
                            break;
                    }
                }
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    initilizeStatus = InitilizeStatus.Initilized;
                    ShowDebugLog("Google Mobile Ads Initilized.");

                    // if start the game, check for resession/App-Open scenario
                    if (isCallingFromStartFunc)
                    {
                        Action actionForAppOpenOrResession = new Action(() =>
                        {
                            CheckRequestAppOpenAdOrResessionAd();
                        });
                        ActionsPerformWithDelay(actionForAppOpenOrResession, 0.1f);

                        Action actionForBanners = new Action(() =>
                        {
                            LoadBanner(BannerType.SmallBannerType);
                            LoadBanner(BannerType.LargeBannerType);
#if GoogleMobileAdsNative
                            LoadNativeAd();
#endif
                        });
                        ActionsPerformWithDelay(actionForBanners, 4);
                    }
                });
            });
        }
        catch (Exception) { }
    }
    void CheckRequestAppOpenAdOrResessionAd()
    {
        if (PlayerPrefs.GetInt("isResession", 0).Equals(0))
            LoadAppOpenAd();
        else
            LoadInterstitial(InterstitialType.interstitialStatic);
        PlayerPrefs.SetInt("isResession", 1);
    }

    InitilizeStatus initilizeStatus = InitilizeStatus.None;
    enum InitilizeStatus
    {
        None,
        Initilizing,
        Initilized,
        Failed
    }

    #endregion

    #region Banner

    public void ShowBanner(BannerType banner, AdPosition pos = AdPosition.Center)
    {
        switch (banner)
        {
            case BannerType.SmallBannerType:
                {
                    if (isSmallBannerGamePlayShow) HideSmallBannerGamePlay();
                    if (isLargeBannerShow) HideLargeBanner();
                    if (isAdaptiveBannerShow) HideAdaptiveBanner();
                    if (isSmartBannerShow) HideSmartBanner();

#if AdsManager_Applovin
                    AdsManager_Applovin.Instance.HideBanners();
#endif
                    if (IsBannerLoaded(BannerType.SmallBannerType, true))
                        ShowSmallBanner(pos.Equals(AdPosition.Center) ? lastSmallBannerPosition : pos);
#if AdsManager_Applovin
                    else if (AdsManager_Applovin.Instance.IsBannerLoaded(AdsManager_Applovin.BannerType.SmallBannerType))
                    {
                        AdsManager_Applovin.Instance.ShowBanner(AdsManager_Applovin.BannerType.SmallBannerType,
                            pos.Equals(AdPosition.TopLeft) ? AdsManager_Applovin.BannerPosition.TopLeft :
                            pos.Equals(AdPosition.TopRight) ? AdsManager_Applovin.BannerPosition.TopRight :
                            pos.Equals(AdPosition.Top) ? AdsManager_Applovin.BannerPosition.TopCenter :
                            pos.Equals(AdPosition.BottomLeft) ? AdsManager_Applovin.BannerPosition.BottomLeft :
                            pos.Equals(AdPosition.BottomRight) ? AdsManager_Applovin.BannerPosition.BottomRight :
                            pos.Equals(AdPosition.Bottom) ? AdsManager_Applovin.BannerPosition.BottomCenter :
                            AdsManager_Applovin.BannerPosition.TopCenter);
                    }
#endif
                    break;
                }
            case BannerType.SmallBannerGamePlayType:
                {
                    if (isSmallBannerShow) HideSmallBanner();
                    if (isLargeBannerShow) HideLargeBanner();
                    if (isAdaptiveBannerShow) HideAdaptiveBanner();
                    if (isSmartBannerShow) HideSmartBanner();

#if AdsManager_Applovin
                    AdsManager_Applovin.Instance.HideBanners();
#endif

                    if (IsBannerLoaded(BannerType.SmallBannerGamePlayType, true))
                        ShowSmallBannerGamePlay(pos.Equals(AdPosition.Center) ? lastSmallBannerGamePlayPosition : pos);
#if AdsManager_Applovin
                    else if (AdsManager_Applovin.Instance.IsBannerLoaded(AdsManager_Applovin.BannerType.SmallBannerType))
                    {
                        AdsManager_Applovin.Instance.ShowBanner(AdsManager_Applovin.BannerType.SmallBannerType,
                            pos.Equals(AdPosition.TopLeft) ? AdsManager_Applovin.BannerPosition.TopLeft :
                            pos.Equals(AdPosition.TopRight) ? AdsManager_Applovin.BannerPosition.TopRight :
                            pos.Equals(AdPosition.Top) ? AdsManager_Applovin.BannerPosition.TopCenter :
                            pos.Equals(AdPosition.BottomLeft) ? AdsManager_Applovin.BannerPosition.BottomLeft :
                            pos.Equals(AdPosition.BottomRight) ? AdsManager_Applovin.BannerPosition.BottomRight :
                            pos.Equals(AdPosition.Bottom) ? AdsManager_Applovin.BannerPosition.BottomCenter :
                            AdsManager_Applovin.BannerPosition.TopCenter);
                    }
#endif
                    break;
                }
            case BannerType.LargeBannerType:
                {
                    if (isSmallBannerShow) HideSmallBanner();
                    if (isSmallBannerGamePlayShow) HideSmallBannerGamePlay();
                    if (isAdaptiveBannerShow) HideAdaptiveBanner();
                    if (isSmartBannerShow) HideSmartBanner();

#if AdsManager_Applovin
                    AdsManager_Applovin.Instance.HideBanners();
#endif

                    if (IsBannerLoaded(BannerType.LargeBannerType, true))
                        ShowLargeBanner(pos.Equals(AdPosition.Center) ? lastLargeBannerPosition : pos);
#if AdsManager_Applovin
                    else if (AdsManager_Applovin.Instance.IsBannerLoaded(AdsManager_Applovin.BannerType.LargeBannerType))
                    {
                        AdsManager_Applovin.Instance.ShowBanner(AdsManager_Applovin.BannerType.LargeBannerType,
                            pos.Equals(AdPosition.TopLeft) ? AdsManager_Applovin.BannerPosition.TopLeft :
                            pos.Equals(AdPosition.TopRight) ? AdsManager_Applovin.BannerPosition.TopRight :
                            pos.Equals(AdPosition.Top) ? AdsManager_Applovin.BannerPosition.TopCenter :
                            pos.Equals(AdPosition.BottomLeft) ? AdsManager_Applovin.BannerPosition.BottomLeft :
                            pos.Equals(AdPosition.BottomRight) ? AdsManager_Applovin.BannerPosition.BottomRight :
                            pos.Equals(AdPosition.Bottom) ? AdsManager_Applovin.BannerPosition.BottomCenter :
                            AdsManager_Applovin.BannerPosition.TopCenter);
                    }
#endif
                    break;
                }
            case BannerType.AdaptiveBannerType:
                {
                    if (isSmallBannerShow) HideSmallBanner();
                    if (isSmallBannerGamePlayShow) HideSmallBannerGamePlay();
                    if (isLargeBannerShow) HideLargeBanner();
                    if (isSmartBannerShow) HideSmartBanner();

                    ShowAdaptiveBanner(pos.Equals(AdPosition.Center) ? lastAdaptiveBannerPosition : pos);
                    break;
                }
            case BannerType.SmartBannerType:
                {
                    if (isSmallBannerShow) HideSmallBanner();
                    if (isSmallBannerGamePlayShow) HideSmallBannerGamePlay();
                    if (isLargeBannerShow) HideLargeBanner();
                    if (isAdaptiveBannerShow) HideAdaptiveBanner();

                    ShowSmartBanner(pos.Equals(AdPosition.Center) ? lastSmartBannerPosition : pos);
                    break;
                }
        }

    }
    public void HideBanners()
    {
        if (isSmallBannerShow) HideSmallBanner();
        if (isSmallBannerGamePlayShow) HideSmallBannerGamePlay();
        if (isLargeBannerShow) HideLargeBanner();
        if (isAdaptiveBannerShow) HideAdaptiveBanner();
        if (isSmartBannerShow) HideSmartBanner();

#if AdsManager_Applovin
        AdsManager_Applovin.Instance.HideBanners();
#endif
    }

    bool IsBannerLoaded(BannerType banner, bool isShowAfterLoad = false)
    {
        bool returnValue = false;
        switch (banner)
        {
            case BannerType.SmallBannerType: { returnValue = isSmallBannerLoaded; break; }
            case BannerType.SmallBannerGamePlayType: { returnValue = isSmallBannerGamePlayLoaded; break; }
            case BannerType.LargeBannerType: { returnValue = isLargeBannerLoaded; break; }
            case BannerType.SmartBannerType: { returnValue = isSmartBannerLoaded; break; }
            case BannerType.AdaptiveBannerType: { returnValue = isAdaptiveBannerLoaded; break; }
        }
        if (!returnValue) LoadBanner(banner, isShowAfterLoad);
        return returnValue;
    }
    void LoadBanner(BannerType banner, bool isShowAfterLoad = false)
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;

        switch (banner)
        {
            case BannerType.SmallBannerType:
                {
                    if (!isSmallBannerIdAvailable && !isTestMode) return;
                    if (smallBannerView == null)
                    {
                        smallBannerView = new BannerView(smallBannerID, AdSize.Banner, lastSmallBannerPosition);
                        smallBannerView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
                        {
                            smallBannerView?.Destroy(); smallBannerView = null; isSmallBannerLoaded = false;
                            ShowDebugLog("SmallBanner Failed To Load");
                        };
                        smallBannerView.OnAdLoaded += (object sender, EventArgs e) =>
                        {
                            isSmallBannerLoaded = true;
                            ShowDebugLog("SmallBanner Loaded");

                            if (!isBannerDestroyAndReload) return;
                            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                            {
                                Action action = new Action(() =>
                                {
                                    DestroyBannerAndReload(BannerType.SmallBannerType);
                                });
                                ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                            });
                        };
                        smallBannerView.LoadAd(new AdRequest.Builder().Build());
                        if (!isShowAfterLoad)
                            smallBannerView?.Hide();
                        else
                        {
                            isSmallBannerShow = true;
                            ShowBannerBG(smallBannerView, lastSmallBannerPosition, AdSize.Banner);
                        }
                    }
                    break;
                }
            case BannerType.SmallBannerGamePlayType:
                {
                    if (!isSmallBannerGamePlayIdAvailable && !isTestMode) return;
                    if (smallBannerGamePlayView == null)
                    {
                        smallBannerGamePlayView = new BannerView(smallBannerGamePlayID, AdSize.Banner, lastSmallBannerGamePlayPosition);
                        smallBannerGamePlayView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
                        {
                            smallBannerGamePlayView?.Destroy(); smallBannerGamePlayView = null; isSmallBannerGamePlayLoaded = false;
                            ShowDebugLog("SmallBannerGamePlay Failed To Load");
                        };
                        smallBannerGamePlayView.OnAdLoaded += (object sender, EventArgs e) =>
                        {
                            isSmallBannerGamePlayLoaded = true;
                            ShowDebugLog("SmallBannerGamePlay Loaded");

                            if (!isBannerDestroyAndReload) return;
                            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                            {
                                Action action = new Action(() =>
                                {
                                    DestroyBannerAndReload(BannerType.SmallBannerGamePlayType);
                                });
                                ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                            });
                        };
                        smallBannerGamePlayView.LoadAd(new AdRequest.Builder().Build());
                        if (!isShowAfterLoad)
                            smallBannerGamePlayView?.Hide();
                        else
                        {
                            isSmallBannerGamePlayShow = true;
                            ShowBannerBG(smallBannerGamePlayView, lastSmallBannerGamePlayPosition, AdSize.Banner);
                        }
                    }
                    break;
                }
            case BannerType.LargeBannerType:
                {
                    if (!isLargeBannerIdAvailable && !isTestMode) return;
                    if (largeBannerView == null)
                    {
                        largeBannerView = new BannerView(largeBannerID, AdSize.MediumRectangle, lastLargeBannerPosition);
                        largeBannerView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
                        {
                            largeBannerView?.Destroy(); largeBannerView = null; isLargeBannerLoaded = false;
                            ShowDebugLog("LargeBanner Failed To Load");
                        };
                        largeBannerView.OnAdLoaded += (object sender, EventArgs e) =>
                        {
                            isLargeBannerLoaded = true;
                            ShowDebugLog("LargeBanner Loaded");

                            if (!isBannerDestroyAndReload) return;
                            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                            {
                                Action action = new Action(() =>
                                {
                                    DestroyBannerAndReload(BannerType.LargeBannerType);
                                });
                                ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                            });
                        };
                        largeBannerView.LoadAd(new AdRequest.Builder().Build());
                        if (!isShowAfterLoad)
                            largeBannerView?.Hide();
                        else
                        {
                            isLargeBannerShow = true;
                            ShowBannerBG(largeBannerView, lastLargeBannerPosition, AdSize.MediumRectangle);
                        }
                    }
                    break;
                }
            case BannerType.SmartBannerType:
                {
                    if (!isSmartBannerIdAvailable && !isTestMode) return;
                    if (smartBannerView == null)
                    {
                        smartBannerView = new BannerView(smartBannerID, AdSize.SmartBanner, lastSmartBannerPosition);
                        smartBannerView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
                        {
                            smartBannerView?.Destroy(); smartBannerView = null; isSmartBannerLoaded = false;
                            ShowDebugLog("SmartBanner Failed To Load");
                        };
                        smartBannerView.OnAdLoaded += (object sender, EventArgs e) =>
                        {
                            isSmartBannerLoaded = true;
                            ShowDebugLog("SmartBanner Loaded");

                            if (!isBannerDestroyAndReload) return;
                            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                            {
                                Action action = new Action(() =>
                                {
                                    DestroyBannerAndReload(BannerType.SmartBannerType);
                                });
                                ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                            });
                        };
                        smartBannerView.LoadAd(new AdRequest.Builder().Build());
                        if (!isShowAfterLoad)
                            smartBannerView?.Hide();
                        else
                        {
                            isSmartBannerShow = true;
                            ShowBannerBG(smartBannerView, lastSmartBannerPosition, AdSize.SmartBanner);
                        }
                    }
                    break;
                }
            case BannerType.AdaptiveBannerType:
                {
                    if (!isAdaptiveBannerIdAvailable && !isTestMode) return;
                    if (adaptiveBannerView == null)
                    {
                        adaptiveBannerView = new BannerView(adaptiveBannerID, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), lastAdaptiveBannerPosition);
                        adaptiveBannerView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
                        {
                            adaptiveBannerView?.Destroy(); adaptiveBannerView = null; isAdaptiveBannerLoaded = false;
                            ShowDebugLog("AdaptiveBanner Failed To Load");
                        };
                        adaptiveBannerView.OnAdLoaded += (object sender, EventArgs e) =>
                        {
                            isAdaptiveBannerLoaded = true;
                            ShowDebugLog("AdaptiveBanner Loaded");

                            if (!isBannerDestroyAndReload) return;
                            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                            {
                                Action action = new Action(() =>
                                {
                                    DestroyBannerAndReload(BannerType.AdaptiveBannerType);
                                });
                                ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                            });
                        };
                        adaptiveBannerView.LoadAd(new AdRequest.Builder().Build());
                        if (!isShowAfterLoad)
                            adaptiveBannerView?.Hide();
                        else
                        {
                            isAdaptiveBannerShow = true;
                            ShowBannerBG(adaptiveBannerView, lastAdaptiveBannerPosition, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth));
                        }
                    }
                    break;
                }
        }
    }

    public enum BannerType
    {
        SmallBannerType,
        SmallBannerGamePlayType,
        LargeBannerType,
        AdaptiveBannerType,
        SmartBannerType
    }

    #region Small Banner

    AdPosition lastSmallBannerPosition = AdPosition.Top;
    bool isSmallBannerShow = false;
    bool isSmallBannerLoaded = false;

    void ShowSmallBanner(AdPosition pos)
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;
        if (!isSmallBannerIdAvailable && !isTestMode) return;
        try
        {
            if (smallBannerView != null)
            {
                smallBannerView.SetPosition(pos);
                smallBannerView.Show();

                isSmallBannerShow = true;
                if (isBannerDestroyAndReload) isSmallBannerAtleastOneImpression = true;
                lastSmallBannerPosition = pos;
                ShowBannerBG(smallBannerView, pos, AdSize.Banner);
                ShowDebugLog("Show SmallBanner : " + pos.ToString());
                return;
            }
            if (IsLowMemory(NoSmallBannerBelow)) return;
            if (!IsValidID(ref smallBannerID, "Small Banner")) return;

            smallBannerView = new BannerView(smallBannerID, AdSize.Banner, pos);
            smallBannerView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
            {
                smallBannerView?.Destroy(); smallBannerView = null; isSmallBannerLoaded = false;
                ShowDebugLog("SmallBanner Failed To Load");
                if (isBannerDestroyAndReload) isSmallBannerAtleastOneImpression = false;
            };
            smallBannerView.OnAdLoaded += (object sender, EventArgs e) =>
            {
                isSmallBannerLoaded = true;
                if (isBannerDestroyAndReload) isSmallBannerAtleastOneImpression = false;
                ShowDebugLog("SmallBanner Loaded");

                if (!isBannerDestroyAndReload) return;
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Action action = new Action(() =>
                    {
                        DestroyBannerAndReload(BannerType.SmallBannerType);
                    });
                    ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                });
            };
            smallBannerView.LoadAd(new AdRequest.Builder().Build());

            isSmallBannerShow = true;
            if (isBannerDestroyAndReload) isSmallBannerAtleastOneImpression = true;
            lastSmallBannerPosition = pos;
            ShowBannerBG(smallBannerView, pos, AdSize.Banner);
            ShowDebugLog("Create SmallBanner : " + pos.ToString());
        }
        catch (Exception) { }
    }
    void HideSmallBanner(bool isFromFocus = false)
    {
        smallBannerView?.Hide();
        if (!isFromFocus)
            isSmallBannerShow = false;

        HideBannerBG(AdSize.Banner);
        ShowDebugLog("Hide Small Banner");
    }

    #endregion

    #region Small Banner GamePlay

    AdPosition lastSmallBannerGamePlayPosition = AdPosition.Top;
    bool isSmallBannerGamePlayShow = false;
    bool isSmallBannerGamePlayLoaded = false;

    void ShowSmallBannerGamePlay(AdPosition pos)
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;
        if (!isSmallBannerGamePlayIdAvailable && !isTestMode) return;
        try
        {
            if (smallBannerGamePlayView != null)
            {
                smallBannerGamePlayView.SetPosition(pos);
                smallBannerGamePlayView.Show();

                isSmallBannerGamePlayShow = true;
                if (isBannerDestroyAndReload) isSmallBannerGamePlayAtleastOneImpression = true;
                lastSmallBannerGamePlayPosition = pos;
                ShowBannerBG(smallBannerGamePlayView, pos, AdSize.Banner);
                ShowDebugLog("Show SmallBannerGamePlay : " + pos.ToString());
                return;
            }
            if (IsLowMemory(NoSmallBannerBelow)) return;
            if (!IsValidID(ref smallBannerGamePlayID, "Small Banner GamePlay")) return;

            smallBannerGamePlayView = new BannerView(smallBannerGamePlayID, AdSize.Banner, pos);
            smallBannerGamePlayView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
            {
                smallBannerGamePlayView?.Destroy(); smallBannerGamePlayView = null; isSmallBannerGamePlayLoaded = false;
                ShowDebugLog("SmallBannerGamePlay Failed To Load");
                if (isBannerDestroyAndReload) isSmallBannerGamePlayAtleastOneImpression = false;
            };
            smallBannerGamePlayView.OnAdLoaded += (object sender, EventArgs e) =>
            {
                isSmallBannerGamePlayLoaded = true;
                if (isBannerDestroyAndReload) isSmallBannerGamePlayAtleastOneImpression = false;
                ShowDebugLog("SmallBannerGamePlay Loaded");
                if (!isBannerDestroyAndReload) return;
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Action action = new Action(() =>
                    {
                        DestroyBannerAndReload(BannerType.SmallBannerGamePlayType);
                    });
                    ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                });
            };
            smallBannerGamePlayView.LoadAd(new AdRequest.Builder().Build());

            isSmallBannerGamePlayShow = true;
            if (isBannerDestroyAndReload) isSmallBannerGamePlayAtleastOneImpression = true;
            lastSmallBannerGamePlayPosition = pos;
            ShowBannerBG(smallBannerGamePlayView, pos, AdSize.Banner);
            ShowDebugLog("Create SmallBannerGamePlay : " + pos.ToString());
        }
        catch (Exception) { }
    }
    void HideSmallBannerGamePlay(bool isFromFocus = false)
    {
        smallBannerGamePlayView?.Hide();
        if (!isFromFocus)
            isSmallBannerGamePlayShow = false;

        HideBannerBG(AdSize.Banner);
        ShowDebugLog("Hide SmallBannerGamePlay");
    }

    #endregion

    #region Large Banner

    AdPosition lastLargeBannerPosition = AdPosition.BottomRight;
    bool isLargeBannerShow = false;
    bool isLargeBannerLoaded = false;

    void ShowLargeBanner(AdPosition pos)
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;
        if (!isLargeBannerIdAvailable && !isTestMode) return;
        try
        {
            if (largeBannerView != null)
            {
                largeBannerView.SetPosition(pos);
                largeBannerView.Show();

                isLargeBannerShow = true;
                if (isBannerDestroyAndReload) isLargeBannerAtleastOneImpression = true;
                lastLargeBannerPosition = pos;
                ShowBannerBG(largeBannerView, pos, AdSize.MediumRectangle);
                ShowDebugLog("Show LargeBanner : " + pos.ToString());
                return;
            }
            if (IsLowMemory(NoLargeBannerBelow)) return;
            if (!IsValidID(ref largeBannerID, "Large Banner")) return;

            largeBannerView = new BannerView(largeBannerID, AdSize.MediumRectangle, pos);
            largeBannerView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
            {
                largeBannerView?.Destroy(); largeBannerView = null; isLargeBannerLoaded = false;
                ShowDebugLog("LargeBanner Failed To Load");
                if (isBannerDestroyAndReload) isLargeBannerAtleastOneImpression = false;
            };
            largeBannerView.OnAdLoaded += (object sender, EventArgs arg) =>
            {
                isLargeBannerLoaded = true;
                if (isBannerDestroyAndReload) isLargeBannerAtleastOneImpression = false;
                ShowDebugLog("Large Banner Loaded");

                if (!isBannerDestroyAndReload) return;
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Action action = new Action(() =>
                    {
                        DestroyBannerAndReload(BannerType.LargeBannerType);
                    });
                    ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                });
            };
            largeBannerView.LoadAd(new AdRequest.Builder().Build());

            isLargeBannerShow = true;
            if (isBannerDestroyAndReload) isLargeBannerAtleastOneImpression = true;
            lastLargeBannerPosition = pos;
            ShowBannerBG(largeBannerView, pos, AdSize.MediumRectangle);

            ShowDebugLog("Create LargeBanner : " + pos.ToString());
        }
        catch (Exception) { }
    }
    void HideLargeBanner(bool isFromFocus = false)
    {
        largeBannerView?.Hide();
        if (!isFromFocus)
            isLargeBannerShow = false;

        HideBannerBG(AdSize.MediumRectangle);
        ShowDebugLog("Hide LargeBanner");
    }

    #endregion

    #region Adaptive Banner

    AdPosition lastAdaptiveBannerPosition = AdPosition.Bottom;
    bool isAdaptiveBannerShow = false;
    bool isAdaptiveBannerLoaded = false;

    void ShowAdaptiveBanner(AdPosition pos)
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;
        if (!isAdaptiveBannerIdAvailable && !isTestMode) return;
        try
        {
            if (adaptiveBannerView != null)
            {
                adaptiveBannerView.SetPosition(pos);
                adaptiveBannerView.Show();

                isAdaptiveBannerShow = true;
                if (isBannerDestroyAndReload) isAdaptiveBannerAtleastOneImpression = true;
                lastAdaptiveBannerPosition = pos;
                ShowBannerBG(adaptiveBannerView, pos, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth));
                ShowDebugLog("Show AdaptiveBanner : " + pos.ToString());
                return;
            }
            if (IsLowMemory(NoSmallBannerBelow)) return;
            if (!IsValidID(ref adaptiveBannerID, "Adaptive Banner")) return;

            adaptiveBannerView = new BannerView(adaptiveBannerID, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), pos);
            adaptiveBannerView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
            {
                adaptiveBannerView?.Destroy(); adaptiveBannerView = null; isAdaptiveBannerLoaded = false;
                ShowDebugLog("Adaptive Banner Failed To Load");
                if (isBannerDestroyAndReload) isAdaptiveBannerAtleastOneImpression = false;
            };
            adaptiveBannerView.OnAdLoaded += (object sender, EventArgs args) =>
            {
                isAdaptiveBannerLoaded = true;
                if (isBannerDestroyAndReload) isAdaptiveBannerAtleastOneImpression = false;
                ShowDebugLog("Adaptive Banner Loaded");

                if (!isBannerDestroyAndReload) return;
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Action action = new Action(() =>
                    {
                        DestroyBannerAndReload(BannerType.AdaptiveBannerType);
                    });
                    ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                });
            };
            adaptiveBannerView.LoadAd(new AdRequest.Builder().Build());

            isAdaptiveBannerShow = true;
            if (isBannerDestroyAndReload) isAdaptiveBannerAtleastOneImpression = true;
            lastAdaptiveBannerPosition = pos;
            ShowBannerBG(adaptiveBannerView, pos, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth));

            ShowDebugLog("Create Adaptive Banner : " + pos.ToString());
        }
        catch (Exception) { }
    }
    void HideAdaptiveBanner(bool isFromFocus = false)
    {
        adaptiveBannerView?.Hide();
        if (!isFromFocus)
            isAdaptiveBannerShow = false;

        HideBannerBG(AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth));
        ShowDebugLog("Hide Adaptive Banner");
    }

    #endregion

    #region Smart Banner

    AdPosition lastSmartBannerPosition = AdPosition.Top;
    bool isSmartBannerShow = false;
    bool isSmartBannerLoaded = false;

    void ShowSmartBanner(AdPosition pos)
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;
        if (!isSmartBannerIdAvailable && !isTestMode) return;
        try
        {
            if (smartBannerView != null)
            {
                smartBannerView.SetPosition(pos);
                smartBannerView.Show();

                isSmartBannerShow = true;
                if (isBannerDestroyAndReload) isSmartBannerAtleastOneImpression = true;
                lastSmartBannerPosition = pos;
                ShowBannerBG(smartBannerView, pos, AdSize.SmartBanner);
                ShowDebugLog("Show SmartBanner : " + pos.ToString());
                return;
            }
            if (IsLowMemory(NoSmallBannerBelow)) return;
            if (!IsValidID(ref smartBannerID, "Smart Banner")) return;

            smartBannerView = new BannerView(smartBannerID, AdSize.SmartBanner, pos);
            smartBannerView.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
            {
                smartBannerView?.Destroy(); smartBannerView = null; isSmartBannerLoaded = false;
                ShowDebugLog("Smart Banner Failed To Load");
                if (isBannerDestroyAndReload) isSmartBannerAtleastOneImpression = false;
            };
            smartBannerView.OnAdLoaded += (object sender, EventArgs args) =>
            {
                isSmartBannerLoaded = true;
                if (isBannerDestroyAndReload) isSmartBannerAtleastOneImpression = false;
                ShowDebugLog("Smart Banner Loaded");

                if (!isBannerDestroyAndReload) return;
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Action action = new Action(() =>
                    {
                        DestroyBannerAndReload(BannerType.SmartBannerType);
                    });
                    ActionsPerformWithDelay(action, bannerAutoDestroyTime);
                });
            };
            smartBannerView.LoadAd(new AdRequest.Builder().Build());

            isSmartBannerShow = true;
            if (isBannerDestroyAndReload) isSmartBannerAtleastOneImpression = true;
            lastSmartBannerPosition = pos;
            ShowBannerBG(smartBannerView, pos, AdSize.SmartBanner);

            ShowDebugLog("Create SmartBanner : " + pos.ToString());
        }
        catch (Exception) { }
    }
    void HideSmartBanner(bool isFromFocus = false)
    {
        smartBannerView?.Hide();
        if (!isFromFocus)
            isSmartBannerShow = false;

        HideBannerBG(AdSize.SmartBanner);
        ShowDebugLog("Hide Smart Banner");
    }

    #endregion

    #region Banners BG

#if !BannerHolder
    Texture2D bannersBGTexture = null;
    Rect bannerBGRectPosition;
    AdSize bannerBGAdSize = AdSize.Banner;
    bool isShowBannerBG;
    void OnGUI()
    {
        if (isShowBannerBG)
        {
            GUI.DrawTexture(bannerBGRectPosition, bannersBGTexture);
        }
    }
#endif
    void ShowBannerBG(BannerView banner, AdPosition pos, AdSize adSize)
    {
        if (!isBannerBGEnable) return;

#if BannerHolder
        if (BannerHolder.Instance != null)
        {
            BannerHolder.Instance.ShowBlackBg(adSize, pos);
            return;
        }
#else
        // generate Banners background once
        if (bannersBGTexture == null)
        {
            try
            {
                bannersBGTexture = new Texture2D(2, 2);
                Color[] blackPixels = new Color[2 * 2];
                for (int i = 0; i < blackPixels.Length; i++)
                    blackPixels[i] = Color.black;
                bannersBGTexture.SetPixels(blackPixels);
                bannersBGTexture.Apply();
            }
            catch (Exception) { }
        }
        float height = banner.GetHeightInPixels();
        float width = banner.GetWidthInPixels();
        float heightPadding = 15.0f;
        float widthPadding = 30.0f;
        bannerBGAdSize = adSize;
#if UNITY_EDITOR
        if (PlayerSettings.Android.renderOutsideSafeArea)
            Debug.LogError("Please uncheck (PlayerSetting -> Resolution and Presentation -> Render outside safe area) for perfect Banners BG Rendering.");
#endif
        switch (pos)
        {
            case AdPosition.TopLeft:
                {
                    bannerBGRectPosition = new Rect(0, 0, width + widthPadding, height + heightPadding);
                    break;
                }
            case AdPosition.TopRight:
                {
                    bannerBGRectPosition = new Rect((Screen.width - width - widthPadding), 0, (width + widthPadding), (height + heightPadding));
                    break;
                }
            case AdPosition.BottomLeft:
                {
                    bannerBGRectPosition = new Rect(0, Screen.height - height + heightPadding, (width - widthPadding), (height - heightPadding));
                    break;
                }
            case AdPosition.BottomRight:
                {
                    bannerBGRectPosition = new Rect((Screen.width - width - widthPadding), Screen.height - height - heightPadding, (width + widthPadding), (height + heightPadding));
                    break;
                }
            case AdPosition.Top:
                {
                    bannerBGRectPosition = new Rect(((Screen.width / 2) - (width / 2)) - widthPadding / 2, 0, (width + widthPadding), (height + heightPadding));
                    break;
                }
            case AdPosition.Bottom:
                {
                    bannerBGRectPosition = new Rect((Screen.width / 2) - (width / 2) - widthPadding / 2, Screen.height - height - heightPadding, (width + widthPadding), (height + heightPadding));
                    break;
                }
        }
        isShowBannerBG = true;
#endif
    }
    void HideBannerBG(AdSize adSize)
    {
        if (!isBannerBGEnable) return;

#if BannerHolder
        if (BannerHolder.Instance != null)
        {
            BannerHolder.Instance.HideBlackBg();
            return;
        }
#else
        if (adSize.Equals(bannerBGAdSize))
            isShowBannerBG = false;
#endif
    }

    #endregion

    #region Banner Auto Destroy & Reload

    readonly bool isBannerDestroyAndReload = false;
    readonly float bannerAutoDestroyTime = 30;

    bool isSmallBannerAtleastOneImpression = false;
    bool isSmallBannerGamePlayAtleastOneImpression = false;
    bool isLargeBannerAtleastOneImpression = false;
    bool isAdaptiveBannerAtleastOneImpression = false;
    bool isSmartBannerAtleastOneImpression = false;

    void DestroyBannerAndReload(BannerType bannerType)
    {
        switch (bannerType)
        {
            case BannerType.SmallBannerType:
                {
                    if (isSmallBannerShow) return;
                    if (!isSmallBannerAtleastOneImpression) return;

                    smallBannerView?.Destroy(); smallBannerView = null; isSmallBannerLoaded = false;
                    LoadBanner(bannerType);
                    break;
                }
            case BannerType.SmallBannerGamePlayType:
                {
                    if (isSmallBannerGamePlayShow) return;
                    if (!isSmallBannerGamePlayAtleastOneImpression) return;
                    smallBannerGamePlayView?.Destroy(); smallBannerGamePlayView = null; isSmallBannerGamePlayLoaded = false;
                    LoadBanner(bannerType);
                    break;
                }
            case BannerType.LargeBannerType:
                {
                    if (isLargeBannerShow) return;
                    if (!isLargeBannerAtleastOneImpression) return;
                    largeBannerView?.Destroy(); largeBannerView = null; isLargeBannerLoaded = false;
                    LoadBanner(bannerType);
                    break;
                }
            case BannerType.SmartBannerType:
                {
                    if (isSmartBannerShow) return;
                    if (!isSmartBannerAtleastOneImpression) return;
                    smartBannerView?.Destroy(); smartBannerView = null; isSmartBannerLoaded = false;
                    LoadBanner(bannerType);
                    break;
                }
            case BannerType.AdaptiveBannerType:
                {
                    if (isAdaptiveBannerShow) return;
                    if (!isAdaptiveBannerAtleastOneImpression) return;
                    adaptiveBannerView?.Destroy(); adaptiveBannerView = null; isAdaptiveBannerLoaded = false;
                    LoadBanner(bannerType);
                    break;
                }
        }
    }

    #endregion

    #endregion

    #region Native

#if GoogleMobileAdsNative

    bool isNativeRequestSend = false;
    public bool isNativeLoaded = false;
    readonly int maxNativeImpressionToReloadNewAd = 3;
    int nativeAdCount = 0;

    public void LoadNativeAd()
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;
        if (!isNativeIdAvailable && !isTestMode) return;
        if (IsLowMemory(NoNativeBelow)) return;
        if (isNativeRequestSend) return;

        AdLoader adLoader = new AdLoader.Builder(nativeID).ForNativeAd().Build();
        adLoader.OnNativeAdLoaded += (object sender, NativeAdEventArgs e) =>
        {
            nativeAdView = e.nativeAd;
            isNativeRequestSend = true;
            isNativeLoaded = true;
            nativeAdCount = 0;
            ShowDebugLog("Native Ad Loaded");
        };
        adLoader.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e)=>
        {
            nativeAdView = null;
            isNativeRequestSend = false;
            isNativeLoaded = false;
            ShowDebugLog("Native Ad FailedToLoad. error :"+e.ToString());
        };
        adLoader.OnNativeAdClicked += (object sender, EventArgs e) =>
        {
            nativeAdCount++;
            if (nativeAdCount < maxNativeImpressionToReloadNewAd) return;
            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                isNativeRequestSend = false;
                isNativeLoaded = false;
                LoadNativeAd();
            });
        };
        adLoader.OnNativeAdImpression += (object sender, EventArgs e) =>
        {
            nativeAdCount++;
            if (nativeAdCount < maxNativeImpressionToReloadNewAd) return;
            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                isNativeRequestSend = false;
                isNativeLoaded = false;
                LoadNativeAd();
            });
        };
        adLoader.LoadAd(new AdRequest.Builder().Build());
        ShowDebugLog("Native Ad Request Send");

        isNativeRequestSend = true;
        isNativeLoaded = false;
    }

    public NativeAd GetNativeAdReference()
    {
        return nativeAdView;
    }
    public Texture2D GetNative_Texture(NativeType type)
    {
        Texture2D iconTexture = null;
        switch (type)
        {
            case NativeType.Icon_RawImage:
                {
                    iconTexture = nativeAdView.GetIconTexture();
                    break;
                }
            case NativeType.AdChoicesLogo_RawImage:
                {
                    iconTexture = nativeAdView.GetAdChoicesLogoTexture();
                    break;
                }
        }
        return iconTexture;
    }
    public string GetNative_String(NativeType type)
    {
        string returnData = "";
        switch (type)
        {
            case NativeType.Heading_Text:
                {
                    returnData = nativeAdView.GetHeadlineText();
                    break;
                }
            case NativeType.Body_Text:
                {
                    returnData = nativeAdView.GetBodyText();
                    break;
                }
            case NativeType.Price_Text:
                {
                    returnData = nativeAdView.GetPrice();
                    break;
                }
            case NativeType.Rating_Text:
                {
                    returnData = nativeAdView.GetStarRating().ToString();
                    break;
                }
            case NativeType.Store_Text:
                {
                    returnData = nativeAdView.GetStore();
                    break;
                }
            case NativeType.Action_Text:
                {
                    returnData = nativeAdView.GetCallToActionText();
                    break;
                }
            case NativeType.Advertiser_Text:
                {
                    returnData = nativeAdView.GetAdvertiserText();
                    break;
                }
        }
        return returnData;
    }
    public enum NativeType
    {
        Heading_Text,
        Body_Text,
        Price_Text,
        Rating_Text,
        Store_Text,
        Action_Text,
        Advertiser_Text,
        Icon_RawImage,
        AdChoicesLogo_RawImage
    }

#endif

    #endregion

    #region Interstitial

    public void LoadInterstitial(InterstitialType type)
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;
        if (IsLowMemory(NoAnyInterstitialBelow)) return;

        interstitialChangeLabel:

        switch (type)
        {
            case InterstitialType.interstitial_withoutFB:
                {
                    if (isInterstitial_withoutFB_IdAvailable || isTestMode)
                    {
                        if (interstitial_withoutFB != null) return;
                        if (IsLowMemory(NoInterstitialStaticAbove))
                        {
                            if (isInterstitialStaticIdAvailable || isTestMode)
                            {
                                if (interstitialStatic != null) return;
                                Load_Interstitial_static_Internal();
                                return;
                            }
                            else return;
                        }
                        else Load_Interstitial_withoutFB_Internal();
                    }
                    else
                    {
                        type = InterstitialType.interstitial;
                        goto interstitialChangeLabel;
                    }
                    break;
                }
            case InterstitialType.interstitial:
                {
                    if (isInterstitialIdAvailable || isTestMode)
                    {
                        if (interstitial != null) return;
                        if (IsLowMemory(NoInterstitialStaticAbove))
                        {
                            if (isInterstitialStaticIdAvailable || isTestMode)
                            {
                                if (interstitialStatic != null) return;
                                Load_Interstitial_static_Internal();
                                return;
                            }
                            else return;
                        }
                        else Load_Interstitial_Internal();
                    }
                    else
                    {
                        type = InterstitialType.interstitialStatic;
                        goto interstitialChangeLabel;
                    }
                    break;
                }
            case InterstitialType.interstitialStatic:
                {
                    if (isInterstitialStaticIdAvailable || isTestMode)
                    {
                        if (interstitialStatic != null) return;
                        Load_Interstitial_static_Internal();
                        return;
                    }
                    else return;
                }
        }
    }

    public bool IsInterstitialLoaded(InterstitialType interstitialType)
    {
        bool isLoaded = false;
        interstitialChangeLabel:

        switch (interstitialType)
        {
            case InterstitialType.interstitial_withoutFB:
                {
                    isLoaded = interstitial_withoutFB != null ? interstitial_withoutFB.IsLoaded() : false;
                    if (!isLoaded) { interstitialType = InterstitialType.interstitialStatic; goto interstitialChangeLabel; }
                    break;
                }
            case InterstitialType.interstitial:
                {
                    isLoaded = interstitial != null ? interstitial.IsLoaded() : false;
                    if (!isLoaded) { interstitialType = InterstitialType.interstitialStatic; goto interstitialChangeLabel; }
                    break;
                }
            case InterstitialType.interstitialStatic:
                {
                    isLoaded = interstitialStatic != null ? interstitialStatic.IsLoaded() : false;
#if AdsManager_Applovin
                    if (!isLoaded)
                    {
                        if(AdsManager_Applovin.Instance.IsInterstitialLoaded(true)) isLoaded = true;
                        else isLoaded = AdsManager_Applovin.Instance.IsInterstitialLoaded();

                        isLoaded = AdsManager_Applovin.Instance.IsInterstitialLoaded(true) ? true : AdsManager_Applovin.Instance.IsInterstitialLoaded();
                    }
#endif
                    break;
                }
        }
        return isLoaded;
    }

    bool IsInterstitialLoaded_internal(InterstitialType interstitialType)
    {
        bool isLoaded = false;
        switch (interstitialType)
        {
            case InterstitialType.interstitial_withoutFB:
                {
                    isLoaded = interstitial_withoutFB != null ? interstitial_withoutFB.IsLoaded() : false;
                    break;
                }
            case InterstitialType.interstitial:
                {
                    isLoaded = interstitial != null ? interstitial.IsLoaded() : false;
                    break;
                }
            case InterstitialType.interstitialStatic:
                {
                    isLoaded = interstitialStatic != null ? interstitialStatic.IsLoaded() : false;
                    return isLoaded;
                }
        }
        return isLoaded;
    }

    public void ShowInterstitial(InterstitialType interstitialType, float afterAdTimeScale = 1)
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (IsLowMemory(NoAnyInterstitialBelow)) return;

        Action action = null;

        interstitialChangeLabel:
        switch (interstitialType)
        {
            case InterstitialType.interstitial_withoutFB:
                {
                    if (IsInterstitialLoaded_internal(InterstitialType.interstitial_withoutFB))
                    {
                        action = new Action(() =>
                        {
                            IsAdmobShowingFullScreenAd = false;
                            ShowDebugLog("Interstitial withoutFB Show");
                            interstitial_withoutFB?.Show();
                        });
                        goto showingAdLabel;
                    }
                    else
                    {
                        interstitialType = InterstitialType.interstitialStatic;
                        goto interstitialChangeLabel;
                    }
                }
            case InterstitialType.interstitial:
                {
                    if (IsInterstitialLoaded_internal(InterstitialType.interstitial))
                    {
                        action = new Action(() =>
                        {
                            IsAdmobShowingFullScreenAd = false;
                            ShowDebugLog("Interstitial Show");
                            interstitial?.Show();
                        });
                        goto showingAdLabel;
                    }
                    else
                    {
                        interstitialType = InterstitialType.interstitialStatic;
                        goto interstitialChangeLabel;
                    }
                }
            case InterstitialType.interstitialStatic:
                {
                    if (IsInterstitialLoaded_internal(InterstitialType.interstitialStatic))
                    {
                        action = new Action(() =>
                        {
                            IsAdmobShowingFullScreenAd = false;
                            ShowDebugLog("Interstitial Static Show");
                            interstitialStatic?.Show();
                        });
                        goto showingAdLabel;
                    }
#if AdsManager_Applovin
                    else if (AdsManager_Applovin.Instance.IsInterstitialLoaded())
                    {
                        action = new Action(() =>
                        {
                            IsAdmobShowingFullScreenAd = false;
                            AdsManager_Applovin.Instance.ShowInterstitial();
                        });
                        goto showingAdLabel;
                    }
#endif
                    else
                    {
                        Time.timeScale = afterAdTimeScale; return;
                    }
                }
        }

        showingAdLabel:

        ShowLoadingAdBG();
        IsAdmobShowingFullScreenAd = true;
        isAppOpenCanShow = false;
        Time.timeScale = afterAdTimeScale;
        if (action != null) ActionsPerformWithDelay(action, loadingAdTimeDelay);
    }

    public enum InterstitialType
    {
        interstitial,
        interstitialStatic,
        interstitial_withoutFB
    }

    bool IsInterstitialLoaded(CustomNetworks networkType)
    {
        bool isLoaded = false;
        switch (networkType)
        {
            case CustomNetworks.Admob:
                {
                    isLoaded = IsInterstitialLoaded(InterstitialType.interstitial_withoutFB);
                    if (!isLoaded) isLoaded = IsInterstitialLoaded(InterstitialType.interstitial);
                    if (!isLoaded) isLoaded = IsInterstitialLoaded(InterstitialType.interstitialStatic);
                    break;
                }
            case CustomNetworks.Applovin:
                {
#if AdsManager_Applovin
                   isLoaded = AdsManager_Applovin.Instance.IsInterstitialLoaded();
#endif
                    break;
                }
        }
        return isLoaded;
    }

    void Load_Interstitial_Internal()
    {
        InterstitialType type = InterstitialType.interstitial;
        try
        {
            if (!IsValidID(ref interstitialID, type.ToString())) return;

            interstitial = new InterstitialAd(interstitialID);
            interstitial.OnAdClosed += (object sender, EventArgs e) => { interstitial = null; };
            interstitial.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
            {
                interstitial = null; ShowDebugLog(type + " Failed To Load. error :" + e.LoadAdError.ToString());
            };
            interstitial.OnAdFailedToShow += (object sender, AdErrorEventArgs e) =>
            {
                if (interstitial != null) interstitial = null; ShowDebugLog(type + " Failed To Show. error :" + e.AdError.ToString());
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    HideLoadingAdBG();
                    AfterLoadingBGBannerToShow();
                    Time.timeScale = lastTimeScale;
                    IsAdmobShowingFullScreenAd = false;
                });
            };
            interstitial.OnAdLoaded += (object sender, EventArgs e) => { ShowDebugLog(type + " Loaded"); };
            interstitial.OnAdOpening += (object sender, EventArgs e) => { isAppOpenCanShow = false; };
            interstitial.LoadAd(new AdRequest.Builder().Build());
            ShowDebugLog(type + " Request Send");
        }
        catch (Exception) { }
    }
    void Load_Interstitial_withoutFB_Internal()
    {
        InterstitialType type = InterstitialType.interstitial_withoutFB;
        try
        {
            if (!IsValidID(ref interstitialWithoutFBID, type.ToString())) return;

            interstitial_withoutFB = new InterstitialAd(interstitialWithoutFBID);
            interstitial_withoutFB.OnAdClosed += (object sender, EventArgs e) => { interstitial_withoutFB = null; };
            interstitial_withoutFB.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
            {
                interstitial_withoutFB = null; ShowDebugLog(type + " Failed To Load. error :" + e.LoadAdError.ToString());
            };
            interstitial_withoutFB.OnAdFailedToShow += (object sender, AdErrorEventArgs e) =>
            {
                if (interstitial_withoutFB != null) interstitial_withoutFB = null; ShowDebugLog(type + " Failed To Show. error :" + e.AdError.ToString());
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    HideLoadingAdBG();
                    AfterLoadingBGBannerToShow();
                    Time.timeScale = lastTimeScale;
                    IsAdmobShowingFullScreenAd = false;
                });
            };
            interstitial_withoutFB.OnAdLoaded += (object sender, EventArgs e) => { ShowDebugLog(type + " Loaded"); };
            interstitial_withoutFB.OnAdOpening += (object sender, EventArgs e) => { isAppOpenCanShow = false; };
            interstitial_withoutFB
.LoadAd(new AdRequest.Builder().Build());
            ShowDebugLog(type + " Request Send");
        }
        catch (Exception) { }
    }
    void Load_Interstitial_static_Internal()
    {
        InterstitialType type = InterstitialType.interstitialStatic;
        try
        {
            if (!IsValidID(ref interstitialStaticID, type.ToString())) return;

            interstitialStatic = new InterstitialAd(interstitialStaticID);
            interstitialStatic.OnAdClosed += (object sender, EventArgs e) => { interstitialStatic = null; };
            interstitialStatic.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs e) =>
            {
                interstitialStatic = null; ShowDebugLog(type + " Failed To Load. error :" + e.LoadAdError.ToString());
            };
            interstitialStatic.OnAdFailedToShow += (object sender, AdErrorEventArgs e) =>
            {
                interstitialStatic = null; ShowDebugLog(type + " Failed To Show. error :" + e.AdError.ToString());
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    HideLoadingAdBG();
                    AfterLoadingBGBannerToShow();
                    Time.timeScale = lastTimeScale;
                    IsAdmobShowingFullScreenAd = false;
                });
            };
            interstitialStatic.OnAdLoaded += (object sender, EventArgs e) => { ShowDebugLog(type + " Loaded"); };
            interstitialStatic.OnAdOpening += (object sender, EventArgs e) => { isAppOpenCanShow = false; };
            interstitialStatic.LoadAd(new AdRequest.Builder().Build());
            ShowDebugLog(type + " Request Send");
        }
        catch (Exception) { }
    }

    #endregion

    #region Rewarded

    public void LoadRewardedAd(RewardedType rewardedType = RewardedType.rewardedVideo)
    {
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;
        if (IsLowMemory(NoAnyRewardedBelow)) return;
        if (rewardedAd != null) return;
        if (rewardedInterstitialAd != null) return;

        try
        {
            switch (rewardedType)
            {
                case RewardedType.rewardedVideo:
                    {
                        goto videoAdLable;
                    }
                case RewardedType.rewardedInterstitial:
                    {
                        if (isRewardedInterstitialIdAvailable || isTestMode)
                        {
                            if (IsValidID(ref rewardedInterstitialID, rewardedType.ToString()))
                            {
                                RewardedInterstitialAd.LoadAd(rewardedInterstitialID, new AdRequest.Builder().Build(), RewardedInterstitialCallback);
                                ShowDebugLog(rewardedType + " Request send");
                                return;
                            }
                        }
                        goto videoAdLable;
                    }
            }

            videoAdLable:
            if (IsLowMemory(NoRewardedInterstitialAbove))
            {
                if (isRewardedInterstitialIdAvailable || isTestMode)
                {
                    if (IsValidID(ref rewardedInterstitialID, RewardedType.rewardedInterstitial.ToString()))
                    {
                        RewardedInterstitialAd.LoadAd(rewardedInterstitialID, new AdRequest.Builder().Build(), RewardedInterstitialCallback);
                        ShowDebugLog(RewardedType.rewardedInterstitial.ToString() + " Request send");
                        return;
                    }
                }
            }
            if (!isRewardedVideoIdAvailable && !isTestMode) return;
            if (!IsValidID(ref rewardedID, rewardedType.ToString())) return;

            rewardedAd = new RewardedAd(rewardedID);
            rewardedAd.OnUserEarnedReward += (object sender, Reward args) =>
            {
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    GetReward();
                    rewardedAd = null;
                });

            };
            rewardedAd.OnAdClosed += (object sender, EventArgs args) => { rewardedAd = null; };
            rewardedAd.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs args) =>
            {
                rewardedAd?.Destroy(); rewardedAd = null; ShowDebugLog(rewardedType + " Failed To Load. error :" + args.LoadAdError.ToString());
            };
            rewardedAd.OnAdFailedToShow += (object sender, AdErrorEventArgs args) =>
            {
                rewardedAd?.Destroy(); rewardedAd = null;
                ShowDebugLog(rewardedType + " Failed To Load. error :" + args.AdError.ToString());
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    HideLoadingAdBG();
                    AfterLoadingBGBannerToShow();
                    Time.timeScale = lastTimeScale;
                    IsAdmobShowingFullScreenAd = false;
                });
            };
            rewardedAd.OnAdLoaded += (object sender, EventArgs args) => { ShowDebugLog(rewardedType + " Loaded"); };
            rewardedAd.OnAdOpening += (object sender, EventArgs args) => { isAppOpenCanShow = false; };
            rewardedAd.LoadAd(new AdRequest.Builder().Build());

            ShowDebugLog(rewardedType + " Request send");
        }
        catch (Exception) { }
    }
    public bool IsRewardedLoaded()
    {
        bool isLoaded = false;
        isLoaded = rewardedAd != null ? rewardedAd.IsLoaded() : rewardedInterstitialAd != null ? true : false;
        if (isLoaded) return true;

#if AdsManager_Applovin
         isLoaded = isLoaded ? isLoaded : AdsManager_Applovin.Instance.IsRewardedVideoLoaded();
        if (isLoaded) return true;
         isLoaded = isLoaded ? isLoaded : AdsManager_Applovin.Instance.IsRewardedInterstitialLoaded();
        if (isLoaded) return true;
#endif
        return isLoaded;
    }
    bool IsRewardedLoaded(CustomNetworks networkType)
    {
        bool isLoaded = false;
        switch (networkType)
        {
            case CustomNetworks.Admob:
                {
                    isLoaded = rewardedAd != null ? rewardedAd.IsLoaded() : rewardedInterstitialAd != null ? true : false;
                    break;
                }
            case CustomNetworks.Applovin:
                {
#if AdsManager_Applovin
                    isLoaded = AdsManager_Applovin.Instance.IsRewardedVideoLoaded();
                    if (isLoaded) return true;
                    isLoaded = isLoaded ? isLoaded : AdsManager_Applovin.Instance.IsRewardedInterstitialLoaded();
                    if (isLoaded) return true;
#endif
                    break;
                }
        }
        return isLoaded;
    }
    public void ShowRewardedAd()
    {
        if (!IsInternetConnection()) return;
        if (IsLowMemory(NoAnyRewardedBelow)) return;
        if (IsRewardedLoaded(CustomNetworks.Admob))
        {
            ShowLoadingAdBG();
            IsAdmobShowingFullScreenAd = true;
            isAppOpenCanShow = false;
            if (rewardedAd != null)
            {
                Action actions = new Action(() =>
                {
                    rewardedAd.Show();
                    IsAdmobShowingFullScreenAd = false;
                    ShowDebugLog("Rewarded Show");
                });
                ActionsPerformWithDelay(actions, loadingAdTimeDelay);
            }
            else if (rewardedInterstitialAd != null)
            {
                Action actions = new Action(() =>
                {
                    rewardedInterstitialAd.Show((Reward reward) =>
                        {
                            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                            {
                                GetReward();
                                rewardedInterstitialAd = null;
                                ShowDebugLog("Rewarded Interstitial Show");
                            });

                        });
                    IsAdmobShowingFullScreenAd = false;
                });
                ActionsPerformWithDelay(actions, loadingAdTimeDelay);
            }
        }
#if AdsManager_Applovin
        else if (IsRewardedLoaded(CustomNetworks.Applovin))
        {
            if (AdsManager_Applovin.Instance.IsRewardedVideoLoaded())
                AdsManager_Applovin.Instance.ShowRewardedVideo();
            else if (AdsManager_Applovin.Instance.IsRewardedInterstitialLoaded())
                AdsManager_Applovin.Instance.ShowRewardedInterstitial();
        }
#endif
    }
    public void GetReward()
    {
        Action action = new Action(() =>
        {
            GetRewardCustomCode();
            ShowDebugLog("GetReward()");
        });
        ActionsPerformWithDelay(action, 0.2f);
    }
    void RewardedInterstitialCallback(RewardedInterstitialAd ad, AdFailedToLoadEventArgs error)
    {
        if (error != null || ad == null)
        {
            ShowDebugLog("Rewarded Interstitial Failed To Load. error :" + error);
            rewardedInterstitialAd = null;
            return;
        }
        ShowDebugLog("Rewarded Interstitial Loaded");
        rewardedInterstitialAd = ad;
        rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (object sender, AdErrorEventArgs args) =>
        {
            rewardedInterstitialAd = null;
            ShowDebugLog("Rewarded Interstitial Failed To Present. error :" + args.AdError.ToString());
            HideLoadingAdBG();
        };
        rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (object sender, EventArgs args) => { rewardedInterstitialAd = null; };
        rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (object sender, EventArgs args) => { isAppOpenCanShow = false; };
    }
    public enum RewardedType
    {
        rewardedVideo,
        rewardedInterstitial
    }

    #endregion

    #region App-Open

    public void LoadAppOpenAd()
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (!CheckInitialization()) return;
        if (IsLowMemory(NoAppOpenBelow)) return;

        try
        {
            if (!isAppOpenIdAvailable && !isTestMode) return;
            if (!IsValidID(ref appOpenID, "App Open")) return;

            ShowDebugLog("AppOpen Send Request");
            AppOpenAd.LoadAd(appOpenID, Screen.orientation, new AdRequest.Builder().Build(), (appOpenAdInternal, error) =>
            {
                if (error != null)
                {
                    ShowDebugLog("App Open Failed to Load. error :" + error.ToString());
                    GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        appOpenAd = null;
                    });
                    return;
                }
                appOpenAd = appOpenAdInternal;
                ShowDebugLog("App Open Loaded");

                appOpenAd.OnAdFailedToPresentFullScreenContent += (object sender, AdErrorEventArgs args) => { appOpenAd = null; HideLoadingAdBG(); };
            });
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }
        catch (Exception) { }
    }
    public bool IsAppOpenAdLoaded()
    {
        return appOpenAd != null;
    }
    public bool IsAppOpenAdLoaded(CustomNetworks networkType)
    {
        bool isLoaded = false;
        switch (networkType)
        {
            case CustomNetworks.Admob:
                {
                    isLoaded = appOpenAd != null;
                    break;
                }
            case CustomNetworks.Applovin:
                {
#if AdsManager_Applovin
                    isLoaded = AdsManager_Applovin.Instance.IsAppOpenLoaded();
#endif
                    break;
                }
        }
        return isLoaded;
    }
    public void ShowAppOpenAd()
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (IsLowMemory(NoAppOpenBelow)) return;

        if (IsAppOpenAdLoaded(CustomNetworks.Admob))
        {
            ShowLoadingAdBG();
            IsAdmobShowingFullScreenAd = true;
            Action actions = new Action(() =>
            {
                appOpenAd?.Show();
                IsAdmobShowingFullScreenAd = false;
                ShowDebugLog("App Open Show");
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        appOpenAd = null;
                    });
            });
            ActionsPerformWithDelay(actions, loadingAdTimeDelay);
        }
        else if (IsAppOpenAdLoaded(CustomNetworks.Applovin))
        {
#if AdsManager_Applovin
            AdsManager_Applovin.Instance.ShowAppOpen();
#endif
        }
    }

    public bool isAppOpenCanShow { get; set; } = true;
    void OnAppStateChanged(GoogleMobileAds.Common.AppState state)
    {

        // Display the app open ad when the app is foregrounded.
        if (state == GoogleMobileAds.Common.AppState.Foreground)
        {
            if (IsAppOpenAdLoaded())
            {
                if (isAppOpenCanShow)
                {
                    GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        ShowAppOpenAd();
                    });
                }
            }
            isAppOpenCanShow = true;
        }
    }

    #endregion

    #region Additional Support

    #region Extra

    public void RemovedAdsSuccess()
    {
        smallBannerView?.Destroy();
        smallBannerGamePlayView?.Destroy();
        largeBannerView?.Destroy();
        adaptiveBannerView?.Destroy();
        smartBannerView?.Destroy();
        interstitial?.Destroy();
        interstitialStatic?.Destroy();
        interstitial_withoutFB?.Destroy();
        appOpenAd?.Destroy();
        PlayerPrefs.SetInt(removeAdsValue, 1);
        ShowDebugLog(removeAdsValue + " Successful");
    }

    bool IsInternetConnection()
    {
        return Application.internetReachability == NetworkReachability.NotReachable ? false : true;
    }

    [HideInInspector] public float lastTimeScale = 1.0f;
    bool lastAudioListenerState = false;
    void OnApplicationPause(bool pause)
    {
#if !UNITY_EDITOR
        if (pause)
        {
            lastTimeScale = Time.timeScale;
            Time.timeScale = 0;
            lastAudioListenerState = AudioListener.pause;
            AudioListener.pause = pause;
        }
        else
        {
            if (LoadingAdBG != null)
            {
                if (LoadingAdBG.activeSelf)
                {
                    if (IsAdmobShowingFullScreenAd) { Time.timeScale = lastTimeScale; return; }
#if UTM_Class
        if (UTM_Handler.IsUTMShowingFullScreenAd) return;
#endif
                    HideLoadingAdBG();
                    Action actions = new Action(() =>
                   {
                       AfterLoadingBGBannerToShow();
                       Time.timeScale = lastTimeScale;
                   });
                    ActionsPerformWithDelay(actions, 0.1f);
                }
                else
                {
                    Time.timeScale = lastTimeScale;
                }
            }
            AudioListener.pause = lastAudioListenerState;
        }
#endif
    }

    private void OnApplicationFocus(bool focus)
    {
#if !UNITY_EDITOR
        if (focus)
        {
            if (afterLoadingBannerShowAction != null) return;

            if (isSmallBannerShow) ShowBanner(BannerType.SmallBannerType, lastSmallBannerPosition);
            else if (isSmallBannerGamePlayShow) ShowBanner(BannerType.SmallBannerGamePlayType, lastSmallBannerGamePlayPosition);
            else if (isLargeBannerShow) ShowBanner(BannerType.LargeBannerType, lastLargeBannerPosition);
            else if (isAdaptiveBannerShow) ShowBanner(BannerType.AdaptiveBannerType, lastAdaptiveBannerPosition);
            else if (isSmartBannerShow) ShowBanner(BannerType.SmartBannerType, lastSmartBannerPosition);

        }
        else
        {
            if (isSmallBannerShow) HideSmallBanner(true);
            if (isSmallBannerGamePlayShow) HideSmallBannerGamePlay(true);
            if (isLargeBannerShow) HideLargeBanner(true);
            if (isAdaptiveBannerShow) HideAdaptiveBanner(true);
            if (isSmartBannerShow) HideSmartBanner(true);
        }
#endif
    }

    public void ActionsPerformWithDelay(Action action, float delay = 1f)
    {
        StartCoroutine(ActionPerformIenumerator(action, delay));
    }

    System.Collections.IEnumerator ActionPerformIenumerator(Action action, float delay = 1f)
    {
        yield return new WaitForSecondsRealtime(delay);
        try
        {
            action();
        }
        catch (Exception) { }
    }

    bool IsValidID(ref string ID, string type)
    {
#if !UNITY_EDITOR
                if (PlayerPrefs.HasKey(type))
                    return Convert.ToBoolean(PlayerPrefs.GetInt(type));
#endif
        ID = ID.Replace(" ", String.Empty);
        if (!ID.StartsWith("ca-app-pub-") || ID.Equals(""))
        {
            Debug.LogError("Invalid ID Found Against '" + type.ToString() + "'");
            PlayerPrefs.SetInt(type, 0);
            return false;
        }
        PlayerPrefs.SetInt(type, 1);
        return true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void ShowAdInspector()
    {
        GoogleMobileAds.Api.MobileAds.OpenAdInspector(error => {
            ShowDebugLog("Failed to Show Ad Inspector. error :" + error);
        });
    }

    void ShowDebugLog(string msg)
    {
        if (isDebugLog)
            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log("Admob Log :" + msg);
            });
    }

    public enum CustomNetworks
    {
        Admob,
        Applovin
    }

    bool IsAdmobShowingFullScreenAd { get; set; } = false;

    #endregion

    #region LoadingAdBG

    GameObject LoadingAdsBGRef = null;
    GameObject LoadingAdBG
    {
        get
        {
            if (LoadingAdsBGRef == null)
            {
                LoadingAdsBGRef = transform.Find("loadingAdBG").gameObject;
                if (LoadingAdsBGRef == null)
                    Debug.LogError("LoadingAdBG can't find under the child of AdsManager_AdmobMediation GameObject");
            }
            return LoadingAdsBGRef;
        }
    }
    public void ShowLoadingAdBG()
    {
        if (isSmallBannerShow || isSmallBannerGamePlayShow || isLargeBannerShow || isAdaptiveBannerShow || isSmartBannerShow)
        {
            afterLoadingBannerShowAction = null;
            afterLoadingBannerShowAction = ShowBanner;

            if (isSmallBannerShow) HideSmallBanner(isSmallBannerShow);
            if (isSmallBannerGamePlayShow) HideSmallBannerGamePlay(isSmallBannerGamePlayShow);
            if (isLargeBannerShow) HideLargeBanner(isLargeBannerShow);
            if (isAdaptiveBannerShow) HideAdaptiveBanner(isAdaptiveBannerShow);
            if (isSmartBannerShow) HideSmartBanner(isSmartBannerShow);
        }
#if AdsManager_Applovin
        AdsManager_Applovin.Instance.SaveLastBannerStatus();
#endif

#if !UNITY_EDITOR
                LoadingAdBG?.SetActive(true);
#endif

        if (tempAutomaticDisableLoadingBG != null) StopCoroutine(tempAutomaticDisableLoadingBG);
        tempAutomaticDisableLoadingBG = AutomaticDisableLoadingBG();
        StartCoroutine(tempAutomaticDisableLoadingBG);

#if ANRSupervisor
        ANRSupervisorObservation(ANRSupervisorObservingANRState.Start);
#endif
    }
    public void HideLoadingAdBG()
    {
        LoadingAdBG?.SetActive(false);
        if (tempAutomaticDisableLoadingBG != null) StopCoroutine(tempAutomaticDisableLoadingBG);

#if ANRSupervisor
        ANRSupervisorObservation(ANRSupervisorObservingANRState.Stop);
#endif
    }
    public void AfterLoadingBGBannerToShow()
    {
        if (isSmallBannerShow || isSmallBannerGamePlayShow || isLargeBannerShow || isAdaptiveBannerShow || isSmartBannerShow)
        {
            if (isSmallBannerShow)
                afterLoadingBannerShowAction?.Invoke(BannerType.SmallBannerType, lastSmallBannerPosition);
            else if (isSmallBannerGamePlayShow)
                afterLoadingBannerShowAction?.Invoke(BannerType.SmallBannerGamePlayType, lastSmallBannerGamePlayPosition);
            else if (isLargeBannerShow)
                afterLoadingBannerShowAction?.Invoke(BannerType.LargeBannerType, lastLargeBannerPosition);
            else if (isAdaptiveBannerShow)
                afterLoadingBannerShowAction?.Invoke(BannerType.AdaptiveBannerType, lastAdaptiveBannerPosition);
            else if (isSmartBannerShow)
                afterLoadingBannerShowAction?.Invoke(BannerType.SmartBannerType, lastSmartBannerPosition);
        }
#if AdsManager_Applovin
        AdsManager_Applovin.Instance.LoadLastBannerStatus();
#endif
        afterLoadingBannerShowAction = null;
    }

    Action<BannerType, AdPosition> afterLoadingBannerShowAction = null;
    readonly float loadingAdTimeDelay = 0.2f;

    System.Collections.IEnumerator tempAutomaticDisableLoadingBG = null;
    System.Collections.IEnumerator AutomaticDisableLoadingBG(float timeDuaration = 4.0f)
    {
        yield return new WaitForSecondsRealtime(timeDuaration);
        if (LoadingAdBG.activeSelf)
        {
            HideLoadingAdBG();
            AfterLoadingBGBannerToShow();

        }
    }

    #endregion

    #region Memory Information

    readonly int memoryThreshHold = 500; //in MBs
    static int memoryAvailable = 0;
    static System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"\d+");

    // return True if memory low by defined threshHold
    public bool IsLowMemory(int threshold = -1)
    {
        #region IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor) return false;
        #endregion
        try
        {
            if (!isMemoryThreshold) return false;
            if (isLowMemoryAnalytics) return LowMemoryAndEventSend(threshold);

            threshold = threshold.Equals(-1) ? memoryThreshHold : threshold;
            return LoadMemoryInfo().Equals(true) ? (memoryAvailable / 1024) <= threshold : false;
        }
        catch (Exception) { return true; }
    }
    static bool LoadMemoryInfo()
    {
        try
        {
            //if file not exist retrun from here
            if (!System.IO.File.Exists("/proc/meminfo")) return false;
            System.IO.FileStream fs = new System.IO.FileStream("/proc/meminfo", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.ToLower().Replace(" ", "");
                if (line.Contains("memavailable")) { memoryAvailable = int.Parse(re.Match(line).Value); }
            }
            sr.Close(); fs.Close(); fs.Dispose();
            return true;
        }
        catch (Exception) { return false; }
    }

    bool LowMemoryAndEventSend(int threshold = -1)
    {
        try
        {
            bool returnValue = false;
            string parameterValue = "defaultValue";
            if (LoadMemoryInfo().Equals(true))
            {
                threshold = threshold.Equals(-1) ? memoryThreshHold : threshold;
                float availableInMB = (memoryAvailable / 1024);
                if (availableInMB <= threshold)
                {
                    if (availableInMB < 50)
                        parameterValue = "less than 50";
                    else if (availableInMB < 100)
                        parameterValue = "less than 100";
                    else if (availableInMB < 150)
                        parameterValue = "less than 150";
                    else if (availableInMB < 200)
                        parameterValue = "less than 200";
                    else if (availableInMB < 250)
                        parameterValue = "less than 250";
                    else if (availableInMB < 300)
                        parameterValue = "less than 300";
                    else if (availableInMB < 350)
                        parameterValue = "less than 350";
                    else if (availableInMB < 400)
                        parameterValue = "less than 400";
                    else if (availableInMB < 450)
                        parameterValue = "less than 450";
                    else if (availableInMB < 500)
                        parameterValue = "less than 500";
                    else
                        parameterValue = "greater than 500";
                    returnValue = true;
                }
                else
                {
                    parameterValue = "greater than 500";
                    returnValue = false;
                }
            }
            if (isLowMemoryAnalytics)
            {
#if Firebase_Analytics
                //check if Firebase is initilized successfully, then send these events
                Firebase.Analytics.FirebaseAnalytics.LogEvent("MemoryInfo", new Firebase.Analytics.Parameter("lowMemory", parameterValue));
#endif
            }
            return returnValue;
        }
        catch (Exception) { return true; }
    }

    #endregion

    #region ANR Supervisor

    bool isANRSupervisorInitilized = false;
#if UNITY_ANDROID
    AndroidJavaClass ANRSupervisorClass;
#endif
    void ANRSupervisorObservation(ANRSupervisorObservingANRState state)
    {
#if UNITY_ANDROID
        if (!isANRSupervisorInitilized)
        {
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    try
                    {
                        ANRSupervisorClass = new AndroidJavaClass("ANRSupervisor");
                        ANRSupervisorClass.CallStatic("create");
                        ShowDebugLog("ANRSupervisor create");
                        isANRSupervisorInitilized = true;

                    }
                    catch (Exception) { return; }
                }
                else return;
            }
        }
        switch (state)
        {
            case ANRSupervisorObservingANRState.Start:
                {
                    try
                    {
                        ANRSupervisorClass.CallStatic("start");
                        ShowDebugLog("ANRSupervisor start");
                    }
                    catch (Exception) { }
                    break;
                }
            case ANRSupervisorObservingANRState.Stop:
                {
                    try
                    {
                        ANRSupervisorClass.CallStatic("stop");
                        ShowDebugLog("ANRSupervisor stop");
                    }
                    catch (Exception) { }
                    break;
                }
        }
#endif
    }
    enum ANRSupervisorObservingANRState
    {
        Start,
        Stop
    }
    #endregion

    #region ANR Handler

#if unity_release
    void ANRHandlerThreadStart()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass pluginClass = null;
            try
            {
                pluginClass = new AndroidJavaClass("com.anr.unity.ProcessExecuter");
            }
            catch (Exception) { return; }

            if (pluginClass != null)
            {
                try
                {
                    pluginClass.CallStatic("create");
                    pluginClass.CallStatic("start");
                    ShowDebugLog("ANR Handler Thread Started");
                }
                catch (Exception) { return; }
            }
        }
    }
#endif

    #endregion

    #endregion

    #region Editor Properties

#if UNITY_EDITOR
    [CustomEditor(typeof(AdsManager_AdmobMediation))]
    public class AdsIDsEditor : Editor
    {
        readonly string pluginVersion = "2.5.0";

        #region SerializedProperty

        SerializedProperty isTestMode;
        SerializedProperty isSmallBannerIdAvailable;
        SerializedProperty isSmallBannerGamePlayIdAvailable;
        SerializedProperty smallBannerID;
        SerializedProperty smallBannerGamePlayID;
        SerializedProperty isLargeBannerIdAvailable;
        SerializedProperty largeBannerID;
        SerializedProperty isAdaptiveBannerIdAvailable;
        SerializedProperty adaptiveBannerID;
        SerializedProperty isSmartBannerIdAvailable;
        SerializedProperty smartBannerID;
        SerializedProperty isNativeIdAvailable;
        SerializedProperty nativeID;
        SerializedProperty isInterstitialIdAvailable;
        SerializedProperty interstitialID;
        SerializedProperty isInterstitialStaticIdAvailable;
        SerializedProperty interstitialStaticID;
        SerializedProperty isInterstitial_withoutFB_IdAvailable;
        SerializedProperty interstitial_withoutFB_ID;
        SerializedProperty isRewardedVideoIdAvailable;
        SerializedProperty rewardedID;
        SerializedProperty isRewardedInterstitialIdAvailable;
        SerializedProperty rewardedInterstitialID;
        SerializedProperty isAppOpenIdAvailable;
        SerializedProperty appOpenID;
        SerializedProperty isDebugLog;
        SerializedProperty isBannerBGEnable;

        SerializedProperty isMemoryThreshold;
        SerializedProperty initilizationRequiredMinValue;
        SerializedProperty smallBannerRequiredMinValue;
        SerializedProperty largeBannerRequiredMinValue;
        SerializedProperty nativeRequiredMinValue;
        SerializedProperty interstitialRequiredMinValue;
        SerializedProperty interstitialStaticRequiredMinValue;
        SerializedProperty rewardedRequiredMinValue;
        SerializedProperty rewardedInterstitialRequiredMinValue;
        SerializedProperty appOpenRequiredMinValue;
        SerializedProperty isLowMemoryAnalytics;

        #endregion

        void OnEnable()
        {
            isTestMode = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isTestMode));
            isSmallBannerIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isSmallBannerIdAvailable));
            isSmallBannerGamePlayIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isSmallBannerGamePlayIdAvailable));
            smallBannerID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.smallBannerID));
            smallBannerGamePlayID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.smallBannerGamePlayID));
            isLargeBannerIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isLargeBannerIdAvailable));
            largeBannerID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.largeBannerID));
            isAdaptiveBannerIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isAdaptiveBannerIdAvailable));
            adaptiveBannerID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.adaptiveBannerID));
            isSmartBannerIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isSmartBannerIdAvailable));
            smartBannerID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.smartBannerID));
            isNativeIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isNativeIdAvailable));
            nativeID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.nativeID));
            isInterstitialIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isInterstitialIdAvailable));
            interstitialID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.interstitialID));
            isInterstitialStaticIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isInterstitialStaticIdAvailable));
            interstitialStaticID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.interstitialStaticID));
            isInterstitial_withoutFB_IdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isInterstitial_withoutFB_IdAvailable));
            interstitial_withoutFB_ID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.interstitialWithoutFBID));
            isRewardedVideoIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isRewardedVideoIdAvailable));
            rewardedID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.rewardedID));
            isRewardedInterstitialIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isRewardedInterstitialIdAvailable));
            rewardedInterstitialID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.rewardedInterstitialID));
            isAppOpenIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isAppOpenIdAvailable));
            appOpenID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.appOpenID));
            isDebugLog = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isDebugLog));
            isBannerBGEnable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isBannerBGEnable));

            isMemoryThreshold = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isMemoryThreshold));
            initilizationRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoInitilizationBelow));
            smallBannerRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoSmallBannerBelow));
            largeBannerRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoLargeBannerBelow));
            nativeRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoNativeBelow));
            interstitialRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoAnyInterstitialBelow));
            interstitialStaticRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoInterstitialStaticAbove));
            rewardedRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoAnyRewardedBelow));
            rewardedInterstitialRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoRewardedInterstitialAbove));
            appOpenRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoAppOpenBelow));
            isLowMemoryAnalytics = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isLowMemoryAnalytics));

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUIStyle headingStyle = new GUIStyle(EditorStyles.helpBox);
            headingStyle.normal.textColor = Color.blue;
            headingStyle.fontStyle = FontStyle.Bold;
            headingStyle.fontSize = 12;

            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("AD IDs", headingStyle);
            EditorGUILayout.Space();

            isTestMode.boolValue = EditorGUILayout.Toggle("Test Mode", isTestMode.boolValue);
            EditorGUILayout.Space(); EditorGUILayout.Space();
            if (!isTestMode.boolValue)
            {

                isSmallBannerIdAvailable.boolValue = EditorGUILayout.Toggle("Small Banner", isSmallBannerIdAvailable.boolValue);
                if (isSmallBannerIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(smallBannerID);
                    EditorGUILayout.Space();
                }
                isSmallBannerGamePlayIdAvailable.boolValue = EditorGUILayout.Toggle("Small Banner GamePlay", isSmallBannerGamePlayIdAvailable.boolValue);
                if (isSmallBannerGamePlayIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(smallBannerGamePlayID);
                    EditorGUILayout.Space();
                }
                isLargeBannerIdAvailable.boolValue = EditorGUILayout.Toggle("Large Banner", isLargeBannerIdAvailable.boolValue);
                if (isLargeBannerIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(largeBannerID);
                    EditorGUILayout.Space();
                }
                isAdaptiveBannerIdAvailable.boolValue = EditorGUILayout.Toggle("Adaptive Banner", isAdaptiveBannerIdAvailable.boolValue);
                if (isAdaptiveBannerIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(adaptiveBannerID);
                    EditorGUILayout.Space();
                }
                isSmartBannerIdAvailable.boolValue = EditorGUILayout.Toggle("Smart Banner", isSmartBannerIdAvailable.boolValue);
                if (isSmartBannerIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(smartBannerID);
                    EditorGUILayout.Space();
                }
                isNativeIdAvailable.boolValue = EditorGUILayout.Toggle("Native", isNativeIdAvailable.boolValue);
                if (isNativeIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(nativeID);
                    EditorGUILayout.Space();
                }
                isInterstitialIdAvailable.boolValue = EditorGUILayout.Toggle("Interstitial", isInterstitialIdAvailable.boolValue);
                if (isInterstitialIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(interstitialID);
                    EditorGUILayout.Space();
                }
                isInterstitialStaticIdAvailable.boolValue = EditorGUILayout.Toggle("Interstitial Static", isInterstitialStaticIdAvailable.boolValue);
                if (isInterstitialStaticIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(interstitialStaticID);
                    EditorGUILayout.Space();
                }
                isInterstitial_withoutFB_IdAvailable.boolValue = EditorGUILayout.Toggle("Interstitial without FB", isInterstitial_withoutFB_IdAvailable.boolValue);
                if (isInterstitial_withoutFB_IdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(interstitial_withoutFB_ID);
                    EditorGUILayout.Space();
                }
                isRewardedVideoIdAvailable.boolValue = EditorGUILayout.Toggle("Rewarded", isRewardedVideoIdAvailable.boolValue);
                if (isRewardedVideoIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(rewardedID);
                    EditorGUILayout.Space();
                }
                isRewardedInterstitialIdAvailable.boolValue = EditorGUILayout.Toggle("Rewarded Interstitial", isRewardedInterstitialIdAvailable.boolValue);
                if (isRewardedInterstitialIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(rewardedInterstitialID);
                    EditorGUILayout.Space();
                }
                isAppOpenIdAvailable.boolValue = EditorGUILayout.Toggle("App Open", isAppOpenIdAvailable.boolValue);
                if (isAppOpenIdAvailable.boolValue)
                {
                    EditorGUILayout.PropertyField(appOpenID);
                    EditorGUILayout.Space();
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("Testing Mode Enable With Admob Test IDs, Make Sure To Disable After Testing.", MessageType.Warning);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("Other Settings", headingStyle);
            EditorGUILayout.Space();

            isDebugLog.boolValue = EditorGUILayout.Toggle("Debug Log", isDebugLog.boolValue);
            if (isDebugLog.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("Debugging Enable, Help you to debug every Request/Response via Debug.Log(msg), Make sure to Disable after Testing.", MessageType.Warning);
                EditorGUILayout.EndHorizontal();
            }
            isBannerBGEnable.boolValue = EditorGUILayout.Toggle("Banner BG", isBannerBGEnable.boolValue);

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("Memory Settings", headingStyle);
            EditorGUILayout.Space();

            isMemoryThreshold.boolValue = EditorGUILayout.Toggle("Memory Threshold", isMemoryThreshold.boolValue);
            if (isMemoryThreshold.boolValue)
            {
                EditorGUILayout.PropertyField(initilizationRequiredMinValue);
                EditorGUILayout.PropertyField(smallBannerRequiredMinValue);
                EditorGUILayout.PropertyField(largeBannerRequiredMinValue);
                EditorGUILayout.PropertyField(nativeRequiredMinValue);
                EditorGUILayout.PropertyField(interstitialRequiredMinValue);
                EditorGUILayout.PropertyField(interstitialStaticRequiredMinValue);
                EditorGUILayout.PropertyField(rewardedRequiredMinValue);
                EditorGUILayout.PropertyField(rewardedInterstitialRequiredMinValue);
                EditorGUILayout.PropertyField(appOpenRequiredMinValue);
                isLowMemoryAnalytics.boolValue = EditorGUILayout.Toggle("Low Memory Analytics", isLowMemoryAnalytics.boolValue);
            }
            else
            {
                EditorGUILayout.HelpBox("No Available Memory Threshold Applied.", MessageType.Warning);
            }
            EditorGUILayout.EndVertical();

            GUIStyle versionStyle = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleRight
            };
            EditorGUILayout.LabelField("Mediation Plugin Version: " + pluginVersion, versionStyle);
            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CustomScriptDefineSymbols : EditorWindow
    {
        class FileStructure
        {
            public string filePath;
            public string fileName;
            public string fileType;

            public FileStructure(string path, string name, string type)
            {
                filePath = path;
                fileName = name;
                fileType = type;
            }
        }

        static System.Collections.Generic.List<FileStructure> files = new System.Collections.Generic.List<FileStructure>()
        {
            new FileStructure("Assets/Mobify/Admob Mediation/Scripts/","AdsManager_AdmobMediation",".cs"),
            new FileStructure("Assets/Firebase/Plugins/","Firebase.Analytics",".dll"),
            new FileStructure("Assets/Plugins/Android/","ANRSupervisor",".java"),
            new FileStructure("Assets/Plugins/Android/","unity-release",".aar"),
            new FileStructure("Assets/GoogleMobileAdsNative/","GoogleMobileAdsNative",".dll")
        };

        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                foreach (FileStructure file in files)
                {
                    bool isAssetPresent = AssetDatabase.LoadAssetAtPath(file.filePath + "" + file.fileName + "" + file.fileType, typeof(UnityEngine.Object)) != null;
                    if (isAssetPresent) SetEnabled(file.fileName, true);
                }
                EditorApplication.projectChanged += OnProjectChanged;
            }
        }
        static void SetEnabled(string defineName, bool enable)
        {
            defineName = new System.Text.RegularExpressions.Regex("['*-.,&#^@]").Replace(defineName, "_");
            foreach (var group in buildTargetGroups)
            {
                var defines = GetDefinesList(group);

                if (enable)
                {
                    if (defines.Contains(defineName))
                        return;
                    defines.Add(defineName);
                }
                else
                {
                    if (!defines.Contains(defineName))
                        return;
                    while (defines.Contains(defineName))
                    {
                        defines.Remove(defineName);
                    }
                }
                string definesString = string.Join(";", defines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
            }
        }
        static void OnProjectChanged()
        {
            foreach (FileStructure file in files)
            {
                bool isAssetPresent = AssetDatabase.LoadAssetAtPath(file.filePath + "" + file.fileName + "" + file.fileType, typeof(UnityEngine.Object)) != null;
                if (!isAssetPresent) SetEnabled(file.fileName, false);
            }
        }
        static System.Collections.Generic.List<string> GetDefinesList(BuildTargetGroup group)
        {
            return new System.Collections.Generic.List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
        }
        private static readonly BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.iOS, };
    }

#endif

    #endregion

    #region Custom Code

    readonly string removeAdsValue = "ADSUNLOCK";
    public string rewardedAdName = "";
    public delegate void GunAdDelegate(int index);
    public static GunAdDelegate GunAdHandler;
    public delegate void ModeAdDelegate(int index);
    public static ModeAdDelegate ModeAdHandler;
    public static bool DailyRewardChk = true, isGunAd = false;
    [HideInInspector] public int prefIndex, modeIndex = 0;

    public void GetRewardCustomCode()
    {
        if (prefIndex != 0 && (rewardedAdName == "" || rewardedAdName == null) && (WeaponStore.Instance != null && WeaponStore.Instance.fullSpecificationPanel.activeInHierarchy))
        {
            PlayerPrefs.SetInt("AdCount" + prefIndex, (PlayerPrefs.GetInt("AdCount" + prefIndex) + 1));
            if (GunAdHandler != null)
                GunAdHandler(prefIndex);

        }

        if (modeIndex != 0 && (MainMenuManager.Instance.modeSelectionPanel.activeInHierarchy))
        {
            PlayerPrefs.SetInt("ModeAdCount" + modeIndex, (PlayerPrefs.GetInt("ModeAdCount" + modeIndex) + 1));
            if (ModeAdHandler != null)
                ModeAdHandler(modeIndex);
        }

        if (isGunAd)
        {
            isGunAd = false;
            if (PlayerPrefs.HasKey("PendingGun"))
            {
                PlayerPrefs.SetInt("weapon15", 1);
                PlayerPrefs.SetInt("AssaultEquipped", 15);
                PlayerPrefs.DeleteKey("PendingGun");
            }
            else if (PlayerPrefs.HasKey("PendingGun1"))
            {
                PlayerPrefs.SetInt("weapon14", 1);
                PlayerPrefs.SetInt("AssaultEquipped", 14);
                PlayerPrefs.DeleteKey("PendingGun1");
            }
            if (WeaponStore.Instance != null)
            {
                WeaponStore.Instance.HideAds();
                WeaponStore.Instance.gameObject.GetComponent<NewStoreManager>()._AssaultButton();
            }
        }

        if (LoadOutManager.Instance != null)
        {
            if (LoadOutManager.Instance.isLoadoutAd > 0)
            {
                if (LoadOutManager.Instance.isLoadoutAd == 1)
                {
                    PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 1);
                    ConstantUpdate.Instance.UpdateCurrency();
                    if (AudioManager.instance != null)
                        AudioManager.instance.YoureWelcomeClick();
                }
                else
                {
                    PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 1);
                    ConstantUpdate.Instance.UpdateCurrency();
                    if (AudioManager.instance != null)
                        AudioManager.instance.YoureWelcomeClick();
                }
                LoadOutManager.Instance.isLoadoutAd = 0;
            }
        }

        if (RewardedAds.Instance != null)
        {
            if (RewardedAds.Instance.freeExplosive_G65)
            {
                PlayerPrefs.SetInt("nadeAddCount", PlayerPrefs.GetInt("nadeAddCount") + 1);
                if (PlayerPrefs.GetInt("nadeAddCount") >= 2)
                {
                    PlayerPrefs.SetInt("nadeAddCount", 0);
                    RewardedAds.Instance.freeExplosive_G65 = false;
                    RewardedAds.Instance.rewardText.text = "2 Explosive-G65!";
                    RewardedAds.Instance.ShowRewardPanel();
                    PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 2);
                    ConstantUpdate.Instance.UpdateCurrency();
                    if (SceneManager.GetActiveScene().name != "MainMenu")
                    {
                        GameManager.Instance.fpsPlayer.PlayerWeaponsComponent.InitWeapons();
                        GameManager.Instance.UpdateText();
                    }
                    if (AudioManager.instance != null)
                        AudioManager.instance.YoureWelcomeClick();
                }
            }
            else if (RewardedAds.Instance.freeAdrenaline_25)
            {
                PlayerPrefs.SetInt("adralineAddCount", PlayerPrefs.GetInt("adralineAddCount") + 1);
                if (PlayerPrefs.GetInt("adralineAddCount") >= 2)
                {
                    PlayerPrefs.SetInt("adralineAddCount", 0);
                    RewardedAds.Instance.freeAdrenaline_25 = false;
                    RewardedAds.Instance.rewardText.text = "2 Adrenaline-H25!";
                    RewardedAds.Instance.ShowRewardPanel();
                    PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 2);
                    ConstantUpdate.Instance.UpdateCurrency();
                    if (SceneManager.GetActiveScene().name != "MainMenu")
                    {
                        GameManager.Instance._MedicKitButton();
                    }
                    if (AudioManager.instance != null)
                        AudioManager.instance.YoureWelcomeClick();
                }
            }

            RewardedAds.Instance.freeAdrenaline_25 = false;
            RewardedAds.Instance.freeExplosive_G65 = false;
            RewardedAds.Instance.freeGold_12 = false;
            RewardedAds.Instance.freeSP_8 = false;

        }


        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (GameManager.Instance.freeRetryButtonPressed)
            {
                GameManager.Instance.freeRetryButtonPressed = false;
                GameManager.Instance.RevivePlayer();
                if (AudioManager.instance != null)
                    AudioManager.instance.YoureWelcomeClick();
            }
        }

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (GameManager.Instance.freeSecretButtonPressed)
            {
                GameManager.Instance.freeSecretButtonPressed = false;
                GameManager.Instance.SecretReward();
                if (AudioManager.instance != null)
                    AudioManager.instance.YoureWelcomeClick();
            }
        }

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (GameManager.Instance.doubleRewardButtonPressed)
            {
                GameManager.Instance.doubleRewardButtonPressed = false;
                GameManager.Instance.DoubleReward();
                if (AudioManager.instance != null)
                    AudioManager.instance.EnableSoundsAfterAds();
            }
        }
        if(rewardedAdName == "freecoins")
        {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 100);
            MainMenuManager.Instance.rewardedPanel.SetActive(true);
            MainMenuManager.Instance.rewardedText.text = "100 Coins";
            MainMenuManager.Instance.goldText.text = PlayerPrefs.GetInt("gold").ToString();
            rewardedAdName = "";
        }

        if (RewardedAds.Instance != null)
            RewardedAds.Instance.ShowAddsSeenCount();
    }

    public void SaveLastBannerState()
    {
        if (isSmallBannerShow || isSmallBannerGamePlayShow || isLargeBannerShow || isAdaptiveBannerShow || isSmartBannerShow)
        {
            afterLoadingBannerShowAction = null;
            afterLoadingBannerShowAction = ShowBanner;

            if (isSmallBannerShow) HideSmallBanner(isSmallBannerShow);
            if (isSmallBannerGamePlayShow) HideSmallBannerGamePlay(isSmallBannerGamePlayShow);
            if (isLargeBannerShow) HideLargeBanner(isLargeBannerShow);
            if (isAdaptiveBannerShow) HideAdaptiveBanner(isAdaptiveBannerShow);
            if (isSmartBannerShow) HideSmartBanner(isSmartBannerShow);
        }
    }

    #endregion
}