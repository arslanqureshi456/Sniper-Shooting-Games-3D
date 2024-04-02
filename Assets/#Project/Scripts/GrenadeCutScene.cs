using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeCutScene : MonoBehaviour
{
    public float speed = 1.0f;
    public FPSPlayer _fpsPlayer;
    public GameObject grenadeCutScenePanel, crosshair;
    [HideInInspector] public Transform target_1;
    [HideInInspector] public Transform target_2;

    private enum State
    {
        Forward,
        Barckward,
        Done,
    }
    private State _state;
    private bool moveForward, moveBackward = false;

    private void OnEnable()
    {
        _fpsPlayer.invulnerable = true;
        grenadeCutScenePanel.SetActive(true);
        GameManager.Instance.gamePlayPanel.SetActive(false);
        crosshair.SetActive(false);
        _state = State.Forward;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Forward:
                MoveForward();
                break;
            case State.Barckward:
                StartCoroutine(MoveBackward());
                break;
            case State.Done:
                transform.LookAt(this.target_1);
                break;
        }
    }

    private void MoveForward()
    {
        float step = speed * Time.unscaledDeltaTime; // calculate distance to move
        if (!moveForward)
            transform.position = Vector3.MoveTowards(transform.position, target_1.position, step);
        transform.LookAt(target_1);
        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target_1.position) < 2f)
        {
            moveForward = true;
            _state = State.Barckward;
        }
    }

    private IEnumerator MoveBackward()
    {
        target_2 = GameObject.FindWithTag("Player").transform;
        yield return new WaitForSecondsRealtime(0.5f);
        float step = speed * Time.unscaledDeltaTime; // calculate distance to move
        if (!moveBackward)
            transform.position = Vector3.MoveTowards(transform.position, target_2.position, step);

        transform.LookAt(this.target_1);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target_2.position) < 2f)
        {
            moveBackward = true;
            _state = State.Done;
            Time.timeScale = 1;
            StartCoroutine(DisableCamera());
        }
    }

    private IEnumerator DisableCamera()
    {
        yield return new WaitForSecondsRealtime(5f);
        this.GetComponent<Camera>().enabled = false;
        this.enabled = false;
        //this.gameObject.SetActive(false);
        _fpsPlayer.invulnerable = false;
        grenadeCutScenePanel.SetActive(false);
        crosshair.SetActive(true);
        GameManager.Instance.gamePlayPanel.SetActive(true);
    }

    private void OnDisable()
    {
        StopCoroutine("DisableCamera");
        StopCoroutine("MoveBackward");
    }
}