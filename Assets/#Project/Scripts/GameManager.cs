using DG.Tweening.Core;
using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using TacticalAI;
using UnityEngine;

// Unity Analytics
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using Firebase.Analytics;

public class GameManager : MonoBehaviour
{
    private bool isAllObjectives = true;
    private int successfulObjectivesCount = 0;
    public GameObject[] objectivesGoldStars;
    public GameObject objectivesPerfectStars;
    public Animator completeTauntAnimator;
    public GameObject normalFPS, coverStrikeFPS;
    public GameObject[] PickAbleWeapons;
    public GameObject pickAbleHeal, pickAbleNade;
    public GameObject cursorImage, cursorImage1;

    //AI behavrior delegates
    public delegate void BulletReaction();

    public static BulletReaction firstBulletDelegate, secondBulletDelegate, soldierAnimResetDelegate;
    public Transform playerSpawnPoint;
    public static bool isFirstNade = true, isFirstFire = true, isFirstGang = true;
    public int levelUp = 0;

    [HideInInspector]
    public bool allowCoverSound = true;

    public static int totalFiringAgents = 0;
    private const float GAMEPLAYCOLORLIGHTDARK = 0.28f, ENEMYCOLORLIGHTDARK = 0.48f, GUNSCOLORLIGHTDARK = 0.17f, GAMEPLAYCOLORDAY = 1f, ENEMYCOLORDAY = 1f, GUNSCOLORDAY = 0.6f;
    private int lightIndex = 0;
    private int flashesCount = 0;

    //public Material[] gunsMats,enemyMats;
    public Transform rainTextures;

    public RenderTexture gunsRenderTex;
    public static GameManager Instance;
    public GameObject[] rainOBJs;
    public Material[] GamePlayMats, GamePlayPropsMats;
    public FPSPlayer fpsPlayer, fpsPlayerCS;
    public SmoothMouseLook fpsCamera, fpsCameraCS;
    public PlayerWeapons playerweapons;
    public Camera mainCamera;
    public GameObject[] SniperLevelObjects;
    public GameObject[] FailedReason;
    public GameObject LightsObject, ThunderSounds;

    [Space(20)]
    // Panels
    public GameObject
        fpsUICanvasPanel,
        fpsUICanvasPanel1,
        settingPanel,
        levelPausePanel,
        levelFailPanel,
        levelCompletePanel,
        levelCompletePanelSingle,
        levelUpPanel,
        //rateUsPanel,
        //removeAdsPanel,
        rewardPanel,
        loadingPanel,
        passesPanel,
        controlsPanel,
        gamePlayPanel,
        languagesPanel,
        bulletTutorialPanel,
        adrenalineTutorialPanel,
        inAppProcessPanel,
        gunsRewardsPanel,
        sniperLevelsCompletedPanel,
        assaultLevelsCompletedPanel,
        coverStrikeLevelsCompletedPanel,
        nextLevelPanel,
        runningFireAnim,
        crateScene,
        barrelCutSceneObject,
        grenadeCutSceneObject,
        ObjectivesPanel,
        revivePanel,
        countdownPanel,
        autoShoot_Assault,
        autoShoot_CoverStrike;

    //public Animator animatedHeadShot;

    public GameObject
      
        scopeImage_1,
        scopeImage_2,
        sniper_1,
        sniper_2,
        sniper_3,
        sniper_4,
        sniper_5,
        sniper_6,
        leftFireButton,
        rightFireButton,
        leftFireButton1,
        rightFireButton1,
        sniperRightFireButton,
        outOfBulletsImage,
        alertTextNade,
        alertTextAdrenaline,
        sniperProgressText,
        assaultProgressText,
        coverStrikeProgressText,
        adsAvailibleText,
        failedButtons,
        
        completeButotns,
        pauseButtons;

    public GameObject[] labelInfos;

    // Gameplay
    public AudioClip
        gameplay_1,
        sniperBackground,
        headshotClip,
        gameOver,
        getReady,
        medicKit,
        reload,
        gameplay_2,
        completeStatsPanelSound;

    public Text
        remainingEnemyText,
        remainingTimeText,
        remainingGrenadeText,
        remainingMedicKitText,
        remainingGrenadeText1,
        remainingMedicKitText1,
        playerHealthText,
        currentLevelText,
        bulletsCount; //When you don't have grenade/adrenaline

    public Image
        playerHealthFillImage,
        crossHair;

    public GameObject secretRewardPanel;
    public GameObject[] secretRewards = new GameObject[3];

    //Settings
    public Toggle autoShoot, bloodEffect;

    public Slider
        soundFxSlider,
        backgroundMusicSlider,
        controlSenstivitySlider,
        sprintSpeedSlider;


 

    public Button  doubleRewardButton;
    public GameObject doubleRewardNotAvailable;
    [HideInInspector] public int totalEnemies = 0;
    [HideInInspector] public int enemyKilled;
    [HideInInspector] public int headShot;
    [HideInInspector] public int totalTime;
    [HideInInspector] public int levelIndex;
    [HideInInspector] public int sniperLevelIndex;

    [HideInInspector] public bool isLevelFailed = false;
    [HideInInspector] public bool isLevelCompleted = false;

    public bl_HudManager _bl_HudManager;
    [HideInInspector] public int sp = 0;
    [HideInInspector] public int gold = 0;
    private float startPlayTime;
    private int killMultiplier = 55, headShotMultiplier = 28, levelMultiplier = 150, timeMultiplier = 50;

    // Level Fail
    public Text levelFailReasonText;

    public Button freeRetryVideoButton;

    //Weapon Popups for Purchase
    public static bool purchaseButtonPresed = false;

    public static int currentPopupIndex = 0;
    public GameObject popup;
    public Image popupImage;
    public List<int> lockedWeaponIndexes;
    public Sprite[] popupSprites;

    // Video Reward
    public GameObject videoRewardPanel;

    public Text rewardText;

    //Level Complete Reward
    public GameObject[] rewards = new GameObject[6];

    // Level Up
    //private int CurrentExp;
    //private int LastLevel = 0;
    //private bool levelup = false;
    //float tempTime = 0;
    private float /*TargetKills, TargetTime, TargetHead, TargetGold ,TargetObjectives,*/ CurrentTime, tempPass;

    public float CurrentKills, CurrentHead, CurrentObjectives, currentProgress;
    public string levelRatio = "", levelRatioCurrentLevel = "";
    private float ExperienceVal = 0;
    private float LerpAmount = 0;
    private float tempTime = 0;
    private bool DoScore = false;

    //Controls
    public GameObject sniperControls, assaultControls, coverStrikeControls;

    // Passes
    [HideInInspector] public bool isPassPurchased = false;

    public Text[] passPricesText;

    //Levels
    public GameObject[] assaultLevels;

    public GameObject[] sniperLevels;
    public GameObject[] coverStrikeLevels;

    private int[] objectivesCompleted = new int[3];

    //Tutorial for Sniper
    public GameObject[] zoomObjects, fireObjects, assaultWeaponSwitchObjects, sniperWeaponSwitchObjects, coverWeaponSwitchObjects;

    public GameObject assaultWeaponSwitchButton, sniperWeaponSwitchButton, coverStrikeWeaponSwitchButton;
    public GameObject zoomButton;
    private int Liked = 0;
    public Image LikedBTN, DisLikedBTN, LikedBTNPause, DisLikedBTNPause, LikedBTNFailed, DisLikedBTNFailed;
    private bool isRain = false;

    //Gun Rewards
    private int currentGunReward = -10;

    public GameObject GunRewardedAd, GunsRewardWinText, GunsRewardLoseText;
    private bool notAdseen = false;

    //Heli Strike
    public GameObject _heliObject, weaponsCamera;

    public int levelIndexAnalytics = 0;

    public Canvas mainCavas;
    public GameObject playerModel, playerModelLastBullet, bulletsVideo, miniMap;

    //public GameObject[] lastBullletGuns;
    public SmoothMouseLook mouseLookScript;

    public bool isForcedFailed = false;

    //Objectives
    public AssaultLevels[] assaultLevelObjectives, sniperLevelObjectives, coverStrikeLevelObjectives;

    public AssaultLevels activeObjectives;
    public GameObject[] ObjectivesContainer;
    public Text[] ObjectivesTexts;
    public Transform[] ObjectivesStatToggles;
    public int[] objectiveStats;

    public GameObject[] sniperObjectivesInfo, objectivesInfo, coverStrikeObjectivesInfo;
    public Text[] objectivesInfoValue;

    //ABDUL
    private bool isAllLevelCompleted, isModePopupShown = false;

    private int count = 0;

    public Weapon[] _weapons;
    public float damageFactor = 0;
    private BulletDamage bulletDamage;

    public RectTransform fireButtonRect;

 

    // Gameplay Banner Background
    public GameObject GameplayBannerBG;

    public Text doubleSPText, doubleGoldText, doubleSPPanelText, doubleGoldPanelText;
    public GameObject doubleRewardPanel;

    // FPS Counting Data

    private void OnEnable()
    {
        GoogleMobileAdsManager.handleFullScreenAdClose += DelayedAddsShowBanner;

        try
        {
            GoogleMobileAdsManager.Instance.RePosition(GoogleMobileAds.Api.AdPosition.BottomLeft);
            fpsUICanvasPanel.SetActive(false);
            fpsUICanvasPanel1.SetActive(false);
            objectiveStats = new int[(int)AssaultLevels.LevelConidtions.totalCount];
            soldierAnimResetDelegate = firstBulletDelegate = secondBulletDelegate = null;
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
            allowCoverSound = isFirstNade = isFirstFire = isFirstGang = true;
            totalFiringAgents = 0;
            //Domes[0].SetActive(true);
            //Domes[1].SetActive(true);
            switch (LevelSelectionNew.modeSelection)
            {
                case LevelSelectionNew.modeType.SNIPER:
                     //AudioManager.instance.PlaySniperBGM();

                    //if (PlayerPrefs.GetInt("sniperCurrentLevel") <= 3)
                    //    doubleRewardButton.gameObject.SetActive(false);

                    normalFPS.SetActive(true);
                    sniperControls.SetActive(true);
                    assaultControls.SetActive(false);
                    sniperLevelIndex = PlayerPrefs.GetInt("sniperCurrentLevel");
                    currentLevelText.text = "Mission: " + sniperLevelIndex;
                    activeObjectives = sniperLevelObjectives[PlayerPrefs.GetInt("sniperCurrentLevel") - 1];
                    if (sniperLevelIndex.Equals(1) && !PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1))
                    {
                        sniperRightFireButton.SetActive(false);
                        foreach (GameObject go in fireObjects)
                        {
                            go.SetActive(false);
                        }
                        foreach (GameObject go in zoomObjects)
                        {
                            go.SetActive(true);
                        }
                    }
#if UNITY_ANDROID
                    Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " With Level : " + sniperLevelIndex + "CurrentLevel : " + PlayerPrefs.GetInt("sniperCurrentLevel"));
#endif
                    break;

                case LevelSelectionNew.modeType.ASSAULT:

                    
                    //if(PlayerPrefs.GetInt("Start_CutScene") == 0)
                    //{
                    //    Invoke("EnablePlayerAfterCutScene", 1);
                    //}
                    //else
                    //{
                    //    normalFPS.SetActive(true);
                    //    sniperControls.SetActive(false);
                    //    assaultControls.SetActive(true);
                    //}

                    normalFPS.SetActive(true);
                    sniperControls.SetActive(false);
                    assaultControls.SetActive(true);

                    //if (PlayerPrefs.GetInt("currentLevel") <= 3)
                    //    doubleRewardButton.gameObject.SetActive(false);

                    levelIndexAnalytics = PlayerPrefs.GetInt("currentLevel");

                    if (PlayerPrefs.GetInt("currentLevel") > 30)
                    {
                        levelIndex = PlayerPrefs.GetInt("LevelIndex");
#if UNITY_EDITOR
                        print("levelIndex : " + levelIndex);
#endif
                    }
                    else
                    {
                        levelIndex = PlayerPrefs.GetInt("currentLevel");
                    }

                    currentLevelText.text = "Mission: " + levelIndexAnalytics;
                    activeObjectives = assaultLevelObjectives[levelIndex - 1];
                    // Additional Autoshoot Toggle Setting
                    if (SaveManager.Instance.state.autoShoot == 1)
                    {
                        autoShoot_Assault.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        autoShoot_Assault.transform.GetChild(0).GetComponent<Toggle>().isOn = false;
                    }

                    // Camera Shaker Turns ON , (For Camera Recoiling On Fire In Sniper Mode)
                    Camera.main.GetComponent<CameraShaker>().enabled = true;
                    // AudioManager.instance.StopBRBGM();
#if UNITY_ANDROID
                    Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " With Level : " + levelIndexAnalytics);
#endif
                    break;

                case LevelSelectionNew.modeType.COVERSTRIKE:
                    // AudioManager.instance.PlayCoverStrikeBGM();

                    //if (PlayerPrefs.GetInt("CoverStrikeCurrentLevel") <= 3)
                    //    doubleRewardButton.gameObject.SetActive(false);

                    fpsPlayer = fpsPlayerCS;
                    coverStrikeFPS.SetActive(true);
                    sniperControls.SetActive(false);
                    assaultControls.SetActive(false);
                    coverStrikeControls.SetActive(true);
                    sniperLevelIndex = PlayerPrefs.GetInt("CoverStrikeCurrentLevel");
                    currentLevelText.text = "Mission: " + sniperLevelIndex;
                    activeObjectives = coverStrikeLevelObjectives[sniperLevelIndex - 1];
                    // Additional Autoshoot Toggle Setting
                    if (SaveManager.Instance.state.autoShoot == 1)
                    {
                        autoShoot_CoverStrike.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        autoShoot_CoverStrike.transform.GetChild(0).GetComponent<Toggle>().isOn = false;
                    }
#if UNITY_ANDROID
                    Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " With Level : " + sniperLevelIndex);
#endif
                    break;
            }
            EnableCurrentLevel();
            startPlayTime = Time.time;
            totalEnemies = activeObjectives.minKills;
            PlayerPrefs.SetInt("CanThrowGrenade", 0);

            if (PlayerPrefs.HasKey("HPStats"))
                bulletDamage = JsonUtility.FromJson<BulletDamage>(PlayerPrefs.GetString("HPStats"));
            else
                bulletDamage = new BulletDamage();

            damageFactor = bulletDamage.GetAvg();//Average
#if UNITY_EDITOR
            print("Average: " + damageFactor);
#endif
            if (bulletDamage.isFailed)
            {
                bulletDamage.isFailed = false;
                damageFactor = 350 - PlayerPrefs.GetFloat("Difficulty");
            }
            else
            {
                float baseValue = Mathf.Ceil(damageFactor / 10);
#if UNITY_EDITOR
                print("baseValue: " + baseValue);
#endif
                float baseCase = baseValue - 5f;// Position
#if UNITY_EDITOR
                print("baseCase: " + baseCase);
#endif
                if (baseCase > 0)//Divide
                {
                    damageFactor = (40 / (1.25f + baseCase * 0.25f)) - PlayerPrefs.GetFloat("Difficulty");
#if UNITY_EDITOR
                    print("damageFactor(Divide): " + damageFactor);
#endif
                }
                else//Multiply
                {
                    damageFactor = (40 * (1.25f + -baseCase * 0.25f)) - PlayerPrefs.GetFloat("Difficulty");
#if UNITY_EDITOR
                    print("damageFactor(Multiply): " + damageFactor);
#endif
                }
            }
            damageFactor = ((PlayerPrefs.GetInt("LevelCount") / (damageFactor)));
#if UNITY_EDITOR
            print("Final Damage: " + damageFactor);
