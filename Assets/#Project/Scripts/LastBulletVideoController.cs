using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBulletVideoController : MonoBehaviour
{
    UnityEngine.Video.VideoPlayer player;
    int count = 0;
    void Start()
    {
        player = GetComponent<UnityEngine.Video.VideoPlayer>();
        player.loopPointReached += LoopStarted;
    }
    void LoopStarted(UnityEngine.Video.VideoPlayer vp)
    {
        count++;
        if (count >= 3)
        {
            gameObject.SetActive(false);
            GameManager.Instance.AfterBulletsVideo();
        }
        //vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }
}
