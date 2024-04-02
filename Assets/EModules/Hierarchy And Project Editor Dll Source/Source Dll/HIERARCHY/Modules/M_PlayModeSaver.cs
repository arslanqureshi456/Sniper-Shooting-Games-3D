#if UNITY_EDITOR
//#define LOG
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.SceneManagement;



//namespace EModules

namespace EModules.EModulesInternal {
internal partial class Hierarchy {





    /*
    */
    
    
    
    
    
    
    
    
    
    /* static Dictionary<Type, FieldInfo[]> SCAN_FIELDS_CACHE = new Dictionary<Type, FieldInfo[]>();
     static FieldInfo[] GET_FIELDS(Type type)
     {
         if (SCAN_FIELDS_CACHE.ContainsKey(type)) return SCAN_FIELDS_CACHE[type];
    
    
         var result = type.GetFields(flags).Where(f => {
             if (f.FieldType != unityObjectType) return false;
             if (f.IsPublic) return true;
             var atr =
                 f.GetCustomAttributes(
                     typeof(SerializeField), false);
             if (atr != null && atr.Length != 0) return true;
             return false;
         }).ToArray();
    
         SCAN_FIELDS_CACHE.Add(type, result);
    
         return result;
    
     }
    */
    
    
    
    
    
    
    internal partial class M_PlayModeKeeper : Adapter.Module, IModuleOnnector_M_PlayModeKeeper {
        internal override int STATIC_WIDTH()
        {   return WIDTH;
        }
        
        
        
        
        static KeeperDataUnityJsonData ToJson( UnityEngine.Object obj )
        {   var jsonData = new KeeperDataUnityJsonData();
            jsonData.default_json = EditorJsonUtility.ToJson( obj );
            if ( Hierarchy.HierarchyAdapterInstance.par.DataKeeperParams.SAVE_UNITYOBJECT )
            {   var f = Adapter.GET_FIELDS_AND_VALUES(obj, obj.GetType());
                jsonData.fields_name = new string[f.Length];
                // f.Keys.CopyTo( jsonData.fields_name , 0 );
                /*jsonData.fields_new_value = new long[f.Count];
                for ( int i = 0 ; i < jsonData.fields_name.Length ; i++ ) {
                    var v = ((UnityEngine.Object)f[jsonData.fields_name[i]].GetValue(obj));
                    jsonData.fields_value[i] = v ? v.GetInstanceID() : -1;
                }*/
                
                jsonData.fields_new_value = new KeeperDataFieldValue[f.Length];
                for ( int i = 0 ; i < jsonData.fields_name.Length ; i++ )
                {   //var v = ((UnityEngine.Object)f[jsonData.fields_name[i]].GetValue(obj));
                    jsonData.fields_name[i] = f[i].Key;
                    var v = f[i].Value.Value as UnityEngine.Object;
                    jsonData.fields_new_value[i] = new KeeperDataFieldValue() { FILEID = v ? v.GetInstanceID() : -1 };
                }
            }
            return jsonData;
        }
        
        static void FromJsonOverwrite( KeeperDataUnityJsonData jsonData, UnityEngine.Object obj )      //  if (json.Length != 2) return;
        {   if ( jsonData == null ) return;
            /*  MonoBehaviour.print(obj);
              MonoBehaviour.print( jsonData );
              MonoBehaviour.print( jsonData.default_json);*/
            if ( !string.IsNullOrEmpty( jsonData.default_json ) )
            {   var fff = Adapter.GET_FIELDS(obj.GetType());
                var f = fff.Values.Select(_f => new {_f, value = _f.GetAllValues(obj, 0, 0) } ).ToArray();
                EditorJsonUtility.FromJsonOverwrite( jsonData.default_json, obj );
                foreach ( var item in f )
                {   item._f.SetAllValues( obj, item.value );
                }
            }
            if ( jsonData.fields_name != null )     //MonoBehaviour.print(obj);
            {   //Debug.Log( "ASD" );
                var fff = Adapter.GET_FIELDS_AND_VALUES(obj, obj.GetType(), searchVals: 4);//.ToDictionary(v=>v.Key, v=>v.Value)
                Dictionary<Adapter.FieldAdapter, Dictionary<string, object>> result = new Dictionary<Adapter.FieldAdapter, Dictionary<string, object>>();
                foreach ( var item in fff )
                {   if ( !result.ContainsKey( item.Value.Key ) ) result.Add( item.Value.Key, new Dictionary<string, object>() );
                    result[item.Value.Key].Add( item.Key, item.Value.Value );
                }
                foreach ( var item in result )
                {   for ( int i = 0 ; i < jsonData.fields_name.Length ; i++ )
                    {
                    
                        if ( !item.Value.ContainsKey( jsonData.fields_name[i] ) /*|| !Adapter.unityObjectType.IsAssignableFrom( f[jsonData.fields_name[i]].FieldType )*/) continue;
                        if ( jsonData.fields_new_value[i].FILEID == -1 ) item.Value[jsonData.fields_name[i]] = null;
                        else
                        {   var newV = M_PlayModeKeeper.GET_ID( (int)jsonData.fields_new_value[i].FILEID, m_OLD_NEW );
                            if ( newV ) item.Value[jsonData.fields_name[i]] = newV;
                        }
                        
                    }
                    item.Key.SetAllValues( obj, item.Value );
                }
                /* var f = Adapter.GET_FIELDS(obj.GetType());
                 for ( int i = 0 ; i < jsonData.fields_name.Length ; i++ ) {
                     if ( !f.ContainsKey( jsonData.fields_name[i] )/ * || !Adapter.unityObjectType.IsAssignableFrom( f[jsonData.fields_name[i]].FieldType )* /) continue;
                     if ( jsonData.fields_value[i] == -1 ) f[jsonData.fields_name[i]].SetValue( obj , (UnityEngine.Object)null );
                     else f[jsonData.fields_name[i]].SetValue( obj , M_PlayModeKeeper.GET_ID( jsonData.fields_value[i] , m_OLD_NEW ) );
                 }*/
            }
            
        }
        
        
        
        
        
        
        internal static ObjectCacheHelper<GameObject, SingleList> DataKeeperCache;
        
        
        public M_PlayModeKeeper( int restWidth, int sib, bool enable, Adapter adapter ) : base( restWidth, sib, enable, adapter )
        {   /*EditorApplication.playmodeStateChanged -= PlaymodeStateChanged;
            EditorApplication.playmodeStateChanged += PlaymodeStateChanged;*/
            DataKeeperCache = new ObjectCacheHelper<GameObject, SingleList>( property => property.GetHash9(), property => property.GetHash10(), Adapter.CacherType.KeeperData, adapter, "DataKeeperCache" );
            
            adapter.PLAYMODECHANGE1 = PlaymodeStateChanged;
            adapter.SubcripePlayModeStateChange();
            
            
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
            
            // Initialize();
            if ( !skipInit )
            {   wasPlayed = false;
                wasLastSaved = false;
            }
            
            
            if ( Application.isPlaying ) skipInit = false;
            
            
            /* EditorPrefs.SetString("Hierarchy Plugin/Data Keeper/Last State", "");
             EditorPrefs.SetString("Hierarchy Plugin/Data Keeper/Current State", "");*/
            
        }
        
        internal override bool enable
        {   get
            {   return true;
            }
            set { }
        }
        
        /*[UnityEditor.Callbacks.DidReloadScripts]
        static void ReloadedScripts()
        {
        
        
            MonoBehaviour.print(Application.isPlaying);
        }*/
        
        
        static bool wasPlayed
        {   get { return EditorPrefs.GetBool( "Hierarchy Plugin/Data Keeper/wasPlayed" ); }
            set { EditorPrefs.SetBool( "Hierarchy Plugin/Data Keeper/wasPlayed", value ); }
        }
        static bool skipInit
        {   get { return EditorPrefs.GetBool( "Hierarchy Plugin/Data Keeper/skipInit" ); }
            set { EditorPrefs.SetBool( "Hierarchy Plugin/Data Keeper/skipInit", value ); }
        }
        static bool wasLastSaved
        {   get { return EditorPrefs.GetBool( "Hierarchy Plugin/Data Keeper/wasLastSaved" ); }
            set     //   MonoBehaviour.print(value);
            {   EditorPrefs.SetBool( "Hierarchy Plugin/Data Keeper/wasLastSaved", value );
            }
        }
        // static bool skipInit = false;
        
        
        
        
        
