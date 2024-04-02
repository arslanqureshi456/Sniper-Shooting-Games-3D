using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class LoadOutManager : MonoBehaviour
{
    //public InAppManager _InAppManager;
    //string iap_AllSniper = "all_sniper_guns";
    //string iap_Dis_AllSniper = "dis_all_sniper_guns";
    //string iap_AllAssault = "all_assault_guns";
    //string iap_Dis_AllAssault = "dis_all_assault_guns";
    //string iap_AllWeapons = "all_guns";
    //string iap_Dis_AllWeapons = "dis_all_guns";
    public GameObject AllButton, AssaultButton, SniperButton;
    public delegate void SetStateDelegate();
    public SetStateDelegate EquipGunDelegate;
    //public delegate void InAppDelegate();
    //public InAppDelegate loadOutInAppDelegate;
    public static LoadOutManager Instance = null;
    public Transform GunImages;
    public Transform GunContainer;
    int selectedGun = 0;
    public GameObject GoldIcon, SPIcon, StatsContainer, tutorialPanel, InAPPButton, BuyButton, EquippedButton, EquipButton, PlayButton, watchVideoButton;
    public int isLoadoutAd = 0;
    public GameObject[] Classes;
    public Text Title, Desc, Code,RealPrice,DiscountPrice,PriceText,RealPriceAssault,DiscountPriceAssault,RealPriceSniper,DiscountPriceSniper,RealPriceAll,DiscountPriceAll, adCountText;
    public Image SelectedImage,TagImage,TopBarImage;

    public Image
        damageSlider,
        fireRateSlider,
        accuracySlider,
        reloadTimeSlider,
        zoomSlider,
        rangeSlider,
        magSizeSlider;
    public Text
        damageText,
        fireRateText,
        accuracyText,
        reloadTimeText,
        zoomText,
        rangeText,
        magSizeText;

    

    private void OnDisable()
    {
        //for (int i = 0; i < GunContainer.childCount; i++)
        //{
        //    GunContainer.GetChild(i).gameObject.SetActive(false);
        //}
    }
    void ResetClasses()
    {
        for (int i = 0; i < Classes.Length; i++)
        {
            Classes[i].SetActive(false);
        }
    }
    public void OnEnable()
    {
        try
        {
            //RealPriceAssault.text = _InAppManager.AllAssaultReal;
            if (RealPriceAssault.text == "")
                RealPriceAssault.text = "Purchase";
            //DiscountPriceAssault.text = _InAppManager.AllAssaultDiscount;
            //RealPriceSniper.text = _InAppManager.AllSniperReal;
            if (RealPriceSniper.text == "")
                RealPriceSniper.text = "Purchase";
            //DiscountPriceSniper.text = _InAppManager.AllSniperDiscount;
            //RealPriceAll.text = _InAppManager.AllWeaponsReal;
            if (RealPriceAll.text == "")
                RealPriceAll.text = "Purchase";
            //DiscountPriceAll.text = _InAppManager.AllWeaponsDiscount;

            AllButton.SetActive(false);
            SniperButton.SetActive(false);
            AssaultButton.SetActive(false);
            GunImages.GetChild(selectedGun).gameObject.SetActive(false);
            if (Instance == null)
                Instance = this;

            if (PlayerPrefs.GetInt("sniperLevelUnlocked-1").Equals(1) || PlayerPrefs.GetInt("levelUnlocked-1").Equals(1))
            {
                tutorialPanel.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("ResetSniperMode").Equals(1))
            {
                tutorialPanel.SetActive(false);
            }

            switch (LevelSelectionNew.modeSelection)
            {
                //Sniper
                case LevelSelectionNew.modeType.SNIPER:
                    //SniperButton.SetActive(true);
                    for (int i = 0; i < GunContainer.childCount; i++)
                    {
                        if (GunContainer.GetChild(i).GetComponent<StoreWeapons>().weapon.Category == "Sniper")
                            GunContainer.GetChild(i).gameObject.SetActive(true);
                    }
                    selectedGun = PlayerPrefs.GetInt("SniperEquipped");
                    break;
                //Assault
                case LevelSelectionNew.modeType.ASSAULT:
                    //AssaultButton.SetActive(true);
                    for (int i = 0; i < GunContainer.childCount; i++)
                    {
                        if (GunContainer.GetChild(i).GetComponent<StoreWeapons>().weapon.Category == "Assault")
                            GunContainer.GetChild(i).gameObject.SetActive(true);
                    }
                    selectedGun = PlayerPrefs.GetInt("AssaultEquipped");
                    //Debug.Log("selected gun asault " + selectedGun);
                    break;
                //Multiplayer
                default:
                    //AllButton.SetActive(true);
                    for (int i = 0; i < GunContainer.childCount; i++)
                    {
                        GunContainer.GetChild(i).gameObject.SetActive(true);
                    }
                    selectedGun = PlayerPrefs.GetInt("AssaultEquipped");
                    break;
            }
            GunImages.GetChild(selectedGun).gameObject.SetActive(true);
        }
        catch { }
    }
    public void SelectGun(Weapon weapon,Color color)
    {
        try
        {
            ResetClasses();
            switch (weapon.Class.ToLower())
            {
                case "essential":
                    Classes[0].SetActive(true);
                    break;
                case "common":
                    Classes[1].SetActive(true);
                    break;
                case "rare":
                    Classes[2].SetActive(true);
                    break;
                case "extraordinary":
                    Classes[3].SetActive(true);
                    break;
            }
            AudioManager.instance.BackButtonClick();
            Title.text = weapon._name;
            Desc.text = weapon.Desc;
            Code.text = weapon.Code;
            GunImages.GetChild(selectedGun).gameObject.SetActive(false);
            GunImages.GetChild(weapon.PrefsIndex).gameObject.SetActive(true);
            TagImage.color = weapon.ClassColor;
            SelectedImage.color = color;
            TopBarImage.color = color;
            ShowStats(weapon);
            if (PlayerPrefs.GetInt("weapon" + weapon.PrefsIndex).Equals(1))
            {
                BuyButton.SetActive(false);
                InAPPButton.SetActive(false);
                watchVideoButton.SetActive(false);
                PlayButton.SetActive(true);
                if ((weapon.Category == "Assault" && PlayerPrefs.GetInt("AssaultEquipped") == weapon.PrefsIndex) || (weapon.Category == "Sniper" && PlayerPrefs.GetInt("SniperEquipped") == weapon.PrefsIndex))
                {
                    EquippedButton.SetActive(true);
                    EquipButton.SetActive(false);
                }
                else
                {
                    EquippedButton.SetActive(false);
                    EquipButton.SetActive(true);
                }
            }
            else
            {
                PlayButton.SetActive(false);
                InAPPButton.SetActive(true);
                BuyButton.SetActive(true);
                watchVideoButton.SetActive(true);
                InAPPButton.SetActive(true);
                EquipButton.SetActive(false);
                EquippedButton.SetActive(false);
                PriceText.text = System.String.Empty + (weapon._goldPrice + weapon._spPrice);
                adCountText.text = (PlayerPrefs.GetInt("AdCount" + weapon.PrefsIndex)) + "/" + (weapon.adCount);

                //RealPrice.text = _InAppManager.GetPriceWithId(weapon.RealPriceID);
                //DiscountPrice.text = _InAppManager.GetPriceWithId(weapon.DiscountPriceID);

                if (weapon._spPrice == 0)// Assault
                {
                    GoldIcon.SetActive(true);
                    SPIcon.SetActive(false);
                }
                else// Sniper
                {
                    GoldIcon.SetActive(false);
                    SPIcon.SetActive(true);
                }
            }
            selectedGun = weapon.PrefsIndex;
        }
        catch { }
    }
    void ShowStats(Weapon weapon)
    {
        damageSlider.fillAmount = weapon._baseDamage / 103;
        fireRateSlider.fillAmount = 0.06f / weapon._baseFireRate;
        accuracySlider.fillAmount = weapon._baseAccuracy / 102;
        reloadTimeSlider.fillAmount = 1 / weapon._baseReloadTime;
        zoomSlider.fillAmount = 10 / weapon._baseZoom;
        rangeSlider.fillAmount = weapon._baseRange / 135;
        magSizeSlider.fillAmount = weapon._baseMagzineSize / 54;

        damageText.text = System.String.Empty + weapon._baseDamage;
        fireRateText.text = System.String.Empty + weapon._baseFireRate;
        accuracyText.text = System.String.Empty + weapon._baseAccuracy;
        reloadTimeText.text = System.String.Empty + weapon._baseReloadTime;
        zoomText.text = System.String.Empty + weapon._baseZoom;
        rangeText.text = System.String.Empty + weapon._baseRange;
        magSizeText.text = System.String.Empty + weapon._baseMagzineSize;
    }
    public void EquipGun()
    {
        try
        {
            EquipGunDelegate();
        }
        catch { }
    }
    public void BuyGun()
    {
        try
        {
            MainMenuManager.buyDeletegate(true);
        }
        catch { }
    }

    public void InAppGun()
    {
        try
        {
            MainMenuManager.onInAppDelegate();
        }
        catch { }
    }
    //public void BuyAllAssault()
    //{
    //    //_InAppManager.PurchaseAllAssault();
    //}
    //public void BuyAllSniper()
    //{
    //    //_InAppManager.PurchaseAllSniper();
    //}
    //public void BuyAllWeapons()
    //{
    //    //_InAppManager.PurchaseAllWeapons();
    //}
    //public void GotoStore()
    //{
    //    try
    //    {
    //        MainMenuManager.Instance.isLoadout = true;
    //        gameObject.SetActive(false);
    //        MainMenuManager.Instance._StoreButton();
    //    }
    //    catch { }
    //}
    //public void PurchaseOneNade()
    //{
    //    try
    //    {
    //        isLoadoutAd = 1;
    //        //if (GoogleMobileAdsManager.Instance.rewarded.IsLoaded())
    //        //{
    //        //    GoogleMobileAdsManager.Instance.ShowRewarded();
    //        //}
    //        //else
    //        if (UnityAdsManager.Instance.IsRewardedVideoReady())
    //        {
    //            UnityAdsManager.Instance.ShowUnityRewardedVideoAd();
    //        }
    //    }
    //    catch { }
    //}
    //public void PurchaseAdrenaline()
    //{
    //    try
    //    {
    //        isLoadoutAd = 2;
    //        //if (GoogleMobileAdsManager.Instance.rewarded.IsLoaded())
    //        //{
    //        //    GoogleMobileAdsManager.Instance.ShowRewarded();
    //        //}
    //        //else 
    //        if (UnityAdsManager.Instance.IsRewardedVideoReady())
    //        {
    //            UnityAdsManager.Instance.ShowUnityRewardedVideoAd();
    //        }
    //    }
    //    catch { }
    //}

    public void _PlayButton()
    {
        try
        {
            //print("LoadOutManager_Level : " + PlayerPrefs.GetInt("currentLevel"));
            AudioManager.instance.LoadOutSelectSound();
            AudioManager.instance.backgroundMusicSouce.GetComponent<AudioSource>().enabled = false;
            MainMenuManager.Instance.StartLoading();
        }
        catch { }
    }

   

    public void WatchVideo()
    {
        try
        {
            MainMenuManager.gunDelegate();
        }
        catch { }
    }
}