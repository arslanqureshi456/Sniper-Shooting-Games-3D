using JetBrains.Annotations;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static InAppManager;
using static LoadOutManager;

public class WeaponStore : MonoBehaviour
{

    public GunInAppDelegate onGunInAppHandler;
    public InAppManager inAppManager;
    public static int weaponPopupButtonPressed = 0;

    //public InAppManager _InAppManager;
    public static WeaponStore Instance;
    public SetStateDelegate EquipGunDelegate;
    //public MultiPlayerMenu _multiPlayerMenu;
    public GameObject
        //allPressedButton,
        assaultPressedButton,
        sniperPressedButton;

    // Full specifications panel
    public GameObject
        assaultPanel,
        sniperPanel,
        fullSpecificationPanel,
        specsPanel,
        permissionPanel,
        fullBulletsSpecificationPanel,
        loadingPanel,
        tutorialPanel;
    public GameObject StorePanel;
    public Image priceIcon;
    public Sprite
        goldSprite,
        secretPointSprite;
    public Text
        weaponNameText,
        bulletsNameText,
        priceText,
        tutotialPriceText,
        adCountText;
    public GameObject
        speciComingSoonBTN,
        buyButton,
        inAppButton,
        watchVideoButton,
        ownedButton,
        playButton ,
        bulletsBuy,
        bulletsOwned,
        equipgun,
        equipedgun;
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
        magSizeText,
        gunsInAppPrice,
        gunsInAppDiscount;


    public Image ClassImage;

    public Transform weaponModelsContainer;
    public Transform BulletsModelsContainer;

    public NewStoreManager _newStoreManager;
    public GameObject[] Classes;
    public int currentIndex, currentPopupIndex = 0, currentBulletIndex = 0, currentOrderIndex;

    public GameObject previousGunsBtn;
    public GameObject nextGunsBtn;

    //RectTransform tempTrans;
    public GameObject[] GunsAdBTNs;

