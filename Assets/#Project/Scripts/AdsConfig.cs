using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsConfig : MonoBehaviour
{
    public static AdsConfig Instance;
    public enum Status
    {
        Show,
        Hide
    }

    public enum Priority
    {
        Admob,
        Unity,
        Admob_Unity,
        Unity_Admob
    }

    [Space(5)]
    public Status loadingADStatus = Status.Show;
    [Space(5)]
    public Priority loadingADPhase_1Priority = Priority.Admob;
    [Range(1, 10)]
    public int loadingADFrequency_1 = 1;
    public Priority loadingADPhase_2Priority = Priority.Admob_Unity;
    [Range(1, 10)]
    public int loadingADFrequency_2 = 1;
    public Priority loadingADPhase_3Priority = Priority.Admob_Unity;
    [Range(1, 10)]
    public int loadingADFrequency_3 = 1;

    [Space(30)]
    public Status completeADStatus = Status.Show;
    [Space(5)]
    public Priority completeADPhase_1Priority = Priority.Admob;
    [Range(1, 10)]
    public int completeADFrequency_1 = 1;
    public Priority completeADPhase_2Priority = Priority.Admob_Unity;
    [Range(1, 10)]
    public int completeADFrequency_2 = 1;
    public Priority completeADPhase_3Priority = Priority.Admob_Unity;
    [Range(1, 10)]
    public int completeADFrequency_3 = 1;

    [Space(30)]
    public Status pauseADStatus = Status.Show;
    [Space(5)]
    public Priority pauseADPhase_1Priority = Priority.Admob;
    [Range(1, 10)]
    public int pauseADFrequency_1 = 1;
    public Priority pauseADPhase_2Priority = Priority.Admob_Unity;
    [Range(1, 10)]
    public int pauseADFrequency_2 = 1;
    public Priority pauseADPhase_3Priority = Priority.Admob_Unity;
    [Range(1, 10)]
    public int pauseADFrequency_3 = 1;

    [Space(30)]
    public Status sessionStartADStatus = Status.Show;

    [Space(10)]
    public Status gameplayToMainMenuADStatus = Status.Show;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
