using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
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
        settingPanel,
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


    // Settings
    public Toggle autoShoot, bloodEffect;
    public Slider soundFxSlider, backgroundMusicSlider, controlSenstivitySlider, difficultySlider;


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
    public GameObject rewardedPanel;
    public Text rewardedText;
    public Text goldText;

    private void Start()
    {
        goldText.text = PlayerPrefs.GetInt("gold").ToString();
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
        if (SaveManager.Instance.Session == 1 && PlayerPrefs.GetInt("Appopen") == 1)
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
        AdsManager.instance.RemoveAllBanners();
        Invoke("DelayedAdd", 1.5f);
       

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
        // Audio
        AudioManager.instance.ChangeBackgroundVolume(SaveManager.Instance.state.backgroundVolume);
        AudioManager.instance.ChangeSoundFxVolume(SaveManager.Instance.state.soundFxVolume);
        soundFxSlider.value = SaveManager.Instance.state.soundFxVolume;
        backgroundMusicSlider.value = SaveManager.Instance.state.backgroundVolume;
        controlSenstivitySlider.value = SaveManager.Instance.state.controlSensitivity;
        //difficultySlider.value = PlayerPrefs.GetFloat("Difficulty");

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

        if(PlayerPrefs.GetInt("Appopen") == 0)
        {
            PlayerPrefs.SetInt("Appopen", 1);
            ShowSessionAd();
            if (sessionAdLoading.activeInHierarchy)
            {
                sessionAdLoading.SetActive(false);
            }
        }
        // calling session ad here
        if (SaveManager.Instance.Session == 1 && PlayerPrefs.GetInt("Appopen") == 1)
        {
            Invoke("ShowSessionAd", 3);
        }


#if UNITY_EDITOR
        print("Sniper Mode : " + PlayerPrefs.GetInt("Mode1"));
        print("GameManagerStatic.Instance.GunsStoreFrom : " + GameManagerStatic.Instance.GunsStoreFrom);
#endif

    }

    private void GoToMainMenu() {
        EnablePanel(mainMenuPanel);
    }



    void ShowSessionAd()
    {
        if (GameManagerStatic.Instance.isSessionShown == 0)
        {
            AdsManager.instance.ShowStaticInterstitial();
            GameManagerStatic.Instance.isSessionShown = 1;
            SaveManager.Instance.Session = 0;
            if (sessionAdLoading.activeInHierarchy)
            {
                sessionAdLoading.SetActive(false);
            }
        }
        else
        {
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
        AdsManager.instance.ShowTopSmallBanner();
    }
    private void EnablePanel(GameObject panelToShow)
    {
        mainMenuPanel.SetActive(false);
        quitPanel.SetActive(false);
        //levelSelectionPanel.SetActive(false);
        // storePanel.SetActive(false);
        settingPanel.SetActive(false);
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
        
        EnablePanel(modeselection);


        #region Show Session Ad If Not Shown Yet
        if (SaveManager.Instance.Session == 1)
        {
            if (GameManagerStatic.Instance.isSessionShown == 0)
            {
                AdsManager.instance.ShowStaticInterstitial();
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
         Application.OpenURL("https://play.google.com/store/apps/developer?id=6420928521636203444");
    }

    public void _PrivacyPolicyButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Go To PrivacyPolicyButton");
#endif
        AudioManager.instance.NormalClick2();
        Application.OpenURL("https://trilogixs.com/privacypolicy.html");
    }


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



        if (tutorialPanels[2].activeSelf)
        {
            Analytics.CustomEvent("Next_Tut", new Dictionary<string, object>//LevelSelection Screen
        {
            { "level_index" , 1 }
        });
        }
    }
    public void _LoadOutPanelBack()
    {
        isLoadout = false;
        AudioManager.instance.otherAudioSource.volume = 0;
        AudioManager.instance.BackClickNew();
        loadoutPanel.SetActive(false);
        modeSelectionPanel.SetActive(true);
        
    }
    public void _CloseStoreButton()
    {
        if (mainMenuPanel.activeInHierarchy)
        {
            
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
        AdsManager.instance.ShowBottomLeftCubeBanner();
        AdsManager.instance.ShowBothInterstitial();
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
        AdsManager.instance.ShowTopSmallBanner();
        EnablePanel(mainMenuPanel);
    }

    public void _YesButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Quit Game Completely");
#endif
        AdsManager.instance.RemoveAllBanners();
        Application.Quit();
    }

    public void _ClosePackPanel()
    {
        AudioManager.instance.BackClickNew();
        //if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
        //{
            if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
            {
              //  GoogleMobileAdsManager.Instance.ShowInterstitial();
            }
        //}
    }


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
            AdsManager.instance.RemoveAllBanners();
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
        AdsManager.instance.RemoveAllBanners();
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

    public void _SettingButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Menu Settings Button");
#endif
        AudioManager.instance.NormalClick();
        EnablePanel(settingPanel);
        AdsManager.instance.ShowBottomLeftCubeBanner();
    }

    public void _CloseSettingButton()
    {
        AudioManager.instance.BackClickNew();

        //if (!tutorialPanels[0].activeSelf &&
        //    SaveManager.Instance.state.nameEntryPanelSeen.Equals(0))
        //{
        //    // tutorialNameEntryPanel.SetActive(true);
        //}
        AdsManager.instance.RemoveBottomLeftCubeBanner();
        //AdsManager.instance.ShowTopSmallBanner();
        EnablePanel(mainMenuPanel);
    }

    public void _SoundFxSlider(float value)
    {
        AudioManager.instance.ChangeSoundFxVolume(value);
        SaveManager.Instance.state.soundFxVolume = value;
        SaveManager.Instance.Save();
    }
    public void _DifficultySlider(float value)
    {
        PlayerPrefs.SetFloat("Difficulty", difficultySlider.value);
    }
    public void _BackgroundMusicSlider(float value)
    {
        AudioManager.instance.ChangeBackgroundVolume(value);
        SaveManager.Instance.state.backgroundVolume = value;
        SaveManager.Instance.Save();
    }

    public void _ControlSensitivitySlider(float value)
    {
        SaveManager.Instance.state.controlSensitivity = value;
        SaveManager.Instance.Save();
    }

    public void _AutoShoot(bool value)
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Menu AutoShootToggle");
#endif
        if (AudioManager.instance != null)
            AudioManager.instance.NormalClick();

        if (value)
        {
            SaveManager.Instance.state.autoShoot = 1;
        }
        else
        {
            SaveManager.Instance.state.autoShoot = 0;
        }
        SaveManager.Instance.Save();
    }
    public void _BloodEffectToggle(bool value)
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Menu BloodEffectToggle");
#endif
        if (AudioManager.instance != null)
            AudioManager.instance.NormalClick();

        if (value)
        {
            PlayerPrefs.SetInt("BloodEffect", 1);
        }
        else
        {
            PlayerPrefs.SetInt("BloodEffect", 0);
        }
    }

    public void FreeCoinsVideo()
    {
        AdsManager.instance.ShowAdmobRewardedAdWithName("freecoins");
    }


    #endregion



    private void OnDisable()
    {
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