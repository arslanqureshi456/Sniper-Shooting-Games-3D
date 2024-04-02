using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Cover : MonoBehaviour
{
    private bool isHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isHit && other.CompareTag("HitBox"))
        {
            isHit = true;
            other.GetComponent<CS_HitBox>()._enemy.isCoverAvailable = true;
            this.gameObject.SetActive(false);
        }
    }
}
