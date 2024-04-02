using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator _animator;

    private void Start()
    {
        StartCoroutine(FadeToLevel());
    }

    public IEnumerator FadeToLevel()
    {
        yield return new WaitForSecondsRealtime(4.5f);
        _animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene("GameScene");
    }

    void OnDisable()
    {
        StopCoroutine("FadeToLevel");
    }
}
