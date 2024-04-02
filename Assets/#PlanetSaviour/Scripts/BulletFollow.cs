using System.Collections;
using UnityEngine;

public class BulletFollow : MonoBehaviour
{
    public static Transform enemyObject;
    public static RaycastHit hit;

    public Transform startMarker;
    public Vector3 endMarker;
    public Transform cameraWrapper, mainCamera;
    public GameObject windEffect, explosion, muzzleFlash;
   // private Vector3[] startRotations = new[] { new Vector3(0f, 500f, 0f), new Vector3(0f, 230f, 0f), new Vector3(45f, 500f, 0f) };
    private Vector3 startRotations = new Vector3(0f, 380, 0f);
    private Vector3 startRotation, targetRotation;

    private float currentXRotation, currentYRotation = 0;
    private bool began = true, isUserControlled = false, ALlowTouch = false;
    private Vector2 startTouchPos;
    private float MINMOVEDISTANCE = 0.0075f, TOUCHSENSITIVITY = 0.34f;

    private float speed = 0.5f;
    private float lerpFactor = 0.005f;
    private float lerpAmount = 0f;

    //private bool isSpeedUp = false;

    private int gotPos = 0;

    // Fire Explosion For Firing
    public GameObject FireEffect;

    private void Awake()
    {
        //startRotation = startRotations[Random.Range(0, startRotations.Length)];
        startRotation = startRotations;
        targetRotation = new Vector3(18, 360, 0);
    }

    private void Start()
    {
        transform.position = startMarker.position + transform.forward * 0.55f;
        //Invoke("DelayedTrail", 2f);
        transform.LookAt(endMarker);
        // StartCoroutine(DelayedParticles());
        Instantiate(FireEffect, new Vector3 (startMarker.position.x + 0.1f, startMarker.position.y , startMarker.position.z + 0.05f), startMarker.rotation);
       // Invoke("FireEffectAtKillCam", 1);
    }

    void FireEffectAtKillCam()
    {
        Instantiate(FireEffect, startMarker.position, startMarker.rotation);
    }

