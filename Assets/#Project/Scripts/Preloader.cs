using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEngine.Analytics;

public class Preloader : MonoBehaviour
{
    public static bool fromPreloader = false;

    //Auto Quality inputs
    [Header("-----RAM-----")]
    public float maxRam;
    public float ramPriorityValue = 0.15f;
    [Header("-----Shader Level-----")]
    public float maxShaderLevel;
    public float shaderLevelPriorityValue = 0.5f;
    [Header("-----Processor-----")]
    public float maxProcessorClock;
    public float processorPriorityValue = 0.175f;
    [Header("-----VRAM-----")]
    public float maxVram;
    public float vramPriorityValue = 0.175f;

    private float ramResult, shaderResult, processorResult, vRamResult;

    private void OnEnable()
    {
        ramResult = SystemInfo.systemMemorySize / maxRam;
        ramResult *= ramPriorityValue;

        shaderResult = SystemInfo.graphicsShaderLevel / maxShaderLevel;
        shaderResult *= shaderLevelPriorityValue;

        processorResult = SystemInfo.processorFrequency / maxProcessorClock;
        processorResult *= processorPriorityValue;

        vRamResult = SystemInfo.graphicsMemorySize / maxVram;
        vRamResult *= vramPriorityValue;
        vRamResult = Mathf.Clamp((ramResult + shaderResult + processorResult + vRamResult), 0f, 1f) * 2;
        if (vRamResult >= 1.85f)
            AutoQualityChooser.finalResult = 2;
        else if (vRamResult <= 0.88f)
            AutoQualityChooser.finalResult = 0;
        else
            AutoQualityChooser.finalResult = (int)vRamResult;
#if UNITY_EDITOR
        print("FinaResult: " + AutoQualityChooser.finalResult);
#endif
        //Analytics
        PlayerPrefs.SetInt("SessionCount", PlayerPrefs.GetInt("SessionCount") + 1);
        Analytics.CustomEvent("Session", new Dictionary<string, object>
        {
        { "level_index", PlayerPrefs.GetInt("SessionCount") }
        });
#if UNITY_EDITOR
        Debug.Log("CustomEvent: " + "Session");
#endif
        PlayerPrefs.SetInt("SessionTime", 0);
        //PlayerPrefs.SetInt("MissionStart_Session", 0);
        PlayerPrefs.SetInt("MissionComplete_Session", 0);

        #region Sessions Count

        //if (PlayerPrefs.GetInt("FirstTime") == 0)  // if first sedssion then stores this data
        //{
        //    PlayerPrefs.SetString("Date", System.DateTime.Now.ToString("dd"));
        //    PlayerPrefs.SetInt("Day", (PlayerPrefs.GetInt("Day") + 1));
        //    PlayerPrefs.SetInt("FirstTime", 1);
        //}

        //if (PlayerPrefs.GetString("Date") != System.DateTime.Now.ToString("dd")) // if current date is not same as stored date
        //{
        //    PlayerPrefs.SetInt("Day", (PlayerPrefs.GetInt("Day") + 1));
        //    PlayerPrefs.SetInt("Session", 0);
        //    PlayerPrefs.SetString("Date", System.DateTime.Now.ToString("dd"));
        //}

        //PlayerPrefs.SetInt("TotalSessions", PlayerPrefs.GetInt("TotalSessions") + 1);

        //PlayerPrefs.SetInt("Session", (PlayerPrefs.GetInt("Session") + 1));

        //int Day = PlayerPrefs.GetInt("Day");
        //int Session = PlayerPrefs.GetInt("Session");

        //// Analytic to store daily base session count
        //Analytics.CustomEvent("SessionsCount", new Dictionary<string, object>
        //{
        //{ "Day" + Day, Session }
        //});

        //// Analytic to store total sessions count
        //Analytics.CustomEvent("TotalSessions", new Dictionary<string, object>
        //{
        //{ "Total" , PlayerPrefs.GetInt("TotalSessions") }
        //});

        #endregion
    }
    private void Start()
    {
        if (SystemInfo.systemMemorySize < 2800)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

       // identifierText.text = "V " + Application.version;

#if UNITY_ANDROID
        LocalizationManager.SetLanguageVal(PlayerPrefs.GetInt("SelectedLanguage"));
        Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.InversedLarge);
        Handheld.StartActivityIndicator();
#endif
        StartCoroutine(LoadMainMenu());


    }
    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(1.5f);
        // For new users
        if (SaveManager.Instance.state.loadFirstTime.Equals(0))
        {

            PlayerPrefs.SetInt("AllowSessionAd", 0);
            
            PlayerPrefs.SetInt("Width", Screen.width);
            PlayerPrefs.SetInt("Height", Screen.height);
            // Sniper
            PlayerPrefs.SetInt("sniperLevelUnlocked-0", 1);
            PlayerPrefs.SetInt("sniperCurrentLevel", 1);
            PlayerPrefs.SetInt("sniperNextLevel", 1);

            //Cover Strike
            PlayerPrefs.SetInt("CoverStrikeLevelUnlocked-0", 1);
            PlayerPrefs.SetInt("CoverStrikeCurrentLevel", 1);
            PlayerPrefs.SetInt("CoverStrikeNextLevel", 1);

            //MultiPlayer
            PlayerPrefs.SetInt("MultiLevelUnlocked-0", 1);
            PlayerPrefs.SetInt("MultiCurrentLevel", 1);
            PlayerPrefs.SetInt("MultiNextLevel", 1);

            //BR
            PlayerPrefs.SetInt("BRLevelUnlocked-0", 1);
            PlayerPrefs.SetInt("BRCurrentLevel", 1);
            PlayerPrefs.SetInt("BRNextLevel", 1);

            //FFA
            PlayerPrefs.SetInt("FFALevelUnlocked-0", 1);
            PlayerPrefs.SetInt("FFACurrentLevel", 1);
            PlayerPrefs.SetInt("FFANextLevel", 1);

            // Assault
            PlayerPrefs.SetInt("levelUnlocked-0", 1);
            PlayerPrefs.SetInt("currentLevel", 1);
            PlayerPrefs.SetInt("nextLevel", 1);
            PlayerPrefs.SetInt("LevelCount", 1);

            PlayerPrefs.SetFloat("Difficulty", 7f);
            PlayerPrefs.SetInt("BloodEffect", 1);

            PlayerPrefs.SetInt("weapon0", 1);
            PlayerPrefs.SetInt("weapon13", 1); // Gun number 13 (AK-47) unlocking
            PlayerPrefs.SetString("WeaponName", "AK-47"); // Change name of weapon
            PlayerPrefs.SetInt("selectedWeaponIndex", 13); // Change selected weapon as AK-47
            PlayerPrefs.SetInt("SniperEquipped", -99);
            PlayerPrefs.SetInt("AssaultEquipped", 13); // Equipped gun as AK-47

            if(PlayerPrefs.HasKey("CampMode") || PlayerPrefs.HasKey("MultiMode"))
            {
                PlayerPrefs.SetInt("CampMode", 0);
                PlayerPrefs.SetInt("MultiMode", 0);
            }

            //PlayerPrefs.SetInt("CampMode", 0);
            //PlayerPrefs.SetInt("MultiMode", 2);
            PlayerPrefs.SetInt("V" + Application.version, 1);

            Handheld.StopActivityIndicator();
            PlayerPrefs.SetInt("Mode1", 1);
            PlayerPrefs.SetInt("weapon19", 1);
            PlayerPrefs.SetInt("SniperEquipped", 19);
            Invoke("LoadMenuScene", 8f);

        }
        else
        {
           

            if (!PlayerPrefs.GetInt("V" + Application.version).Equals(1))
            {
                if (PlayerPrefs.HasKey("CampMode") || PlayerPrefs.HasKey("MultiMode"))
                {
                    PlayerPrefs.SetInt("CampMode", 0);
                    PlayerPrefs.SetInt("MultiMode", 0);
                }
               // PlayerPrefs.SetInt("MultiMode", 2);
               // PlayerPrefs.SetInt("CampMode", 0);//5
                //Cover Strike
                PlayerPrefs.SetInt("CoverStrikeLevelUnlocked-0", 1);
                PlayerPrefs.SetInt("CoverStrikeCurrentLevel", 1);
                PlayerPrefs.SetInt("CoverStrikeNextLevel", 1);

                //MultiPlayer
                PlayerPrefs.SetInt("MultiLevelUnlocked-0", 1);
                PlayerPrefs.SetInt("MultiCurrentLevel", 1);
                PlayerPrefs.SetInt("MultiNextLevel", 1);

                //BR
                PlayerPrefs.SetInt("BRLevelUnlocked-0", 1);
                PlayerPrefs.SetInt("BRCurrentLevel", 1);
                PlayerPrefs.SetInt("BRNextLevel", 1);

                //FFA
                PlayerPrefs.SetInt("FFALevelUnlocked-0", 1);
                PlayerPrefs.SetInt("FFACurrentLevel", 1);
                PlayerPrefs.SetInt("FFANextLevel", 1);
                PlayerPrefs.SetInt("V" + Application.version, 1);
            }

            if (!PlayerPrefs.HasKey("AllowSessionAd"))
            {
                
                PlayerPrefs.SetInt("AllowSessionAd", 0);
#if UNITY_EDITOR
                print("Ad Not Session");
#endif
            }
            else
            {
                PlayerPrefs.SetInt("AllowSessionAd", 1);
#if UNITY_EDITOR
                print("Ad Yes Session");
#endif
            }

            Handheld.StopActivityIndicator();
            fromPreloader = true;

            
            SaveManager.Instance.Session = 1;
            PlayerPrefs.SetInt("Mode1", 1);
            Invoke("LoadMenuScene", 8);
        }


    }

    void LoadMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnDisable()
    {
        StopCoroutine("LoadMainMenu");
        CancelInvoke("LoadMenuScene");
    }
}