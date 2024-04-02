using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteLabelAnim : MonoBehaviour
{
    public Transform handGuns, backGuns;
    void Start()
    {
        if(LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            handGuns.GetChild(PlayerPrefs.GetInt("SniperEquipped")).gameObject.SetActive(true);
            backGuns.GetChild(PlayerPrefs.GetInt("SniperEquipped")).gameObject.SetActive(true);
        }else
        {
            handGuns.GetChild(PlayerPrefs.GetInt("AssaultEquipped")).gameObject.SetActive(true);
            backGuns.GetChild(PlayerPrefs.GetInt("AssaultEquipped")).gameObject.SetActive(true);
        }
    }
}
