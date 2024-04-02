using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EModules;
using EModules.EModulesInternal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;



public class EModulesHierarchy_ProjectSettings_HighLighter : ScriptableObject/*, ISerializable, IDeserializationCallback*/, PluginIDField {

    [HideInInspector, SerializeField]    internal int pluginID;
    int PluginIDField.pluginID
    {   get { return pluginID; }
        set { pluginID = value; }
    }
    
    const string modName0  = "HierarchySettings_HighLighter";
    const string modName1  = "ProjectSettings_HighLighter";
    
    
    
    
    [SerializeField]
    internal bool Initialized;
    [SerializeField]
    List<Adapter.ColorFilter> _colorFilters  = new List<Adapter.ColorFilter>();
    internal class FakeClass {
        internal bool Initialized;
        internal List<Adapter.ColorFilter> _colorFilters  = new List<Adapter.ColorFilter>();
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
        
        
            if (adapter.par.SaveSettingsHighLighterToLibrary)
            {   var f = Adapter.ReadLibraryFile(adapter.pluginID == Initializator.HIERARCHY_ID ? modName0 : modName1);
                var reader = new System.IO.StringReader(f);
                {   sw:;
                    switch (reader.ReadLine())
                    {   case "Initialized": _GetFakeClass[adapter.pluginID].Initialized = bool.Parse( reader.ReadLine()); goto sw; //0
                        case "_colorFilters":  _GetFakeClass[adapter.pluginID]._colorFilters.Add(Adapter.ColorFilter.ReadFromString(ref reader)); goto sw; //0
                        case null: break;
                    }
                }
                reader.Dispose();
            }
            else
            {   _GetFakeClass[adapter.pluginID].Initialized = _getAssetInstance(adapter).Initialized;
                _GetFakeClass[adapter.pluginID]._colorFilters = _getAssetInstance(adapter)._colorFilters;
            }
        }
        return _GetFakeClass[adapter.pluginID];
    }
    
    internal static void SetUndo(Adapter adapter, string undoScring)
    {   if (adapter.par.SaveSettingsHighLighterToLibrary) return;
        Undo.RecordObject(  _getAssetInstance(adapter), undoScring);
    }
    
    internal static void SetDirty(Adapter adapter)
    {
    
        if (adapter.par.SaveSettingsHighLighterToLibrary)
        {   if (_GetFakeClass[adapter.pluginID] == null) return;
            var result = new System.Text.StringBuilder();
            result.AppendLine("Initialized");
            result.AppendLine(_GetFakeClass[adapter.pluginID].Initialized.ToString());
            foreach (var item in _GetFakeClass[adapter.pluginID]._colorFilters) { result.AppendLine("_colorFilters"); item.SaveToString(ref result); } //0
            Adapter.WriteLibraryFile(adapter.pluginID == Initializator.HIERARCHY_ID ? modName0 : modName1, ref result);
        }
        else
        {   if (_GetFakeClass[adapter.pluginID] != null)
            {   _getAssetInstance(adapter).Initialized =  _GetFakeClass[adapter.pluginID].Initialized  ;
                _getAssetInstance(adapter)._colorFilters = _GetFakeClass[adapter.pluginID]._colorFilters ;
                Adapter.SetDirty( _getAssetInstance(adapter) );
            }
        }
    }
    
    internal static void SwitchLibraryMode(Adapter adapter, bool newValue, bool keepYourSettings)
    {   if (newValue == adapter.par.SaveSettingsHighLighterToLibrary) return;
        Instance(adapter);
        adapter.par.SaveSettingsHighLighterToLibrary = newValue;
        adapter.SavePrefs();
        SetDirty(adapter);
        
        if (!keepYourSettings)
        {   if (adapter.par.SaveSettingsHighLighterToLibrary)
            {   /*  foreach (var item in Resources.FindObjectsOfTypeAll<EModulesHierarchy_ProjectSettings_HighLighter>())
                  {   if (item.pluginID == adapter.pluginID)
                          DestroyImmediate(item, true);
                  }*/
                var path = AssetDatabase.GetAssetPath(_getAssetInstance(adapter));
                if (!string.IsNullOrEmpty(path)) AssetDatabase.DeleteAsset(path);
                //  DestroyImmediate(_getAssetInstance(adapter), true);
            }
            else
            {   Adapter.RemoveLibraryFile(adapter.pluginID == Initializator.HIERARCHY_ID ? modName0 : modName1);
            }
        }
        /* else
         {   adapter.par.SaveSettingsHighLighterToLibrary = newValue;
         }*/
        
        
        cache = null;
        fa.m_cache.Clear();
        
        Adapter.EditorUtilityRequestScriptReload();
    }
    
    
    [NonSerialized] static   EModulesHierarchy_ProjectSettings_HighLighter cache = null;
    [NonSerialized] static  FileAssetInitializator<EModulesHierarchy_ProjectSettings_HighLighter> fa = new FileAssetInitializator<EModulesHierarchy_ProjectSettings_HighLighter>();
    internal static EModulesHierarchy_ProjectSettings_HighLighter _getAssetInstance(Adapter adapter)
    {
    
        if (adapter.par.SaveSettingsHighLighterToLibrary) return null;
        
        
        if (fa.TryGetCachedAsset( adapter, ref cache )) return cache;
        bool wasCreated;
        cache = fa.TryGetAsset( adapter, adapter.pluginID == Initializator.HIERARCHY_ID ? (modName0 + ".asset") : (modName1 + ".asset"),
                                out wasCreated, CreateFile : true, useOldLoader : true);
        return cache;
    }
    
    
    
}


