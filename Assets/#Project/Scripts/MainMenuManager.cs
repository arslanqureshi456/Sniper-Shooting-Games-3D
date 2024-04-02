using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //Specfication arrow delegates
    public delegate void SpecificationDelegate();
    public SpecificationDelegate[] specificationDelegates = new SpecificationDelegate[25];
    public InAppManager _InAppManager;
    //Store delegates
    public delegate void SetLastCard();
    public static SetLastCard lastSniperCard;
    public static SetLastCard lastAssaultCard;
    public static SetLastCard StoreRedirect;
    public static SetLastCard LastLevelToggle;

    public static MainMenuManager Instance = null;
    public delegate void GunBuyDelegate(bool isSpecification);
    public static GunBuyDelegate buyDeletegate;
    public delegate void ProcessPaymentDelegate();
    public static ProcessPaymentDelegate processDelegate;
    public delegate void InAppDelegate();
    public static InAppDelegate onInAppDelegate;
    public delegate void GunDelegate();
    public static GunDelegate gunDelegate;

    public delegate void GunOffers();
    public GunOffers[] GunOffersDelegate = new GunOffers[10];

    public static int sniperModeButtonPressed = 0;
    public static int coverStrikeModeButtonPressed = 0;
    public static int TDMModeButtonPressed = 0;
    public static int freeForAllModeButtonPressed = 0;
    public static int BRModeButtonPressed = 0;
    public static int weaponModeButtonPressed = 0;
    public static int weaponIndex;

    int currentMode = 0;
    public Weapon[] OfferWeapons;
    public GameObject
        mainMenuPanel,
        quitPanel,
        storePanel,
        loadingPanel,
        PurchaseLevelPanel,
        creditsPanel,
        modeSelectionPanel,
        languagePanel,
        loadoutPanel,
        premissionPanel,
        tutorialPermissionPanel,
        tutorialNameEntryPanel,
        goldSpPanel,
        welcomeReward,
        nameTyperPanel,
        inAppProcessPanel,
        modeselection;
    //Not enough currency Panel
    public GameObject purchaseTextGold, purchaseTextSP, goldRewardedAdPopup, spRewardedAdPopup;
    public Sprite gold, sp;
    public Image currencyImage;
    //Piggy bank
    public GameObject  rewardPanel;
    public Text bankedSPText, bankedGoldText;

    // For Tutorial
    public GameObject[] tutorialPanels, highlightImages;
    public bool isLoadout = false, isRedirect, isGoldRewardedADPopup, isSPRewardedADPopup = false;
    // Packs
    //public GameObject _packsPanel;
    //public GameObject[] packsPanel; // Starter, Extraordinary, Premium;
    public Text[] packsPriceText, disPacksPriceText;

  

    bool _isGold = false;

    public GameObject gunsCamera,environmentCamera;

    

    public enum GunAdType
    {
        none,
        sniper,
        assault
    }
    public GunAdType gunAd = GunAdType.none;

    public GameObject RemoveAdButton; // RemoveAdButton to remove this after in-app for remove ads
    public GameObject IAPAllGunsBtn; // button to purchase all guns
    public GameObject sessionAdLoading;
    public Text versionText;


    private void Start()
    {
        GoToMainMenu();

#if UNITY_ANDROID
        Debug.Log("Debug : MainMenu Scene");
#endif
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }


        // calling session ad loading here
        if (SaveManager.Instance.Session == 1)
        {
            sessionAdLoading.SetActive(true);
            versionText.text = "V " + Application.version;
        }


        // Check To See If All Guns Are Purchased Or Unlocked
        for (int i = 0; i < 25; i++)
        {
            if (PlayerPrefs.GetInt("weapon" + i) == 1)
            {
                IAPAllGunsBtn.SetActive(false);
            }
            else
            {
                IAPAllGunsBtn.SetActive(true);
            }
        }
        // Check If IAPAllGuns Button Active In Hierarchy
        if (!IAPAllGunsBtn.activeInHierarchy)
        {
            GameManagerStatic.Instance.AllGunsUnlocked = 1;
        }
        // Check To See If All Modes Are Purchased Or Unlocked
        if (PlayerPrefs.GetInt("Mode1") == 1 &&
            PlayerPrefs.GetInt("Mode2") == 1 &&
            PlayerPrefs.GetInt("Mode3") == 1 &&
            PlayerPrefs.GetInt("Mode4") == 1 &&
            PlayerPrefs.GetInt("Mode5") == 1)
        {
            GameManagerStatic.Instance.AllModesUnlocked = 1;
        }


        //ADS
        GoogleMobileAdsManager.Instance.HideMedBanner();
        //Invoke("DelayedAdd", 1.5f);
       

        LastLevelToggle = lastSniperCard = null;
        lastAssaultCard = null;

        Screen.SetResolution(PlayerPrefs.GetInt("Width"), PlayerPrefs.GetInt("Height"), true, 60);

        //Dynamic Offers Shiz
        Transform trans = storePanel.transform.GetChild(3).GetChild(0).GetChild(0);
        int len = trans.childCount;
        for (int i = 0; i < len; i++)
        {
            trans.GetChild(i).GetComponent<StoreWeapons>().SetOffersDelegate();
        }
        trans = storePanel.transform.GetChild(3).GetChild(1).GetChild(0);
        len = trans.childCount;
        for (int i = 0; i < len; i++)
        {
            trans.GetChild(i).GetComponent<StoreWeapons>().SetOffersDelegate();
        }


        // Unity Analytics
        //Debug.Log("Login 1");
        Analytics.CustomEvent("Login", new Dictionary<string, object>
        {
        { "level_index", 1 }
        });
#if UNITY_EDITOR
        Debug.Log("CustomEvent: " + "Login");
