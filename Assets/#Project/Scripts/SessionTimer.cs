using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SessionTimer : MonoBehaviour
{
    private int time = 0;
    private int carrierDataNetwork, localAreaNetwork, reachable = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            time++;
            if (time >= 20)
            {
                time = 0;
                if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
                {
                    carrierDataNetwork = reachable = 1;
                    localAreaNetwork = 0;
#if UNITY_EDITOR
                    print("DATA: True");
#endif
                }
                else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    localAreaNetwork = reachable = 1;
                    carrierDataNetwork = 0;
#if UNITY_EDITOR
                    print("WIFI: True");
#endif
                }
                else if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    carrierDataNetwork = localAreaNetwork = reachable = 0;
#if UNITY_EDITOR
                    print("DATA/WIFI: False");
#endif
                }

                PlayerPrefs.SetInt("SessionTime", PlayerPrefs.GetInt("SessionTime") + 1);
                AnalyticsResult result = Analytics.CustomEvent("SessionTime", new Dictionary<string, object>
            {
            { "level_index", PlayerPrefs.GetInt("SessionTime") },
            { "MissionComplete", PlayerPrefs.GetInt("MissionCompleteEvent")},
            { "WIFI", localAreaNetwork},
            { "DATA", carrierDataNetwork},
            { "InternetConnection", reachable}
            });
#if UNITY_EDITOR
                Debug.Log("CustomEvent: " + "SessionTime");
#endif
                PlayerPrefs.SetInt("MissionCompleteEvent", 0);
                HandleAnalyticsResult(result);
            }
            yield return new WaitForSecondsRealtime(1);
        }

    }

    private void HandleAnalyticsResult(AnalyticsResult result)
    {
        switch (result)
        {
            case AnalyticsResult.Ok:
#if UNITY_EDITOR
                print("Events Sent Successfully");
#endif
                break;
            case AnalyticsResult.NotInitialized:
#if UNITY_EDITOR
                print("Events Not Initialized");
#endif
                break;
            case AnalyticsResult.AnalyticsDisabled:
#if UNITY_EDITOR
                print("Events Disabled");
#endif
                break;
            case AnalyticsResult.TooManyItems:
#if UNITY_EDITOR
                print("Events have too many items");
#endif
                break;
            case AnalyticsResult.SizeLimitReached:
#if UNITY_EDITOR
                print("Events size limit reached");
#endif
                break;
            case AnalyticsResult.TooManyRequests:
#if UNITY_EDITOR
                print("Events too many requests");
#endif
                break;
            case AnalyticsResult.InvalidData:
#if UNITY_EDITOR
                print("Events Invalid data");
#endif
                break;
            case AnalyticsResult.UnsupportedPlatform:
#if UNITY_EDITOR
                print("Events UnSupported Platform");
#endif
                break;
        }
    }

    void OnDisable()
    {
        StopCoroutine("Timer");
    }
}
