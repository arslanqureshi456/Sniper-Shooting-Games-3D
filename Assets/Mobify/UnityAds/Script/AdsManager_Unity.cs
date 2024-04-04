using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager_Unity : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    #region ID's - variables

    [SerializeField] string gameId = "ENTER_UNITY_GAME_ID";
    [SerializeField] string interstitialAdUnit = "Android_Interstitial";
    [SerializeField] string rewardedAdUnit = "Android_Rewarded";
    [SerializeField] bool isTestMode = false;

    #region For Editor Variable

    [SerializeField] bool isInitilizedOnStart = false;
    [SerializeField] bool isAdsAlwaysLoad = false;
    [SerializeField] bool isMemoryThreshold = false;
    [SerializeField] int NoInitilizationBelow = 400;
    [SerializeField] int NoInterstitialBelow = 500;
    [SerializeField] int NoRewardedBelow = 600;

    [SerializeField] MemoryThreshold NoUnityAdsBelowTotalRAM = MemoryThreshold._1536MB;
    public enum MemoryThreshold
    {
        _NoThreshold,
        _512MB,
        _1024MB,
        _1536MB,
        _2048MB,
        _3072MB,
        _4096MB
    }

    #endregion

    #endregion

    #region Instance

    static AdsManager_Unity _instance = null;
    public static AdsManager_Unity Instance
    {
        get
        {
            if (_instance == null)
            {
                if (FindObjectOfType<AdsManager_Unity>())
                {
                    _instance = FindObjectOfType<AdsManager_Unity>();
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

    bool IsInitilizedSDK
    {
        get
        {
            return Advertisement.isInitialized;
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        if (!IsInternetConnection()) yield break;
        if (IsLowTotalRAM()) yield break;

        if (isInitilizedOnStart || isAdsAlwaysLoad)
        {
            if (isAdsAlwaysLoad)
            {
                actionForLoadInterstitial = new Action(() => { LoadInterstitial(); });
                actionForLoadRewarded = new Action(() => { LoadRewardedAd(); });
            }
            InitilizeSDK();
        }
    }

    public void InitilizeSDK()
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (IsLowTotalRAM()) return;
        if (IsLowAvailableMemory(NoInitilizationBelow)) return;
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            if (initilizeStatus.Equals(InitilizeStatus.Initilizing) || initilizeStatus.Equals(InitilizeStatus.Initilized)) return;
            initilizeStatus = InitilizeStatus.Initilizing;

            ShowDebugLog("SDK Initilizing...");
            try
            {
                Advertisement.Initialize(gameId, isTestMode, this);
            }
            catch (Exception) { initilizeStatus = InitilizeStatus.None; }
        }
    }

    public void OnInitializationComplete()
    {
        initilizeStatus = InitilizeStatus.Initilized;
        ShowDebugLog("SDK Initialized.");
        if (actionForLoadInterstitial != null)
        {
            actionForLoadInterstitial.Invoke();
            actionForLoadInterstitial = null;
        }
        if (actionForLoadRewarded != null)
        {
            actionForLoadRewarded.Invoke();
            actionForLoadRewarded = null;
        }
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        initilizeStatus = InitilizeStatus.Failed;
        ShowDebugLog("SDK Initialization Failed. Error: " + error.ToString() + " , Msg: " + message);
        actionForLoadInterstitial = null;
        actionForLoadRewarded = null;
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

    #region Interstitial

    public bool IsInterstitialLoaded { get; private set; } = false;
    Action actionForLoadInterstitial = null;
    public void LoadInterstitial()
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (IsLowTotalRAM()) return;
        if (IsLowAvailableMemory(NoInterstitialBelow)) return;

        if (!IsInitilizedSDK)
        {
            actionForLoadInterstitial = new Action(() =>
            {
                LoadInterstitial();
            });
            InitilizeSDK();
            return;
        }
        if (IsInterstitialLoaded) return;

        ShowDebugLog("Interstitial Loading...");
        try
        {
            Advertisement.Load(interstitialAdUnit, this);
        }
        catch (Exception) { }
    }
    public void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInitilizedSDK) { return; }
        if (!IsInterstitialLoaded)
        {
            if (isAdsAlwaysLoad)
                LoadInterstitial();
            return;
        }
        if (IsLowAvailableMemory(NoInterstitialBelow)) return;

        // Note that if the ad content wasn't previously loaded, this method will fail
        ShowDebugLog("Interstitial Show");
        try
        {
            Advertisement.Show(interstitialAdUnit, this);
        }
        catch (Exception) { }
    }

    #endregion

    #region Rewarded

    public bool IsRewardedLoaded { get; private set; } = false;
    Action actionForLoadRewarded = null;
    public void LoadRewardedAd()
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInternetConnection()) return;
        if (IsLowTotalRAM()) return;
        if (IsLowAvailableMemory(NoRewardedBelow)) return;

        if (!IsInitilizedSDK)
        {
            actionForLoadRewarded = new Action(() =>
            {
                LoadRewardedAd();
            });
            InitilizeSDK();
            return;
        }
        if (IsRewardedLoaded) return;

        ShowDebugLog("Rewarded Loading...");
        try
        {
            Advertisement.Load(rewardedAdUnit, this);
        }
        catch (Exception) { }
    }
    public void ShowRewardedAd()
    {
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) return;
        if (!IsInitilizedSDK) { InitilizeSDK(); return; }
        if (!IsRewardedLoaded)
        {
            if (isAdsAlwaysLoad)
                LoadRewardedAd();
            return;
        }
        if (IsLowAvailableMemory(NoRewardedBelow)) return;

        ShowDebugLog("Rewarded Show");
        try
        {
            Advertisement.Show(rewardedAdUnit, this);
        }
        catch (Exception) { }
    }

    void GetReward()
    {
        ShowDebugLog("GetReward ( )");
        GetRewardCustomCode();
    }

    #endregion

    #region Handler

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(interstitialAdUnit)) { IsInterstitialLoaded = true; ShowDebugLog("Interstitial Loaded"); }
        if (placementId.Equals(rewardedAdUnit)) { IsRewardedLoaded = true; ShowDebugLog("Rewarded Loaded"); }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {

        if (placementId.Equals(interstitialAdUnit)) { IsInterstitialLoaded = false; ShowDebugLog("Interstitial FailedToLoad: Error :" + error.ToString() + ", msg :" + message); }
        if (placementId.Equals(rewardedAdUnit)) { IsRewardedLoaded = false; ShowDebugLog("Rewarded FailedToLoad: Error :" + error.ToString() + ", msg :" + message); }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        if (placementId.Equals(interstitialAdUnit))
        {
            IsInterstitialLoaded = false;
            if (isAdsAlwaysLoad) LoadInterstitial();
            ShowDebugLog("Interstitial FailedToShow: Error :" + error.ToString() + ", msg :" + message);
        }
        else if (placementId.Equals(rewardedAdUnit))
        {
            IsRewardedLoaded = false;
            if (isAdsAlwaysLoad) LoadRewardedAd();
            ShowDebugLog("Rewarded FailedToShow: Error :" + error.ToString() + ", msg :" + message);
        }
    }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(interstitialAdUnit))
        {
            IsInterstitialLoaded = false;
            if (isAdsAlwaysLoad) LoadInterstitial();
        }
        else if (placementId.Equals(rewardedAdUnit))
        {
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                GetReward();
            }
            IsRewardedLoaded = false;
            if (isAdsAlwaysLoad) LoadRewardedAd();
        }
    }

    #endregion

    #region Additional

    public bool isDebugLog = false;
    public void ShowDebugLog(string msg)
    {
        if (isDebugLog) Debug.Log("UnityAds Log: " + msg);
    }

    bool IsInternetConnection()
    {
        bool isInternet = false;
        try
        {
            isInternet = Application.internetReachability == NetworkReachability.NotReachable ? false : true;
        }
        catch (Exception) { }
        return isInternet;
    }

    bool IsLowTotalRAM()
    {
        if (PlayerPrefs.HasKey("IsLowTotalRAM"))
            return PlayerPrefs.GetInt("IsLowTotalRAM", 0) == 1 ? true : false;

        int totalMemory = SystemInfo.systemMemorySize;
        bool isLowRAM = false;
        switch (NoUnityAdsBelowTotalRAM)
        {
            case MemoryThreshold._512MB:
                {
                    if (totalMemory <= 512) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._1024MB:
                {
                    if (totalMemory <= 1024) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._1536MB:
                {
                    if (totalMemory <= 1536) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._2048MB:
                {
                    if (totalMemory <= 2048) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._3072MB:
                {
                    if (totalMemory <= 3072) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._4096MB:
                {
                    if (totalMemory <= 4096) isLowRAM = true;
                    break;
                }
        }
        PlayerPrefs.SetInt("IsLowTotalRAM", isLowRAM == true ? 1 : 0);
        return isLowRAM;
    }

    #region Memory Information

    readonly int memoryThreshHold = 400; //in MBs
    static int memoryAvailable = 0;
    static System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"\d+");

    // return True if memory low by defined threshHold
    public bool IsLowAvailableMemory(int threshold = -1)
    {
        #region IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor) return false;
        #endregion
        try
        {
            if (!isMemoryThreshold) return false;

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

    #endregion

    #endregion

    #region Editor Properties

#if UNITY_EDITOR
    [CustomEditor(typeof(AdsManager_Unity))]
    public class AdsIDsEditor : Editor
    {
        readonly string pluginVersion = "1.0.0";

        #region SerializedProperty

        SerializedProperty isDebugLog;
        SerializedProperty gameId;
        SerializedProperty interstitialAdUnit;
        SerializedProperty rewardedAdUnit;
        SerializedProperty isTestMode;
        SerializedProperty isInitilizedOnStart;
        SerializedProperty isAdsAlwaysLoad;

        SerializedProperty isMemoryThreshold;
        SerializedProperty initilizationRequiredMinValue;
        SerializedProperty interstitialRequiredMinValue;
        SerializedProperty rewardedRequiredMinValue;

        SerializedProperty NoUnityAdsBelowTotalRAM;

        #endregion

        void OnEnable()
        {
            isDebugLog = serializedObject.FindProperty(nameof(AdsManager_Unity.isDebugLog));
            gameId = serializedObject.FindProperty(nameof(AdsManager_Unity.gameId));
            interstitialAdUnit = serializedObject.FindProperty(nameof(AdsManager_Unity.interstitialAdUnit));
            rewardedAdUnit = serializedObject.FindProperty(nameof(AdsManager_Unity.rewardedAdUnit));
            isTestMode = serializedObject.FindProperty(nameof(AdsManager_Unity.isTestMode));
            isInitilizedOnStart = serializedObject.FindProperty(nameof(AdsManager_Unity.isInitilizedOnStart));
            isAdsAlwaysLoad = serializedObject.FindProperty(nameof(AdsManager_Unity.isAdsAlwaysLoad));

            isMemoryThreshold = serializedObject.FindProperty(nameof(AdsManager_Unity.isMemoryThreshold));
            initilizationRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_Unity.NoInitilizationBelow));
            interstitialRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_Unity.NoInterstitialBelow));
            rewardedRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_Unity.NoRewardedBelow));

            NoUnityAdsBelowTotalRAM = serializedObject.FindProperty(nameof(AdsManager_Unity.NoUnityAdsBelowTotalRAM));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUIStyle customStyle = new GUIStyle(EditorStyles.helpBox);
            customStyle.normal.textColor = Color.blue;
            customStyle.fontStyle = FontStyle.Bold;
            customStyle.fontSize = 12;

            EditorGUILayout.BeginVertical(customStyle);
            EditorGUILayout.LabelField("AD IDs", customStyle);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(gameId);
            EditorGUILayout.PropertyField(interstitialAdUnit);
            EditorGUILayout.PropertyField(rewardedAdUnit);
            EditorGUILayout.Space();

            isTestMode.boolValue = EditorGUILayout.Toggle("Test Mode", isTestMode.boolValue);
            if (isTestMode.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("Test Mode is Enable. Make Sure to Disable after Testing.", MessageType.Warning);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(customStyle);
            EditorGUILayout.LabelField("Other Settings", customStyle);
            EditorGUILayout.Space();

            isAdsAlwaysLoad.boolValue = EditorGUILayout.Toggle("Always Ads Loaded", isAdsAlwaysLoad.boolValue);
            if (!isAdsAlwaysLoad.boolValue)
            {
                isInitilizedOnStart.boolValue = EditorGUILayout.Toggle("Auto Initilized on Start", isInitilizedOnStart.boolValue);
                if (!isInitilizedOnStart.boolValue)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("Unity Ads not Initilized on Start. Either call yourself 'InitilizeSDK()' OR any 1st Loading Ad Request will initilize SDK.", MessageType.Warning);
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space();
            isDebugLog.boolValue = EditorGUILayout.Toggle("Debug Log", isDebugLog.boolValue);
            if (isDebugLog.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("Debugging Enable, Help you to debug every Request/Response via Debug.Log(msg), Make sure to Disable after Testing.", MessageType.Warning);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(customStyle);
            EditorGUILayout.LabelField("Memory Settings", customStyle);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(NoUnityAdsBelowTotalRAM);
            isMemoryThreshold.boolValue = EditorGUILayout.Toggle("Memory Threshold", isMemoryThreshold.boolValue);
            if (isMemoryThreshold.boolValue)
            {
                EditorGUILayout.PropertyField(initilizationRequiredMinValue);
                EditorGUILayout.PropertyField(interstitialRequiredMinValue);
                EditorGUILayout.PropertyField(rewardedRequiredMinValue);
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
            EditorGUILayout.LabelField("Applovin Plugin Version: " + pluginVersion, versionStyle);

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
            new FileStructure("Assets/Mobify/UnityAds/Script/","AdsManager_Unity",".cs")
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

        if (rewardedAdName == "freecoins")
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

    #endregion
}