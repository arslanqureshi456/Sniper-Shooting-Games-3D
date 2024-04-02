#if UNITY_EDITOR
    #define PROJECT
    #define HIERARCHY
#endif

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PROJECT
    using EModules.Project;
#endif
//namespace EModules



namespace EModules.EModulesInternal {
internal class FakeClass {

}
}

namespace EModules.EProjectInternal {
internal class FakeClass {

}
}



namespace EModules.EModulesInternal

{
internal class Utilities {




    /* internal static int GetLocalIdentifierInFile(int id)
     {
       //  return Unsupported.GetLocalIdentifierInFile( id );
       var gameObject = EditorUtility.InstanceIDToObject( id );
    
    
    
       return GetLocalIdentifierInFile( gameObject );
     }*/
    
    
    
    
    
    
    
    
    //EditorWindow
    internal static Adapter.HierarchyObject[] GetOnlyTopObjects( Adapter.HierarchyObject[] affectedObjectsArray, Adapter adapter )
    {   if ( adapter.pluginID == Initializator.HIERARCHY_ID )
        {   /*var convertedObject = affectedObjectsArray.Select(o => o.go);
                return convertedObject.
                       Where( g => g.GetComponentsInParent<Transform>( true ).Where( p => p != g.transform ).Count( p => convertedObject.Contains( p.gameObject ) ) == 0 ).
                       Select( g => g.gameObject ).Select( adapter.GetHierarchyObjectByInstanceID ).ToArray();
            */
            
            var converted = affectedObjectsArray.Select(a => new { a, par = a.go.GetComponentsInParent<Transform>( true ).Where(p => p != a.go.transform) } );
            return
                converted.Where( c => c.par.Count( p => affectedObjectsArray.Any( a => a.go == p.gameObject ) ) == 0 ).
                Select( g => g.a ).ToArray();
        }
        else
        {
        
        }
        
        return affectedObjectsArray.
               Where( g => g.GetAllParents( adapter ).Count( p => affectedObjectsArray.Contains( p ) ) == 0 ).ToArray();
    }
    
    internal static void Swap<T>( ref List<T> list, int ind1, int ind2 )
    {   var captureMemory = list[ind1];
        var target = list[ind2];
        
        list.RemoveAt( ind1 );
        if ( ind1 >= list.Count ) list.Add( target );
        else list.Insert( ind1, target );
        
        list.RemoveAt( ind2 );
        if ( ind2 >= list.Count ) list.Add( captureMemory );
        else list.Insert( ind2, captureMemory );
    }
    
    internal static void MoveFromTo<T>( ref List<T> list, int from, int to )
    {   var target = list[from];
    
        list.RemoveAt( from );
        
        if ( to >= list.Count ) list.Add( target );
        else list.Insert( to, target );
    }
    
    static    Dictionary<Texture, bool> _IsPrefabIcon = new Dictionary<Texture, bool>();
    internal static bool IsPrefabIcon( Texture t )
    {   if ( !_IsPrefabIcon.ContainsKey( t ) )
        {   _IsPrefabIcon.Add( t, !AssetDatabase.GetAssetPath( t ).StartsWith( "Assets" )
                               /*t.name == "Prefab Icon"
                               || t.name == "PrefabNormal Icon"
                               || t.name == "PrefabModel Icon"*/);
        }
        return (_IsPrefabIcon[t]);
    }
    
    internal  static Dictionary<Type, Adapter.TempColorClass> ObjectContent_cache = new Dictionary<Type, Adapter.TempColorClass>();
    internal  static Dictionary<int, Adapter.TempColorClass> ObjectContent_Objectcache = new Dictionary<int, Adapter.TempColorClass>();
    internal static Dictionary<int, Dictionary<Type, Adapter.TempColorClass>> cache_ObjectContent_byType = new Dictionary<int, Dictionary<Type, Adapter.TempColorClass>>();
    internal static Dictionary<int, Dictionary<int, Adapter.TempColorClass>> cache_ObjectContent_byId = new Dictionary<int, Dictionary<int, Adapter.TempColorClass>>();
    internal   static  Dictionary<int, Adapter.TempColorClass> new_perfomance_includecaher_icons = new Dictionary<int, Adapter.TempColorClass>();
    
    // static GUIContent getContent = new GUIContent();
    