        const int POS = 2000;
        [MenuItem( "CONTEXT/Component/Remove Component from 'PlayMode Keeper'", true, POS )]
        public static bool STATIC_REMOVE_VALID( MenuCommand menuCommand )
        {
        
            if ( !Hierarchy.HierarchyAdapterInstance.ENABLE_RIGHTDOCK_PROPERTY || !Hierarchy.HierarchyAdapterInstance.par.DataKeeperParams.ENABLE ) return false;
            
            var comp = menuCommand.context as Component;
            if ( !comp ) return false;
            
            /* foreach (var gameObject in SELECTED_GAMEOBJECTS())
             {
                 if (!gameObject.scene.IsValid()) continue;
                 var getted = DataKeeperCache.GetValue(gameObject.scene, gameObject);
                 if (getted != null && getted.list.Count > 0 && getted.list[0] == 1) return true;
             }*/
            
            if ( !comp.gameObject.scene.IsValid() ) return false;
            var getted = DataKeeperCache.GetValue(comp.gameObject.scene, Hierarchy.HierarchyAdapterInstance.GetHierarchyObjectByInstanceID(comp.gameObject));
            if ( getted != null && getted.list.Count > 0 && getted.list.Contains( comp.GetInstanceID() ) ) return true;
            
            
            return false;
        }
        [MenuItem( "CONTEXT/Component/Remove Component from 'PlayMode Keeper'", false, POS )]
        public static void STATIC_REMOVE( MenuCommand menuCommand )
        {
        
            var comp = menuCommand.context as Component;
            if ( !comp ) return;
            
            /* foreach (var gameObject in SELECTED_GAMEOBJECTS())
             {
                 var s = gameObject.scene;
                 if (!s.IsValid()) continue;
                 if (DISABLE_DESCRIPTION(s)) continue;
                 var d = M_Descript.des(s);
                 if (d == null) return;
                 Undo.RecordObject(d.component, "Remove Selected Objects from 'PlayMode Data Keeper'");
                 ((M_PlayModeKeeper)modules.First(m => m is M_PlayModeKeeper)).SetValue(gameObject, false, null);
             }*/
            
            var s = comp.gameObject.scene;
            if ( !s.IsValid() ) return;
            if ( Hierarchy.HierarchyAdapterInstance.DISABLE_DESCRIPTION( s ) ) return;
            var d = M_Descript.des(s.GetHashCode());
            if ( d == null ) return;
            Hierarchy.HierarchyAdapterInstance.SET_UNDO( d, "Remove Selected Component from 'PlayMode Data Keeper'" );
            
            var getted = DataKeeperCache.GetValue(comp.gameObject.scene, Hierarchy.HierarchyAdapterInstance.GetHierarchyObjectByInstanceID(comp.gameObject));
            if ( getted == null ) getted = new SingleList() { list = new List<int>( 1 ) { 0 } };
            getted.list.Remove( comp.GetInstanceID() );
            getted.list.RemoveAt( 0 );
            
            ((M_PlayModeKeeper)Hierarchy.HierarchyAdapterInstance.modules.First( m => m is M_PlayModeKeeper )).SetValue( comp.gameObject, false, getted.list.ToArray() );
            Hierarchy.HierarchyAdapterInstance.RepaintWindowInUpdate();
        }
        [MenuItem( "CONTEXT/Component/Add Component to 'PlayMode Keeper'", true, POS )]
        public static bool STATIC_ADD_VALID( MenuCommand menuCommand )
        {   if ( !Hierarchy.HierarchyAdapterInstance.ENABLE_RIGHTDOCK_PROPERTY || !Hierarchy.HierarchyAdapterInstance.par.DataKeeperParams.ENABLE ) return false;
        
            var comp = menuCommand.context as Component;
            if ( !comp ) return false;
            
            
            /*  foreach (var gameObject in SELECTED_GAMEOBJECTS())
              {
                  if (!gameObject.scene.IsValid()) continue;
                  var getted = DataKeeperCache.GetValue(gameObject.scene, gameObject);
                  if (getted == null || getted.list.Count == 0 || getted.list[0] != 1) return true;
              }*/
            
            if ( !comp.gameObject.scene.IsValid() ) return false;
            var getted = DataKeeperCache.GetValue(comp.gameObject.scene, Hierarchy.HierarchyAdapterInstance.GetHierarchyObjectByInstanceID(comp.gameObject));
            if ( getted == null || getted.list.Count == 0 || !getted.list.Contains( comp.GetInstanceID() ) ) return true;
            
            
            return false;
        }
        [MenuItem( "CONTEXT/Component/Add Component to 'PlayMode Keeper'", false, POS )]
        public static void STATIC_ADD( MenuCommand menuCommand )
        {   var comp = menuCommand.context as Component;
            if ( !comp ) return;
            /* foreach (var gameObject in SELECTED_GAMEOBJECTS())
             {
                 var s = gameObject.scene;
                 if (!s.IsValid()) continue;
                 if (DISABLE_DESCRIPTION(s)) continue;
                 var d = M_Descript.des(s);
                 if (d == null) return;
                 Undo.RecordObject(d.component, "Add Selected Component to 'PlayMode Data Keeper'");
                 ((M_PlayModeKeeper)modules.First(m => m is M_PlayModeKeeper)).SetValue(gameObject, true, null);
             }*/
            
            
            
            var s = comp.gameObject.scene;
            if ( !s.IsValid() ) return;
            if ( Hierarchy.HierarchyAdapterInstance.DISABLE_DESCRIPTION( s ) ) return;
            var d = M_Descript.des(s.GetHashCode());
            if ( d == null ) return;
            Hierarchy.HierarchyAdapterInstance.SET_UNDO( d, "Add Selected Component to 'PlayMode Data Keeper'" );
            
            var getted = DataKeeperCache.GetValue(comp.gameObject.scene, Hierarchy.HierarchyAdapterInstance.GetHierarchyObjectByInstanceID(comp.gameObject));
            if ( getted == null ) getted = new SingleList() { list = new List<int>( 1 ) { 0 } };
            if ( !getted.list.Contains( comp.GetInstanceID() ) ) getted.list.Add( comp.GetInstanceID() );
            getted.list.RemoveAt( 0 );
            
            ((M_PlayModeKeeper)Hierarchy.HierarchyAdapterInstance.modules.First( m => m is M_PlayModeKeeper )).SetValue( comp.gameObject, false, getted.list.ToArray() );
            
            Hierarchy.HierarchyAdapterInstance.RepaintWindowInUpdate();
            
        }
        
        
        static Dictionary<int, int> m_EMPTY = new Dictionary<int, int>();
        static Dictionary<int, int> m_NEW_OLD = new Dictionary<int, int>();
        static Dictionary<int, int> m_OLD_NEW = new Dictionary<int, int>();
        
        /*
        
        [MenuItem( "PluginEditor/KeeperSave")]
        static void ks()
        {   var k =   Initializator.Adapters[Initializator.HIERARCHY_NAME].__modulesOrdered.First(m => m is M_PlayModeKeeper) as  M_PlayModeKeeper;
            k.SaveCurrent();
        }
        
        [MenuItem( "PluginEditor/KeeperLoad")]
        static void kl()
        {   var k =   Initializator.Adapters[Initializator.HIERARCHY_NAME].__modulesOrdered.First(m => m is M_PlayModeKeeper) as  M_PlayModeKeeper;
            k.Load();
        }
        */
        
        
        
        
        private void PlaymodeStateChanged()
        {   if ( !Application.isPlaying ) skipInit = true;
            // MonoBehaviour.print(Application.isPlaying + " " + wasLastSaved);
            if ( !Application.isPlaying )
            {
            
                if ( wasPlayed )     //MonoBehaviour.print("1");
                {
                
                    wasLastSaved = false;
                    wasPlayed = false;
                    Load();
                }
                else       // MonoBehaviour.print("2");
                {   SaveLast();
                    wasLastSaved = true;
                }
            }
            
            if ( Application.isPlaying )     // MonoBehaviour.print("3");
            {   if ( !wasLastSaved ) SaveLast();
                if ( wasPlayed ) SaveCurrent();
                wasLastSaved = true;
                wasPlayed = true;
            }
            
            /* if (!Application.isPlaying)
             {
            
                 if (wasPlayed)
                 {
                     wasLastSaved = false;
                     wasPlayed = false;
                     Load();
                 } else
                 {
                     SaveLast();
                     wasLastSaved = true;
                 }
             }
            
             if (Application.isPlaying)
             {
                 if (!wasLastSaved) SaveLast();
                 if (wasPlayed) SaveCurrent();
                 wasLastSaved = true;
                 wasPlayed = true;
             }*/
            
        }
        
        /*   Default = 0,
        IgnoreCase = 1,
        DeclaredOnly = 2,
        Instance = 4,
        Static = 8,
        Public = 16,
        NonPublic = 32,
        FlattenHierarchy = 64,
        InvokeMethod = 256,
        CreateInstance = 512,*/
        /* BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;
         BindingFlags PropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
        
         class BakedFields
         {
             internal FieldInfo[] fields;
             internal PropertyInfo[] props;
         }
        
         static Dictionary<string, BakedFields> baked_types = new Dictionary<string, BakedFields>();*/
        //  BindingFlags flags = (BindingFlags)(-1);
        void test()     //var t = typeof(Light);
        {   //  MonoBehaviour.print(t + " " + t.GetProperties(PropertyFlags).Where(p => p.CanRead && p.CanWrite).Count());
        }
        
        
        
        static int gameObject_ID;
        static int comp_ID;
        internal static void RECORD_FLUSH( KeeperData source, Component comp )      /* RECORD */
        {   gameObject_ID = comp.gameObject.GetInstanceID();
            comp_ID = comp.GetInstanceID();
            if ( !source.comp_to_Type.ContainsKey( comp_ID ) )     //  MonoBehaviour.print(comp.GetType().FullName);
            {   source.comp_to_Type.Add( comp_ID, comp.GetType().FullName );
            }
            
            if ( !source.field_records.ContainsKey( gameObject_ID ) ) source.field_records.Add( gameObject_ID, new KeeperDataItem(
                            comp.gameObject.transform.parent ? comp.gameObject.transform.parent.gameObject.GetInstanceID() : -1,
                            comp.gameObject.transform.GetSiblingIndex(),
                            comp.gameObject.name,
                            comp.gameObject.activeSelf
                ) { records = new Dictionary<long, KeeperDataUnityJsonData>() } );
            if ( !source.field_records[gameObject_ID].records.ContainsKey( comp_ID ) )     //    if (comp is PrefabLibrary)   MonoBehaviour.print(((PrefabLibrary)comp).Explosiont_Prefab);
            {   source.field_records[gameObject_ID].records.Add( comp_ID, ToJson( comp ) );
                /* source.prop_records.Add(id, new Dictionary<PropertyInfo, object>());
                 //   MonoBehaviour.print(comp.GetType() + " " + comp.GetType().GetFields(FieldFlags).Length);
                
                 var type = comp.GetType();
                 if (!baked_types.ContainsKey(type.Name)) baked_types.Add(type.Name,
                      new BakedFields() {
                          fields = type.GetFields(FieldFlags).Where(field => field.IsPublic || field.GetCustomAttributes(false).Length != 0).ToArray(),
                          props = type.GetProperties(PropertyFlags).Where(p => p.CanRead && p.CanWrite).ToArray(),
                      });
                 var baked = baked_types[type.Name];
                 foreach (var field in baked.fields) source.field_records[id].Add(field, field.GetValue(comp));
                 foreach (var prop in baked.props) source.prop_records[id].Add(prop, prop.GetValue(comp, null));*/
            }
            /* RECORD */
            
            
            /*
                            Editor.
                             editor = Editor.CreateEditor(activeGO.transform);
            
                            SerializedObject so = new SerializedObject(Selection.activeGameObject.GetComponent<Renderer>());
            
                            so.FindProperty("m_ScaleInLightmap").cop
                            so.ApplyModifiedProperties();
                            new SerializedObject()
                            so.*/
        }
        
        
        
        
        string flush_current()
        {   if ( !adapter.par.DataKeeperParams.ENABLE ) return "";
        
            var result = new KeeperData();
            if ( adapter.par.DataKeeperParams.USE_SCRIPTS )
            {   foreach ( var mDataKeeperValue in Hierarchy_GUI.Instance( adapter ).m_DataKeeper_Values )
                {   if ( !mDataKeeperValue.value || mDataKeeperValue.value.GetClass() == null ) continue;
                    foreach ( var finded in Resources.FindObjectsOfTypeAll( mDataKeeperValue.value.GetClass() ) )
                    {   var comp = finded as Component;
                        if ( !comp || !comp.gameObject || !comp.gameObject.scene.IsValid() ) continue;
                        /* RECORD */
                        RECORD_FLUSH( result, comp );
                        /* if (!result.records.ContainsKey(comp.GetInstanceID()))
                         {
                             var id = comp.GetInstanceID();
                             result.records.Add(id, new Dictionary<FieldInfo, object>());
                             foreach (var field in comp.GetType().GetFields(flags))
                             {
                                 if (field.IsPublic || field.GetCustomAttributes(false).Length != 0)
                                 {
                                     result.records[id].Add(field, field.GetValue(comp));
                                 }
                             }
                        
                         }*/
                        /* RECORD */
                    }
                }
            }
            foreach ( var cache in DataKeeperCache.cacheDic )
            {   var s = cache.Key;
                List<Adapter.HierarchyObject> removeList = new List<Adapter.HierarchyObject>();
                foreach ( var obj in cache.Value )
                {   var o = Adapter.GET_OBJECT(obj.Key.id) as GameObject;
                    if ( !o || !o.scene.IsValid() ) continue;
                    // MonoBehaviour.print("ASD");
                    var getted = DataKeeperCache.GetValue(s, Hierarchy.HierarchyAdapterInstance.GetHierarchyObjectByInstanceID(o));
                    if ( getted == null || getted.list.Count == 0 ) continue;
                    bool wasWRite = false;
                    if ( getted.list[0] == 1 )
                    {   foreach ( var comp in o.GetComponents<Component>() )
                        {   if ( !comp ) continue;
                            RECORD_FLUSH( result, comp );
                            wasWRite = true;
                        }
                        if ( wasWRite ) result.field_records[obj.Key.id].ALL = true;
                        else removeList.Add( obj.Key );
                        // result.ALL = true;
                    }
                    else
                    {   for ( int i = 1 ; i < getted.list.Count ; i++ )
                        {   var comp = Adapter.GET_OBJECT(getted.list[i]) as Component;
                            if ( !comp ) continue;
                            RECORD_FLUSH( result, comp );
                            wasWRite = true;
                        }
                        if ( wasWRite ) result.field_records[obj.Key.id].ALL = false;
                        else removeList.Add( obj.Key );
                        // result.ALL = false;
                    }
                }
                if ( removeList.Count != 0 )
                    foreach ( var i in removeList )
                    {   cache.Value.Remove( i );
                    }
            }
            return Adapter.SERIALIZE_SINGLE( result );
        }
        
