using UnityEngine;
using System.Collections;

public class EventsDebug : MonoBehaviour {

	public void OnDecrease(GameObject sourceObject)
    {
#if UNITY_EDITOR
        Debug.Log(sourceObject.name + " was damaged");
#endif
    }

    public void OnDeath(GameObject sourceObject)
    {
#if UNITY_EDITOR
        Debug.Log(sourceObject.name + " is dead");
#endif
    }

    public void OnIncrease(GameObject sourceObject)
    {
#if UNITY_EDITOR
#endif
        Debug.Log(sourceObject.name +"'s health was increased");
    }

    public void OnChange(GameObject sourceObject)
    {
#if UNITY_EDITOR
        Debug.Log(sourceObject.name + " was changed");
#endif
    }

    public void OnFull(GameObject sourceObject)
    {
#if UNITY_EDITOR
        Debug.Log(sourceObject.name +"'s  was restored");
#endif
    }
}
