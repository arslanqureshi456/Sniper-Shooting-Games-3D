using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class ModeUnlock : MonoBehaviour
{
    public InAppManager _InAppManager;
    public Image currencyIcon;
    public Sprite gold, sp;
    public Text adCountText, discountedPriceText, currencyText;
    public Mode[] _modes;

    public int index = 0;

    private void OnEnable()
    {
        foreach(Mode _mode in _modes)
        {
            if (_mode.index.Equals((int)LevelSelectionNew.modeSelection))
            {
                index = _mode.index;
                adCountText.text = (PlayerPrefs.GetInt("ModeAdCount" + _mode.index)) + "/" + _mode.adCount;
                //discountedPriceText.text = System.String.Empty + _InAppManager.GetPrice(_mode.discountedPriceID);
                currencyText.text = System.String.Empty + (_mode.goldPrice +
                    _mode.spPrice);

                if (_mode.goldPrice > 0)
                    currencyIcon.sprite = gold;
                else
                    currencyIcon.sprite = sp;
            }
        }
    }

    string modeName = "";
    private void CheckAdCount(int index)
    {
        switch (index)
        {
            case 1:
                modeName = "Sniper";
                break;
            case 2:
                modeName = "TDM";
                break;
            case 3:
                modeName = "FFA";
                break;
            case 4:
                modeName = "BR";
                break;
            case 5:
                modeName = "CS";
                break;
        }

        foreach (Mode _mode in _modes)
        {
            Analytics.CustomEvent("ModeAdCount_" + modeName, new Dictionary<string, object>
        {
            { "level_index" , PlayerPrefs.GetInt("ModeAdCount" + index) }
        });

            if (_mode.index.Equals(index))
            {
                if (PlayerPrefs.GetInt("ModeAdCount" + index) == _mode.adCount)
                {
                    PlayerPrefs.SetInt("Mode" + index, 1);
                    LevelSelectionNew.Instance.ModeChanged(index);
                    LevelSelectionNew.Instance.ModeLocksCheck();
                    CloseButton();
                }

                adCountText.text = (PlayerPrefs.GetInt("ModeAdCount" + index)) + "/" + _mode.adCount;
            }
        }
    }

    public void WatchVideoButton()
    {
        try
        {
            if (GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
            {
                GoogleMobileAdsManager.ModeAdHandler = CheckAdCount;
                GoogleMobileAdsManager.Instance.modeIndex = index;
                GoogleMobileAdsManager.Instance.ShowRewarded();
                GoogleMobileAdsManager.Instance.HideBanner();
            }
            else if (UnityAdsManager.Instance.IsRewardedVideoReady())
            {
                UnityAdsManager.ModeAdHandler = CheckAdCount;
                UnityAdsManager.Instance.modeIndex = index;
                UnityAdsManager.Instance.ShowUnityRewardedVideoAd();
                GoogleMobileAdsManager.Instance.HideBanner();
            }
        }
        catch { }
    }

    public void InAppButton()
    {
        foreach (Mode _mode in _modes)
        {
            if (_mode.index.Equals(index))
            {
//                _InAppManager.PurchaseMode(_mode.discountedPriceID, _mode.index);
                CloseButton();
            }
        }
    }

    public void CurrencyBuyButton()
    {
        foreach (Mode _mode in _modes)
        {
            if (_mode.index.Equals(index))
            {
                if (PlayerPrefs.GetInt("gold") >= _mode.goldPrice &&
                PlayerPrefs.GetInt("secretPoints") >= _mode.spPrice)
                {
                    PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - _mode.goldPrice);
                    PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") - _mode.spPrice);
                    ConstantUpdate.Instance.UpdateCurrency();

                    PlayerPrefs.SetInt("Mode" + index, 1);
                    LevelSelectionNew.Instance.ModeChanged(index);
                    LevelSelectionNew.Instance.ModeLocksCheck();
                    CloseButton();
                }
                else
                {
                    if (_mode.spPrice == 0)//gold
                    {
                        if ((_mode.goldPrice - PlayerPrefs.GetInt("gold")) <= 220)
                        {
                            MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(true);
                            MainMenuManager.Instance.isGoldRewardedADPopup = true;
                        }
                        else
                            MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(true);
                    }
                    else if ((_mode.spPrice - PlayerPrefs.GetInt("secretPoints")) <= 100)
                    {
                        MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(false);
                        MainMenuManager.Instance.isSPRewardedADPopup = true;
                    }
                    else
                        MainMenuManager.Instance.EnableNotEnoughCurrencyPanel(false);
                }
            }
        }
    }

    public void CloseButton()
    {
        this.gameObject.SetActive(false);
    }
}
