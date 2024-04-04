using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerStatic 
{
    private static GameManagerStatic instance;

    private GameManagerStatic() { }

    public static GameManagerStatic Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameManagerStatic();
            }
            return instance;
        }
    }
    public bool gunPickup;
    public bool isFeaturedGuns;
    public int GunsStoreFrom; // (1) if goes from levelselection
    public int FromGunsToStore; // From Guns Panel To Store , To Get More SP or Gold
    //public int GunType; // 0 for Assault Guns , 1 for Sniper Guns.   (used , when we are on guns store and changing guns type)
    public int isGamePlaySceneEnabled; // we do this for unity ads to load. when user goes for gameplay ,  this int becomes 1 , after this int to be 1 if user come back to mainmenu then we will initialize unity ads 
    public int isUnityAdsInitialized; // to check if unity ads already initialized
    public int MenusBackCount; // Count Back Btn pressed on menu scene t0 show interstitial on every 3 back btns clicked
    public int isSessionShown; // check if session shown on start , otherwise it will be shown on Play Button in menu screen
    public int AllGunsUnlocked; // check if all guns are unlocked
    public int AllModesUnlocked; // check if all modes are unlocked
    public int lastlevel; // last level , so on next level button, we have to go to main menu scene
    //public bool NextLevelClicked;
    public string interstitial;
}