#endif
            // When a new user plays 1st level of assault mode after watching cutscene
            PlayerPrefs.SetInt("Start_CutScene", 1);

#if UNITY_EDITOR
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            {
                print("Sniper Level : " + PlayerPrefs.GetInt("sniperCurrentLevel"));
            }
#endif
        }
        catch { }
    }

   
    private void EnablePlayerAfterCutScene()
    {
        normalFPS.SetActive(true);
        sniperControls.SetActive(false);
        assaultControls.SetActive(true);
    }

    private void ResetGunTextures()
    {
        //for (int i = 0; i < gunsMats.Length; i++)
        //{
        //    gunsMats[i].SetTexture("_MainTex",rainTextures.GetChild(i).GetComponent<RawImage>().texture);
        //}
    }

    private void DayWeather()
    {
        //Color c;
        //c = new Color(0.65f, 0.65f, 0.65f, 1);
        //for (int i = 0; i < gunsMats.Length; i++)
        //    gunsMats[i].SetColor("_Color", c);
        //c = new Color(1, 1, 1, 1);
        //for (int i = 0; i < enemyMats.Length; i++)
        //    enemyMats[i].SetColor("_Color", c);
    }

    private void SetWeather(int Level)
    {
        //Set default
        ResetGunTextures();
        //Shader d;
        switch (((Level - 1) % 10))
        {
            case 0://Day
            case 1:// Day

                DayWeather();
                break;

            case 2:// Clouds + light dark
                HideFlash();
                RenderSettings.fogColor = new Color(0.36f, 0.36f, 0.36f, 1);
                break;

            case 3:// Day
            case 4:// Day
                DayWeather();
                break;

            case 5:// Clouds Light dark + lighting
                HideFlash();
                RenderSettings.fogColor = new Color(0.36f, 0.36f, 0.36f, 1);
                StartCoroutine(LightingControlRoutine());
                break;

            case 6:// Day
                DayWeather();
                break;

            case 7:// Clouds Light drk
                HideFlash();
                RenderSettings.fogColor = new Color(0.36f, 0.36f, 0.36f, 1);
                break;

            case 8:// Clouds Light dark + lighting
                HideFlash();
                RenderSettings.fogColor = new Color(0.36f, 0.36f, 0.36f, 1);
                StartCoroutine(LightingControlRoutine());
                break;

            case 9:// Day rain
                isRain = true;
                for (int i = 0; i < rainOBJs.Length; i++)
                {
                    rainOBJs[i].SetActive(true);
                }
                DayWeather();
                //StartCoroutine(LightingControlRoutine());
                break;
        }
    }

    private void Start()
    {
        try
        {
            GoogleMobileAdsManager.Instance.HideMedBanner();
            GoogleMobileAdsManager.Instance.RequestInterstitial();
            // StartCoroutine(DelayedAddsShowBannerExtra(1));
            GoogleMobileAdsManager.Instance.HideBanner();
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.0025f;
            WeaponBehavior.bulletCount = 0;

            successfulObjectivesCount = 0;
            headShot = 0;

            // Getting Player Position and Rotation
            fpsPlayer.GetComponent<FPSRigidBodyWalker>().startingPos = GameObject.FindWithTag("PlayerSpawnPoint").transform.position;
            fpsPlayer.transform.position = GameObject.FindWithTag("PlayerSpawnPoint").transform.position;

            fpsCamera.rotationX = GameObject.FindWithTag("PlayerSpawnPoint").transform.eulerAngles.y;
            fpsCamera.rotationY = GameObject.FindWithTag("PlayerSpawnPoint").transform.eulerAngles.x;
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            {
                fpsCamera.minimumX = fpsCamera.transform.eulerAngles.x - 50;
                fpsCamera.maximumX = fpsCamera.transform.eulerAngles.x + 50;
                fpsCamera.minimumY = fpsCamera.transform.eulerAngles.y - 85f;
                fpsCamera.maximumY = fpsCamera.transform.eulerAngles.y + 85f;
            }

            //if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            //{
            //    switch (sniperLevelIndex)
            //    {
            //        case 1:
            //            //fpsCamera.minimumX = -81.731f;
            //            //fpsCamera.maximumX = -81.731f;
            //            //fpsCamera.minimumY = -5.923f;
            //            //fpsCamera.maximumY = -5.923f;
            //            //for (int i = 0; i < SniperLevelObjects.Length; i++)
            //            //    SniperLevelObjects[i].SetActive(false);
            //            fpsCamera.rotationX = -81.71725f;
            //            fpsCamera.rotationY = -5.961714f;
            //            //StartCoroutine(LockMouse());
            //            break;
            //        case 2:
            //            fpsCamera.rotationX = -41.88387f;
            //            fpsCamera.rotationY = -4.822535f;
            //            break;
            //        case 3:
            //            fpsCamera.rotationX = -52.402f;
            //            fpsCamera.rotationY = 3.872973f;
            //            break;
            //        case 4:
            //            fpsCamera.rotationX = 147.7678f;
            //            fpsCamera.rotationY = -8.593589f;
            //            break;
            //        case 5:
            //            fpsCamera.rotationX = -112.13f;
            //            fpsCamera.rotationY = 1.859801f;
            //            break;
            //        case 6:
            //            fpsCamera.rotationX = -124.1798f;
            //            fpsCamera.rotationY = 0.9686397f;
            //            break;
            //        case 7:
            //            fpsCamera.rotationX = -176.7751f;
            //            fpsCamera.rotationY = -11.75029f;
            //            break;
            //        case 8:
            //            fpsCamera.rotationX = 90.35828f;
            //            fpsCamera.rotationY = -7.176598f;
            //            break;
            //        case 9:
            //            fpsCamera.rotationX = -112.3715f;
            //            fpsCamera.rotationY = -2.528543f;
            //            break;
            //        case 10:
            //            fpsCamera.rotationX = 137.5081f;
            //            fpsCamera.rotationY = -11.78747f;
            //            break;
            //    }
            //}

            //go = GameObject.FindWithTag("EnemySpawnPoint");
            //Vector3 relativePos = GameObject.FindWithTag("EnemySpawnPoint").transform.position - fpsCamera.transform.position;

            // the second argument, upwards, defaults to Vector3.up
            //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            //fpsCamera.rotationX = rotation.eulerAngles.y;
            //fpsCamera.rotationY = rotation.eulerAngles.x;
            if (PlayerPrefs.GetInt("BloodEffect") == 1)
            {
                bloodEffect.isOn = true;
            }
            else
            {
                bloodEffect.isOn = false;
            }
            AudioManager.instance.ChangeBackgroundVolume(SaveManager.Instance.state.backgroundVolume);
            AudioManager.instance.ChangeSoundFxVolume(SaveManager.Instance.state.soundFxVolume);
            soundFxSlider.value = SaveManager.Instance.state.soundFxVolume;
            backgroundMusicSlider.value = SaveManager.Instance.state.backgroundVolume;
            sprintSpeedSlider.value = SaveManager.Instance.state.sprintSpeed;

           // fpsCamera.sensitivity = fpsCameraCS.sensitivity = SaveManager.Instance.state.controlSensitivity;
            controlSenstivitySlider.value = SaveManager.Instance.state.controlSensitivity;
            fpsPlayer.GetComponent<FPSRigidBodyWalker>().sprintSpeed = SaveManager.Instance.state.sprintSpeed;

            //AutoShoot
            ToggleFireButtons();

            UpdateText();
            //InitializeTime();
            // Unity Analytics

            //PlayerPrefs.SetInt("MissionStart_Session", PlayerPrefs.GetInt("MissionStart_Session") + 1);
            //Analytics.CustomEvent("MissionStart_Session", new Dictionary<string, object>
            //{
            //{ "V" + Application.version, PlayerPrefs.GetInt("MissionStart_Session") }
            //});

            //PlayerPrefs.SetInt("MissionStart", PlayerPrefs.GetInt("MissionStart") + 1);
            //Debug.Log("MissionStart " + PlayerPrefs.GetInt("MissionStart"));
            //Analytics.CustomEvent("MissionStart", new Dictionary<string, object>
            //{
            //{ "V" + Application.version, PlayerPrefs.GetInt("MissionStart") }
            //});
            //Debug.Log("GameStart " + 1);
            Analytics.CustomEvent("GameStart", new Dictionary<string, object>
        {
        { "level_index", 1 }
        });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "GameStart");
#endif
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            {
                //Debug.Log("LevelStartSniper " + sniperLevelIndex);
                // new events add for experiment
                Analytics.CustomEvent("LevelStartSniperMode", new Dictionary<string, object>
            {
            { "level_index" , sniperLevelIndex }
            });

                // Firebase Event
                //FirebaseAnalytics.LogEvent("LevelStartSniperMode",
                //new Parameter("level_index", sniperLevelIndex));
#if UNITY_EDITOR
                Debug.Log("CustomEvent: " + "LevelStartSniperMode");
#endif
            }
            else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
            {
                //Debug.Log("LevelStartAssault " + levelIndexAnalytics);
                // new events add for experiment
                Analytics.CustomEvent("LevelStartAssaultMode", new Dictionary<string, object>
            {
            { "level_index" , levelIndexAnalytics }
            });
#if UNITY_EDITOR
                Debug.Log("CustomEvent: " + "LevelStartAssaultMode");
                Debug.Log("LevelStartAssaultMode level_index " + levelIndexAnalytics);
#endif
            }
            else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.COVERSTRIKE)
            {
                //Debug.Log("LevelStartAssault " + levelIndexAnalytics);
                // new events add for experiment
                Analytics.CustomEvent("LevelStartCSMode", new Dictionary<string, object>
            {
            { "level_index" , sniperLevelIndex }
            });
#if UNITY_EDITOR
                Debug.Log("CustomEvent: " + "LevelStartCSMode");
#endif
            }
            AudioManager.instance.backAudioSource.PlayOneShot(getReady);

            //Quality Settings
            if (SystemInfo.systemMemorySize > 3500)
            {
                QualitySettings.SetQualityLevel(2);
            }
            else if (SystemInfo.systemMemorySize > 2000)
            {
                QualitySettings.SetQualityLevel(1);
            }
            else
                QualitySettings.SetQualityLevel(0);

            float width1 = Screen.width * Screen.width;
            float height1 = Screen.height * Screen.height;
            float ypotinousa = width1 + height1;
            ypotinousa = Mathf.Sqrt(ypotinousa);
            float diagonalInches = ypotinousa / Screen.dpi;
            //inch = inch / Screen.dpi;

            double width, height;
            //Quality Settings
            if (SystemInfo.systemMemorySize > 3500)
            {
                width = Screen.width;
                height = Screen.height;
                Screen.SetResolution((int)width, (int)height, true, 144);
            }
            else if (SystemInfo.systemMemorySize > 2000)
            {
                if (diagonalInches < 7)
                {
                    width = Screen.width * 0.9;
                    height = Screen.height * 0.9;
                    Screen.SetResolution((int)width, (int)height, true, 60);
                }
                else
                {
                    width = Screen.width * 0.9;
                    height = Screen.height * 0.9;
                    Screen.SetResolution((int)width, (int)height, true, 60);
                }
            }
            else
            {
                if (diagonalInches < 7)
                {
                    width = Screen.width * 0.8;
                    height = Screen.height * 0.8;
                    Screen.SetResolution((int)width, (int)height, true, 60);
                }
                else
                {
                    width = Screen.width * 0.9;
                    height = Screen.height * 0.9;
                    Screen.SetResolution((int)width, (int)height, true, 60);
                }
            }

            // Enabling Heli(Chopper) For Heli Levels
            switch (levelIndex)
            {
                case 10:
                case 16:
                case 19:
                case 21:
                case 25:
                case 30:
                    if (_heliObject != null)
                        _heliObject.SetActive(true);
                    break;
            }

            // Enable Game FPS After Some Time
            Invoke("EnablingFPS", 5);

            // Target FPS / Lock FPS
            Application.targetFrameRate = 30;

            // Setting Camera Dome Issue (Adnan)
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
            {
                if (!PlayerPrefs.GetInt("levelUnlocked-5").Equals(1))
                {
                    Camera.main.enabled = true;
                    Camera.main.farClipPlane = 250;
                }
                else
                {
                    Camera.main.enabled = true;
                    Camera.main.farClipPlane = 250;
                }
            }
            else
            {
                Camera.main.enabled = true;
                Camera.main.farClipPlane = 250;
            }

            // Disable Weapon Camera On Start For Sniper Mode Levels
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            {
                weaponsCamera.GetComponent<Camera>().enabled = false;
            }
            if(PlayerPrefs.GetInt("sniperCurrentLevel") == 25)
            {
                GameManagerStatic.Instance.lastlevel = 1;
            }
            else
            {
                GameManagerStatic.Instance.lastlevel = 0;
            }

            GameManagerStatic.Instance.isGamePlaySceneEnabled = 1; // after this , user goes to main menu , then unit ads will be initialized

            Invoke("ShowBanner", 0.5f);
        }
        catch { }

    }

    // Banner Show In Gameplay
    void ShowBanner()
    {
        GameplayBannerBG.SetActive(true);
        GoogleMobileAdsManager.Instance.ShowBanner();
    }
    

    // Enabling Game FPS Script
    private void EnablingFPS()
    {
        GetComponent<FPSCounter>().enabled = true;
    }

    public void ShowObjectives()
    {
        Time.timeScale = 0;
        switch (LevelSelectionNew.modeSelection)
        {
            case LevelSelectionNew.modeType.SNIPER:
                sniperObjectivesInfo[PlayerPrefs.GetInt("sniperCurrentLevel") - 1].SetActive(true);
                //objectivesInfo[(int)AssaultLevels.LevelConidtions.isHeadShot].SetActive(true);
                //objectivesInfoValue[(int)AssaultLevels.LevelConidtions.isHeadShot].text = "1";
                //objectivesInfo[(int)AssaultLevels.LevelConidtions.isTime].SetActive(true);
                //objectivesInfoValue[(int)AssaultLevels.LevelConidtions.isTime].text = "30";
                weaponsCamera.GetComponent<Camera>().enabled = true;
                break;

            case LevelSelectionNew.modeType.ASSAULT:
                for (int i = 0; i < activeObjectives.Conditions.Length; i++)
                {
                    objectivesInfo[(int)activeObjectives.Conditions[i].condition].SetActive(true);
                    objectivesInfoValue[(int)activeObjectives.Conditions[i].condition].text = "" + (int)activeObjectives.Conditions[i].value;
                }
                break;

            case LevelSelectionNew.modeType.COVERSTRIKE:
                coverStrikeObjectivesInfo[PlayerPrefs.GetInt("CoverStrikeCurrentLevel") - 1].SetActive(true);
                for (int i = 1; i < activeObjectives.Conditions.Length; i++)
                {
                    objectivesInfo[(int)activeObjectives.Conditions[i].condition].SetActive(true);
                    objectivesInfoValue[(int)activeObjectives.Conditions[i].condition].text = "" + (int)activeObjectives.Conditions[i].value;
                }
                break;
        }
        gamePlayPanel.SetActive(false);
        if (LevelSelectionNew.modeSelection != LevelSelectionNew.modeType.ASSAULT)
        {
            ObjectivesPanel.SetActive(true);
        }
    }

    public void HideObjectives()
    {
        Time.timeScale = 1;
        ObjectivesPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
        cursorImage.SetActive(true);
        cursorImage1.SetActive(true);
        if (soldierAnimResetDelegate != null)
            soldierAnimResetDelegate();

        if (AudioManager.instance)
            AudioManager.instance.ObjectiveOKSound();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            _PauseButton();
        }
    }
