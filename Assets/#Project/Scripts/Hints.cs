using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class Hints : MonoBehaviour
{
    public float delay = 1.0f;
    public Text hintText;
    public GameObject[] HintObjects;
    public string[] hints;
    int temp = 0;

    private void OnEnable()
    {
        StartCoroutine(Hint());
    }

    private IEnumerator Hint()
    {
        while(true)
        {
            HintObjects[temp].SetActive(false);
            temp = Random.Range(0, HintObjects.Length);
            HintObjects[temp].SetActive(true);
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnDisable()
    {
        StopCoroutine("Hint");
    }
}
