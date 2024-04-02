using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiPlayerController : MonoBehaviour
{
    public GameObject[] Cards;
 
    List<int> randomIndexes = new List<int>();
    public AudioSource GlobalSource,MusicSource;
    public AudioClip CardsClip;
    int team1Total = 0, team2Total = 0,teamCount;

    void OnEnable()
    {
        switch(AutoQualityChooser.finalResult)
        {
            case 1:
                teamCount = 3;
                break;
            case 2:
                teamCount = 4;
                break;
            //case 2:
            //    teamCount = 5;
                //break;
        }
        //Debug.Log("max player anim" + teamCount);
        int ran;
        do{
            ran = Random.Range(0,10);
            while(randomIndexes.Exists(x => x == ran))
            {
                ran = Random.Range(0,10);
            }
            randomIndexes.Add(ran);
        }while(randomIndexes.Count < 10);
        Invoke("Delay",1.1f);
        MusicSource.volume = PlayerPrefs.GetFloat("Music") / 8;
    }
    void Delay()
    {
        StartCoroutine("EnableCards");
    }
    IEnumerator EnableCards()
    {
        for(int i = 0; i < randomIndexes.Count ; i++ )
        {
            if(randomIndexes[i] < 5 && team1Total < teamCount)
            {
                team1Total++;
            }else if (randomIndexes[i] >= 5 && team2Total < teamCount)
            {
                team2Total++;
            }else
            {
                continue;
            }
            Cards[randomIndexes[i]].SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.45f,0.5f));
        }
        yield return new WaitForSeconds(3f);
        //_lobby.LoadGameLevel();
       // _networkController.LoadLevel();
        MusicSource.volume = MusicSource.volume * 3;
        GlobalSource.Stop();
        GlobalSource.clip = null;

        SceneManager.LoadScene("MultiPlayer");
        //DestroyImmediate(gameObject,true);
    }

    private void OnDisable()
    {
        StopCoroutine("EnableCards");
        CancelInvoke("Delay");
    }
}