#endif

    private void FixedUpdate()
    {
        if (!isLevelFailed && !isLevelCompleted)
            CheckLevelFail();
    }

    private void LateUpdate()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            if (sniperLevelIndex.Equals(1) && !PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1))
            {
                if (fpsPlayer.InputComponent.zoomPress)
                {
                    foreach (GameObject go in zoomObjects)
                    {
                        go.SetActive(false);
                    }
                    foreach (GameObject go in fireObjects)
                    {
                        go.SetActive(true);
                    }
                }
                //else if (fpsPlayer.InputComponent.firePress)
                //{
                //    foreach (GameObject go in fireObjects)
                //    {
                //        go.SetActive(false);
                //    }
                //}
            }
        }
    }

    public void ToggleFireButtons()
    {
        if (SaveManager.Instance.state.autoShoot.Equals(1))
        {
            WeaponBehavior.isAutoFire = true;
            autoShoot.isOn = true;
            if (PlayerPrefs.GetInt("selectedWeaponIndex") < 20)// Assault
            {
                leftFireButton.SetActive(false);
                rightFireButton.SetActive(false);
                leftFireButton1.SetActive(false);
                rightFireButton1.SetActive(false);
            }
            else
            {
                leftFireButton.SetActive(true);
                rightFireButton.SetActive(true);
                leftFireButton1.SetActive(true);
                rightFireButton1.SetActive(true);
            }

            zoomButton.SetActive(true);
        }
        else
        {
            WeaponBehavior.isAutoFire = false;
            autoShoot.isOn = false;
            leftFireButton.SetActive(true);
            rightFireButton.SetActive(true);
            leftFireButton1.SetActive(true);
            rightFireButton1.SetActive(true);

            zoomButton.SetActive(false);
        }
    }
    void EnableHudIcons()
    {
        _bl_HudManager.enabled = true;
    }

    private void DisableHudIcons()
    {
        _bl_HudManager.enabled = false;
    }

    private void EnableCurrentLevel()
    {
        switch (LevelSelectionNew.modeSelection)
        {
            case LevelSelectionNew.modeType.SNIPER:
                sniperLevels[sniperLevelIndex - 1].SetActive(true);
                break;

            case LevelSelectionNew.modeType.ASSAULT:
                assaultLevels[levelIndex - 1].SetActive(true);
                break;

            case LevelSelectionNew.modeType.COVERSTRIKE:
                coverStrikeLevels[sniperLevelIndex - 1].SetActive(true);
                break;
        }
    }

    private void SetTotalEnemies()
    {
        switch (sniperLevelIndex)
        {
            case 1:
                totalEnemies = 1;
                break;

            case 2:
                totalEnemies = 1;
                break;

            case 3:
                totalEnemies = 1;
                break;

            case 4:
                totalEnemies = 2;
                break;

            case 5:
                totalEnemies = 1;
                break;

            case 6:
                totalEnemies = 1;
                break;

            case 7:
                totalEnemies = 1;
                break;

            case 8:
                totalEnemies = 2;
                break;

            case 9:
                totalEnemies = 4;
                break;

            case 10:
                totalEnemies = 3;
                break;
        }
    }

    public void DisableLevelCompleteButtons()
    {
        //homeButton.SetActive(false);
        //restartButton.SetActive(false);
        nextButton.SetActive(false);
        homebutton_complete.SetActive(false);
        //nextButton1.SetActive(false);
    }

    private bool CheckGunsReward()
    {
        //if(fpsPlayer.hitPoints <= 15)
        //{
        //    if(!PlayerPrefs.HasKey("CompleteGunReward"))
        //    {
        //        if (PlayerPrefs.GetInt("weapon17") == 1)
        //            return false;
        //        gunsRewardsPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        //        currentGunReward = 0;
        //        PlayerPrefs.SetInt("CompleteGunReward", 1);
        //        GunsRewardLoseText.SetActive(true);

        //        PlayerPrefs.SetInt("weapon17", 1);
        //        PlayerPrefs.SetInt("AssaultEquipped",17);
        //        return true;
        //    }
        //}else
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT && PlayerPrefs.GetInt("currentLevel") == 5)
        {
            if (PlayerPrefs.GetInt("weapon12") == 1)
                return false;
            gunsRewardsPanel.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            currentGunReward = 3;
            GunsRewardWinText.SetActive(true);

            PlayerPrefs.SetInt("weapon12", 1);
            PlayerPrefs.SetInt("AssaultEquipped", 12);
            return true;
        }
        //else if (fpsPlayer.hitPoints <= 15)
        //{
        //    if (PlayerPrefs.GetInt("CompleteGunReward") == 2)
        //    {
        //        if (PlayerPrefs.GetInt("weapon7") == 1)
        //            return false;
        //        gunsRewardsPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        //        currentGunReward = 2;
        //        PlayerPrefs.SetInt("CompleteGunReward", 2);
        //        GunsRewardLoseText.SetActive(true);
        //        PlayerPrefs.SetInt("PendingGun", 1);
        //        if (!UnityAdsManager.Instance.IsRewardedVideoReady())
        //            GunRewardedAd.GetComponent<Button>().interactable = false;
        //        GunRewardedAd.SetActive(true);
        //        return true;
        //    }
        //}
        else if (PlayerPrefs.GetInt("LastLevel") == 3 && !PlayerPrefs.HasKey("LevelBasedRewardIndex"))
        {
            PlayerPrefs.SetInt("LevelBasedRewardIndex", 1);
            if (PlayerPrefs.GetInt("weapon18") == 1)
                return false;
            gunsRewardsPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            currentGunReward = 1;
            GunsRewardWinText.SetActive(true);

            PlayerPrefs.SetInt("weapon18", 1);
            PlayerPrefs.SetInt("AssaultEquipped", 18);
            return true;
        }
        else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT && PlayerPrefs.GetInt("currentLevel") == 16) //&& UnityAdsManager.Instance.IsRewardedVideoReady())
        {
            if (PlayerPrefs.GetInt("weapon14") == 1)
                return false;
            gunsRewardsPanel.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
            currentGunReward = 4;
            GunsRewardWinText.SetActive(true);
            PlayerPrefs.SetInt("PendingGun1", 1);
            GunRewardedAd.SetActive(true);
            return true;
        }
        else if (PlayerPrefs.GetInt("LastLevel") >= 5 && PlayerPrefs.GetInt("LevelBasedRewardIndex") < 2)
        {
            PlayerPrefs.SetInt("LevelBasedRewardIndex", 2);
            if (PlayerPrefs.GetInt("weapon13") == 1)
                return false;
            gunsRewardsPanel.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
            currentGunReward = 5;
            GunsRewardWinText.SetActive(true);

            PlayerPrefs.SetInt("weapon13", 1);
            PlayerPrefs.SetInt("AssaultEquipped", 13);
            return true;
        }
        else if (PlayerPrefs.GetInt("LastLevel") >= 8 && PlayerPrefs.GetInt("LevelBasedRewardIndex") < 3)
        {
            PlayerPrefs.SetInt("LevelBasedRewardIndex", 3);
            if (PlayerPrefs.GetInt("weapon8") == 1)
                return false;
            gunsRewardsPanel.transform.GetChild(0).GetChild(6).gameObject.SetActive(true);
            currentGunReward = 6;
            GunsRewardWinText.SetActive(true);

            PlayerPrefs.SetInt("weapon8", 1);
            PlayerPrefs.SetInt("AssaultEquipped", 8);
            return true;
        }
        else if (LevelSelectionNew.modeSelection == 0 && PlayerPrefs.GetInt("sniperCurrentLevel") == 5)
        {
            if (PlayerPrefs.GetInt("weapon21") == 1)
                return false;
            gunsRewardsPanel.transform.GetChild(0).GetChild(7).gameObject.SetActive(true);
            currentGunReward = 7;
            GunsRewardWinText.SetActive(true);

            PlayerPrefs.SetInt("weapon21", 1);
            PlayerPrefs.SetInt("SniperEquipped", 21);
            return true;
        }
        else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER && PlayerPrefs.GetInt("sniperCurrentLevel") == 8)
        {
            if (PlayerPrefs.GetInt("weapon23") == 1)
                return false;
            gunsRewardsPanel.transform.GetChild(0).GetChild(8).gameObject.SetActive(true);
            currentGunReward = 8;
            GunsRewardWinText.SetActive(true);

            PlayerPrefs.SetInt("weapon23", 1);
            PlayerPrefs.SetInt("SniperEquipped", 23);
            return true;
        }

        return false;
    }

    public void ClaimGunsReward()
    {
        switch (currentGunReward)
        {
            case 2:
                UnityAdsManager.isGunAd = true;
                notAdseen = true;
                UnityAdsManager.Instance.ShowUnityRewardedVideoAd();
                break;

            case 4:
                UnityAdsManager.isGunAd = true;
                notAdseen = true;
                UnityAdsManager.Instance.ShowUnityRewardedVideoAd();
                break;
        }
        gunsRewardsPanel.SetActive(false);
        StartCoroutine(AfterRewards());
    }

    public void CloseGunsRewardsPanel()
    {
        gunsRewardsPanel.SetActive(false);
        StartCoroutine(AfterRewards());
    }

    private IEnumerator AfterRewards()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " Entering AfterRewards");
#endif
        if (isLevelFailed)
        {
           // GoogleMobileAdsManager.Instance.ShowMedBanner();
            yield break;
        }
        
        nextButton.SetActive(true);
        homebutton_complete.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        Screen.SetResolution(PlayerPrefs.GetInt("Width"), PlayerPrefs.GetInt("Height"), true, 60);

       // GoogleMobileAdsManager.Instance.ShowMedBanner();
    }

    private void ShowPanel(GameObject panelToShow)
    {
        fpsUICanvasPanel.SetActive(false);
        fpsUICanvasPanel1.SetActive(false);
        settingPanel.SetActive(false);
        levelPausePanel.SetActive(false);
        levelFailPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        levelUpPanel.SetActive(false);
        //rateUsPanel.SetActive(false);
        //removeAdsPanel.SetActive(false);
        rewardPanel.SetActive(false);
        loadingPanel.SetActive(false);

        panelToShow.SetActive(true);
    }

    public void ShowScope()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            miniMap.SetActive(false);
            //GoogleMobileAdsManager.Instance.HideBanner();
            //GameplayBannerBG.SetActive(false);
        }

        if (sniper_1.activeInHierarchy || sniper_2.activeInHierarchy)
        {
            scopeImage_1.SetActive(true);
            scopeImage_2.SetActive(false);
            sniper_1.SetActive(false);
            sniper_2.SetActive(false);
        }

        if (sniper_3.activeInHierarchy || sniper_4.activeInHierarchy || sniper_5.activeInHierarchy || sniper_6.activeInHierarchy)
        {
            scopeImage_1.SetActive(false);
            scopeImage_2.SetActive(true);
            sniper_3.SetActive(false);
            sniper_4.SetActive(false);
            sniper_5.SetActive(false);
            sniper_6.SetActive(false);
        }
    }

    public void HideScope()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            miniMap.SetActive(true);
           // GameplayBannerBG.SetActive(true);
            //   GoogleMobileAdsManager.Instance.ShowBanner();
        }

        //if (!sniper_1.activeInHierarchy || sniper_1_Updated.activeInHierarchy)
        //{
        //    scopeImage_1.SetActive(false);
        //    scopeImage_2.SetActive(false);
        //    sniper_1.SetActive(true);
        //    sniper_1_Updated.SetActive(true);
        //}

        //if (!sniper_2.activeInHierarchy || sniper_2_Updated.activeInHierarchy)
        //{
        //    scopeImage_1.SetActive(false);
        //    scopeImage_2.SetActive(false);
        //    sniper_2.SetActive(true);
        //    sniper_2_Updated.SetActive(true);
        //}
        if (!sniper_1.activeInHierarchy || sniper_2.activeInHierarchy)
        {
            scopeImage_1.SetActive(false);
            scopeImage_2.SetActive(false);
            sniper_1.SetActive(true);
            sniper_2.SetActive(true);
        }

        if (!sniper_3.activeInHierarchy || sniper_4.activeInHierarchy || sniper_5.activeInHierarchy || sniper_6.activeInHierarchy)
        {
            scopeImage_1.SetActive(false);
            scopeImage_2.SetActive(false);
            sniper_3.SetActive(true);
            sniper_4.SetActive(true);
            sniper_5.SetActive(true);
            sniper_6.SetActive(true);
        }
    }

    public void PickedNade()
    {
        PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 1);
        fpsPlayer.PlayerWeaponsComponent.GrenadeWeaponBehaviorComponent.maxAmmo = PlayerPrefs.GetInt("Grenade");
        fpsPlayer.PlayerWeaponsComponent.GrenadeWeaponBehaviorComponent.ammo = fpsPlayer.PlayerWeaponsComponent.GrenadeWeaponBehaviorComponent.maxAmmo;
        GameManager.Instance.UpdateText();
    }

    public void UpdateText()
    {
        playerHealthFillImage.fillAmount = fpsPlayer.hitPoints / 100f;
        playerHealthText.text = (int)fpsPlayer.hitPoints + "%";
        remainingEnemyText.text = enemyKilled + "/" + totalEnemies;
        remainingGrenadeText1.text = remainingGrenadeText.text = System.String.Empty + PlayerPrefs.GetInt("Grenade");
        remainingMedicKitText1.text = remainingMedicKitText.text = System.String.Empty + PlayerPrefs.GetInt("Injection");
    }

    public void HideHeadShotText()
    {
        //animatedHeadShot.enabled = false;
        //animatedHeadShotText.SetActive(false);
        //animatedHeadShotText_2.SetActive(false);
    }

    //public void ShowHeadShotText(Transform transform)
    //{
    //    objectiveStats[(int)AssaultLevels.LevelConidtions.isHeadShot]++;
    //    if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER && totalEnemies - enemyKilled == 1)
    //        Invoke("DelayedHeadShot", 1.2f);
    //    else
    //        AudioManager.instance.otherAudioSource.PlayOneShot(headshotClip);
    //    //animatedHeadShotText.SetActive(true);
    //}
    //void DelayedHeadShot()
    //{
    //    AudioManager.instance.otherAudioSource.PlayOneShot(headshotClip);
    //}

    

    

    private void InitializeTime()
    {
        // Get Current Level
        switch (levelIndex)
        {
            case 18:
                totalTime = 80;
                InvokeRepeating("StartTime", 1f, 1f);
                break;

            case 21:
                totalTime = 90;
                InvokeRepeating("StartTime", 1f, 1f);
                break;

            case 24:
                totalTime = 110;
                InvokeRepeating("StartTime", 1f, 1f);
                break;

            case 26:
                totalTime = 240;
                InvokeRepeating("StartTime", 1f, 1f);
                break;

            case 27:
                totalTime = 360;
                InvokeRepeating("StartTime", 1f, 1f);
                break;
        }
    }

    private void StartTime()
    {
        remainingTimeText.text = "TIME LEFT: " + totalTime--;
    }

    public void ForceLevelFail()
    {
        isForcedFailed = true;
        try
        {
            StartCoroutine(LevelFail());
        }
        catch { }
    }

    private bool isMedicKitSoundPlayed = false;

    public void CheckLevelFail()
    {
        if (fpsPlayer.hitPoints <= 15 && !isMedicKitSoundPlayed && PlayerPrefs.GetInt("Injection") > 0)
        {
            isMedicKitSoundPlayed = true;
            AudioManager.instance.backAudioSource.PlayOneShot(medicKit);
        }

        // Get Current Level
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
        {
            if (PlayerPrefs.GetInt("currentLevel") > 5 && fpsPlayer.hitPoints <= 1)
            {
                try
                {
                    StartCoroutine(LevelFail());
                }
                catch { }
            }
        }
        else
        {
            if (fpsPlayer.hitPoints <= 1)
            {
                try
                {
                    StartCoroutine(LevelFail());
                }
                catch { }
            }
        }
    }

    public void EnablePlayerModel()
    {
        playerModel.SetActive(true);
    }

    public void LevelFailed()
    {
        try
        {
            StartCoroutine(LevelFail());
        }
        catch { }
    }

    private IEnumerator LevelFail()
    {
        if (isLevelFailed)
            yield break;
        isLevelFailed = true;
        CalculateScore();
        gamePlayPanel.SetActive(false);
        
        if (!isForcedFailed)
            EnablePlayerModel();

        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
        {
            bulletDamage.isFailed = true;
            bulletDamage.AddHealth(fpsPlayer.hitPoints);
            PlayerPrefs.SetString("HPStats", JsonUtility.ToJson(bulletDamage));
        }

        // Unity Analytics
        PlayerPrefs.SetInt("Death", PlayerPrefs.GetInt("Death") + 1);
#if UNITY_EDITOR
        Debug.Log("CustomEvent: " + "Death");
#endif
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            AnalyticsResult result = Analytics.CustomEvent("FailSniper", new Dictionary<string, object>
            {
            { "level_index", sniperLevelIndex }
            });

            // Firebase Event
            //FirebaseAnalytics.LogEvent("LevelFailSniper",
            //new Parameter("level_index", sniperLevelIndex));
            HandleAnalyticsResult(result);
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "FailSniper");
#endif
        }
        else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
        {
            AnalyticsResult result = Analytics.CustomEvent("FailAssault", new Dictionary<string, object>
        {
            { "level_index", levelIndexAnalytics }
            });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "FailAssault");
