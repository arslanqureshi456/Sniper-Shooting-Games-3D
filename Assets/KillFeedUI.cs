using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KillFeedUI : MonoBehaviour
{
    [SerializeField] private Text KillerText;
    [SerializeField] private Text KilledText;
    [SerializeField] private GameObject rifleImage;
    [SerializeField] private GameObject sniperImage;
    //[SerializeField] private Text WeaponText;
    //[SerializeField] private Image KillTypeImage;
    private CanvasGroup Alpha;

    public void Init(KillFeed feed)
    {
        KillerText.text = feed.Killer;
        KilledText.text = feed.Killed;

        if(feed.GunID == 1)
        {
            rifleImage.SetActive(true);
        }
        else
        {
            sniperImage.SetActive(true);
        }
        //WeaponText.text = string.Format("{0}", feed.HowKill);
        //KillTypeImage.gameObject.SetActive(feed.HeatShot);
        Alpha = GetComponent<CanvasGroup>();
        StartCoroutine(Hide(7));
    }

    IEnumerator Hide(float time)
    {
        yield return new WaitForSeconds(time);
        while (Alpha.alpha > 0)
        {
            Alpha.alpha -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        StopCoroutine("Hide");
    }
}