using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage
{
    public bool isActive, isFailed;
    public int count;
    public float[] hps = new float[3];

    public void AddHealth(float val)
    {
        hps[count] =  Mathf.Clamp(val,0,100);
        count++;
        if (count >= 3)
        {
            isActive = true;
            count = 0;
        }
    }

    public float GetAvg()
    {
        if (isActive)
        {
            return Mathf.Clamp((hps[0] + hps[1] + hps[2]) / 3, 0, 100);
        }
        else
            return 1;
    }
}