#endif
            HandleAnalyticsResult(result);
        }
        else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.COVERSTRIKE)
        {
            AnalyticsResult result = Analytics.CustomEvent("FailCoverStrike", new Dictionary<string, object>
        {
            { "level_index", sniperLevelIndex }
            });
            HandleAnalyticsResult(result);
        }

        if (!PlayerPrefs.GetInt("levelUnlocked-1").Equals(1))
            failedLoadoutButton.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);//0.5f
        Time.timeScale = 0;
        mainCamera.farClipPlane = 1;
        DisableHudIcons();
        
        //levelFailReasonText.text = System.String.Empty + levelFailReason;
        // Ads
        GameplayBannerBG.SetActive(false);
        GoogleMobileAdsManager.Instance.HideBanner();
        //UpdatePrefs();
        ShowPanel(levelFailPanel);

        // Disabling Failed Buttons
        failedLoadoutButton.GetComponent<Button>().interactable = false;
        failedRestartButton.GetComponent<Button>().interactable = false;

        yield return new WaitForSecondsRealtime(0.15f);

        AudioManager.instance.PlayGameOver();
        yield return new WaitForSecondsRealtime(0.3f);
        UpdatePrefs();
        failedButtons.SetActive(true);
       

        if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
        {
            GoogleMobileAdsManager.Instance.HideMedBanner();
            GoogleMobileAdsManager.Instance.ShowInterstitial();
        }
        else
        {
            GoogleMobileAdsManager.Instance.ShowMedBanner();
        }
        yield return new WaitForSecondsRealtime(0.1f);
        // Enabling Failed Buttons
        failedLoadoutButton.GetComponent<Button>().interactable = true;
        failedRestartButton.GetComponent<Button>().interactable = true;
    }

    public void PlaySniperBackgroundSound()
    {
        if (AudioManager.instance.backgroundMusicSouce.clip != sniperBackground)
        {
            AudioManager.instance.backgroundMusicSouce.clip = sniperBackground;
            AudioManager.instance.backgroundMusicSouce.Play();
        }
    }

    private int carrierDataNetwork, localAreaNetwork, reachable = 0;

    public void OnEnemyKilled()
    {
        if (isLevelCompleted || isLevelFailed)
            return;

        if ((totalEnemies - enemyKilled) < 2)
        {
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER &&
                AudioManager.instance)
            {
                PlaySniperBackgroundSound();
            }
        }

       

        enemyKilled++;

        

        GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isMinKills] = enemyKilled;
        UpdateText();
        if (totalEnemies - enemyKilled <= 0)
        {
            // Sounds at the end of gameplay
            StartCoroutine(DelayedGameplay2Sound(1.0f));

            fpsPlayer.invulnerable = true;
            for (int i = 0; i < 25; i++)
            {
                if (PlayerPrefs.GetInt("weapon" + i).Equals(1))
                {
                    weaponCount++;
                }
            }

            if (_heliObject != null)
                _heliObject.SetActive(false);

            DisableHudIcons();
            isLevelCompleted = true;

            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
            {
                bulletDamage.isFailed = false;
                bulletDamage.AddHealth(fpsPlayer.hitPoints);
                PlayerPrefs.SetString("HPStats", JsonUtility.ToJson(bulletDamage));
            }

            // Unity Analytics
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                carrierDataNetwork = reachable = 1;
                localAreaNetwork = 0;
#if UNITY_EDITOR
                print("DATA: True");
#endif
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                localAreaNetwork = reachable = 1;
                carrierDataNetwork = 0;
#if UNITY_EDITOR
                print("WIFI: True");
#endif
            }
            else if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                carrierDataNetwork = localAreaNetwork = reachable = 0;
#if UNITY_EDITOR
                print("DATA/WIFI: False");
#endif
            }

            PlayerPrefs.SetInt("MissionCompleteEvent", 1);
            PlayerPrefs.SetInt("MissionComplete_Session", PlayerPrefs.GetInt("MissionComplete_Session") + 1);

            PlayerPrefs.SetInt("MissionComplete", PlayerPrefs.GetInt("MissionComplete") + 1);
#if UNITY_EDITOR
            Debug.Log("MissionComplete " + PlayerPrefs.GetInt("MissionComplete"));
#endif
            Analytics.CustomEvent("MissionComplete", new Dictionary<string, object>
            {
            { "level_index", PlayerPrefs.GetInt("MissionComplete") }
            });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "MissionComplete");
#endif
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            {
                // new events add for experiment
                Analytics.CustomEvent("LevelCompleteSniperMode", new Dictionary<string, object>
                {
                    { "level_index" , sniperLevelIndex },
                    { "Death", PlayerPrefs.GetInt("Death") },
                    { "Health", fpsPlayer.hitPoints},
                    { "Bullets", WeaponBehavior.bulletCount / totalEnemies},
                    { "Time", PlayedTime },
                    { "Injection", PlayerPrefs.GetInt("Injection") },
                    { "Grenade", PlayerPrefs.GetInt("Grenade") },
                    { "HeadShot", headShot },
                    { "WeaponCount", weaponCount },
                    { "AverageFPS", FPSCounter.instance.avgFPS}
                });
                // Firebase Event
                //FirebaseAnalytics.LogEvent("LevelCompleteSniper",
                //new Parameter("level_index", sniperLevelIndex));
#if UNITY_EDITOR
                Debug.Log("CustomEvent: " + "LevelCompleteSniperMode");
#endif

                Debug.Log("CustomEvent: " + "LevelCompleted : " + sniperLevelIndex);
                PlayerPrefs.SetInt("Death", 0);
                InAppManager.inAppCount = 0;
                if (Heli_Health.instance != null)
                {
                    Heli_Health.instance.DisableHeliSound();
                }
            }
            
           

            fpsPlayer.GetComponent<Ironsights>().enabled = false;
            playerweapons.GetComponent<GunSway>().enabled = false;
            playerweapons.GetComponent<PlayerWeapons>().enabled = false;
            playerweapons.GetComponent<WeaponEffects>().enabled = false;

            // Removing Gameplay Controlls Here
            #region Hiding Gameplay Controlls
                sniperControls.SetActive(false);
            #endregion

           

            UnlockNextLevel();

            
            try
            {
                StartCoroutine(GiveReward());
#if UNITY_ANDROID
                Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " After GiveReward CoroutineCalled");