#endif

        if (tutorialPanels[0].activeSelf)
        {
            switch (AutoQualityChooser.finalResult)
            {
                case 0:
#if UNITY_EDITOR
                    Debug.Log("FinalResultZero");
#endif
                    Analytics.CustomEvent("FinalResultZero", new Dictionary<string, object>
                {
                { "level_index", 1}
                });
#if UNITY_EDITOR
                    Debug.Log("CustomEvent: " + "FinalResultZero");
#endif
                    Analytics.CustomEvent("FinalResultOne", new Dictionary<string, object>
                {
                { "level_index", 0 }
                });
#if UNITY_EDITOR
                    Debug.Log("CustomEvent: " + "FinalResultOne");
#endif
                    Analytics.CustomEvent("FinalResultTwo", new Dictionary<string, object>
                {
                { "level_index", 0 }
                });
#if UNITY_EDITOR
                    Debug.Log("CustomEvent: " + "FinalResultTwo");
#endif
                    break;
                case 1:
#if UNITY_EDITOR
                    Debug.Log("FinalResultOne");
#endif
                    Analytics.CustomEvent("FinalResultZero", new Dictionary<string, object>
                {
                { "level_index" , 0}
                });
#if UNITY_EDITOR
                    Debug.Log("CustomEvent: " + "FinalResultZero");
#endif
                    Analytics.CustomEvent("FinalResultOne", new Dictionary<string, object>
                {
                { "level_index" , 1 }
                });
#if UNITY_EDITOR
                    Debug.Log("CustomEvent: " + "FinalResultOne");
#endif
                    Analytics.CustomEvent("FinalResultTwo", new Dictionary<string, object>
                {
                { "level_index" , 0 }
                });
#if UNITY_EDITOR
                    Debug.Log("CustomEvent: " + "FinalResultTwo");
#endif
                    break;
                case 2:
                    //Debug.Log("FinalResultTwo");
                    Analytics.CustomEvent("FinalResultZero", new Dictionary<string, object>
                {
                { "level_index" , 0}
                });
#if UNITY_EDITOR
                    Debug.Log("CustomEvent: " + "FinalResultZero");
#endif
                    Analytics.CustomEvent("FinalResultOne", new Dictionary<string, object>
                {
                { "level_index" , 0 }
                });
#if UNITY_EDITOR
                    Debug.Log("CustomEvent: " + "FinalResultOne");
#endif
                    Analytics.CustomEvent("FinalResultTwo", new Dictionary<string, object>
                {
                { "level_index" , 1 }
                });
#if UNITY_EDITOR
                    Debug.Log("CustomEvent: " + "FinalResultTwo");
#endif
                    break;
            }



#if UNITY_EDITOR
print("Multiplayer Mode : " + PlayerPrefs.GetInt("MultiplayerMode"));
            print("Current Mode : "+ PlayerPrefs.GetInt("currentMode"));
#endif

        }
        
        AudioListener.volume = 1;

        // For Scroll Rect Issue
        Cursor.lockState = CursorLockMode.None;
        //ABDUL
        if (PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1) || PlayerPrefs.GetInt("levelUnlocked-1").Equals(1) ||
            PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-1").Equals(1))
        {
            foreach(GameObject go in tutorialPanels)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in highlightImages)
            {
                go.SetActive(false);
            }
        }

        if (SaveManager.Instance.state.gamePlayLoadoutButtonPressed == 1)
        {
            SaveManager.Instance.state.gamePlayLoadoutButtonPressed = 0;
            storePanel.SetActive(true);
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            {
                specificationDelegates[PlayerPrefs.GetInt("SniperEquipped")]();
            }
            else
            {
                specificationDelegates[PlayerPrefs.GetInt("AssaultEquipped")]();
            }
        }
        else if (SaveManager.Instance.state.nextButtonPressed.Equals(1))
        {
            SaveManager.Instance.state.nextButtonPressed = 0;
            EnablePanel(modeSelectionPanel);
        }
        else
        {
                
            EnablePanel(mainMenuPanel);
            if (!tutorialPanels[0].activeSelf &&
            SaveManager.Instance.state.nameEntryPanelSeen.Equals(0))
            {
               // tutorialNameEntryPanel.SetActive(true);
            }

            if (!PlayerPrefs.HasKey("MainMenuCount"))
            {
                PlayerPrefs.SetInt("MainMenuCount", 1);
            }
        }



        //Piggy Visuals

        bankedSPText.text = System.String.Empty + PlayerPrefs.GetInt("BankedSP");
        bankedGoldText.text = System.String.Empty + PlayerPrefs.GetInt("BankedGold");


        // Weapon Popup for Purchase
        if (GameManager.purchaseButtonPresed)
        {
            GameManager.purchaseButtonPresed = false;
            _FeaturedWeaponsButton(GameManager.currentPopupIndex);
        }

        //// Login
        //if (!PlayerPrefs.GetInt("Start_CutScene").Equals(0))
        //{
        //    if (!PlayerPrefs.GetInt("Login").Equals(1))
        //    {
        //        //if (!PlayerPrefs.HasKey("Name") || PlayerPrefs.GetString("Name").Equals(""))
        //        PlayerPrefs.SetString("Name", "Player" + Random.Range(1, 9999).ToString());
        //        PlayerPrefs.SetInt("Login", 1);
        //        _CloseLoginButton();
        //        //AudioManager.instance.LoginButtonClick();
        //        //loginPanel.SetActive(true);
        //    }
        //}

        // Audio
        AudioManager.instance.ChangeBackgroundVolume(SaveManager.Instance.state.backgroundVolume);
        AudioManager.instance.ChangeSoundFxVolume(SaveManager.Instance.state.soundFxVolume);
        
        AudioManager.instance.backgroundMusicSouce.Play();

        

        if (Preloader.fromPreloader)
        {
            Preloader.fromPreloader = false;
            AudioManager.instance.WelcomeBackClick();
        }

        if(sniperModeButtonPressed.Equals(1))
        {
            sniperModeButtonPressed = 0;
            EnablePanel(modeselection);
            currentMode = 0;
            EnablePanel(modeSelectionPanel);
            LevelSelectionNew.Instance.ModeChanged(1);
        }
        else if(coverStrikeModeButtonPressed.Equals(1))
        {
            coverStrikeModeButtonPressed = 0;
            EnablePanel(modeselection);
            currentMode = 0;
            EnablePanel(modeSelectionPanel);
            LevelSelectionNew.Instance.ModeChanged(5);
        }
        else if (TDMModeButtonPressed.Equals(1))
        {
            TDMModeButtonPressed = 0;
            EnablePanel(modeselection);
            currentMode = 1;
            EnablePanel(modeSelectionPanel);
            LevelSelectionNew.Instance.ModeChanged(2);
        }
        else if (freeForAllModeButtonPressed.Equals(1))
        {
            freeForAllModeButtonPressed = 0;
            EnablePanel(modeselection);
            currentMode = 1;
            EnablePanel(modeSelectionPanel);
            LevelSelectionNew.Instance.ModeChanged(3);
        }
        else if (BRModeButtonPressed.Equals(1))
        {
            BRModeButtonPressed = 0;
            EnablePanel(modeselection);
            currentMode = 1;
            EnablePanel(modeSelectionPanel);
            LevelSelectionNew.Instance.ModeChanged(4);
        }
        else if (weaponModeButtonPressed.Equals(1))
        {
            //weaponModeButtonPressed = 0;
            WeaponStore.weaponPopupButtonPressed = 1;
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT ||
                LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.COVERSTRIKE)
            {
                lastSniperCard = null;
                lastAssaultCard = null;
                storePanel.SetActive(true);
                storePanel.GetComponent<NewStoreManager>()._AssaultButton();
                //yield return new WaitForSeconds(0.1f);
                WeaponStore.Instance.assaultPanel.transform.GetChild(0).GetChild(weaponIndex).GetComponent<StoreWeapons>().SelectGun();
            }
            else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            {
                lastSniperCard = null;
                lastAssaultCard = null;
                storePanel.SetActive(true);
                storePanel.GetComponent<NewStoreManager>()._SniperButton();
                //yield return new WaitForSeconds(0.1f);
                WeaponStore.Instance.sniperPanel.transform.GetChild(0).GetChild(weaponIndex).GetComponent<StoreWeapons>().SelectGun();
            }
        }

        // Remove , removeadbutton after in-app
        if (PlayerPrefs.GetInt("ADSUNLOCK") == 1)
        {
            RemoveAdButton.SetActive(false);
        }

        // calling session ad here
        if (SaveManager.Instance.Session == 1)
        {
            Invoke("ShowSessionAd", 3);
        }
        else {

            GoogleMobileAdsManager.handleFullScreenAdClose?.Invoke();
        }

        //if (GameManagerStatic.Instance.NextLevelClicked)
        //{
        //    _PlayButton();
        //    GameManagerStatic.Instance.NextLevelClicked = false;
        //}

