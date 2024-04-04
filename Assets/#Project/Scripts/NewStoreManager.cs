using UnityEngine;
using UnityEngine.UI;

public class NewStoreManager : MonoBehaviour
{
    //public InAppManager _InAppManager;
    public GameObject snipersPanel, gadgetsPanel, goldPanel, spPanel, nadePanel, passesPanel, BulletsPanel, freeRewardsPanel, featuredPanel;
    public GameObject snipersButtonPressed, assaultButtonPressed, gadgetsPressedButton, nadePressedButton, BulletsPressedButton,
        goldPressedButton, spPressedButton, passesPressedButton, rewardsButtonPressed, featuredButtonPressed;
    public GameObject NadesValue, GoldValue, InjectionValue;
    public GameObject notEnoughCurrencyPanel;
    public GameObject purchaseTextGold, purchaseTextSP;
    public Image _image;
    public Sprite gold, sp;
    //public Text[] goldTitleText, spTitleText;
    public Text[] goldPriceText, goldDiscountedPriceText, spPriceText, spDiscountedPriceText, bundlesPricesText;
    public GameObject[] addsAvailibleTexts, addsNotAvailibleTexts;
    //Image TrailPanel;
    private bool _isGold = false;
    int LastPanel = 0;
    private void OnEnable()
    {
        LastPanel = 0;
        //TrailPanel = transform.GetChild(0).GetComponent<Image>();
       Invoke("SetAddsText" , 1);
        //_InAppManager.SetGoldPrice();
        //_InAppManager.SetSPPrice();
        //_InAppManager.SetPassPrices();
        //_InAppManager.SetGunPrices();
        //_InAppManager.SetBundlesPrices();
        //goldPriceText[0].text = _InAppManager.goldPrice_1;//health > 0 ? "Player is Alive" : "Player is Dead";
        if (goldPriceText[0].text == "")
            goldPriceText[0].text = "Purchase";
        //goldPriceText[1].text = _InAppManager.goldPrice_2;
        //goldPriceText[2].text = _InAppManager.goldPrice_3;
        //goldPriceText[3].text = _InAppManager.goldPrice_4;
        //goldPriceText[4].text = _InAppManager.goldPrice_5;
        //goldPriceText[5].text = _InAppManager.goldPrice_6;

        //spPriceText[0].text = _InAppManager.spPrice_1;
        if (spPriceText[0].text == "")
            spPriceText[0].text = "Purchase";
        //spPriceText[1].text = _InAppManager.spPrice_2;
        //spPriceText[2].text = _InAppManager.spPrice_3;
        //spPriceText[3].text = _InAppManager.spPrice_4;
        //spPriceText[4].text = _InAppManager.spPrice_5;
        //spPriceText[5].text = _InAppManager.spPrice_6;

        //goldDiscountedPriceText[0].text = _InAppManager.goldDiscountedPrice_2;
        if (goldDiscountedPriceText[0].text == "")
            goldDiscountedPriceText[0].text = "Purchase";
        //goldDiscountedPriceText[1].text = _InAppManager.goldDiscountedPrice_3;
        if (goldDiscountedPriceText[1].text == "")
            goldDiscountedPriceText[1].text = "Purchase";
        //goldDiscountedPriceText[2].text = _InAppManager.goldDiscountedPrice_4;
        if (goldDiscountedPriceText[2].text == "")
            goldDiscountedPriceText[2].text = "Purchase";
        //goldDiscountedPriceText[3].text = _InAppManager.goldDiscountedPrice_5;
        if (goldDiscountedPriceText[3].text == "")
            goldDiscountedPriceText[3].text = "Purchase";
        //goldDiscountedPriceText[4].text = _InAppManager.goldDiscountedPrice_6;
        if (goldDiscountedPriceText[4].text == "")
            goldDiscountedPriceText[4].text = "Purchase";

        //spDiscountedPriceText[0].text = _InAppManager.spDiscountedPrice_2;
        if (spDiscountedPriceText[0].text == "")
            spDiscountedPriceText[0].text = "Purchase";
        //spDiscountedPriceText[1].text = _InAppManager.spDiscountedPrice_3;
        if (spDiscountedPriceText[1].text == "")
            spDiscountedPriceText[1].text = "Purchase";
        //spDiscountedPriceText[2].text = _InAppManager.spDiscountedPrice_4;
        if (spDiscountedPriceText[2].text == "")
            spDiscountedPriceText[2].text = "Purchase";
        //spDiscountedPriceText[3].text = _InAppManager.spDiscountedPrice_5;
        if (spDiscountedPriceText[3].text == "")
            spDiscountedPriceText[3].text = "Purchase";
        //spDiscountedPriceText[4].text = _InAppManager.spDiscountedPrice_6;
        if (spDiscountedPriceText[4].text == "")
            spDiscountedPriceText[4].text = "Purchase";


        //passPricesText[0].text = _InAppManager.PremiumPass1;
        //passPricesText[1].text = _InAppManager.PremiumPass1Discount;
        //if (passPricesText[1].text == "")
        //    passPricesText[1].text = "Purchase";
        //passPricesText[2].text = _InAppManager.PremiumPass2;
        //passPricesText[3].text = _InAppManager.PremiumPass2Discount;
        //if (passPricesText[3].text == "")
        //    passPricesText[3].text = "Purchase";
        //passPricesText[4].text = _InAppManager.PremiumPass3;
        //passPricesText[5].text = _InAppManager.PremiumPass3Discount;
        //if (passPricesText[5].text == "")
        //    passPricesText[5].text = "Purchase";

        //bundlesPricesText[0].text = _InAppManager.Essential;
        //bundlesPricesText[1].text = _InAppManager.EssentialDiscount;
        //bundlesPricesText[2].text = _InAppManager.ProStarter;
        //bundlesPricesText[3].text = _InAppManager.ProStarterDiscount;
        //bundlesPricesText[4].text = _InAppManager.Extraordinary;
        //bundlesPricesText[5].text = _InAppManager.ExtraordinaryDiscount;
        //bundlesPricesText[6].text = _InAppManager.Premium;
        //bundlesPricesText[7].text = _InAppManager.PremiumDiscount;


        //Guns orignal
        //for (int i = 0; i < gunPrices.Length; i++)
        //    gunPrices[i].text = _InAppManager.GunPrice[i];


        //Guns discounts
        //for (int i = 0; i < gunPrices.Length; i++)
        //    gunDiscountPrcies[i].text = _InAppManager.GunPriceDiscount[i];
    }
    private void EnablePanel(GameObject go)
    {
        try
        {
            SetAddsText();
        }
        catch { }
        snipersPanel.SetActive(false);
       // assaultPanel.SetActive(false);
        gadgetsPanel.SetActive(false);
        goldPanel.SetActive(false);
        spPanel.SetActive(false);
        nadePanel.SetActive(false);
        passesPanel.SetActive(false);
        BulletsPanel.SetActive(false);
        freeRewardsPanel.SetActive(false);
        featuredPanel.SetActive(false);
        go.SetActive(true);
    }
    void SetAddsText()
    {
        for (int i = 0; i < addsAvailibleTexts.Length; i++)
        {
            try
            {
                if (AdsManager_AdmobMediation.Instance.IsRewardedLoaded())
                {
                    addsAvailibleTexts[i].SetActive(true);
                    addsNotAvailibleTexts[i].SetActive(false);
#if UNITY_EDITOR
                    Debug.Log("rewarded loaded");
#endif
                }
                else
                {
                    addsAvailibleTexts[i].SetActive(false);
                    addsNotAvailibleTexts[i].SetActive(true);
                }
#if UNITY_EDITOR
                Debug.Log("rewarded not loaded");
#endif
            }
            catch { }
        }
    }

