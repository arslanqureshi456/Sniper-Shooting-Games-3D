using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstantUpdate : MonoBehaviour
{
    public static ConstantUpdate Instance;
    public float factor = 0.05f, factorTarget = 0.0005f, facStart = 0.09f;
    float lerpAmount = 1;
    public float SPTarget = 0, GoldTarget = 0;
    float SPStart, GoldStart;
    public float LERFAC = 0.04f;
    public Text mainMenuGold,
        mainMenuSP,
        storeGold,
        storeSP,
        fullSpecificationGold,
        fullSpecificationSP,
        adrenaline_25Gadget,
        explosive_G65Gadget,
        PPGold,
        PPSP,
        modeSelectionGold,
        modeSelectionSP,
        bulletsGOld,
        bulletsSp;
        //loadOutSP,
        //campaignGOld,
       //campaignSP,
        //multiGold,
       // multiSP;

    private int _gold, _sp, _adrenaline_25, _explosive_G65 = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            _gold = PlayerPrefs.GetInt("gold");
            _sp = PlayerPrefs.GetInt("secretPoints");
            _adrenaline_25 = PlayerPrefs.GetInt("Injection");
            _explosive_G65 = PlayerPrefs.GetInt("Grenade");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void FixedUpdate()
    {
        factor = Mathf.Lerp(factor, 0.00006f, 0.0687f + (lerpAmount * 0.0973f));
        lerpAmount += factor;
        _gold = (int)Mathf.Lerp(GoldStart, GoldTarget, lerpAmount);
        _sp = (int)Mathf.Lerp(SPStart, SPTarget, lerpAmount);
        UpdateValues();
    }
    void UpdateValues()
    {
        if (lerpAmount >= 1)
        {
            _gold = PlayerPrefs.GetInt("gold");
            _sp = PlayerPrefs.GetInt("secretPoints");
            _adrenaline_25 = PlayerPrefs.GetInt("Injection");
            _explosive_G65 = PlayerPrefs.GetInt("Grenade");
            enabled = false;
        }
        /*campaignGOld.text = multiGold.text  =*/ bulletsGOld.text = PPGold.text = mainMenuGold.text = modeSelectionGold.text = storeGold.text =
            fullSpecificationGold.text = System.String.Empty + _gold;
        /*campaignSP.text = multiSP.text  =*/ bulletsSp.text = PPSP.text = mainMenuSP.text = modeSelectionSP.text = storeSP.text =
            fullSpecificationSP.text = adrenaline_25Gadget.text =
          explosive_G65Gadget.text = System.String.Empty + _sp;
        adrenaline_25Gadget.text = System.String.Empty + _adrenaline_25;
        explosive_G65Gadget.text = System.String.Empty + _explosive_G65;
    }
    private void Start()
    {
        UpdateValues();
    }

    public void UpdateCurrency()
    {
        //Debug.Log("calling update");
        factor = 0.13f;
        lerpAmount = 0;
        enabled = true;
        SPStart = _sp;
        GoldStart = _gold;
        GoldTarget = PlayerPrefs.GetInt("gold");
        SPTarget = PlayerPrefs.GetInt("secretPoints");
        //UpdateValues();
    }
}
