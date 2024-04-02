using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class WelcomeRewardManager : MonoBehaviour
{
    public AudioClip goldSound, gunSound;
    public AudioSource global;
    public GameObject RewardPanel;
    public void PlayGoldSound()
    {
        global.PlayOneShot(goldSound);
    }
    public void PlaySniperSound()
    {
        global.PlayOneShot(gunSound);
    }
    public void ClaimReward()
    {
        AudioManager.instance.BackClickNew();

        Analytics.CustomEvent("Claim_Tut", new Dictionary<string, object>
        {
            { "level_index", 1 }
        });
#if UNITY_EDITOR
        Debug.Log("CustomEvent: " + "Claim_Tut");
#endif
        PlayerPrefs.SetInt("WelcomeReward", 1);
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 50);
        ConstantUpdate.Instance.UpdateCurrency();
        RewardPanel.SetActive(false);
    }
}
