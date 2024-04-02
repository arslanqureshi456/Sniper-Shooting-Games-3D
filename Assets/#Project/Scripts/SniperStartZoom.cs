using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperStartZoom : MonoBehaviour
{
    public Transform[] targets;
    float lerpAmount, step = 0.85f;
    int level = 0;
    bool move = false;
    public GameObject gameUI;
    private void Start()
    {
        Invoke("starting", 0.5f);
    }

    void starting()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
        {
            if (!PlayerPrefs.GetInt("levelUnlocked-5").Equals(1))
            {
                enabled = false;
                GetComponent<Camera>().depth = 50;
                GetComponent<Camera>().enabled = false;
                Camera.main.farClipPlane = 250;
                Camera.main.enabled = true;
                AudioManager.instance.PlayAssaultBGM();
            }
            else
            {
                GameManager.Instance.ObjectivesPanel.transform.GetChild(2).gameObject.SetActive(false);
                gameUI.SetActive(false);
                try
                {
                    StartCoroutine(Delayed());
                }
                catch { }
                Camera.main.farClipPlane = 250;
            }
        }
        else
        {
            gameUI.SetActive(false);
            try
            {
                StartCoroutine(Delayed());
            }
            catch { }
        }
    }

    IEnumerator Delayed()
    {
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            GameManager.Instance.weaponsCamera.GetComponent<Camera>().depth = -20;
            yield return new WaitForSeconds(0.1f);
            level = PlayerPrefs.GetInt("sniperCurrentLevel") - 1;
            GameManager.Instance.fpsCamera.ForceLookAt(targets[level].position);

            GameManager.Instance.gamePlayPanel.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            // yield return new WaitForSeconds(0.2f);
            transform.rotation = GameManager.Instance.fpsCamera.transform.rotation;
            transform.position = targets[level].position;
            yield return new WaitForSeconds(1);
            move = true;
            //GetComponent<Camera>().enabled = false;
        }
        else
        {
            if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)
            {
                GameManager.Instance.ObjectivesPanel.SetActive(true);
                for (int i = 0; i < GameManager.Instance.activeObjectives.Conditions.Length; i++)
                {
                    GameManager.Instance.objectivesInfo[(int)GameManager.Instance.activeObjectives.Conditions[i].condition].SetActive(true);
                    GameManager.Instance.objectivesInfoValue[(int)GameManager.Instance.activeObjectives.Conditions[i].condition].text = "" + (int)GameManager.Instance.activeObjectives.Conditions[i].value;
                }
            }
            yield return new WaitForSeconds(1.78f);
            GameManager.Instance.ShowObjectives();
            GameManager.Instance.ObjectivesPanel.transform.GetChild(2).gameObject.SetActive(true);
            enabled = false;
            GetComponent<Camera>().depth = 50;
            GetComponent<Camera>().enabled = false;
            Camera.main.farClipPlane = 250;
            Camera.main.enabled = true;
            GameManager.Instance.fpsPlayer.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if(move)
        {
            lerpAmount += step * Mathf.Clamp((1 - lerpAmount), 0.1f, 1) * Time.deltaTime;
            transform.position = Vector3.Lerp(targets[level].position, Camera.main.transform.position, lerpAmount);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Camera.main.transform.rotation, lerpAmount);
            if (lerpAmount >= 1)
            {
                GameManager.Instance.ShowObjectives();
                GameManager.Instance.gamePlayPanel.SetActive(true);
                enabled = false;
                GetComponent<Camera>().depth = 50;
                GetComponent<Camera>().enabled = false;
                Camera.main.farClipPlane = 1000; // 1000;
                Camera.main.enabled = true;
                GameManager.Instance.weaponsCamera.GetComponent<Camera>().depth = 0;
            }
        }
    }

    private void OnDisable()
    {
        StopCoroutine("Delayed");
        CancelInvoke("starting");
    }
}
