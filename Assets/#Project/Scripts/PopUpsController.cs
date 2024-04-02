using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpsController : MonoBehaviour
{
    //public InAppManager _InAppManager;
    int index = 0;
    public Animator[] bundles;
    public Text[] discount, price;
    void OnEnable()
    {
        //_InAppManager.SetPacksPrice();
        index = Random.Range(0, bundles.Length);
        Expand();
    }
    public void ClosePanel()
    {
        bundles[index].SetTrigger("Shrink");
        gameObject.SetActive(false);
        MainMenuManager.Instance._ClosePackPanel();
    }
    public void NextBundle()
    {
        bundles[index].SetTrigger("Shrink");
        index++;
        if (index == bundles.Length)
            index = 0;
        Invoke("Expand",0.1f);
    }
    public void PreviousBundle()
    {
        bundles[index].SetTrigger("Shrink");
        index--;
        if (index == -1)
            index = bundles.Length - 1;
        Invoke("Expand", 0.1f);
    }
    void Expand()
    {
        //discount[index].text = _InAppManager.dispacksPrices[index];
        //price[index].text = _InAppManager.packsPrices[index];
        bundles[index].SetTrigger("Expand");
    }

    private void OnDisable()
    {
        CancelInvoke("Expand");
    }
}