        void SaveLast()
        {
            #if LOG
            MonoBehaviour.print("SaveLast");
            #endif
            /*  MonoBehaviour.print("Save");*/
            var flush = flush_current();
            /*if (editMode)
            {
                editMode = false;*/
            EditorPrefs.SetString( "Hierarchy Plugin/Data Keeper/Last State", flush );
            //  }
            // EditorPrefs.SetString("Hierarchy Plugin/Data Keeper/Current State", flush);
        }
        void SaveCurrent()
        {
            #if LOG
            MonoBehaviour.print("SaveCurrent");
            #endif
            /*  MonoBehaviour.print("Save");*/
            var flush = flush_current();
            /*   if (editMode)
               {
                   editMode = false;
                   EditorPrefs.SetString("Hierarchy Plugin/Data Keeper/Last State", flush);
               }*/
            EditorPrefs.SetString( "Hierarchy Plugin/Data Keeper/Current State", flush );
        }
        List<Component> writeChanges = new List<Component>();
        int OB_ID = 0;
        int COMPID = 0;
        
        /*  static UnityEngine.Object GET_OBJECT_BYID(int id)
          {
              if (m_NEW_OLD.ContainsKey(id)) return EditorUtility.Instance IDToObject(m_NEW_OLD[id]);
              return EditorUtility.Instance IDToObject(id);
          }*/
        
        
        void Load()
        {
            #if LOG
            MonoBehaviour.print("Load");
            #endif
            var last = EditorPrefs.GetString("Hierarchy Plugin/Data Keeper/Last State", "");
            var current = EditorPrefs.GetString("Hierarchy Plugin/Data Keeper/Current State", "");
            EditorPrefs.SetString( "Hierarchy Plugin/Data Keeper/Current State", "" );
            
            if ( !string.IsNullOrEmpty( last ) )
            {
                #if !UNITY_EDITOR
                try
                #endif
                { last_state = Adapter.DESERIALIZE_SINGLE<KeeperData>( last ); }
                #if !UNITY_EDITOR
                catch { last_state = null; }
                #endif
                // last_state = (KeeperData)DESERIALIZE_SINGLE(last, formatter);
                /*                    try { last_state = (KeeperData)DESERIALIZE_SINGLE(last, formatter); }
                                    catch { last_state = new KeeperData(); }*/
            }
            
            if ( string.IsNullOrEmpty( current ) ) return;
            
            #if !UNITY_EDITOR
            try
            {
            #endif
                current_state = Adapter.DESERIALIZE_SINGLE<KeeperData>( current );
                #if !UNITY_EDITOR
            }
            catch { current_state = null; }
                #endif
            m_NEW_OLD.Clear();
            m_OLD_NEW.Clear();
            
            if ( current_state != null )
            {   List<Scene> scenes = new List<Scene>();
                //  var use_add_remove = ;
                
                
                
                Dictionary<GameObject, KeeperDataItem> hierarchychanges = new Dictionary<GameObject, KeeperDataItem>();
                
                //CREATE GAMEOBJECT
                TRY_CREATE_OBJECTS( true, current_state, "Perform PlayMode Data Keeper", ref hierarchychanges );
                //CREATE GAMEOBJECT
                
                
                foreach ( var obValue in current_state.field_records )
                {   var o = GET_ID(obValue.Key, m_OLD_NEW) as GameObject;
                    var changes = false;
                    changes = DO_CHANGES( o, obValue.Key, adapter.par.DataKeeperParams.USE_ADD_REMOVE, current_state, last_state, "Perform PlayMode Data Keeper", ref hierarchychanges, false );
                    if ( changes && !scenes.Contains( o.scene ) ) scenes.Add( o.scene );
                }
                foreach ( var obValue in current_state.field_records )
                {   var o = GET_ID(obValue.Key, m_OLD_NEW) as GameObject;
                    var changes = false;
                    changes = DO_CHANGES( o, obValue.Key, adapter.par.DataKeeperParams.USE_ADD_REMOVE, current_state, last_state, "Perform PlayMode Data Keeper", ref hierarchychanges, true );
                    if ( changes && !scenes.Contains( o.scene ) ) scenes.Add( o.scene );
                }
                
                CHACNGE_HIERARCHY( hierarchychanges, "Perform PlayMode Data Keeper", true );
                
                
                
                
                current_state_back = current_state;
                current_state = null;
                
                foreach ( var scene in scenes )
                {   adapter.MarkSceneDirty( scene );
                }
                writeChanges.Clear();
            }
            
            
            
        }
        
        KeeperData current_state_back;
        int[] keysArray = new int[20];
        int inter;
        KeeperDataUnityJsonData kpLast;
        /* List<int> removearray = new List<int>();
         List<int> addarray = new List<int>();*/
        
        
        static UnityEngine.Object GET_ID( long _id, Dictionary<int, int> id_translater )
        {   var id = (int)_id;
            if ( id_translater.ContainsKey( id ) ) return Adapter.GET_OBJECT( id_translater[id] );
            return Adapter.GET_OBJECT( id );
        }
        
        static Dictionary<GameObject, long> createdObject = new Dictionary<GameObject, long>();
        static bool wasCreated;
        
        void TRY_CREATE_OBJECTS( bool WRITE_OLD_NEW, KeeperData source, string UNDO_TEXT, ref Dictionary<GameObject, KeeperDataItem> hierarchychanges )
        {   wasCreated = false;
            foreach ( var obValue in source.field_records )
            {   var o = Adapter.GET_OBJECT(obValue.Key) as GameObject;
            
                if ( !o )
                {   if ( !wasCreated ) createdObject.Clear();
                    wasCreated = true;
                    var ob = new GameObject();
                    ob.name = obValue.Value.GameObject_Name;
                    ob.SetActive( obValue.Value.GameObject_Active );
                    createdObject.Add( ob, obValue.Key );
                    
                    // Debug.Log(ob.name + " " + obValue.Value.GameObject_Active + " " +  obValue.Value.GameObject_SiblingPos);
                    
                    if ( WRITE_OLD_NEW )
                    {   m_NEW_OLD.Add( ob.GetInstanceID(), (int)obValue.Key );
                        m_OLD_NEW.Add( (int)obValue.Key, ob.GetInstanceID() );
                        
                        
                        foreach ( var t in source.comp_to_Type )
                        {   if ( t.Value == "UnityEngine.Transform" )
                            {   var trID = ob.GetComponent<Transform>().GetInstanceID();
                            
                                m_NEW_OLD.Add( trID, (int)t.Key );
                                m_OLD_NEW.Add( (int)t.Key, trID );
                                //Debug.Log("ASD");
                                break;
                            }
                        }
                        
                        
                    }
                    
                    
                    // UnityEngine.Object.DestroyImmediate(ob.GetComponent<Transform>());
                    /*
                    source.field_records[]*/
                    // current_state.field_records[obValue.Key]. = ob.GetInstanceID();
                }
            }
            
            if ( wasCreated )
            {   foreach ( var o in createdObject )
                {   var parent = GET_ID(source.field_records[o.Value].GameObject_ParentID, m_OLD_NEW);
                    Undo.SetTransformParent( o.Key.transform, parent ? ((GameObject)parent).transform : null, UNDO_TEXT );
                    Undo.RegisterCreatedObjectUndo( o.Key, UNDO_TEXT );
                    hierarchychanges.Add( o.Key, source.field_records[o.Value] );
                }
                
                /*  var sorted = createdObject.Select(o => new { o, source.field_records[o.Value].GameObject_SiblingPos }).OrderBy(s => s.GameObject_SiblingPos).ToArray();
                  foreach (var o in sorted)
                  {
                      Undo.RegisterFullObjectHierarchyUndo(o.o.Key, UNDO_TEXT);
                      o.o.Key.transform.SetAsLastSibling();
                  }
                
                
                  foreach (var o in sorted)
                  {
                      o.o.Key.transform.SetSiblingIndex(o.GameObject_SiblingPos);
                      // Hierarchy.SetDirty(o.o);
                      Undo.RegisterCreatedObjectUndo(o.o.Key, UNDO_TEXT);
                  }*/
            }
        }
        
        
        void CHACNGE_HIERARCHY( Dictionary<GameObject, KeeperDataItem> hierarchychanges, string UNDO_TEXT, bool use_new )
        {   if ( hierarchychanges.Count == 0 ) return;
        
        
            foreach ( var o in hierarchychanges )
            {   var parent = use_new ? GET_ID(o.Value.GameObject_ParentID, m_OLD_NEW) : GET_ID(o.Value.GameObject_ParentID, m_EMPTY);
                Undo.SetTransformParent( o.Key.transform, parent ? ((GameObject)parent).transform : null, UNDO_TEXT );
            }
            
            var sorted = hierarchychanges.Select(o => new { o.Key, o.Value.GameObject_SiblingPos }).OrderBy(s => s.GameObject_SiblingPos).ToArray();
            foreach ( var o in sorted )
            {   Undo.RegisterFullObjectHierarchyUndo( o.Key, UNDO_TEXT );
                o.Key.transform.SetAsLastSibling();
            }
            
            
            foreach ( var o in sorted )
            {   o.Key.transform.SetSiblingIndex( o.GameObject_SiblingPos );
                Adapter.SetDirty( o.Key );
                // Undo.RegisterCreatedObjectUndo(o.o.Key, UNDO_TEXT);
            }
            
        }
        
        
        
