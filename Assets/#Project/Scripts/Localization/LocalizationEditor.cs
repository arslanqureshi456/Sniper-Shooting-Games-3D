using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;
using UnityEngine.Networking;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine.UI;
#if UNITY_EDITOR
[CustomEditor(typeof(LocalizedText))]
[CanEditMultipleObjects]
#endif
#if UNITY_EDITOR
public class LocalizationEditor : Editor
{
    string[] AllowedLanguages = { "English", "Indonesian", "Mexican", "Russian", "Arabic", "French", "German", "Hindi", "Japanese", "Korean", "Turkish", "Chinese" };
    static string[] LanguageCodes = { "en", "id", "es", "ru", "ar", "fr", "de", "hi", "ja", "ko", "tr", "zh-CN" };
    LocalizedText localizedText;
    string EnglishInput;
    int currentTranslation = 0;
    //private void Awake()
    //{
    //    if (localizedText == null)
    //    {
    //        localizedText = (LocalizedText)target;
    //        if (localizedText.Translations.Length == 0)
    //        {
    //            localizedText.Translations = new string[AllowedLanguages.Length];
    //            EditorUtility.SetDirty(localizedText);
    //        }

    //    }
    //}
    void OnEnable()
    {
        if (localizedText == null)
        {
            localizedText = (LocalizedText)target;
        }
        if (localizedText.Translations == null || localizedText.Translations.Length != AllowedLanguages.Length)
        {
            localizedText.Translations = new string[AllowedLanguages.Length];
            EditorUtility.SetDirty(localizedText);
        }
    }
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //EnglishInput = EditorGUILayout.TextField("Translate", EnglishInput);
        // Show the custom GUI controls.
        if (GUILayout.Button("Translate!"))
        {
            EnglishInput = EnglishInput = localizedText.transform.GetComponent<Text>().text;
            localizedText.Translations[0] = EnglishInput;
            currentTranslation = 1;
            EditorCoroutineUtility.StartCoroutineOwnerless(GetRequest());
            //for (int i=0; i < AllowedLanguages.Length; i++)
            //{
            //    damageProp.GetArrayElementAtIndex(damageProp.arraySize - 1).stringValue = EnglishInput;
            //} 
        }
        ShowLanguages();
        //serializedObject.ApplyModifiedProperties();
    }
#endif
    void ShowLanguages()
    {
        for (int i = 0; i < AllowedLanguages.Length; i++)
        {
            if (localizedText != null)
                EditorGUILayout.TextField(AllowedLanguages[i], localizedText.Translations[i]);
            else
                EditorGUILayout.TextField(AllowedLanguages[i], "N/A");

        }

    }
    IEnumerator GetRequest()
    {
        while (currentTranslation < AllowedLanguages.Length)
        {
            string uri = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=" + LanguageCodes[currentTranslation] + "&dt=t&q=" + EnglishInput;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();
                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
#if UNITY_EDITOR
                    Debug.Log("error");
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
#endif
                }
                else if (webRequest.isDone)
                {
#if UNITY_EDITOR
                    Debug.Log(" text " + webRequest.downloadHandler.text);
#endif
                    var TableReturned = JSONNode.Parse(webRequest.downloadHandler.text);
                    System.Text.StringBuilder tranlatedText = new System.Text.StringBuilder();
                    if (TableReturned[0].Count > 0)
                    {
                        for (int i = 0; (i < TableReturned[0].Count); i++)
                        {
                            tranlatedText.Append((string)TableReturned[0][i][0]);
                        }

                    }
                    localizedText.Translations[currentTranslation] = tranlatedText.ToString();
                }
            }
            yield return new WaitForSecondsRealtime(Random.Range(5.3f,9.4f));
            currentTranslation++;
        }
        //serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(localizedText);
        Repaint();
        yield return null;
    }
    // Custom GUILayout progress bar.
    void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}
#endif
