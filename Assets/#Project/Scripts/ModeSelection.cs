using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelection : MonoBehaviour
{
    //public enum modeType
    //{
    //    ASSAULT,
    //    SNIPER,
    //    MULTIPLAYER,
    //    FREEFORALL
    //}
    [Serializable]
    public class DescriptionsContainer
    {
        public string[] LocalizedDesc;
    }
    //public static modeType modeSelection = modeType.ASSAULT;
    public LevelSelection _levelSelection;

    public GameObject
        mainMenuPanel,
        storePanel,
        modeSelectionPanel,
        loadingPanel;

    public GameObject
        sniperModeDetails,
        assaultModeDetails,
        //comingSoon,
        multiplayerModeDetails,
        noInternet,
        ramCheck,
        sniperLevelError,
        sniperCompletedNotification,
        multiPlayerButton;

    //public GameObject giftPanel;
    bool isFirst = true;
    //public Text
    //    weaponNameText,
    //    explosive_G65Text,
    //    adrenaline_25Text;

    public Button playButton, playAgainButton;

    // For Tutorial
    public GameObject tutorialPanel, highlightImage;

    //Assault
    public GameObject assaultLock;

    //Sniper
    public GameObject sniperLock;
    public Slider sniperSlider;
    public Text descriptionText, sniperLevelText;
    public Text sniperXpRewardText, sniperSPRewardText, sniperGoldRewardText;
    public int[] sniperTotalEnemies = new int[10];
    public string[] descriptions = new string[10];
    public DescriptionsContainer[] LocalDesc;

    //Multiplayer
    public GameObject multiplayerLock;
    public Text multiplayerText;

    public GameObject[] selectedImages;
    //public GameObject[] weapons;
    //public ScriptableObject[] scriptableWeapons;

    //Audio
    public AudioClip mainMenu, gameplay_3;

    private void OnEnable()
    {
        isFirst = true;
        multiPlayerButton.SetActive(true);
        //if (SystemInfo.systemMemorySize >= 2800)
        //{
        //    multiPlayerButton.SetActive(true);
        //}
        //else
        //    multiPlayerButton.SetActive(false);
        if (PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1) || PlayerPrefs.GetInt("levelUnlocked-1").Equals(1))
        {
            tutorialPanel.SetActive(false);
            highlightImage.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("ResetSniperMode").Equals(1))
        {
            tutorialPanel.SetActive(false);
            highlightImage.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Mode").Equals(0))
        {
            _SniperModeButton();
        }
        else if (PlayerPrefs.GetInt("Mode").Equals(1) )
        {
            _AssaultModeButton();
        }
        else if(PlayerPrefs.GetInt("Mode").Equals(2))
        {
            _MultiplayerModeButton();
        }
        //sniperLevelUnlocked-5"
        //if (PlayerPrefs.GetInt("sniperLevelUnlocked-5").Equals(1) && PlayerPrefs.GetInt("OnceAssault") != 1)
        //{
        //    PlayerPrefs.SetInt("OnceAssault", 1);
        //    _AssaultModeButton();
        //}
        //explosive_G65Text.text = System.String.Empty + PlayerPrefs.GetInt("Grenade");
        //adrenaline_25Text.text = System.String.Empty + PlayerPrefs.GetInt("Injection");
        //HideAllWeapons();
        //weaponNameText.text = System.String.Empty + scriptableWeapons[7].name;
        //weaponNameText.text = System.String.Empty + PlayerPrefs.GetString("WeaponName");
        //weapons[PlayerPrefs.GetInt("selectedWeaponIndex")].SetActive(true);

        if (AudioManager.instance !=null && AudioManager.instance.backgroundMusicSouce.clip != gameplay_3)
        {
            AudioManager.instance.backgroundMusicSouce.clip = gameplay_3;
            AudioManager.instance.backgroundMusicSouce.Play();
        }
    }

    private void InitSniperLevel()
    {
        //if (PlayerPrefs.GetInt("sniperNextLevel") == 11)
        //{
        //    sniperLevelsCompletedPanel.SetActive(true);
        //    return;
        //}
        PlayerPrefs.SetInt(" ", PlayerPrefs.GetInt("sniperNextLevel"));
        //if (!PlayerPrefs.GetInt("sniperLevelUnlocked-10").Equals(1))
        //{
        //    playButton.interactable = true;
        //    sniperLock.SetActive(false);
        //    int passLevel = 0;
        //    float pass = PlayerPrefs.GetFloat("Pass_Level");
        //    switch (pass)
        //    {
        //        case 1.5f:
        //            passLevel = 1;
        //            break;
        //        case 2f:
        //            passLevel = 2;
        //            break;
        //        case 3f:
        //            passLevel = 3;
        //            break;
        //        default:
        //            passLevel = 1;
        //            break;
        //    }
        //    int xp = (sniperTotalEnemies[PlayerPrefs.GetInt("sniperCurrentLevel") - 1] * 55) + 150;
        //    xp *= passLevel;
        //    int sp = xp / 20;
        //    int gold = xp / 4;

        //    sniperXpRewardText.text = System.String.Empty + xp;
        //    sniperSPRewardText.text = System.String.Empty + sp;
        //    sniperGoldRewardText.text = System.String.Empty + gold;

        //    descriptionText.text = descriptions[PlayerPrefs.GetInt("sniperCurrentLevel") - 1];
        //    sniperSlider.value = PlayerPrefs.GetInt("sniperCurrentLevel") - 1;
        //    sniperLevelText.text = (PlayerPrefs.GetInt("sniperCurrentLevel") - 1) + "/10";

        //    for (int i = 6; i < 10; i++)
        //    {
        //        if (PlayerPrefs.GetInt("weaponEquipped-" + i).Equals(1))
        //        {
        //            PlayerPrefs.SetInt("selectedWeaponIndex", i);
        //            HideAllWeapons();
        //            weaponNameText.text = System.String.Empty + scriptableWeapons[i].name;
        //            weapons[i].SetActive(true);
        //        }
        //    }
        //}
        //else
        //{
        //    playAgainButton.gameObject.SetActive(true);
        //    sniperLock.SetActive(true);
        //    sniperXpRewardText.text = System.String.Empty + 0;
        //    sniperSPRewardText.text = System.String.Empty + 0;
        //    sniperGoldRewardText.text = System.String.Empty + 0;

        //    descriptionText.text = "Coming Soon...";
        //    sniperSlider.value = 10;
        //    sniperLevelText.text = 10 + "/10";
        //}
                                               //5
        if (!PlayerPrefs.GetInt("levelUnlocked-5").Equals(1) && !PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1))
        {
            sniperLock.SetActive(true);
            playButton.interactable = false;
            sniperLevelError.SetActive(true);
            sniperCompletedNotification.SetActive(false);
        }
        else if (!PlayerPrefs.GetInt("sniperLevelUnlocked-10").Equals(1))
        {
            //playButton.interactable = true;
            sniperLock.SetActive(false);
            int passLevel = 0;
            float pass = PlayerPrefs.GetFloat("Pass_Level");
            switch (pass)
            {
                case 1.5f:
                    passLevel = 1;
                    break;
                case 2f:
                    passLevel = 2;
                    break;
                case 3f:
                    passLevel = 3;
                    break;
                default:
                    passLevel = 1;
                    break;
            }
            int xp = (sniperTotalEnemies[PlayerPrefs.GetInt("sniperCurrentLevel") - 1] * 55) + 150;
            xp *= passLevel;
            int sp = xp / 20;
            int gold = xp / 4;
            playButton.interactable = true;
            sniperXpRewardText.text = System.String.Empty + xp;
            sniperSPRewardText.text = System.String.Empty + sp;
            sniperGoldRewardText.text = System.String.Empty + gold;

            descriptionText.text = descriptions[PlayerPrefs.GetInt("sniperCurrentLevel") - 1];
            sniperSlider.value = PlayerPrefs.GetInt("sniperCurrentLevel") - 1;
            sniperLevelText.text = (PlayerPrefs.GetInt("sniperCurrentLevel") - 1) + "/10";

            if (!PlayerPrefs.GetInt("weapon19").Equals(1))
            {
                PlayerPrefs.SetInt("weapon19", 1);
                PlayerPrefs.SetInt("SniperEquipped", 19);
            }
        }
        else
        {
            sniperLevelError.SetActive(false);
            sniperCompletedNotification.SetActive(true);
            playButton.gameObject.SetActive(false);
            //playAgainButton.gameObject.SetActive(true);
            sniperLock.SetActive(true);
            sniperXpRewardText.text = System.String.Empty + 0;
            sniperSPRewardText.text = System.String.Empty + 0;
            sniperGoldRewardText.text = System.String.Empty + 0;

            descriptionText.text = "Coming Soon...";
            sniperSlider.value = 10;
            sniperLevelText.text = 10 + "/10";
        }
    }

    private void InitAssaultLevel()
    {
        _levelSelection.enabled = true;
        playAgainButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        playButton.interactable = true;
        assaultLock.SetActive(false);
        ////5
        //if (PlayerPrefs.GetInt("sniperLevelUnlocked-5").Equals(1) && !PlayerPrefs.GetInt("levelUnlocked-1").Equals(1))
        //{
        //    _levelSelection.enabled = true;
        //    playAgainButton.gameObject.SetActive(false);
        //    playButton.interactable = true;
        //    assaultLock.SetActive(false);

        //    if (!PlayerPrefs.GetInt("weapon0").Equals(1))
        //    {
        //        giftPanel.SetActive(true);
        //    }
        //    else
        //    {
        //        //for (int i = 0; i < 6; i++)
        //        //{
        //            //if (PlayerPrefs.GetInt("weaponEquipped-" + i).Equals(1))
        //            //{
        //                PlayerPrefs.SetInt("selectedWeaponIndex", PlayerPrefs.GetInt("AssaultEquipped"));
        //                HideAllWeapons();
        //                //weaponNameText.text = System.String.Empty + scriptableWeapons[PlayerPrefs.GetInt("AssaultEquipped")].name;
        //                //weapons[PlayerPrefs.GetInt("AssaultEquipped")].SetActive(true);
        //        //    }
        //        //}
        //    }
        //}
        //else if (!PlayerPrefs.GetInt("sniperLevelUnlocked-5").Equals(1) && PlayerPrefs.GetInt("levelUnlocked-1").Equals(1))// For Old Users...
        //{
        //    _levelSelection.enabled = true;
        //    playAgainButton.gameObject.SetActive(false);
        //    playButton.interactable = true;
        //    assaultLock.SetActive(false);
        //    PlayerPrefs.SetInt("selectedWeaponIndex", PlayerPrefs.GetInt("AssaultEquipped"));
        //    //for (int i = 0; i < 6; i++)
        //    //{
        //    //    if (PlayerPrefs.GetInt("weaponEquipped-" + i).Equals(1))
        //    //    {
        //    //        PlayerPrefs.SetInt("selectedWeaponIndex", i);
        //    //        HideAllWeapons();
        //    //        weaponNameText.text = System.String.Empty + scriptableWeapons[i].name;
        //    //        weapons[i].SetActive(true);
        //    //    }
        //    //}
        //}
        //else if (PlayerPrefs.GetInt("sniperLevelUnlocked-5").Equals(1) && PlayerPrefs.GetInt("levelUnlocked-1").Equals(1))
        //{
        //    _levelSelection.enabled = true;
        //    playAgainButton.gameObject.SetActive(false);
        //    playButton.interactable = true;
        //    assaultLock.SetActive(false);
        //    PlayerPrefs.SetInt("selectedWeaponIndex", PlayerPrefs.GetInt("AssaultEquipped"));
        //    //for (int i = 0; i < 6; i++)
        //    //{
        //    //    if (PlayerPrefs.GetInt("weaponEquipped-" + i).Equals(1))
        //    //    {
        //    //        PlayerPrefs.SetInt("selectedWeaponIndex", i);
        //    //        HideAllWeapons();
        //    //        weaponNameText.text = System.String.Empty + scriptableWeapons[i].name;
        //    //        weapons[i].SetActive(true);
        //    //    }
        //    //}
        //}
        //else if (PlayerPrefs.GetInt("ResetSniperMode").Equals(1))
        //{
        //    _levelSelection.enabled = true;
        //    playAgainButton.gameObject.SetActive(false);
        //    playButton.interactable = true;
        //    assaultLock.SetActive(false);

        //    if (!PlayerPrefs.GetInt("weapon0").Equals(1))
        //    {
        //        giftPanel.SetActive(true);
        //    }
        //    else
        //    {
        //        PlayerPrefs.SetInt("selectedWeaponIndex", PlayerPrefs.GetInt("AssaultEquipped"));
        //        //for (int i = 0; i < 6; i++)
        //        //{
        //        //    if (PlayerPrefs.GetInt("weaponEquipped-" + i).Equals(1))
        //        //    {
        //        //        PlayerPrefs.SetInt("selectedWeaponIndex", i);
        //        //        HideAllWeapons();
        //        //        weaponNameText.text = System.String.Empty + scriptableWeapons[i].name;
        //        //        weapons[i].SetActive(true);
        //        //    }
        //        //}
        //    }
        //}
        //else
        //{
        //    _levelSelection.enabled = false;
        //    playButton.interactable = false;
        //    assaultLock.SetActive(true);
        //}
    }

    private void InitMultiplayer()
    {                               //5
        if(PlayerPrefs.GetInt("LastLevel") >= 0)
        {
            PlayerPrefs.SetInt("selectedWeaponIndex", PlayerPrefs.GetInt("AssaultEquipped"));
            //for (int i = 0; i < 6; i++)
            //{
            //    if (PlayerPrefs.GetInt("weaponEquipped-" + i).Equals(1))
            //    {
            //        PlayerPrefs.SetInt("selectedWeaponIndex", i);
            //        HideAllWeapons();
            //        weaponNameText.text = System.String.Empty + scriptableWeapons[i].name;
            //        weapons[i].SetActive(true);
            //    }
            //}

            playAgainButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
            playButton.interactable = true;
            multiplayerLock.SetActive(false);
        }
        else
        {
            //multiplayerText.text = "Multiplayer Mode will Unlock At <color=orange>Player Level 5</color>";
            playButton.interactable = false;
            multiplayerLock.SetActive(true);
        }
        //if (!PlayerPrefs.GetInt("sniperLevelUnlocked-5").Equals(1) && PlayerPrefs.GetInt("levelUnlocked-5").Equals(1))
        //    multiplayerText.text = "First, Complete <color=orange>5</color> Sniper Missions";
        //else if(PlayerPrefs.GetInt("sniperLevelUnlocked-5").Equals(1) && !PlayerPrefs.GetInt("levelUnlocked-5").Equals(1))
        //    multiplayerText.text = "First, Complete <color=orange>5</color> Assault Missions";
        //else
        //    multiplayerText.text = "First, Complete <color=orange>5</color> Assault Missions";

        //if (PlayerPrefs.GetInt("sniperLevelUnlocked-5").Equals(1) && PlayerPrefs.GetInt("levelUnlocked-5").Equals(1))
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        if (PlayerPrefs.GetInt("weaponEquipped-" + i).Equals(1))
        //        {
        //            PlayerPrefs.SetInt("selectedWeaponIndex", i);
        //            HideAllWeapons();
        //            weaponNameText.text = System.String.Empty + scriptableWeapons[i].name;
        //            weapons[i].SetActive(true);
        //        }
        //    }

        //    playButton.interactable = true;
        //    multiplayerLock.SetActive(false);
        //}
        //else
        //{
        //    playButton.interactable = false;
        //    multiplayerLock.SetActive(true);
        //}
    }

    private void EnablePanel(GameObject panelToShow)
    {
        mainMenuPanel.SetActive(false);
        storePanel.SetActive(false);
        modeSelectionPanel.SetActive(false);
        loadingPanel.SetActive(false);

        panelToShow.SetActive(true);
    }

    private void EnableDetailPanel(GameObject panelToShow)
    {
        sniperModeDetails.SetActive(false);
        assaultModeDetails.SetActive(false);
        //comingSoon.SetActive(false);
        multiplayerModeDetails.SetActive(false);
        noInternet.SetActive(false);
        ramCheck.SetActive(false);

        panelToShow.SetActive(true);
    }

    private void EnableSelectedImage(int index)
    {
        foreach (GameObject go in selectedImages)
        {
            go.SetActive(false);
        }

        selectedImages[index].SetActive(true);
    }

    //private void HideAllWeapons()
    //{
    //    foreach (GameObject go in weapons)
    //    {
    //        go.SetActive(false);
    //    }
    //}

    #region Button Methods
    public void _SniperModeButton()
    {
        if (!isFirst)
        {
            AudioManager.instance.NormalClick();
        }
        else
            isFirst = false;
        //modeSelection = LevelSelectionNew.modeType.SNIPER;
       // PlayerPrefs.SetInt("Mode", (int)modeSelection);
        InitSniperLevel();
        EnableSelectedImage(0);
        EnableDetailPanel(sniperModeDetails);
    }

    public void _AssaultModeButton()
    {
        if (!isFirst)
        {
            AudioManager.instance.NormalClick();
        }
        else
            isFirst = false;
        //modeSelection = LevelSelectionNew.modeType.ASSAULT;
        //PlayerPrefs.SetInt("Mode", (int)modeSelection);
        InitAssaultLevel();
        
        EnableSelectedImage(1);
        EnableDetailPanel(assaultModeDetails);
    }

    public void _MultiplayerModeButton()
    {
        if (!isFirst)
        {
            AudioManager.instance.NormalClick();
        }
        else
            isFirst = false;

        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    EnableSelectedImage(2);
        //    EnableDetailPanel(noInternet);
        //    playButton.interactable = false;
        //}
        //else if (SystemInfo.systemMemorySize >= 2800)
        //{
            //modeSelection = LevelSelectionNew.modeType.MULTIPLAYER;
            //PlayerPrefs.SetInt("Mode", (int)modeSelection);
            InitMultiplayer();
            EnableSelectedImage(2);
            EnableDetailPanel(multiplayerModeDetails);
            //if (SystemInfo.deviceModel.Contains("SAMSUNG") || SystemInfo.deviceModel.Contains("samsung")
            //   || SystemInfo.deviceModel.Contains("HUAWEI") || SystemInfo.deviceModel.Contains("huawei"))
            //{
            //    modeSelection = 2;
            //    PlayerPrefs.SetInt("Mode", modeSelection);
            //    InitMultiplayer();
            //    EnableSelectedImage(2);
            //    EnableDetailPanel(multiplayerModeDetails);
            //}
       // }
        //else
        //{
        //    EnableSelectedImage(2);
        //    EnableDetailPanel(ramCheck);
        //    playButton.interactable = false;
        //}
    }

    //public void _SecretModeButton()
    //{
    //    if (!isFirst)
    //    {
    //        AudioManager.instance.NormalClick();
    //    }
    //    else
    //        isFirst = false;
    //    playButton.interactable = false;
    //    EnableSelectedImage(3);
    //    EnableDetailPanel(comingSoon);
    //}

    //public void _DailyModeButton()
    //{
    //    if (!isFirst)
    //    {
    //        AudioManager.instance.NormalClick();
    //    }
    //    else
    //        isFirst = false;
    //    playButton.interactable = false;
    //    EnableSelectedImage(4);
    //    EnableDetailPanel(comingSoon);
    //}
    public void _PlayButton()
    {
        //if(modeSelection == modeType.MULTIPLAYER)// Multiplayer
        //{
        //    //PlayerPrefs.SetInt("Mode", modeSelection);
        //    //AudioManager.instance.LetsGoButtonClick();
        //    _multiPlayerMenu.OnMultiPlayer();
        //}
        //else
        //{
        //    //PlayerPrefs.SetInt("Mode", modeSelection);
        //    //AudioManager.instance.LetsGoButtonClick();
        //    EnablePanel(loadingPanel);
        //}
    }
    public void _CloseLevelSelectionButton()
    {
        if (AudioManager.instance !=null)
        {
            AudioManager.instance.backgroundMusicSouce.clip = mainMenu;
            AudioManager.instance.backgroundMusicSouce.Play();
        }
        AudioManager.instance.BackClickNew();
        EnablePanel(mainMenuPanel);
    }
    public void _PlusButton()
    {
        storePanel.SetActive(true);
        storePanel.GetComponent<NewStoreManager>()._SPButton();
    }

    //public void _EquipButton()
    //{
    //    AudioManager.instance.NormalClick();
    //    PlayerPrefs.SetInt("weapon0", 1);
    //    PlayerPrefs.SetInt("weapon0", 1);
    //    PlayerPrefs.SetString("WeaponName", "MP-52");
    //    PlayerPrefs.SetInt("selectedWeaponIndex", 0);
    //    PlayerPrefs.SetInt("AssaultEquipped", 0);

    //    giftPanel.SetActive(false);
    //    PlayerPrefs.SetInt("Mode", 1);
    //    OnEnable();
    //}

    public void _ResetSniperMissions()
    {
        AudioManager.instance.NormalClick();
        for (int i = 0; i <= 10; i++)
        {
            PlayerPrefs.SetInt("sniperLevelUnlocked-" + i, 0);
        }

        PlayerPrefs.SetInt("sniperLevelUnlocked-0", 1);
        PlayerPrefs.SetInt("sniperCurrentLevel", 1);
        PlayerPrefs.SetInt("sniperNextLevel", 1);

        playButton.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(false);

        PlayerPrefs.SetInt("ResetSniperMode", 1);

        InitSniperLevel();
    }
    #endregion
}
