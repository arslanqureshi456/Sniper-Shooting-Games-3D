using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpController : MonoBehaviour
{
    public Text previousText, nextText,SPText;
    public Canvas _canvas;
    public Camera animationCamera;
    public AudioClip medal, previous, next;
    public AudioSource globalSource;
    float currentSP = 0, TargetSp = 0;
    float factor = 0;
    int count = 0;

    private void Start()
    {
        previousText.text = System.String.Empty + (PlayerPrefs.GetInt("LastLevel") - 1);
        nextText.text = System.String.Empty + (PlayerPrefs.GetInt("LastLevel"));
        //_canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //_canvas.worldCamera = animationCamera;
        //animationCamera.gameObject.SetActive(true);
        TargetSp = PlayerPrefs.GetInt("tempsecretPoints");
        //currentSP = PlayerPrefs.GetInt("secretPoints");
        factor = TargetSp / 100;
        enabled = false;
    }
    private void Update()
    {
        count++;
        currentSP += factor;
        if(count >= 100)
        {
            currentSP = TargetSp;
            PlayerPrefs.SetInt("tempsecretPoints",0);
            PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") + (int)currentSP);
            enabled = false;
        }
        SPText.text = "" + (int)currentSP;
    }
    public void PlayMedal()
    {
        globalSource.PlayOneShot(medal);
    }

    public void PlayNext()
    {
        globalSource.PlayOneShot(next);
    }

    public void PlayPrevious()
    {
        globalSource.PlayOneShot(previous);
    }
    public void StartSp()
    {
        enabled = true;
    }
    public void _ContinueButton()
    {
        //Ads
        //if (!MultiPlayerMenu.isMulti)
        //{
        //    if (!GameManager.Instance.rateUsPanel.activeSelf)
        //    {
        //        GoogleMobileAdsManager.Instance.HideBanner();
        //        GoogleMobileAdsManager.Instance.ShowMedBanner();
        //    }
        //    // StartCoroutine(GameManager.Instance.LevelCompleteAd(0f));
        //}
        //else
        //{
           // GoogleMobileAdsManager.Instance.HideBanner();
           // GoogleMobileAdsManager.Instance.ShowMedBanner();
        //}
        animationCamera.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        //_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
}
