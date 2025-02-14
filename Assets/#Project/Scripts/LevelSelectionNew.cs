using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class LevelSelectionNew : MonoBehaviour
{
    public delegate void LastLevel();
    public LastLevel lastLevel = null;
    public static LevelSelectionNew Instance;
    public GameObject sniperPopUp;
    public enum modeType
    {
        ASSAULT,
        SNIPER,
        MULTIPLAYER,
        FREEFORALL,
        BR,
        COVERSTRIKE
    }

    public GameObject storePanel;
    //ABDUL
    public GameObject assaultLock;
    public GameObject sniperLock;
    public GameObject coverStrikeLock;
    public GameObject FFALock;
    public GameObject FFASpecsError;
    public GameObject BRLock;
    public GameObject BRSpecsError;
    public GameObject multiplayerSpecsError;
    public GameObject multiplayerLock;
    public GameObject ratioObject;
    public GameObject headingsMain;
    public GameObject[] Headings;
    public Button playButton;
    public static modeType modeSelection = modeType.ASSAULT;
    public LevelSelectionSizeToggle[] toggleWindows;
    public Image fillImage;
    public Text ratioText;
    int currentRow = 0;
    public int currentUnlocked = 0;
    public string currentModePrefs = "", currentUnlockedPrefs = "";
    bool isMulti = false;

    private int count = 0;
    public GameObject unlockModePopup;

    public GameObject DetailsPanel;

    #region Modes Progress
    [Space(20)]
    [Header("Modes Progress")]
    [Space(20)]
    private string currentUnlockedAssaultPrefs = "";
    public Image assaultfillImage;
    public Text assaultratioText;
    // For Tutorial
    public Image tutorialAssaultfillImage;
    public Text tutorialAssaultratioText;

    private string currentUnlockedSniperPrefs = "";
    public Image sniperfillImage;
    public Text sniperratioText;

    private string currentUnlockedCStPrefs = "";
    public Image csfillImage;
    public Text csratioText;

    private string currentUnlockedTDMPrefs = "";
    public Image TDMfillImage;
    public Text TDMratioText;

    private string currentUnlockedFFAPrefs = "";
    public Image FFAfillImage;
    public Text FFAratioText;

    private string currentUnlockedRRPrefs = "";
    public Image BRfillImage;
    public Text BRratioText;
    #endregion

    #region Modes Lock Check
    public GameObject sniperLockImage;
    public GameObject csLockImage;
    public GameObject tdmLockImage;
    public GameObject ffaLockImage;
    public GameObject brLockImage;
    #endregion

    #region Modes Misc
    public GameObject ModesTopBar;
    public GameObject ModesPanel;
    public GameObject ModesDetailsPanel;
    public GameObject ModesLockedBG;
    #endregion


    public void OnEnable()
    {
#if UNITY_ANDROID
        if (!DetailsPanel.activeInHierarchy)
            Debug.Log("Debug : Mode Selection Screen");
#endif

#if UNITY_EDITOR
        Debug.Log("Mode init " + (int)modeSelection);
        print("Sniper Mode : " + PlayerPrefs.GetInt("Mode1")); 
#endif
        Instance = this;
        if(transform.name.Contains("Multi"))
        {
            isMulti = true;
            modeSelection = (modeType)PlayerPrefs.GetInt("MultiMode");
        } 
        else
        {
            isMulti = false;
            modeSelection = (modeType)PlayerPrefs.GetInt("CampMode");
        }
        //Debug.Log("Mode found " + modeSelection);

        Invoke("Initialize", 0.01f);
        Invoke("EnableAudioSound", 0.5f);

    }

    void Initialize()
    {
        try
        {
            // ModeChanged((int)modeSelection);

            #region checking mode unlocked
            //if ((int)modeSelection == 0) // Assault Mode
            //{
            //    ModeChanged((int)modeType.ASSAULT);
            //}

            //else if ((int)modeSelection == 1)   // Check if Sniper Mode is Unlocked
            //{
            //    if (PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1))
            //    {
            //        ModeChanged((int)modeType.SNIPER);
            //    }
            //    else
            //    {
            //        ModeChanged((int)modeType.ASSAULT);
            //    }
            //}

            //else if ((int)modeSelection == 5)   // Check if CoverStrike Mode is Unlocked
            //{
            //    if (PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-1").Equals(1))
            //    {
            //        ModeChanged((int)modeType.COVERSTRIKE);
            //    }
            //    else
            //    {
            //        ModeChanged((int)modeType.ASSAULT);
            //    }
            //}

            //if ((int)modeSelection == 2) // Multiplayer Mode
            //{
            //    ModeChanged((int)modeType.MULTIPLAYER);
            //}

            //else if ((int)modeSelection == 3)   // Check if FFA Mode is Unlocked
            //{
            //    if (PlayerPrefs.GetInt("Mode3") == 1)
            //    {
            //        ModeChanged((int)modeType.FREEFORALL);
            //    }
            //    else
            //    {
            //        ModeChanged((int)modeType.MULTIPLAYER);
            //    }
            //            }

            //            else if ((int)modeSelection == 4)   // Check if BR Mode is Unlocked
            //            {
            //                print("a");
            //                if (PlayerPrefs.GetInt("Mode4") == 1)
            //                {
            //                    print("b");
            //                    ModeChanged((int)modeType.BR);
            //                    print("c");
            //                }
            //                else
            //                {
            //                    print("d");
            //                    ModeChanged((int)modeType.MULTIPLAYER);
            //                    print("e");
            //                }
            //            }
            //#if UNITY_EDITOR
            //            Debug.Log("Mode init " + (int)modeSelection);
            //#endif
            #endregion
        }
        catch { }
    }

    private void Start()
    {
        #region Modes Progress
        // Assault Mode
        currentUnlockedAssaultPrefs = "levelUnlocked-";
        int countAssault = 0;
        for (int j = 1; j <= 80; j++)
        {
            if (PlayerPrefs.GetInt(currentUnlockedAssaultPrefs + j).Equals(1))
            {
                countAssault++;
            }
        }
        assaultratioText.text = (countAssault) + "/80";
        assaultfillImage.fillAmount = (countAssault) / 80f;
        // Fot Tutorial
        tutorialAssaultratioText.text = (countAssault) + "/80";
        tutorialAssaultfillImage.fillAmount = (countAssault) / 80f;

        // Sniper Mode
        currentUnlockedAssaultPrefs = "sniperLevelUnlocked-";
        int countSniper = 0;
        for (int j = 1; j <= 18; j++)
        {
            if (PlayerPrefs.GetInt(currentUnlockedAssaultPrefs + j).Equals(1))
            {
                countSniper++;
            }
        }
        sniperratioText.text = (countSniper) + "/18";
        sniperfillImage.fillAmount = (countSniper) / 18f;

        // CS Mode
        currentUnlockedAssaultPrefs = "CoverStrikeLevelUnlocked-";
        int countCS = 0;
        for (int j = 1; j <= 10; j++)
        {
            if (PlayerPrefs.GetInt(currentUnlockedAssaultPrefs + j).Equals(1))
            {
                countCS++;
            }
        }
        csratioText.text = (countCS) + "/10";
        csfillImage.fillAmount = (countCS) / 10f;

        // TDM Mode
        currentUnlockedAssaultPrefs = "MultiLevelUnlocked-";
        int countTDM = 0;
        for (int j = 1; j <= 40; j++)
        {
            if (PlayerPrefs.GetInt(currentUnlockedAssaultPrefs + j).Equals(1))
            {
                countTDM++;
            }
        }
        TDMratioText.text = (countTDM) + "/40";
        TDMfillImage.fillAmount = (countTDM) / 40f;

        // FFA Mode
        currentUnlockedAssaultPrefs = "FFALevelUnlocked-";
        int countFFA = 0;
        for (int j = 1; j <= 40; j++)
        {
            if (PlayerPrefs.GetInt(currentUnlockedAssaultPrefs + j).Equals(1))
            {
                countFFA++;
            }
        }
        FFAratioText.text = (countFFA) + "/40";
        FFAfillImage.fillAmount = (countFFA) / 40f;

        // BR Mode
        currentUnlockedAssaultPrefs = "BRLevelUnlocked-";
        int countBR = 0;
        for (int j = 1; j <= 40; j++)
        {
            if (PlayerPrefs.GetInt(currentUnlockedAssaultPrefs + j).Equals(1))
            {
                countBR++;
            }
        }
        BRratioText.text = (countBR) + "/40";
        BRfillImage.fillAmount = (countBR) / 40f;
        #endregion

        #region Modes Lock Check
        ModeLocksCheck();
        #endregion
    }

    // if we come back from loadoutscreen to mode selection , then turn audio volume to 1 after 1 second so , we can clearly hear the back button sound
    void EnableAudioSound()
    {
        AudioManager.instance.otherAudioSource.volume = SaveManager.Instance.state.soundFxVolume;
    }

       public void ModeLocksCheck()
        {
            if (!PlayerPrefs.GetInt("levelUnlocked-5").Equals(1) && !PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1) &&
                             !PlayerPrefs.GetInt("Mode1").Equals(1))
            {
                sniperLockImage.SetActive(true);
            }
            else
            {
                sniperLockImage.SetActive(false);
            }
            if ((PlayerPrefs.GetInt("LastLevel") >= 4 || PlayerPrefs.GetInt("Mode2").Equals(1)) && AutoQualityChooser.finalResult != 0)
            {
                tdmLockImage.SetActive(false);
            }
            else
            {
                tdmLockImage.SetActive(true);
            }
            if ((PlayerPrefs.GetInt("LastLevel") >= 6 || PlayerPrefs.GetInt("Mode3").Equals(1)) && AutoQualityChooser.finalResult != 0)
            {
                ffaLockImage.SetActive(false);
            }
            else
            {
                ffaLockImage.SetActive(true);
            }
            if ((PlayerPrefs.GetInt("LastLevel") >= 9 || PlayerPrefs.GetInt("Mode4").Equals(1)) && AutoQualityChooser.finalResult != 0)
            {
                brLockImage.SetActive(false);
            }
            else
            {
                brLockImage.SetActive(true);
            }
            if (!PlayerPrefs.GetInt("sniperLevelUnlocked-6").Equals(1) && !PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-1").Equals(1) &&
                    !PlayerPrefs.GetInt("Mode5").Equals(1))
            {
                csLockImage.SetActive(true);
            }
            else
            {
                csLockImage.SetActive(false);
            }
        }

    public void ModeChanged(int i)
    {
        try
        {

            AudioManager.instance.ModeChangeSound();
            if (isMulti)
                PlayerPrefs.SetInt("MultiMode", i);
            else
                PlayerPrefs.SetInt("CampMode", i);
            headingsMain.SetActive(false);
            Headings[(int)modeSelection].SetActive(false);
            if (toggleWindows[(int)modeSelection])
                toggleWindows[(int)modeSelection].gameObject.SetActive(false);
            modeSelection = (modeType)i;
            Headings[(int)modeSelection].SetActive(true);
            if (toggleWindows[(int)modeSelection])
                toggleWindows[(int)modeSelection].gameObject.SetActive(true);
            headingsMain.SetActive(true);
            switch (modeSelection)
            {
                case modeType.ASSAULT:
                    currentRow = (PlayerPrefs.GetInt("nextLevel") - 1) / 7;
                    currentUnlockedPrefs = "levelUnlocked-";
                    toggleWindows[(int)modeSelection].SetFocusTarget(currentRow);
                    InitAssaultLevel();
                    currentUnlocked = PlayerPrefs.GetInt("nextLevel");
                    //ratioText.text = (currentUnlocked - 1) + "/80";
                    //fillImage.fillAmount = (currentUnlocked - 1) / 80f;
                    count = 0;
                    for (int j = 1; j <= 80; j++)
                    {
                        if (PlayerPrefs.GetInt(currentUnlockedPrefs + j).Equals(1))
                        {
                            count++;
                        }
                    }
                    ratioText.text = (count) + "/80";
                    fillImage.fillAmount = (count) / 80f;
                    ratioObject.SetActive(true);
                    currentModePrefs = "currentLevel";
                    //Invoke("DelayedAssault", 2.5f);
                    break;
                case modeType.SNIPER:
                    InitSniperLevel();
                    currentUnlockedPrefs = "sniperLevelUnlocked-";
                    currentUnlocked = PlayerPrefs.GetInt("sniperNextLevel");
                    //ratioText.text = (currentUnlocked - 1) + "/10";
                    //fillImage.fillAmount = (currentUnlocked - 1) / 10f;
                    count = 0;
                    for (int j = 1; j <= 18; j++)
                    {
                        if (PlayerPrefs.GetInt(currentUnlockedPrefs + j).Equals(1))
                        {
                            count++;
                        }
                    }
                    ratioText.text = (count) + "/18";
                    fillImage.fillAmount = (count) / 18f;
                    //toggleWindows[i].Focus((PlayerPrefs.GetInt("nextLevel") - 1) / 5);
                    ratioObject.SetActive(true);
                    currentModePrefs = "sniperCurrentLevel";
                    break;
                case modeType.COVERSTRIKE:
                    InitCoverStrike();
                    currentUnlockedPrefs = "CoverStrikeLevelUnlocked-";
                    currentUnlocked = PlayerPrefs.GetInt("CoverStrikeNextLevel");
                    //ratioText.text = (currentUnlocked - 1) + "/10";
                    //fillImage.fillAmount = (currentUnlocked - 1) / 10f;
                    count = 0;
                    for (int j = 1; j <= 10; j++)
                    {
                        if (PlayerPrefs.GetInt(currentUnlockedPrefs + j).Equals(1))
                        {
                            count++;
                        }
                    }
                    ratioText.text = (count) + "/10";
                    fillImage.fillAmount = (count) / 10f;
                    //toggleWindows[i].Focus((PlayerPrefs.GetInt("nextLevel") - 1) / 5);
                    ratioObject.SetActive(true);
                    currentModePrefs = "CoverStrikeCurrentLevel";
                    break;
                case modeType.MULTIPLAYER:
                    //toggleWindows[i].Focus((PlayerPrefs.GetInt("nextLevel") - 1) / 5);
                    InitMultiplayer();
                    currentUnlockedPrefs = "MultiLevelUnlocked-";
                    currentUnlocked = PlayerPrefs.GetInt("MultiNextLevel");
                    count = 0;
                    for (int j = 1; j <= 40; j++)
                    {
                        if (PlayerPrefs.GetInt(currentUnlockedPrefs + j).Equals(1))
                        {
                            count++;
                        }
                    }
                    ratioText.text = (count) + "/40";
                    fillImage.fillAmount = (count) / 40f;
                    ratioObject.SetActive(true);
                    currentModePrefs = "MultiCurrentLevel";
                    break;
                case modeType.FREEFORALL:
                    InitFFA();
                    currentUnlockedPrefs = "FFALevelUnlocked-";
                    currentUnlocked = PlayerPrefs.GetInt("FFANextLevel");
                    count = 0;
                    for (int j = 1; j <= 40; j++)
                    {
                        if (PlayerPrefs.GetInt(currentUnlockedPrefs + j).Equals(1))
                        {
                            count++;
                        }
                    }
                    ratioText.text = (count) + "/40";
                    fillImage.fillAmount = (count) / 40;
                    ratioObject.SetActive(true);
                    currentModePrefs = "FFACurrentLevel";
                    break;
                case modeType.BR:
                    InitBR();
                    currentUnlockedPrefs = "BRLevelUnlocked-";
                    currentUnlocked = PlayerPrefs.GetInt("BRNextLevel");
                    count = 0;
                    for (int j = 1; j <= 40; j++)
                    {
                        if (PlayerPrefs.GetInt(currentUnlockedPrefs + j).Equals(1))
                        {
                            count++;
                        }
                    }
                    ratioText.text = (count) + "/40";
                    fillImage.fillAmount = (count) / 40;
                    ratioObject.SetActive(true);
                    currentModePrefs = "BRCurrentLevel";
                    break;
                default:
                    playButton.interactable = false;
                    ratioObject.SetActive(false);
                    //toggleWindows[(int)modeSelection].SetFocusTarget(currentRow);
                    break;
            }
            toggleWindows[(int)modeSelection].OnClick();
        }
        catch { }
    }

    public void HideSniperPopUp()
    {
        PlayerPrefs.SetInt("weapon19", 1);
        PlayerPrefs.SetInt("SniperEquipped", 19);
        PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") - 0);
        ConstantUpdate.Instance.UpdateCurrency();
        sniperPopUp.SetActive(false);
        PlayerPrefs.SetInt("HasSeenSniper", 1);
    }
    private void InitSniperLevel()
    {
        try
        {
            if (!PlayerPrefs.GetInt("levelUnlocked-5").Equals(1) && !PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1) &&
                !PlayerPrefs.GetInt("Mode" + (int)LevelSelectionNew.modeSelection).Equals(1))
            {
                sniperLock.SetActive(true);
                ModesLockedBG.SetActive(true);
                playButton.interactable = false;
                unlockModePopup.SetActive(true);
                ModesTopBar.SetActive(true);
                ModesPanel.SetActive(true);
                ModesDetailsPanel.SetActive(false);
            }
            else
            {
                //if (PlayerPrefs.GetInt("HasSeenSniper") != 1)
                //{
                //    sniperPopUp.SetActive(true);
                //}
                PlayerPrefs.SetInt("sniperCurrentLevel", PlayerPrefs.GetInt("sniperNextLevel"));
                sniperLock.SetActive(false);
                ModesLockedBG.SetActive(false);
                playButton.interactable = true;
                ModesTopBar.SetActive(false);
                ModesPanel.SetActive(false);
                ModesDetailsPanel.SetActive(true);
#if UNITY_ANDROID
                Debug.Log("Debug : Sniper Level Selection Screen");
#endif
                //sniperLevelError.SetActive(true);
            }
            //else if (!PlayerPrefs.GetInt("sniperLevelUnlocked-10").Equals(1))
            //{
            //    //playButton.interactable = true;
            //    sniperLock.SetActive(false);
            //    if (!PlayerPrefs.GetInt("weapon19").Equals(1))
            //    {
            //        PlayerPrefs.SetInt("weapon19", 1);
            //        PlayerPrefs.SetInt("SniperEquipped", 19);
            //    }
            //}
            //else
            //{
            //    sniperLevelError.SetActive(false);
            //    sniperCompletedNotification.SetActive(true);
            //    playButton.gameObject.SetActive(false);
            //    sniperLock.SetActive(true);
            //}
        }
        catch { }
    }
    private void InitAssaultLevel()
    {
        try
        {
            if (!PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-3").Equals(1) && !PlayerPrefs.GetInt("levelUnlocked-1").Equals(1))
            {
                assaultLock.SetActive(true);
                ModesLockedBG.SetActive(true);
                playButton.interactable = false;
                ModesTopBar.SetActive(false);
                ModesPanel.SetActive(false);
                ModesDetailsPanel.SetActive(true);
            }
            else
            {
                assaultLock.SetActive(false);
                ModesLockedBG.SetActive(false);
                PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("nextLevel"));
                playButton.gameObject.SetActive(true);
                playButton.interactable = true;
                ModesTopBar.SetActive(false);
                ModesPanel.SetActive(false);
                ModesDetailsPanel.SetActive(true);
#if UNITY_ANDROID
                Debug.Log("Debug : Assault Level Selection Screen");
#endif
            }

            assaultLock.SetActive(false);
            ModesLockedBG.SetActive(false);
            playButton.interactable = true;
            PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("nextLevel"));

        }
        catch { }
    }
    private void InitBR()
    {
        try
        {
            if (AutoQualityChooser.finalResult == 0)
            {
                BRSpecsError.SetActive(true);
                playButton.interactable = false;
                BRLock.SetActive(false);
                ModesLockedBG.SetActive(false);
                return;
            }                         //3
            if (PlayerPrefs.GetInt("LastLevel") >= 9 || PlayerPrefs.GetInt("Mode" + (int)LevelSelectionNew.modeSelection).Equals(1))
            {
                PlayerPrefs.SetInt("selectedWeaponIndex", PlayerPrefs.GetInt("AssaultEquipped"));
                playButton.gameObject.SetActive(true);
                playButton.interactable = true;
                BRLock.SetActive(false);
                ModesLockedBG.SetActive(false);
                BRSpecsError.SetActive(false);
                ModesTopBar.SetActive(false);
                ModesPanel.SetActive(false);
                ModesDetailsPanel.SetActive(true);
#if UNITY_ANDROID
                Debug.Log("Debug : BR Level Selection Screen");
#endif

            }
            else
            {
                playButton.interactable = false;
                BRLock.SetActive(true);
                ModesLockedBG.SetActive(true);
                BRSpecsError.SetActive(false);
                unlockModePopup.SetActive(true);
                ModesTopBar.SetActive(true);
                ModesPanel.SetActive(true);
                ModesDetailsPanel.SetActive(false);
            }
        }
        catch { }
    }
    private void InitFFA()
    {
        try
        {
            if (AutoQualityChooser.finalResult == 0)
            {
                FFASpecsError.SetActive(true);
                playButton.interactable = false;
                FFALock.SetActive(false);
                ModesLockedBG.SetActive(false);
                return;
            }                         //3
            if (PlayerPrefs.GetInt("LastLevel") >= 6 || PlayerPrefs.GetInt("Mode" + (int)LevelSelectionNew.modeSelection).Equals(1))
            {
                PlayerPrefs.SetInt("selectedWeaponIndex", PlayerPrefs.GetInt("AssaultEquipped"));
                playButton.gameObject.SetActive(true);
                playButton.interactable = true;
                FFALock.SetActive(false);
                ModesLockedBG.SetActive(false);
                FFASpecsError.SetActive(false);
                ModesTopBar.SetActive(false);
                ModesPanel.SetActive(false);
                ModesDetailsPanel.SetActive(true);
#if UNITY_ANDROID
                Debug.Log("Debug : FFA Level Selection Screen");
#endif

            }
            else
            {
                playButton.interactable = false;
                FFALock.SetActive(true);
                ModesLockedBG.SetActive(true);
                FFASpecsError.SetActive(false);
                unlockModePopup.SetActive(true);
                ModesTopBar.SetActive(true);
                ModesPanel.SetActive(true);
                ModesDetailsPanel.SetActive(false);
            }
        }
        catch { }
    }
    public void InitCoverStrike()
    {
        try
        {
            if (!PlayerPrefs.GetInt("sniperLevelUnlocked-6").Equals(1) && !PlayerPrefs.GetInt("CoverStrikeLevelUnlocked-1").Equals(1) &&
                !PlayerPrefs.GetInt("Mode" + (int)LevelSelectionNew.modeSelection).Equals(1))
            {
                coverStrikeLock.SetActive(true);
                ModesLockedBG.SetActive(true);
                playButton.interactable = false;
                unlockModePopup.SetActive(true);
                ModesTopBar.SetActive(true);
                ModesPanel.SetActive(true);
                ModesDetailsPanel.SetActive(false);
            }
            else
            {
                //if (PlayerPrefs.GetInt("HasSeenSniper") != 1)
                //{
                //    sniperPopUp.SetActive(true);
                //    PlayerPrefs.SetInt("HasSeenSniper", 1);
                //    PlayerPrefs.SetInt("weapon19", 1);
                //    PlayerPrefs.SetInt("SniperEquipped", 19);
                //}
                coverStrikeLock.SetActive(false);
                ModesLockedBG.SetActive(false);
                playButton.interactable = true;
                PlayerPrefs.SetInt("CoverStrikeCurrentLevel", PlayerPrefs.GetInt("CoverStrikeNextLevel"));
                ModesTopBar.SetActive(false);
                ModesPanel.SetActive(false);
                ModesDetailsPanel.SetActive(true);
#if UNITY_ANDROID
                Debug.Log("Debug : CoverStrike Level Selection Screen");
#endif
                //sniperLevelError.SetActive(true);
            }
        }
        catch { }
    }
    private void InitMultiplayer()
    {
        try
        {
            if (AutoQualityChooser.finalResult == 0)
            {
                multiplayerSpecsError.SetActive(true);
                playButton.interactable = false;
                multiplayerLock.SetActive(false);
                ModesLockedBG.SetActive(false);
                return;
            }//3
            if (PlayerPrefs.GetInt("LastLevel") >= 4 || PlayerPrefs.GetInt("Mode" + (int)LevelSelectionNew.modeSelection).Equals(1))
            {
                PlayerPrefs.SetInt("selectedWeaponIndex", PlayerPrefs.GetInt("AssaultEquipped"));
                playButton.gameObject.SetActive(true);
                playButton.interactable = true;
                multiplayerLock.SetActive(false);
                ModesLockedBG.SetActive(false);
                multiplayerSpecsError.SetActive(false);

                // if multiplayer mode unlocks by  user
                PlayerPrefs.SetInt("MultiplayerMode", 1);
                PlayerPrefs.SetInt("currentMode", 1);
                ModesTopBar.SetActive(false);
                ModesPanel.SetActive(false);
                ModesDetailsPanel.SetActive(true);
#if UNITY_ANDROID
                Debug.Log("Debug : TDM Level Selection Screen");
#endif
            }
            else
            {
                playButton.interactable = false;
                multiplayerLock.SetActive(true);
                ModesLockedBG.SetActive(true);
                multiplayerSpecsError.SetActive(false);
                unlockModePopup.SetActive(true);
                ModesTopBar.SetActive(true);
                ModesPanel.SetActive(true);
                ModesDetailsPanel.SetActive(false);
            }
        }
        catch { }
    }
    public void _CloseLevelSelectionButton()
    {
        try
        {
            AudioManager.instance.NormalClick();
            if (AudioManager.instance !=null)
            {
                AudioManager.instance.PlayMainMenu();
            }
            AudioManager.instance.BackClickNew();
            MainMenuManager.Instance.OpenMainMenu();
        }
        catch { }
    }

    public void _PlusButton()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : Mode Screen To Store");
