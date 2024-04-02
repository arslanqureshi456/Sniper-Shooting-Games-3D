using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfterTime : MonoBehaviour
{
	public float time;

	private void OnEnable()
	{
        Destroy(this.gameObject , time);
    }
}
