using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCamera : MonoBehaviour
{
    Transform Player;

    // Update is called once per frame
    void LateUpdate()
    {
        if(Player != null)
        {
            transform.position = new Vector3(Player.position.x,480, Player.position.z);
        }
        else
        {
            if(GameObject.FindGameObjectWithTag("Player") != null)
                Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