    public GameObject IAPAllAssaultBtn; // button to purchase all assault guns
    public GameObject IAPAllSniperBtn;  // button to purchase all sniper guns

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        _newStoreManager = GetComponent<NewStoreManager>();
    }

    private void Start()
    {
        #region Check Unlocked All Guns
        CheckUnlockedGuns();
        #endregion

        Invoke("hidebtns", 1);   
    }

    public void CheckUnlockedGuns()
    {
        if (currentOrderIndex < 19)
        {

            for (int i = 0; i < 19; i++)
            {
                if (PlayerPrefs.GetInt("weapon" + i) == 1)
                {
                    IAPAllAssaultBtn.SetActive(false);
                }
                else
                {
                    IAPAllAssaultBtn.SetActive(true);
                }
            }
            IAPAllSniperBtn.SetActive(false);
        }


        else
        {
            if (PlayerPrefs.GetInt("SniperEquipped") >= 19)
            {
                for (int i = 19; i < 25; i++)
                {
                    if (PlayerPrefs.GetInt("weapon" + i) == 1)
                    {
                        IAPAllSniperBtn.SetActive(false);
                    }
                    else
                    {
                        IAPAllSniperBtn.SetActive(true);
                    }
                }
            }
            else
            {
                IAPAllSniperBtn.SetActive(false);
            }
            IAPAllAssaultBtn.SetActive(false);
        }
    }

    void hidebtns()
    {
        if (GameManagerStatic.Instance.isFeaturedGuns == true)
        {
            previousGunsBtn.SetActive(false);
            nextGunsBtn.SetActive(false);
        }
        else if (GameManagerStatic.Instance.isFeaturedGuns == false)
        {
            previousGunsBtn.SetActive(true);
            nextGunsBtn.SetActive(true);
        }
#if UNITY_EDITOR
        print("isFeaturedGuns : " + GameManagerStatic.Instance.isFeaturedGuns);
#endif
    }

    private void EnableWeaponTypeButton(GameObject go)
    {
        //allPressedButton.SetActive(false);
        assaultPressedButton.SetActive(false);
        sniperPressedButton.SetActive(false);

        go.SetActive(true);
    }

    private void EnableWeaponPanel(int startIndex, int endIndex)
    {
        //HideAllWeaponPanels();
        //for (int i = startIndex; i <= endIndex; i++)
        //{
        //    weaponPanels[i].SetActive(true);
        //}
    }

    private void HideAllWeaponPanels()
    {
        //foreach (GameObject go in weaponPanels)
        //{
        //    go.SetActive(false);
        //}
    }

    private void HideAllWeapons()
    {
        for (int i = 0; i < weaponModelsContainer.childCount; i++)
            weaponModelsContainer.GetChild(i).gameObject.SetActive(false);
    }
    void MoveSLiderDelayed()
    {
        //tempTrans.localPosition = new Vector3(-350, 0, 0);
    }
    public void ShowSelectedWeapon()
    {
        //if (currentIndex < 13)
        //{
        //    assaultPanel.SetActive(true);
        //    tempTrans = assaultPanel.transform.GetChild(0).GetComponent<RectTransform>();
        //    tempTrans.anchoredPosition3D = new Vector3(currentIndex * -(tempTrans.GetComponent<GridLayoutGroup>().cellSize.x + tempTrans.GetComponent<GridLayoutGroup>().spacing.x), 0, 0);
        //}
        //else
        //{
        //    assaultPanel.SetActive(false);
        //    sniperPanel.SetActive(true);
        //    tempTrans = sniperPanel.transform.GetChild(0).GetComponent<RectTransform>();
        //    tempTrans.anchoredPosition3D = new Vector3((currentIndex - 6) * -(tempTrans.GetComponent<GridLayoutGroup>().cellSize.x + tempTrans.GetComponent<GridLayoutGroup>().spacing.x), 0, 0);
        //}
    }

    private void ProcessBulletPayment()
    {
#if UNITY_EDITOR
        Debug.Log("purchasing bull " + currentBulletIndex);
#endif
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - 1500);
        PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") - 650);

        PlayerPrefs.SetInt("bulletUnlocked-" + currentBulletIndex, 1);

        ConstantUpdate.Instance.UpdateCurrency();
        if (PlayerPrefs.GetInt("bulletUnlocked-" + currentBulletIndex).Equals(1))
        {
            bulletsBuy.SetActive(false);
            bulletsOwned.SetActive(true);
        }
        else
        {
            bulletsBuy.SetActive(true);
            bulletsOwned.SetActive(false);
        }
        //SetBulletCards();

        AudioManager.instance.ThankYouClick();
    }
    public void RewardedGunAd()
    {
        AdsManager_AdmobMediation.isGunAd = true;
        FakeLoadingReward.instance.FakeLoadingCanvas.SetActive(true);
    }
    public void HideAds()
    {
        GunsAdBTNs[0].SetActive(false);
        GunsAdBTNs[1].SetActive(false);
    }

    #region ButtonMethods
    public void _AllButton()
    {
        AudioManager.instance.StoreButtonClick();
        //EnableWeaponTypeButton(allPressedButton);
        EnableWeaponPanel(0, 9);
    }

    public void _AssaultButton()
    {
        AudioManager.instance.StoreButtonClick();
        EnableWeaponTypeButton(assaultPressedButton);
        EnableWeaponPanel(0, 5);
    }

    public void _SniperButton()
    {
        AudioManager.instance.StoreButtonClick();
        EnableWeaponTypeButton(sniperPressedButton);
        EnableWeaponPanel(6, 9);
    }

    void ResetClasses()
    {
        for (int i = 0; i < Classes.Length; i++)
        {
            Classes[i].SetActive(false);
        }
    }

    private int tempIndex = 0;
    private bool once = false;
    [HideInInspector]public int gunNumber;
    public void _SelectWeaponButton(Weapon weapon, int index)
    {
#if UNITY_EDITOR
        print("PlayerPrefs.GetInt(weapon.PrefsIndex)" + index);
#endif
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
        if(!AudioManager.instance.otherAudioSource.isPlaying)
            AudioManager.instance.StoreButtonClick();
        HideAllWeapons();
        ShowSelectedWeapon();

        currentIndex = weapon.PrefsIndex;
        currentOrderIndex = index;
        gunNumber = index;
        speciComingSoonBTN.SetActive(false);
        StorePanel.SetActive(false);
        weaponModelsContainer.GetChild(currentIndex).gameObject.SetActive(true);
        weaponNameText.text = System.String.Empty + weapon._name;

        damageSlider.fillAmount = weapon._baseDamage / 100;
        fireRateSlider.fillAmount = 0.06f / weapon._baseFireRate;
        accuracySlider.fillAmount = weapon._baseAccuracy / 100;
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
        ClassImage.color = weapon.ClassColor;
        //classText.text = weapon.Class;

        if (weapon.isComingSoon)
        {
            buyButton.SetActive(false);
            watchVideoButton.SetActive(false);
            inAppButton.SetActive(false);
            ownedButton.SetActive(false);
            speciComingSoonBTN.SetActive(true);
            playButton.SetActive(false);
            equipgun.SetActive(false);
            equipedgun.SetActive(false);
        }
        else
        {
            if (PlayerPrefs.GetInt("weapon" + weapon.PrefsIndex).Equals(1))
            {
                buyButton.SetActive(false);
                watchVideoButton.SetActive(false);
                inAppButton.SetActive(false);
                if (GameManagerStatic.Instance.GunsStoreFrom == 1)
                {
                    ownedButton.SetActive(false);
                    playButton.SetActive(true);
                }
                else
                {
                    ownedButton.SetActive(true);
                    playButton.SetActive(false);
                }
               // equipgun.SetActive(true);
                if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER && PlayerPrefs.GetInt("SniperEquipped") == index)
                {
                    equipedgun.SetActive(true);
                    equipgun.SetActive(false);
                }
                else if(PlayerPrefs.GetInt("AssaultEquipped") == index)
                {
                    equipedgun.SetActive(true);
                    equipgun.SetActive(false);
                }
                else
                {
                    equipedgun.SetActive(false);
                    equipgun.SetActive(true);
                }

                //  MainMenuManager.Instance.gunsCamera.SetActive(true);
            }
            else
            {
                buyButton.SetActive(true);
                watchVideoButton.SetActive(true);
                inAppButton.SetActive(true);
                ownedButton.SetActive(false);
                playButton.SetActive(false);
                equipgun.SetActive(false);
                equipedgun.SetActive(false);
                //gunsInAppPrice.text = _InAppManager.GetPriceWithId(weapon.RealPriceID);
                if (gunsInAppPrice.text == "")
                    gunsInAppPrice.text = "Purchase";
                //gunsInAppDiscount.text = _InAppManager.GetPriceWithId(weapon.DiscountPriceID);
                
                if (weapon._spPrice == 0)// Assault
                {
                    priceIcon.sprite = goldSprite;
                    tutotialPriceText.text = priceText.text = System.String.Empty + weapon._goldPrice;
                    adCountText.text = (PlayerPrefs.GetInt("AdCount" + weapon.PrefsIndex)) + "/" + (weapon.adCount);
                }
                else// Sniper
                {
                    priceIcon.sprite = secretPointSprite;
                    tutotialPriceText.text = priceText.text = System.String.Empty + weapon._spPrice;
                    adCountText.text = (PlayerPrefs.GetInt("AdCount" + weapon.PrefsIndex)) + "/" + (weapon.adCount);
                }
            }
        }
        if(GameManagerStatic.Instance.GunsStoreFrom == 0)
        {
            equipgun.SetActive(false);
            equipedgun.SetActive(false);
        }

        if (!once)
        {
            once = true;
            tempIndex = index;
        }

        if (MainMenuManager.weaponModeButtonPressed.Equals(1) &&
            index == tempIndex)
        {
            tutorialPanel.SetActive(true);
            watchVideoButton.GetComponent<Button>().interactable = false;
            inAppButton.GetComponent<Button>().interactable = false;
            nextGunsBtn.GetComponent<Button>().interactable = false;
            previousGunsBtn.GetComponent<Button>().interactable = false;
            playButton.GetComponent<Image>().enabled = false;
            playButton.transform.GetChild(0).gameObject.SetActive(false);
            playButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
            tutorialPanel.SetActive(false);

        if (PlayerPrefs.GetInt("AssaultEquipped").Equals(tempIndex) ||
            PlayerPrefs.GetInt("SniperEquipped").Equals(tempIndex))
            MainMenuManager.weaponModeButtonPressed = 0;

        fullSpecificationPanel.SetActive(true);

        if (GameManagerStatic.Instance.isFeaturedGuns == false)
        {
            previousGunsBtn.SetActive(true);
            nextGunsBtn.SetActive(true);
        }
    }

    public void _SelectBulletButton(ScriptableBullets bullet, int index)
    {
        AudioManager.instance.StoreButtonClick();
        BulletsModelsContainer.GetChild(currentBulletIndex).gameObject.SetActive(false);
        currentBulletIndex = index;
        BulletsModelsContainer.GetChild(currentBulletIndex).gameObject.SetActive(true);

        StorePanel.SetActive(false);
        //TITLES
        bulletsNameText.text = bullet.bulletName;

        fullBulletsSpecificationPanel.SetActive(true);
    }
    

    public void _EquipBullet(ScriptableBullets Bullet)
    {
        AudioManager.instance.StoreButtonClick();
        PlayerPrefs.SetInt("equippedBullet", Bullet.PrefIndex);
        //SetBulletCards();
    }


    public void GunSpecificationBuy()
    {
        MainMenuManager.buyDeletegate(true);
        MainMenuManager.Instance._GrantPermission(); // Adnan
        MainMenuManager.Instance.gunsCamera.SetActive(false);
        //MainMenuManager.Instance.gunsCamera.SetActive(true) ;
        //MainMenuManager.Instance.gunsCamera.GetComponent<Camera>().depth = 2;
    }
    public void GunSpecificationInApp()
    {
        MainMenuManager.onInAppDelegate();
    }



    public void _CloseFullSpecificationPanelButton()
    {
        if (MainMenuManager.Instance.mainMenuPanel.activeInHierarchy)
        {
#if UNITY_ANDROID
            Debug.Log("Debug : Load Out To Menu Screen");
#endif
        }
        else
        {
#if UNITY_ANDROID
            Debug.Log("Debug : Levels Load Out To Level Screen");
#endif
        }

        MainMenuManager.weaponModeButtonPressed = 0;
        AudioManager.instance.BackButtonClick();
        fullSpecificationPanel.SetActive(false);


        if (weaponPopupButtonPressed.Equals(1))
        {
            weaponPopupButtonPressed = 0;
            loadingPanel.SetActive(true);
            SceneManager.LoadScene("GameScene");
        }
        else if (GameManagerStatic.Instance.GunsStoreFrom == 1)
        {
            MainMenuManager.Instance.storePanel.SetActive(false);
            MainMenuManager.Instance.modeSelectionPanel.SetActive(true);

        }
        else
        {
            #region oldwork
            //StorePanel.SetActive(true);
            //ShowSelectedWeapon();storePanel.SetActive(false);
            #endregion

            StorePanel.SetActive(true);
            MainMenuManager.Instance.storePanel.SetActive(false);
        }

        GameManagerStatic.Instance.GunsStoreFrom = 0;
#if UNITY_EDITOR
        print("GameManagerStatic.Instance.GunsStoreFrom : " + GameManagerStatic.Instance.GunsStoreFrom);
#endif
        if (GameManagerStatic.Instance.isFeaturedGuns == true)
        {
            MainMenuManager.Instance._CloseStoreButton();
        }
        GameManagerStatic.Instance.isFeaturedGuns = false;
#if UNITY_EDITOR
        print("isFeaturedGuns : " + GameManagerStatic.Instance.isFeaturedGuns);
#endif
        GameManagerStatic.Instance.interstitial = "Interstitial";
        FakeLoadingInterstitial.instance.FakeLoadingCanvas.SetActive(true);
    }
    public void _CloseFullSpecificationBulletsPanelButton()
    {
        AudioManager.instance.BackButtonClick();
        fullBulletsSpecificationPanel.SetActive(false);
        StorePanel.SetActive(true);
    }
    public void NextGun()
    {
        if(currentOrderIndex == 18)
            MainMenuManager.Instance.specificationDelegates[0]();
        //else if(currentOrderIndex == 24)
            else if (currentOrderIndex == 21)
                    MainMenuManager.Instance.specificationDelegates[19]();
        else
            MainMenuManager.Instance.specificationDelegates[++currentOrderIndex]();

        inAppManager.SetGunHandler();
    }
    public void PreviousGun()
    {
        if(currentOrderIndex == 0)
            MainMenuManager.Instance.specificationDelegates[18]();
        else if (currentOrderIndex == 19)
            MainMenuManager.Instance.specificationDelegates[21]();

        //MainMenuManager.Instance.specificationDelegates[24]();

        else
            MainMenuManager.Instance.specificationDelegates[--currentOrderIndex]();


        inAppManager.SetGunHandler();
    }

    public void WatchVideoButton()
    {
        MainMenuManager.gunDelegate();
    }

    public void PlayButton()
    {
        try
        {
#if UNITY_ANDROID
                Debug.Log("Debug : LoadOut To Gameplay Scene");
#endif
                MainMenuManager.Instance.modeSelectionPanel.SetActive(false);
                MainMenuManager.Instance.StartLoading();
            //print("LoadOutManager_Level : " + PlayerPrefs.GetInt("currentLevel"));
            AudioManager.instance.LoadOutSelectSound();
            AudioManager.instance.backgroundMusicSouce.GetComponent<AudioSource>().enabled = false;
            MainMenuManager.Instance.storePanel.SetActive(false);
          
            if (MainMenuManager.Instance.tutorialPanels[3].activeSelf)
            {
                Analytics.CustomEvent("Loadout_Tut", new Dictionary<string, object>//Loadout Screen
        {
            { "level_index" , 1 }
        });
            }
        }
        catch { }
    }

    public void EquipGun()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            PlayerPrefs.SetInt("SniperEquipped", gunNumber);
            equipedgun.SetActive(true);
            equipgun.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("AssaultEquipped", gunNumber);
            equipedgun.SetActive(true);
            equipgun.SetActive(false);
        }
        AudioManager.instance.NormalClick();
    }

    //specificationDelegates[PlayerPrefs.GetInt("AssaultEquipped")] ();
   
    #endregion

    private void OnDisable()
    {
        CancelInvoke("hidebtns");
        CancelInvoke("hideownedatstart");
    }
}