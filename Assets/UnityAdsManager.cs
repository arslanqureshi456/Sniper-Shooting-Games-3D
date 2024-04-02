using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Analytics;


public class UnityAdsManager : MonoBehaviour , IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener  //IUnityAdsListener
{
    public static UnityAdsManager Instance;

    public delegate void GunAdDelegate(int index);
    public static GunAdDelegate GunAdHandler;
    public delegate void ModeAdDelegate(int index);
    public static ModeAdDelegate ModeAdHandler;
    public static bool DailyRewardChk = true, isGunAd = false;
    [HideInInspector] public int prefIndex, modeIndex = 0;
    private StoreWeapons[] storeWeapons;

    [SerializeField]
    private string gameID;

    [SerializeField]
    private bool enableTestMode;

    private bool isInternet;
    private string RewardedVideoID = "Rewarded_Android", UnityinterstitialID = "Interstitial_Android";
    public bool IsUnityRewarded, IsUnityInterstitial;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // check the availability of internet connection
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            isInternet = false;
        }
        else
        {
            isInternet = true;
        }
        InitializeUnityAds();
    }

   public void InitializeUnityAds()
    {
#if UNITY_EDITOR
        print("UnityAds Initialization");
#endif
        if (isInternet)
        {
            if (!GoogleMobileAdsManager.Instance.IsLowMemory())
            {
                Advertisement.Initialize(gameID, enableTestMode, this);
            }
        }
    }

    public bool IsVideoReady()
    {

        //if (deviceExcluded)
        //    return false;

        if (PlayerPrefs.GetInt("ADSUNLOCK") == 1)
            return false;

        if (IsUnityInterstitial) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsRewardedVideoReady()
    {
        //if (deviceExcluded)
        //    return false;

        if (PlayerPrefs.GetInt("ADSUNLOCK") == 1)
            return false;
        if  (IsUnityRewarded) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowUnityVideoAd()
    {
        //if (deviceExcluded)
        //    return;

        if (PlayerPrefs.GetInt("ADSUNLOCK") == 1)
            return;
        if (isInternet)
        {
            if (!GoogleMobileAdsManager.Instance.IsLowMemory())
            {
                AudioManager.instance.DisableSoundsForAds();
                StartCoroutine(GoogleMobileAdsManager.Instance.ShowFullScreenAd(() =>
                {
                    Advertisement.Show(UnityinterstitialID, this);
                    //GoogleMobileAdsManager.Instance.adLoadingText.text = "Loading . . .";
                }));
            }
        }
        else
            return;
    }

    public void ShowUnityRewardedVideoAd()
    {
        if (isInternet)
        {
            if (!GoogleMobileAdsManager.Instance.IsLowMemory())
            {
                AudioManager.instance.DisableSoundsForAds();
#if UNITY_EDITOR
                Debug.Log($"MUSTAFA");
#endif
                StartCoroutine(GoogleMobileAdsManager.Instance.ShowFullScreenAd(() =>
                {
                    Advertisement.Show(RewardedVideoID, this);
                   // GoogleMobileAdsManager.Instance.adLoadingText.text = "Loading . . .";
                }));
            }
        }
    }

    private string rewardedAdName;
    public void ShowRewardedByName(string adName)
    {
        if (isInternet)
        {
            if (!GoogleMobileAdsManager.Instance.IsLowMemory())
            {
                AudioManager.instance.DisableSoundsForAds();
                rewardedAdName = adName;
                StartCoroutine(GoogleMobileAdsManager.Instance.ShowFullScreenAd(() =>
                {
                    Advertisement.Show(RewardedVideoID, this);
                   // GoogleMobileAdsManager.Instance.adLoadingText.text = "Loading . . .";
                }));
            }
        }
        else
            return;
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
#if UNITY_EDITOR
        Debug.Log($"KAMAL: {placementId}");
#endif
        if (placementId == RewardedVideoID)
        {
            IsUnityRewarded = true;
#if UNITY_EDITOR
            Debug.Log("KAMAL 2");
#endif
        }
        else if (placementId == UnityinterstitialID)
            IsUnityInterstitial = true;
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
            GoogleMobileAdsManager.handleFullScreenAdClose?.Invoke();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
     // GoogleMobileAdsManager.Instance.adLoadingText.text = "Loading . . .";
    }
    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Time.timeScale = 1;
        if (placementId == RewardedVideoID)
        {
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                try {
                    Time.timeScale = 1;
                    StartCoroutine(GetReward());
                }
                    catch { }
            }
            IsUnityRewarded = false;
            Advertisement.Load(RewardedVideoID, this);
        }
        else if (placementId == UnityinterstitialID)
        {
            IsUnityInterstitial = false;
            Advertisement.Load(UnityinterstitialID, this); 
        }
        GoogleMobileAdsManager.handleFullScreenAdClose?.Invoke();
    }

    IEnumerator GetReward()
    {
        yield return new WaitForSeconds(0.3f);
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
                    yield return new WaitForSeconds(1.0f);
                    if (AudioManager.instance != null)
                        AudioManager.instance.YoureWelcomeClick();
                }
                else
                {
                    PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 1);
                    ConstantUpdate.Instance.UpdateCurrency();
                    yield return new WaitForSeconds(1.0f);
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
                    yield return new WaitForSeconds(1.0f);
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
                    yield return new WaitForSeconds(1.0f);
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
                yield return new WaitForSeconds(1.0f);
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
                yield return new WaitForSeconds(1.0f);
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
                yield return new WaitForSeconds(2.0f);
                if (AudioManager.instance != null)
                    AudioManager.instance.EnableSoundsAfterAds();
            }
        }
        if (RewardedAds.Instance != null)
            RewardedAds.Instance.ShowAddsSeenCount();
    }

   
    public void OnInitializationComplete()
    {
            Advertisement.Load(RewardedVideoID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    }

    private void OnDestroy()
    {
       // Advertisement.RemoveListener(this);
    }

    IEnumerator DelayedEnableSound()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.instance.EnableSoundsAfterAds();
        AudioManager.instance.PlayMainMenuThemeSound();
    }

    [Space(10)]
    public string currentModel;
    public List<string> devicesToExclude;

    private bool deviceExcluded;

    //METHOD FOR CHECKING DEVICE EXCLUSION
    private void GetDeviceExclusion(string currentDeviceModel)
    {
        if (devicesToExclude.Count > 0)
        {
            for (var i = 0; i < devicesToExclude.Count; i++)
            {
                if (currentDeviceModel.Contains(devicesToExclude[i]))
                {
                    deviceExcluded = true;
                    return;
                }
                else
                {
                    deviceExcluded = false;
                }
            }
        }
    }

    //METHOD TO CALL IN AWAKE BEFORE ANY ADS INITIALIZATION
    void CheckForExcludedDevice()
    {
        currentModel = SystemInfo.deviceModel.ToLower();
        GetDeviceExclusion(currentModel);
    }

    void OnDisable()
    {
        StopCoroutine("GetReward");
    }
}