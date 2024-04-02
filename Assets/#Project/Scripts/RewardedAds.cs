using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Linq;

public class RewardedAds : MonoBehaviour
{
	public static RewardedAds Instance;

    public GameObject rewardPanel;
    public Text rewardText;
	[HideInInspector] public bool freeExplosive_G65 = false, freeAdrenaline_25 = false, freeGold_12 = false, freeSP_8 = false;
	public Text[] AddCounters;

    private void OnEnable()
    {
		ShowAddsSeenCount();

	}

    public void Awake()
	{
		MakeSingleton ();
	}

    private void MakeSingleton ()
	{
		if(Instance != null)
		{
			Destroy (gameObject);
		}
		else
		{
			Instance = this;
		}
	}

    public void ShowRewardPanel()
    {
        rewardPanel.SetActive(true);
#if UNITY_EDITOR
        Debug.Log("ShowRewardPanel");
#endif
	}

    #region Button Methods
	public void _FreeExplosive_G65Button()
	{
		try
		{
			if (GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
            {
#if UNITY_ANDROID
                Debug.Log("Debug : Admob_Reward For FreeExplosive_G65Button");
#endif
                freeExplosive_G65 = true;
				freeAdrenaline_25 = false;
				freeGold_12 = false;
				freeSP_8 = false;
				GoogleMobileAdsManager.Instance.ShowRewarded();
				GoogleMobileAdsManager.Instance.HideBanner();
            }
            else if (UnityAdsManager.Instance.IsRewardedVideoReady())
            {
#if UNITY_ANDROID
                Debug.Log("Debug : Unity_Reward For FreeExplosive_G65Button");
#endif
                freeExplosive_G65 = true;
				freeAdrenaline_25 = false;
				freeGold_12 = false;
				freeSP_8 = false;
				UnityAdsManager.Instance.ShowUnityRewardedVideoAd();
                GoogleMobileAdsManager.Instance.HideBanner();
            }
			else
			{
				GoogleMobileAdsManager.handleFullScreenAdClose?.Invoke();
			}
        }
        catch { }
	}
    public void _FreeAdrenaline_25Button()
	{
		try
		{
			if (GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
            {
#if UNITY_ANDROID
                Debug.Log("Debug : Admob_Reward For FreeAdrenaline_25Button");
#endif
                freeExplosive_G65 = false;
				freeAdrenaline_25 = true;
				freeGold_12 = false;
				freeSP_8 = false;
				GoogleMobileAdsManager.Instance.ShowRewarded();
                GoogleMobileAdsManager.Instance.HideBanner();
            }
            else if (UnityAdsManager.Instance.IsRewardedVideoReady())
            {
#if UNITY_ANDROID
                Debug.Log("Debug : Unity_Reward For FreeAdrenaline_25Button");
#endif
                freeExplosive_G65 = false;
				freeAdrenaline_25 = true;
				freeGold_12 = false;
				freeSP_8 = false;
				UnityAdsManager.Instance.ShowUnityRewardedVideoAd();
                GoogleMobileAdsManager.Instance.HideBanner();
            }
            else
            {
                GoogleMobileAdsManager.handleFullScreenAdClose?.Invoke();
            }
        }
        catch { }
    }

	public void _FreeGold_12Button()
	{
		try
		{
			if (GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
            {
#if UNITY_ANDROID
                Debug.Log("Debug : Admob_Reward For FreeGold_12Button");
#endif
                GoogleMobileAdsManager.Instance.ShowRewardedByName("freeGold_12");
                GoogleMobileAdsManager.Instance.HideBanner();
            }
            else
			if (UnityAdsManager.Instance.IsRewardedVideoReady())
            {
#if UNITY_ANDROID
                Debug.Log("Debug : Unity_Reward For FreeGold_12Button");
#endif
                UnityAdsManager.Instance.ShowRewardedByName("freeGold_12");
                GoogleMobileAdsManager.Instance.HideBanner();
            }
            else
            {
                GoogleMobileAdsManager.handleFullScreenAdClose?.Invoke();
                GoogleMobileAdsManager.Instance.RequestRewarded();
            }
        }
        catch { }
	}

	public void _FreeSP_8Button()
	{
		try
		{
			if (GoogleMobileAdsManager.Instance.isAdmobRewardLoaded())
            {
#if UNITY_ANDROID
                Debug.Log("Debug : Admob_Reward For FreeSP_8Button");
#endif
                GoogleMobileAdsManager.Instance.ShowRewardedByName("freeSP_8");
                GoogleMobileAdsManager.Instance.HideBanner();
            }
            else
			if (UnityAdsManager.Instance.IsRewardedVideoReady())
            {
#if UNITY_ANDROID
                Debug.Log("Debug : Unity_Reward For FreeSP_8Button");
#endif
                UnityAdsManager.Instance.ShowRewardedByName("freeSP_8");
                GoogleMobileAdsManager.Instance.HideBanner();
            }
            else
			{
                GoogleMobileAdsManager.handleFullScreenAdClose?.Invoke();
                GoogleMobileAdsManager.Instance.RequestRewarded();
			}
		}
        catch { }
	}

	public void _CloseButton()
    {
		AudioManager.instance.BackButtonClick();
        rewardPanel.SetActive(false);
		switch(MainMenuManager.Instance.gunAd)
        {
			case MainMenuManager.GunAdType.assault:
				WeaponStore.Instance.GetComponent<NewStoreManager>()._AssaultButton();
				break;
			case MainMenuManager.GunAdType.sniper:
				WeaponStore.Instance.GetComponent<NewStoreManager>()._SniperButton();
				break;
		}
		MainMenuManager.Instance.gunAd = MainMenuManager.GunAdType.none;

	}
	public void ShowAddsSeenCount()
    {
        AddCounters[0].text = PlayerPrefs.GetInt("nadeAddCount") + "/2";
        AddCounters[1].text = PlayerPrefs.GetInt("adralineAddCount") + "/2";
        AddCounters[2].text = PlayerPrefs.GetInt("nadeAddCount") + "/2";
        AddCounters[3].text = PlayerPrefs.GetInt("adralineAddCount") + "/2";
    }
	#endregion
}