#if UNITY_EDITOR
        print("Sniper Mode : " + PlayerPrefs.GetInt("Mode1"));
        print("GameManagerStatic.Instance.GunsStoreFrom : " + GameManagerStatic.Instance.GunsStoreFrom);
#endif

        // initialize Unity Ads , If Not Initilized
        if (GameManagerStatic.Instance.isGamePlaySceneEnabled == 1 && GameManagerStatic.Instance.isUnityAdsInitialized == 0)
        {
            UnityAdsManager.Instance.InitializeUnityAds();
            GameManagerStatic.Instance.isUnityAdsInitialized = 1;
        }
    }

    private void GoToMainMenu() {
        GoogleMobileAdsManager.handleFullScreenAdClose += DelayedAdd;

        EnablePanel(mainMenuPanel);
    }


    void LoadAdmobInterstitial()
    {
        if (PlayerPrefs.GetInt("AllowSessionAd").Equals(1))
        {
            GoogleMobileAdsManager.Instance.RequestInterstitial();
        }
    }

    void ShowSessionAd()
    {
        if ((GoogleMobileAdsManager.Instance != null && GoogleMobileAdsManager.Instance.IsAdmobSessionLoaded()) && GameManagerStatic.Instance.isSessionShown == 0)
        {
            GoogleMobileAdsManager.Instance.ShowSessionInterstitial();
            GameManagerStatic.Instance.isSessionShown = 1;
            SaveManager.Instance.Session = 0;
            if (sessionAdLoading.activeInHierarchy)
            {
                sessionAdLoading.SetActive(false);
            }
        }
        else
        {
            GoogleMobileAdsManager.Instance.RequestSessionInterstitial();
            sessionAdLoading.SetActive(false);
        }
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !quitPanel.activeInHierarchy && mainMenuPanel.activeInHierarchy)
        {
            _QuitButton();
        }
    }
    public void DelayedAdd()
    {
        GoogleMobileAdsManager.Instance.ShowBanner();
    }
    private void EnablePanel(GameObject panelToShow)
    {
        mainMenuPanel.SetActive(false);
        quitPanel.SetActive(false);
        //levelSelectionPanel.SetActive(false);
         // storePanel.SetActive(false);
        loadingPanel.SetActive(false);
        modeSelectionPanel.SetActive(false);
        modeselection.SetActive(false);
        panelToShow.SetActive(true);
    }

   

    //public void ShowRewardPanel()
    //{
    //    rewardPanel.SetActive(true);
    //}

    #region Button Methods
