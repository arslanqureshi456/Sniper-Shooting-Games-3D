using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public float movementSpeed = 5f;
    public PlayerCharacter _playerCharacter;
    public GameObject controls, killCamera, reloadingCircle;

    private Transform playerSpawnPoint;

    private GameObject[] waveStartPoints;
    private GameObject[] waveObjects;

    private NavMeshAgent _navMeshAgent;
    private FPSPlayer _fpsPlayer;
    private List<GameObject> covers = new List<GameObject>();
    private Transform target;
    [HideInInspector] public int waveObjectcurrentIndex = 0;
    private int waveStartPointcurrentIndex = 0;
    //private bool dontFollow = false;
    [HideInInspector] public bool canMove = false;
    [HideInInspector] public bool canCrouch = false;
    [HideInInspector] public bool isReloading = false;

    private GameObject[] enemies;
    public Transform enemyTarget;

    private float speed = 0.5f;
    private float lerpFactor = 0.005f;
    private float lerpAmount = 0f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        waveObjectcurrentIndex = waveStartPointcurrentIndex = 0;
        playerSpawnPoint = GameObject.FindWithTag("PlayerSpawnPoint").transform;
        _fpsPlayer = GetComponent<FPSPlayer>();

        //Adjust player position and rotation
        _fpsPlayer.GetComponent<FPSRigidBodyWalker>().startingPos = playerSpawnPoint.position;
        _fpsPlayer.transform.position = playerSpawnPoint.position;
        _fpsPlayer.MouseLookComponent.transform.eulerAngles = playerSpawnPoint.eulerAngles;
        _fpsPlayer.MouseLookComponent.minimumY = -30f;
        _fpsPlayer.MouseLookComponent.maximumY = 30f;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = movementSpeed;
        _navMeshAgent.enabled = true;

        waveObjects = GameObject.FindGameObjectsWithTag("Wave");

        StartNextWave();
    }

    private void Update()
    {
        if (target != null)
        {
            if (canMove && _navMeshAgent.enabled)
            {
                //print("Here");
                //dontFollow = true;
                // Player model and camera rotation
                _playerCharacter.transform.LookAt(target);

                lerpAmount += speed * lerpFactor;
                Vector3 temp = enemyTarget.transform.position - _fpsPlayer.CameraControlComponent.transform.position;

                Quaternion Rot = Quaternion.LookRotation(temp, Vector3.up);
                _fpsPlayer.MouseLookComponent.rotationX = Mathf.Lerp(_fpsPlayer.MouseLookComponent.rotationX, Rot.eulerAngles.y, lerpAmount);//Rot.eulerAngles.y;
                _fpsPlayer.MouseLookComponent.rotationY = 0f;

                //Vector3 relativePos = target.position - transform.position;

                //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                //_fpsPlayer.MouseLookComponent.rotationX = rotation.eulerAngles.y;
                //_fpsPlayer.MouseLookComponent.rotationY = 0f;

                HidePanel();
                canCrouch = false;
                _navMeshAgent.SetDestination(target.position);
                _playerCharacter.moveInputs = new Vector2(0f, 1f);
            }
            else
            {
                if (canCrouch)
                {
                    _playerCharacter.walkerComponent.crouched = true;
                    //_playerCharacter.GetComponent<Animator>().SetBool("CanRoll", true);
                }
                else if (_playerCharacter.walkerComponent.crouched)
                {
                    _playerCharacter.walkerComponent.crouched = false;
                }
            }

            if (_navMeshAgent.enabled && _navMeshAgent.remainingDistance != Mathf.Infinity && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending)
            {
                //dontFollow = false;
                ShowPanel();
                canMove = false;
                _navMeshAgent.enabled = false;
                _playerCharacter.moveInputs = Vector2.zero;

                //enemies = GameObject.FindGameObjectsWithTag("Enemy");
                //if (enemies != null)
                //{
                //    foreach (GameObject go in enemies)
                //    {
                //        if (go.GetComponent<CS_Enemy>().HP > 0)
                //        {
                //            enemyTarget = go.transform;
                //            break;
                //        }
                //    }

                //    lerpAmount += speed * lerpFactor;
                //    Vector3 temp = enemyTarget.transform.position - _fpsPlayer.CameraControlComponent.transform.position;
                //    float _angle = (Quaternion.Angle(Quaternion.LookRotation(temp, Vector3.up), transform.rotation));

                //    Quaternion Rot = Quaternion.LookRotation(temp, Vector3.up);
                //    _fpsPlayer.MouseLookComponent.rotationX = Mathf.Lerp(_fpsPlayer.MouseLookComponent.rotationX, Rot.eulerAngles.y, lerpAmount);//Rot.eulerAngles.y;
                //    _fpsPlayer.MouseLookComponent.rotationY = 0f;
                //}
            }

            //if (!dontFollow && _navMeshAgent.enabled && _navMeshAgent.remainingDistance != Mathf.Infinity && _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending)
            //{
            //    dontFollow = true;
            //}
        }

        //if (dontFollow)
        //{
        //    print("Here...");
        //    lerpAmount += speed * lerpFactor;
        //    Vector3 temp = enemyTarget.transform.position - _fpsPlayer.CameraControlComponent.transform.position;

        //    Quaternion Rot = Quaternion.LookRotation(temp, Vector3.up);
        //    _fpsPlayer.MouseLookComponent.rotationX = Mathf.Lerp(_fpsPlayer.MouseLookComponent.rotationX, Rot.eulerAngles.y, lerpAmount);//Rot.eulerAngles.y;
        //    _fpsPlayer.MouseLookComponent.rotationY = 0f;
        //}
    }

    private Transform GetNearestCover()
    {
        canMove = true;

        covers.Clear();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Cover"))
        {
            if (Vector3.Distance(transform.position, go.transform.position) > 1f)
                covers.Add(go);
        }

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = this.transform.position;
        foreach (GameObject potentialTarget in covers)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }
        return bestTarget;
    }

    private Transform GetRandomCover()
    {
        canMove = true;
        _navMeshAgent.enabled = true;

        covers.Clear();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Cover"))
        {
            if (Vector3.Distance(transform.position, go.transform.position) > 2.5f)
                covers.Add(go);
        }

        Transform bestTarget = null;
        bestTarget = covers[Random.Range(0, covers.Count)].transform;
        return bestTarget;
    }

    public void StartNextWave()
    {
        canMove = true;
        _navMeshAgent.enabled = true;
        foreach (GameObject go in waveObjects)
        {
            go.SetActive(false);
        }
        waveObjects[waveObjectcurrentIndex].SetActive(true);
        waveStartPoints = GameObject.FindGameObjectsWithTag("Cover");
        target = waveStartPoints[waveStartPointcurrentIndex].transform;
        waveObjects[waveObjectcurrentIndex].GetComponent<CS_Spawner>().enabled = true;
        enemyTarget = waveObjects[waveObjectcurrentIndex].GetComponent<CS_Spawner>()._destTransforms[0].parent;
        waveObjectcurrentIndex++;
        waveStartPointcurrentIndex++;

        if (waveObjectcurrentIndex.Equals(waveObjects.Length))
            waveObjectcurrentIndex = 0;

        if (waveStartPointcurrentIndex.Equals(waveStartPoints.Length))
            waveStartPointcurrentIndex = 0;
    }

    public void HidePanel()
    {
        controls.SetActive(false);
    }

    public void ShowPanel()
    {
        controls.SetActive(true);
    }

    public void ChangeCover()
    {
        target = GetRandomCover();
    }

    public void EnableKillCam()
    {
        killCamera.SetActive(true);
    }

    public void EnableReloadingCircle()
    {
        reloadingCircle.SetActive(true);
    }

    public void DisableReloadingCircle()
    {
        reloadingCircle.SetActive(false);
    }
}