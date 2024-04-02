using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LanguageChanger : MonoBehaviour
{
    public int LanguageIndex;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangeLanguage);
        //if(PlayerPrefs.HasKey("CurrentLanguage"))
        //    LocalizationManager.SetLanguage(PlayerPrefs.GetInt("CurrentLanguage"));
        //else
        //    LocalizationManager.SetLanguage(0);
    }
    public void ChangeLanguage()
    {
        AudioManager.instance.NormalClick();
        LocalizationManager.SetLanguage(LanguageIndex);
        PlayerPrefs.SetInt("SelectedLanguage", LanguageIndex);
        transform.parent.GetComponent<LanugageSelectionManager>().ResetButtons();
    }
}