//    public void _EnableEditNamePanelButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Enable EditName Panel");
//#endif
//        AudioManager.instance.LoginButtonClick();
//        SaveManager.Instance.state.nameEntryPanelSeen = 1;
//        SaveManager.Instance.Save();
//        //tutorialNameEntryPanel.SetActive(false);
//        loginPanel.SetActive(true);
 //  }

    //public void _PlayerName(string name)
    //{
    //    PlayerPrefs.SetString("Name", name);
    //}

//    public void _CloseLoginButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Submit OR Close EditName Panel");
//#endif
//        //AudioManager.instance.BackClickNew();
//        EnablePanel(mainMenuPanel);
//        ShowWelcome();
//        _playerLevel.OnEnable();
//    }

    public List<int> lockedWeaponIndexes;
    
    public void _LoadoutButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Menu LoadOut Button");
#endif
        #region oldwork
        //lastSniperCard = null;
        //lastAssaultCard = null;
        //storePanel.SetActive(true);
        //storePanel.GetComponent<NewStoreManager>()._AssaultButton();
        #endregion

        storePanel.SetActive(true);
        specificationDelegates[19]();
        GameManagerStatic.Instance.GunsStoreFrom = 0;
#if UNITY_EDITOR
        print("Assault Gun : " + PlayerPrefs.GetInt("AssaultEquipped"));
        print("Sniper Gun : " + PlayerPrefs.GetInt("SniperEquipped"));
#endif
        WeaponStore.Instance.CheckUnlockedGuns();
        //specificationDelegates[19]();

        // Initialize Admob Rewarded , If Not Initialized
        if (!GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
        {
            GoogleMobileAdsManager.Instance.RequestRewarded();
        }
    }

    public void _FeaturedWeaponsButton(int index)
    {
        storePanel.SetActive(true);
        AudioManager.instance.StoreButtonClick();
        GunOffersDelegate[index]();
        //storePanel.GetComponent<WeaponStore>()._SelectWeaponButton(index);
    }

    public void _TowardFeaturedWeapons()
    {
        GameManagerStatic.Instance.isFeaturedGuns = true;
        WeaponStore.Instance.previousGunsBtn.SetActive(false);
        WeaponStore.Instance.nextGunsBtn.SetActive(false);

    }

    public void _PlayButton()
    {
        //Debug.Log("MainMenuPlay 1");
        AudioManager.instance.PlayButtonClick();
//        if (tutorialPanels[0].activeSelf)
//        {
//            Analytics.CustomEvent("Play_Tut", new Dictionary<string, object>//MainMenuPlay
//        {
//            { "level_index", 1 }
//        });
//#if UNITY_EDITOR
//            Debug.Log("CustomEvent: " + "Play_Tut");
//#endif 
//        }
        
        EnablePanel(modeselection);

        // Loading Admob Interstitial
        if (!GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
        {
            Invoke("LoadAdmobInterstitial", 0.5f);
        }

        #region Show Session Ad If Not Shown Yet
        if (SaveManager.Instance.Session == 1)
        {
            if ((GoogleMobileAdsManager.Instance != null && GoogleMobileAdsManager.Instance.IsAdmobSessionLoaded()) && GameManagerStatic.Instance.isSessionShown == 0)
            {
                GoogleMobileAdsManager.Instance.ShowSessionInterstitial();
                GameManagerStatic.Instance.isSessionShown = 1;
                SaveManager.Instance.Session = 0;
            }
        }
        #endregion
    }

    public void _RateButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Go To RateGameButton");
#endif
        AudioManager.instance.NormalClick2();
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void _MoreButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Go To MoreGamesButton");
#endif
        AudioManager.instance.NormalClick2();
         Application.OpenURL("https://play.google.com/store/apps/dev?id=7036477655681473153");
    }

    public void _PrivacyPolicyButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Go To PrivacyPolicyButton");
#endif
        AudioManager.instance.NormalClick2();
        Application.OpenURL("http://snipershootinggames3d.trilogixs.com/");
    }

//    public void _WebsiteButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To WebsiteButton");
//#endif
//        AudioManager.instance.NormalClick2();
//        Application.OpenURL("https://gamexis.com/");
//    }

//    public void _FBButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To FaceBookButton");
//#endif
//        AudioManager.instance.NormalClick2();
//        Application.OpenURL("https://www.facebook.com/gamexisofficial/");
//    }

//    public void _TwitterButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To TwitterButton");
//#endif
//        AudioManager.instance.NormalClick2();
//        Application.OpenURL("https://twitter.com/gamexisofficial");
//    }

//    public void _YoutubeButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To YoutubeButton");
//#endif
//        AudioManager.instance.NormalClick2();
//        Application.OpenURL("https://www.youtube.com/channel/UCsGGnLVuwtRaKzNP3qqJc4Q");
//    }

//    public void _InstagramButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To InstagramButton");
//#endif
//        AudioManager.instance.NormalClick2();
//        Application.OpenURL("https://www.instagram.com/gamexisofficial/");
//    }

//    public void _SettingButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Menu Settings Button");
//#endif
//        AudioManager.instance.NormalClick();
//        EnablePanel(settingPanel);
//        GoogleMobileAdsManager.Instance.RePosition(GoogleMobileAds.Api.AdPosition.BottomLeft);
//        GoogleMobileAdsManager.Instance.ShowMedBanner();
//        GoogleMobileAdsManager.Instance.HideBanner();
//    }

//    public void _CloseSettingButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Close Menu Settings");
//#endif
//        AudioManager.instance.BackClickNew();

