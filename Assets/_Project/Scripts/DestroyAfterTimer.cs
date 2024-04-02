using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimer : MonoBehaviour
{
	public float time;

	private void OnEnable()
	{
		Invoke ("Hide", time);
	}

	private void Hide()
	{
		this.gameObject.SetActive (false);
	}

	private void OnDisable()
	{
		CancelInvoke("Hide");
	}
}