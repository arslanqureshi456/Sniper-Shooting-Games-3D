using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectWrapper : MonoBehaviour
{
    public Transform startMarker;
    public Vector3 endMarker;
    public GameObject bullet,bulletMulti;
    public GameObject[] Guns;
    public Transform[] startPoint;
    public Texture[] BulletTextures;

    public void Start()
    {
        transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        Guns[GameManager.Instance.fpsPlayer.PlayerWeaponsComponent.currentWeapon - 1].SetActive(true);
        startMarker = startPoint[GameManager.Instance.fpsPlayer.PlayerWeaponsComponent.currentWeapon - 1];
        switch(LevelSelectionNew.modeSelection)
        {
            case LevelSelectionNew.modeType.SNIPER:
                Camera.main.GetComponent<Camera>().enabled = false;
                GameManager.Instance.mainCamera.transform.GetChild(5).gameObject.SetActive(false);
                bullet.GetComponent<BulletFollow>().startMarker = startMarker;
                bullet.GetComponent<BulletFollow>().endMarker = endMarker;
                GameObject temp = Instantiate(bullet, startMarker.position, Quaternion.identity);
               // temp.transform.GetChild(2).GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", BulletTextures[PlayerPrefs.GetInt("equippedBullet")]);
                break;
            case LevelSelectionNew.modeType.ASSAULT:
#if UNITY_EDITOR
                print("Gun Number For Bullet Follow Effect : " + GameManager.Instance.fpsPlayer.PlayerWeaponsComponent.currentWeapon);
#endif
               
                Camera.main.GetComponent<Camera>().enabled = false;
                GameManager.Instance.mainCamera.transform.GetChild(5).gameObject.SetActive(false);
                bullet.GetComponent<BulletFollow>().startMarker = startMarker;
                bullet.GetComponent<BulletFollow>().endMarker = endMarker;
                GameObject temp2 = Instantiate(bullet, startMarker.position, Quaternion.identity);
               // temp2.transform.GetChild(2).GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", BulletTextures[PlayerPrefs.GetInt("equippedBullet")]);
                break;
        }
       // FindObjectOfType<bl_HudManager>().gameObject.SetActive(false);
        GameManager.Instance.gamePlayPanel.SetActive(false);
    }
}