//        if (!tutorialPanels[0].activeSelf &&
//            SaveManager.Instance.state.nameEntryPanelSeen.Equals(0))
//        {
//           // tutorialNameEntryPanel.SetActive(true);
//        }
//        GoogleMobileAdsManager.Instance.RePosition(GoogleMobileAds.Api.AdPosition.Bottom);
//        GoogleMobileAdsManager.Instance.HideMedBanner();
//        GoogleMobileAdsManager.Instance.ShowBanner();
//        EnablePanel(mainMenuPanel);
//    }

//    public void _SoundFxSlider(float value)
//    {
//        AudioManager.instance.ChangeSoundFxVolume(value);
//        SaveManager.Instance.state.soundFxVolume = value;
//        SaveManager.Instance.Save();
//    }
//    public void _DifficultySlider(float value)
//    {
//        PlayerPrefs.SetFloat("Difficulty", difficultySlider.value);
//    }
//    public void _BackgroundMusicSlider(float value)
//    {
//        AudioManager.instance.ChangeBackgroundVolume(value);
//        SaveManager.Instance.state.backgroundVolume = value;
//        SaveManager.Instance.Save();
//    }

//    public void _ControlSensitivitySlider(float value)
//    {
//        SaveManager.Instance.state.controlSensitivity = value;
//        SaveManager.Instance.Save();
//    }

//    public void _AutoShoot(bool value)
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Menu AutoShootToggle");
//#endif
//        if (AudioManager.instance != null)
//            AudioManager.instance.NormalClick();

//        if (value)
//        {
//            SaveManager.Instance.state.autoShoot = 1;
//        }
//        else
//        {
//            SaveManager.Instance.state.autoShoot = 0;
//        }
//        SaveManager.Instance.Save();
//    }
//    public void _BloodEffectToggle(bool value)
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Menu BloodEffectToggle");
//#endif
//        if (AudioManager.instance != null)
//            AudioManager.instance.NormalClick();

//        if (value)
//        {
//            PlayerPrefs.SetInt("BloodEffect", 1);
//        }
//        else
//        {
//            PlayerPrefs.SetInt("BloodEffect", 0);
//        }
//    }

//    public static int storeCount = 0;
//    public void _StoreButton()
//    {
//        //GoogleMobileAdsManager.handleFullScreenAdClose -= DelayedAdd;
//#if UNITY_ANDROID
//        Debug.Log("Debug : Menu Store Button");
//#endif
//        storeCount++;
//        AudioManager.instance.NormalClick2();
//        storePanel.SetActive(true);
//        storePanel.GetComponent<NewStoreManager>()._SPButton();
//        WeaponStore.Instance.StorePanel.SetActive(true);

//        // Initialize Admob Rewarded , If Not Initialized
//        if (!GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
//        {
//            GoogleMobileAdsManager.Instance.RequestRewarded();
//        }
//    }
    //public void _PassesButton()
    //{
    //    storeCount++;
    //    AudioManager.instance.NormalClick2();
    //    storePanel.SetActive(true);
    //    storePanel.GetComponent<NewStoreManager>()._PassesButton();
    //}
//    public void _FreeRewardsButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Free Reward Button");
//#endif
//        storeCount++;
//        AudioManager.instance.NormalClick2();
//        storePanel.SetActive(true);
//        storePanel.GetComponent<NewStoreManager>()._FreeRewardsButton();
//        WeaponStore.Instance.StorePanel.SetActive(true);

//        // Initialize Admob Rewarded , If Not Initialized
//        if (!GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
//        {
//            GoogleMobileAdsManager.Instance.RequestRewarded();
//        }
//    }
//    public void _OpenGoldOrSPButton()
//    {
//#if UNITY_ANDROID
//        if (modeSelectionPanel.activeInHierarchy)
//        {
//            Debug.Log("Debug : Mode Unlock Panel To Store");
//        }
//        else if (storePanel.activeInHierarchy)
//        {
//            Debug.Log("Debug : Loadout Panel To Store");
//        }
//#endif
//        if (gunsCamera.activeInHierarchy)
//        {
//            Camera.main.depth = 2;
//        }
//        goldSpPanel.SetActive(false);
//        //GetComponent<GadgetStore>()._CloseButton();
//        storePanel.SetActive(true);
//        WeaponStore.Instance.StorePanel.SetActive(true);
//        if (WeaponStore.Instance != null && WeaponStore.Instance.fullSpecificationPanel.activeInHierarchy)
//        {
//            WeaponStore.Instance.fullSpecificationPanel.SetActive(false);
//            WeaponStore.Instance.StorePanel.SetActive(true);
//                GameManagerStatic.Instance.FromGunsToStore = 1;
//        }
//        if (loadoutPanel.activeSelf)
//        {
//            loadoutPanel.SetActive(false);
//            isLoadout = true;
//        }
//        else
//        {
//            isRedirect = true;
//        }
        
//        if (_isGold)
//        {
//            if (isGoldRewardedADPopup)
//            {
//                isGoldRewardedADPopup = false;
//                goldRewardedAdPopup.SetActive(true);
//            }
//            WeaponStore.Instance.GetComponent<NewStoreManager>()._GoldButton();
//        }
//        else
//        {
//            if (isSPRewardedADPopup)
//            {
//                isSPRewardedADPopup = false;
//                spRewardedAdPopup.SetActive(true);
//            }
//            WeaponStore.Instance.GetComponent<NewStoreManager>()._SPButton();
//        }
//        gunsCamera.SetActive(true);
//        Camera.main.depth = 2;
//        if ((!modeSelectionPanel.activeInHierarchy) && (WeaponStore.Instance.fullSpecificationPanel.activeInHierarchy))
//        {
//            GameManagerStatic.Instance.FromGunsToStore = 1;
//        }

