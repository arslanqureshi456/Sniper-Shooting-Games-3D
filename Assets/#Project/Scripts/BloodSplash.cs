using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplash : MonoBehaviour
{
    //Vector3 SafeTemp;
    void Start()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
            yield return new WaitForSecondsRealtime(1.4f);
        else
            yield return new WaitForSecondsRealtime(0.75f);
        if (BulletFollow.enemyObject)
        {
            transform.position = new Vector3(BulletFollow.enemyObject.transform.position.x, transform.position.y, BulletFollow.enemyObject.transform.position.z);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        yield break;
    }

    private void OnDisable()
    {
        StopCoroutine("Delay");
    }
}
