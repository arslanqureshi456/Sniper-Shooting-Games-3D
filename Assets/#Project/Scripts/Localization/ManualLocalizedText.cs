using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;

public class ManualLocalizedText : MonoBehaviour
{
    Text text = null;
    public string[] Translations;
    bool first = true;
    float mult;
    void OnEnable()
    {
        if (first)
        {
            text = GetComponent<Text>();
            //mult = 1 / text.canvas.scaleFactor;
            //if (text.transform.parent && text.transform.parent.parent.name == "Specs")
            //    LocalizationManager.UpgradesFontSizeHandler += ChangeUpgradeFontSize;
            //}else if(text.transform.parent && text.transform.parent.parent.name == "BottomBar")
            //    LocalizationManager.MainMenuFontSizeHandler += ChangeUpgradeFontSize;
            LocalizationManager.LocalContainer += ChangeTranslation;
            first = false;
        }
        if(text != null && text.gameObject.activeSelf)
        {
            if (LocalizationManager.GetLanguage() == 4)
            {
                text.text = ArabicFixer.Fix(Translations[LocalizationManager.GetLanguage()]);
            }
            else
                text.text = "" + Translations[LocalizationManager.GetLanguage()];
        }
        //Invoke("DelayedTextCheck", 0.15f);
    }
    //private void OnDisable()
    //{
    //    text.resizeTextMaxSize = 40;
    //    LocalizationManager.ResetFontSizes();
    //}
    //void DelayedTextCheck()
    //{
    //    LocalizationManager.SetUpgradesMinSize((int)(text.cachedTextGenerator.fontSizeUsedForBestFit * mult));
    //}
    //public void ChangeUpgradeFontSize(int size)
    //{
    //    //text.resizeTextForBestFit = false;
    //    //text.min
    //    text.resizeTextMaxSize = size;
    //    //text.fontSize = size;
    //    //text.cachedTextGenerator.fontSizeUsedForBestFit = 23;
    //}
    public void ChangeTranslation(int i)
    {
        if(text != null && text.gameObject.activeSelf)
        {
            if (i == 4)
            {
                text.text = ArabicFixer.Fix(Translations[i]);
            }
            else
                text.text = "" + Translations[i];
        }
        
    }
}
