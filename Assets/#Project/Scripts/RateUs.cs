using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RateUs : MonoBehaviour
{
    public GameObject rateUsPanel, noInternetPanel, good, bad;
                                //rateUsButton, feedbackButton, laterButton,
    public GameObject[] filledStars;
    public Canvas _canvas;
    public AudioSource globalSource;
    public Camera animationCamera;
    public AudioClip BulletsGlide;
    private int number;
    public Transform[] BulletStars;
    public GameObject[] Stars;
    public Text TextField;

    private int star_1, star_2, star_3, star_4, star_5 = 0;

    private void Start()
    {
        //_canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //_canvas.worldCamera = animationCamera;
        //animationCamera.gameObject.SetActive(true);
        TextField.text = PlayerPrefs.GetString("Name");
    }
    private void OnEnable()
	{
        AdsManager.instance.RemoveAllBanners();
    }

	private void FillStar()
	{
		HideAllFilledStars ();
        for (int j = 0; j < BulletStars[number].childCount; j++)
        {
            BulletStars[number].GetChild(j).gameObject.SetActive(true);
        }
    }

	private void HideAllFilledStars()
	{
        for(int i =0;i<BulletStars.Length;i++)
        {
            for(int j = 0;j<BulletStars[i].childCount;j++)
            {
                BulletStars[i].GetChild(j).gameObject.SetActive(false);
            }
        }
		//foreach(GameObject go in filledStars)
		//{
		//	go.SetActive (false);
		//}
	}

	private void SendEmail()
	{
        string email = "gamexisg27@gmail.com";//"rcsm.help@gmail.com";

        string subject = MyEscapeURL ("Real Commando Secret Mission _v" + Application.version);
		string body = MyEscapeURL ("");

		Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}

	private string MyEscapeURL(string URL)
	{
		return UnityWebRequest.EscapeURL (URL).Replace ("+", "%20");
	}

    private IEnumerator AnimateStars()
    {
        filledStars[0].SetActive(true);
        yield return new WaitForSeconds(0.25f);
        filledStars[1].SetActive(true);
        yield return new WaitForSeconds(0.25f);
        filledStars[2].SetActive(true);
        yield return new WaitForSeconds(0.25f);
        filledStars[3].SetActive(true);
        yield return new WaitForSeconds(0.25f);
        filledStars[4].SetActive(true);
        yield return new WaitForSeconds(0.25f);
        HideAllFilledStars();
        yield return new WaitForSeconds(0.25f);

        StartCoroutine(AnimateStars());
    }

	#region Button Methods

	public void _StarButton(int amount)
	{
        switch(amount)
        {
            case 1:
                star_1 = star_2 = star_3 = star_4 = star_5 = 0;
                star_1 = 1;
                break;
            case 2:
                star_1 = star_2 = star_3 = star_4 = star_5 = 0;
                star_2 = 1;
                break;
            case 3:
                star_1 = star_2 = star_3 = star_4 = star_5 = 0;
                star_3 = 1;
                break;
            case 4:
                star_1 = star_2 = star_3 = star_4 = star_5 = 0;
                star_4 = 1;
                break;
            case 5:
                star_1 = star_2 = star_3 = star_4 = star_5 = 0;
                star_5 = 1;
                break;
        }

        StopAllCoroutines();
        AudioManager.instance.NormalClick();

        number = amount;
        
        for(int i = 0;i<5;i++)
            Stars[i].SetActive(false);
        Stars[amount].SetActive(true);
        FillStar();
        rateUsPanel.SetActive(false);
        if (amount < 3)
        {
            good.SetActive(false);
            bad.SetActive(true);
            //rateUsButton.SetActive(false);
            //feedbackButton.SetActive(true);
            //laterButton.SetActive(true);
        }
        else
        {
            _NowButton();
            //good.SetActive(true);
            //bad.SetActive(false);

            //rateUsButton.SetActive(true);
            //feedbackButton.SetActive(false);
            //laterButton.SetActive(true);
        }
    }

	public void _NowButton()
	{
		if(Application.internetReachability != NetworkReachability.NotReachable)
		{
            Analytics.CustomEvent("RateUsPanel", new Dictionary<string, object>
            {
            { "level_index", PlayerPrefs.GetInt("RateUsPanel") },
                { "1Star", star_1 },
                { "2Star", star_2 },
                { "3Star", star_3 },
                { "4Star", star_4 },
                { "5Star", star_5 },
                { "RateUs", 1 },
                { "Feedback", 0 },
                { "Skip", 0 }
            });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "RateUsPanel");
#endif
            PlayerPrefs.SetInt("RateUsPanel", PlayerPrefs.GetInt("RateUsPanel") + 1);

            this.gameObject.SetActive (false);
			PlayerPrefs.SetInt ("ISRATED", 1);
			Application.OpenURL ("https://play.google.com/store/apps/details?id=" + Application.identifier);
		}
		else
		{

            rateUsPanel.SetActive(false);
            noInternetPanel.SetActive (true);
		}
	}
	public void _FeedbackButton()
	{
		if(Application.internetReachability != NetworkReachability.NotReachable)
		{
            Analytics.CustomEvent("RateUsPanel", new Dictionary<string, object>
            {
            { "level_index", PlayerPrefs.GetInt("RateUsPanel") },
                { "1Star", star_1 },
                { "2Star", star_2 },
                { "3Star", star_3 },
                { "4Star", star_4 },
                { "5Star", star_5 },
                { "RateUs", 0 },
                { "Feedback", 1 },
                { "Skip", 0 }
            });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "RateUsPanel");
#endif
            PlayerPrefs.SetInt("RateUsPanel", PlayerPrefs.GetInt("RateUsPanel") + 1);

            this.gameObject.SetActive (false);
			PlayerPrefs.SetInt ("ISRATED", 1);
			SendEmail ();
		}
		else
		{
            bad.SetActive(false);
			noInternetPanel.SetActive (true);
		}
	}

	public void _LaterButton()
	{
        Analytics.CustomEvent("RateUsPanel", new Dictionary<string, object>
            {
            { "level_index" , PlayerPrefs.GetInt("RateUsPanel") },
                { "1Star", star_1 },
                { "2Star", star_2 },
                { "3Star", star_3 },
                { "4Star", star_4 },
                { "5Star", star_5 },
                { "RateUs", 0 },
                { "Feedback", 0 },
                { "Skip", 1 }
            });
#if UNITY_EDITOR
        Debug.Log("CustomEvent: " + "RateUsPanel");
#endif
        PlayerPrefs.SetInt("RateUsPanel", PlayerPrefs.GetInt("RateUsPanel") + 1);

        // Ads
        //GoogleMobileAdsManager.Instance.HideBanner();
        //GoogleMobileAdsManager.Instance.ShowMedBanner();
        //StartCoroutine(GameManager.Instance.LevelCompleteAd(0f));
       // AdsManager.instance.ShowTopSmallBanner();
        AudioManager.instance.BackButtonClick();
        this.gameObject.SetActive (false);
	}

    public void _NevenRateUs()
    {
        PlayerPrefs.SetInt("NeverRateUs", 1);
        AudioManager.instance.BackButtonClick();
        this.gameObject.SetActive(false);
      //  AdsManager.instance.ShowBothInterstitial();
    }
    public void PlaySound()
    {
        globalSource.PlayOneShot(BulletsGlide);
    }
   
    public void CloseFeedback()
    {
        rateUsPanel.SetActive(true);
        bad.SetActive(false);
    }
    public void CloseRedirect()
    {
        rateUsPanel.SetActive(true);
        good.SetActive(false);
    }
    public void CloseInternet()
    {
        rateUsPanel.SetActive(true);
        noInternetPanel.SetActive(false);
    }
    #endregion

    void OnDisable()
    {
        StopCoroutine("AnimateStars");
    }
}