#endif
            }
            catch { }

            // Disabling IronSight And Gunsway Scripts From FPSPlayer And Weapons Respectively to handle exceptions at level complete
            try
            {
                fpsPlayer.GetComponent<Ironsights>().enabled = false;
                playerweapons.GetComponent<GunSway>().enabled = false;
            }
            catch { }
            // Loading Ads After Level Completion Here
            //GoogleMobileAdsManager.Instance.RequestInterstitial();
           
        }

        if(gold >= 1)
        {
            doubleRewardButton.gameObject.SetActive(true);
        }
        else
        {
            doubleRewardButton.gameObject.SetActive(false);
            restartButton.transform.position = restartButton2Position.position;
        }
    }

   

    private IEnumerator DelayedGameplay2Sound(float delay)
    {
        yield return new WaitForSecondsRealtime(1.0f);
        if (AudioManager.instance.backgroundMusicSouce.clip != completeStatsPanelSound)
        {
            AudioManager.instance.backgroundMusicSouce.clip = completeStatsPanelSound;
            AudioManager.instance.backgroundMusicSouce.Play();
        }
        yield return new WaitForSecondsRealtime(1.0f);
        if (AudioManager.instance.backgroundMusicSouce.clip != gameplay_2)
        {
            AudioManager.instance.backgroundMusicSouce.clip = gameplay_2;
            AudioManager.instance.backgroundMusicSouce.Play();
            normalFPS.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    private void DelayedFinishVoice()
    {
        AudioManager.instance.PlayAwsome();
        //if (Random.Range(0, 2) > 0)
        //    AudioManager.instance.PlayAwsome();
        //else
        //    AudioManager.instance.PlayGreatJob();
    }

    private IEnumerator GiveReward()
    {
        yield return new WaitForSeconds(2.0f);
        CalculateScore();
        UpdatePrefs();

        if (PlayerPrefs.GetInt("levelUnlocked-6").Equals(1) ||
            PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1) ||
            PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-1").Equals(1))
        {
            weaponsCamera.SetActive(false);
            fpsPlayer.PlayerWeaponsComponent.gameObject.SetActive(false);
            mainCavas.renderMode = RenderMode.ScreenSpaceCamera;
            gamePlayPanel.SetActive(false);
            
            if (isAllObjectives)
                completeTauntAnimator.SetInteger("Full", 1);
            else
                completeTauntAnimator.SetInteger("Short", 1);
            mainCavas.renderMode = RenderMode.ScreenSpaceOverlay;
            nextLevelPanel.SetActive(false);
            if (isRain)
            {
                for (int i = 0; i < rainOBJs.Length; i++)
                    rainOBJs[i].SetActive(false);
            }
        }
        else
        {
            if (successfulObjectivesCount >= 3)
            {
                weaponsCamera.SetActive(false);
                mainCavas.renderMode = RenderMode.ScreenSpaceCamera;
                gamePlayPanel.SetActive(false);
               
                if (isAllObjectives)
                    completeTauntAnimator.SetInteger("Full", 1);
                else
                    completeTauntAnimator.SetInteger("Short", 1);
                mainCavas.renderMode = RenderMode.ScreenSpaceOverlay;
                nextLevelPanel.SetActive(false);
                
            }
        }
        // Editing Text For DoubleRewardButton
        doubleSPText.text = (sp * 2).ToString();
        doubleGoldText.text = (gold * 2).ToString();
        StartCoroutine(ShowLevelComplete());
    }

    private float sucessfulObjectives = 0;

    private void SetObjectivesUI()
    {
        GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isMinBullets] = WeaponBehavior.bulletCount;
        //Debug.Log("BUllets fired  " + WeaponBehavior.bulletCount);
        GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isAccuracy] = (int)((float)(totalEnemies * 7) / (float)WeaponBehavior.bulletCount);
        sucessfulObjectives = 0;
        for (int i = 0; i < 3; i++)
        {
            objectivesGoldStars[i].SetActive(false);
        }
        objectivesPerfectStars.SetActive(false);
        if (isLevelCompleted)
        {
            objectiveStats[(int)AssaultLevels.LevelConidtions.isObjective] = 10;
            objectiveStats[(int)AssaultLevels.LevelConidtions.isTime] = 50;
        }
        else
        {
            objectiveStats[(int)AssaultLevels.LevelConidtions.isObjective] = 0;
            objectiveStats[(int)AssaultLevels.LevelConidtions.isTime] = 0;
        }
        if (!PlayerPrefs.HasKey("Pass_Level"))
        {
            PlayerPrefs.SetFloat("Pass_Level", 1);
        }
        tempPass = PlayerPrefs.GetFloat("Pass_Level");
        sucessfulObjectives = ExperienceVal = 0;
        for (int i = 0; i < activeObjectives.Conditions.Length; i++)
        {
            //Debug.Log(" condition  " + activeObjectives.Conditions[i].condition + "  val " + objectiveStats[(int)activeObjectives.Conditions[i].condition] + "  comp  " + activeObjectives.Conditions[i].value);
            if (!activeObjectives.Conditions[i].isSmaller && objectiveStats[(int)activeObjectives.Conditions[i].condition] >= activeObjectives.Conditions[i].value)
            {
                objectivesCompleted[i] = 1;
                AddExperience((int)activeObjectives.Conditions[i].condition, i, activeObjectives.Conditions[i].multipliyer, true);
                successfulObjectivesCount++;
            }
            else if (activeObjectives.Conditions[i].isSmaller && objectiveStats[(int)activeObjectives.Conditions[i].condition] < activeObjectives.Conditions[i].value)
            {
                objectivesCompleted[i] = 1;
                AddExperience((int)activeObjectives.Conditions[i].condition, i, activeObjectives.Conditions[i].multipliyer, true);
                successfulObjectivesCount++;
            }
            else
            {
                objectivesCompleted[i] = 0;
                AddExperience((int)activeObjectives.Conditions[i].condition, i, activeObjectives.Conditions[i].multipliyer, false);
                isAllObjectives = false;
            }
        }
        switch (LevelSelectionNew.modeSelection)
        {
            case LevelSelectionNew.modeType.SNIPER:
                count = 0;
                for (int j = 1; j <= 15; j++)
                {
                    if (PlayerPrefs.GetInt("sniperLevelUnlocked-" + j).Equals(1))
                    {
                        count++;
                    }
                }
                levelRatioCurrentLevel = "" + (count) + "/15";
                levelRatio = levelRatioCurrentLevel;
                currentProgress = (int)((((float)(count)) / 56f) * 100);
                //levelRatioCurrentLevel = "" + (PlayerPrefs.GetInt("sniperCurrentLevel")) + "/10";
                //levelRatio = "" + (PlayerPrefs.GetInt("sniperNextLevel") - 1) + "/10";
                //currentProgress = (int)(((float)(PlayerPrefs.GetInt("sniperNextLevel") - 1) / 10f) * 100);
                break;

            case LevelSelectionNew.modeType.ASSAULT:
                count = 0;
                for (int j = 1; j <= 80; j++)
                {
                    if (PlayerPrefs.GetInt("levelUnlocked-" + j).Equals(1))
                    {
                        count++;
                    }
                }
                levelRatioCurrentLevel = "" + (count) + "/80";
                levelRatio = levelRatioCurrentLevel;
                currentProgress = (int)((((float)(count)) / 80f) * 100);
                //levelRatioCurrentLevel = "" + (PlayerPrefs.GetInt("currentLevel")) + "/80";
                //levelRatio = "" + (PlayerPrefs.GetInt("nextLevel") - 1) + "/80";
                //currentProgress = (int)((((float)(PlayerPrefs.GetInt("nextLevel") - 1)) / 80) * 100);
                break;

            case LevelSelectionNew.modeType.COVERSTRIKE:
                //ABDUL
                count = 0;
                for (int j = 1; j <= 10; j++)
                {
                    if (PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-" + j).Equals(1))
                    {
                        count++;
                    }
                }
                levelRatioCurrentLevel = "" + (count) + "/10";
                levelRatio = levelRatioCurrentLevel;
                currentProgress = (int)((((float)(count)) / 10f) * 100);
                //ratioText.text = (count) + "/10";
                //fillImage.fillAmount = (count) / 10f;

                //levelRatioCurrentLevel = "" + (PlayerPrefs.GetInt("CoverStrikeCurrentLevel") - 1) + "/10";
                //levelRatio = "" + (PlayerPrefs.GetInt("CoverStrikeNextLevel") - 1) + "/10";
                //currentProgress = (int)((((float)(PlayerPrefs.GetInt("CoverStrikeNextLevel") - 1)) / 10f) * 100);
                break;
        }

        gold = (int)(ExperienceVal * tempPass / 4);
        sp = (int)(ExperienceVal * tempPass / 12); // 20
        for (int i = 0; i < successfulObjectivesCount; i++)
            objectivesGoldStars[i].SetActive(true);
        if (successfulObjectivesCount == 3)
            objectivesPerfectStars.SetActive(true);
        else
        {
            //doubleRewardButton.gameObject.SetActive(false);
            if(sniperLevelIndex!=1)
            restartButton.SetActive(true);
        }
    }

    private void AddExperience(int i, int j, float multiPliyer, bool isDone)
    {
        ObjectivesContainer[i].SetActive(true);
        if (isDone)
        {
            ObjectivesTexts[i].text = "" + (int)(((70 * multiPliyer) / 4) * tempPass);
            ObjectivesStatToggles[i].GetChild(0).gameObject.SetActive(true);
            ObjectivesStatToggles[i].GetChild(1).gameObject.SetActive(false);
            ExperienceVal += 70 * multiPliyer;//90
            sucessfulObjectives++;
        }
        else
        {
            ObjectivesTexts[i].text = "0";
            ObjectivesStatToggles[i].GetChild(0).gameObject.SetActive(false);
            ObjectivesStatToggles[i].GetChild(1).gameObject.SetActive(true);
        }
    }

    //public void UpdatePassScore()
    //{
    //    float pass = PlayerPrefs.GetFloat("Pass_Level");

    //    LerpAmount = 0;
    //    TargetKills = (int)((enemyKilled * killMultiplier * pass) / 4);

    //    TargetHead = (int)((headShot * headShotMultiplier * pass) / 4);

    //    TargetObjectives = (int)((levelMultiplier * pass) / 4);

    //    TargetTime = (int)((((tempTime / 30) * timeMultiplier) * pass) / 4);

    //    //sp = (int)((totalReward - tempReward) / 40);
    //    PlayerPrefs.SetInt("TempExperience", (int)((ExperinceVal * pass) - (ExperinceVal * tempPass)));
    //    TargetGold = (int)(ExperinceVal / 4 * pass);

    //    //PlayerPrefs.SetInt("Experience", PlayerPrefs.GetInt("Experience") + totalReward);
    //    // Gold
    //    PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - gold);
    //    PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + (int)TargetGold);
    //    // SP
    //    PlayerPrefs.SetInt("tempsecretPoints", PlayerPrefs.GetInt("tempsecretPoints") - sp);
    //    PlayerPrefs.SetInt("tempsecretPoints", PlayerPrefs.GetInt("tempsecretPoints") + (int)(ExperinceVal / 20 * pass));
    //    //PlayerPrefs.SetInt("Experience", PlayerPrefs.GetInt("Experience") + totalReward);
    //    // Gold
    //    // SP
    //    //PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") + sp);
    //    switch (pass)
    //    {
    //        case 1.5f:
    //            passText.text = "PREMIUM PASS 1";
    //            break;
    //        case 2f:
    //            passText.text = "PREMIUM PASS 2";
    //            break;
    //        case 3f:
    //            passText.text = "PREMIUM PASS 3";
    //            break;
    //    }
    //}
    private void CalculateScore()
    {
        SetObjectivesUI();
    }

    private void UpdatePrefs()
    {
        PlayerPrefs.SetInt("TempExperience", (int)(ExperienceVal * tempPass));
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + gold);
        PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") + sp);

    }

    public void ShowLastBullet(Vector3 point)
    {
        fpsUICanvasPanel.SetActive(false);
        fpsUICanvasPanel1.SetActive(false);
        playerModelLastBullet.GetComponent<BulletEffectWrapper>().endMarker = point;
        mouseLookScript.enabled = false;
        fpsPlayer.PlayerWeaponsComponent.weaponOrder[fpsPlayer.PlayerWeaponsComponent.currentWeapon].GetComponent<WeaponBehavior>().enabled = false;
        fpsPlayer.PlayerWeaponsComponent.weaponOrder[fpsPlayer.PlayerWeaponsComponent.currentWeapon].transform.GetChild(3).gameObject.SetActive(false);
        fpsPlayer.PlayerWeaponsComponent.enabled = false;
        gamePlayPanel.SetActive(false);
        playerModelLastBullet.SetActive(true);
        playerModelLastBullet.transform.LookAt(BulletFollow.enemyObject.root.transform);
    }

    public IEnumerator ForceEndGame()
    {
        yield return new WaitForSecondsRealtime(7.5f);
        if (!isLevelCompleted && !isLevelFailed)
        {
            enemyKilled = totalEnemies;
            OnEnemyKilled();
        }
    }

    public void AfterBulletsVideo()
    {
        playerModelLastBullet.SetActive(true);
        playerModelLastBullet.transform.LookAt(BulletFollow.enemyObject.transform);
    }
    
    private void UnlockNextLevel()
    {
        switch (LevelSelectionNew.modeSelection)
        {
            case LevelSelectionNew.modeType.SNIPER:
                if (!PlayerPrefs.GetInt("sniperLevelUnlocked-" + (PlayerPrefs.GetInt("sniperCurrentLevel") + 1)).Equals(1))
                {
                    if (PlayerPrefs.GetInt("sniperCurrentLevel") < 25 && PlayerPrefs.GetInt("SniperComplete") == 0) // Adnan Work
                    {
                        PlayerPrefs.SetInt("sniperLevelUnlocked-" + PlayerPrefs.GetInt("sniperCurrentLevel"), 1);
                        PlayerPrefs.SetInt("sniperNextLevel", PlayerPrefs.GetInt("sniperCurrentLevel") + 1);
                        PlayerPrefs.SetInt("sniperCurrentLevel", PlayerPrefs.GetInt("sniperNextLevel"));
                    }
                    //else
                    if (PlayerPrefs.GetInt("sniperCurrentLevel") == 25) // Adnan Work
                    {
                        
                        PlayerPrefs.SetFloat("SniperComplete", 1);
                        isAllLevelCompleted = true;
                        PlayerPrefs.SetInt("sniperCurrentLevel", 25);
                        PlayerPrefs.SetInt("sniperNextLevel", PlayerPrefs.GetInt("sniperCurrentLevel"));
                    }
                    if (PlayerPrefs.GetInt("SniperFullUnlocked") <= PlayerPrefs.GetInt("sniperCurrentLevel"))
                    {
                        PlayerPrefs.SetInt("SniperFullUnlocked", PlayerPrefs.GetInt("sniperCurrentLevel")); // Unlock All Levels For Gun Reward After Every 5 Levels
                    }
                    else
                    {
                        PlayerPrefs.SetInt("SniperFullUnlocked", 25); // Unlock All Levels For Gun Reward After Every 5 Levels
                    }
                }
                else if (!PlayerPrefs.GetInt("sniperLevelUnlocked-25").Equals(1))
                {
                    PlayerPrefs.SetInt("sniperCurrentLevel", PlayerPrefs.GetInt("sniperNextLevel"));
                }
                //else if (PlayerPrefs.GetInt("sniperLevelUnlocked-16").Equals(1))
                //{
                //    PlayerPrefs.SetInt("sniperNextLevel", PlayerPrefs.GetInt("sniperCurrentLevel") + 1);
                //    PlayerPrefs.SetInt("sniperCurrentLevel", PlayerPrefs.GetInt("sniperNextLevel"));
                //}
                break;

            case LevelSelectionNew.modeType.ASSAULT:
                if (!PlayerPrefs.GetInt("levelUnlocked-" + (PlayerPrefs.GetInt("currentLevel"))).Equals(1))
                {
                    if (PlayerPrefs.GetInt("currentLevel") < 80)
                    {
                        PlayerPrefs.SetInt("levelUnlocked-" + PlayerPrefs.GetInt("LevelCount"), 1);
                        PlayerPrefs.SetInt("nextLevel", PlayerPrefs.GetInt("LevelCount") + 1);
                        PlayerPrefs.SetInt("currentLevel", (PlayerPrefs.GetInt("nextLevel")));
                        PlayerPrefs.SetInt("LevelCount", (PlayerPrefs.GetInt("nextLevel")));
                        if (PlayerPrefs.GetInt("currentLevel") > 30)
                        {
                            PlayerPrefs.SetInt("LevelIndex", Random.Range(1, 31));
                        }
                    }
                    else if (PlayerPrefs.GetInt("currentLevel") == 80)
                    {
                        isAllLevelCompleted = true;
                        PlayerPrefs.SetInt("currentLevel", 80);
                        PlayerPrefs.SetInt("nextLevel", 80);
                        PlayerPrefs.SetInt("levelUnlocked-" + PlayerPrefs.GetInt("currentLevel"), 1);
                    }

                    if (PlayerPrefs.GetInt("AssaultFullUnlocked") <= PlayerPrefs.GetInt("currentLevel"))
                    {
                        PlayerPrefs.SetInt("AssaultFullUnlocked", PlayerPrefs.GetInt("currentLevel")); // Unlock All Levels For Gun Reward After Every 5 Levels
                    }
                    else
                    {
                        PlayerPrefs.SetInt("AssaultFullUnlocked", 20); // Unlock All Levels For Gun Reward After Every 5 Levels
                    }
                }
                else if (!PlayerPrefs.GetInt("levelUnlocked-80").Equals(1))
                {
                    PlayerPrefs.SetInt("currentLevel", (PlayerPrefs.GetInt("nextLevel")));
                }
                else if (PlayerPrefs.GetInt("levelUnlocked-80").Equals(1))
                {
                    PlayerPrefs.SetInt("nextLevel", PlayerPrefs.GetInt("LevelCount") + 1);
                    PlayerPrefs.SetInt("currentLevel", (PlayerPrefs.GetInt("nextLevel")));
                }
                break;

            case LevelSelectionNew.modeType.COVERSTRIKE:
                if (!PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-" + (PlayerPrefs.GetInt("CoverStrikeCurrentLevel") + 1)).Equals(1))
                {

                    if (PlayerPrefs.GetInt("CoverStrikeCurrentLevel") < 10)
                    {
                        PlayerPrefs.SetInt("CoverStrikeLevelUnlocked-" + PlayerPrefs.GetInt("CoverStrikeCurrentLevel"), 1);
                        PlayerPrefs.SetInt("CoverStrikeNextLevel", PlayerPrefs.GetInt("CoverStrikeCurrentLevel") + 1);
                        PlayerPrefs.SetInt("CoverStrikeCurrentLevel", PlayerPrefs.GetInt("CoverStrikeNextLevel"));
                    }
                   
                    else if (PlayerPrefs.GetInt("CoverStrikeCurrentLevel") == 10)
                    {
                        isAllLevelCompleted = true;
                        PlayerPrefs.SetInt("CoverStrikeCurrentLevel", 10);
                        PlayerPrefs.SetInt("CoverStrikeNextLevel", 10);
                        PlayerPrefs.SetInt("CoverStrikeLevelUnlocked-" + PlayerPrefs.GetInt("CoverStrikeCurrentLevel"), 1);
                    }

                    if (PlayerPrefs.GetInt("CoverFullUnlocked") <= PlayerPrefs.GetInt("CoverStrikeCurrentLevel"))
                    {
                        PlayerPrefs.SetInt("CoverFullUnlocked", PlayerPrefs.GetInt("CoverStrikeCurrentLevel")); // Unlock All Levels For Gun Reward After Every 5 Levels
                    }
                    else
                    {
                        PlayerPrefs.SetInt("CoverFullUnlocked", 10); // Unlock All Levels For Gun Reward After Every 5 Levels
                    }
                }
                else if (!PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-10").Equals(1))
                {
                    PlayerPrefs.SetInt("CoverStrikeCurrentLevel", PlayerPrefs.GetInt("CoverStrikeNextLevel"));
                }
                else if (PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-10").Equals(1))// All levels are unlocked
                {
                    PlayerPrefs.SetInt("CoverStrikeNextLevel", PlayerPrefs.GetInt("CoverStrikeCurrentLevel") + 1);
                    PlayerPrefs.SetInt("CoverStrikeCurrentLevel", PlayerPrefs.GetInt("CoverStrikeNextLevel"));
                }

#if UNITY_EDITOR
                print("Cover Next Level 1st : " + PlayerPrefs.GetInt("CoverFullUnlocked"));
#endif
                break;
        }

        SaveManager.Instance.state.loadFirstTime = 1;
        SaveManager.Instance.Save();
    }

    private int weaponCount = 0;
    private GunScript[] _gunScripts;
    static int completeads;
    private IEnumerator ShowLevelComplete()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " Entered ShowLevelComplete");
#endif
        // Disabling Complete Buttons
        nextButton.GetComponent<Button>().interactable = false;

        loadoutButton.GetComponent<Button>().interactable = false;
        restartButton.GetComponent<Button>().interactable = false;
        doubleRewardButton.GetComponent<Button>().interactable = false;
        homebutton_complete.GetComponent<Button>().interactable = false;
#if UNITY_ANDROID
        Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " Complete Buttons Disabled");
#endif
#if UNITY_EDITOR
        print("Yahan Daalo Ad");
#endif
        
#if UNITY_EDITOR
        print("Complete");
#endif
        DisableLevelCompleteButtons();
        

        // Ads
        GameplayBannerBG.SetActive(false);
        GoogleMobileAdsManager.Instance.HideBanner();
        if(completeads == 0)
        {
            if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
            {
                GoogleMobileAdsManager.Instance.HideMedBanner();
                GoogleMobileAdsManager.Instance.ShowInterstitial();
            }
            else
            {
                GoogleMobileAdsManager.Instance.ShowMedBanner();
                GoogleMobileAdsManager.Instance.RequestInterstitial();
            }
            completeads = 1;
        }
        else if(completeads == 1)
        {
            if (UnityAdsManager.Instance.IsVideoReady())
            {
                GoogleMobileAdsManager.Instance.HideMedBanner();
                UnityAdsManager.Instance.ShowUnityVideoAd();
            }
            else if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
            {
                GoogleMobileAdsManager.Instance.HideMedBanner();
                GoogleMobileAdsManager.Instance.ShowInterstitial();
            }
            else
            {
                GoogleMobileAdsManager.Instance.ShowMedBanner();
                GoogleMobileAdsManager.Instance.RequestInterstitial();
            }
            completeads = 0;
        }

        
        
        mainCamera.farClipPlane = 1;

        isBannerLoaded = false;
        ShowPanel(levelCompletePanel);
#if UNITY_ANDROID
        Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " Showing LevelCompletePanel");
#endif
        completeButotns.SetActive(true);
        homebutton_complete.SetActive(true);
        // Fade In and Out
        fpsPlayer.levelLoadFadeObj.gameObject.SetActive(false);
        StartCoroutine(AfterRewards());
        yield return new WaitForSeconds(2.0f);
#if UNITY_ANDROID
        Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " ShowingAdsOnCompletion");
