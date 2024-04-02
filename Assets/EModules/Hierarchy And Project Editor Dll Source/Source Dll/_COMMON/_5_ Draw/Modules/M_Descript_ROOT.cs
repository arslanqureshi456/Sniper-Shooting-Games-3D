#if !UNITY_EDITOR
    #define REMOVE_ONLY_FLAGS
#endif

#if UNITY_EDITOR
    #define HIERARCHY
    //#define LOG
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
#if PROJECT
    using EModules.Project;
#endif
namespace EModules.EModulesInternal

{


internal partial class Adapter {

    internal UnityEngine.Object GUI_TO_OBJECT( string guid )
    {   var path = AssetDatabase.GUIDToAssetPath(guid);
        if ( string.IsNullOrEmpty( path ) ) return null;
        var asset = AssetDatabase.LoadAssetAtPath< UnityEngine.Object>( path );
        return asset;
    }
    
    internal void TRY_PING_OBJECT( GameObject o )
    {   var selO = GetHierarchyObjectByInstanceID( o );
        TRY_PING_OBJECT( selO );
        
    }
    internal void TRY_PING_OBJECT( Adapter.HierarchyObject o )
    {   if ( par.ENABLE_PING_Fix )
        {   UnityEngine.Object target =  null;
            if ( IS_HIERARCHY() ) target = o.go;
            else target = GUI_TO_OBJECT( o.project.guid );
            
            if ( target ) EditorGUIUtility.PingObject( target );
        }
        
    }
    
    internal UnityEngine.Object HO_TO_OBJECT( Adapter.HierarchyObject o )
    {   UnityEngine.Object target =  null;
        if ( IS_HIERARCHY() ) target = o.go;
        else target = GUI_TO_OBJECT( o.project.guid );
        return target;
    }
    
    internal void HO_RECORD_UNDO( Adapter.HierarchyObject o, string name )
    {   UnityEngine.Object target =  null;
        if ( IS_HIERARCHY() ) target = o.go;
        else target = GUI_TO_OBJECT( o.project.guid );
        
        if ( target ) Undo.RecordObject( target, name );
    }
    
    internal void HO_SETDIRTY( Adapter.HierarchyObject o )
    {   UnityEngine.Object target =  null;
        if ( IS_HIERARCHY() ) target = o.go;
        else target = GUI_TO_OBJECT( o.project.guid );
        
        if ( target ) EditorUtility.SetDirty( target );
    }
    
    
    internal class M_DescriptionCommon : Adapter.Module {
    
    
    
    
    
        internal virtual void TrySaveCustomRegistrator<TValue>( Adapter.HierarchyObject o, TValue value, Adapter.CacherType type )
        {
        }
        internal virtual void UpdateSwitchRegistratorEnable()
        {
        }
        internal virtual void RegistrateDescription( IDescriptionRegistrator reg, Adapter adapter )
        {
        }
        internal virtual void TrySaveHiglighterRegistrator( Adapter.HierarchyObject o, TempColorClass colors32 )
        {
        }
        
        
        internal GoGuidPair tempPair = GoGuidPairConstructor_WithoutCache();
        internal GoGuidPair SetPair( Adapter.HierarchyObject _o )
        {   tempPair.go = _o.go;
        
            if ( !adapter.IS_HIERARCHY() )
            {
#pragma warning disable
                tempPair.guid = _o.project.guid;
#pragma warning restore
                tempPair.path = _o.project.assetPath;
            }
            
            return tempPair;
        }
        
        
        DoubleList<GoGuidPair, string> dl = new DoubleList<GoGuidPair, string>();
        static internal Dictionary<int, Dictionary<int, Dictionary<Adapter.HierarchyObject, int>>> __cacheDescriptionDic =
            new Dictionary<int, Dictionary<int, Dictionary<Adapter.HierarchyObject, int>>>();
        internal Dictionary<int, Dictionary<Adapter.HierarchyObject, int>> cacheDescriptionDic
        {   get
            {   if ( !__cacheDescriptionDic.ContainsKey( adapter.pluginID ) ) __cacheDescriptionDic.Add( adapter.pluginID, new Dictionary<int, Dictionary<HierarchyObject, int>>() );
                return __cacheDescriptionDic[adapter.pluginID];
            }
        }
        
