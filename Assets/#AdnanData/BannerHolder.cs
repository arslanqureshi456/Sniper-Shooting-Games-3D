using UnityEngine;

public class BannerHolder : MonoBehaviour
{
    public Texture2D tex;

    bool check;

    float height;
    float width;

    public float heightPadding = 15.0f; //Set it to 0.025f
    public float widthPadding = 30.0f; //Set it to 0.0175f

    void OnEnable()
    {

        if (Application.internetReachability != NetworkReachability.NotReachable || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            check = true;
        }
        else
        {
           check=false;
        }

        if (PlayerPrefs.GetInt("ADSUNLOCK") == 1 || !GoogleMobileAdsManager.Instance.IsSmallBannerLoaded())
        {
            gameObject.SetActive(false);
            return;
        }

        height = GoogleMobileAdsManager.Instance.bannerBGHeight;
        width = GoogleMobileAdsManager.Instance.bannerBGWidth;
    }


    void OnGUI()
    {
        if (check)
        {
            if (GoogleMobileAdsManager.Instance)
            {
                GUI.DrawTexture(new Rect((((Screen.width/2) - (width/2)) - (widthPadding/2)), 0, (width + widthPadding), (height + heightPadding)), tex);
                //Debug.LogError("ScreenWidth : " + Screen.width+
                //    " BannerWidth : " + width + 
                //    "ImageWidth : " + tex.width);

            }
        }
        else
        {
            return;
           // GUI.DrawTexture(new Rect(0, Screen.height - (Screen.height * 0.4f), Screen.width * 0.35f, Screen.height * 0.4f), tex);
        }


    }

    //public bool IsInternetConnection()
    //{
    //    if (Application.internetReachability != NetworkReachability.NotReachable || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
