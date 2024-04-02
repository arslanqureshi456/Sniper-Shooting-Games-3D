using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;

public class GoogleMobileAdsManager : MonoBehaviour
{
    public static GoogleMobileAdsManager Instance;

    public delegate void CloseFullScreenAds();
    public static CloseFullScreenAds handleFullScreenAdClose;

    public delegate void GunAdDelegate(int index);
    public static GunAdDelegate GunAdHandler;
    public delegate void ModeAdDelegate(int index);
    public static ModeAdDelegate ModeAdHandler;
    public static bool DailyRewardChk = true, isGunAd = false;
    [HideInInspector] public int prefIndex, modeIndex = 0;

    public bool enableTestMode = false;

    // [Space(10)]
    // public string appID;
    [Space(10)]
    public string bannerID;
    [Space(10)]
    public string lowRefreshBannerID;
    [Space(10)]
    public string medBannerID;
    [Space(10)]
    public string interstitialID;
    [Space(10)]
    public string staticInterstitialID;
    [Space(10)]
    public string sessionInterstitialID;
    [Space(10)]
    public string appOpenInterstitialID;
    [Space(10)]
    public string rewardBasedVideoID;

    [HideInInspector] public BannerView bannerView = null;
    [HideInInspector] public BannerView medBannerView = null;
    [HideInInspector] private InterstitialAd interstitial = null;
    [HideInInspector] public AppOpenAd appOpen = null;
    [HideInInspector] public InterstitialAd sessionIinterstitial = null;
    [HideInInspector] private RewardedAd rewarded = null;

    public GameObject InterstitialLoadingPanel;
    public GameObject InterstitialNotLoadedPanel;
    public GameObject RewardedLoadingPanel;
    public GameObject RewardedNotLoadedPanel;


    [HideInInspector] public float bannerBGHeight;
    [HideInInspector] public float bannerBGWidth;

    private bool isInternet;

    // Ads Background Black Image
    public GameObject adLoadingObj;
    public Text adLoadingText;