        private bool DO_CHANGES(
            GameObject o, long recordId, bool use_add_remove,
            KeeperData source,
            KeeperData last,
            string UNDO_TEXT, ref Dictionary<GameObject, KeeperDataItem> hierarchychanges, bool APPPY_JSON )
        {
        
            if ( !o || !o.scene.IsValid() || !o.scene.isLoaded )
            {   return false;
            }
            
            /* if (o.name == "Directional Light")
                 Debug.Log(o.name);*/
            //OB_ID = GET_ID(o.GetInstanceID(), source_translator).;
            /* if (m_NEW_OLD.ContainsKey(id)) OB_ID = (m_NEW_OLD[id]);
             else OB_ID = (o.GetInstanceID());*/
            OB_ID = (int)recordId;
            if ( last == null ) last = new KeeperData() { comp_to_Type = new Dictionary<long, string>(), field_records = new Dictionary<long, KeeperDataItem>() };
            
            if ( !source.field_records.ContainsKey( OB_ID ) )
            {   adapter.logProxy.LogWarning( "[Hierarchy Plugin] Error load '" + o.name + "' keeper state" );
                return false;
            }
            
            // OB_ID = GET_ID(o.GetInstanceID(), source_translator).GetInstanceID();
            var LAST_HAVE_OBJECT = last.field_records.ContainsKey(OB_ID);
            var haveChanges = false;
            
            
            
            /*  if (createdObject.Count != 0)
              {
                  foreach (var o in createdObject)
                  {
                      var parent = EditorUtility.Instance IDToObject(current_state.field_records[m_NEW_OLD[o.GetInstanceID()]].GameObject_ParentID) as Transform;
                      Undo.SetTransformParent(o.transform, parent, UNDO_TEXT);
                  }
            
                  var sorted = createdObject.Select(o => new { o, current_state.field_records[id].GameObject_SiblingPos }).OrderBy(s => s.GameObject_SiblingPos).ToArray();
                  foreach (var o in sorted)
                  {
                      Undo.RegisterFullObjectHierarchyUndo(o.o, UNDO_TEXT);
                      o.o.transform.SetAsLastSibling();
                  }
            
            
                  foreach (var o in sorted)
                  {
                      o.o.transform.SetSiblingIndex(o.GameObject_SiblingPos);
                      Hierarchy.SetDirty(o.o);
                  }
              }*/
            
            
            
            if ( !APPPY_JSON )
            {   var s = source.field_records[OB_ID];
                var l = LAST_HAVE_OBJECT ? last.field_records[OB_ID] : null;
                
                //SET ACTIVE
                if ( adapter.par.DataKeeperParams.SAVE_ENABLINGDISABLING_GAMEOBJEST && s.ALL )
                {   if ( !LAST_HAVE_OBJECT || s.GameObject_Active != l.GameObject_Active )
                    {   Undo.RecordObject( o, UNDO_TEXT );
                        o.SetActive( s.GameObject_Active );
                        EditorUtility.SetDirty( o );
                        haveChanges = true;
                    }
                }
                
                //PARENT
                if ( adapter.par.DataKeeperParams.SAVE_GAMEOBJET_HIERARCHY && s.ALL )
                {   if ( !LAST_HAVE_OBJECT || s.GameObject_ParentID != l.GameObject_ParentID ||
                            s.GameObject_SiblingPos != l.GameObject_SiblingPos )
                    {   hierarchychanges.Add( o, s );
                        haveChanges = true;
                    }
                    
                }
            }
            
            
            
            
            
            
            
            
            if ( source.field_records[OB_ID].records.Count > keysArray.Length ) Array.Resize( ref keysArray, source.field_records[OB_ID].records.Count );
            inter = 0;
            foreach ( var kp in source.field_records[OB_ID].records )
            {   keysArray[inter++] = (int)kp.Key;
                //Debug.Log(kp.Key);
            }
            //  Dictionary<string, Type> ass = null;
            for ( int i = 0 ; i < inter ; i++ )
            {   COMPID = keysArray[i];
                if ( m_OLD_NEW.ContainsKey( COMPID ) ) COMPID = m_OLD_NEW[COMPID];
                /*  }
                
                  foreach (var record in source.field_records[OB_ID].records)
                  {*/
                var comp = Adapter.GET_OBJECT(COMPID) as Component;
                var LAST_HAVE_COMP = LAST_HAVE_OBJECT && last.field_records[OB_ID].records.ContainsKey(COMPID);
                
                if ( !APPPY_JSON && use_add_remove )
                {   if ( !comp && !LAST_HAVE_COMP )     // System.AppDomain.CurrentDomain.GetAssemblies(
                    {   /* if (ass == null)
                         {
                        
                             ; fghj
                             /*  foreach (var assembly in )
                               {
                                   //if (assembly.GetTypes().Any(at=>at.FullName == source.comp_to_Type[COMPID]))
                                   Debug.Log(assembly.FullName);
                               }#1#
                             // ass = System.AppDomain.CurrentDomain.GetAssemblies().Where(a=>a.SelectMany(a => a.GetTypes()).ToDictionary(asd => FullName);
                             /*   foreach (var type in ass) {
                                    MonoBehaviour.print(type.FullName);
                                }#1#
                         }*/
                        // var t = ass.First(a => a.FullName == source.comp_to_Type[COMPID]);
                        
                        var getted_t = Adapter.GET_TYPE_BY_STRING(source.comp_to_Type[COMPID]);
                        
                        if ( getted_t != null )     //  MonoBehaviour.print(t);
                        {   // bool needRegistrate = false;
                            comp = o.AddComponent( getted_t );
                            // comp = o.AddComponent(Type.GetType(source.comp_to_Type[COMPID]));
                            var kp = source.field_records[OB_ID].records[COMPID];
                            source.field_records[OB_ID].records.Remove( COMPID );
                            if ( last.field_records.ContainsKey( OB_ID ) && last.field_records[OB_ID].records.ContainsKey( COMPID ) )
                            {   kpLast = last.field_records[OB_ID].records[COMPID];
                                last.field_records[OB_ID].records.Remove( COMPID );
                            }
                            else kpLast = null;
                            
                            m_NEW_OLD.Add( comp.GetInstanceID(), COMPID );
                            m_OLD_NEW.Add( COMPID, comp.GetInstanceID() );
                            
                            COMPID = comp.GetInstanceID();
                            source.field_records[OB_ID].records.Add( COMPID, kp );
                            source.comp_to_Type.Add( COMPID, comp.GetType().FullName );
                            if ( kpLast != null )
                            {   last.field_records[OB_ID].records.Add( COMPID, kpLast );
                                last.comp_to_Type.Add( COMPID, comp.GetType().FullName );
                            }
                            Undo.RegisterCreatedObjectUndo( comp, UNDO_TEXT );
                        }
                        else
                        {   adapter.logProxy.LogWarning( "[Hierarchy Plugin] Reference save error '" + source.comp_to_Type[COMPID] + "'" );
                        }
                        
                        /*  EditorUtility.SetDirty(comp);
                          EditorUtility.SetDirty(comp.gameObject);*/
                        
                        /*  Undo.RecordObject(comp, UNDO_TEXT);
                          EditorJsonUtility.FromJsonOverwrite(kp, comp);
                          EditorUtility.SetDirty(comp);
                          EditorUtility.SetDirty(comp.gameObject);
                          haveChanges = true;*/
                        
                    }
                    
                }
                
                if ( APPPY_JSON )
                {   if ( !comp )
                    {   continue;
                    }
                    if ( source.field_records[OB_ID].records.ContainsKey( COMPID ) )     //    var id = comp.GetInstanceID();
                    {   if ( source.field_records[OB_ID].records[COMPID] != ToJson( comp ) )     // MonoBehaviour.print("ASD");
                        {   Undo.RecordObject( comp, UNDO_TEXT );
                            FromJsonOverwrite( source.field_records[OB_ID].records[COMPID], comp );
                            EditorUtility.SetDirty( comp );
                            EditorUtility.SetDirty( comp.gameObject );
                            haveChanges = true;
                        }
                    }
                }
                
            }
            
            /*if (removearray.Count != 0) {
                for (int i = 0; i < removearray.Count; i++) {
                    source.field_records[OB_ID].records.Remove(i);
            
                }
            }*/
            
            
            if ( !APPPY_JSON && use_add_remove )
            {   if ( LAST_HAVE_OBJECT )     //  MonoBehaviour.print("ASD");
                {   foreach ( var lastcompID in last.field_records[OB_ID].records )
                    {   if ( !source.field_records[OB_ID].records.ContainsKey( lastcompID.Key ) )
                        {   var forDestroy = Adapter.GET_OBJECT(lastcompID.Key) as Component;
                            // MonoBehaviour.print("1 " + forDestroy);
                            if ( forDestroy ) Undo.DestroyObjectImmediate( forDestroy );
                        }
                    }
                    /*   foreach (var lastcompID in source.field_records[OB_ID].records)
                       {
                           if (!last.field_records[OB_ID].records.ContainsKey(lastcompID.Key))
                           {
                               var forDestroy = Adapter.GET_OBJECT(lastcompID.Key) as Component;
                               MonoBehaviour.print("2 " + forDestroy);
                               if (forDestroy) Undo.DestroyObjectImmediate(forDestroy);
                           }
                       }*/
                }
                else if ( source.field_records[OB_ID].ALL )       //  MonoBehaviour.print("2");
                {   var comps = o.GetComponents<Component>();
                    for ( int i = comps.Length - 1 ; i >= 0 ; i-- )
                    {   if ( !comps[i] ) continue;
                        if ( !source.field_records[OB_ID].records.ContainsKey( comps[i].GetInstanceID() ) )
                        {   Undo.DestroyObjectImmediate( comps[i] );
                        }
                    }
                }
                
            }
            return haveChanges;
            
        }
        
        
        
        
        bool LAST_VALIDATE_UNDO( GameObject o, Component[] comps )
        {   if ( last_state == null || current_state_back == null || Application.isPlaying ) return false;
        
            if ( !o || !o.scene.IsValid() || m_NEW_OLD.ContainsKey( o.GetInstanceID() ) ) return false;
            
            var ob_id = o.GetInstanceID();
            
            var LAST_HAVE_OBJECT = last_state.field_records.ContainsKey(ob_id);
            
            
            ////#tag TODO  Now I added disable cancellation for the created objects
            if ( !last_state.field_records.ContainsKey( o.GetInstanceID() ) ) return false;
            
            
            
            var currentids = comps.Select(c => (long)c.GetInstanceID()).ToArray();
            
            if ( adapter.par.DataKeeperParams.USE_ADD_REMOVE )
            {   if ( LAST_HAVE_OBJECT )
                {   foreach ( var lastcompID in last_state.field_records[ob_id].records )
                    {   if ( !currentids.Contains( lastcompID.Key ) )
                        {   return true;
                        }
                    }
                    if ( last_state.field_records[ob_id].ALL )
                        foreach ( var lastcompID in currentids )
                        {   if ( !last_state.field_records[ob_id].records.ContainsKey( lastcompID ) )
                            {   return true;
                            }
                        }
                        
                        
                }
                
                if ( current_state_back.field_records.ContainsKey( ob_id ) && current_state_back.field_records[ob_id].ALL )
                {   for ( int i = comps.Length - 1 ; i >= 0 ; i-- )
                    {   if ( !comps[i] ) continue;
                        if ( !current_state_back.field_records[ob_id].records.ContainsKey( comps[i].GetInstanceID() ) )
                        {   return true;
                        }
                    }
                }
                
            }
            
            
            if ( LAST_HAVE_OBJECT )
            {
            
                if ( current_state_back.field_records.ContainsKey( ob_id ) )
                {   var l = last_state.field_records[ob_id];
                    // var c = current_state_back.field_records[ob_id];
                    /*                        var pp =   l.GameObject_ParentID != c.GameObject_ParentID || l.GameObject_SiblingPos != c.GameObject_SiblingPos;
                                            var aa = l.GameObject_Active != c.GameObject_Active;
                                            if (l.GameObject_Name != c.GameObject_Name ||*/
                    var pp = l.GameObject_ParentID != (o.transform.parent ? o.transform.parent.gameObject.GetInstanceID() : -1) || l.GameObject_SiblingPos != o.transform.GetSiblingIndex();
                    var aa = l.GameObject_Active != o.activeSelf;
                    if ( l.GameObject_Name != o.name ||
                            pp && adapter.par.DataKeeperParams.SAVE_GAMEOBJET_HIERARCHY && l.ALL ||
                            aa && adapter.par.DataKeeperParams.SAVE_ENABLINGDISABLING_GAMEOBJEST && l.ALL
                       ) return true;
                }
            }
            
            /*  if (par.DataKeeperParams.USE_ADD_REMOVE)
              {
                  if (LAST && !comp || comp && !LAST) return true;
              }
            */
            
            foreach ( var comp in comps )
            {
            
            
                var LAST = last_state.field_records[ob_id].records.ContainsKey(comp.GetInstanceID());
                // var CURRENT = current_state.field_records[ob_id].ContainsKey(comp.GetInstanceID());
                
                if ( adapter.par.DataKeeperParams.USE_ADD_REMOVE )
                {   if ( LAST && !comp ) return true;
                }
                
                if ( !comp ) continue;
                if ( last_state.field_records[o.GetInstanceID()].records.ContainsKey( comp.GetInstanceID() ) )
                {   if ( last_state.field_records[o.GetInstanceID()].records[comp.GetInstanceID()] != ToJson( comp ) )
                    {   return true;
                    }
                }
            }
            return false;
        }
        
        
        
