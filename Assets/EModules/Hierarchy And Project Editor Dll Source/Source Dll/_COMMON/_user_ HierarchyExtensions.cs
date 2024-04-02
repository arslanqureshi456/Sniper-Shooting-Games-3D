#if UNITY_EDITOR
    #define HIERARCHY
    #define PROJECT
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using EModules;

#if HIERARCHY
    using EModules.EModulesInternal;
#endif
using UnityEditor;
using UnityEngine;
#if PROJECT && !UNITY_EDITOR
    using EModules.Project;
#endif

#if HIERARCHY
namespace HierarchyExtensions {
public class Utilities {



    public class TopMenuItems {
    
        #if HIERARCHY
        public static void hipergrapgh()
        {   Adapter.hipergrapgh();
        }
        public static bool hipergrapghvalidate()
        {   if ( Adapter.HierAdapter == null ) return false;
            return Adapter.hipergrapghvalidate();
        }
        
        public static void MoveSelPrev_2() { Adapter.MoveSelPrev( Adapter.HierAdapter ); }
        public static void MoveSelNext_2() { Adapter.MoveSelNext( Adapter.HierAdapter ); }
        
        public static void ToggleFreeze()
        {   Adapter.ToggleFreeze();
        }
        public static void UnlockAll()
        {   Adapter.UnclockALl();
        }
        #endif
        
        
        public static void ShowSettings()
        {   Adapter.SHOW_HIER_SETTINGS_DEFAULT(0);
        }
    }
    
    
    
    
    
    
    
    
    public static void RegistrateDescription( IDescriptionRegistrator o )
    {   if ( o == null || !o.gameObject || !o.gameObject.scene.IsValid() || !o.gameObject.scene.isLoaded ) return;
        reg( o );
    }
    
    static Action<EditorWindow, int, bool> s = null;
    static Action d = null;
    static Adapter adapter;
    static Action<IDescriptionRegistrator> reg = null;
    
    
    public class GetComponentFast<T> where T : Component {
        static  T tc;
        static  T[] tcarray;
        //static   Dictionary<Component , T> td;
        static Dictionary<int, T> cache = new Dictionary<int,  T>();
        static Dictionary<int, T[]> cachearray = new Dictionary<int,  T[]>();
        static bool wasInit = false;
        public static T Get( GameObject go )
        {   if ( !wasInit )
            {   wasInit = true;
                Adapter.M_Colors.additionalClear += Clear;
            }
            if ( !Adapter.HierAdapter.NEW_PERFOMANCE ) return go.GetComponent<T>();
            /*  td = new Dictionary<Component , T>();
              if ( !cache.TryGetValue( id , out td ) ) cache.Add( id , value: () );*/
            //id = go.GetInstanceID();
            if ( !cache.TryGetValue( go.GetInstanceID(), out tc ) ) cache.Add( go.GetInstanceID(), tc = go.GetComponent<T>() );
            
            return tc;
        }
        
        static void Clear()
        {   cache.Clear();
            cachearray.Clear();
        }
        
        
        public static T[] GetAll( GameObject go )
        {   if ( !wasInit )
            {   wasInit = true;
                Adapter.M_Colors.additionalClear += Clear;
            }
            if ( !Adapter.HierAdapter.NEW_PERFOMANCE ) return go.GetComponents<T>();
            /*  td = new Dictionary<Component , T>();
              if ( !cache.TryGetValue( id , out td ) ) cache.Add( id , value: () );*/
            //id = go.GetInstanceID();
            if ( !cachearray.TryGetValue( go.GetInstanceID(), out tcarray ) ) cachearray.Add( go.GetInstanceID(), tcarray = go.GetComponents<T>() );
            
            return tcarray;
        }
    }
    
    
    internal static void InitializeUtilities( object data, Adapter _adapter )
    {
    
        var l = data as IList;
        if ( l == null ) return;
        /* if (!(l[0] is string)) return;
         var s1 = (string) l[0];*/
        adapter = _adapter;
        s = l[2] as Action<EditorWindow, int, bool>;
        d = l[3] as Action;
        reg = l[4] as Action<IDescriptionRegistrator>;
        #if UNITY_EDITOR
        if ( d == null ) Debug.LogError( "error InitializeUtilities" );
        #endif
    }
    
    
    
    public static GameObject[] GetAffectsGameObjects( GameObject go )
    {   if ( !go ) return new GameObject[0];
        var sel = Selection.gameObjects.Where(g => g.scene.IsValid()).ToArray();
        if ( sel.Contains( go ) ) return sel;
        return new[] { go };
    }
    
    
    public static void DuplicateSelection()
    {   if ( Selection.gameObjects.Count( s => s.scene.IsValid() ) == 0 ) return;
        d();
    }
    
    public static void SetExpandedRecursive( int instanceId, bool isExpand )      // EditorApplication.ExecuteMenuItem( "Window/Hierarchy" );
    {   // var window = EditorWindow.focusedWindow;
        if ( adapter.window() == null ) return;
        var methodInfo = adapter. window().GetType().GetMethod("SetExpandedRecursive");
        methodInfo.Invoke( adapter.window(), new object[] { instanceId, isExpand } );
    }
    
    
    public static void SetExpanded( int instanceId, bool isExpand )      //EditorApplication.ExecuteMenuItem( "Window/Hierarchy" );
    {   //var window = EditorWindow.focusedWindow;
        if ( adapter.window() == null || s == null ) return;
        s( adapter.window(), instanceId, isExpand );
    }
}


}
#endif

#if PROJECT
namespace ProjectExtensions {
public class Utility {
    static Action<EditorWindow, int, bool> s = null;
    static Action<EditorWindow, int, bool> expWithChild = null;
    static Adapter adapter;
    
    internal static void InitializeUtilities( object data, Adapter _adapter )
    {   var l = data as IList;
        if ( l == null ) return;
        adapter = _adapter;
        s = l[2] as Action<EditorWindow, int, bool>;
        expWithChild = l[5] as Action<EditorWindow, int, bool>;
        //s = l[0] as Action<EditorWindow, int, bool>;
    }
    
    
    public static void SetExpandedRecursiveInProjectWindow( int instanceId, bool isExpand )      // EditorApplication.ExecuteMenuItem( "Window/Project" );
    {   // var window = EditorWindow.focusedWindow;
        if ( adapter.window() == null || s == null ) return;
        expWithChild( adapter.window(), instanceId, isExpand );
    }
    
    
    public static void SetExpandedInProjectWindow( int instanceId, bool isExpand )      // EditorApplication.ExecuteMenuItem( "Window/Project" );
    {   // var window = EditorWindow.focusedWindow;
        if ( adapter.window() == null || s == null ) return;
        s( adapter.window(), instanceId, isExpand );
    }
    
    
    public static int[] GetAffectsGameObjects( int instanceId )
    {   var _o = EModules.EProjectInternal.Project.adapter.GetHierarchyObjectByInstanceID( instanceId );
    
        if ( EModules.EProjectInternal.Project.adapter.SELECTED_GAMEOBJECTS().All( selO => selO != _o ) )
        {   return new[] { _o.id };
        }
        
        return EModules.EProjectInternal.Project.adapter.SELECTED_GAMEOBJECTS().Select( s => s.id ).ToArray();
    }
    
    
}
}
#endif
