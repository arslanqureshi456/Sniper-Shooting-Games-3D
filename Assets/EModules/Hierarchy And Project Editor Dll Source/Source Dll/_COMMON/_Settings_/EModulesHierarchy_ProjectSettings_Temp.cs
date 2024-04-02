using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EModules;
using EModules.EModulesInternal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;



public class EModulesHierarchy_ProjectSettings_Temp : ScriptableObject/*, ISerializable, IDeserializationCallback*/, PluginIDField, EModulesHierarchy_ProjectSettings_Temp.HierarchyPrefabInterface {

    internal interface HierarchyPrefabInterface {
        int _COUNT { get; set; }
        List<int> _KEYS { get; set; }
        List<UnityEngine.Object> _VALUES { get; set; }
    }
    public int _COUNT { get { return _prefab_key_Count; } set { _prefab_key_Count = value; } }
    public List<int> _KEYS { get { return GET_prefab_key; } set { GET_prefab_key = value; } }
    public List<UnityEngine.Object> _VALUES { get { return GET_prefab_value; } set { GET_prefab_value = value; } }
    
    const int STEP = 500;
    
    
    [ SerializeField]     int _prefab_key_Count = 0;
    [ SerializeField]    List<int> GET_prefab_key = new int[STEP].ToList();
    [ SerializeField]    List<UnityEngine.Object> GET_prefab_value = new UnityEngine.Object[STEP].ToList();
    
    [NonSerialized]  CachedArray __GET_PREFABS;
    internal CachedArray GET_PREFABS
    {   get
        {   if ( __GET_PREFABS  == null )
                // {   __GET_PREFABS = new CachedArray( GET_prefab_key, GET_prefab_value, _prefab_key_Count );
            {   __GET_PREFABS = new CachedArray( this );
            }
            return __GET_PREFABS;
        }
    }
    
    internal class CachedArray {
        /* int[] keys;
         UnityEngine.Object[] values;
         internal int Count;*/
        EModulesHierarchy_ProjectSettings_Temp.HierarchyPrefabInterface target;
        internal CachedArray(/* int[] keys
                              ,  UnityEngine.Object[] values
                              , int Count */ EModulesHierarchy_ProjectSettings_Temp.HierarchyPrefabInterface target)
        {   /*this.keys = keys;
             this.values = values;
             this.Count = Count;*/
            this.target = target;
        }
        
        
        
        
        internal void ClearPrefabs()
        {   _prafab_init = false;
            _prefab_dic.Clear();
        }
        bool? _prafab_init;
        Dictionary<int, int> _prefab_dic = new Dictionary<int, int>();
        internal Dictionary<int, int> PrefabsDic
        {   get
            {   if ( _prafab_init != true )
                {   _prafab_init = true;
                    if ( !Application.isPlaying )
                    {   target._COUNT = 0;
                        _prefab_dic.Clear();
                        /*= new int[0];
                        GET_prefab_value = new UnityEngine.Object[0];
                        _prefab_dic.Clear();*/
                    }
                }
                if ( _prefab_dic.Count != target._COUNT )
                {   if ( target._KEYS.Count != target._VALUES.Count || target._COUNT != target._VALUES.Count || target._COUNT != target._VALUES.Count )
                    {   var asdasd = target._VALUES.ToArray();
                        System.Array.Resize( ref asdasd, target._COUNT = target._KEYS.Count );
                        target._VALUES = asdasd.ToList();
                    }
                    _prefab_dic.Clear();
                    for ( int i = 0 ; i < target._KEYS.Count ; i++ )
                    {   _prefab_dic.Add( target._KEYS[i], i );
                    }
                }
                
                return _prefab_dic;
            }
        }
        
        int tempValue;
        internal bool  GetValueByKey(int key, out UnityEngine.Object value )
        {   if (!_prefab_dic .TryGetValue(key, out tempValue ))
            {   /*PrefabsDicAdd(key,)
                  tempValue = target._COUNT - 1;*/
                value = null;
                return false;
            }
            value = target._VALUES[tempValue];
            return true;
        }
        
        internal void PrefabsDicAdd( int key, UnityEngine.Object value )
        {   if ( _prefab_dic.ContainsKey( key ) )
            {   var i = _prefab_dic[key];
                if ( i != -1 )
                {   target._VALUES[i] = value;
                    return;
                }
                else
                {   internal_Add_OnlyArray( key, value );
                    _prefab_dic[key] = target._COUNT - 1;
                    return;
                }
            }
            else
            {   internal_Add_OnlyArray( key, value );
                _prefab_dic.Add( key, target._COUNT - 1 );
            }
        }
        
        
        void internal_Add_OnlyArray( int key, UnityEngine.Object value )
        {   var c =  target._COUNT;
            if (c >= target._KEYS.Count )
            {   target._KEYS.AddRange( Enumerable.Repeat( -1, STEP ) );
                target._VALUES.AddRange( Enumerable.Repeat( (UnityEngine.Object)null, STEP ) );
            }
            target._KEYS[c] = key;
            target._VALUES[c] = value;
            target._COUNT = c + 1;
        }
        
        /* if ( _prefab_dic.ContainsKey( key ) )
            {   var i = ArrayUtility.FindIndex(GET_prefab_key, k => k == key);
                if ( i != -1 )
                {   GET_prefab_value[i] = value;
                    _prefab_dic[key] = value;
                    return;
                }
                else
                {   var k = GET_prefab_key;
                    ArrayUtility.Add( ref k, key );
                    GET_prefab_key = k;
                    var v = GET_prefab_value;
                    ArrayUtility.Add( ref v, value );
                    GET_prefab_value = v;
                    _prefab_dic[key] = value;
                    return;
                }
            }
            else
            {   var k = GET_prefab_key;
                ArrayUtility.Add( ref k, key );
                GET_prefab_key = k;
                var v = GET_prefab_value;
                ArrayUtility.Add( ref v, value );
                GET_prefab_value = v;
                _prefab_dic.Add( key, value );
            }*/
        
        
    }
    
    
    
    
    [HideInInspector, SerializeField]    internal int pluginID;
    int PluginIDField.pluginID
    {   get { return pluginID; }
        set { pluginID = value; }
    }
    
    
    
    [NonSerialized] static   EModulesHierarchy_ProjectSettings_Temp cache = null;
    [NonSerialized] static  FileAssetInitializator<EModulesHierarchy_ProjectSettings_Temp> fa = new FileAssetInitializator<EModulesHierarchy_ProjectSettings_Temp>();
    internal static EModulesHierarchy_ProjectSettings_Temp Instance(Adapter adapter)
    {   if (fa.TryGetCachedAsset( adapter, ref cache )) return cache;
        bool wasCreated;
        cache = fa.TryGetAsset( adapter, adapter.pluginID == Initializator.HIERARCHY_ID ? "HierarchySettings_Temp.asset" : "ProjectSettings_Temp.asset",
                                out wasCreated, false);
        return cache;
    }
    
    
    
}
