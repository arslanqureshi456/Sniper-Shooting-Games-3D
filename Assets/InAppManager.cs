using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
using Unity.Services.Core;

using Unity.Services.Core.Environments;

public class InAppManager : MonoBehaviour, IStoreListener
{
    public string environment = "production";
    static int gunIndex, modeIndex = 0;
    static string _gunID, modeID = "";
    static float _perkPoints = 0f;

    public delegate void GunInAppDelegate();
    public GunInAppDelegate onGunInAppHandler;
    public StoreWeapons _storeWeaponsdd;
    public StoreWeapons[] _storeWeapons;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;


   

    public static string iap_RemoveAds = "remove_ads";

   
    //Guns all
    public static string iap_Dis_AllSniper = "dis_all_sniper_guns";
   


    
    public static string iap_Gun20_Dis = "dis_barrett_m82";
    public static string iap_Gun21_Dis = "dis_jaguar_50";
    public static string iap_Gun22_Dis = "dis_dragon_m24";
    public static string iap_Gun23_Dis = "dis_mecha_samurai";
    public static string iap_Gun24_Dis = "dis_skull_g550";
    public static string iap_Gun25_Dis = "dis_stinger_rifle";

    

    async void Start()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            // An error occurred during initialization.
        }
    }

    public void InitializeIAP()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    private void OnEnable()
    {
        onGunInAppHandler = _storeWeaponsdd.InAppSpecificationReturn;
    }

    public void SetGunHandler()
    {
        if (PlayerPrefs.GetInt("SessionCount") == 1)
        {
            _storeWeaponsdd = _storeWeapons[WeaponStore.Instance.gunNumber];
            onGunInAppHandler = _storeWeaponsdd.InAppSpecificationReturn;
        }
    }

    public void InitializePurchasing()
    {
        
            if (IsInitialized())
            {
                return;
            }
            
                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
                

                builder.AddProduct(iap_RemoveAds, ProductType.Consumable);

               
                builder.AddProduct(iap_Gun20_Dis, ProductType.Consumable);
                builder.AddProduct(iap_Gun21_Dis, ProductType.Consumable);
                builder.AddProduct(iap_Gun22_Dis, ProductType.Consumable);
                builder.AddProduct(iap_Gun23_Dis, ProductType.Consumable);
                builder.AddProduct(iap_Gun24_Dis, ProductType.Consumable);
                builder.AddProduct(iap_Gun25_Dis, ProductType.Consumable);

                builder.AddProduct(iap_Dis_AllSniper, ProductType.Consumable);
                

                UnityPurchasing.Initialize(this, builder);
        
    }


    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

   

    #region Button Methods
    public void PurchaseGun(string id, int index)
    {
#if UNITY_ANDROID
        Debug.Log("Debug : In-App For Gun " + id);
#endif
        if (!IsInitialized())
        {
            InitializeIAP();
        }
        gunIndex = index;
        _gunID = id;
        BuyProductID(id);
    }

    

    
    public void PurchaseRemoveAds()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : In-App For " + iap_RemoveAds);
#endif
        if (!IsInitialized())
        {
            InitializeIAP();
        }
        BuyProductID(iap_RemoveAds);
    }

    public void PurchaseAllSniper()
    {
#if UNITY_ANDROID
        Debug.Log("Debug : In-App For " + iap_Dis_AllSniper);
#endif
        if (!IsInitialized())
        {
            InitializeIAP();
        }
        BuyProductID(iap_Dis_AllSniper);
    }
    
    #endregion

    void BuyProductID(string productId)
    {
       

        MainMenuManager.Instance.ShowInAppProcess();
        StartCoroutine(Purchase(productId));
    }
    System.Collections.IEnumerator Purchase(string productId)
    {
        yield return new WaitForSeconds(4.0f);
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                MainMenuManager.Instance.HideInAppProcess(1);
            }
        }
        else
        {
            MainMenuManager.Instance.HideInAppProcess(1);
        }
        yield break;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
    }
    public static int inAppCount = 0;
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        inAppCount++;
         if (String.Equals(args.purchasedProduct.definition.id, iap_RemoveAds, StringComparison.Ordinal))
        {
            AudioManager.instance.ThankYouClick();
            PlayerPrefs.SetInt("ADSUNLOCK", 1);
            if(MainMenuManager.Instance != null)
            {
                MainMenuManager.Instance.RemoveAdButton.SetActive(false);
            }
        }
        
        else if (String.Equals(args.purchasedProduct.definition.id, _gunID, StringComparison.Ordinal))
        {
            AudioManager.instance.ThankYouClick();
            PlayerPrefs.SetInt("weapon" + gunIndex, 1);
            
                PlayerPrefs.SetInt("SniperEquipped", gunIndex);
            

            onGunInAppHandler();
            if (PlayerPrefs.GetInt("SessionCount") == 1)
            {
                MainMenuManager.Instance.specificationDelegates[PlayerPrefs.GetInt("AssaultEquipped")]();
            }

            // Analytic To Get Gun Unlock By IAP
            Analytics.CustomEvent("GunByIAP", new Dictionary<string, object>
            {
               { "level_index", gunIndex }
            });
#if UNITY_EDITOR
            Debug.Log("CustomEvent: " + "GunByIAP");
#endif
        }
        
        else if (String.Equals(args.purchasedProduct.definition.id, iap_Dis_AllSniper, StringComparison.Ordinal))
        {
            AudioManager.instance.ThankYouClick();
                for (int i = 19; i < 22; i++)
            {
                PlayerPrefs.SetInt("weapon" + i, 1);
            }
            if (PlayerPrefs.GetInt("SniperEquipped") == -99)
                PlayerPrefs.SetInt("SniperEquipped", 19);
            MainMenuManager.Instance.storePanel.SetActive(false);
            MainMenuManager.Instance.storePanel.SetActive(true);
            MainMenuManager.Instance.specificationDelegates[WeaponStore.Instance.currentOrderIndex]();
            if (WeaponStore.Instance != null)
                WeaponStore.Instance.IAPAllSniperBtn.SetActive(false);

            for (int i = 0; i < 19; i++)
            {
                if (PlayerPrefs.GetInt("weapon" + i) == 1)
                {
                    if (MainMenuManager.Instance != null)
                        MainMenuManager.Instance.IAPAllGunsBtn.SetActive(false);
                }
            }
        }
        MainMenuManager.Instance.HideInAppProcess(1);
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            MainMenuManager.Instance.HideInAppProcess(2);
        else
            GameManager.Instance.HideInAppProcess(2);
    }

    void OnDisable()
    {
        StopCoroutine("Purchase");
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}