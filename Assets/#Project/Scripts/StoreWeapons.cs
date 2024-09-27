using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class StoreWeapons : MonoBehaviour
{
    public InAppManager _InAppManager;
    public bool isLoadout = false;
    public static int ASSAULTGUNCOUNT = 19;

    GameObject EquippedButton, BuyButton, EquipButton, InAppButton, watchVideoButton;
    //Text Price, Title, DiscountPrice, RealPrice,Desc,Code;
    bool isSet = false;
    public Weapon weapon;
    int siblingIndex;

    void GetRefrences()
    {
        isSet = true;
        if (!isLoadout)
        {
            //transform.GetChild(9).GetChild(2).GetComponent<Text>().text = (PlayerPrefs.GetInt("AdCount" + weapon.PrefsIndex)) + "/" + (weapon.adCount);
            transform.GetChild(3).GetChild(0).GetComponent<Text>().text = System.String.Empty + (weapon._goldPrice + weapon._spPrice);
            transform.GetChild(2).GetChild(0).GetComponent<Text>().text = weapon._name;
            Color t = GetComponent<Image>().color;
            transform.GetChild(2).GetComponent<Image>().color = new Color(t.r, t.g, t.b, 1);
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = weapon.Code;
            transform.GetChild(2).GetChild(1).GetComponent<Text>().text = weapon.Desc;
            transform.GetChild(2).GetChild(2).GetComponent<Image>().color = new Color(weapon.ClassColor.r, weapon.ClassColor.g, weapon.ClassColor.b, 1);
            //transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>().text = weapon.Class;
            //transform.GetChild(8).GetChild(0).GetComponent<Text>().text = _InAppManager.GetPriceWithId(weapon.RealPriceID);
            if (transform.GetChild(8).GetChild(0).GetComponent<Text>().text == "")
                transform.GetChild(8).GetChild(0).GetComponent<Text>().text = "Purchase";
            //transform.GetChild(8).GetChild(2).GetChild(0).GetComponent<Text>().text = _InAppManager.GetPriceWithId(weapon.DiscountPriceID);
            BuyButton = transform.GetChild(3).gameObject;
            EquippedButton = transform.GetChild(4).gameObject;
            EquipButton = transform.GetChild(5).gameObject;
            InAppButton = transform.GetChild(8).gameObject;
            watchVideoButton = transform.GetChild(9).gameObject;
            watchVideoButton.transform.GetChild(2).GetComponent<Text>().text = (PlayerPrefs.GetInt("AdCount" + weapon.PrefsIndex)) + "/" + (weapon.adCount);
        }
        else
        {
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = System.String.Empty + (weapon._goldPrice + weapon._spPrice);
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = weapon._name;
            Color t = GetComponent<Image>().color;
            transform.GetChild(0).GetComponent<Image>().color = new Color(t.r, t.g, t.b, 1);
            transform.GetChild(0).GetChild(1).GetComponent<Image>().color = weapon.ClassColor;
            //transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = weapon.Class;
            //transform.GetChild(5).GetChild(0).GetComponent<Text>().text = _InAppManager.GetPriceWithId(weapon.RealPriceID);
            if (transform.GetChild(5).GetChild(0).GetComponent<Text>().text == "")
                transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "Purchase";
            //transform.GetChild(5).GetChild(2).GetChild(0).GetComponent<Text>().text = _InAppManager.GetPriceWithId(weapon.DiscountPriceID);
            BuyButton = transform.GetChild(1).gameObject;
            EquippedButton = transform.GetChild(2).gameObject;
            EquipButton = transform.GetChild(3).gameObject;
            InAppButton = transform.GetChild(5).gameObject;

        }
        //if (weapon.RealPriceID != "")
        //{

        //}
        BuyButton.GetComponent<Button>().onClick.AddListener(BuyGunWrapper);
        EquipButton.GetComponent<Button>().onClick.AddListener(EquipGun);
        if (!isLoadout)
        {
            watchVideoButton.GetComponent<Button>().onClick.AddListener(WatchVideoButton);
            InAppButton.GetComponent<Button>().onClick.AddListener(InAppFunction);
        }
        else
            InAppButton.GetComponent<Button>().onClick.AddListener(InAppFunctionSpecification);
        GetComponent<Button>().onClick.AddListener(SelectGun);
    }
    void SetButtonStats()
    {
        try
        {
            if (!isLoadout)
                watchVideoButton.transform.GetChild(2).GetComponent<Text>().text = (PlayerPrefs.GetInt("AdCount" + weapon.PrefsIndex)) + "/" + (weapon.adCount);
            if (weapon.isComingSoon)
            {
                BuyButton.SetActive(false);
                InAppButton.SetActive(false);
                EquippedButton.SetActive(false);
                EquipButton.SetActive(false);
                watchVideoButton.SetActive(false);
                //Price.text = System.String.Empty + (weapon._goldPrice + weapon._spPrice);
                return;
            }
            if (PlayerPrefs.GetInt("weapon" + weapon.PrefsIndex) == 1)
            {
                BuyButton.SetActive(false);
                InAppButton.SetActive(false);
                if (!isLoadout)
                    watchVideoButton.SetActive(false);
                if ((weapon.Category == "Assault" && PlayerPrefs.GetInt("AssaultEquipped") == weapon.PrefsIndex) || (weapon.Category == "Sniper" && PlayerPrefs.GetInt("SniperEquipped") == weapon.PrefsIndex))
                {
                    if (weapon.Category == "Assault" && MainMenuManager.lastAssaultCard == null)
                    {
                        MainMenuManager.lastAssaultCard = SetButtonStats;
                        if (isLoadout && MainMenuManager.lastSniperCard == null)
                        {
                            SelectGun();
                        }
                    }
                    else if (weapon.Category == "Sniper" && MainMenuManager.lastSniperCard == null)
                    {
                        MainMenuManager.lastSniperCard = SetButtonStats;
                        if (isLoadout && MainMenuManager.lastAssaultCard == null)
                        {
                            SelectGun();
                        }
                    }
                    if (!isLoadout)
                    {
                        EquippedButton.SetActive(true);
                        EquipButton.SetActive(false);
                    }


                }
                else if (!isLoadout)
                {
                    EquippedButton.SetActive(false);
                    EquipButton.SetActive(true);
                }
            }
            else if (!isLoadout)
            {
                BuyButton.SetActive(true);
                InAppButton.SetActive(true);
                watchVideoButton.SetActive(true);
                EquippedButton.SetActive(false);
                EquipButton.SetActive(false);
            }
        }
        catch { }
        
    }
    public void SelectGun()
    {
        //int a = transform.GetSiblingIndex();
        //if (weapon.Category != "Assault" && !isLoadout)
        //    a += ASSAULTGUNCOUNT;
        MainMenuManager.buyDeletegate = BuyGun;
        MainMenuManager.onInAppDelegate = InAppFunctionSpecification;
        MainMenuManager.gunDelegate = WatchVideoButton;

        if (!isLoadout)
        {
            WeaponStore.Instance._SelectWeaponButton(weapon, siblingIndex);
        }
        else
        {
            LoadOutManager.Instance.SelectGun(weapon, GetComponent<Image>().color);
            LoadOutManager.Instance.EquipGunDelegate = EquipGun;
        }

        AudioManager.instance.StoreButtonClick();

    }
    public void EquipGun()
    {
        if (weapon.Category == "Assault")
        {
            PlayerPrefs.SetInt("AssaultEquipped", weapon.PrefsIndex);
        }
        else
        {
            PlayerPrefs.SetInt("SniperEquipped", weapon.PrefsIndex);
        }
        AudioManager.instance.StoreButtonClick();
        PlayerPrefs.SetString("WeaponName", weapon._name);
        PlayerPrefs.SetInt("selectedWeaponIndex", weapon.PrefsIndex);

        SetButtonStats();
        if (weapon.Category == "Assault" && MainMenuManager.lastAssaultCard != null)
        {
            MainMenuManager.lastAssaultCard();
            MainMenuManager.lastAssaultCard = SetButtonStats;
        }
        else if (weapon.Category == "Sniper" && MainMenuManager.lastSniperCard != null)
        {
            MainMenuManager.lastSniperCard();
            MainMenuManager.lastSniperCard = SetButtonStats;
        }
        if (isLoadout)
            SelectGun();
    }
    void BuyGunWrapper()
    {
        BuyGun(false);
    }
    public void BuyGun(bool isSpecification)
    {
        //Camera.main.depth = 0;
        AudioManager.instance.NormalClick();
        if (PlayerPrefs.GetInt("gold") >= weapon._goldPrice &&
                PlayerPrefs.GetInt("secretPoints") >= weapon._spPrice)
        {
            if (isSpecification)
                MainMenuManager.processDelegate = ProcessSpecificationPayment;
            else
                MainMenuManager.processDelegate = ProcessPayment;
            MainMenuManager.Instance.ShowPremissionPanel();
        }
        else
        {
            if (weapon._spPrice == 0)//gold
            {
                if ((weapon._goldPrice - PlayerPrefs.GetInt("gold")) <= 220)
                {
                    if (weapon.Category == "Assault")
                        MainMenuManager.Instance.gunAd = MainMenuManager.GunAdType.assault;
                    MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(true);
                    MainMenuManager.Instance.isGoldRewardedADPopup = true;
                }
                else
                    MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(true);
            }
            else if ((weapon._spPrice - PlayerPrefs.GetInt("secretPoints")) <= 100)
            {
                if (weapon.Category == "Sniper")
                    MainMenuManager.Instance.gunAd = MainMenuManager.GunAdType.sniper;
                MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(false);
                MainMenuManager.Instance.isSPRewardedADPopup = true;
            }
            else
            {
                MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(false);
            }
            MainMenuManager.Instance.gunsCamera.SetActive(false);
        }
    }
    void ProcessSpecificationPayment()
    {
        ProcessPayment();
        SelectGun();
    }
    void ProcessPayment()
    {
        Analytics.CustomEvent("GunPurchased" + weapon._name, new Dictionary<string, object>
        {
            { "level_index" , 1 }
        });
#if UNITY_EDITOR
        Debug.Log("CustomEvent: " + "GunPurchased");
#endif
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - weapon._goldPrice);
        PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") - weapon._spPrice);

       // PlayerPrefs.SetFloat("PerkPoints", PlayerPrefs.GetFloat("PerkPoints") + weapon.perkPoints);
        //if (SceneManager.GetActiveScene().name == "MainMenu")
        //    PerkPoints.Instance.UpdateScore();

        PlayerPrefs.SetInt("weapon" + weapon.PrefsIndex, 1);
        if (weapon.PrefsIndex < 19)
            PlayerPrefs.SetInt("AssaultEquipped", weapon.PrefsIndex);
        else
            PlayerPrefs.SetInt("SniperEquipped", weapon.PrefsIndex);

        ConstantUpdate.Instance.UpdateCurrency();
        SetButtonStats();
        AudioManager.instance.ThankYouClick();
        if (isLoadout)
            SelectGun();


#if UNITY_EDITOR
        print("StoreWeapons : Test1");
#endif
        Invoke("EnablingGunsCamera", 0.1f);
#if UNITY_EDITOR
        print("StoreWeapons : Test2");
#endif

        // Analytic To Get Gun Unlock By Using Coins
        Analytics.CustomEvent("GunByCoins", new Dictionary<string, object>
        {
            { "level_index", weapon.PrefsIndex }
        });


#if UNITY_EDITOR
        Debug.Log("CustomEvent: " + "GunByCoins");
        print("GunByCoins : Gun" +weapon.PrefsIndex);
#endif

    }

    void EnablingGunsCamera()
    {
        MainMenuManager.Instance.gunsCamera.SetActive(true);
    }

    public void InAppFunction()
    {
        _InAppManager.onGunInAppHandler = SetButtonStats;
        _InAppManager.PurchaseGun(weapon.RealPriceID, weapon.PrefsIndex);
    }
    public void InAppSpecificationReturn()
    {
        SetButtonStats();
        SelectGun();
    }
    public void InAppFunctionSpecification()
    {
        _InAppManager.onGunInAppHandler = InAppSpecificationReturn;
        _InAppManager.PurchaseGun(weapon.RealPriceID, weapon.PrefsIndex);

    }
    private void OnEnable()
    {
        if (!isSet)
            GetRefrences();
        SetButtonStats();
    }
    public void SetOffersDelegate()
    {
        if (weapon.OfferIndex != -1)
        {
            FindObjectOfType<MainMenuManager>().GunOffersDelegate[weapon.OfferIndex] += SelectGun;
        }
        if (!isLoadout)
        {
            siblingIndex = transform.GetSiblingIndex();
            if (weapon.Category == "Sniper")
                siblingIndex += 19;
            MainMenuManager.Instance.specificationDelegates[siblingIndex] = SelectGun;
        }
    }

    public void WatchVideoButton()
    {
        try
        {
#if UNITY_ANDROID
                Debug.Log("Debug : Admob_Reward For Gun " + WeaponStore.Instance.currentOrderIndex);
#endif
                AdsManager.GunAdHandler = CheckAdCount;
                AdsManager.instance.prefIndex = weapon.PrefsIndex;
            AdsManager.instance.ShowAdmobRewardedAd();
        }
        catch { }
    }

    public void CheckAdCount(int index)
    {
        try
        {
            PlayerPrefs.SetInt("GunAdCount", PlayerPrefs.GetInt("GunAdCount") + 1);
            Analytics.CustomEvent("GunAdCount", new Dictionary<string, object>
        {
            { "level_index", PlayerPrefs.GetInt("GunAdCount") }
        });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "GunAdCount");
#endif
            Analytics.CustomEvent("GunAdCount_" + weapon._name, new Dictionary<string, object>
        {
            { "level_index", PlayerPrefs.GetInt("AdCount" + index) }
        });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "GunAdCount_");
