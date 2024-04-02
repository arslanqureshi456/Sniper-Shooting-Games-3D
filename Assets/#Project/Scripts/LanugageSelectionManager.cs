using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanugageSelectionManager : MonoBehaviour
{
    public GameObject[] Buttons;
    void Start()
    {
        ResetButtons();
    }
    public void ResetButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (PlayerPrefs.GetInt("SelectedLanguage") == i)
            {
                Buttons[i].transform.GetChild(0).gameObject.SetActive(true);
                Buttons[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                Buttons[i].transform.GetChild(0).gameObject.SetActive(false);
                Buttons[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