#endif
        try
        {
            AudioManager.instance.NormalClick2();
            storePanel.SetActive(true);
            storePanel.GetComponent<NewStoreManager>()._SPButton();
            WeaponStore.Instance.StorePanel.SetActive(true);
        }
        catch { }

    }
    public void _PlayButton()
    {
        try
        {
            AudioManager.instance.NormalClick();
            MainMenuManager.Instance.StartLoading();
        }
        catch { }
    }

    public void _BackToModeSelection()
    {
       // AdsManager.instance.ShowBothInterstitial();
#if UNITY_ANDROID
        Debug.Log("Debug : " + modeSelection + " LevelSelection To ModeSelection");
#endif
    }

    public void OnLevelSelect(int i)
    {
        try
        {
            currentRow = i / 5;
            if (PlayerPrefs.GetInt(currentUnlockedPrefs + i).Equals(1))//if(currentUnlocked/* - 1*/ > i)//ABDUL
            {
                playButton.interactable = true;
                PlayerPrefs.SetInt(currentModePrefs, (i + 1));
            }
            else
            {
                playButton.interactable = false;
            }
            if (modeSelection == modeType.ASSAULT)
                toggleWindows[(int)modeSelection].Focus(currentRow);

        }
        catch { }
    }

    public void ModeTutorialEvent()
    {
        Analytics.CustomEvent("Mode_Tut", new Dictionary<string, object>//Mode Selection
        {
            { "level_index", 1 }
        });
    }

    private void OnDisable()
    {
        CancelInvoke("Initialize");
        CancelInvoke("EnableAudioSound");
    }
}