#endif
        
        yield return new WaitForSecondsRealtime(0.1f);
        // Enabling Complete Buttons
        nextButton.GetComponent<Button>().interactable = true;

        //tryNewModeButton.GetComponent<Button>().interactable = true;
        //tryNewWeaponButton.GetComponent<Button>().interactable = true;
        //loadoutButton.GetComponent<Button>().interactable = true;
        restartButton.GetComponent<Button>().interactable = true;
        doubleRewardButton.GetComponent<Button>().interactable = true;
        homebutton_complete.GetComponent<Button>().interactable = true;
#if UNITY_ANDROID
        Debug.Log("Debug : Gameplay For : " + LevelSelectionNew.modeSelection + " EnabledLevelCompleteButtons");
#endif
    }

    public float PlayedTime
    {
        get
        {
            return Time.time - startPlayTime;
        }
    }

    private GameObject g;

    public void InstantiatePickAble(int number, Transform Pos)
    {
        float randomFactor = Random.Range(1f, 3f);
        g = Instantiate(PickAbleWeapons[number], Pos.position, Pos.rotation);
        g.GetComponent<Rigidbody>().AddForce(new Vector3(randomFactor, randomFactor, randomFactor), ForceMode.Impulse);
    }

    public void InstantiatePickAbleHeal(Transform Pos)
    {
        g = Instantiate(pickAbleHeal, Pos.position + Vector3.up * 0.23f, Pos.rotation);
    }

    public void InstantiatePickAbleNade(Transform Pos)
    {
        g = Instantiate(pickAbleNade, Pos.position + Vector3.up * 0.18f, Pos.rotation);
    }

    public void ShowLevelUp()
    {
        //if (!PlayerPrefs.HasKey("Experience"))
        //{
        //    return;
        //}
        //else
        //{
        //    LastLevel = PlayerPrefs.GetInt("LastLevel");
        //    CurrentExp = PlayerPrefs.GetInt("Experience");
        //    PlayerPrefs.SetInt("PreviousLevel", LastLevel);

        //    while (CurrentExp > ((LastLevel + 1) * 640) + (340 * MultiPliyer(LastLevel + 1)))//440
        //    {
        //        if (CurrentExp > ((LastLevel + 1) * 640) + (340 * MultiPliyer(LastLevel + 1)))
        //        {
        //            LastLevel++;
        //            PlayerPrefs.SetInt("LastLevel", LastLevel);

        //            levelup = true;
        //        }
        //    }

        //if (levelUpPanel != null && PlayerLevel.isLevelUp == true)
        //{
        //    levelUp = 1;
        //    PlayerLevel.isLevelUp = false;
        //    levelUpPanel.SetActive(true);
        //}

        //    int nextDisplay = (LastLevel * 340) + 640;
        //    int currentExp = CurrentExp - (((LastLevel) * 640) + (340 * MultiPliyer(LastLevel)));

        //    PlayerPrefs.SetFloat("Val", (float)currentExp / nextDisplay);
        //    PlayerPrefs.SetInt("Next", nextDisplay);
        //    PlayerPrefs.SetInt("CurrentExp", currentExp);
        //}
    }

    private int MultiPliyer(int val)
    {
        if (val < 2)
            return 0;
        if (val == 2)
            return 1;
        return ((val - 1) + MultiPliyer(val - 1));
    }

    public void ShowAlert(bool isMedicKitButton)
    {
        if (isMedicKitButton)
        {
            alertTextAdrenaline.SetActive(true);
        }
        else
        {
            alertTextNade.SetActive(true);
        }
    }

    public void ShowPopup(int index)
    {
        currentPopupIndex = index;
        popupImage.sprite = popupSprites[currentPopupIndex];
        popup.SetActive(true);
    }

    public void RevivePlayer()
    {
        sniperProgressText.SetActive(false);
        assaultProgressText.SetActive(false);
        coverStrikeProgressText.SetActive(false);
        failedButtons.SetActive(false);
        GoogleMobileAdsManager.Instance.HideMedBanner();
        GoogleMobileAdsManager.Instance.ShowBanner();
        Time.timeScale = 1;
        //InitializeTime();
        mainCamera.farClipPlane = 250;
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        Invoke("EnableHudIcons",1.0f);

        fpsUICanvasPanel.SetActive(true);
        fpsUICanvasPanel1.SetActive(true);
        levelFailPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
        playerModel.SetActive(false);
        fpsPlayer.levelLoadFadeRef.FadeAndLoadLevel(Color.black, 1.5f, false);
    }

    public void DoubleReward()
    {
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + gold);
        PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") + sp);
        gold *= 2;
        sp *= 2;
        levelCompletePanel.SetActive(false);
        levelCompletePanel.SetActive(true);
        doubleSPPanelText.text = sp.ToString();
        doubleGoldPanelText.text = gold.ToString();
        doubleRewardButton.gameObject.SetActive(false);
        Invoke("ShowDoubleRewardPanel", 2.0f);
    }

    void ShowDoubleRewardPanel()
    {
        doubleRewardPanel.SetActive(true);
        AudioManager.instance.YoureWelcomeClick();
    }

    public void OKDoubleRewardPanel()
    {
        doubleRewardPanel.SetActive(false);
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();
    }

    public void SecretReward()
    {
        int randomIndex = Random.Range(0, 3);
        switch (randomIndex)
        {
            //case 0:
            //    PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") + sp);
            //    rewardText.text = "You have been Rewarded with	<size=70><color=orange>Double SP!</color></size>";
            //    spText.text = System.String.Empty + (sp *= 2);
            //    break;
            case 0:
                PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + gold);
                rewardText.text = "You have been Rewarded with	 <size=70><color=orange>Double Gold!</color></size>";
                //goldText.text = System.String.Empty + (gold *= 2);
                break;

            case 1:
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 2);
                rewardText.text = "You have been Rewarded	<size=70><color=orange>2 Adrenaline-H25!</color></size>";
                break;

            case 2:
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 2);
                rewardText.text = "You have been Rewarded	<size=70><color=orange>2 Explosive-G65!</color></size>";
                break;
        }

        // Ads
        GoogleMobileAdsManager.Instance.HideMedBanner();
        GoogleMobileAdsManager.Instance.ShowBanner();

        videoRewardPanel.SetActive(true);
    }

    public void ShowSecretReward()
    {
        AudioManager.instance.YoureWelcomeClick();
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
        {
            GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isCollectDrop]++;
            switch (levelIndex)
            {
                case 5:
                    secretRewards[0].SetActive(true);
                    break;

                case 8:
                    secretRewards[1].SetActive(true);
                    break;

                case 14:
                    secretRewards[2].SetActive(true);
                    break;

                case 21:
                    secretRewards[3].SetActive(true);
                    break;

                case 26:
                    secretRewards[4].SetActive(true);
                    break;

                case 29:
                    secretRewards[5].SetActive(true);
                    break;
            }

            DisableHudIcons();
            secretRewardPanel.SetActive(true);
            fpsUICanvasPanel.SetActive(false);
            fpsUICanvasPanel1.SetActive(false);
        }

        Time.timeScale = 0;
    }

    //ADS
    private void ShowAdmob()
    {
        if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
        {
            GoogleMobileAdsManager.Instance.ShowInterstitial();
#if UNITY_EDITOR
            Debug.Log("Admob1");
#endif
        }
    }

    private void ShowUnity()
    {
        if (UnityAdsManager.Instance.IsVideoReady())
        {
            UnityAdsManager.Instance.ShowUnityVideoAd();
        }
    }

    private void ShowAdmob_Unity()
    {
        if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
        {
            GoogleMobileAdsManager.Instance.ShowInterstitial();
#if UNITY_EDITOR
            Debug.Log("Admob2");
#endif
        }
        else if (UnityAdsManager.Instance.IsVideoReady())
        {
            UnityAdsManager.Instance.ShowUnityVideoAd();
        }
    }

    private void ShowUnity_Admob()
    {
        if (UnityAdsManager.Instance.IsVideoReady())
        {
            UnityAdsManager.Instance.ShowUnityVideoAd();
        }
        else if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
        {
            GoogleMobileAdsManager.Instance.ShowInterstitial();
#if UNITY_EDITOR
            Debug.Log("Admob3");
#endif
        }
    }

    private static int completeCount_1, completeCount_2, completeCount_3, sniperCompleteCount = 0;
    // Ads
    //public IEnumerator LevelCompleteAd(float delay)
    //{
    //}

    //public IEnumerator LevelFailAd(float delay)
    //{
    //    Screen.SetResolution(PlayerPrefs.GetInt("Width"), PlayerPrefs.GetInt("Height"), true, 60);

    //    yield return new WaitForSecondsRealtime(delay);
    //    // Remove Ads
    //    switch (levelIndex)
    //    {
    //        case 14:
    //        case 25:
    //            // Ads
    //            GoogleMobileAdsManager.Instance.HideMedBanner();
    //            GoogleMobileAdsManager.Instance.ShowBanner();
    //            removeAdsPanel.SetActive(true);
    //            break;
    //    }
    //}

    #region Button Methods

    private static int pauseCount_1, pauseCount_2, pauseCount_3, sniperPauseCount = 0;

    public void _PauseButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " Pause Button");
#endif
        pauseButtons.SetActive(true);
        CalculateScore();
        
        GameplayBannerBG.SetActive(false);
            GoogleMobileAdsManager.Instance.HideBanner();

        if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
        {
            GoogleMobileAdsManager.Instance.HideMedBanner();
            GoogleMobileAdsManager.Instance.ShowInterstitial();
        }
        else if (UnityAdsManager.Instance.IsVideoReady())
        {
            GoogleMobileAdsManager.Instance.HideMedBanner();
            UnityAdsManager.Instance.ShowUnityVideoAd();
        }
        else
        {
            GoogleMobileAdsManager.Instance.ShowMedBanner();
            GoogleMobileAdsManager.Instance.RequestInterstitial();
        }
        //Invoke("DelayedAddsShowBanner" , 0.5f);

        mainCamera.farClipPlane = 1;

        DisableHudIcons();

        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        isBannerLoaded = false;
        ShowPanel(levelPausePanel);

        // Fade In and Out
        fpsPlayer.levelLoadFadeObj.gameObject.SetActive(false);

       
         Time.timeScale = 0;
    }

    //private IEnumerator DelayedAddsShowBannerExtra(int val)
    //{
    //    yield return new WaitForSecondsRealtime(2.5f);
    //    switch (val)
    //    {
    //        case 0:
    //            GoogleMobileAdsManager.Instance.ShowMedBanner();
    //            break;

    //        case 1:
    //            // GoogleMobileAdsManager.Instance.ShowBanner();
    //            break;
    //    }
    //}
    bool isBannerLoaded = false;
    void DelayedAddsShowBanner()
    {
        if(!isBannerLoaded)
            GoogleMobileAdsManager.Instance.ShowMedBanner();

        isBannerLoaded = true;
       // Time.timeScale = 0;
    }

    public void _SettingButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " Settings Button");
#endif
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        mainCamera.farClipPlane = 1;

        DisableHudIcons();

        ShowPanel(settingPanel);

        // Fade In and Out
        fpsPlayer.levelLoadFadeObj.gameObject.SetActive(false);

        Time.timeScale = 0;

        GoogleMobileAdsManager.Instance.ShowMedBanner();
        GameplayBannerBG.SetActive(false);
        GoogleMobileAdsManager.Instance.HideBanner();
    }

    public void _CloseSettingButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " Close Settings Button");
#endif
        Time.timeScale = 1;
        if(LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            mainCamera.farClipPlane = 1000;
        }
        else
        {
            mainCamera.farClipPlane = 250;
        }
        
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();
        Invoke("EnableHudIcons",1.0f);
        fpsUICanvasPanel.SetActive(true);
        fpsUICanvasPanel1.SetActive(true);
        settingPanel.SetActive(false);

        if (soldierAnimResetDelegate != null)
            soldierAnimResetDelegate();

        // Additional Autoshoot Toggle Setting
        if (SaveManager.Instance.state.autoShoot == 1)
        {
            autoShoot_CoverStrike.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
            autoShoot_Assault.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
        }
        else
        {
            autoShoot_CoverStrike.transform.GetChild(0).GetComponent<Toggle>().isOn = false;
            autoShoot_Assault.transform.GetChild(0).GetComponent<Toggle>().isOn = false;
        }


        GoogleMobileAdsManager.Instance.HideMedBanner();
        GameplayBannerBG.SetActive(true);
        GoogleMobileAdsManager.Instance.ShowBanner();
    }

    public void _ResumeButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " Resume Button");
#endif
        // Ads
        pauseButtons.SetActive(false);
        //sniperProgressText.SetActive(false);
        //assaultProgressText.SetActive(false);
        //coverStrikeProgressText.SetActive(false);
        GoogleMobileAdsManager.Instance.HideMedBanner();
       // GoogleMobileAdsManager.Instance.ShowBanner();
        // StartCoroutine(DelayedAddsShowBanner(1));

        Time.timeScale = 1;
        successfulObjectivesCount = 0;
        mainCamera.farClipPlane = 1000;
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        Invoke("EnableHudIcons",1.0f);
        fpsUICanvasPanel.SetActive(true);
        fpsUICanvasPanel1.SetActive(true);
        levelPausePanel.SetActive(false);

        if (PlayerPrefs.GetInt("ADSUNLOCK") != 1)
            GoogleMobileAdsManager.Instance.RequestInterstitial();
        if (soldierAnimResetDelegate != null)
            soldierAnimResetDelegate();

        countdownPanel.SetActive(true);
        if (!GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded())
        {
            GoogleMobileAdsManager.Instance.RequestInterstitial();
        }

        GoogleMobileAdsManager.Instance.ShowBanner();
        GameplayBannerBG.SetActive(true);
    }

    public void _RestartButton()
    {
        if (pauseButtons.activeInHierarchy)
        {
#if UNITY_ANDROID
            Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " RestartButton From PausePanel");
#endif
        }
        else if (completeButotns.activeInHierarchy)
        {
#if UNITY_ANDROID
            Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " RestartButton From CompletePanel");
#endif
        }
        else if (failedButtons.activeInHierarchy)
        {
#if UNITY_ANDROID
            Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " RestartButton From FailPanel");
#endif
        }

        Screen.SetResolution(PlayerPrefs.GetInt("Width"), PlayerPrefs.GetInt("Height"), true, 60);

        // Ads
       // GoogleMobileAdsManager.Instance.HideMedBanner();
        Time.timeScale = 1;
        mainCamera.farClipPlane = 250;
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        Invoke("EnableHudIcons",1.0f);
        loadingPanel.SetActive(true);
        PlayerPrefs.SetInt("currentLevel", levelIndexAnalytics);
        PlayerPrefs.SetInt("sniperCurrentLevel", sniperLevelIndex);
        PlayerPrefs.SetInt("CoverStrikeCurrentLevel", sniperLevelIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
#if UNITY_EDITOR
        Debug.Log("Current Sniper Level " + sniperLevelIndex);
# endif
    }

    public void _HomeButton()
    {
        if (pauseButtons.activeInHierarchy)
        {
#if UNITY_ANDROID
            Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " HomeButton From PausePanel");
#endif
        }
        else if (completeButotns.activeInHierarchy)
        {
#if UNITY_ANDROID
            Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " HomeButton From CompletePanel");
#endif
        }
        else if (failedButtons.activeInHierarchy)
        {
#if UNITY_ANDROID
            Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " HomeButton From FailPanel");
#endif
        }
        // Ads
        //GoogleMobileAdsManager.Instance.HideMedBanner();
        Invoke("DelayedAddsShowBanner",1);

        Time.timeScale = 1;
        mainCamera.farClipPlane = 250;
        loadingPanel.SetActive(true);
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();
        PlayerPrefs.SetInt("MMPopupCount", 0);
        SceneManager.LoadScene("MainMenu");
        GameManagerStatic.Instance.GunsStoreFrom = 0;
    }

    public void _LoadOutButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " Check LoadOut Button");
