using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EModules;
using EModules.EModulesInternal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


public class EModulesHierarchy_ProjectSettings_ComponentsIcons : ScriptableObject/*, ISerializable, IDeserializationCallback*/, PluginIDField {



    [HideInInspector, SerializeField]    internal int pluginID;
    int PluginIDField.pluginID
    {   get { return pluginID; }
        set { pluginID = value; }
    }
    
    
    
    const string modName0  = "HierarchySettings_ComponentsIcons";
    const string modName1  = "ProjectSettings_ComponentsIcons";
    
    
    
    
    
    
    
    
    [SerializeField] internal bool Initialized;
    [HideInInspector, SerializeField]    List<string> listKey = new List<string>();
    [HideInInspector, SerializeField]    List<Hierarchy_GUI.CustomIconParams> listValueNew = new List<Hierarchy_GUI.CustomIconParams>();
    
    
    internal class FakeClass {
        internal bool Initialized;
        internal List<string> listKey = new List<string>();
        internal List<Hierarchy_GUI.CustomIconParams> listValueNew = new List<Hierarchy_GUI.CustomIconParams>();
    }
    [UnityEditor.Callbacks.DidReloadScripts]
    static void _onReload()
    {   _GetFakeClass = new FakeClass[10];
    }
    [NonSerialized]
    static FakeClass[] _GetFakeClass = new FakeClass[10];
    internal static FakeClass Instance(Adapter adapter)
    {   if (_GetFakeClass[adapter.pluginID] == null)
        {   _GetFakeClass[adapter.pluginID] = new FakeClass();
        
        
            if (adapter.par.SaveSettingsCustomIconsToLibrary)
            {   var f = Adapter.ReadLibraryFile(adapter.pluginID == Initializator.HIERARCHY_ID ? modName0 : modName1);
                var reader = new System.IO.StringReader(f);
                {   sw:;
                    switch (reader.ReadLine())
                    {   case "Initialized": _GetFakeClass[adapter.pluginID].Initialized = bool.Parse( reader.ReadLine()); goto sw; //0
                        case "listKey": _GetFakeClass[adapter.pluginID].listKey.Add( reader.ReadLine() ); goto sw; //0
                        case "listValueNew": _GetFakeClass[adapter.pluginID].listValueNew.Add(Hierarchy_GUI.CustomIconParams.ReadFromString(ref reader)); goto sw; //0
                        case null: break;
                    }
                }
                reader.Dispose();
            }
            else
            {   _GetFakeClass[adapter.pluginID].Initialized = _getAssetInstance(adapter).Initialized;
                _GetFakeClass[adapter.pluginID].listKey = _getAssetInstance(adapter).listKey;
                _GetFakeClass[adapter.pluginID].listValueNew = _getAssetInstance(adapter).listValueNew;
            }
        }
        return _GetFakeClass[adapter.pluginID];
    }
    
    
    internal static void SetUndo(Adapter adapter, string undoScring)
    {   if (adapter.par.SaveSettingsCustomIconsToLibrary) return;
        Undo.RecordObject(  _getAssetInstance(adapter), undoScring);
    }
    
    internal static void SetDirty(Adapter adapter)
    {
    
        if (adapter.par.SaveSettingsCustomIconsToLibrary)
        {   if (_GetFakeClass[adapter.pluginID] == null) return;
            var result = new System.Text.StringBuilder();
            result.AppendLine("Initialized");
            result.AppendLine(_GetFakeClass[adapter.pluginID].Initialized.ToString());
            foreach (var item in _GetFakeClass[adapter.pluginID].listKey) { result.AppendLine("listKey"); result.AppendLine(item); } //0
            foreach (var item in _GetFakeClass[adapter.pluginID].listValueNew) { result.AppendLine("listValueNew"); item.SaveToString(ref result); } //1
            
            Adapter.WriteLibraryFile(adapter.pluginID == Initializator.HIERARCHY_ID ? modName0 : modName1, ref result);
        }
        else
        {   if (_GetFakeClass[adapter.pluginID] != null)
            {   _getAssetInstance(adapter).Initialized =  _GetFakeClass[adapter.pluginID].Initialized  ;
                _getAssetInstance(adapter).listKey = _GetFakeClass[adapter.pluginID].listKey ; //0
                _getAssetInstance(adapter).listValueNew = _GetFakeClass[adapter.pluginID].listValueNew ; //1
                Adapter.SetDirty( _getAssetInstance(adapter) );
            }
        }
    }
    
    
    internal static void SwitchLibraryMode(Adapter adapter, bool newValue, bool keepYourSettings)
    {   if (newValue == adapter.par.SaveSettingsCustomIconsToLibrary) return;
        Instance(adapter);
        adapter.par.SaveSettingsCustomIconsToLibrary = newValue;
        adapter.SavePrefs();
        SetDirty(adapter);
        if (!keepYourSettings)
        {   if (adapter.par.SaveSettingsCustomIconsToLibrary)
            {   /* foreach (var item in Resources.FindObjectsOfTypeAll<EModulesHierarchy_ProjectSettings_ComponentsIcons>())
                 {   if (item.pluginID == adapter.pluginID)
                         DestroyImmediate(item, true);
                 }*/
                var path = AssetDatabase.GetAssetPath(_getAssetInstance(adapter));
                if (!string.IsNullOrEmpty(path)) AssetDatabase.DeleteAsset(path);
                //DestroyImmediate(, true);
            }
            else
            {   Adapter.RemoveLibraryFile(adapter.pluginID == Initializator.HIERARCHY_ID ? modName0 : modName1);
            }
        }
        /*  else
          {   adapter.par.SaveSettingsCustomIconsToLibrary = newValue;
          }*/
        
        
        cache = null;
        fa.m_cache.Clear();
        
        Adapter.EditorUtilityRequestScriptReload();
    }
    
    
    
    
    
    [NonSerialized] static   EModulesHierarchy_ProjectSettings_ComponentsIcons cache = null;
    [NonSerialized] static  FileAssetInitializator<EModulesHierarchy_ProjectSettings_ComponentsIcons> fa = new FileAssetInitializator<EModulesHierarchy_ProjectSettings_ComponentsIcons>();
    internal static EModulesHierarchy_ProjectSettings_ComponentsIcons _getAssetInstance(Adapter adapter)
    {   if (fa.TryGetCachedAsset( adapter, ref cache )) return cache;
        bool wasCreated;
        cache = fa.TryGetAsset( adapter, adapter.pluginID == Initializator.HIERARCHY_ID ? (modName0 + ".asset") : (modName1 + ".asset"), out wasCreated, CreateFile : true, useOldLoader : true);
        return cache;
    }
    
    
}
