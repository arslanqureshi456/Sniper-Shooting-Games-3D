using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_Enemy : MonoBehaviour
{
    public Transform target;
    public Transform destinationPoint;
    public CS_Spawner spawner;
    public Transform eyeTransfrom;

    [Header("Enemy Movement")]
    [Space(5)]
    public float movementSpeed = 5f;

    [Header("Enemy Health")]
    [Space(5)]
    public float HP = 100f;
    public AudioClip[] enemyDownClip;
    public AudioClip fallDownClip;
    public float bodyStayTime = 10f;
    public float weaopnStayTime = 15f;
    public GameObject weaponObject;
    public GameObject weaponObjectToSpawn;
    public Vector3 weaponObjectRotation = new Vector3(0.0f, 180f, 180f);
    public Rigidbody[] rigidbodies;
    public Collider[] collidersToEnable;

    [Header("Enemy Shooting")]
    [Space(5)]
    public float damage = 1f;
    public int bulletsPerMagzineDefault = 30;
    public float reloadTime = 1.5f;
    public float fireRate = 15f;
    public float inaccuracy = 3;
    public AudioClip bulletClip;
    [Range(0.0f, 1.0f)]
    public float bulletSoundVolume = 1;
    public GameObject bulletObject;
    public Transform bulletSpawn;
    public LayerMask layerMask;
    public bool canThrowGrenade = false;
    [Range(0.0f, 1.0f)]
    public float oddsToSecondaryFire = 0.1f;
    public float minTimeBetweenSecondaryFire = 5f;
    public float grenadeThrowTime = 0.5f;
    public GameObject grenadeObject;
    public Transform grenadeSpawn;

    private bool canFire = false;

    public enum AIState
    {
        run,
        cover,
        standingFire,
        crouchReload,
        standingReload,
        grenade,
    }

    [Header("Enemy Animation States")]
    [Space(5)]
    public bool canTakeCover = false;
    [Range(0.0f, 1.0f)]
    public float oddsToCover = 0.1f;
    public float minTimeBetweenCover = 5f;
    public Animator _animator;
    public AIState _AIState;

    private FPSPlayer fPSPlayer;
    private CameraControl CameraControlComponent;
    private Transform mainCamTransform;
    private GameObject playerObj;
    private NavMeshAgent _agent;
    private RaycastHit hit;
    private Quaternion fireRotation;
    private AudioSource _audioSource;
    private int bulletsPerMagzine;
    private float nextTimeToFire = 0f;
    private bool isEnemyDead = false;
    private bool destinationReached = false;
    [HideInInspector] public bool isCoverAvailable = false;

    private void Start()
    {
        mainCamTransform = Camera.main.transform;
        CameraControlComponent = mainCamTransform.GetComponent<CameraControl>();
        playerObj = CameraControlComponent.playerObj;
        fPSPlayer = playerObj.GetComponent<FPSPlayer>();
        _audioSource = GetComponent < AudioSource>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = movementSpeed;
        _agent.enabled = true;
        bulletsPerMagzine = bulletsPerMagzineDefault;
        target = playerObj.transform;
        //99
        //33
        //

        damage = /*0.005f + */((PlayerPrefs.GetInt("CoverStrikeCurrentLevel") / (70 - PlayerPrefs.GetFloat("Difficulty"))));
    }

    private void Update()
    {
        if (_agent.enabled)
            _agent.destination = destinationPoint.position;
        
        if (destinationReached)
            transform.LookAt(target.position);

        if (_AIState == AIState.standingFire)
        {
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                if (!Player.Instance.canMove)
                {
                    Invoke("CanFire" , 1.5f);
                    Fire();
                }
                else
                    canFire = false;
            }
        }

        if (_agent.enabled && _agent.remainingDistance != Mathf.Infinity && _agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
        {
            if (isCoverAvailable)
            {
                _AIState = AIState.cover;
                UpdateAIState();
                StartCoroutine(Engage());
            }
            else
            {
                _AIState = AIState.standingFire;
                UpdateAIState();
            }

            destinationReached = true;
            _agent.enabled = false;
        }
    }

    private void UpdateAIState()
    {
        switch(_AIState)
        {
            case AIState.run:
                _animator.SetTrigger("Run");
                break;
            case AIState.cover:
                _animator.SetTrigger("Cover");
                break;
            case AIState.standingFire:
                _animator.SetTrigger("StandingFire");
                break;
            case AIState.standingReload:
                _animator.SetTrigger("StandingReload");
                break;
            case AIState.crouchReload:
                _animator.SetTrigger("CrouchReload");
                break;
            case AIState.grenade:
                _animator.SetTrigger("Grenade");
                break;
        }
    }

    // Call once, if not engaged
    private IEnumerator Engage()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        _AIState = AIState.standingFire;
        UpdateAIState();
    }

    private void Fire()
    {
        if(canFire)
        { 
            if(bulletsPerMagzine > 0)
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

                if (bulletClip)
                {
                    _audioSource.volume = bulletSoundVolume;
                    _audioSource.PlayOneShot(bulletClip);
                }

                fireRotation *= Quaternion.Euler(Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), 0);
                Instantiate(bulletObject, bulletSpawn.position, fireRotation);
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.gameObject.layer.Equals(11))
                    {
                        if (hit.collider.gameObject.GetComponent<FPSPlayer>())
                        {
                            if(hit.collider.gameObject.GetComponent<FPSPlayer>().hitPoints <= 0f)
                            {
                                this.enabled = false;
                            }

                            if (Player.Instance.canCrouch)
                                hit.collider.gameObject.GetComponent<FPSPlayer>().ApplyDamage(damage / 2);
                            else
                                hit.collider.gameObject.GetComponent<FPSPlayer>().ApplyDamage(damage);
                        }
                    }
                }

                bulletsPerMagzine--;
                if(canThrowGrenade)
                {
                    if (Random.value < oddsToSecondaryFire)
                    {
                        StartCoroutine(ThrowGrenade());
                    }
                }
            
                if(Random.value < oddsToCover && canTakeCover &&
                    _AIState != AIState.grenade && _AIState != AIState.crouchReload && _AIState != AIState.standingReload)
                {
                    if(isCoverAvailable)
                        StartCoroutine(TakeCover());
                }
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    private IEnumerator Reload()
    {
        if (isCoverAvailable)
            _AIState = AIState.crouchReload;
        else
            _AIState = AIState.standingReload;
        UpdateAIState();

        yield return new WaitForSeconds(reloadTime);

        bulletsPerMagzine = bulletsPerMagzineDefault;
        _AIState = AIState.standingFire;
        UpdateAIState();
    }

    private IEnumerator ThrowGrenade()
    {
        StartCoroutine(SetTimeUntilNextGrenade());
        _AIState = AIState.grenade;
        UpdateAIState();
        yield return new WaitForSeconds(0.25f);

        GameObject grenade = Instantiate(grenadeObject, /*grenadeSpawn.position*/transform.position + new Vector3(0f, 2f, 0.5f), /*grenadeSpawn.rotation*/transform.rotation) as GameObject;
        grenade.GetComponent<CS_Grenade>().SetTarget(target.position);
        yield return new WaitForSeconds(0.25f);
        _AIState = AIState.standingFire;
        UpdateAIState();


    }

    private IEnumerator SetTimeUntilNextGrenade()
    {
        canThrowGrenade = false;
        yield return new WaitForSeconds(minTimeBetweenSecondaryFire);
        canThrowGrenade = true;
    }

    private IEnumerator TakeCover()
    {
        _AIState = AIState.cover;
        UpdateAIState();
        yield return new WaitForSeconds(Random.Range(2.5f, 5f));
        StartCoroutine(SetTimeUntilNextCover());
        _AIState = AIState.standingFire;
        UpdateAIState();
    }

    private IEnumerator SetTimeUntilNextCover()
    {
        canTakeCover = false;
        yield return new WaitForSeconds(minTimeBetweenCover);
        canTakeCover = true;
    }

    private void EnableRagdoll()
    {
        isEnemyDead = true;
        if (spawner != null)
            spawner.UnregisterSpawnedNPC(this.gameObject);
        GameManager.Instance.OnEnemyKilled();
        // Death sound
        if (AudioManager.instance !=null)
        {
            AudioManager.instance.otherAudioSource.volume = 0.5f;
            AudioManager.instance.otherAudioSource.PlayOneShot(enemyDownClip[Random.Range(0,enemyDownClip.Length)]);
        }

        // Drop weapon
        weaponObject.SetActive(false);
        GameObject go = Instantiate(weaponObjectToSpawn, weaponObject.transform.position, weaponObject.transform.rotation) as GameObject;
        go.transform.Rotate(weaponObjectRotation);
        Vector3 tempGunpos = go.transform.position + (transform.forward * 0.45f);
        go.transform.position = tempGunpos;
        if (weaopnStayTime > 0.0f && go.GetComponent<WeaponPickup>())
        {
            go.GetComponent<WeaponPickup>().StartCoroutine(go.GetComponent<WeaponPickup>().DestroyWeapon(weaopnStayTime));
        }

        // Weapon sound
        if (AudioManager.instance !=null)
            AudioManager.instance.otherAudioSource.PlayOneShot(fallDownClip);

        int i;
        for (i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
        }

        //for (i = 0; i < collidersToEnable.Length; i++)
        //{
        //    collidersToEnable[i].enabled = true;
        //}

        //Aplly force
        //for (i = 0; i < rigidbodies.Length; i++)
        //{
        //    rigidbodies[i].AddForce(2.5f, 2.5f, -5f, ForceMode.Impulse);
        //}

        _animator.enabled = false;
        this.enabled = false;
        Invoke("DestroyBody" , bodyStayTime);
    }

    public void ApplyDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0f && !isEnemyDead)
        {
            EnableRagdoll();
        }
    }

    void DestroyBody()
    {
        Destroy(this.gameObject);
    }

    void CanFire()
    {
        canFire = true;
    }

    private void OnDisable()
    {
        CancelInvoke("CanFire");
        CancelInvoke("DestroyBody");
        StopCoroutine("Engage");
        StopCoroutine("ThrowGrenade");
        StopCoroutine("TakeCover");
        StopCoroutine("Reload");
        StopCoroutine("SetTimeUntilNextGrenade");
        StopCoroutine("SetTimeUntilNextCover");
    }
}