    private void FixedUpdate()
    {
        try
        {
            if (gotPos.Equals(3))
                return;
        }
        catch { }
        //if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT ||
        //    LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER && lerpAmount < 0.85f)
        //{
        //    particlesCover.transform.localEulerAngles = bullet.transform.localEulerAngles = new Vector3(Mathf.Lerp(0, 4, lerpAmount), Mathf.Lerp(0, 11.5f, lerpAmount), 0);
        //    transform.LookAt(endMarker);
        //    bullet.transform.LookAt(endMarker);
        //    particlesCover.transform.LookAt(endMarker);
        //    muzzle_part.transform.LookAt(endMarker);
        //}
        lerpAmount += speed * lerpFactor;
        try
        {
            if (enemyObject)
                transform.position = Vector3.Lerp(startMarker.position, enemyObject.position, lerpAmount);
            else
                transform.position = Vector3.Lerp(startMarker.position, endMarker, lerpAmount);
            if (!isUserControlled)
                cameraWrapper.localEulerAngles = Vector3.Lerp(startRotation, targetRotation, lerpAmount);

        }
        catch { }

        try
        {
            if (lerpAmount < 0.01f)//0.015f
            {
                speed = 0.025f;//0.05f
            }
            else if (lerpAmount < 0.975f)
            {
                if (!ALlowTouch)
                {
                    //if (PlayerPrefs.GetInt("OpenBulletTutorial") != 1) //if (PlayerPrefs.GetInt("sniperCurrentLevel") == 1)
                    //{
                    //    speed = 0.1f;
                    //    GameManager.Instance.OpenBulletTutorial();
                    //}
                    //else
                        speed = 5f;

                    ALlowTouch = true;
                    //5
                }
                mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, new Vector3(0, -10, -500), lerpAmount);//2050
                //mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, new Vector3(0, -10, -500), lerpAmount);//2050
            }
            else
            {
                speed = 0.5f;//0.05
                if (gotPos.Equals(2))
                    MoveCameraBack();
            }
        }
        catch { }
    }
    void Update()
    {
        if (ALlowTouch)
        {
            //if (Application.platform == RuntimePlatform.Android)
            //    PhoneInput();
            //else
            //    PCClicks();
        }
    }
    private IEnumerator DelayedParticles()
    {
        yield return new WaitForSeconds(0.15f);
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        explosion.SetActive(true);
    }

    private void DelayedTrail()
    {
        windEffect.SetActive(true);
    }

    private void MoveCameraBack()
    {
        if(enemyObject)
        {
            Vector3 relativePos = enemyObject.position - mainCamera.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            if (!isUserControlled)
                cameraWrapper.rotation = Quaternion.Lerp(cameraWrapper.rotation, rotation, lerpAmount);
            Invoke("StopCamera", 8f);
            
        }
    }

    private void StopCamera()
    {
        gotPos = 3;
    }

    private bool isHit = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HitBox"))
        {
            if (!isHit)
            {
                try
                {
                    isHit = true;
                    gotPos = 2;
                    transform.GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(2).gameObject.SetActive(false);
                    transform.GetChild(3).gameObject.SetActive(false);
                    transform.GetChild(4).gameObject.SetActive(false);

                    GetComponent<AudioSource>().enabled = false;
                    other.gameObject.GetComponent<TacticalAI.HitBox>().Damage(100f, hit);
#if UNITY_EDITOR
                 //   Debug.LogError("New Bullet Sound Here");
#endif
                AudioManager.instance.LastKillBulletSound();
                    // Change Camera Position Of Camera At Trigger Position
                    mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, new Vector3(0, -10, -2500), lerpAmount);
                    Invoke("LastEnemyDeadSound", 0.4f);
                    Destroy(gameObject, 5);
                }
                catch { }
            }
        }
    }

    void LastEnemyDeadSound()
    {
#if UNITY_EDITOR
      //  Debug.LogError("Last Enemy Death Sound Here");
#endif
        AudioManager.instance.PlayDeathSoundSniper();
    }

    void PhoneInput()
    {
        if (Input.touchCount > 0)
        {
            if (began)
            {
                began = false;
                if (!isUserControlled)
                {
                    GameManager.Instance.CloseBulletTutorial();
                    isUserControlled = true;
                    currentXRotation = cameraWrapper.localEulerAngles.y;
                    currentYRotation = cameraWrapper.localEulerAngles.x;
                }
                startTouchPos = Input.GetTouch(0).position;
            }
            else if ((Input.GetTouch(0).position - startTouchPos).sqrMagnitude > MINMOVEDISTANCE)
            {
                currentXRotation += (Input.GetTouch(0).position.x - startTouchPos.x) * TOUCHSENSITIVITY;
                currentYRotation -= (Input.GetTouch(0).position.y - startTouchPos.y) * TOUCHSENSITIVITY;
                startTouchPos = Input.GetTouch(0).position;
                speed = 0.1f;
                RotateCamera();
            }
        }
        else
        {
            if (!began)
            {
                began = true;
                speed = 3f;
            }
        }
    }
    void PCClicks()
    {
        if (Input.GetMouseButton(0))
        {
            if (began)
            {
                began = false;
                if (!isUserControlled)
                {
                    GameManager.Instance.CloseBulletTutorial();
                    isUserControlled = true;
                    currentXRotation = cameraWrapper.localEulerAngles.y;
                    currentYRotation = cameraWrapper.localEulerAngles.x;
                }

                startTouchPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            }
            else if ((new Vector2(Input.mousePosition.x, Input.mousePosition.y) - startTouchPos).sqrMagnitude > MINMOVEDISTANCE)
            {
                currentXRotation += (Input.mousePosition.x - startTouchPos.x) * TOUCHSENSITIVITY;
                currentYRotation -= (Input.mousePosition.y - startTouchPos.y) * TOUCHSENSITIVITY;
                startTouchPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                speed = 0.1f;
                RotateCamera();
            }
        }
        else
        {
            if (!began)
            {
                began = true;
                speed = 3f;
            }
        }
    }
    void RotateCamera()
    {
        cameraWrapper.localEulerAngles = new Vector3(currentYRotation, currentXRotation, 0);
    }

    private void OnDisable()
    {
        CancelInvoke("StopCamera");
        CancelInvoke("LastEnemyDeadSound");
    }
}
