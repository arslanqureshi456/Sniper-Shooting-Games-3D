using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heli : MonoBehaviour
{
    public enum FireOption
    {
        bullet,
        missile,
        both
    };
    public FireOption fireOptions = FireOption.both;
    public GameObject bulletObject, missileObject;
    public Transform bulletSpawn;
    public Transform[] missilesSpawn;
    public LayerMask layerMask;

    private CameraControl CameraControlComponent;
    private Transform mainCamTransform;
    private GameObject playerObj;

    private Transform missileSpawn;
    private RaycastHit hit;
    private Quaternion fireRotation;
    private float fireRate = 15f;
    private float inaccuracy = 1;
    private float nextTimeToFire = 0f;
    private bool canFireMissile = false;
    
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float delay = 5f;
    [SerializeField]
    private float speed = 5;

    [Header ("Positions")]
    public Vector3 Level10Pos;
    public Vector3 Level16Pos;
    public Vector3 Level19Pos;
    public Vector3 Level21Pos;
    public Vector3 Level25Pos;
    public Vector3 Level30Pos;

    [Header("Offsets")]
    public Vector3 Level10Offset;
    public Vector3 Level16Offset;
    public Vector3 Level19Offset;
    public Vector3 Level21Offset;
    public Vector3 Level25Offset;
    public Vector3 Level30Offset;

    private struct PointInSpace
    {
        public Vector3 Position;
        public float Time;
    }

    

    private Queue<PointInSpace> pointsInSpace = new Queue<PointInSpace>();

    private void Awake()
    {
        //if (GameManager.Instance.assaultLevels[9].activeInHierarchy)
        //{
        //    transform.position = Level10Pos;
        //    offset = Level10Offset;
        //}
        //else if (GameManager.Instance.assaultLevels[15].activeInHierarchy)
        //{
        //    transform.position = Level16Pos;
        //    offset = Level16Offset;
        //}
        //else if (GameManager.Instance.assaultLevels[18].activeInHierarchy)
        //{
        //    transform.position = Level19Pos;
        //    offset = Level19Offset;
        //}
        //else if (GameManager.Instance.assaultLevels[20].activeInHierarchy)
        //{
        //    transform.position = Level21Pos;
        //    offset = Level21Offset;
        //}
        //else if (GameManager.Instance.assaultLevels[24].activeInHierarchy)
        //{
        //    transform.position = Level25Pos;
        //    offset = Level25Offset;
        //}
        //else if (GameManager.Instance.assaultLevels[29].activeInHierarchy)
        //{
        //    transform.position = Level30Pos;
        //    offset = Level30Offset;
        //}
    }

    private void Start()
    {
        switch (fireOptions)
        {
            case FireOption.both:
                StartCoroutine(SwitchFire());
                break;
            case FireOption.bullet:
                canFireMissile = false;
                ChangeInAccuracy();
                ChangeRateOfFire();
                break;
            case FireOption.missile:
                canFireMissile = true;
                ChangeInAccuracy();
                ChangeRateOfFire();
                break;
        }

        mainCamTransform = Camera.main.transform;
        CameraControlComponent = mainCamTransform.GetComponent<CameraControl>();
        playerObj = CameraControlComponent.playerObj;
        target = playerObj.transform;
        //StartCoroutine(RandomOffset());
    }

    private void FixedUpdate()
    {
        if(transform.position.y > 25)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
            transform.position = smoothedPosition;

            transform.LookAt(target);

            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Fire();
            }
        }
        else
        {
            transform.position += Vector3.up * 9f * Time.deltaTime;
        }
    }

   
    private void Fire()
    {
        if (canFireMissile)
        {
            missileSpawn = missilesSpawn[Random.Range(0, missilesSpawn.Length)];
            bool amAtTarget = Vector3.Angle(missileSpawn.forward, target.position - missileSpawn.position) < 10;
            if (amAtTarget)
            {
                fireRotation.SetLookRotation(target.position - missileSpawn.position);
            }
            else
            {
                fireRotation = Quaternion.LookRotation(missileSpawn.forward);
            }

            fireRotation *= Quaternion.Euler(Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), 0);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.gameObject.layer.Equals(11))
                {
                    Instantiate(missileObject, missileSpawn.position, fireRotation);
                }
            }
        }
        else
        {
            bool amAtTarget = Vector3.Angle(bulletSpawn.forward, target.position - bulletSpawn.position) < 10;
            if (amAtTarget)
            {
                fireRotation.SetLookRotation(target.position - bulletSpawn.position);
            }
            else
            {
                fireRotation = Quaternion.LookRotation(bulletSpawn.forward);
            }

            fireRotation *= Quaternion.Euler(Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), 0);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.gameObject.layer.Equals(11))
                {
                    Instantiate(bulletObject, bulletSpawn.position, fireRotation);
                }
            }
        }
    }

    private void ChangeRateOfFire()
    {
        if (canFireMissile)
        {
            fireRate = 0.1f;
        }
        else
        {
            fireRate = 15f;
        }
    }

    private void ChangeInAccuracy()
    {
        if (canFireMissile)
        {
            inaccuracy = 5f;
        }
        else
        {
            inaccuracy = 3f;
        }
    }

    private IEnumerator SwitchFire()
    {
        yield return new WaitForSeconds(20.0f);
        canFireMissile = !canFireMissile;
        ChangeInAccuracy();
        ChangeRateOfFire();
        StartCoroutine(SwitchFire());
    }

    private IEnumerator RandomOffset()
    {
        yield return new WaitForSeconds(10.0f);
        offset = new Vector3(Random.Range(-35f, -20f), 30f, Random.Range(-30f, 30f));
        StartCoroutine(RandomOffset());
    }

    private void OnDisable()
    {
        StopCoroutine("SwitchFire");
        StopCoroutine("RandomOffset");
    }
}