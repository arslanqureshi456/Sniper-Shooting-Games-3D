using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCamera : MonoBehaviour
{
    public FakeFire _fakeFire;

    private float speed = 20f;
    [HideInInspector] public Transform target_1;
    private bool moveForward, runOnce;

    void Update()
    {
        if (transform == null || target_1 == null)
        {
            GameManager.Instance.fpsCamera.enabled = true;
            _fakeFire.enabled = false;
            DestroyImmediate(gameObject, true);
            return;
        }
        float step = speed * Time.unscaledDeltaTime; // calculate distance to move
        if (!moveForward)
            transform.position = Vector3.MoveTowards(transform.position, target_1.position + new Vector3(0f, 1f, 0f), step);
        transform.LookAt(target_1);

        GameManager.Instance.fpsCamera.enabled = false;
        GameManager.Instance.fpsCamera.transform.LookAt(target_1);

        if (Vector3.Distance(transform.position, target_1.position) < 2.5f)
        {
            moveForward = true;
            if (!runOnce)
            {
                runOnce = true;
                _fakeFire.target = target_1;
                _fakeFire.enabled = true;
                StartCoroutine(GameManager.Instance.fpsPlayer.ActivateBulletTime(1f));
                StartCoroutine(DisableScript());
            }
        }
    }

    private IEnumerator DisableScript()
    {
        yield return new WaitForSecondsRealtime(5f);//3.5
        GameManager.Instance.fpsCamera.enabled = true;
        _fakeFire.enabled = false;
        this.enabled = false;
    }

    private void OnDisable()
    {
        StopCoroutine("DisableScript");
    }
}