        void LAST_DO_UNDO( GameObject o, Component[] comps )
        {   if ( last_state == null ) return;
        
            Dictionary<GameObject, KeeperDataItem> hierarchychanges = new Dictionary<GameObject, KeeperDataItem>();
            
            ////#tag TODO  Now I added disable cancellation for the created objects
            // Now I added disable undo for the created objects so just o.GetInstanceID ()
            var changes = false;
            changes = DO_CHANGES( o, o.GetInstanceID(), true, last_state, current_state_back, "Revert Last PlayMode Changes", ref hierarchychanges, false );
            changes |= DO_CHANGES( o, o.GetInstanceID(), true, last_state, current_state_back, "Revert Last PlayMode Changes", ref hierarchychanges, true );
            if ( changes ) adapter.MarkSceneDirty( o.scene );
            /*  if (DO_CHANGES(o, o.GetInstanceID(), true, last_state, current_state_back, "Revert Last PlayMode Changes", ref hierarchychanges))
              {
            
            
            
                  EditorSceneManager.MarkSceneDirty(o.scene);
              }*/
            
            CHACNGE_HIERARCHY( hierarchychanges, "Revert Last PlayMode Changes", true );
            
            /* OB_ID = o.GetInstanceID();
             foreach (var comp in comps)
             {
                 if (!comp || !o || !o.scene.IsValid() || !last_state.field_records.ContainsKey(OB_ID)) continue;
                 COMPID = comp.GetInstanceID();
                 //  if (!comp || !comp.gameObject || !comp.gameObject.scene.IsValid()) continue;
                 if (last_state.field_records[OB_ID].ContainsKey(COMPID)))
                 {
            
                     if (par.DataKeeperParams.USE_ADD_REMOVE)
                     {
            
                     } else
                     {
            
                     }
            
                     var id = comp.GetInstanceID();
                     if (last_state.field_records[id] != EditorJsonUtility.ToJson(comp))
                     {
                         Undo.RecordObject(comp, "Revert Last PlayMode Changes");
                         EditorJsonUtility.FromJsonOverwrite(last_state.field_records[id], comp);
                         EditorUtility.SetDirty(comp);
                         EditorUtility.SetDirty(comp.gameObject);
                         if (!scenes.Contains(comp.gameObject.scene)) scenes.Add(comp.gameObject.scene);
                     }
            
                 }
             }*/
            
            writeChanges.Clear();
        }
        
        
        
        //  static bool editMode = true;
        private void Update()
        {
        
            /* if (!Application.isPlaying)
             {
                 if (applyOnUpdate != null)
                 {
                     applyOnUpdate();
                     applyOnUpdate = null;
                 }
            
                 editMode = true;
            
            
             }*/
            
        }
        
        
        
        
        KeeperData last_state, current_state;
        
        
        
        internal override bool SKIP()
        {   return adapter.DISABLE_DES() || !Hierarchy.HierarchyAdapterInstance.par.DataKeeperParams.ENABLE;
        }
        
        
        static int WIDTH = 22;
        
        
        GUIContent PLAY_CONT_STORE = new GUIContent() { tooltip = "Left CLICK - choose ALL persistent components\n( Ctrl+Left CLICK - include children )" };
        // GUIContent EDIT_CONT_STORE = new GUIContent() { text = "", tooltip = "" };
        GUIContent PLAY_CONT_LINES = new GUIContent() { tooltip = "Left CLICK - choose persistent component\n( Ctrl+Left CLICK - include children )" };
        // GUIContent EDIT_CONT_LINES = new GUIContent() { text = "", tooltip = "" };
        Texture2D compstexture, storagetexture;
        //SingleList currentList = null;
        
        Rect borderR = new Rect(), labelrect = new Rect(), leftR = new Rect(), rightR = new Rect();
        Color alpha = new Color(1, 1, 1, 0.4f);
        internal override float Draw( Rect _drawRect, Adapter.HierarchyObject _o )
        {   // GUI.BeginClip( _drawRect );
            // _drawRect.y = _drawRect.x = 0;
            var res = ___Draw( _drawRect, _o);
            // GUI.EndClip();
            return
                res;
        }
        
        
        
        SingleList GET_CURRENT_LIST(Adapter.HierarchyObject _o, ref bool contains )
        {   var currentList = DataKeeperCache.GetValue( _o.scene, _o );
            if ( currentList  == null )
            {   contains = false;
                return null;
            }
            if ( currentList.list == null || currentList.list.Count == 0 ) currentList.list = new List<int>( 1 ) { 0 };
            
            if ( currentList.list[0] == 1 )
            {   compstexture = adapter.GetIcon( "STORAGE_ALLCOMPS" );
                storagetexture = adapter.GetIcon( "STORAGE_ACTIVE" );
            }
            else
            {   for ( int i = currentList.list.Count - 1 ; i > 0 ; i-- )
                {
                
                    if ( !Adapter.GET_OBJECT( currentList.list[i] ) )     // MonoBehaviour.print("ASD");
                    {   currentList.list.RemoveAt( i );
                    }
                }
                
                if ( currentList.list.Count > 1 )
                {   compstexture = adapter.GetIcon( "STORAGE_ONECOMP" );
                    storagetexture = adapter.GetIcon( "STORAGE_ACTIVE" );
                }
                else
                {   contains = false;
                    return null;
                }
            }
            return currentList;
        }
        
        
        float ___Draw( Rect _drawRect, Adapter.HierarchyObject _o )
        {
        
            var o = _o.go;
            
            
            if ( SKIP() || !o ) return 0;
            
            if ( Hierarchy.HierarchyAdapterInstance.DISABLE_DESCRIPTION( _o ) )
            {   /*  var fs = Adapter.GET_SKIN().label.fontSize;
                 var al = Adapter.GET_SKIN().label.alignment;
                 Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleRight;
                if (  ) Adapter.GET_SKIN().label.fontSize = adapter.FONT_8();
                 else Adapter.GET_SKIN().label.fontSize = adapter.WINDOW_FONT_8();*/
                
                GUI.Label( _drawRect, Adapter.CacheDisableConten, !callFromExternal() ? adapter.STYLE_LABEL_8_right : adapter.STYLE_LABEL_8_WINDOWS_right );
                
                /*Adapter.GET_SKIN().label.alignment = al;
                Adapter.GET_SKIN().label.fontSize = fs;*/
                return width;
            }
            
            
            
            if ( !START_DRAW( _drawRect, _o ) ) return 0;
            
            
            
            // var FI = 20;
            var SI = 12;
            
            borderR = _drawRect;
            borderR.x += borderR.width / 2 - SI;
            borderR.y += (borderR.height - SI) / 2;
            borderR.width = SI * 2;
            //drawRect.width = drawRect.height = FI;
            
            // var id = o.GetInstanceID();
            
            var contains = DataKeeperCache.HasKey(_o.scene, _o);
            
            var auto = adapter.par.DataKeeperParams.USE_SCRIPTS && Hierarchy_GUI.Instance(adapter).DataKeeper_IsObjectIncluded(o);
            /*   if (auto)
                       MonoBehaviour.print(o);*/
            //SingleList currentList = null;
            
            
            if ( contains )     //MonoBehaviour.print("ASD");
            {   /* currentList = */GET_CURRENT_LIST( _o, ref contains );
            }
            if ( !contains )
            {   //currentList = null;
                compstexture = adapter.GetIcon( "STORAGE_NOCOMP" );
                storagetexture = adapter.GetIcon( "STORAGE_PASSIVE" );
            }
            
            
            leftR = rightR = borderR;
            leftR.width = leftR.height = rightR.width = rightR.height = SI;
            leftR.x += 1;
            rightR.x += rightR.width;
            rightR.x -= 2 + 1;
            rightR.y -= 2;
            rightR.width += 4;
            rightR.height += 4;
            
            /*var guic = GUI.color;
            if ( !o.activeInHierarchy ) GUI.color *= alpha;
            Adapter.DrawTexture( rightR, storagetexture );
            Adapter.DrawTexture( leftR, compstexture );
            
            {   if ( auto )
                {   labelrect = _drawRect;
                    labelrect.y = _drawRect.y + _drawRect.height - 7;
                    labelrect.height = 9;
                    labelrect.width = 23;
                    Adapter.DrawTexture( labelrect, adapter.GetIcon( "STORAGE_AUTO" ) );
                }
            }
            GUI.color = guic;*/
            var guic = Color.white;
            if ( !o.activeInHierarchy ) guic.a *= alpha.a;
            Draw_AdapterTexture( rightR, storagetexture, guic );
            Draw_AdapterTexture( leftR, compstexture, guic );
            /*    Adapter.DrawTexture( rightR, storagetexture );
            Adapter.DrawTexture( leftR, compstexture );*/
            
            {   if ( auto )
                {   labelrect = _drawRect;
                    labelrect.y = _drawRect.y + _drawRect.height - 7;
                    labelrect.height = 9;
                    labelrect.width = 23;
                    //    Adapter.DrawTexture( labelrect, adapter.GetIcon( "STORAGE_AUTO" ) );
                    Draw_AdapterTexture( labelrect, adapter.GetIcon( "STORAGE_AUTO" ), guic );
                }
            }
            // GUI.color = guic;
            
            
            
            // /*MOUSE RECTS*/
            leftR = rightR = borderR;
            leftR.y = rightR.y = _drawRect.y;
            leftR.height = rightR.height = _drawRect.height;
            leftR.width = rightR.width = SI;
            rightR.x += rightR.width;
            /*
                            leftR.x += 1;
                            rightR.x += 1;*/
            
            // Adapter.GET_SKIN().button.active.background = Hierarchy.GetIcon("BUT");
            
            Draw_ModuleButton( leftR, PLAY_CONT_STORE, BUTTON_ACTION_0_HASH, true, useContentForButton: true, args: null);
            Draw_ModuleButton( rightR, PLAY_CONT_LINES, BUTTON_ACTION_1_HASH, true, useContentForButton: true, args: null );
            
            /*_DRAW_BUTTON( 1, rightR, o );
            _DRAW_BUTTON( 0, leftR, o );*/
            
            
            END_DRAW( _o );
            return width;
        }
        
        
        
