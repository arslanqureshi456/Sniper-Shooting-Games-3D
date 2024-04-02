using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownPanel : MonoBehaviour
{
    public Text _text;

    private int time = 3;

    private void OnEnable()
    {
        time = 3;
        Time.timeScale = 0;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            _text.text = System.String.Empty + time;
            time--;
            if (time < 0)
            {
                _text.text = System.String.Empty + "Ready!";
                Time.timeScale = 1;
                this.gameObject.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(1);
        }
    }

    private void OnDisable()
    {
        StopCoroutine("Timer");
    }
}