    private void EnableButton(GameObject go)
    {
        try
        {
            snipersButtonPressed.SetActive(false);
            assaultButtonPressed.SetActive(false);
            gadgetsPressedButton.SetActive(false);
            nadePressedButton.SetActive(false);
            goldPressedButton.SetActive(false);
            spPressedButton.SetActive(false);
            passesPressedButton.SetActive(false);
            BulletsPressedButton.SetActive(false);
            rewardsButtonPressed.SetActive(false);
            BulletsPressedButton.SetActive(false);
            featuredButtonPressed.SetActive(false);
            go.SetActive(true);
        }
        catch { }
    }


    void EnableValues(GameObject pan)
    {
        GoldValue.SetActive(false);
        NadesValue.SetActive(false);
        InjectionValue.SetActive(false);

        pan.SetActive(true);
    }
    #region ButtonMethods
    public void _AssaultButton()
    {
        try
        {
            //TrailPanel.color = new Color(0.123487f, 0.3492858f, 0.7075472f, 0.21f);
            AudioManager.instance.BackButtonClick();
            EnableButton(assaultButtonPressed);
            //EnablePanel(assaultPanel);
            EnableValues(GoldValue);
            MainMenuManager.StoreRedirect = _AssaultButton;
        }
        catch { }
    }
    public void _BulletsButton()
    {
        //TrailPanel.color = new Color(0.123487f, 0.3492858f, 0.7075472f, 0.21f);
        AudioManager.instance.BackButtonClick();
        EnableButton(BulletsPressedButton);
        EnablePanel(BulletsPanel);
        EnableValues(GoldValue);
    }
    public void _SniperButton()
    {
        //TrailPanel.color = new Color(0, 255, 255, 0.14f);
        AudioManager.instance.BackButtonClick();
        EnableButton(snipersButtonPressed);
        EnablePanel(snipersPanel);
        EnableValues(GoldValue);
        MainMenuManager.StoreRedirect = _SniperButton;
    }
    public void _NadesButton()
    {
        //TrailPanel.color = new Color(0.3437611f, 0.4955146f, 0.7075472f, 0.19f);
        AudioManager.instance.BackButtonClick();

        EnableButton(nadePressedButton);
        EnablePanel(nadePanel);
        EnableValues(NadesValue);
    }
    public void _GadgetsButton()
    {
        //TrailPanel.color = new Color(0.2158686f, 0.4622642f, 0.2158686f, 0.19f);
        AudioManager.instance.BackButtonClick();

        EnableButton(gadgetsPressedButton);
        EnablePanel(gadgetsPanel);
        EnableValues(InjectionValue);
    }
    public void _PassesButton()
    {
        //TrailPanel.color = new Color(0.6320754f, 0.2529287f, 0.2176486f, 0.21f);
        AudioManager.instance.BackButtonClick();

        EnableButton(passesPressedButton);
        EnablePanel(passesPanel);
        EnableValues(GoldValue);
    }
    public void _GoldButton()
    {
        //TrailPanel.color = new Color(0.9150943f, 0.9087689f, 0.3064703f, 0.16f);
        AudioManager.instance.BackButtonClick();

        //if (WeaponStore.Instance.fullSpecificationPanel.activeInHierarchy)
        //{
        //    WeaponStore.Instance.fullSpecificationPanel.SetActive(false);
        //    WeaponStore.Instance.StorePanel.SetActive(true);
        //}

        EnableButton(goldPressedButton);
        EnablePanel(goldPanel);
        EnableValues(GoldValue);
    }
    public void _FeaturedButton()
    {
        //TrailPanel.color = new Color(0.9150943f, 0.9087689f, 0.3064703f, 0.16f);
        AudioManager.instance.BackButtonClick();

        EnableButton(featuredButtonPressed);
        EnablePanel(featuredPanel);
    }
    public void _SPButton()
    {
        //TrailPanel.color = new Color(0.8773585f, 0.5131935f, 0, 0.18f);
        AudioManager.instance.BackButtonClick();

        //if (WeaponStore.Instance.fullSpecificationPanel.activeInHierarchy)
        //{
        //    WeaponStore.Instance.fullSpecificationPanel.SetActive(false);
        //    WeaponStore.Instance.StorePanel.SetActive(true);
        //}

        EnableButton(spPressedButton);
        EnablePanel(spPanel);
        EnableValues(GoldValue);
        
    }
    public void _FreeRewardsButton()
    {
        //TrailPanel.color = new Color(0.8773585f, 0.5131935f, 0, 0.18f);
        AudioManager.instance.BackButtonClick();

        EnableButton(rewardsButtonPressed);
        EnablePanel(freeRewardsPanel);
        EnableValues(GoldValue);
    }

    public void _Redirect()
    {

    }
    #endregion
}
