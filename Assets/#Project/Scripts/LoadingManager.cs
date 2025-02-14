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
        if (PlayerPrefs.GetInt("AllowSessionAd").Equals(0))
        {
           // AdsManager.instance.ShowBottomLeftCubeBanner();
            // loadingBanner();
        }
        else
        {
          //  AdsManager.instance.RemoveAllBanners();
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
        //if (GoogleMobileAdsManager.Instance.medBannerView != null)
        //{
        //    GoogleMobileAdsManager.Instance.RePosition(GoogleMobileAds.Api.AdPosition.BottomLeft);
        //    GoogleMobileAdsManager.Instance.ShowMedBanner();
       // AdsManager.instance.ShowBottomLeftCubeBanner();
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.2f);
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
#if UNITY_EDITOR
                print("Ad Session Shown");
#endif
          //  AdsManager.instance.ShowBottomLeftCubeBanner();
           // AdsManager.instance.ShowBothInterstitial();
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
        StopCoroutine("LoadScene");
    }
}
