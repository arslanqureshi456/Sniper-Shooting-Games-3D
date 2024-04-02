using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoQualityChooser : MonoBehaviour
{
    [Space(10)]
    public List<GameObject> trees = new List<GameObject>();
    public List<GameObject> bushes = new List<GameObject>();
    public List<GameObject> lattice = new List<GameObject>();
    public List<GameObject> glass = new List<GameObject>();
    public List<GameObject> sfx = new List<GameObject>();


    public static int finalResult;

    private void OnEnable()
    {
        Screen.SetResolution(PlayerPrefs.GetInt("Width"), PlayerPrefs.GetInt("Height"), true, 60);
        SetQuality();
    }

    private void SetQuality()
    {
        switch (finalResult)
        {
            case 0://Low
                CameraCulling.defaultCam = 100;
                CameraCulling.nearCam = 30;
                CameraCulling.farCam = 100;
                CameraCulling.skinCam = 15;
                CameraCulling.skinFar = 30;
                QualitySettings.SetQualityLevel(0);
                Screen.SetResolution((int)(PlayerPrefs.GetInt("Width") * 0.7f), (int)(PlayerPrefs.GetInt("Height") * 0.7f), true);
                HideTrees();
                HideBushes();
                HideLattice();
                HideGlass();
                HideSFX();
                break;
            case 1://Medium
                CameraCulling.defaultCam = 100;
                CameraCulling.nearCam = 30;
                CameraCulling.farCam = 200;
                CameraCulling.skinCam = 20;
                CameraCulling.skinFar = 70;
                QualitySettings.SetQualityLevel(1);
                Screen.SetResolution((int)(PlayerPrefs.GetInt("Width") * 0.75f), (int)(PlayerPrefs.GetInt("Height") * 0.75f), true); // 0.8
                HideTrees();
                HideGlass();
               // HideLattice();
                HideSFX();
                break;
            case 2://High
                CameraCulling.defaultCam = 150;
                CameraCulling.nearCam = 50;
                CameraCulling.farCam = 300;
                CameraCulling.skinCam = 40;
                CameraCulling.skinFar = 130;
                QualitySettings.SetQualityLevel(2);
                Screen.SetResolution(PlayerPrefs.GetInt("Width"), PlayerPrefs.GetInt("Height"), true);
                break;
        }

        CameraCulling.skyDome = 3600;

        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            CameraCulling.skinCam *= 5;
            CameraCulling.skinFar *= 5;
            CameraCulling.defaultCam = (int)(CameraCulling.defaultCam * 2f);
            CameraCulling.nearCam = (int)(CameraCulling.nearCam * 2f);
        }
    }

    private void HideTrees()
    {
        foreach (GameObject go in trees)
        {
            go.SetActive(false);
        }
    }

    private void HideBushes()
    {
        foreach (GameObject go in bushes)
        {
            go.SetActive(false);
        }
    }

    private void HideLattice()
    {
        foreach (GameObject go in lattice)
        {
            go.SetActive(false);
        }
    }

    private void HideGlass()
    {
        foreach (GameObject go in glass)
        {
            go.SetActive(false);
        }
    }

    private void HideSFX()
    {
        foreach (GameObject go in sfx)
        {
            go.SetActive(false);
        }
    }

    public void DefaultCamValue(string val)
    {
        CameraCulling.defaultCam = int.Parse(val);
    }

    public void NearCamValue(string val)
    {
        CameraCulling.nearCam = int.Parse(val);
    }

    public void FarCamValue(string val)
    {
        CameraCulling.farCam = int.Parse(val);
    }
}