//        PlayerLevelScore.instance.UpdateScore();


//        // Initialize Admob Rewarded , If Not Initialized
//        if (!GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
//        {
//            GoogleMobileAdsManager.Instance.RequestRewarded();
//        }
//    }

    //public void _OpenGoldRewardedAdPopup()
    //{
    //    goldRewardedAdPopup.SetActive(true);
    //}

    //public void _OpenSPRewardedAdPopup()
    //{
    //    spRewardedAdPopup.SetActive(true);
    //}



    public void _CloseNotEnoughCurrencyButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Closing NotEnoughCurrency");
#endif
        goldSpPanel.SetActive(false);
        Invoke("EnablingGunsCamera", 0.1f);
    }

    void EnablingGunsCamera()
    {
#if UNITY_EDITOR
        print("GunsCamera Turns ON");
#endif
        gunsCamera.SetActive(true);
        if (gunsCamera.activeInHierarchy)
        {
            Camera.main.depth = 2;
        }
    }

    public void EnableNotEnoughCurrencyPanel(bool isGold)
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Opening NotEnoughCurrency");
#endif
        _isGold = isGold;
        if (_isGold)
        {
            purchaseTextGold.SetActive(true);
            purchaseTextSP.SetActive(false);
            currencyImage.sprite = gold;
        }
        else
        {
            _isGold = false;
            purchaseTextGold.SetActive(false);
            purchaseTextSP.SetActive(true);
            currencyImage.sprite = sp;
        }
        goldSpPanel.SetActive(true);
    }
    public void OpenNadesButton()
    {
        isLoadout = true;
        storePanel.SetActive(true);
        loadoutPanel.SetActive(false);
        WeaponStore.Instance.GetComponent<NewStoreManager>()._NadesButton();

    }
    public void OpenAdrenaline()
    {
        isLoadout = true;
        storePanel.SetActive(true);
        loadoutPanel.SetActive(false);
        WeaponStore.Instance.GetComponent<NewStoreManager>()._GadgetsButton();
    }
    public void ShowPremissionPanel()
    {
        if (weaponModeButtonPressed.Equals(1))
        {
            tutorialPermissionPanel.SetActive(true); 
        }
        premissionPanel.SetActive(true);
        // _GrantPermission();
    }
    public void _GrantPermission()
    {
        try
        {
            AudioManager.instance.NormalClick();
            Camera.main.depth = 2;
            _ClosePermissionPanelButton();
            processDelegate();
        }
        catch { }
    }
    public void _ClosePermissionPanelButton()
    {
        AudioManager.instance.NormalClick();
        premissionPanel.SetActive(false);
        Camera.main.depth = 2;
    }
    //    public void _ModeSelectionCampaignButton()
    //    {
    //        //AudioManager.instance.NormalClick();
    //        if (tutorialPanels[0].activeSelf)
    //        {
    //            Analytics.CustomEvent("Campaign_Tut", new Dictionary<string, object>//MainMenuPlay
    //        {
    //            { "level_index" , 1 }
    //        });
    //#if UNITY_EDITOR
    //            Debug.Log("CustomEvent: " + "Campaign_Tut");
    //#endif
    //        }

    //        currentMode = 0;
    //        EnablePanel(modeSelectionPanel);
    //        PlayerPrefs.SetInt("currentMode", 0);
    //    }
    //    public void _ModeSelectionCampaignBack()
    //    {
    //        AudioManager.instance.BackClickNew();
    //        EnablePanel(modeselection);
    //        #region oldwork
    //        //// New Back Button , scenario is to take mode buttons to main menu
    //        //_ModeSelectionBack();
    //        #endregion
    //    }

    //    public void _ModeSelectionMultiButton()
    //    {
    //        //AudioManager.instance.NormalClick();
    //        currentMode = 1;
    //        EnablePanel(modeSelectionPanel);
    //        PlayerPrefs.SetInt("currentMode", 1);
    //    }
    public void _ModeSelectionBack()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : ModeSelection To MenuScreen");
