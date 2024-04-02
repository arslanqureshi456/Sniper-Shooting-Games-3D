using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevivePlayerLoading : MonoBehaviour
{
    public float delay = 10f;
    public Image fillImage;
    public Text fillCount;

    private float count = 0;

    private void OnEnable()
    {
        StartCoroutine(StartLoading(delay));
    }

    private IEnumerator StartLoading(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.unscaledDeltaTime;
            float lerpValue = animationTime / seconds;
            count = Mathf.Lerp(1f, 0f, lerpValue);
            fillImage.fillAmount = count;
            fillCount.text = System.String.Empty + (int)(count * delay);
            yield return null;
        }

        CloseButton();
    }

    public void CloseButton()
    {
        Time.timeScale = 1;
        GameManager.Instance.isLevelFailed = false;
        GameManager.Instance.LevelFailed();
        if (this.gameObject.activeSelf)
            this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine("StartLoading");
    }
}
