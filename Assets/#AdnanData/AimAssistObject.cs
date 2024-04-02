using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistObject : MonoBehaviour
{
    public static AimAssistObject instance;
    private void Start()
    {
        instance = this;
        if ((LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT) && (WeaponBehavior.instance != null && WeaponBehavior.isAutoFire))
        {
            Invoke("EnableCollider", 0.5f);
        }
        else if(LevelSelectionNew.modeSelection != LevelSelectionNew.modeType.ASSAULT)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
        {
            if (WeaponBehavior.instance != null && !WeaponBehavior.isAutoFire)
            {
                DisableCollider();
            }
            else
            {
                if (WeaponBehavior.instance != null && WeaponBehavior.instance.raycastedObject != null)
                {
                    DisableCollider();
                }
                else
                {
                    EnableCollider();
                }
            }
        }
        else
        {
            DisableCollider();
        }

    }
    public void EnableCollider()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }
    public void DisableCollider()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }

    private void OnDisable()
    {
        CancelInvoke("EnableCollider");
    }

    private void OnDestroy()
    {
        CancelInvoke("EnableCollider");
    }
}
