using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 50f;

    public bool x_Axis = false;
    public bool y_Axis = true;
    public bool z_Axis = false;

    private void Update()
    {
        if(x_Axis)
        {
            this.transform.Rotate(Time.deltaTime * speed, 0f, 0f);
        }
        else if(y_Axis)
        {
            this.transform.Rotate(0f, Time.deltaTime * speed, 0f);
        }
        else if(z_Axis)
        {
            this.transform.Rotate(0f, 0f, Time.deltaTime * speed);
        }
        
    }
}