    /* internal static GUIContent ObjectContent(Adapter adapter, Adapter.HierarchyObject o, Type type)
     {
       if (adapter.IS_PROJECT())
       {
         getContent.image = AssetDatabase.GetCachedIcon( o.project.assetPath );
         return getContent;
       }
    
       var temoO  = EditorUtility.InstanceIDToObject(o.id);
       var result = ObjectContent(adapter, temoO, type );
    
       return result;
     }
     GUIContent c;*/
    //static GUIContent c222 = new GUIContent();
    static Type fakeType = typeof(int);
    //static bool skipAdd = false;
    // static  Adapter.TempColorClass __ObjectContent_IncludeCacherTempColor2 = new Adapter.TempColorClass();
    //static  Adapter.TempColorClass __ObjectContent_IncludeCacherTempColor = new Adapter.TempColorClass();
    static  Adapter.TempColorClass tempColorResult;
    internal static Adapter.TempColorClass ObjectContent_IncludeCacher( Adapter adapter, Adapter.HierarchyObject o, Type type )
    {   return ObjectContent_IncludeCacher( adapter, o, type, false );
    }
    internal static Adapter.TempColorClass ObjectContent_IncludeCacher( Adapter adapter, Adapter.HierarchyObject o, Type type, bool includePrefab )
    //  internal static GUIContent ObjectContent(Adapter adapter, UnityEngine.Object o, Type type)
    {
    
        if ( adapter.NEW_PERFOMANCE && new_perfomance_includecaher_icons.ContainsKey( o.id ) ) return new_perfomance_includecaher_icons[o.id];
        
        
        var cacher = adapter.ColorModule.GetImageForObject_OnlyCacher( o, true );
        if ( cacher.add_icon )
        {   /*  getContent.text = cacher.name;
                  getContent.tooltip = "";
                  getContent.image = cacher;*/
            //return __ObjectContent_IncludeCacherTempColor2.AddIcon(  cacher.add_icon, cacher.add_hasiconcolor, cacher.add_iconcolor);
            if ( adapter.NEW_PERFOMANCE )
            {   var result = new  Adapter.TempColorClass();
                cacher.OverrideTo( ref result );
                new_perfomance_includecaher_icons.Add( o.id, result );
            }
            
            return cacher;
        }
        
        
        
        
        o.cache_prefab = false;
        
        
        if ( adapter.IS_HIERARCHY() )     // HIERARCHY
        {
        
            tempColorResult = __internal_ObjectContent( adapter, o != null ? o.GetHardLoadObject() : null, type );
            
        }
        else       // PROJECT
        {   var id = o != null ? o.id : -1;
        
            var filtered2 = adapter.ColorModule.GetFilter(adapter, o);
            if ( filtered2 != null ) tempColorResult = filtered2;
            
            
            if ( !cache_ObjectContent_byId.ContainsKey( id ) ) cache_ObjectContent_byId.Add( id, new Dictionary<int, Adapter.TempColorClass>() );
            
            
            if ( !cache_ObjectContent_byId[id].ContainsKey( o.id ) )
            {   var icon = AssetDatabase.GetCachedIcon( o.project.assetPath );
            
                /*if (icon == null || string.IsNullOrEmpty( icon.name ))
                {   return __ObjectContent_IncludeCacherTempColor.AddIcon(icon);
                }*/
                cache_ObjectContent_byId[id].Add( o.id, new Adapter.TempColorClass().AddIcon( icon ) );
            }
            tempColorResult = cache_ObjectContent_byId[id][o.id];
            
            
        }
        
        //COLUP
        if ( tempColorResult.add_icon != null )     // Debug.Log(context.name);
        {   if ( tempColorResult.add_icon == adapter.NullContext )
            {   tempColorResult = adapter.__INTERNAL_TempColor_Empty;
            }
            else if ( adapter.IS_HIERARCHY() && Utilities.IsPrefabIcon( tempColorResult.add_icon ) )
            {   if ( adapter.par.SHOW_PREFAB_ICON && includePrefab )
                {   var prefab_root = adapter.FindPrefabRoot(o.go);
                    // var prefab_src = PrefabUtility.GetPrefabParent(prefab_root);
                    if ( prefab_root != o.go ) tempColorResult = adapter.__INTERNAL_TempColor_Empty;
                    else     // cache_prefab = true;
                    {   o.cache_prefab = true;
                    }
                }
                else
                {   tempColorResult = adapter.__INTERNAL_TempColor_Empty;
                }
            }
        }
        
        if ( !tempColorResult.add_icon ) //#COLUP
        {   var filtered = adapter.ColorModule.GetFilter(adapter, o);
            if ( filtered != null ) tempColorResult = filtered;
        }
        
        if ( adapter.NEW_PERFOMANCE )
        {   var result = new  Adapter.TempColorClass();
            result.el = new SingleList() { list = asdasd.ToList() };
            tempColorResult.OverrideTo( ref result );
            new_perfomance_includecaher_icons.Add( o.id, result );
        }
        
        return tempColorResult;
        
    }
    static int[] asdasd = new int[5];
    