        public DoubleList<GoGuidPair, string> getDoubleList( int s )
        {
        
            var d = adapter.MOI.des(s);
            
            dl.listKeys = d.GetHash1_Fix2_0();
            dl.listValues = d.GetHash2();
            
            return dl;
        }
        //** KEYS **//
        internal void SetValue( string value, int s, Adapter.HierarchyObject o )
        {   if ( adapter.DISABLE_DESCRIPTION( o ) ) return;
            // Debug.Log(value + " " + !o.Validate());
            if ( !o.Validate() ) return;
            
            
            var d = adapter.MOI.des(s);
            if ( d == null ) return;
            
            UA();
            
            var list = getDoubleList( s );
            var p  = SetPair(o);
            if ( list.ContainsKey( p ) )
            {   list[p] = value;
            }
            else list.Add( p, value );
            
            //if (  string.IsNullOrEmpty( value ) )list.RemoveAll(p);
            //  Debug.Log(p.GetHashCode());
            
            if ( !Application.isPlaying )
            {   adapter.SetDirtyDescription( d, s );
                adapter.MarkSceneDirty( s );
            }
        }
        internal bool HasKey( int s, Adapter.HierarchyObject _o )
        {   if ( !_o.Validate() ) return false;
            //   if (adapter.pluginID == 0)Debug.Log(cacheDic.Count);
            if ( !cacheDescriptionDic.ContainsKey( s ) )
            {   var d = adapter.MOI.des(s);
                var h = d.GetHash1_Fix2_0();
                cacheDescriptionDic.Add( s, new Dictionary<Adapter.HierarchyObject, int>() );
                // Debug.Log("ASD" + adapter.pluginID);
                while ( d.GetHash1_Fix2_0().Count != d.GetHash2().Count )
                {   if ( d.GetHash2().Count < d.GetHash1_Fix2_0().Count ) d.GetHash2().Add( "" );
                    else d.GetHash2().RemoveAt( d.GetHash2().Count - 1 );
                }
                for ( int i = 0 ; i < h.Count ; i++ )
                {   var getO = adapter.GetHierarchyObjectByPair( ref h, i);
                    if ( !getO.Validate() ) continue;
                    if ( cacheDescriptionDic[s].ContainsKey( getO ) )
                    {   d.GetHash1_Fix2_0().RemoveAt( i );
                        d.GetHash2().RemoveAt( i );
                        i--;
                    }
                    else
                    {   cacheDescriptionDic[s].Add( getO, i );
                    }
                }
            }
            
            return cacheDescriptionDic[s].ContainsKey( _o );
        }
        internal string GetValue( int s, Adapter.HierarchyObject o )
        {   if ( !HasKey( s, o ) ) return null;
            var d = adapter.MOI. des(s);
            if ( d == null || !cacheDescriptionDic[s].ContainsKey( o ) ) return null;
            var ptr = cacheDescriptionDic[s][o];
            return d.GetHash2()[ptr];
        }
        //** KEYS **//
        /// <summary>
        ///  OLD CACHE
        /*
         internal bool HasKey(int s, Adapter.HierarchyObject _o)
        {   if (!_o.Validate()) return false;
           if (!cacheDic.ContainsKey( s ))
           {   var d = adapter.MOI.des(s);
               var h = d.GetHash1_Fix2_0();
               cacheDic.Add( s, new Dictionary<long, int>() );
               for (int i = 0 ; i < h.Count ; i++)
               {   var getO = adapter.GetHierarchyObjectByPair( ref h, i);
                   if (!getO.Validate()) continue;
                   if (cacheDic[s].ContainsKey( getO.fileID ))
                   {   d.GetHash1_Fix2_0().RemoveAt( i );
                       d.GetHash2().RemoveAt( i );
                       i--;
                   }
                   else
                   {   cacheDic[s].Add( getO.fileID, i );
                   }
               }
           }
           return cacheDic[s].ContainsKey( _o.fileID );
        }
        internal string GetValue(int s, Adapter.HierarchyObject o)
        {   if (!HasKey( s, o )) return null;
           var d = adapter.MOI. des(s);
           if (d == null || !cacheDic[s].ContainsKey( o.fileID )) return null;
           var ptr = cacheDic[s][o.fileID];
           return d.GetHash2()[ptr];
        }
         */
        /// </summary>
        /// <param name="_o"></param>
        /// <param name="d"></param>
        /// <param name="upw"></param>
        
        
        
