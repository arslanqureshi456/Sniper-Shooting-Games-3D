using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_HitBox : MonoBehaviour
{
    public CS_Enemy _enemy;
    public int damageMultiplyer = 1;
    public GameObject damageTextPrefab;
    public AudioClip headShot;

    private bool once = false;

    public void Damage(float damage)
    {
        damage *= damageMultiplyer;
        //GameObject damageTextInstance = Instantiate(damageTextPrefab, this.transform.position, this.transform.rotation);
        //damageTextInstance.transform.GetChild(0).GetComponent<TextMesh>().text = ((int)damage).ToString();
        if (_enemy.HP <= 0)
            return;

        if (GameManager.Instance.totalEnemies - GameManager.Instance.enemyKilled == 1)// Last enemy
        {
            if (_enemy.HP < damage)
            {
                // Activate bullet time
               // GameManager.Instance.fpsPlayer.StartCoroutine(GameManager.Instance.fpsPlayer.ActivateBulletTime(1f));
                StartCoroutine(GameManager.Instance.fpsPlayer.ActivateBulletTime(1));
            }
        }

        if (damageMultiplyer > 1 && !once)
        {
            once = true;
            PlayAudioAtPos.PlayClipAt(headShot, this.transform.position, 0.6f, 0.0f);
           // GameManager.Instance.ShowHeadShotAnim();
            //GameManager.Instance.headShot++;
            //GameManager.Instance.ShowHeadShotText();
            //GameManager.Instance.ShowHeadShotScoreText();
        }
        //else if(damage > _enemy.HP && !once)
        //{
        //    once = true;
        //    GameManager.Instance.ShowHeadShotScoreText();
        //}

        if (_enemy)
        {
            _enemy.ApplyDamage(damage);
        }
    }
}