    /*if (!cache_ObjectContent_byType[id].ContainsKey( type == null ? fakeType : type )) {
      skipAdd = false;
      if (adapter.IS_PROJECT()) {
        c.image = AssetDatabase.GetCachedIcon( o.project.assetPath );
        c.text = c.image.name;
        c.tooltip = "";
        if (!c.image) return null;
      }
      else c = __internal_ObjectContent( adapter, o != null ? o.GetHardLoadObject() : null, type );
    
      if (!skipAdd) cache_ObjectContent_byType[id].Add( type == null ? fakeType : type, new GUIContent( c.text, c.image, c.tooltip ) );
    }*/
    
    internal static Adapter.TempColorClass ObjectContent_NoCacher( Adapter adapter, UnityEngine.Object o, Type type )
    {   var id = o != null ? o.GetInstanceID() : -1;
        //  var id = o ? o.GetInstanceID() : -1;
        
        if ( !cache_ObjectContent_byType.ContainsKey( id ) ) cache_ObjectContent_byType.Add( id, new Dictionary<Type, Adapter.TempColorClass>() );
        
        if ( !cache_ObjectContent_byType[id].ContainsKey( type == null ? fakeType : type ) )
        {
        
        
            //# todo FIX
            //if (adapter.IS_PROJECT() && o) throw new Exception( "Cannot load project content by object" );
            
            var  c = __internal_ObjectContent( adapter, o, type );
            // if (c == null || !c.image || string.IsNullOrEmpty( c.image.name )) return new GUIContent( c );
            // if (adapter.IS_PROJECT() && !c.image) return null;
            cache_ObjectContent_byType[id].Add( type == null ? fakeType : type, c );
            // cache_ObjectContent_byType[id].Add( type == null ? fakeType : type, new GUIContent( c.text, c.image, c.tooltip ) );//#COLUP
        }
        
        if ( adapter.IS_HIERARCHY() ) return __internal_ObjectContent( adapter, o, type );
        
        return cache_ObjectContent_byType[id][type == null ? fakeType : type];
    }
    static  Adapter.TempColorClass __internal_ObjectContentTempColor = new Adapter.TempColorClass();
    internal static Adapter.TempColorClass __internal_ObjectContent( Adapter adapter, UnityEngine.Object o, Type type )
    {   if ( o == null )     //  return  new GUIContent(EditorGUIUtility.ObjectContent(null, type));
        {   if ( !ObjectContent_cache.ContainsKey( type ) )
            {
            
                var g = EditorGUIUtility.ObjectContent(null, type);
                //var result = new GUIContent(g.text, g.image, g.tooltip);
                var result = new Adapter.TempColorClass( ).AddIcon(g.image, g.text);
                ObjectContent_cache.Add( type, result );
            }
            var g2 = ObjectContent_cache[type];
            return g2;
            /* getContent.text = g2.text;
             getContent.tooltip = g2.tooltip;
             getContent.image = g2.image;
             return getContent;*/
        }
        else
        {
        
        
        
            if ( Application.isPlaying )     //var key = o.GetType().Name.GetHashCode();
            {   var key = o.GetInstanceID();
                if ( !ObjectContent_Objectcache.ContainsKey( key ) )
                {   var g = EditorGUIUtility.ObjectContent(o, type);
                    var result = new Adapter.TempColorClass( ).AddIcon(g.image);
                    // var result = new GUIContent(EditorGUIUtility.ObjectContent(o, type));
                    ObjectContent_Objectcache.Add( key, result );
                }
                
                var g2 = ObjectContent_Objectcache[key];
                return g2;
                /* getContent.text = g2.text;
                 getContent.tooltip = g2.tooltip;
                 getContent.image = g2.image;
                 return getContent;*/
            }
            else
            {   return __internal_ObjectContentTempColor.AddIcon( EditorGUIUtility.ObjectContent( o, type ).image );
            }
        }
    }
    
    
    internal static GameObject[] AllSceneObjects( Scene scene )
    {   list.Clear();
        if (scene.isLoaded && scene.IsValid())
            foreach ( var g in scene.GetRootGameObjects() ) WriteT( g.transform );
        return list.ToArray();
    }
    
    
    internal static GameObject[] AllSceneObjects()     //   EditorSceneManager.GetActiveScene();
    {   return Resources.FindObjectsOfTypeAll<Transform>().Select( t => t.gameObject ).Where( g => g.scene.IsValid() ).ToArray();
        /*  list.Clear();
          for (int i = 0; i < SceneManager.sceneCount; i++)
          {
              var s = SceneManager.GetSceneAt(i);
              if (!s.IsValid() || !s.isLoaded) continue;
              foreach (var g in s.GetRootGameObjects()) WriteT(g.transform);
          }
        
          // foreach (var g in EditorSceneManager.GetActiveScene().GetRootGameObjects()) WriteT(g.transform);
        
        
          return list.ToArray();*/
    }
    
    
    //  internal static HideFlags fillter = HideFlags.DontSave | HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy;
    static List<GameObject> list = new List<GameObject>();
    static void WriteT( Transform t )
    {   if ( !VALIDATE_FLAGS( t.gameObject.hideFlags ) )
        {
        
            return;
        }
        list.Add( t.gameObject );
        for ( int i = 0, len = t.childCount ; i < len ; i++ ) WriteT( t.GetChild( i ) );
    }
    
    
    //   static bool broadLaunch
    /*  internal static void BroadCastActionReverse(GameObject o, Action<GameObject> ac)
      {
          list.Clear();
          WriteT(o.transform);
          list.Reverse();
    
          foreach (var gameObject1 in list)
          {
              if ((gameObject1.hideFlags & fillter) != 0) return;
              Hierarchy.counter ++;
              ac(gameObject1);
             // for (int i = 0, len = gameObject1.transform.childCount; i < len; i++) BroadCastActionReverse(gameObject1.transform.GetChild(i).gameObject, ac);
          }
    
    
    
      }*/
    