        internal void INTERNAL__SetValue( Scene _s, Adapter.HierarchyObject o, bool All, int[] comps )
        {   if ( Hierarchy.HierarchyAdapterInstance.DISABLE_DESCRIPTION( _s ) ) return;
            if ( !o.Validate() ) return;
            var d = M_Descript.des(_s.GetHashCode());
            if ( d == null ) return;
            
            var s =  _s.GetHashCode();
            
            if ( comps == null )
            {   comps = new int[0];
                if ( DataKeeperCache.HasKey( s, o ) )
                {   var v = DataKeeperCache.GetValue(s, o);
                    for ( int i = 1 ; i < v.list.Count ; i++ )
                    {   ArrayUtility.Add( ref comps, v.list[i] );
                    }
                }
            }
            
            if ( !All && comps.Length == 0 )
            {   adapter.SET_UNDO( d, "Change PlayMode Data Keeper" );
                DataKeeperCache.SetValue( null, s, o.go, true );
            }
            else
            {   adapter.SET_UNDO( d, "Change PlayMode Data Keeper" );
                if ( !DataKeeperCache.HasKey( s, o ) ) DataKeeperCache.SetValue( new SingleList() { list = new List<int>() { 0 } }, s, o.go, true );
                var v = DataKeeperCache.GetValue(s, o);
                
                if ( All )
                {   v.list[0] = 1;
                    DataKeeperCache.SetValue( v, s, o.go, true );
                }
                else
                {   v.list = new List<int>( 1 ) { 0 };
                    for ( int i = 0 ; i < comps.Length ; i++ )
                    {   v.list.Add( comps[i] );
                    }
                    var r = o.go.GetComponents<Component>();
                    if ( r.Select( c => c.GetInstanceID() ).Count( i => v.list.Contains( i ) ) == r.Length ) v.list[0] = 1;
                    DataKeeperCache.SetValue( v, s, o.go, true );
                }
                
            }
            
            // if (Application.isPlaying && last_state)
            /* Hierarchy.SetDirty(d.component);
             Hierarchy.SetDirty(d.gameObject);
             Hierarchy.MarkSceneDirty(d.gameObject.scene);*/
        }
        
        // static Dictionary<int, KeeperDataItem> baked_new_objects = new Dictionary<int, KeeperDataItem>();
        
        internal struct GID : IEquatable<GID>, IEqualityComparer<GID>
        {   public static bool operator ==( GID x, GID y )
            {   return x.Equals( y );
            }
            public static bool operator !=( GID x, GID y )
            {   return !x.Equals( y );
            }
            
            internal Component component;
            internal int index;
            public bool Equals( GID other )
            {   return ((GID)other).index == index;
            }
            
            public override bool Equals( object obj )
            {   return ((GID)obj).index == index;
            }
            
            public bool Equals( GID x, GID y )
            {   return x.Equals( y );
            }
            
            public int GetHashCode( GID obj )
            {   return index;
            }
            
            public override int GetHashCode()
            {   return index;
            }
        }
        
        Dictionary<Type, List<GID>> GetIndexesDic( GameObject o, Component[] comps )
        {   if ( comps.Length == 0 ) return new Dictionary<Type, List<GID>>();
            var ttt = HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll(o).GroupBy(c => c.GetType()).ToDictionary(g => g.Key, g => g.ToList());
            return
                comps.Where( c => c && ttt.ContainsKey( c.GetType() ) )
            .Select( c => new { c, index = ttt[c.GetType()].IndexOf( c ) } )
            .Where( i => i.index != -1 ).GroupBy( c => c.c.GetType() ).ToDictionary( c => c.Key, c => c.Select( ac => new GID() { component = ac.c, index = ac.index } ).ToList() );
        }
        
        void SetValue( GameObject o, bool All, int[] comps )
        {   /*                List<GID> g1 = new List<GID>(1) { new GID() { component = o.transform, index = 3 } };
                            List<GID> g2 = new List<GID>(1) { new GID() { component = o.GetComponents<Component>()[1], index = 3 } };
                            Debug.Log(g1.Intersect(g2).Count());*/
            
            if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO.go != o ) )
            {   var selO = adapter.GetHierarchyObjectByInstanceID( o );
                INTERNAL__SetValue( o.scene, selO, All, comps );
                /* Undo.RecordObject(r, "Change sortingLayerName");
                 r.sortingLayerName = sortingLayer;
                 Hierarchy.SetDirty(r);
                 Hierarchy.MarkSceneDirty(o.scene);*/
                
                if ( adapter.par.ENABLE_PING_Fix ) adapter.TRY_PING_OBJECT( selO );
                
            }
            else
            {   var casd = (comps ?? new int[0]).Select(c => Adapter.GET_OBJECT(c) as Component).Where(c => c).ToArray();
                Dictionary<Type, List<GID>> refComps = GetIndexesDic(o, casd);
                //  Debug.Log(refComps.First().Key);
                foreach ( var objectToUndo in adapter.SELECTED_GAMEOBJECTS() )
                {   /* var targetComps = objectToUndo.GetComponents<Component>().Where(c => refComps.ContainsKey(c.GetType())).GroupBy(c => c.GetType()).ToDictionary(g => g.Key, g => g.ToList());
                         .GroupBy(c => c.GetType()).ToDictionary(g => g.Key.GetType(), g => g.ToList());
                    */
                    var targetComps = GetIndexesDic(objectToUndo.go, HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll(objectToUndo.go).Where(c => refComps.ContainsKey(c.GetType())).ToArray());
                    //  Debug.Log(targetComps.First().Value[0].index + " " + targetComps.Count + " " + targetComps.First().Value.Count);
                    //   Debug.Log(refComps.First().Value[0].index + " " + refComps.Count + " " + refComps.First().Value.Count);
                    var result = targetComps.Select(t => t.Value.Intersect(refComps[t.Key])).SelectMany(s => s.ToArray()).Select(c => c.component.GetInstanceID()).ToArray();
                    
                    //Debug.Log(result[0]);
                    
                    INTERNAL__SetValue( objectToUndo.go.scene, objectToUndo, All, result );
                    
                    /*   var c = cache.GetValue(objectToUndo.GetInstanceID());
                       if (!c) continue;
                       Undo.RecordObject(c, "Change sortingLayerName");
                       c.sortingLayerName = sortingLayer;
                       Hierarchy.SetDirty(c);
                       Hierarchy.MarkSceneDirty(c.gameObject.scene);*/
                    //  if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(objectToUndo);
                    
                }
            }
        }
        
        
        List<int> GetAdded( GameObject o, Component[] comps )
        {   var selO = adapter.GetHierarchyObjectByInstanceID( o );
        
            var getted = DataKeeperCache.GetValue(o.scene, selO);
            List<int> added = null;
            if ( getted != null && getted.list.Count > 0 )
            {   if ( getted.list[0] == 1 ) added = comps.Select( c => c.GetInstanceID() ).ToList();
                else
                {   added = getted.list.ToList();
                    if ( added.Count > 0 ) added.RemoveAt( 0 );
                }
            }
            else
            {   added = new List<int>();
            }
            return added;
        }
        
