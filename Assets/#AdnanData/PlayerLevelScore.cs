using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLevelScore : MonoBehaviour
{
    // Replacing Here The Logic Of In-Apps From Perk-Level To Player-Level
    public Text[] GoldAmounts, SPAmounts;
    public GameObject[] ExtraTexts;
    public static float _MultiPliyer = 0;
    public static PlayerLevelScore instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        // Updating In-Apps Extra Amount With Player Level Here
        if (SceneManager.GetActiveScene().name == "MainMenu")
            UpdateScore();
    }


    // Working For In-Apps By Player Level

    int currentLevel;
    int currentMultiplier;
    public void UpdateScore()
    {
        currentLevel = PlayerPrefs.GetInt("LastLevel");
        if(currentLevel < 1)
        {
            currentMultiplier = 0;
        }
        else if(currentLevel >= 1 && currentLevel < 2)
        {
            currentMultiplier = 1;
        }
        else if (currentLevel >= 2 && currentLevel < 3)
        {
            currentMultiplier = 2;
        }
        else if (currentLevel >= 3 && currentLevel < 4)
        {
            currentMultiplier = 3;
        }
        else if (currentLevel >= 4 && currentLevel < 5)
        {
            currentMultiplier = 4;
        }
        else if (currentLevel >= 5 && currentLevel < 6)
        {
            currentMultiplier = 5;
        }
        else if (currentLevel >= 6 && currentLevel < 7)
        {
            currentMultiplier = 6;
        }
        else if (currentLevel >= 7 && currentLevel < 8)
        {
            currentMultiplier = 7;
        }
        else if (currentLevel >= 8 && currentLevel < 9)
        {
            currentMultiplier = 8;
        }
        else if (currentLevel >= 9 && currentLevel < 10)
        {
            currentMultiplier = 9;
        }
        else if (currentLevel >= 10)
        {
            currentMultiplier = 10;
        }

        SetStrings();
    }

    void SetStrings()
    {
        switch (currentMultiplier)
        {
            case 0:
                _MultiPliyer = 0.0f * 0.01f;
                break;
            case 1:
                _MultiPliyer = 7.0f * 0.01f;
                break;
            case 2:
                _MultiPliyer = 7.0f * 0.01f;
                break;
            case 3:
                _MultiPliyer = 10.0f * 0.01f;
                break;
            case 4:
                _MultiPliyer = 12.0f * 0.01f;
                break;
            case 5:
                _MultiPliyer = 12.0f * 0.01f;
                break;
            case 6:
                _MultiPliyer = 15.0f * 0.01f;
                break;
            case 7:
                _MultiPliyer = 15.0f * 0.01f;
                break;
            case 8:
                _MultiPliyer = 17.0f * 0.01f;
                break;
            case 9:
                _MultiPliyer = 20.0f * 0.01f;
                break;
            case 10:
                _MultiPliyer = 25.0f * 0.01f;
                break;
        }
        if (_MultiPliyer != 0)
        {
#if UNITY_EDITOR
            print("_MultiPliyer : " + _MultiPliyer);
#endif
            GoldAmounts[0].text = "+" + (int)(3000 * _MultiPliyer);
            SPAmounts[0].text = "+" + (int)(1500 * _MultiPliyer);
            GoldAmounts[1].text = "+" + (int)(4000 * _MultiPliyer);
            SPAmounts[1].text = "+" + (int)(3000 * _MultiPliyer);
            GoldAmounts[2].text = "+" + (int)(6000 * _MultiPliyer);
            SPAmounts[2].text = "+" + (int)(5000 * _MultiPliyer);
            GoldAmounts[3].text = "+" + (int)(8000 * _MultiPliyer);
            SPAmounts[3].text = "+" + (int)(8000 * _MultiPliyer);
            GoldAmounts[4].text = "+" + (int)(1000 * _MultiPliyer);
            SPAmounts[4].text = "+" + (int)(500 * _MultiPliyer);
            GoldAmounts[5].text = "+" + (int)(1500 * _MultiPliyer);
            SPAmounts[5].text = "+" + (int)(800 * _MultiPliyer);
            for (int i = 0; i < ExtraTexts.Length; i++)
                ExtraTexts[i].SetActive(true);
        }
        else
        {
            for (int i = 0; i < ExtraTexts.Length; i++)
                ExtraTexts[i].SetActive(false);
            GoldAmounts[0].text = "";
            SPAmounts[0].text = "";
            GoldAmounts[1].text = "";
            SPAmounts[1].text = "";
            GoldAmounts[2].text = "";
            SPAmounts[2].text = "";
            GoldAmounts[3].text = "";
            SPAmounts[3].text = "";
            GoldAmounts[4].text = "";
            SPAmounts[4].text = "";
            GoldAmounts[5].text = "";
            SPAmounts[5].text = "";
        }

    }
}