    //static int lastIndex = 0;
    
    
    static bool CheckParentTransform( Transform current, Transform parent )
    {   var p = current;
        while ( p )
        {   if ( p == parent ) return true;
            p = p.parent;
        }
        return false;
    }
    
    
    
    internal static IEnumerable<Adapter.HierarchyObject> AllSceneObjectsInterator( Adapter adapter, bool GetInScenes = false, UnityEngine.Object activeParent = null,
            string extension = null )        // in this example we return less than N if necessary
    {
    
        if ( adapter == null ) yield break;
        
        GameObject activeGO = null;
        Transform activeTransform = null;
        bool haveActiveParentTransform = false;
        if ( adapter.IS_HIERARCHY() )
        {   activeGO = activeParent as GameObject;
            if ( activeGO ) activeTransform = activeGO.transform;
            haveActiveParentTransform = activeTransform;
        }
        
        string activePath = null;
        if ( adapter.IS_PROJECT() )
        {   if ( activeParent ) activePath = AssetDatabase.GetAssetPath( activeParent );
            haveActiveParentTransform = !string.IsNullOrEmpty( activePath );
            /*activeGO = activeParent as GameObject;
            if (activeGO) activeTransform = activeGO.transform;
            haveActiveParentTransform = activeTransform;*/
        }
        // Debug.Log((bool)activeParent + " " + (bool)activeGO);
        
        
        if ( adapter.pluginID == Initializator.HIERARCHY_ID || GetInScenes )
        {   for ( int index = SceneManager.sceneCount - 1 ; index >= 0 ; index-- )
            {   var s = SceneManager.GetSceneAt(index);
                if ( !s.IsValid() || !s.isLoaded ) continue;
                var source = s.GetRootGameObjects();
                
                
                List<Transform> O_T = new List<Transform>();
                List<int> O_index = new List<int>();
                
                // DoubleList<Transform, int> offsetter = new DoubleList<Transform, int>();
                
                for ( int sss = source.Length - 1 ; sss >= 0 ; sss-- )
                {   if ( !source[sss] ) continue;
                    O_T.Add( source[sss].transform );
                    O_index.Add( source[sss].transform.childCount - 1 );
                    
                    
                    var current_T = O_T[O_T.Count - 1];
                    var childIndex = O_index[O_index.Count - 1];
                    
                    do
                    {   adapter.lastScanIndex = source.Length - 1 - sss;
                        current_T = O_T[O_T.Count - 1];
                        childIndex = O_index[O_index.Count - 1];
                        
                        
                        if ( childIndex < 0 || !current_T || childIndex >= current_T.childCount )
                        {   if ( O_T.Count == 1 ) break;
                        
                            O_T.RemoveAt( O_T.Count - 1 );
                            O_index.RemoveAt( O_index.Count - 1 );
                            
                            if ( current_T && VALIDATE_FLAGS( current_T.gameObject.hideFlags ) && (!haveActiveParentTransform
                                    || CheckParentTransform( current_T, activeTransform )) ) yield return adapter.GetHierarchyObjectByInstanceID( current_T.gameObject );
                            continue;
                        }
                        
                        /* if (childIndex >= current_T.childCount)
                        {
                        O_index[O_index.Count - 1] = current_T.childCount - 1;
                        continue;
                        }*/
                        
                        
                        var child = current_T.GetChild(O_index[O_index.Count - 1]);
                        O_index[O_index.Count - 1] = O_index[O_index.Count - 1] - 1;
                        
                        if ( child.childCount == 0 )
                        {   if ( VALIDATE_FLAGS( current_T.gameObject.hideFlags ) && (!haveActiveParentTransform
                                    || CheckParentTransform( current_T, activeTransform )) ) yield return adapter.GetHierarchyObjectByInstanceID( child.gameObject );
                            continue;
                        }
                        
                        O_T.Add( child );
                        O_index.Add( child.childCount - 1 );
                        
                        
                    } while ( O_T.Count > 0 );
                    
                    adapter.lastScanIndex = source.Length - 1 - sss;
                    if ( O_T[0] && VALIDATE_FLAGS( O_T[0].gameObject.hideFlags ) && (!haveActiveParentTransform
                            || CheckParentTransform( O_T[0], activeTransform )) ) yield return adapter.GetHierarchyObjectByInstanceID( O_T[0].gameObject );
                            
                    //if (O_T[0] && (O_T[0].gameObject.hideFlags & fillter) == 0 && (!haveActiveParentTransform || CheckParentTransform( O_T[0], activeTransform ))) yield return adapter.GetHierarchyObjectByInstanceID( O_T[0].gameObject );
                    O_T.Clear();
                    O_index.Clear();
                }
            }
        }
        
        if ( adapter.pluginID == Initializator.PROJECT_ID )     //  var LLL = Application.dataPath.Length - "Assets".Length;
        {   // var paths =  System.IO.Directory.GetFiles( Application.dataPath,"*.*",System.IO.SearchOption.AllDirectories ).Where(p=>!p.EndsWith(".meta")).Select(p=>p.Replace('\\','/').Substring(LLL)).ToArray();
            var paths = Adapter.ALL_ASSETS_PATHS;
            for ( int i = 0 ; i < paths.Length ; i++ )
            {   if ( !paths[i].StartsWith( "Assets" ) ) continue;
                if ( haveActiveParentTransform && !paths[i].StartsWith( activePath ) ) continue;
                if ( extension != null && !paths[i].EndsWith( extension ) ) yield return null;
                var path = AssetDatabase.AssetPathToGUID( paths[i] );
                if ( string.IsNullOrEmpty( path ) ) continue;
                var g = AssetDatabase.AssetPathToGUID( paths[i] );
                yield return adapter.GetHierarchyObjectByGUID( ref g, "" );
            }
        }
    }
    
