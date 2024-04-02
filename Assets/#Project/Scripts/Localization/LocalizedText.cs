using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;

public class LocalizedText : MonoBehaviour
{
    Text text = null;
    public string[] Translations;
    bool first = true;
    float mult;
    void OnEnable()
    {
        if(first)
        {
            text = GetComponent<Text>();
            //if (text.transform.parent && text.transform.parent.parent.name == "Specs")
            //    LocalizationManager.UpgradesFontSizeHandler += ChangeUpgradeFontSize;
            LocalizationManager.LocalContainer += ChangeTranslation;
            first = false;
        }
        if (text != null && text.gameObject.activeSelf)
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
    private void OnDisable()
    {
        first = true;
        LocalizationManager.LocalContainer -= ChangeTranslation;
    }
    //void DelayedTextCheck()
    //{
    //    mult = 1 / text.canvas.scaleFactor;
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
        if (text != null && text.gameObject.activeSelf)
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