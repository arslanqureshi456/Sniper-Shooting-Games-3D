using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

public class SocialNetworks : MonoBehaviour {

	private string gameName;
	private string gameURL;

	private void Start()
	{
		gameName = Application.productName;
		gameURL = "https://play.google.com/store/apps/details?id=" + Application.identifier;
	}

	public void _ShareURLButton()
	{
#if UNITY_ANDROID
        Debug.Log("Debug : Go To ShareGameButton");
#endif
        if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			StartCoroutine(ShareAndroidText());
		}
	}
	IEnumerator ShareAndroidText()
	{
		yield return new WaitForEndOfFrame();
		//execute the below lines if being run on a Android device
		#if UNITY_ANDROID
		//Reference of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");

		//Reference of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");

		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");

		//add data to be passed to the other activity i.e., the data to be sent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), gameName);

		//intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Text Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), gameURL);

		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

		//start the activity by sending the intent data
		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
		currentActivity.Call("startActivity", jChooser);
		#endif
	}

	void OnDisable()
	{
		StopCoroutine("ShareAndroidText");
	}

}