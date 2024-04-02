using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SceneStart : MonoBehaviour
{
    public GameObject Gameplay, HeliScene;//, ParachuteScene;
    public GameObject Night, Day;
    void OnEnable()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)// Sniper
        {
            SetWeather(PlayerPrefs.GetInt("sniperCurrentLevel"));

        }
        else// Assault
        {
            SetWeather(PlayerPrefs.GetInt("currentLevel"));
        }
        //ParachuteScene.SetActive(true);
        //return;
        //HeliScene.SetActive(true);
        if (PlayerPrefs.HasKey("HasSeenScene") || LevelSelectionNew.modeSelection != LevelSelectionNew.modeType.ASSAULT)
        {
            Gameplay.SetActive(true);
        }
        else
        {
            HeliScene.SetActive(true);
            //Debug.Log("PlayerLanding");
            //Analytics.CustomEvent("PlayerLanding", new Dictionary<string, object>
            //{
            //{ "V" + Application.version, 1}
            //});
        }
    }
    void SetWeather(int Level)
    {
        switch (((Level-1) % 10))
        {
            case 0:// Day
            case 1:// Day
            case 3:// Day
            case 4:// Day
            case 6:// Day
            case 9:// Day
                Day.SetActive(true);
                break;
            default:// clouds light dark lighting rain
                Night.SetActive(true);
                break;
        }
    }
    public void OnSceneEnd()
    {
        HeliScene.SetActive(false);
        //ParachuteScene.SetActive(false);
        Time.timeScale = 1;
        PlayerPrefs.SetInt("HasSeenScene", 1);
        Gameplay.SetActive(true);
    }
}