#endif
        AudioManager.instance.PlayMainMenuThemeSound();
        AudioManager.instance.BackClickNew();

        if (!tutorialPanels[0].activeSelf &&
            SaveManager.Instance.state.nameEntryPanelSeen.Equals(0))
        {
            //tutorialNameEntryPanel.SetActive(true);
        }

        EnablePanel(mainMenuPanel);
    }
    public void _LoadOutPanelButton()
    {
        #region oldwork
        //try
        //{
        //    lastSniperCard = null;
        //    lastAssaultCard = null;
        //    AudioManager.instance.LevelSelectSound();
        //    if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.FREEFORALL || LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.BR)
        //    {
        //        GetComponent<MultiPlayerMenu>().OnMultiPlayer();
        //        return;
        //    }

        //    Analytics.CustomEvent("Next_Tut", new Dictionary<string, object>//MainMenuPlay
        //{
        //    { "level_index" , 1 }
        //});
        //    Debug.Log("CustomEvent: " + "Next_Tut");

        //    loadoutPanel.SetActive(true);
        //    modeSelectionPanelCampaign.SetActive(false);
        //    modeSelectionPanelMulti.SetActive(false);
        //}
        //catch { }
        #endregion

       
#if UNITY_ANDROID
            Debug.Log("Debug : LevelSelection to Loadout");
#endif

            storePanel.SetActive(true);
        // modeSelectionPanel.SetActive(false);


        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {

            specificationDelegates[PlayerPrefs.GetInt("SniperEquipped")]();
        }
        else
        {
            specificationDelegates[PlayerPrefs.GetInt("AssaultEquipped")]();
        }
        WeaponStore.Instance.ownedButton.SetActive(false);
            WeaponStore.Instance.playButton.SetActive(true);
            WeaponStore.Instance.equipedgun.SetActive(true);
            GameManagerStatic.Instance.GunsStoreFrom = 1;
            WeaponStore.Instance.CheckUnlockedGuns();

            // Load Admob , If Not Loaded
            #region Loading Admob Interstitial If Not Loaded
            if (GoogleMobileAdsManager.Instance != null && !GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
            {
                LoadAdmobInterstitial();
            }
            #endregion


        if (tutorialPanels[2].activeSelf)
        {
            Analytics.CustomEvent("Next_Tut", new Dictionary<string, object>//LevelSelection Screen
        {
            { "level_index" , 1 }
        });
        }
        // Initialize Admob Rewarded , If Not Initialized
        if (!GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
        {
            GoogleMobileAdsManager.Instance.RequestRewarded();
        }
    }
    public void _LoadOutPanelBack()
    {
        isLoadout = false;
        AudioManager.instance.otherAudioSource.volume = 0;
        AudioManager.instance.BackClickNew();
        loadoutPanel.SetActive(false);
        modeSelectionPanel.SetActive(true);
        //if (currentMode == 0)
        //    modeSelectionPanelCampaign.SetActive(true);
        //else
        //    modeSelectionPanelMulti.SetActive(true);
    }
    public void _CloseStoreButton()
    {
        if (mainMenuPanel.activeInHierarchy)
        {
            //GoogleMobileAdsManager.handleFullScreenAdClose += DelayedAdd;
#if UNITY_ANDROID
            Debug.Log("Debug : StorePanel To Menu");
#endif
        }
        AudioManager.instance.BackClickNew();
        if (WeaponStore.weaponPopupButtonPressed.Equals(1))
        {
            WeaponStore.weaponPopupButtonPressed = 0;
            loadingPanel.SetActive(true);
            SceneManager.LoadScene("GameScene");
        }
        else if (isLoadout)
        {
            storePanel.SetActive(false);
            loadoutPanel.SetActive(true);
            isLoadout = false;
        }
        else if (GameManagerStatic.Instance.FromGunsToStore == 1)
        {
            WeaponStore.Instance.StorePanel.SetActive(false);
            WeaponStore.Instance.fullSpecificationPanel.SetActive(true);
            GameManagerStatic.Instance.FromGunsToStore = 0;
        }
        else
        {
            if (isRedirect && StoreRedirect != null)
            {
                isRedirect = false;
                StoreRedirect();
            }
            else
            {
                storePanel.SetActive(false);
                mainMenuPanel.SetActive(true);
            }
        }
    }

    public void _QuitButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : MenuScene To QuitPanel");
#endif
        AudioManager.instance.NormalClick();
        EnablePanel(quitPanel);
    }

    public void _CloseQuitButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : QuitPanel To MenuScene");
#endif
        AudioManager.instance.BackClickNew();

        if (!tutorialPanels[0].activeSelf &&
            SaveManager.Instance.state.nameEntryPanelSeen.Equals(0))
        {
           // tutorialNameEntryPanel.SetActive(true);
        }

        EnablePanel(mainMenuPanel);
    }

    public void _YesButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Quit Game Completely");
#endif
        GoogleMobileAdsManager.Instance.HideBanner();
        Application.Quit();
    }

    public void _ClosePackPanel()
    {
        AudioManager.instance.BackClickNew();
        if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
        {
            if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
            {
              //  GoogleMobileAdsManager.Instance.ShowInterstitial();
            }
        }
    }

//    public void _AboutUsButton()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Open AboutUsPanel");
//#endif
//        AudioManager.instance.NormalClick();
//        EnablePanel(aboutUsPanel);
//        GoogleMobileAdsManager.Instance.RePosition(GoogleMobileAds.Api.AdPosition.BottomLeft);
//       // GoogleMobileAdsManager.Instance.ShowMedBanner();
//        GoogleMobileAdsManager.Instance.HideBanner();
//    }

//    public void _CloseAboutUsPanel()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Close AboutUsPanel");
//#endif
//        AudioManager.instance.BackClickNew();

//        if (!tutorialPanels[0].activeSelf &&
//            SaveManager.Instance.state.nameEntryPanelSeen.Equals(0))
//        {
//           // tutorialNameEntryPanel.SetActive(true);
//        }
//       // GoogleMobileAdsManager.Instance.RePosition(GoogleMobileAds.Api.AdPosition.Bottom);
//        //GoogleMobileAdsManager.Instance.HideMedBanner();
//       // GoogleMobileAdsManager.Instance.ShowBanner();
//        EnablePanel(settingPanel);
//    }

    //IAP
//    public void PurchaseGoldBundle01()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseGoldBundle01();
//    }

//    public void PurchaseGoldBundle02()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseGoldBundle02();
//    }

//    public void PurchaseGoldBundle03()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseGoldBundle03();
//    }

//    public void PurchaseGoldBundle04()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseGoldBundle04();
//    }

//    public void PurchaseGoldBundle05()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseGoldBundle05();
//    }

//    public void PurchaseGoldBundle06()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseGoldBundle06();
//    }

//    public void PurchaseSPBundle01()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseSPBundle01();
//    }

//    public void PurchaseSPBundle02()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseSPBundle02();
//    }
//    public void PurchaePremiumPass()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchasePremiumPass1();
//    }
//    public void PurchaePremiumPass2()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchasePremiumPass2();
//    }
//    public void PurchaePremiumPass3()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchasePremiumPass3();
//    }
//    public void PurchaseSPBundle03()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseSPBundle03();
//    }

//    public void PurchaseSPBundle04()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseSPBundle04();
//    }

