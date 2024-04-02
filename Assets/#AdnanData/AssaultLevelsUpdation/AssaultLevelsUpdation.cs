using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AssaultLevelsUpdation : MonoBehaviour
{
    public static AssaultLevelsUpdation instance;
    public GameObject[] levelButtons; // level buttons length
    //public GameObject[] levelsBar; // dotted bars for levels unlocked
    public int lastVal; // last value of array
    public int currentVal, maxrange, minrange; // Current level , maximum number of array , minimum number of array

    public Button NextBtn, PreviousBtn;

    // Start is called before the first frame update

    
    void OnEnable()
    {
        Invoke("Firstlook", 0.05f);
    }
    void Start()
    {
        instance = this;
    }
    void Firstlook()
    { if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
             currentVal = PlayerPrefs.GetInt("sniperCurrentLevel");
        }
        
        FindingRange(currentVal);
        FirstDisplay();
        checkLeftRightButtons();
    }

    void FindingRange(int val)
    {
        int x = 0;

        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER && currentVal < 25)
        {
            do
            {
                x++;
            }
            while ((val + x) % (levelButtons.Length - 1) != 0);

            }
            if (currentVal % (levelButtons.Length - 1) == 0)
            {
                maxrange = currentVal;
            }
            else
            {
                maxrange = currentVal + x;
            }
            minrange = maxrange - (levelButtons.Length - 2);
#if UNITY_EDITOR
        // Debug.Log("last value : " + lastVal);
       // Debug.LogError("x : " + x + " min :" + minrange + " max " + maxrange);
