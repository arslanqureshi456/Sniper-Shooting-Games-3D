using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    public static AimAssist Instance;
    public float inputX = 0, inputY, captureSpeed;
    public float Limit = 0, Speed = 0, multipliyer = 0.1f;
    public Transform currentEnemyHitboxTransform = null;
    public bool isAimingActive = false, isReset = false, isAllowed = true, isAimAssistedGun = false;
    public InputControl inputs = null;
    public Quaternion AssistRot;
    Transform camTrans = null;
    public TacticalAI.HealthScript healthSc = null;
    private void OnEnable()
    {
        StartCoroutine(FindMouseLook());
        Instance = this;
        inputX = 0;
    }
    IEnumerator FindMouseLook()
    {
        while(true)
        {
            if(FindObjectOfType<SmoothMouseLook>() && FindObjectOfType<InputControl>())
            {
                camTrans = FindObjectOfType<SmoothMouseLook>().transform;
                inputs = FindObjectOfType<InputControl>();
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }
    void LateUpdate()
    {
        //inputX = Limit;
        if (camTrans != null)
        {
            // (Mathf.Abs(inputs.lookX * inputs.lookY) > captureSpeed) ||
            if (isAimingActive && ((currentEnemyHitboxTransform == null || healthSc.health <= 0) || (inputs != null && isReset && (Mathf.Abs(inputs.lookX) > 0.35f) || Mathf.Abs(inputs.lookY) > 0.35f)))
            {
                ResetAssist();
                return;
            }
            if (isAimingActive)
            {
                if (Mathf.Abs(inputs.lookX * inputs.lookY) < 0.05f) // 0.025
                    isReset = true;
                AssistRot = Quaternion.LookRotation(currentEnemyHitboxTransform.position - camTrans.position, Vector3.up);
                AssistRot = Quaternion.Euler(AssistRot.eulerAngles.x, AssistRot.eulerAngles.y, 0);
                //inputX = temp.y  -multipliyer; //Mathf.Lerp(inputX, temp.y, 1)  -multipliyer;
                //inputY = temp.x  -multipliyer;//Mathf.Lerp(inputY, temp.x, 1)  -multipliyer;
            }
        }
        else if(camTrans != null)
        {
            camTrans = FindObjectOfType<SmoothMouseLook>().transform;
            inputs = FindObjectOfType<InputControl>();
        }
    }
    public void SetAssist(Transform trans)
    {
        if (!camTrans ||!inputs || !trans.GetComponent<TacticalAI.HitBox>())
            return;
        if (isAimingActive || !isAllowed)
            return;
        //if(inputs != null && Mathf.Abs(inputs.lookX) < 0.9f && Mathf.Abs(inputs.lookY) < 0.9f)
        //{
        healthSc = trans.GetComponent<TacticalAI.HitBox>().myScript;
        isReset = false;
        currentEnemyHitboxTransform = trans;
        isAimingActive = true;
        isAllowed = false;
        //}
    }
    public void ResetAssist()
    {
        if (!camTrans || !inputs)
            return;
        if (isAimingActive)
        {
            //Debug.Log("ressting aim!");
            Invoke("DelayedAllow" , 0.3f);
            isAimingActive = false;
            currentEnemyHitboxTransform = null;
            inputX = 0;
            inputY = 0;
        }
    }
    void DelayedAllow()
    {
        isAllowed = true;
    }
    public void SetAimAssitedGun()
    {
        if (!camTrans || !inputs)
            return;
        isAimAssistedGun = true;
    }
    public void ResetAimAssistedGun()
    {
        if (!camTrans || !inputs)
            return;
        isAimAssistedGun = false;
        ResetAssist();
    }

    private void OnDisable()
    {
        CancelInvoke("DelayedAllow");
        StopCoroutine("FindMouseLook");
    }
}