    public void Awake()
    {
        MakeSingleton();

        // check the availability of internet connection
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            isInternet = false;
        }
        else
        {
            isInternet = true;
        }

    }

    private void OnEnable() {
        handleFullScreenAdClose += CloseLoadingPanel;
    }

    private void CloseLoadingPanel() {
        adLoadingObj.SetActive(false);
    }


    private void Start()
    {
        //  CheckForExcludedDevice();
        // Set Ads BG Canvas Layer Sorting
#if !UNITY_EDITOR
        adLoadingObj.GetComponent<Canvas>().sortingOrder = 50;
#else
        adLoadingObj.GetComponent<Canvas>().sortingOrder = -10;
#endif

        if (enableTestMode)
        {
            bannerID = medBannerID = lowRefreshBannerID = "ca-app-pub-3940256099942544/6300978111";
            interstitialID = sessionInterstitialID = staticInterstitialID = "ca-app-pub-3940256099942544/1033173712";
            rewardBasedVideoID = "ca-app-pub-3940256099942544/5224354917";
        }
        else
        {
            if(SystemInfo.systemMemorySize <= 3000)
            {
                interstitialID = staticInterstitialID;
                bannerID = lowRefreshBannerID;
            }
#if UNITY_EDITOR
            Debug.Log("Interstitial : " + interstitialID);
#endif
        }

        //if (deviceExcluded)
        //    return;

        if (isInternet)
        {
            // Initialize the Google Mobile Ads SDK.
            if (!IsLowMemory())
            {
                MobileAds.Initialize(status =>
            {
#if UNITY_EDITOR
                Debug.Log("Initialized Google Mobile Ads");
#endif
            });
                if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
                {
                    this.RequestSessionInterstitial();
                }
            }
        }
    }

  
    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Assigning Banner Background Image Size Here
    void BannerBGSize()
    {
        if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
        {
            bannerBGHeight = bannerView.GetHeightInPixels();
            bannerBGWidth = bannerView.GetWidthInPixels();
        }
    }

    //======================================== Banner AD =============================================================//

    public void RequestBanner()
    {
            if (isInternet)
            {
                if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
                {
                    if (this.bannerView == null)
                    {
                        if (!IsLowMemory())
                        {
                            this.bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Top);
                            AdRequest request = new AdRequest.Builder().Build();
                            this.bannerView.LoadAd(request);
                            this.bannerView.OnAdFailedToLoad += HandleAdFailedToLoad_SmallBanner;
                            // Assigning Banner Background Image Size Here
                            //Invoke("BannerBGSize", 2);
                            HideBanner();
                            BannerBGSize();
                        }
                    }
                }
            }
    }

    public void ShowBanner()
    {
        if (isInternet)
        {
            if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
            {
                    if (this.bannerView != null)
                        this.bannerView.Show();
            }
        }
    }
    public void HideBanner()
    {
        if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
            if (this.bannerView != null)
                this.bannerView.Hide();
    }

    //======================================== Medium Banner AD =============================================================//

    public void RequestMedBanner()
    {
            if (isInternet)
            {
                if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
                {
                    if (this.medBannerView == null)
                    {
                        if (!IsLowMemory())
                        {
                            this.medBannerView = new BannerView(medBannerID, AdSize.MediumRectangle, AdPosition.Bottom);
                            AdRequest request = new AdRequest.Builder().Build();
                            this.medBannerView.LoadAd(request);

                            this.medBannerView.OnAdFailedToLoad += HandleAdFailedToLoad_LargeBanner;
                            HideMedBanner();
                        }
                    }


                }
            }
    }

    public void ShowMedBanner()
    {
        if (isInternet)
        {
            if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
            {
                    if (this.medBannerView != null)
                        this.medBannerView.Show();
            }
        }
    }

    public void HideMedBanner()
    {
        if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
        {
            if (this.medBannerView != null)
                this.medBannerView.Hide();
        }
    }

    //======================================== Interstitial AD =======================================================//

    public void RequestInterstitial()
    {
            if (isInternet)
            {
                if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
                {
                    if (this.interstitial == null)
                    {
                        if (SystemInfo.systemMemorySize >= 3000)
                        {
                            if (IsLowMemory())
                            {
                                this.interstitial = new InterstitialAd(staticInterstitialID);
                            }
                            else
                            {
                                this.interstitial = new InterstitialAd(interstitialID);
                            }
                        }
                        else
                        {
                            if (!IsLowMemory())
                            {
                                this.interstitial = new InterstitialAd(staticInterstitialID);
                            }
                            else
                            {
                                return;
                            }
                        }
                        // Register for ad events.
                        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
                        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
                        AdRequest request = new AdRequest.Builder().Build();
                        this.interstitial.LoadAd(request);
                    }
                }

            }
    }

    public void ShowInterstitial()
    {
        if (isInternet)
        {
            if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
            {
                if (IsAdmobInterstitialLoaded())
                {
#if UNITY_EDITOR
                    Debug.Log("show interstitial");
#endif
                    StartCoroutine(ShowFullScreenAd(() =>
                    {
                        this.interstitial.Show();
                       // adLoadingText.text = "Loading . . .";
                    }));
                }
                else
                    handleFullScreenAdClose?.Invoke();
            }
        }
    }


    public void RequestSessionInterstitial()
    {
            if (isInternet)
            {
                if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
                {
                    if (!IsLowMemory())
                    {
                        if (SaveManager.Instance.state.loadFirstTime != 0)
                        {
                            if (this.sessionIinterstitial == null)
                            {
                                this.sessionIinterstitial = new InterstitialAd(sessionInterstitialID);
                                // Register for ad events.
                                this.sessionIinterstitial.OnAdFailedToLoad += this.HandleSessionInterstitialFailedToLoad;
                                this.sessionIinterstitial.OnAdClosed += this.HandleSessionInterstitialClosed;
                                AdRequest request = new AdRequest.Builder().Build();
                                this.sessionIinterstitial.LoadAd(request);
#if UNITY_EDITOR
                                Debug.Log("Session Ad Request Sent");
#endif
                            }

                        }
                    }
                }
            }
    }

    public void ShowSessionInterstitial()
    {
        if (isInternet)
        {
            if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
            {
                if (this.sessionIinterstitial != null)
                    {
                        if (this.sessionIinterstitial.IsLoaded())
                        {
                            StartCoroutine(ShowFullScreenAd(() =>
                            {
                                this.sessionIinterstitial.Show();
                               // adLoadingText.text = "Loading . . .";
                            }));
                        }
                        else
                            handleFullScreenAdClose?.Invoke();
                    }
            }
        }
    }


  


    //======================================== Rewarded AD =======================================================//

    private static int rewardRequest = 0;
    public void RequestRewarded()
    {
            if (isInternet)
            {
                if (!IsLowMemory())
                {
                    if (this.rewarded == null)
                    {
                        this.rewarded = new RewardedAd(rewardBasedVideoID);

                    }
                    AdRequest request = new AdRequest.Builder().Build();
                    this.rewarded.LoadAd(request);
                    if (rewardRequest == 0)
                    {
                        // Called when the user should be rewarded for interacting with the ad.
                        this.rewarded.OnUserEarnedReward += HandleUserEarnedReward;
                        // Called when the ad is closed.
                        this.rewarded.OnAdClosed += HandleRewardedAdClosed;
                        this.rewarded.OnAdOpening += HandleRewardedAdOpened;
                        rewardRequest = 1;
                    }
                }
            }
    }

    public void ShowRewarded()
    {
        if (isInternet)
        {
                if (isAdmobRewardLoaded())
                {
                        StartCoroutine(ShowFullScreenAd(() =>
                        {
                            this.rewarded.Show();
                           // adLoadingText.text = "Loading . . .";
                        }));
                }
                else
                    handleFullScreenAdClose?.Invoke();
        }
    }

    private string rewardedAdName;
    public void ShowRewardedByName(string adName)
    {
        if (isInternet)
        {
            if (isAdmobRewardLoaded())
            {
                    try
                    {
                        AudioManager.instance.DisableSoundsForAds();
                        rewardedAdName = adName;
                        StartCoroutine(ShowFullScreenAd(() =>
                        {
                            this.rewarded.Show();
                           // adLoadingText.text = "Loading . . .";
                        }));
                    }
                    catch (Exception e)
                    {
#if UNITY_EDITOR
                        Debug.LogError("Exception : " + e);
#endif
                    }
            }
            else
                handleFullScreenAdClose?.Invoke();

        }
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        handleFullScreenAdClose?.Invoke();
    }
    public void HandleRewardedAdOpened(object sender, EventArgs args)
    {
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
            Time.timeScale = 1;
            StartCoroutine(GetReward());
            this.RequestRewarded();
      //  handleFullScreenAdClose?.Invoke();
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


    //==================================================================================================================//

    [Space(10)]
    public string currentModel;
    public List<string> devicesToExclude;

    private bool deviceExcluded;
    
    //METHOD FOR CHECKING DEVICE EXCLUSION
    private void GetDeviceExclusion(string currentDeviceModel)
    {
        if(devicesToExclude.Count > 0)
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = 0; i < devicesToExclude.Count; i++)
        {
            devicesToExclude[i] = devicesToExclude[i].ToLower();
        }
    }