#endif
    }


    void FirstDisplay() // on every enable
    {
        //Debug.LogError(" min :" + minrange + " max " + maxrange + "CurrentVal " + currentVal);
        for (int i = 1; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] != null)
            {
                levelButtons[i].transform.GetChild(3).GetComponent<Text>().text = ((minrange-1) + i).ToString();

                if (((minrange - 1) + i) > currentVal) // locked
                {
#if UNITY_EDITOR
                  //  Debug.Log("locked " + ((minrange - 1) + i));
#endif
                    levelButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                     levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                   // levelsBar[i].SetActive(false);

                }
                else // unlocked
                {
                    levelButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                   // levelsBar[i].SetActive(true);
                    if(((minrange - 1) + i) != currentVal)
                    {
                        levelButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
        }
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER && currentVal < 25)
        {
            levelButtons[(levelButtons.Length - 1) - (maxrange - currentVal)].transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            levelButtons[(levelButtons.Length - 1)].transform.GetChild(2).gameObject.SetActive(true);
        }
        lastVal = maxrange;
#if UNITY_EDITOR
      //  Debug.Log("Current_Level : " + currentVal);
#endif
    }
    public void Next()
    {
        AudioManager.instance.NormalClick();
        
        if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            if (lastVal < 25) // it works for divident of 5 (15levels)
            {
                NextFunction();
            }
        }
    }

    void NextFunction()
    {
            for (int i = 1; i < levelButtons.Length; i++)
            {
            if (levelButtons[i] != null)
                {
                    levelButtons[i].transform.GetChild(3).GetComponent<Text>().text = (lastVal + i).ToString();

                    if (((lastVal) + i) > currentVal) // unlocked showing
                    {
#if UNITY_EDITOR
                    // Debug.Log("locked " + ((lastVal) + i));
#endif
                    levelButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                    levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                   // levelsBar[i].SetActive(false);
                }
                else // locked Showing
                    {
                    levelButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                    //levelsBar[i].SetActive(true);
                    if((lastVal + i) != currentVal)
                    {
                        levelButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
            }
            lastVal += (levelButtons.Length - 1);
        // Here We Are Updating This , If USer IS On Any Screen , He Can Play Last Unlocked Level
        LevelSelectionNew.Instance.playButton.interactable = true;
        DisableButtons();
        DisableCurrentLevelFirst();
        checkLeftRightButtons();
    }

public void Previous()
{
    AudioManager.instance.NormalClick();
    if (lastVal > (levelButtons.Length-1)) // stop previous button working if last number is equal or less than buttons length
        {
        for (int i = 1; i < levelButtons.Length; i++)
            {
                if (levelButtons[i] != null)
                {
                    levelButtons[levelButtons.Length - i].transform.GetChild(3).GetComponent<Text>().text = (lastVal - ((levelButtons.Length - 2) + i)).ToString();

                    if ((lastVal - ((levelButtons.Length - 2) + i)) > currentVal)
                    {
#if UNITY_EDITOR
                    //  Debug.Log("locked " + (lastVal - ((levelButtons.Length - 2) + i)));
#endif
                        levelButtons[levelButtons.Length - i].transform.GetChild(0).gameObject.SetActive(true);
                    levelButtons[levelButtons.Length - i].transform.GetChild(1).gameObject.SetActive(false);
                    //levelsBar[levelButtons.Length - i].SetActive(false);
                }
                    else
                    {
                        levelButtons[levelButtons.Length - i].transform.GetChild(0).gameObject.SetActive(false);
                        //levelsBar[levelButtons.Length - i].SetActive(true);
                        if((lastVal - ((levelButtons.Length - 2) + i)) != currentVal)
                        {
                            levelButtons[levelButtons.Length - i].transform.GetChild(1).gameObject.SetActive(true);
                        }
                        else
                        {
                            levelButtons[levelButtons.Length - i].transform.GetChild(1).gameObject.SetActive(false);
                        }
                    }
                }
            }
            lastVal -= (levelButtons.Length - 1);
        }

        // Here We Are Updating This , If USer IS On Any Screen , He Can Play Last Unlocked Level
        LevelSelectionNew.Instance.playButton.interactable = true;
        DisableButtons();
    DisableCurrentLevelFirst();
    checkLeftRightButtons();
    }

  void DisableCurrentLevel()
    {
        for (int i = 1; i <= (levelButtons.Length-1); i++)
        {
            levelButtons[i].transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    int firstTime;
    public void DisableCurrentLevelFirst()
    {
        if (firstTime == 0)
        {
            levelButtons[(levelButtons.Length - 1) - (maxrange - currentVal)].transform.GetChild(2).gameObject.SetActive(false);
            firstTime = 1;
        }
    }

    void DisableButtons() // Just for Animation Purpose
    {
        for (int i = 1; i <= (levelButtons.Length - 1); i++)
        {
            levelButtons[i].transform.localScale = new Vector3(0,0,0);
        }
        Invoke("EnableButtons", 0.005f);
    }

    void EnableButtons() // Just for Animation Purpose
    {
        for (int i = 1; i <= (levelButtons.Length - 1); i++)
        {
            levelButtons[i].GetComponent<DOTweenAnimation>().DORestart();
            levelButtons[i].transform.GetChild(2).gameObject.SetActive(false);
        }
        Invoke("DisableCurrentLevel", 0.15f); // Disable Current Level Indicator , when buttons enabled again for animation

        // Here We Are Updating This , If USer IS On Any Screen , He Can Play Last Unlocked Level
        //LevelSelectionNew.Instance.playButton.interactable = false; // Next button must be disabled after playing level buttons animations
    }

    void checkLeftRightButtons()
    {
      if (LevelSelectionNew.modeSelection == LevelSelectionNew.modeType.SNIPER)
        {
            if (lastVal == 25)
            {
                NextBtn.interactable = false ;
                PreviousBtn.interactable = true ;
            }
            else if(lastVal == levelButtons.Length - 1)
            {
                NextBtn.interactable = true;
                PreviousBtn.interactable = false;
            }
            else
            {
                NextBtn.interactable = true;
                PreviousBtn.interactable = true;
            }
        }

    }

    private void OnDisable()
    {
        firstTime = 0;

        CancelInvoke("Firstlook");
        CancelInvoke("EnableButtons");
        CancelInvoke("DisableCurrentLevel");
    }
}