        void ApplyToChild( GameObject o, Dictionary<Component, bool> enablelist )
        {   foreach ( var componentsInChild in o.GetComponentsInChildren<Transform>( true ) )
            {   if ( componentsInChild.gameObject == o ) continue;
                var childComps = componentsInChild.GetComponents<Component>();
                var childadded = GetAdded(componentsInChild.gameObject, childComps);
                foreach ( var component in enablelist )     //if (!component.Value) continue;
                {   var type = Adapter.GetTypeName(component.Key);
                    var comp = childComps.FirstOrDefault(c => Adapter.GetTypeName(c) == type);
                    if ( !comp ) continue;
                    var iidd = comp.GetInstanceID();
                    if ( !component.Value ) childadded.RemoveAll( a => a == iidd );
                    else if ( !childadded.Contains( iidd ) ) childadded.Add( iidd );
                }
                var all = childadded.Count == childComps.Length;
                SetValue( componentsInChild.gameObject, all, childadded.ToArray() );
            }
        }
        
        Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_0_HASH = null;
        Adapter.DrawStackMethodsWrapper BUTTON_ACTION_0_HASH { get { return __BUTTON_ACTION_0_HASH ?? (__BUTTON_ACTION_0_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION_0 )); } }
        void BUTTON_ACTION_0( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
        {   _DRAW_BUTTON( 0, inputRect, _o, data.args );
        }
        Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_1_HASH = null;
        Adapter.DrawStackMethodsWrapper BUTTON_ACTION_1_HASH { get { return __BUTTON_ACTION_1_HASH ?? (__BUTTON_ACTION_1_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION_1 )); } }
        void BUTTON_ACTION_1( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
        {   _DRAW_BUTTON( 1, inputRect, _o, data.args );
        }
        void _DRAW_BUTTON( int index, Rect rect, Adapter.HierarchyObject _o, object args )        // var bg = Adapter.GET_SKIN().button.active.background;
        {
        
            // SingleList currentList = (SingleList)args;
            var o = _o.go;
            bool contains = false;
            SingleList currentList =   GET_CURRENT_LIST( _o, ref contains );
            // if ( adapter.ModuleButton( rect, index == 0 ? PLAY_CONT_LINES : PLAY_CONT_STORE, true ) )
            {   if ( Event.current.button == 0 )
                {   /* if (Event.current.control)
                    {
                     SetValue(o, false, new int[0]);
                    } else*/
                    {   var control = Event.current.control;
                        if ( index == 1 )
                        {   var list = new[] { o } .ToList();
                            if ( control ) list.AddRange( o.GetComponentsInChildren<Transform>( true ).Select( t => t.gameObject ) );
                            var state = !(currentList != null && currentList.list.Count > 0 && currentList.list[0] == 1);
                            foreach ( var __o in list )
                                SetValue( __o, state, null );
                            ResetStack();
                        }
                        //Debug.Log( index + " " +  currentList );
                        if ( index == 0 )
                        {   GenericMenu menu = new GenericMenu();
                            var comps = o.GetComponents<Component>();
                            
                            if ( LAST_VALIDATE_UNDO( o, comps ) )
                            {   menu.AddItem( new GUIContent( "Revert Last PlayMode Changes" ), false, () =>
                                {   if ( !o ) return;
                                    LAST_DO_UNDO( o, comps );
                                    ResetStack();
                                } );
                                menu.AddSeparator( "" );
                            }
                            
                            
                            var added = GetAdded(o, comps);
                            
                            menu.AddItem( new GUIContent( "- none -" ), false, () =>
                            {   var list = new[] { o } .ToList();
                                if ( control ) list.AddRange( o.GetComponentsInChildren<Transform>( true ).Select( t => t.gameObject ) );
                                foreach ( var __o in list )
                                    SetValue( __o, false, new int[0] );
                                ResetStack();
                            } );
                            // menu.AddSeparator("");
                            
                            List<string> was = new List<string>();
                            for ( int i = 0 ; i < comps.Length ; i++ )     // MonoBehaviour.print(comps[i].GetType());
                            {   if ( !comps[i] ) continue;
                                var captureID = comps[i].GetInstanceID();
                                var cont = new GUIContent();
                                var enabled = added.Contains(captureID);
                                var auto = adapter.par.DataKeeperParams.USE_SCRIPTS && Hierarchy_GUI.Instance(adapter).DataKeeper_HasScript((comps[i]));
                                
                                if ( enabled || auto ) cont = new GUIContent( Adapter.GetTypeName( comps[i] ) );
                                else cont = new GUIContent( "[ " + Adapter.GetTypeName( comps[i] ) + " ]" );
                                var rr = cont.text;
                                var innn = 0;
                                while ( was.Contains( cont.text ) ) cont.text = rr + " " + (innn++).ToString();
                                was.Add( cont.text );
                                
                                if ( auto ) cont.text += "  ( AUTO )";
                                
                                menu.AddItem( cont, enabled || auto, () =>
                                {   if ( auto ) return;
                                    var c = Adapter.GET_OBJECT(captureID) as Component;
                                    if ( !c ) return;
                                    if ( enabled ) added.RemoveAll( a => a == captureID );
                                    else if ( !added.Contains( captureID ) ) added.Add( captureID );
                                    // var all = added.Count == comps.Length;
                                    SetValue( o, false, added.ToArray() );
                                    if ( control )
                                    {   var enablelist = new Dictionary<Component, bool> { { c, !enabled } };
                                        ApplyToChild( o, enablelist );
                                    }
                                    ResetStack();
                                } );
                            }
                            
                            //  menu.AddSeparator("");
                            menu.AddSeparator( "" );
                            menu.AddItem( new GUIContent( "Apply to Children" ), false, () =>
                            {   var enablelist = comps.Where(c => c).ToDictionary(c => c, c => added.Contains(c.GetInstanceID()));
                                ApplyToChild( o, enablelist );
                                //  adapter.RESET_DRAW_STACKS();
                                ResetStack();
                            } );
                            menu.AddItem( new GUIContent( "Add MonoScript" ), false, () =>
                            {   EditorPrefs.SetInt( "Hierarchy Plugin Menu Item", 3 );
                                adapter.SHOW_HIER_SETTINGS_PLAYMODE_KEEPER();
                            } );
                            menu.ShowAsContext();
                        }
                    }
                    
                    /*
                                            GenericMenu menu = new GenericMenu();
                    
                                            for (int i = 0; i < l.Length; i++)
                                            {
                                                var ind = i;
                                                content.text = l[i];
                                                menu.AddItem(new GUIContent(content), content.text == oldSelect, () => Callback(ind));
                                            }
                                            menu.AddSeparator("");
                    
                                            /*    menu.AddItem(new GUIContent("Show 'Tags And Layers' Settings"), false, () => {
                                                  Selection.objects = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
                                              });
                                              menu.AddSeparator("");#1#
                    
                                            var pos = InputData.WidnwoRect(Event.current.mousePosition, 128, 68);
                                            menu.AddItem(new GUIContent("Assign a New SortingLayer"), false, () => {
                    
                    
                                                InputData.Init(pos, "New SortingLayer name's", (str) => {
                                                    if (string.IsNullOrEmpty(str)) return;
                                                    str = str.Trim();
                                                    var lowwer = l.Select(ord => ord.ToLower()).ToList();
                                                    var ind = lowwer.IndexOf(str.ToLower());
                                                    if (ind != -1)
                                                    {
                                                        SetLayer(r, l[ind]);
                                                        /* Undo.RecordObject(o, "Change tag");
                                                         o.tag = l[ind];
                                                         Hierarchy.SetDirty(o);
                                                         if (Hierarchy.par.ENABLE_PING_Fix) adapter.TRY_PING_OBJECT(o);#1#
                                                    } else
                                                    {
                    
                    
                                                        var UpdateSortingLayersOrder = typeof(InternalEditorUtility).GetMethod("UpdateSortingLayersOrder", (BindingFlags)(-1));
                                                        var AddSortingLayer = typeof(InternalEditorUtility).GetMethod("AddSortingLayer", (BindingFlags)(-1));
                                                        var SetSortingLayerName = typeof(InternalEditorUtility).GetMethod("SetSortingLayerName", (BindingFlags)(-1));
                                                        var GetSortingLayerCount = typeof(InternalEditorUtility).GetMethod("GetSortingLayerCount", (BindingFlags)(-1));
                    
                                                        var count = GetSortingLayerCount.Invoke(null, null);
                                                        AddSortingLayer.Invoke(null, null);
                                                        SetSortingLayerName.Invoke(null, new[] { count, str });
                                                        UpdateSortingLayersOrder.Invoke(null, null);
                    
                                                        SetLayer(r, str);
                                                    }
                                                });
                                            });
                    
                                            menu.AddSeparator("");
                                            menu.AddItem(new GUIContent("[ Show only uppercase letters ]"), par.UPPER_SORT != 0, () => {
                                                par.UPPER_SORT = 1 - par.UPPER_SORT;
                                                SavePrefs();
                                            });
                                            menu.AddItem(new GUIContent("Show 'Tags And Layers' Settings"), false, () => {
                                                Selection.objects = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
                                            });
                    
                    
                                            menu.ShowAsContext();*/
                    Adapter.EventUse();
                    
                }
                
                
                if ( Event.current.button == 1 )
                {
                
                    Adapter.EventUse();
                    /* int[] contentCost = new int[0];
                     GameObject[] obs = new GameObject[0];
                    
                    
                     if (index == 0 && Validate(o))
                     {
                         if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeaderFiltered(out obs, out contentCost, currentList.list);
                         FillterData.Init(Event.current.mousePosition, SearchHelper, "such as " + o.name, obs, contentCost, null, this);
                     } else
                     {
                         CallHeader(out obs, out contentCost);
                         FillterData.Init(Event.current.mousePosition, SearchHelper, "All assigned", obs, contentCost, null, this);
                     }*/
                    var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                    
                    _W__SearchWindow.Init( mp, SearchHelper, index == 0 && Validate( adapter.GetHierarchyObjectByInstanceID( o ) ) ? "such as " + o.name : "All assigned",
                                           index == 0 && Validate( adapter.GetHierarchyObjectByInstanceID( o ) ) ?
                                           CallHeaderFiltered( currentList.list ) :
                                           CallHeader(),
                                           this, adapter, _o );
                                           
                    // EditorGUIUtility.ic
                }
            }
            
            //  Adapter.GET_SKIN().button.active.background = bg;
        }
        
        
        
        
        
        bool Validate( Adapter.HierarchyObject o )
        {
        
            var l = DataKeeperCache.GetValue(o.go.scene, o);
            if ( l == null ) return false;
            for ( int i = l.list.Count - 1 ; i > 0 ; i-- )
                if ( !Adapter.GET_OBJECT( l.list[i] ) ) l.list.RemoveAt( i );
            // return !string.IsNullOrEmpty(o.tag) && o.tag != "Untagged";
            return l.list.Count != 0 && (l.list[0] == 1 || l.list.Count > 1);
        }
        bool Validate( Adapter.HierarchyObject o, List<int> filter )
        {   var l = DataKeeperCache.GetValue(o.go.scene, o);
            if ( l == null ) return false;
            for ( int i = l.list.Count - 1 ; i > 0 ; i-- )
                if ( !Adapter.GET_OBJECT( l.list[i] ) ) l.list.RemoveAt( i );
            // return !string.IsNullOrEmpty(o.tag) && o.tag != "Untagged";
            if ( !(l.list.Count != 0 && (l.list[0] == 1 || l.list.Count > 1)) ) return false;
            
            if ( filter[0] == 1 && l.list[0] == 1 ) return true;
            
            for ( int i = 0 ; i < filter.Count ; i++ )
            {   if ( filter[i] != l.list[i] ) return false;
            }
            return true;
        }
        
        
        
        /* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
                     Validate(o) ?
                     CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
                     CallHeader(),
                     this);*/
        /** CALL HEADER */
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   var result = new _W__SearchWindow.FillterData_Inputs(callFromExternal_objects)
            {   Valudator = Validate,
                    SelectCompareString = (d, i) =>
                {   var k = DataKeeperCache.GetValue(d.go.scene, d);
                    if (k == null) return "";
                    var cost = k.list.Count;
                    if (k.list[0] == 0) cost += 1000000;
                    return cost.ToString();
                },
                SelectCompareCostInt = (d, i) =>
                {   var cost = i;
                    cost += d.go.activeInHierarchy ? 0 : 100000000;
                    return cost;
                }
            };
            return result;
        }
        
        internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered( List<int> filter )
        {   var result = CallHeader();
            result.Valudator = s => Validate( s, filter );
            return result;
        }
        /** CALL HEADER */
        
        
        
        
        /*    internal override bool CallHeader(out GameObject[] obs, out int[] contentCost)
            {
                obs = Utilities.AllSceneObjects().Where(Validate).ToArray();
                contentCost = obs
                   .Select((d, i) => new { data = DataKeeperCache.GetValue(d.scene, d), startIndex = i, obj = d })
        
                   .OrderByDescending(d => d.data.list[0]).ThenBy(d => d.data.list.Count)
                    .Select((d, i) => {
                        var cost = i;
                        cost += d.obj.activeInHierarchy ? 0 : 100000000;
                        return new { d.startIndex, cost = cost };
                    })
                   .OrderBy(d => d.startIndex)
                   .Select(d => d.cost).ToArray();
                return true;
            }
        
            internal void CallHeaderFiltered(out GameObject[] obs, out int[] contentCost, List<int> filter)
            {
                obs = Utilities.AllSceneObjects().Where(s => Validate(s, filter)).ToArray();
                contentCost = obs
                   .Select((d, i) => new { data = DataKeeperCache.GetValue(d.scene, d), startIndex = i, obj = d })
        
                   .OrderByDescending(d => d.data.list[0]).ThenBy(d => d.data.list.Count)
                    .Select((d, i) => {
                        var cost = i;
                        cost += d.obj.activeInHierarchy ? 0 : 100000000;
                        return new { d.startIndex, cost = cost };
                    })
                   .OrderBy(d => d.startIndex)
                   .Select(d => d.cost).ToArray();
                /*   obs = Utilities.AllSceneObjects().Where(s => Validate(s, filter)).ToArray();
                   contentCost = obs
                      .Select((d, i) => new { name = cache.GetValue(d.GetInstanceID()).sortingLayerName, startIndex = i, obj = d })
                      .OrderBy(d => d.name)
                      .Select((d, i) => {
                          var cost = i;
                          cost += d.obj.activeInHierarchy ? 0 : 100000000;
                          return new { d.startIndex, cost = cost };
                      })
                      .OrderBy(d => d.startIndex)
                      .Select(d => d.cost).ToArray();#1#
            }
        */
        
    }
}
}