    internal static HideFlags SearchFlags = HideFlags.HideInInspector;
    static bool VALIDATE_FLAGS( HideFlags flags )
    //  {   return (flags & Adapter.flagsSHOW) != Adapter.flagsSHOW;
    {   return (flags & SearchFlags) != SearchFlags;
    }
    
    /*    static IEnumerable<Transform> GetT(Transform t)
        {
            for (int i = t.childCount - 1; i >= 0; i--)
            {
                yield return GetT(t.GetChild(i));
            }
            yield return t;
            /*if ((t.gameObject.hideFlags & fillter) != 0) return;
    
            for (int i = 0, len = t.childCount; i < len; i++) WriteT(t.GetChild(i));#1#
        }*/
    
    internal static int AllSceneObjectsInteratorCount( Adapter adapter, bool GetInScenes = false )
    {   if ( adapter.pluginID == Initializator.PROJECT_ID && !GetInScenes ) return -1;
        int res = 0;
        for ( int i = 0 ; i < SceneManager.sceneCount ; i++ )
        {   var s = SceneManager.GetSceneAt(i);
            if ( !s.IsValid() || !s.isLoaded ) continue;
            res += s.GetRootGameObjects().Length;
        }
        return res;
    }
    
    internal static int AllSceneObjectsInteratorProgress( Adapter adapter )
    {   return adapter.lastScanIndex;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
}