        internal void CREATE_NEW_ESCRIPTION( Adapter adapter, MousePos pos, Adapter.HierarchyObject _o, int scene, bool singleObject )         //  var o = _o.go;
        {   // Adapter adapter = Initializator.AdaptersByID[  _o.pluginID];
            /* clamp = !clamp;
             // var pos = InputData.WidnwoRect( clamp, Event.current.mousePosition, 190, 68, adapter  );
             if (clamp) pos.y -= pos.height;*/
            
            //  pos.y += pos.height / 2;
            
            // var scene = _o.scene;
            Action<string> act = ( str ) =>
            {
            
                adapter.SET_UNDO( adapter.MOI.des(scene), "Change description" );
                // cacheDic.Clear();
                
                bool result = false;
                try
                {   DoubleList<GoGuidPair, string> list = getDoubleList( scene );
                
                    if ( adapter.SELECTED_GAMEOBJECTS().All( selO => selO != _o ) || singleObject)
                    {   result |= AddDescriptionToList( ref list, _o, scene, str );
                    
                        adapter.TRY_PING_OBJECT(_o);
                    }
                    else
                    {   foreach ( var gameObject in adapter.SELECTED_GAMEOBJECTS() )
                        {   result |= AddDescriptionToList( ref list, gameObject, scene, str );
                        }
                    }
                    
                }
                catch (Exception ex)
                {   Debug.LogError(ex.Message + "\n\n" + ex.StackTrace);
                }
                
                if (!result) return;
                
                UA();
                
                var d = adapter.MOI.des(scene);
                if (( !Application.isPlaying || adapter.pluginID != 0 || Hierarchy_GUI.Instance(adapter).SaveToScriptableObject == "FOLDER") && (d as UnityEngine.Object))
                {   adapter.SetDirtyDescription( d, d.gameObject ? d.gameObject.scene.GetHashCode() : adapter.GET_ACTIVE_SCENE );
                    if ( !Application.isPlaying ) adapter.MarkSceneDirty( d.gameObject ? d.gameObject.scene.GetHashCode() : adapter.GET_ACTIVE_SCENE );
                }
                
            };
            _W__InputWindow.Init( pos, "New description", adapter, act, act, textInputSet: getDoubleList( scene ).ContainsKey( SetPair( _o ) ) ? getDoubleList( scene )[SetPair( _o )] : null );
            
            
        }
        
        protected bool AddDescriptionToList( ref DoubleList<GoGuidPair, string> list, Adapter.HierarchyObject _o, int scene, string str, bool SaveRegistrator = true )
        {
        
            var o = SetPair( _o );
            if ( string.IsNullOrEmpty( str ) )
            {
            
                list.RemoveAll( o );
            }
            else
            {   str = str.Trim();
            
            
            
                /*   Debug.Log(str );
                   Debug.Log(o.GetHashCode() + " " + o.guid.GetHashCode() );
                
                   var b = list.ContainsKey( o );
                   Debug.Log(list.ContainsKey( o ));
                   Debug.Log(list.ContainsKey( o ));*/
                
                if ( list.ContainsKey( o ) )
                {   if ( str == list[o] ) return false;
                    list[o] = str;
                }
                else
                {   list.Add( o, str );
                }
            }
            
            UA();
            
            #if HIERARCHY
            if ( SaveRegistrator && adapter.IS_HIERARCHY() ) Hierarchy.M_Descript.TrySaveDescriptionRegistrator( o.go, str );
            #endif
            
            return true;
        }
        
        
        
        
        