#endif
    // Ads
    GoogleMobileAdsManager.Instance.HideMedBanner();

        Time.timeScale = 1;
        mainCamera.farClipPlane = 250;
        loadingPanel.SetActive(true);
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();
        SaveManager.Instance.state.gamePlayLoadoutButtonPressed = 1;
        SaveManager.Instance.Save();
        SceneManager.LoadScene("MainMenu");
        GameManagerStatic.Instance.GunsStoreFrom = 1;
    }

    public void _PurchaseWeaponButton()
    {
        // Ads
        GoogleMobileAdsManager.Instance.ShowBanner();

        Time.timeScale = 1;
        mainCamera.farClipPlane = 250;
        loadingPanel.SetActive(true);
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        purchaseButtonPresed = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void _NextButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " NextButton Pressed");
#endif
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            //if (sniperLevelIndex < 15)
            //{
                //Debug.Log("SniperNext " + sniperLevelIndex + " levelup " + levelUp);
                //PlayerPrefs.SetInt("sniperCurrentLevel", PlayerPrefs.GetInt("nextLevel"));
#if UNITY_EDITOR
                print("SP: " + sp);
                print("GOLD: " + gold);
                print("SumSP: " + PlayerPrefs.GetInt("secretPoints"));
                print("SumGold: " + PlayerPrefs.GetInt("gold"));
#endif
                Analytics.CustomEvent("SniperNext", new Dictionary<string, object>
            {
            { "level_index", sniperLevelIndex },
            { "levelup", levelUp},
                {"Liked", Liked },
                {"SP", sp },
                {"Gold", gold },
                { "LoadoutCount", 1 },
                {"SumSP", PlayerPrefs.GetInt("secretPoints") },
                {"SumGold", PlayerPrefs.GetInt("gold") }
            });
#if UNITY_EDITOR
                Debug.Log("CustomEvent: " + "SniperNext");
#endif
            //if (isAllLevelCompleted)
            //{
            //    isAllLevelCompleted = false;
            //    GoogleMobileAdsManager.Instance.HideMedBanner();
            //    GoogleMobileAdsManager.Instance.ShowBanner();
            //    sniperLevelsCompletedPanel.SetActive(true);
            //    sniperProgressText.SetActive(false);
            //    //PlayerPrefs.SetInt("CampMode", 0);//Assault
            //    //PlayerPrefs.SetInt("Mode", 1);
            //    //LevelSelectionNew.modeSelection = LevelSelectionNew.modeType.ASSAULT;
            //    return;
            //}
            print("currntlevel : "+PlayerPrefs.GetInt("sniperCurrentLevel"));
            if (GameManagerStatic.Instance.lastlevel == 0)
            {
#if UNITY_EDITOR
                Debug.Log("Next Kaam Chale Ga");
#endif
                loadingPanel.SetActive(true);
                try
                {
                    //if(PlayerPrefs.GetFloat("SniperComplete") == 1)
                    //PlayerPrefs.SetInt("sniperCurrentLevel", PlayerPrefs.GetInt("sniperCurrentLevel")+1);
                    StartCoroutine(LoadScene());
                }
                catch { }
            }
            else
            {
                loadingPanel.SetActive(true);
                try
                {
                    _HomeButton();
                }
                catch { }
                
#if UNITY_EDITOR
                Debug.Log("Home Kaam Chale Ga");
#endif
            }
        }
        else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
        {
#if UNITY_EDITOR
            print("SP: " + sp);
            print("GOLD: " + gold);
            print("SumSP: " + PlayerPrefs.GetInt("secretPoints"));
            print("SumGold: " + PlayerPrefs.GetInt("gold"));
#endif
            //Debug.Log("AssaultNext " + levelIndexAnalytics + " levelup " + levelUp);
            PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("nextLevel"));
            PlayerPrefs.SetInt("LevelCount", PlayerPrefs.GetInt("nextLevel"));
            AnalyticsResult result = Analytics.CustomEvent("AssaultNext", new Dictionary<string, object>
        {
            { "level_index", levelIndexAnalytics },
            { "levelup", levelUp },
            //{"Liked", Liked },
            {"SP", sp },
            {"Gold", gold },
            { "LoadoutCount", 1 },
            {"SumSP", PlayerPrefs.GetInt("secretPoints") },
            {"SumGold", PlayerPrefs.GetInt("gold") },
            {"ObjectiveOne",objectivesCompleted[0]},
            {"ObjectiveTwo", objectivesCompleted[1]},
            {"ObjectiveThree", objectivesCompleted[2]}
            });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "AssaultNext");
#endif
            HandleAnalyticsResult(result);
            if (PlayerPrefs.GetInt("levelUnlocked-80").Equals(1))
            {
                GoogleMobileAdsManager.Instance.HideMedBanner();
                GoogleMobileAdsManager.Instance.ShowBanner();
                assaultLevelsCompletedPanel.SetActive(true);
                assaultProgressText.SetActive(true);
                PlayerPrefs.SetInt("Mode", 0);
                LevelSelectionNew.modeSelection = LevelSelectionNew.modeType.ASSAULT;
                return;
            }
        }
        else if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.COVERSTRIKE)
        {
            //Debug.Log("AssaultNext " + levelIndexAnalytics + " levelup " + levelUp);
            AnalyticsResult result = Analytics.CustomEvent("CoverStrikeNext", new Dictionary<string, object>
        {
            { "level_index", sniperLevelIndex },
            { "levelup", levelUp },
            //{"Liked", Liked },
            {"SP", sp },
            {"Gold", gold },
            { "LoadoutCount", 1 },
            {"SumSP", PlayerPrefs.GetInt("secretPoints") },
            {"SumGold", PlayerPrefs.GetInt("gold") },
            {"ObjectiveOne",objectivesCompleted[0]},
            {"ObjectiveTwo", objectivesCompleted[1]},
            {"ObjectiveThree", objectivesCompleted[2]}
            });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "CoverStrikeNext");
#endif
            HandleAnalyticsResult(result);
            if (isAllLevelCompleted)
            {
                isAllLevelCompleted = false;
                GoogleMobileAdsManager.Instance.HideMedBanner();
                GoogleMobileAdsManager.Instance.ShowBanner();
                coverStrikeLevelsCompletedPanel.SetActive(true);
                coverStrikeProgressText.SetActive(true);
                PlayerPrefs.SetInt("CampMode", 0);//Assault
                //PlayerPrefs.SetInt("Mode", 0);
                //LevelSelectionNew.modeSelection = LevelSelectionNew.modeType.ASSAULT;
                return;
            }
        }

        // Ads
       // GoogleMobileAdsManager.Instance.HideMedBanner();

        Time.timeScale = 1;
        // Collect Garbage
     //   System.GC.Collect();

        if (AudioManager.instance)
            AudioManager.instance.NextLevelSelectSound();

        PlayerPrefs.SetInt("MMPopupCount", 0); // For Main Menu Weapon Popup
                                               //PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("nextLevel"));

        //loadingPanel.SetActive(true);
        //try
        //{
        //    StartCoroutine(LoadScene());
        //}
        //catch { }
    }

    private void HandleAnalyticsResult(AnalyticsResult result)
    {
        switch (result)
        {
            case AnalyticsResult.Ok:
#if UNITY_EDITOR
                print("Events Sent Successfully");
#endif
                break;

            case AnalyticsResult.NotInitialized:
#if UNITY_EDITOR
                print("Events Not Initialized");
#endif
                break;

            case AnalyticsResult.AnalyticsDisabled:
#if UNITY_EDITOR
                print("Events Disabled");
#endif
                break;

            case AnalyticsResult.TooManyItems:
#if UNITY_EDITOR
                print("Events have too many items");
#endif
                break;

            case AnalyticsResult.SizeLimitReached:
#if UNITY_EDITOR
                print("Events size limit reached");
#endif
                break;

            case AnalyticsResult.TooManyRequests:
#if UNITY_EDITOR
                print("Events too many requests");
#endif
                break;

            case AnalyticsResult.InvalidData:
#if UNITY_EDITOR
                print("Events Invalid data");
#endif
                break;

            case AnalyticsResult.UnsupportedPlatform:
#if UNITY_EDITOR
                print("Events UnSupported Platform");
#endif
                break;
        }
    }

    public void SniperContinueButton()
    {
        Time.timeScale = 1;
        // Collect Garbage
     //   System.GC.Collect();

        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        PlayerPrefs.SetInt("MMPopupCount", 0); // For Main Menu Weapon Popup
                                               //PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("nextLevel"));

        SaveManager.Instance.state.nextButtonPressed = 1;
        SaveManager.Instance.Save();
        GoogleMobileAdsManager.Instance.HideBanner();
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    [HideInInspector] public bool freeRetryButtonPressed = false;

    public void _FreeRetryButton()
    {
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();
        if (GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
        {
            if (revivePanel.activeSelf)
                revivePanel.SetActive(false);

            // Ads
            GoogleMobileAdsManager.Instance.HideMedBanner();
            GoogleMobileAdsManager.Instance.ShowBanner();

            freeRetryVideoButton.interactable = false;
            freeRetryButtonPressed = true;
            GoogleMobileAdsManager.Instance.ShowRewarded();
        }
        else if (UnityAdsManager.Instance.IsRewardedVideoReady())
        {
            if (revivePanel.activeSelf)
                revivePanel.SetActive(false);

            // Ads
            GoogleMobileAdsManager.Instance.HideMedBanner();
            GoogleMobileAdsManager.Instance.ShowBanner();

            freeRetryVideoButton.interactable = false;
            freeRetryButtonPressed = true;
            UnityAdsManager.Instance.ShowUnityRewardedVideoAd();
        }
    }

    [HideInInspector] public bool freeSecretButtonPressed = false;

    public void _FreeSecretButton()
    {
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        
    }

    [HideInInspector] public bool doubleRewardButtonPressed = false;

    public void _DoubleRewardButton()
    {
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();
        if (GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
        {
#if UNITY_ANDROID
            Debug.Log("Debug : Admob_Reward For DoubleReward");
#endif
            doubleRewardButtonPressed = true;
            doubleRewardButton.interactable = false;
            isBannerLoaded = false;
            GoogleMobileAdsManager.Instance.ShowRewarded();
            GoogleMobileAdsManager.Instance.HideMedBanner();
        }

        else if (UnityAdsManager.Instance.IsRewardedVideoReady())
        {
#if UNITY_ANDROID
            Debug.Log("Debug : Unity_Reward For DoubleReward");
#endif
            doubleRewardButtonPressed = true;
            doubleRewardButton.interactable = false;
            isBannerLoaded = false;
            UnityAdsManager.Instance.ShowUnityRewardedVideoAd();
            GoogleMobileAdsManager.Instance.HideMedBanner();
        }
        else
        {
            UnityAdsManager.Instance.InitializeUnityAds();
            doubleRewardNotAvailable.SetActive(true);
            Invoke("DoubleRewardNotAvailable", 1.0f);
        }
    }

    void DoubleRewardNotAvailable()
    {
        doubleRewardNotAvailable.SetActive(false);
    }


    public void HealthPickUp()
    {
        fpsPlayer.hitPoints = Mathf.Clamp(fpsPlayer.hitPoints + 25, 0, 100);
        playerHealthFillImage.fillAmount = fpsPlayer.hitPoints / 100;
        GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isMinHealth] = (int)fpsPlayer.hitPoints;
        UpdateText();
    }

    public void _MedicKitButton()
    {
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();
        float hp = 25f;
        if (PlayerPrefs.GetInt("Injection") > 0)
        {
            if (fpsPlayer.hitPoints != 100f)
            {
                fpsPlayer.hitPoints += hp;
                if (fpsPlayer.hitPoints > 100f)
                    fpsPlayer.hitPoints = 100f;
                playerHealthFillImage.fillAmount = fpsPlayer.hitPoints / hp;
                GameManager.Instance.objectiveStats[(int)AssaultLevels.LevelConidtions.isMinHealth] = (int)fpsPlayer.hitPoints;
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") - 1);
                adrenalineTutorialPanel.SetActive(false);
                Time.timeScale = 1;
                UpdateText();
            }
        }
        else
        {
            ShowAlert(true);
        }
    }

    public void _SoundFxSlider(float value)
    {
        AudioManager.instance.ChangeSoundFxVolume(value);
        SaveManager.Instance.state.soundFxVolume = value;
        SaveManager.Instance.Save();
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
        //fpsCamera.sensitivity = fpsCameraCS.sensitivity = value;
        SaveManager.Instance.Save();
    }

    public void _SprintSpeedSlider(float value)
    {
        SaveManager.Instance.state.sprintSpeed = value;
        fpsPlayer.GetComponent<FPSRigidBodyWalker>().sprintSpeed = value;
        SaveManager.Instance.Save();
    }

    public void _AutoShoot(bool value)
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " Autoshoot Toggle");
#endif
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        if (value)
            SaveManager.Instance.state.autoShoot = 1;
        else
            SaveManager.Instance.state.autoShoot = 0;

        SaveManager.Instance.Save();
        ToggleFireButtons();
    }

    public void _BloodEffectToggle(bool value)
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " BloodEffect Toggle");
#endif
        if (AudioManager.instance)
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

    public void _CollectButton()
    {
        if (AudioManager.instance)
            AudioManager.instance.NormalClick();

        rewardPanel.SetActive(false);
        StartCoroutine(ShowLevelComplete());
    }

    public void _ClosePopup()
    {
        // Ads
        GoogleMobileAdsManager.Instance.ShowMedBanner();
        //StartCoroutine(LevelFailAd(0f));

        AudioManager.instance.BackButtonClick();
        popup.SetActive(false);
    }

    //public void _CloseRemoveAdsPanel()
    //{
    //    // Ads
    //    GoogleMobileAdsManager.Instance.HideBanner();
    //    GoogleMobileAdsManager.Instance.ShowMedBanner();

    //    AudioManager.instance.BackButtonClick();
    //    removeAdsPanel.SetActive(false);
    //}

    public void _CloseVideoRewardPanel()
    {
        // Ads
        GoogleMobileAdsManager.Instance.HideBanner();
        GoogleMobileAdsManager.Instance.ShowMedBanner();

        AudioManager.instance.BackButtonClick();
        videoRewardPanel.SetActive(false);
    }

    public void _CloseSecretRewardPanelButton()
    {
        Time.timeScale = 1;
        AudioManager.instance.BackButtonClick();
        switch (levelIndex)
        {
            case 3:
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 2);
                break;

            case 5:
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 2);
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 2);
                //PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 50);
                break;

            case 8:
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 3);
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 3);
                //PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 50);
                break;

            case 14:
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 3);
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 3);
                //PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 50);
                break;

            case 21:
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 3);
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 3);
                break;

            case 26:
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 4);
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 4);
                break;

            case 29:
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + 2);
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + 2);
                //PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 50);
                break;
        }

        fpsPlayer.PlayerWeaponsComponent.InitGrenade();

        if (soldierAnimResetDelegate != null)
            soldierAnimResetDelegate();
        fpsUICanvasPanel.SetActive(true);
        fpsUICanvasPanel1.SetActive(true);
        secretRewardPanel.SetActive(false);
        Invoke("EnableHudIcons",0.5f);
    }

    public void ClickPasses()
    {
        //InAppManager.Instance.SetPassPrices();
        //passPricesText[0].text = InAppManager.Instance.PremiumPass1;
        //passPricesText[1].text = InAppManager.Instance.PremiumPass1Discount;
        //passPricesText[2].text = InAppManager.Instance.PremiumPass2;
        //passPricesText[3].text = InAppManager.Instance.PremiumPass2Discount;
        //passPricesText[4].text = InAppManager.Instance.PremiumPass3;
        //passPricesText[5].text = InAppManager.Instance.PremiumPass3Discount;

        passesPanel.SetActive(true);
        GoogleMobileAdsManager.Instance.HideMedBanner();
    }

    public void PassesBack()
    {
        passesPanel.SetActive(false);
        if (isPassPurchased)
        {
            isPassPurchased = false;
            DoScore = true;
        }

        //Ads
        GoogleMobileAdsManager.Instance.ShowMedBanner();
    }

    public void PurchasePass()
    {
        //InAppManager.Instance.PurchasePremiumPass1();
    }

    public void PurchasePass1()
    {
        //InAppManager.Instance.PurchasePremiumPass2();
    }

    public void PurchasePass2()
    {
        //InAppManager.Instance.PurchasePremiumPass3();
    }

    public void _PurchaseRemoveAds()
    {
        // Ads
        GoogleMobileAdsManager.Instance.HideBanner();
        GoogleMobileAdsManager.Instance.ShowMedBanner();

        AudioManager.instance.StoreButtonClick();
        //removeAdsPanel.SetActive(false);
        //InAppManager.Instance.PurchaseRemoveAds();
    }

    public GameObject notifyText, piggyPackPanel;
    public Text bankedSPText, bankedGoldText, totalPiggyBankText, piggyBankPriceText, disPiggyBankPriceText;
    public Image piggyBankFillImage;

    public void OpenPiggyPackPanel()
    {
        //GoogleMobileAdsManager.Instance.HideMedBanner();
        //bankedSPText.text = System.String.Empty + PlayerPrefs.GetInt("BankedSP");
        //bankedGoldText.text = System.String.Empty + PlayerPrefs.GetInt("BankedGold");
        //totalPiggyBankText.text = System.String.Empty + (PlayerPrefs.GetInt("BankedSP") + PlayerPrefs.GetInt("BankedGold")) + "/4500";
        //piggyBankFillImage.fillAmount = (((float)PlayerPrefs.GetInt("BankedSP")) + ((float)PlayerPrefs.GetInt("BankedGold"))) / 4500;

        //InAppManager.Instance.SetPiggyPackPrice();
        //piggyBankPriceText.text = System.String.Empty + InAppManager.Instance.piggyBankPrice;
        //disPiggyBankPriceText.text = System.String.Empty + InAppManager.Instance.disPiggyBankPrice;
        //if (disPiggyBankPriceText.text == "")
        //    disPiggyBankPriceText.text = "Purchase";

        //piggyPackPanel.SetActive(true);
    }

    public void PurchasePiggyBank()
    {
        //if ((PlayerPrefs.GetInt("BankedSP") + PlayerPrefs.GetInt("BankedGold")) >= 3600)
        //{
        //    InAppManager.Instance.PurchasePiggyBank();
        //}
        //else
        //{
        //    notifyText.SetActive(true);
        //}
    }

    public void CrossPiggyBank()
    {
        GoogleMobileAdsManager.Instance.HideMedBanner();
        piggyPackPanel.SetActive(false);
    }

    public void LikedButton()
    {
        AudioManager.instance.NormalClick();
        LikedBTNFailed.color = LikedBTNPause.color = LikedBTN.color = new Color(1, 1, 1, 1);
        DisLikedBTNFailed.color = DisLikedBTNPause.color = DisLikedBTN.color = new Color(1, 1, 1, 0.45f);
        Liked = 1;
    }

    public void DisLikedButton()
    {
        AudioManager.instance.NormalClick();
        LikedBTNFailed.color = LikedBTNPause.color = LikedBTN.color = new Color(1, 1, 1, 0.40f);
        DisLikedBTNFailed.color = DisLikedBTNPause.color = DisLikedBTN.color = new Color(1, 1, 1, 1);
        Liked = 0;
    }

    public void ControlsButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " Controls Button");