#endif

    public void RePosition(AdPosition _adPosition)
    {
        //if (deviceExcluded)
        //    return;
        if (isInternet)
        {
            if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
            {
                try
                {
                    medBannerView.SetPosition(_adPosition);
                }
                catch (Exception e) {
#if UNITY_EDITOR
                    Debug.LogError("Exception : " + e);
#endif
                }
            }
        }
    }

#region Banner Handlers
    public void HandleAdFailedToLoad_SmallBanner(object sender, AdFailedToLoadEventArgs args)
    {
            bannerView = null;
    }									
									
	public void HandleAdFailedToLoad_LargeBanner(object sender, AdFailedToLoadEventArgs args)
    {
            medBannerView = null;
    }
#endregion

#region Interstitial callback handlers

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
            this.interstitial = null;
    }

    public void HandleSessionInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
            this.sessionIinterstitial = null;
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
            handleFullScreenAdClose?.Invoke();
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.pauseButtons.activeInHierarchy)
                {
                    Time.timeScale = 0;
                }
            }
            this.interstitial = null;
        
    }

    public void HandleSessionInterstitialClosed(object sender, EventArgs args)
    {
            handleFullScreenAdClose?.Invoke();
            this.sessionIinterstitial = null;

    }
#endregion



    public IEnumerator ShowFullScreenAd(Action adToShow, float adWaitTime = 1.0f)
    {
        adLoadingText.text = "Loading Ad . . .";
		adLoadingObj.SetActive(true);
        yield return new WaitForSecondsRealtime(adWaitTime);
        adToShow?.Invoke(); 
    }

    public bool IsSmallBannerLoaded()
    {
        return (bannerView != null) ? true : false;
    }

    public bool IsAdmobInterstitialLoaded()
    {
        if(interstitial != null && !IsLowMemory())
        {
            return interstitial.IsLoaded();
        }
        else
        {
            return false;
        }
    }

    public bool IsAdmobSessionLoaded()
    {
        if (sessionIinterstitial != null && !IsLowMemory())
        {
            return sessionIinterstitial.IsLoaded();
        }
        else
        {
            return false;
        }
    }

    public bool isAdmobRewardLoaded()
    {
        if (rewarded != null && !IsLowMemory())
        {
            return rewarded.IsLoaded();
        }
        else
        {
            return false;
        }
    }

    #region Memory Information

    readonly int memoryThreshHold = 500; //in MBs
    static int memoryAvailable = 0;
    static Regex re = new Regex(@"\d+");

    // return True if memory low by defined threshHold
    //public bool IsLowMemory()
    //{
    //    try
    //    {
    //        return LoadMemoryInfo().Equals(true) ? (memoryAvailable / 1024) <= memoryThreshHold : false;
    //    }
    //    catch (Exception) { return true; }
    //}

    public bool IsLowMemory()
    {
        try
        {
            bool returnValue = false;
            string parameterValue = "defaultValue";
            if (LoadMemoryInfo().Equals(true))
            {
                float availableInMB = (memoryAvailable / 1024);
                if (availableInMB <= memoryThreshHold)
                {
                    if (availableInMB < 200)
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
            return returnValue;
        }
        catch (Exception) { return true; }
    }

    static bool LoadMemoryInfo()
    {
        try
        {
            //if file not exist return from here
            if (!File.Exists("/proc/meminfo")) return false;
            FileStream fs = new FileStream("/proc/meminfo", FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);
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
    #endregion

    private void OnDisable()
    {
        handleFullScreenAdClose -= CloseLoadingPanel;

        StopCoroutine("GetReward");
        StopCoroutine("RequestAdmob");
        CancelInvoke("BannerBGSize");
    }
}