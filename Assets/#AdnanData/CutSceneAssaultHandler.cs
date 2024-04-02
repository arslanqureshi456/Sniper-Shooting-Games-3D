using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneAssaultHandler : MonoBehaviour
{

    public GameObject game; // GameManager
    public GameObject cutSceneData; // CutScene Gameobject
    public GameObject map, healthBar; // Map & Healthbar UI of Player
    public GameObject SFX; //Sound Effects
    public GameObject Fence; // Fence to be setactive(false) for cutscene

    public static CutSceneAssaultHandler instance; // Making Instance


    private void OnEnable()
    {
        try
        {
            instance = this;
            #region CutScene Data
            //if (PlayerPrefs.GetInt("Start_CutScene") == 0)
            //{
            //    cutSceneData.SetActive(true);
            //    game.GetComponent<GameManager>().enabled = false;
            //    map.SetActive(false);
            //    healthBar.SetActive(false);
            //    SFX.SetActive(false);
            //    Fence.SetActive(false);
            //    AudioManager.instance.PlayAssaultBGM();
            //}
            //else if (PlayerPrefs.GetInt("currentLevel") == 1 && PlayerPrefs.GetInt("Start_CutScene") == 1)
            //{
            //    game.GetComponent<GameManager>().enabled = true;
            //    Fence.SetActive(false);
            //}
            //else
            //{
            //    game.GetComponent<GameManager>().enabled = true;
            //}
            #endregion
            // After Commenting Cutscene Data
            //if (PlayerPrefs.GetInt("currentLevel") == 1)
            //{
            //    game.GetComponent<GameManager>().enabled = true;
            //    Fence.SetActive(false);
            //}
            //else
            //{
                game.GetComponent<GameManager>().enabled = true;
            //}

#if UNITY_EDITOR
            print("Current Level : " + PlayerPrefs.GetInt("currentLevel"));
#endif

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

        }
        catch { }
    }
        

}