#endif
        gamePlayPanel.SetActive(false);
        settingPanel.SetActive(false);
        controlsPanel.SetActive(true);
        UiPositionChanger.Instance.SetFirst();
        // Disable Weapon Camera On ButtonsSetting For Sniper Mode Levels
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            weaponsCamera.GetComponent<Camera>().enabled = false;
        }
            GoogleMobileAdsManager.Instance.HideMedBanner();
    }

    public void ControlsReturn()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : " + LevelSelectionNew.modeSelection + " Controls Back Button");
#endif
        gamePlayPanel.SetActive(true);
        settingPanel.SetActive(true);
        controlsPanel.SetActive(false);
        // Enable Weapon Camera On After ButtonsSetting For Sniper Mode Levels
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            weaponsCamera.GetComponent<Camera>().enabled = true;
        }
            GoogleMobileAdsManager.Instance.ShowMedBanner();
    }

    public void LanugagesPanel()
    {
        languagesPanel.SetActive(true);
    }

    public void LanguagesBack()
    {
        languagesPanel.SetActive(false);
    }

    public void UpdateBullets(int total, int leftinClip)
    {
        bulletsCount.text = total + "/" + leftinClip;
        if ((LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT && leftinClip == 5 && total > 0) || (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER && leftinClip == 1 && total > 0))
        {
            AudioManager.instance.PlayReload();
        }
        if (total == 0 && leftinClip == 0 && fpsPlayer.PlayerWeaponsComponent.currentWeapon != fpsPlayer.PlayerWeaponsComponent.grenadeWeapon)
        {
            AudioManager.instance.PlayOutOfAmmo();
            if (!playerweapons.haveNoBullets && !playerweapons.CheckAllEmpty())
            {
                outOfBulletsImage.SetActive(false);
            }
            else
                outOfBulletsImage.SetActive(true);
            //if (!playerweapons.ManageEmptyGun() && !outOfBulletsImage.activeSelf)
            //{
            //    Debug.Log(" in show outs ");
            //    outOfBulletsImage.SetActive(true);

            //}
        }
    }

    //public void UpdateSwtichedWeaponBullets(int total, int leftinClip)
    //{
    //    bulletsCount.text = total + "/" + leftinClip;
    //    if (!outOfBulletsImage.activeSelf && total == 0 && leftinClip == 0)
    //    {
    //        outOfBulletsImage.SetActive(true);
    //    }
    //    else
    //        outOfBulletsImage.SetActive(false);
    //}
    public void ResetCurrentWeaponTexture(int index)
    {
        //if (!isRain || (index) >= gunsMats.Length || index < 0)
        //    return;
        //gunsMats[index].SetTexture("_MainTex", rainTextures.GetChild(index).GetComponent<RawImage>().texture);
        //rainTextures.GetChild(index).gameObject.SetActive(false);
    }

    public void SetCurrentWeaponTexture(int index)
    {
        //if (!isRain || (index) >= gunsMats.Length)
        //    return;
        //gunsMats[index].SetTexture("_MainTex", gunsRenderTex);
        //rainTextures.GetChild(index).gameObject.SetActive(true);
    }

    private IEnumerator LightingControlRoutine()
    {
        ThunderSounds.SetActive(true);
        float time = 0;
        yield return new WaitForSeconds(Random.Range(1.87f, 2.5f));
        while (true)
        {
            flashesCount = Random.Range(3, 7);
            lightIndex = Random.Range(0, 4);
            StartCoroutine(LightingRoutine());
            time = Random.Range(flashesCount * 0.28f, flashesCount * 2.5f);
            yield return new WaitForSeconds(0.9f);
            PlayThunder();
            yield return new WaitForSeconds(time / 1.88f);
            StopThunder();
            yield return new WaitForSeconds(Random.Range(time / 3, time));
        }
    }

    private void PlayThunder()
    {
        AudioManager.instance.PlayThunder();
    }

    private void StopThunder()
    {
        AudioManager.instance.StopThunder();
    }

    private IEnumerator LightingRoutine()
    {
        int currentFlash = 0;
        while (currentFlash <= flashesCount)
        {
            currentFlash++;
            ShowFlash();
            yield return new WaitForSeconds(Random.Range(0.01f, 0.08f));
            HideFlash();
            yield return new WaitForSeconds(Random.Range(0.01f, 0.08f));
        }
        HideFlash();
        yield break;
    }

    private void ShowFlash()
    {
        //LightsObject.SetActive(true);
        //Color c = new Color(0.08f, 0.56f, 0.78f, 1);
        //switch (lightIndex)
        //{
        //    case 0:

        //        for (int i = 0; i < GamePlayMats.Length; i++)
        //        {
        //            GamePlayMats[i].color = c;
        //        }
        //        c = new Color(0.05f, 0.45f, 0.65f, 1);
        //        for (int i = 0; i < gunsMats.Length; i++)
        //            gunsMats[i].SetColor("_Color", c);
        //        c = new Color(0.08f, 0.56f, 0.78f, 1);
        //        for (int i = 0; i < enemyMats.Length; i++)
        //            enemyMats[i].SetColor("_Color", c);
        //        break;
        //    case 1:
        //        c = new Color(0.68f, 0.50f, 0.25f, 1);
        //        for (int i = 0; i < GamePlayMats.Length; i++)
        //        {
        //            GamePlayMats[i].color = c;
        //        }
        //        c = new Color(0.41f, 0.32f, 0.15f, 1);
        //        for (int i = 0; i < gunsMats.Length; i++)
        //            gunsMats[i].SetColor("_Color", c);
        //        c = new Color(0.68f, 0.50f, 0.25f, 1);
        //        for (int i = 0; i < enemyMats.Length; i++)
        //            enemyMats[i].SetColor("_Color", c);
        //        break;
        //    case 2:
        //        c = new Color(0.45f, 0.19f, 0.1f, 1);
        //        for (int i = 0; i < GamePlayMats.Length; i++)
        //        {
        //            GamePlayMats[i].color = c;
        //        }
        //        c = new Color(0.37f, 0.14f, 0.1f, 1);
        //        for (int i = 0; i < gunsMats.Length; i++)
        //            gunsMats[i].SetColor("_Color", c);
        //        c = new Color(0.45f, 0.19f, 0.1f, 1);
        //        for (int i = 0; i < enemyMats.Length; i++)
        //            enemyMats[i].SetColor("_Color", c);
        //        break;
        //    case 3:
        //        c = new Color(0.78f, 0.78f, 0.78f, 1);
        //        for (int i = 0; i < GamePlayMats.Length; i++)
        //        {
        //            GamePlayMats[i].color = c;
        //        }
        //        c = new Color(0.65f, 0.65f, 0.65f, 1);
        //        for (int i = 0; i < gunsMats.Length; i++)
        //            gunsMats[i].SetColor("_Color", c);
        //        c = new Color(0.78f, 0.78f, 0.78f, 1);
        //        for (int i = 0; i < enemyMats.Length; i++)
        //            enemyMats[i].SetColor("_Color", c);
        //        break;
        //}
    }

    private void OnDisable()
    {
        GoogleMobileAdsManager.handleFullScreenAdClose -= DelayedAddsShowBanner;
        StopCoroutine("LightingControlRoutine");
        StopCoroutine("LightingRoutine");
        StopCoroutine("AfterRewards");
        StopCoroutine("LevelFail");
        StopCoroutine("DelayedGameplay2Sound");
        StopCoroutine("GiveReward");
        StopCoroutine("ShowLevelComplete");
        StopCoroutine("LoadScene");

        CancelInvoke("EnablingFPS");
        CancelInvoke("ShowBanner");
        CancelInvoke("ActiveHeadshotBadge");
        CancelInvoke("ActivetripleKillBadge");
        CancelInvoke("StartTime");
        CancelInvoke("ShowAdsOnCompletion");
        CancelInvoke("EnableHudIcons");
        CancelInvoke("DelayedAddsShowBanner");
        CancelInvoke("DelayedHideInAppProcess");
        CancelInvoke("AllowDelayedSound");
        CancelInvoke("DoubleRewardNotAvailable");
    }

    private void HideFlash()
    {
        //LightsObject.SetActive(false);
        //if(isRain)
        //{
        //Color c = new Color(GAMEPLAYCOLORLIGHTDARK, GAMEPLAYCOLORLIGHTDARK, GAMEPLAYCOLORLIGHTDARK, 1);
        //for (int i = 0; i < GamePlayMats.Length; i++)
        //{
        //    GamePlayMats[i].color = c;
        //}
        //c = new Color(GUNSCOLORLIGHTDARK, GUNSCOLORLIGHTDARK, GUNSCOLORLIGHTDARK, 1);
        //for (int i = 0; i < gunsMats.Length; i++)
        //    gunsMats[i].SetColor("_Color", c);
        //c = new Color(ENEMYCOLORLIGHTDARK, ENEMYCOLORLIGHTDARK, ENEMYCOLORLIGHTDARK, 1);
        //for (int i = 0; i < enemyMats.Length; i++)
        //    enemyMats[i].SetColor("_Color", c);
        //}
    }

    public void OpenBulletTutorial()
    {
        PlayerPrefs.SetInt("OpenBulletTutorial", 1);
        bulletTutorialPanel.SetActive(true);
    }

    public void CloseBulletTutorial()
    {
        bulletTutorialPanel.SetActive(false);
    }

    public void ShowAdrenalineTutorial()
    {
        adrenalineTutorialPanel.SetActive(true);
    }

    public void HideAdrenalineTutorial()
    {
        Time.timeScale = 1;
        adrenalineTutorialPanel.SetActive(false);
    }

    public void ShowInAppProcess()
    {
        inAppProcessPanel.SetActive(true);
    }

    public void HideInAppProcess(int State)
    {
        if (State == 0)
            inAppProcessPanel.SetActive(false);
        else
        {
            inAppProcessPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            if (State == 1)
                inAppProcessPanel.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            else
                inAppProcessPanel.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
            Invoke("DelayedHideInAppProcess", 2);
        }
    }

    private void DelayedHideInAppProcess()
    {
        inAppProcessPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        inAppProcessPanel.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
        inAppProcessPanel.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
        inAppProcessPanel.SetActive(false);
    }

    public void PlayCoverSound()
    {
        allowCoverSound = false;
        Invoke("AllowDelayedSound", 4.1f);
        AudioManager.instance.PlayFireTakeCover();
    }

    private void AllowDelayedSound()
    {
        allowCoverSound = true;
    }

    #endregion Button Methods

    public void HideMedBanner()
    {
        if (GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded() && !GoogleMobileAdsManager.Instance.IsLowMemory())
        {
            GoogleMobileAdsManager.Instance.HideMedBanner();
        }
    }

   

    // Elements to Hide or Show , when user unlocks guns as reward after level complete
    public GameObject completeUIPanel;

    

    [Header("----GunPickupButtons----")]
    public GameObject AssaultPickupBtn;

    // Button to be shown , while player is hittong a new gun in gameplay
    public void PickupNewGun()
    {
        try
        {
            WeaponPickup.instance.PickUpItem();
            GameManagerStatic.Instance.gunPickup = false;
            AssaultPickupBtn.SetActive(false);
            AssaultPickupBtn.transform.GetChild(0).gameObject.SetActive(false);
            AssaultPickupBtn.transform.GetChild(1).gameObject.SetActive(false);
            AssaultPickupBtn.transform.GetChild(2).gameObject.SetActive(false);
            AssaultPickupBtn.transform.GetChild(3).gameObject.SetActive(false);
            AssaultPickupBtn.transform.GetChild(4).gameObject.SetActive(false);
        }
        catch { }
    }

    
    [Header("----CompleteButtons----")]
    [Space(20)]
        public GameObject nextButton;
    public GameObject loadoutButton;
        //public GameObject tryNewModeButton;
        //public GameObject tryNewWeaponButton;
        public GameObject restartButton;
    public Transform restartButton2Position;
        public GameObject homebutton_complete;

    [Header("----FailedButtons----")]
    [Space(20)]
    public GameObject failedLoadoutButton;
    public GameObject failedRestartButton;
}