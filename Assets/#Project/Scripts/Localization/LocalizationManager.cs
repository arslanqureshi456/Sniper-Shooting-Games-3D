using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    static int CurrentLangauge = 0, CurrentUpgradesMinFont = 90,CurrentMainMenuFont = 90;
    public delegate void LocalizationContainer(int i);
    public static LocalizationContainer LocalContainer;
    public delegate void FontSizeDelegate(int i);
    public static FontSizeDelegate MainMenuFontSizeHandler = null;
    public static void SetLanguage(int i)
    {
        CurrentLangauge = i;
        LocalContainer(CurrentLangauge);
    }
    public static void SetLanguageVal(int i)
    {
        CurrentLangauge = i;
    }
    //public static void SetUpgradesMinSize(int i)
    //{
    //    if(i < CurrentUpgradesMinFont)
    //    {
    //        CurrentUpgradesMinFont = i;
    //        if(UpgradesFontSizeHandler != null)
    //            UpgradesFontSizeHandler(CurrentUpgradesMinFont);
    //    }
    //}
    public static void SetMainMenuMinSize(int i)
    {
        if (i < CurrentMainMenuFont)
        {
            CurrentMainMenuFont = i;
            if (MainMenuFontSizeHandler != null)
                MainMenuFontSizeHandler(CurrentMainMenuFont);
        }
    }
    public static int GetLanguage()
    {
        return CurrentLangauge;
    }
    //public static void ChangeUpgradesFontSize()
    //{
    //    UpgradesFontSizeHandler(CurrentUpgradesMinFont);
    //}
    public static void ChangeMainMenuFontSize()
    {
        MainMenuFontSizeHandler(CurrentUpgradesMinFont);
    }
    //Call from StoreBack or StoreEnable
    public static void ResetFontSizes()
    {
        CurrentMainMenuFont = CurrentUpgradesMinFont = 90;
    }
}
