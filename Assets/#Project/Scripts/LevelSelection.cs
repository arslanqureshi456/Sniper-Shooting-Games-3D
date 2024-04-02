using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Text descriptionText;

    public GameObject levelDescription;
    //public GameObject reward;

    public Transform content;
    public RectTransform menuContainer;
    public RectTransform aimContainer;

    public Button playButton;
    
    //public AudioClip mainMenu, gameplay_3;

    public Text assaultXpRewardText, assaultSPRewardText, assaultGoldRewardText;
    public int[] assaultTotalEnemies = new int[30];

    [TextArea(1, 5)]
    public string[] descriptions = new string[30];

    private int descriptionIndex = 0;
    private Vector3 desiredMenuPosition;//, desiredAimPosition;
    private bool canMove = false;

    private void OnEnable()
    {
       
        //if (AudioManager.instance)
        //{
        //    AudioManager.instance.backgroundMusicSouce.clip = gameplay_3;
        //    AudioManager.instance.backgroundMusicSouce.Play();
        //}

        if(LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.ASSAULT)// Assault
        {
            descriptionIndex = PlayerPrefs.GetInt("nextLevel") - 1;
            InitAssaultLevel();
            //print("level next" + (PlayerPrefs.GetInt("nextLevel") - 1));
            OnAssaultLevelSelect(PlayerPrefs.GetInt("nextLevel") - 1);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) /*&& Input.mousePosition.x < Screen.width / 2f && !tutorialPanel.activeSelf*/)
        {
            if (aimContainer.gameObject.activeSelf)
            {
                aimContainer.gameObject.SetActive(false);
            }

            //levelDescription.SetActive(false);
            canMove = false;
        }

        if (canMove)
        {
            menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPosition, 0.15f);
        }
    }
    
    private void InitAssaultLevel()
    {
        int i = 0;
        foreach (Transform t in content)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnAssaultLevelSelect(currentIndex));

            if (content.GetChild(i).Find("Text"))
                content.GetChild(i).Find("Text").GetComponent<Text>().text = System.String.Empty + (i + 1);

            if (PlayerPrefs.GetInt("levelUnlocked-" + (i)).Equals(1))
            {
                if (content.GetChild(i).Find("Locked"))
                    content.GetChild(i).Find("Locked").gameObject.SetActive(false);
                if (content.GetChild(i).Find("Unlocked"))
                    content.GetChild(i).Find("Unlocked").gameObject.SetActive(true);
            }
            else
            {
                if (content.GetChild(i).Find("Locked"))
                    content.GetChild(i).Find("Locked").gameObject.SetActive(true);
                if (content.GetChild(i).Find("Unlocked"))
                    content.GetChild(i).Find("Unlocked").gameObject.SetActive(false);
            }

            // Level to play
            if (i.Equals(PlayerPrefs.GetInt("nextLevel") - 1))
            {
                if (content.GetChild(i).Find("Locked"))
                    content.GetChild(i).Find("Locked").gameObject.SetActive(false);
                if (content.GetChild(i).Find("Unlocked"))
                    content.GetChild(i).Find("Unlocked").gameObject.SetActive(false);
            }

            // Change y position
            content.GetChild(i).gameObject.transform.localPosition = new Vector3(content.GetChild(i).gameObject.transform.localPosition.x, Random.Range(-50, 50), content.GetChild(i).gameObject.transform.localPosition.z);

            i++;
        }

        if (descriptionIndex >= 29)
        {
            PlayerPrefs.SetInt("LevelIndex", Random.Range(1, 31));
            descriptionText.text = descriptions[PlayerPrefs.GetInt("LevelIndex") - 1];
        }
        else
        {
            descriptionText.text = descriptions[descriptionIndex];
        }

        PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("nextLevel"));
    }

    #region Button Methods
    public void OnAssaultLevelSelect(int index)
    {
        //Debug.Log("index " + index);
        if (!AudioManager.instance.otherAudioSource.isPlaying)
            AudioManager.instance.NormalClick();


        int passLevel = 0;
        float pass = PlayerPrefs.GetFloat("Pass_Level");
        switch (pass)
        {
            case 1.5f:
                passLevel = 1;
                break;
            case 2f:
                passLevel = 2;
                break;
            case 3f:
                passLevel = 3;
                break;
            default:
                passLevel = 1;
                break;
        }

        if (PlayerPrefs.GetInt("levelUnlocked-" + (index)).Equals(1))
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }

        aimContainer.gameObject.transform.SetParent(content.GetChild(index).gameObject.transform);
        aimContainer.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);

        if (!aimContainer.gameObject.activeSelf)
        {
            aimContainer.gameObject.SetActive(true);
        }
        Vector3 position = content.GetChild(index).gameObject.transform.localPosition;
        desiredMenuPosition = -(position);
        desiredMenuPosition = Vector3.right * (650) + desiredMenuPosition;//355, 300

        canMove = true;



        //if (index.Equals(0) || index.Equals(1))
        //    reward.SetActive(false);
        //else
        //    reward.SetActive(true);

        //levelDescription.SetActive(true);

        PlayerPrefs.SetInt("LevelCount", index + 1);

        if (index > 29)
        {
            PlayerPrefs.SetInt("LevelIndex", Random.Range(1, 31));
            descriptionText.text = descriptions[PlayerPrefs.GetInt("LevelIndex") - 1];
            index = PlayerPrefs.GetInt("LevelIndex") - 1;
        }
        else
        {
            descriptionText.text = descriptions[index];
        }

        int xp = (assaultTotalEnemies[index] * 55) + 150;
        xp *= passLevel;
        int sp = xp / 20;
        int gold = xp / 4;

        assaultXpRewardText.text = System.String.Empty + xp;
        assaultSPRewardText.text = System.String.Empty + sp;
        assaultGoldRewardText.text = System.String.Empty + gold;

        PlayerPrefs.SetInt("currentLevel", (index + 1));
    }
    #endregion
}
