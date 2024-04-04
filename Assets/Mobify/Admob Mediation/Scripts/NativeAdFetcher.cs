using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(RectTransform))]
public class NativeAdFetcher : MonoBehaviour
{
#if GoogleMobileAdsNative
    public AdsManager_AdmobMediation.NativeType nativeType;

    void OnEnable()
    {
        if (!AdsManager_AdmobMediation.Instance.isNativeLoaded) return;
        if (AdsManager_AdmobMediation.Instance.GetNativeAdReference() == null) return;

        switch (nativeType)
        {
            case AdsManager_AdmobMediation.NativeType.Icon_RawImage:
                {
                    try
                    {
                        SetObjectComponents(nativeType, true);
                        AdsManager_AdmobMediation.Instance.GetNativeAdReference().RegisterIconImageGameObject(gameObject);
                    }
                    catch (Exception) { Debug.LogError("No Object Regisger against '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
            case AdsManager_AdmobMediation.NativeType.AdChoicesLogo_RawImage:
                {
                    try
                    {
                        SetObjectComponents(nativeType, true);
                        AdsManager_AdmobMediation.Instance.GetNativeAdReference().RegisterAdChoicesLogoGameObject(gameObject);
                    }
                    catch (Exception) { Debug.LogError("No Object Regisger against '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
            case AdsManager_AdmobMediation.NativeType.Heading_Text:
                {
                    try
                    {
                        SetObjectComponents(nativeType, false);
                        AdsManager_AdmobMediation.Instance.GetNativeAdReference().RegisterHeadlineTextGameObject(gameObject);
                    }
                    catch (Exception) { Debug.LogError("No Object Regisger against '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
            case AdsManager_AdmobMediation.NativeType.Body_Text:
                {
                    try
                    {
                        SetObjectComponents(nativeType, false);
                        AdsManager_AdmobMediation.Instance.GetNativeAdReference().RegisterBodyTextGameObject(gameObject);
                    }
                    catch (Exception) { Debug.LogError("No Object Regisger against '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
            case AdsManager_AdmobMediation.NativeType.Store_Text:
                {
                    try
                    {
                        SetObjectComponents(nativeType, false);
                        AdsManager_AdmobMediation.Instance.GetNativeAdReference().RegisterStoreGameObject(gameObject);
                    }
                    catch (Exception) { Debug.LogError("No Object Regisger against '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
            case AdsManager_AdmobMediation.NativeType.Price_Text:
                {
                    try
                    {
                        SetObjectComponents(nativeType, false);
                        AdsManager_AdmobMediation.Instance.GetNativeAdReference().RegisterPriceGameObject(gameObject);
                    }
                    catch (Exception) { Debug.LogError("No Object Regisger against '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
            case AdsManager_AdmobMediation.NativeType.Action_Text:
                {
                    try
                    {
                        SetObjectComponents(nativeType, false);
                        AdsManager_AdmobMediation.Instance.GetNativeAdReference().RegisterCallToActionGameObject(gameObject);
                    }
                    catch (Exception) { Debug.LogError("No Object Regisger against '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
            case AdsManager_AdmobMediation.NativeType.Advertiser_Text:
                {
                    try
                    {
                        SetObjectComponents(nativeType, false);
                        AdsManager_AdmobMediation.Instance.GetNativeAdReference().RegisterAdvertiserTextGameObject(gameObject);
                    }
                    catch (Exception) { Debug.LogError("No Object Regisger against '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
        }
    }

    void SetObjectComponents(AdsManager_AdmobMediation.NativeType nativeType, bool isTextureObject)
    {
        switch (isTextureObject)
        {
            case true:
                {
                    try
                    {
                        Texture2D texture = AdsManager_AdmobMediation.Instance.GetNative_Texture(nativeType);
                        if (texture == null) { gameObject.SetActive(false); return; }
                        GetComponent<UnityEngine.UI.RawImage>().texture = texture;
                    }
                    catch (Exception) { Debug.LogError("No 'RawImage' Found at '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
            case false:
                {
                    try
                    {
                        string text = AdsManager_AdmobMediation.Instance.GetNative_String(nativeType);
                        if (text == "") { gameObject.SetActive(false); return; }
                        GetComponent<UnityEngine.UI.Text>().text = text;
                    }
                    catch (Exception) { Debug.LogError("No 'Text' Found at '" + name + "' Object"); gameObject.SetActive(false); }
                    break;
                }
        }
    }

    #region Editor Properties
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(NativeAdFetcher))]
    public class NativeAdProperty : UnityEditor.Editor
    {
        #region Property
        UnityEditor.SerializedProperty adTypeProperty;
        #endregion
        void OnEnable()
        {
            adTypeProperty = serializedObject.FindProperty(nameof(NativeAdFetcher.nativeType));
            NativeAdFetcher script = (NativeAdFetcher)target;
            DrawDefaultInspector();
            AdsManager_AdmobMediation.NativeType adType = (AdsManager_AdmobMediation.NativeType)adTypeProperty.enumValueIndex;

            if (script.GetComponent<UnityEngine.UI.Image>() != null || script.GetComponent<UnityEngine.UI.RawImage>() != null)
            {
                adType = AdsManager_AdmobMediation.NativeType.Icon_RawImage;
            }
            else if (script.GetComponent<UnityEngine.UI.Text>() != null)
            {
                adType = AdsManager_AdmobMediation.NativeType.Heading_Text;
            }
            serializedObject.ApplyModifiedProperties();
        }
        public override void OnInspectorGUI()
        {
            NativeAdFetcher script = (NativeAdFetcher)target;
            DrawDefaultInspector();
            AdsManager_AdmobMediation.NativeType adType = (AdsManager_AdmobMediation.NativeType)adTypeProperty.enumValueIndex;

            if (adType == AdsManager_AdmobMediation.NativeType.Icon_RawImage || adType == AdsManager_AdmobMediation.NativeType.AdChoicesLogo_RawImage)
            {
                if (script.GetComponent<UnityEngine.UI.Image>() != null)
                {
                    DestroyImmediate(script.GetComponent<UnityEngine.UI.Image>());
                }
                else if (script.GetComponent<UnityEngine.UI.Text>() != null)
                {
                    DestroyImmediate(script.GetComponent<UnityEngine.UI.Text>());
                }
                if (script.GetComponent<UnityEngine.UI.RawImage>() == null)
                {
                    script.gameObject.AddComponent<UnityEngine.UI.RawImage>();
                }
            }
            else
            {
                if (script.GetComponent<UnityEngine.UI.Image>() != null)
                {
                    DestroyImmediate(script.GetComponent<UnityEngine.UI.Image>());
                }
                else if (script.GetComponent<UnityEngine.UI.RawImage>() != null)
                {
                    DestroyImmediate(script.GetComponent<UnityEngine.UI.RawImage>());
                }
                if (script.GetComponent<UnityEngine.UI.Text>() == null)
                {
                    script.gameObject.AddComponent<UnityEngine.UI.Text>();
                }
            }
            if (script.gameObject.GetComponent<UnityEngine.UI.RawImage>() != null)
            {
                script.gameObject.GetComponent<UnityEngine.UI.RawImage>().raycastTarget = true;
            }
            if (script.gameObject.GetComponent<UnityEngine.UI.Text>() != null)
            {
                script.gameObject.GetComponent<UnityEngine.UI.Text>().raycastTarget = true;
            }

            RectTransform rectTransform = script.gameObject.GetComponent<RectTransform>();
            script.gameObject.GetComponent<BoxCollider>().size = new Vector3(rectTransform.rect.width, rectTransform.rect.height);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion

#endif
}