#endif
            if (PlayerPrefs.GetInt("AdCount" + index) == weapon.adCount)
            {
                PlayerPrefs.SetInt("weapon" + index, 1);
                if (weapon.PrefsIndex < 19)
                {
                    PlayerPrefs.SetInt("AssaultEquipped", index);
                    if (MainMenuManager.lastAssaultCard != null)
                        MainMenuManager.lastAssaultCard();

                }
                else
                {
                    PlayerPrefs.SetInt("SniperEquipped", index);
                    if (MainMenuManager.lastSniperCard != null)
                        MainMenuManager.lastSniperCard();
                }

                // Analytic To Get Gun Unlock By Watching RewardedVideo
                Analytics.CustomEvent("GunByVideo", new Dictionary<string, object>
            {
              { "level_index", weapon.PrefsIndex }
            });
#if UNITY_EDITOR
                Debug.Log("CustomEvent: " + "GunByVideo");
#endif
            }


            if (isLoadout)
            {
                LoadOutManager.Instance.SelectGun(weapon, GetComponent<Image>().color);
                LoadOutManager.Instance.EquipGunDelegate = EquipGun;
            }
            else
            {
                if (!gameObject.activeInHierarchy)
                {
                    WeaponStore.Instance._SelectWeaponButton(weapon, siblingIndex);
                }

                SetButtonStats();
            }
        }
        catch { }
    }

    private void OnDisable()
    {
        CancelInvoke("EnablingGunsCamera");
    }
}
