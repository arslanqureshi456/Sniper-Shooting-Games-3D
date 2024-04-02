using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountScript : MonoBehaviour
{
    public Text countTex;
    public AudioClip CounterBeep;
    public AudioSource GlobalSource;
    
    private int count = 0;
    public static bool allowWarning = true;

    public GameObject playerCards;

    

    int randomCount = 7;
    void Start()
    {
        int randomCount = Random.Range(7, 12);
    }

    public void UpdateCount()
    {
        GlobalSource.PlayOneShot(CounterBeep);
        count++;
        countTex.text = "" + count;

        
        if(count == randomCount)
        {
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.MULTIPLAYER)
            {
                playerCards.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MultiPlayer");
            }
        }
    }

}
