using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour {

	public static float timeLeft = 300.0f;
	public static bool stop = true;

	private float minutes;
	private float seconds;

	public Text text;

	public static bool finalState = false;

    public GameObject finalWinScreen;
	public void Start()
	{
	
		startTimer (5f);
			
	}
	public void startTimer(float from){
		stop = false;
		timeLeft = from;
		Update();
	}

	void Update() {
		if (stop) {
			finalState = true;
            gameObject.SetActive(false);
            finalWinScreen.SetActive(true);
			return;
		}
		timeLeft -= Time.deltaTime;
		minutes = Mathf.Floor(timeLeft / 60);
		seconds = timeLeft % 60;
		if(seconds > 59) seconds = 59;
		if(minutes < 0) {
			stop = true;
			minutes = 0;
			seconds = 0;
		}
        text.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        //        fraction = (timeLeft * 100) % 100;
    }
}