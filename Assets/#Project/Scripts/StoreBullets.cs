using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreBullets : MonoBehaviour
{
    public ScriptableBullets Bullet;
    public delegate void SetLastCard();
    public static SetLastCard lastBulletCard;
    const int ASSAULTGUNCOUNT = 13;
    //GameObject EquippedButton, BuyButton, EquipButton;// InAppButton;
    Text Price, Title;// DiscountPrice, RealPrice;
    bool isSet = false;
    void GetRefrences()
    {
        isSet = true;
        Title = transform.GetChild(2).GetComponent<Text>();
        //Price = transform.GetChild(4).GetChild(0).GetComponent<Text>();
        //RealPrice = transform.GetChild(9).GetChild(0).GetComponent<Text>();
        //DiscountPrice = transform.GetChild(9).GetChild(2).GetChild(0).GetComponent<Text>();
        //BuyButton = transform.GetChild(4).gameObject;
        //EquippedButton = transform.GetChild(5).gameObject;
        //EquipButton = transform.GetChild(6).gameObject;
        //InAppButton = transform.GetChild(9).gameObject;
        Title.text = Bullet.bulletName;
        //Price.text = System.String.Empty + (Bullet.goldPrice + Bullet.spPrice);
        //DiscountPrice.text = InAppManager.Instance.GetPriceWithId(Bullet.DiscountPriceID);
        //RealPrice.text = InAppManager.Instance.GetPriceWithId(Bullet.RealPriceID);
        //BuyButton.GetComponent<Button>().onClick.AddListener(BuyBullet);
        //EquipButton.GetComponent<Button>().onClick.AddListener(EquipBullet);
        //InAppButton.GetComponent<Button>().onClick.AddListener(InAppFunction);
        GetComponent<Button>().onClick.AddListener(SelectBullet);
    }
    void SetButtonStats()
    {
        //if (weapon.isComingSoon)
        //{
            //BuyButton.SetActive(false);
            ////InAppButton.SetActive(false);
            //EquippedButton.SetActive(false);
            //EquipButton.SetActive(false);
            //Price.text = System.String.Empty + (weapon._goldPrice + weapon._spPrice);
            return;
        //}
        //if (PlayerPrefs.GetInt("bulletUnlocked-" + Bullet.PrefIndex) == 1)
        //{
        //    BuyButton.SetActive(false);
        //    InAppButton.SetActive(false);
        //    if (PlayerPrefs.GetInt("equippedBullet") == Bullet.PrefIndex)
        //    {
        //        lastBulletCard = SetButtonStats;
        //        EquippedButton.SetActive(true);
        //        EquipButton.SetActive(false);

        //    }
        //    else
        //    {
        //        EquippedButton.SetActive(false);
        //        EquipButton.SetActive(true);
        //    }

        //}
        //else
        //{
        //    BuyButton.SetActive(true);
        //    InAppButton.SetActive(true);
        //    EquippedButton.SetActive(false);
        //    EquipButton.SetActive(false);
        //}
    }
    public void SelectBullet()
    {
        //WeaponStore.Instance.buyDeletegate = ProcessPayment;
        int a = transform.GetSiblingIndex();
        WeaponStore.Instance._SelectBulletButton(Bullet, a);
    }
    public void EquipBullet()
    {
        WeaponStore.Instance._EquipBullet(Bullet);
        SetButtonStats();
        if(lastBulletCard != null)
        {
            lastBulletCard();
            lastBulletCard = SetButtonStats;
        }
    }
    public void BuyBullet()
    {
        Camera.main.depth = 0;
        //WeaponStore.Instance.buyDeletegate = ProcessPayment;
        if (PlayerPrefs.GetInt("gold") >= Bullet.goldPrice &&
                PlayerPrefs.GetInt("secretPoints") >= Bullet.spPrice)
        {
            //WeaponStore.Instance.ShowPremissionPanel();
        }
        else
        {
            //WeaponStore.Instance._newStoreManager.EnableNotEnoughCurrencyPanel(true);
        }
    }
    void ProcessPayment(bool isSpecification)
    {
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - 1500);
        PlayerPrefs.SetInt("secretPoints", PlayerPrefs.GetInt("secretPoints") - 650);
        PlayerPrefs.SetInt("bulletUnlocked-" + Bullet.PrefIndex, 1);
        ConstantUpdate.Instance.UpdateCurrency();
        SetButtonStats();

        AudioManager.instance.ThankYouClick();
    }
    public void InAppFunction()
    {
        //InAppManager.Instance.GunInAppHandler = SetButtonStats;
        //InAppManager.Instance.PurchaseGun(weapon.RealPriceID);
    }
    private void OnEnable()
    {
        if (!isSet)
            GetRefrences();
        SetButtonStats();
    }
}
