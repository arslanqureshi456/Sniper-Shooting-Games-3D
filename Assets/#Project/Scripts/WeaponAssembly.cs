using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssembly : MonoBehaviour
{
    public RotateSwipe rotateSwipe;
    public bool isMain, allowLerp = true;
    bool hasStarted = false;
    public Directions _direction = Directions.Y_Positive;
    Vector3 origninalPosition;
    Vector3 targetPosition;
    private float speed = 15f;
    private float lerpFactor = 0.005f;
    private float lerpAmount = 0f;
    float multipliyer = 4f;
    public enum Directions
    {
        X_Positive,
        X_Negative,
        Y_Positive,
        Y_Negative,
        Z_Positive,
        Z_Negative
    }
    private void Start()
    {
        targetPosition = transform.position;
        hasStarted = true;
        OnEnable();
    }
    private void OnEnable()
    {
        allowLerp = true;
        if(rotateSwipe != null)
            if (rotateSwipe.enabled)
                rotateSwipe.enabled = false;
        if (hasStarted)
        {
            if (isMain)
                multipliyer = 1f;
            switch (_direction)
            {
                case Directions.X_Positive:
                    origninalPosition = targetPosition + -transform.right * multipliyer;
                    break;
                case Directions.X_Negative:
                    origninalPosition = targetPosition + transform.right * multipliyer;
                    break;
                case Directions.Y_Positive:
                    origninalPosition = targetPosition + -transform.up * multipliyer;
                    break;
                case Directions.Y_Negative:
                    origninalPosition = targetPosition + transform.up * multipliyer;
                    break;
                case Directions.Z_Positive:
                    origninalPosition = targetPosition + -transform.forward * multipliyer;
                    break;
                case Directions.Z_Negative:
                    origninalPosition = targetPosition + transform.forward * multipliyer;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if(allowLerp)
        {
            lerpAmount += speed * lerpFactor;
            this.transform.position = Vector3.Lerp(origninalPosition, targetPosition, lerpAmount);
            if (lerpAmount >= 1)
            {
                allowLerp = false;
                if (rotateSwipe != null)
                    if (!rotateSwipe.enabled)
                        rotateSwipe.enabled = true;
            }
        }
        
    }

    private void OnDisable()
    {
        lerpAmount = 0f;
    }
}