//    public void PurchaseSPBundle05()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseSPBundle05();
//    }

//    public void PurchaseSPBundle06()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseSPBundle06();
//    }
//    public void ResetPiggy()
//    {
//        gunsCamera.SetActive(true);
//        bankedSPText.text = System.String.Empty + PlayerPrefs.GetInt("BankedSP");
//        bankedGoldText.text = System.String.Empty + PlayerPrefs.GetInt("BankedGold");

//        //_InAppManager.SetPiggyPackPrice();
//        //piggyBankPriceText.text = System.String.Empty + _InAppManager.piggyBankPrice;
//        //disPiggyBankPriceText.text = System.String.Empty + _InAppManager.disPiggyBankPrice;
//        //if (disPiggyBankPriceText.text == "")
//        //    disPiggyBankPriceText.text = "Purchase";
//    }
   


//    public void PurchaseProStarterBundle()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseProStarterBundle();
//    }
//    public void PurchaseEssentialBundle()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseEssentialBundle();
//    }
//    public void PurchaseExreaordinaryBundle()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseExtraordinaryBundle();
//    }

//    public void PurchasePremiumBundle()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchasePremiumBundle();
//    }

//    public void PurchaseAdrenaline_H25_1()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseAdrenaline_H25_1();
//    }

//    public void PurchaseAdrenaline_H25_2()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseAdrenaline_H25_2();
//    }

//    public void PurchaseGrenade_G65_1()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseGrenade_G65_1();
//    }

//    public void PurchaseGrenade_G65_2()
//    {
//        AudioManager.instance.StoreButtonClick();
//        //_InAppManager.PurchaseGrenade_G65_2();
//    }
//    public void PurchaseLevelButton()
//    {
//        AudioManager.instance.NormalClick();
//        PurchaseLevelPanel.SetActive(true);
//    }
//    public void PurchaseLevelButtonBack()
//    {
//        AudioManager.instance.BackClickNew();
//        PurchaseLevelPanel.SetActive(false);
//    }
//    public void PurchaseLevelGoldButton()
//    {
//        PurchaseLevelPanel.SetActive(false);
//        storePanel.SetActive(true);
//        storePanel.GetComponent<NewStoreManager>()._GoldButton();
//    }
//    public void PurchaseLevelSPButton()
//    {
//        PurchaseLevelPanel.SetActive(false);
//        storePanel.SetActive(true);
//        storePanel.GetComponent<NewStoreManager>()._SPButton();
//    }
//    public void OpenCredits()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To OpenCredits");
//#endif
//        AudioManager.instance.NormalClick();
//        creditsPanel.SetActive(true);
//        GoogleMobileAdsManager.Instance.HideMedBanner();
//    }
//    public void CloseCredits()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To CloseCredits");
//#endif
//        AudioManager.instance.BackClickNew();
//        creditsPanel.SetActive(false);
//        GoogleMobileAdsManager.Instance.ShowMedBanner();
//    }
//    public void OpenLanuguagePanel()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To LanguagePanel");
//#endif
//        AudioManager.instance.NormalClick();
//        languagePanel.SetActive(true);
//    }
//    public void CLoseLanugagePanel()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To CloseLanguagePanel");
//#endif
//        AudioManager.instance.BackClickNew();
//        languagePanel.SetActive(false);
//    }
//    public void OpenDailyRewards()
//    {
//#if UNITY_ANDROID
//        Debug.Log("Debug : Go To OpenDailyReward");
//#endif
//        dailyRewardsPanel.SetActive(true);
//    }
//    public void ShowWelcome()
//    {
//        if (!PlayerPrefs.GetInt("WelcomeReward").Equals(1) && !PlayerPrefs.GetInt("Start_CutScene").Equals(0)) 
//        {
//#if UNITY_EDITOR
//            print("WelcomeReward appers at this place");
//#endif
//            // welcomeReward.SetActive(true);
//        }
//    }
    public void ShowInAppProcess()
    {
        inAppProcessPanel.SetActive(true);
        gunsCamera.SetActive(false);
    }
    public void HideInAppProcess(int State)
    {
        if(State == 0)
        {
            gunsCamera.SetActive(true);
            inAppProcessPanel.SetActive(false);
           // GoogleMobileAdsManager.Instance.HideBanner();
        }
        else
        {
            inAppProcessPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            if(State == 1)
                inAppProcessPanel.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            else
                inAppProcessPanel.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
            Invoke("DelayedHideInAppProcess",2);
        }
    }
    void DelayedHideInAppProcess()
    {
        inAppProcessPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        inAppProcessPanel.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
        inAppProcessPanel.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
        inAppProcessPanel.SetActive(false);
        gunsCamera.SetActive(true);
    }
    public void StartLoading()
    {
        loadingPanel.SetActive(true);
        GoogleMobileAdsManager.Instance.HideBanner();
    }
    public void OpenMainMenu()
    {
        EnablePanel(mainMenuPanel);

        if (!tutorialPanels[0].activeSelf &&
            SaveManager.Instance.state.nameEntryPanelSeen.Equals(0))
        {
           // tutorialNameEntryPanel.SetActive(true);
        }
    }

    #endregion


   
    private void OnDisable()
    {
        GoogleMobileAdsManager.handleFullScreenAdClose -= DelayedAdd;
        CancelInvoke("DelayedAdd");
        CancelInvoke("EnablingGunsCamera");
        CancelInvoke("DelayedHideInAppProcess");
        CancelInvoke("ShowSessionAd");
    }

    private void OnDestroy()
    {
        CancelInvoke("DelayedAdd");
        CancelInvoke("EnablingGunsCamera");
        CancelInvoke("DelayedHideInAppProcess");
        CancelInvoke("ShowSessionAd");
    }
}