/*
  void RECORD_FLUSH(KeeperData source, Component comp)
            {
                /* RECORD #1#
                if (!source.field_records.ContainsKey(comp.GetInstanceID()))
                {
                    var id = comp.GetInstanceID();
                    source.field_records.Add(id, new Dictionary<FieldInfo, object>());
                    source.prop_records.Add(id, new Dictionary<PropertyInfo, object>());
                    //   MonoBehaviour.print(comp.GetType() + " " + comp.GetType().GetFields(FieldFlags).Length);

                    var type = comp.GetType();
                    if (!baked_types.ContainsKey(type.Name)) baked_types.Add(type.Name,
                         new BakedFields() {
                             fields = type.GetFields(FieldFlags).Where(field => field.IsPublic || field.GetCustomAttributes(false).Length != 0).ToArray(),
                             props = type.GetProperties(PropertyFlags).Where(p => p.CanRead && p.CanWrite).ToArray(),
                         });
                    var baked = baked_types[type.Name];
                    foreach (var field in baked.fields) source.field_records[id].Add(field, field.GetValue(comp));
                    foreach (var prop in baked.props) source.prop_records[id].Add(prop, prop.GetValue(comp, null));
                }
                /* RECORD #1#


                /*
                                Editor.
                                 editor = Editor.CreateEditor(activeGO.transform);

                                SerializedObject so = new SerializedObject(Selection.activeGameObject.GetComponent<Renderer>());

                                so.FindProperty("m_ScaleInLightmap").cop
                                so.ApplyModifiedProperties();
                                new SerializedObject()
                                so.#1#
            }

            string flush_current()
            {
                if (!par.DataKeeperParams.ENABLE) return "";

                var result = new KeeperData();
                if (par.DataKeeperParams.USE_SCRIPTS)
                {
                    foreach (var mDataKeeperValue in Hierarchy_GUI.Initialize().m_DataKeeper_Values)
                    {
                        if (!mDataKeeperValue.value) continue;
                        foreach (var finded in Resources.FindObjectsOfTypeAll(mDataKeeperValue.value.GetClass()))
                        {
                            var comp = finded as Component;
                            if (!comp || !comp.gameObject || !comp.gameObject.scene.IsValid()) continue;
                            /* RECORD #1#
                            RECORD_FLUSH(result, comp);
                            /* if (!result.records.ContainsKey(comp.GetInstanceID()))
                             {
                                 var id = comp.GetInstanceID();
                                 result.records.Add(id, new Dictionary<FieldInfo, object>());
                                 foreach (var field in comp.GetType().GetFields(flags))
                                 {
                                     if (field.IsPublic || field.GetCustomAttributes(false).Length != 0)
                                     {
                                         result.records[id].Add(field, field.GetValue(comp));
                                     }
                                 }

                             }#1#
                            /* RECORD #1#
                        }
                    }
                }

                foreach (var cache in DataKeeperCache.cacheDic)
                {
                    var s = cache.Key;
                    foreach (var obj in cache.Value)
                    {
                        var o = Adapter.GET_OBJECT(obj.Key) as GameObject;
                        if (!o) continue;
                        MonoBehaviour.print("ASD");
                        var getted = DataKeeperCache.GetValue(s, o);
                        if (getted == null || getted.list.Count == 0) continue;
                        if (getted.list[0] == 1)
                        {
                            foreach (var comp in o.GetComponents<Component>())
                            {
                                if (!comp) continue;
                                RECORD_FLUSH(result, comp);
                            }
                        } else
                        {
                            for (int i = 1; i < getted.list.Count; i++)
                            {
                                var comp = Adapter.GET_OBJECT(getted.list[i]) as Component;
                                if (!comp) continue;
                                RECORD_FLUSH(result, comp);
                            }
                        }
                    }
                }
                return SERIALIZE_SINGLE(result, formatter);
            }

            void SaveLast()
            {
                MonoBehaviour.print("SaveLast");
                /*  MonoBehaviour.print("Save");#1#
                var flush = flush_current();
                /*if (editMode)
                {
                    editMode = false;#1#
                EditorPrefs.SetString("Hierarchy Plugin/Data Keeper/Last State", flush);
                //  }
                // EditorPrefs.SetString("Hierarchy Plugin/Data Keeper/Current State", flush);
            }
            void SaveCurrent()
            {
                MonoBehaviour.print("SaveCurrent");
                /*  MonoBehaviour.print("Save");#1#
                var flush = flush_current();
                /*   if (editMode)
                   {
                       editMode = false;
                       EditorPrefs.SetString("Hierarchy Plugin/Data Keeper/Last State", flush);
                   }#1#
                EditorPrefs.SetString("Hierarchy Plugin/Data Keeper/Current State", flush);
            }
            List<Component> writeChanges = new List<Component>();
            void Load()
            {
                MonoBehaviour.print("Load");
                var last = EditorPrefs.GetString("Hierarchy Plugin/Data Keeper/Last State", "");
                var current = EditorPrefs.GetString("Hierarchy Plugin/Data Keeper/Current State", "");
                EditorPrefs.SetString("Hierarchy Plugin/Data Keeper/Current State", "");

                if (!string.IsNullOrEmpty(last))
                {
                    try { last_state = (KeeperData)DESERIALIZE_SINGLE(last, formatter); }
                    catch { last_state = null; }
                    // last_state = (KeeperData)DESERIALIZE_SINGLE(last, formatter);
                    /*                    try { last_state = (KeeperData)DESERIALIZE_SINGLE(last, formatter); }
                                        catch { last_state = new KeeperData(); }#1#
                }

                if (string.IsNullOrEmpty(current)) return;

#if !UNITY_EDITOR
                try {
#endif
                current_state = (KeeperData)DESERIALIZE_SINGLE(current, formatter);
#if !UNITY_EDITOR
                }
                catch { current_state = null; }
#endif

            }

            bool LAST_VALIDATE_UNDO(GameObject o, Component[] comps)
            {
                if (last_state == null) return false;
                foreach (var comp in comps)
                {
                    if (!comp) continue;
                    if (last_state.field_records.ContainsKey(comp.GetInstanceID()))
                    {
                        var id = comp.GetInstanceID();
                        foreach (var record in last_state.field_records[id])
                        {
                            try
                            {
                                var oldVal = record.Key.GetValue(comp);
                                if (Equals(oldVal, record.Value)) continue;
                                return true;
                            }
                            catch { }
                        }
                        foreach (var record in last_state.prop_records[id])
                        {
                            try
                            {
                                var oldVal = record.Key.GetValue(comp, null);
                                if (Equals(oldVal, record.Value)) continue;
                                return true;
                            }
                            catch { }
                        }

                    }
                }
                return false;
            }

            void LAST_DO_UNDO(GameObject o, Component[] comps)
            {
                if (last_state == null) return;
                foreach (var comp in comps)
                {
                    if (!comp) continue;
                    if (last_state.field_records.ContainsKey(comp.GetInstanceID()))
                    {
                        var id = comp.GetInstanceID();
                        var wasChange = false;
                        foreach (var field in last_state.field_records[id])
                        {
                            object oldVal;
                            try { oldVal = field.Key.GetValue(comp); }
                            catch { continue; }
                            if (Equals(oldVal, field.Value)) continue;
                            if (!wasChange) Undo.RecordObject(comp, "Revert Last PlayMode Changes");
                            wasChange = true;
                            field.Key.SetValue(comp, field.Value);
                        }
                        foreach (var field in last_state.prop_records[id])
                        {
                            // var oldVal = field.Key.GetValue(comp, null);
                            object oldVal;
                            try { oldVal = field.Key.GetValue(comp, null); }
                            catch { continue; }
                            if (Equals(oldVal, field.Value)) continue;
                            if (!wasChange) Undo.RecordObject(comp, "Revert Last PlayMode Changes");
                            wasChange = true;
                            field.Key.SetValue(comp, field.Value, null);
                        }
                        if (wasChange)
                        {
                            EditorUtility.SetDirty(comp);
                            EditorUtility.SetDirty(comp.gameObject);
                        }
                        /*

                                                foreach (var record in last_state.field_records)
                                                {
                                                    var obj = Adapter.GET_OBJECT(record.Key);
                                                    if (!obj) continue;
                                                    var wasChange = false;
                                                    foreach (var field in record.Value)
                                                    {
                                                        var oldVal = field.Key.GetValue(obj);
                                                        if (oldVal.Equals(field.Value)) continue;
                                                        wasChange = true;
                                                        Undo.RecordObject(obj, "Revert Last PlayMode Changes");
                                                        field.Key.SetValue(obj, field.Value);
                                                    }
                                                    if (wasChange)
                                                    {
                                                        EditorUtility.SetDirty(obj);
                                                        EditorUtility.SetDirty(o);
                                                    }
                                                }#1#
                    }
                }
            }



            static bool editMode = true;
            private void Update()
            {

                if (!Application.isPlaying)
                {
                    if (applyOnUpdate != null)
                    {
                        applyOnUpdate();
                        applyOnUpdate = null;
                    }

                    editMode = true;

                    if (current_state != null)
                    {

                        MonoBehaviour.print(current_state.field_records.Count);
                        foreach (var record in current_state.field_records)
                        {
                            var comp = Adapter.GET_OBJECT(record.Key) as Component;
                            if (!comp) continue;

                            var wasChange = false;
                            foreach (var field in current_state.field_records[record.Key])
                            {
                                var oldVal = field.Key.GetValue(comp);
                                if (Equals(oldVal, field.Value)) continue;
                                if (!wasChange) Undo.RecordObject(comp, "Perform PlayMode Data Keeper");
                                wasChange = true;
                                MonoBehaviour.print(comp + " " + field.Value);
                                field.Key.SetValue(comp, field.Value);

                            }
                            foreach (var field in current_state.prop_records[record.Key])
                            {
                                var oldVal = field.Key.GetValue(comp, null);
                                if (Equals(oldVal, field.Value)) continue;
                                if (!wasChange) Undo.RecordObject(comp, "Perform PlayMode Data Keeper");
                                wasChange = true;
                                MonoBehaviour.print(comp + " " + field.Value);
                                field.Key.SetValue(comp, field.Value, null);
                            }
                            if (wasChange)
                            {
                                writeChanges.Add(comp);
                                /*  EditorUtility.SetDirty(comp);
                                  EditorUtility.SetDirty(comp.gameObject);#1#
                            }
                        }
                        current_state = null;
                    }


                    if (writeChanges.Count != 0)
                    {
                        List<Scene> scenes = new List<Scene>();
                        foreach (var writeChange in writeChanges)
                        {
                            if (!writeChange || !writeChange.gameObject.scene.IsValid()) continue;
                            EditorUtility.SetDirty(writeChange);
                            EditorUtility.SetDirty(writeChange.gameObject);
                            if (!scenes.Contains(writeChange.gameObject.scene)) scenes.Add(writeChange.gameObject.scene);
                        }
                        foreach (var scene in scenes)
                        {
                            EditorSceneManager.MarkSceneDirty(scene);
                        }
                        writeChanges.Clear();
                    }
                }

            }



            [Serializable]
            internal class KeeperData
            {
                [SerializeField]
                internal Dictionary<int, Dictionary<FieldInfo, System.Object>> field_records = new Dictionary<int, Dictionary<FieldInfo, System.Object>>();
                [SerializeField]
                internal Dictionary<int, Dictionary<PropertyInfo, System.Object>> prop_records = new Dictionary<int, Dictionary<PropertyInfo, System.Object>>();
            }*/

