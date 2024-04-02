using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private void OnEnable()
    {
        // Unity Analytics
        GoogleMobileAdsManager.handleFullScreenAdClose += loadingBanner;
        if (PlayerPrefs.GetInt("AllowSessionAd").Equals(0))
        {
            GoogleMobileAdsManager.Instance.HideBanner();
           // loadingBanner();
        }
        else
        {
            GoogleMobileAdsManager.Instance.HideBanner();
        }
        if (MainMenuManager.Instance.tutorialPanels[0].activeSelf)
        {
            Analytics.CustomEvent("Loading_Tut", new Dictionary<string, object>
        {
            { "level_index", 1 }
        });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "Loading_Tut");
#endif
        }

        StartCoroutine(LoadScene());
    }

    private void loadingBanner()
    {
        if (GoogleMobileAdsManager.Instance.medBannerView != null)
        {
            GoogleMobileAdsManager.Instance.RePosition(GoogleMobileAds.Api.AdPosition.BottomLeft);
            GoogleMobileAdsManager.Instance.ShowMedBanner();
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.2f);
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.GetInt("AllowSessionAd").Equals(1) && GoogleMobileAdsManager.Instance.IsAdmobInterstitialLoaded() && !GoogleMobileAdsManager.Instance.IsLowMemory())
            {
#if UNITY_EDITOR
                print("Ad Session Shown");
#endif
                    GoogleMobileAdsManager.Instance.ShowInterstitial();
            }
            else
            {
                loadingBanner();
            }
        }
        yield return new WaitForSeconds(2);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void OnDisable()
    {
        GoogleMobileAdsManager.handleFullScreenAdClose -= loadingBanner;
        StopCoroutine("LoadScene");
    }
}