        internal override float GetInputWidth()
        {   return 0.5f;
        }
        
        
        public M_DescriptionCommon( int restWidth, int sibbildPos, bool enable, Adapter adapter ) : base( restWidth, sibbildPos, enable, adapter )
        {   adapter.SubcripeSceneLoader( OnSceneLoaded );
            adapter.OnClearObjects -= UA;
            adapter.OnClearObjects += UA;
            adapter.onUndoAction -= UA;
            adapter.onUndoAction += UA;
        }
        
        virtual public void OnSceneLoaded()
        {
        
        }
        
        void UA()
        {
        
            ResetStack();
            //  Debug.Log(adapter.pluginID + " -- " + cacheDescriptionDic.Count);
            // cacheDescriptionDic  = new Dictionary<int, Dictionary<HierarchyObject, int>>();
            __cacheDescriptionDic.Clear();
            // wasInitDes = false;
            // adapter.ClearHierarchyObjects();
            // Debug.Log(adapter.pluginID + " -A - " + cacheDescriptionDic.Count);
        }
        
        
        
        bool wasInitDes = false;
        GUIContent content = new GUIContent();
        internal override float Draw( Rect drawRect, Adapter.HierarchyObject _o )
        {
        
        
            if ( _o.name == "NewBehaviourScript" )
            {
            
                /* var mem = adapter.MOI.des(_o.scene).GetHash1_Fix2_0();
                 Debug.Log(mem.Count);
                
                
                 Debug.Log(cacheDic[_o.scene].Count);
                 Debug.Log(cacheDic[_o.scene].First().Key.project.assetName + " " + cacheDic[_o.scene].Last().Key.project.assetName);
                 Debug.Log(cacheDic[_o.scene].ContainsKey(_o));*/
                
                /*Debug.Log(HasKey( _o.scene, _o ));
                Debug.Log(_o.fileID);
                Debug.Log(cacheDic.Keys.Select(s => s.ToString()).Aggregate((a, b) => a + " " + b));*/
            }
            
            if ( adapter.DISABLE_DESCRIPTION( _o ) )
            {   /*var fs = Adapter.GET_SKIN().label.fontSize;
                var al = Adapter.GET_SKIN().label.alignment;
                Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleRight;
                if ( !callFromExternal() ) Adapter.GET_SKIN().label.fontSize = adapter.FONT_8();
                else Adapter.GET_SKIN().label.fontSize = adapter.WINDOW_FONT_8();*/
                
                GUI.Label( drawRect, Adapter.CacheDisableConten, !callFromExternal() ? adapter.STYLE_LABEL_8_right : adapter.STYLE_LABEL_8_WINDOWS_right );
                
                //                     Adapter.GET_SKIN().label.alignment = al;
                //                     Adapter.GET_SKIN().label.fontSize = fs;
                return width;
            }
            
            if ( adapter.IS_PROJECT() )
            {   if ( !_o.project.IsMainAsset )
                {   //                         var fs = Adapter.GET_SKIN().label.fontSize;
                    //                         var al = Adapter.GET_SKIN().label.alignment;
                    //                         Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleCenter;
                    //                         Adapter.GET_SKIN().label.fontSize = adapter.WINDOW_FONT_8();
                    
                    GUI.Label( drawRect, "-", adapter.STYLE_LABEL_8_WINDOWS_middle );
                    
                    //                         Adapter.GET_SKIN().label.alignment = al;
                    //                         Adapter.GET_SKIN().label.fontSize = fs;
                    return width;
                }
            }
            
            
            
            
            
            //var o = _o.go;
            var d = adapter.MOI. des( _o.scene );
            if ( d == null ) return width;
            
            
            if ( !START_DRAW( drawRect, _o ) ) return 0;
            
            
            content.tooltip = base.ContextHelper;
            
            if ( !wasInitDes )
            {
            
                adapter.onUndoAction -= UA;
                adapter.onUndoAction += UA;
                adapter.bottomInterface.onSceneChange -= UA;
                adapter.bottomInterface.onSceneChange += UA;
                
                wasInitDes = true;
                var list = getDoubleList( _o.scene );
                for ( int i = list.Count - 1 ; i >= 0 ; i-- )
                {   var pp = adapter.GetHierarchyObjectByPair( ref list.listKeys, i );
                    if ( !pp.Validate() || string.IsNullOrEmpty( list.listValues[i] ) )
                    {   list.RemoveAt( i );
                    }
                }
            }
            
            // if (d.GetHash1().Contains(o))
            
            bool hasContent = false;
            
            if ( HasKey( _o.scene, _o ) )      //  MonoBehaviour.print(Resources.FindObjectsOfTypeAll<DescriptionHelper>().Length);
            {   //  var list = getDoubleList(o.scene);
                var ptr = cacheDescriptionDic[_o.scene][_o];
                
                // if (ptr >= d.GetHash2().Count) Debug.Log(ptr);
                
                content.tooltip = content.text = d.GetHash2()[ptr];
                // content.tooltip = list[o];
                
                //                     var fs = Adapter.GET_SKIN().label.fontSize;
                //                     var al = Adapter.GET_SKIN().label.alignment;
                //                     Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleRight;
                //                     if ( !callFromExternal() ) Adapter.GET_SKIN().label.fontSize = adapter.FONT_8();
                //                     else Adapter.GET_SKIN().label.fontSize = adapter.WINDOW_FONT_8();
                
                /*   GUI.enabled = _o.Active();
                   GUI.Label( drawRect, content, !callFromExternal() ? adapter.STYLE_LABEL_8_right : adapter.STYLE_LABEL_8_WINDOWS ); //adapter.STYLE_LABEL_8
                   GUI.enabled = true;*/
                Draw_Label( drawRect, content, !callFromExternal() ? adapter.STYLE_LABEL_8_right : adapter.STYLE_LABEL_8_WINDOWS, true );
                hasContent = true;
                //                     Adapter.GET_SKIN().label.alignment = al;
                //                     Adapter.GET_SKIN().label.fontSize = fs;
            }
            
            //var bg = Adapter.GET_SKIN().button.active.background;
            // Adapter.GET_SKIN().button.active.background = Hierarchy.GetIcon("BUT");
            
            
            Draw_ModuleButton( drawRect, null, BUTTON_ACTION_HASH, hasContent );
            /* if ( adapter.ModuleButton( drawRect, null, hasContent ) && !adapter.IsSceneHaveToSave( _o ) )
             {
             }*/
            
            // Adapter.GET_SKIN().button.active.background = bg;
            
            
            
            END_DRAW( _o );
            
            return width;
        }
        
        
        Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null;
        Adapter.DrawStackMethodsWrapper BUTTON_ACTION_HASH { get { return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION )); } }
        void BUTTON_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o)
        {
        
            if ( adapter.IsSceneHaveToSave( _o ) ) return;
            
            
            if ( Event.current.button == 0 )
            {
            
                var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_190_68, !callFromExternal(), adapter);
                
                //var pos = InputData.WidnwoRect( true, Event.current.mousePosition, 190, 68, adapter  );
                CREATE_NEW_ESCRIPTION( adapter, pos, _o, _o.scene, false );
            }
            
            if ( Event.current.button == 1 )
            {   var list = getDoubleList( _o.scene );
            
                Adapter.EventUse();
                /* int[] contentCost = new int[0];
                 GameObject[] obs = new GameObject[0];*/
                var o  = SetPair(_o);
                
                var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                
                if ( list.ContainsKey( o ) && !string.IsNullOrEmpty( list[o] ) )
                {   /* if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeaderFilltered(out obs, out contentCost, list[o]);
                         FillterData.Init(Event.current.mousePosition, SearchHelper, list[o] == "All" ? "All " : list[o], obs, contentCost, null, this);*/
                    
                    _W__SearchWindow.Init( mp, SearchHelper, list[o] == "All" ? "All " : list[o],
                                           CallHeaderFiltered( list[o] ),
                                           this, adapter, _o );
                                           
                }
                else
                {   /*  CallHeader(out obs, out contentCost);
                          FillterData.Init(Event.current.mousePosition, SearchHelper, "All", obs, contentCost, null, this);*/
                    
                    _W__SearchWindow.Init( mp, SearchHelper, "All",
                                           CallHeader(),
                                           this, adapter, _o );
                }
                
                // EditorGUIUtility.ic
            }
        }
        
        
        
        /** CALL HEADER */
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   Action<_W__SearchWindow.FillterData_Inputs> updateCache = (_W__SearchWindow.FillterData_Inputs input) =>
            {   dictionary_cache.Clear();
                dictionary_cacheSorted.Clear();
                
                if (adapter.IS_HIERARCHY())
                {   for (int i = 0 ; i < SceneManager.sceneCount ; i++)
                    {   var s = SceneManager.GetSceneAt( i );
                        if (!s.IsValid() || !s.isLoaded) continue;
                        var g = getDoubleList( s.GetHashCode() );
                        foreach (var item in g)
                        {   if (!dictionary_cache.ContainsKey( item.Key )) dictionary_cache.Add( item.Key, item.Value );
                        }
                        dictionary_cacheSorted.listKeys.AddRange( g.listKeys );
                        dictionary_cacheSorted.listValues.AddRange( g.listValues );
                    }
                }
                else
                {   var g = getDoubleList( -1 );
                    foreach (var item in g)
                    {   if (!dictionary_cache.ContainsKey( item.Key )) dictionary_cache.Add( item.Key, item.Value );
                    }
                    dictionary_cacheSorted.listKeys.AddRange( g.listKeys );
                    dictionary_cacheSorted.listValues.AddRange( g.listValues );
                }
                
#pragma warning disable
                var tr = new HierarchyObject[dictionary_cacheSorted.listKeys.Count];
                for (int i = 0 ; i < dictionary_cacheSorted.listKeys.Count ; i++)
                    tr[i] = adapter.GetHierarchyObjectByPair( ref dictionary_cacheSorted.listKeys, i );
#pragma warning restore
                    
                /*  input.analizedObjects = tr.ToList();
                  input.analizeEnumerator = null;
                  input.SKIP_SKANNING = true;*/
            };
            
            
            var result = new _W__SearchWindow.FillterData_Inputs( callFromExternal_objects )
            {   analizeEnumerator = null,
                    Valudator = o => dictionary_cache.ContainsKey( SetPair(o) ) && !string.IsNullOrEmpty( dictionary_cache[SetPair(o)] ),
                    SelectCompareString = ( o, i ) => dictionary_cache.ContainsKey(SetPair(o)) ? dictionary_cache[SetPair(o)] : "",
                    SelectCompareCostInt = ( d, i ) =>
                {   var cost = i;
                    if (d.go) cost += d.go.activeInHierarchy ? 0 : 100000000;
                    return cost;
                },
            };
            
            // updateCache(result);
            result.UpdateCache = updateCache;
            
            //  result.analizedObjects = dictionary_cacheSorted.listKeys.Select( g => adapter.GetHierarchyObjectByPair( g ) ).ToList();
            
            
            return result;
        }
        
        internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered( string filter )
        {   var result = CallHeader();
            result.Valudator = o =>
            {   if ( !dictionary_cache.ContainsKey( SetPair( o ) ) ) return false;
                return !string.IsNullOrEmpty( dictionary_cache[SetPair( o )] ) && String.Equals( filter, dictionary_cache[SetPair( o )], StringComparison.CurrentCultureIgnoreCase );
            };
            return result;
        }
        /** CALL HEADER */
        
        
        Dictionary<GoGuidPair, string> dictionary_cache = new Dictionary<GoGuidPair, string>();
        DoubleList<GoGuidPair, string> dictionary_cacheSorted = new DoubleList<GoGuidPair, string>();
        
    }
}
}
