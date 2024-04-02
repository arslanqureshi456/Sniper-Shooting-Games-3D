using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GadgetStore : MonoBehaviour
{
    //public InAppManager _InAppManager;
    public GameObject panel, adrenaline_25Panel, explosive_65Panel;
    public GameObject permissionPanel;

    public int[] price;
    public int[] reward;
    public Text[] priceText;
    public Text[] iap_PriceText;
    public Text[] iap_DisPriceText;
    public Text[] rewardText;

    private NewStoreManager _newStoreManager;
    private int currentIndex = 0;

    private void OnEnable()
    {
        _newStoreManager = GetComponent<NewStoreManager>();

        for (int i = 0; i < 4; i++)
        {
            priceText[i].text = System.String.Empty + price[i];
            if (i >= 0 && i <= 1)
                rewardText[i].text = "Buy <size=40><color=orange>" + reward[i] + "</color></size> Adrenaline-H25";
            else if (i >= 2 && i <= 3)
                rewardText[i].text = "Buy <size=40><color=orange>" + reward[i] + "</color></size> Explosive-G65";
        }

        //_InAppManager.SetAdrenalinePrice();
        //_InAppManager.SetGrenadePrice();
        //iap_PriceText[0].text = System.String.Empty + _InAppManager.adrenaline_h25_1Price;
        //iap_PriceText[1].text = System.String.Empty + _InAppManager.adrenaline_h25_2Price;
        //iap_PriceText[2].text = System.String.Empty + _InAppManager.grenade_g65_1Price;
        //iap_PriceText[3].text = System.String.Empty + _InAppManager.grenade_g65_2Price;

        //iap_DisPriceText[0].text = System.String.Empty + _InAppManager.disAdrenaline_h25_1Price;
        if (iap_DisPriceText[0].text == "")
            iap_DisPriceText[0].text = "Purchase";
        //iap_DisPriceText[1].text = System.String.Empty + _InAppManager.disAdrenaline_h25_2Price;
        //if (iap_DisPriceText[1].text == "")
        //    iap_DisPriceText[1].text = "Purchase";
        //iap_DisPriceText[2].text = System.String.Empty + _InAppManager.disGrenade_g65_1Price;
        if (iap_DisPriceText[2].text == "")
            iap_DisPriceText[2].text = "Purchase";
        //iap_DisPriceText[3].text = System.String.Empty + _InAppManager.disGrenade_g65_2Price;
        //if (iap_DisPriceText[3].text == "")
        //    iap_DisPriceText[3].text = "Purchase";
    }

    private void EnablePanel(GameObject go)
    {
        adrenaline_25Panel.SetActive(false);
        explosive_65Panel.SetActive(false);

        go.SetActive(true);
    }

    private void ProcessPayment()
    {
        PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") - price[currentIndex]);
        GiveReward();
        ConstantUpdate.Instance.UpdateCurrency();
        _ClosePermissionPanelButton();
    }

    private void GiveReward()
    {
        switch (currentIndex)
        {
            case 0:
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + reward[currentIndex]);
                AudioManager.instance.ThankYouClick();
                break;
            case 1:
                PlayerPrefs.SetInt("Injection", PlayerPrefs.GetInt("Injection") + reward[currentIndex]);
                AudioManager.instance.ThankYouClick();
                break;
            case 2:
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + reward[currentIndex]);
                AudioManager.instance.ThankYouClick();
                break;
            case 3:
                PlayerPrefs.SetInt("Grenade", PlayerPrefs.GetInt("Grenade") + reward[currentIndex]);
                AudioManager.instance.ThankYouClick();
                break;
        }
    }

    #region ButtonMethods
    public void _Adrenaline_25Button()
    {
        AudioManager.instance.StoreButtonClick();
        EnablePanel(adrenaline_25Panel);
    }

    public void _Explosive_65Button()
    {
        AudioManager.instance.StoreButtonClick();
        EnablePanel(explosive_65Panel);
    }

    public void _BuyButton(int index)
    {
        AudioManager.instance.StoreButtonClick();
        currentIndex = index;
        if (PlayerPrefs.GetInt("secretPoints") >= price[currentIndex])
        {
            permissionPanel.SetActive(true);
        }
        else if ((price[currentIndex] - PlayerPrefs.GetInt("secretPoints")) <= 100)
        {
            MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(false);
            MainMenuManager.Instance.isSPRewardedADPopup = true;
        }
        else
        {
            MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(false);
        }
    }

    public void _GrantPermission()
    {
        AudioManager.instance.StoreButtonClick();
        ProcessPayment();
        Camera.main.depth = 2;
    }

    public void _CloseButton()
    {
        AudioManager.instance.BackClickNew();
        EnablePanel(panel);
    }

    public void _ClosePermissionPanelButton()
    {
        AudioManager.instance.BackClickNew();
        permissionPanel.SetActive(false);
        Camera.main.depth = 2;
    }
    #endregion
}
