#if UNITY_EDITOR
    #define HIERARCHY
    #define PROJECT
#endif




using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
#if PROJECT
    using EModules.Project;
#endif
//namespace EModules


namespace EModules.EModulesInternal


{




internal partial class Adapter {


    ////////////////////////////////////////////////////////////
    ///////////////* SETTINGS ITEM *//////////////
    ////////////////////////////////////////////////////////////
    
    //public  const string dir =  "Window";
    //public  const string dir = "Hierarchy";
    public  const string dir = "Window/Hierarchy PRO";
    internal const int P = 10000;
    // internal const int P = 2000;
    
    
    #if UNITY_EDITOR
    [MenuItem( dir + "/Project Plugin/Settings", false, P + 3 )]
    static void NewSetWinProj()
    {   SHOW_HIER_SETTINGS_DEFAULT( 1 );
    }
    
    #endif
    
    ////////////////////////////////////////////////////////////
    ///////////////* SELECTIONS *//////////////
    ////////////////////////////////////////////////////////////
    
    #if HIERARCHY
    //[MenuItem( "Window/Hierarchy Plugin/Selection Backward %#z", false, _S___Project_GUIEditorWindow.P + 6 )]
    public static void MoveSelPrev_2() { MoveSelPrev( HierAdapter ); }
    #endif
    #if PROJECT
    #if UNITY_2018_3_OR_NEWER
    [MenuItem( dir + "/Project Plugin/Selection Backward", false, P + 26 )]
    #else
    [MenuItem( dir + "/Project Plugin/Selection Backward", false,  P + 6 )]
    #endif
    public static void MoveSelPrev_1() { MoveSelPrev( ProjAdapter ); }
    #endif
    
    public static void MoveSelPrev( Adapter adapter )
    {
    
        if ( adapter.bottomInterface.LastIndex == adapter.bottomInterface.m_memCache[BottomInterface.MemType.Last][0].Count - 1
                || adapter.bottomInterface.m_memCache[BottomInterface.MemType.Last][0].Count == 0 ) return;
        //  if (LastIndex == -1) LastIndex = 0;
        Select( +1, adapter );
    }
    
    
    #if HIERARCHY
    //  [MenuItem( "Window/Hierarchy Plugin/Selection Forward %#y", false, P + 7 )]
    public static void MoveSelNext_2() { MoveSelNext( HierAdapter ); }
    #endif
    #if PROJECT
    #if UNITY_2018_3_OR_NEWER
    [MenuItem(dir + "/Project Plugin/Selection Forward", false, P + 27 )]
    #else
    [MenuItem(dir + "/Project Plugin/Selection Forward", false, P + 7 )]
    #endif
    
    public static void MoveSelNext_1() { MoveSelNext( ProjAdapter ); }
    #endif
    public static void MoveSelNext( Adapter adapter )
    {   if ( adapter.bottomInterface.LastIndex < 1 ) return;
        Select( -1, adapter );
    }
    
    
    static void Select( int offset, Adapter adapter )
    {   if ( !adapter.bottomInterface.m_memCache.ContainsKey( BottomInterface.MemType.Last ) || adapter.bottomInterface.m_memCache[BottomInterface.MemType.Last] == null
                || adapter.bottomInterface.m_memCache[BottomInterface.MemType.Last][0].Count == 0 ) return;
        var estimIndex = adapter.bottomInterface.LastIndex;
        do
        {   estimIndex += offset;
            if ( estimIndex < 0 || estimIndex >= adapter.bottomInterface.m_memCache[BottomInterface.MemType.Last][0].Count ) return;
            if ( adapter.bottomInterface.m_memCache[BottomInterface.MemType.Last][0][estimIndex] != null )
            {   if ( !adapter.bottomInterface.m_memCache[BottomInterface.MemType.Last][0][estimIndex].IsValid() ) continue;
                if ( !adapter.bottomInterface.m_memCache[BottomInterface.MemType.Last][0][estimIndex].OnClick( true, adapter.LastActiveScene ) )
                {   adapter.RepaintWindow(true);
                
                }
            }
            {   adapter.bottomInterface.LastIndex = estimIndex;
                break;
            }
        } while ( true );
    }
    
    
    
    
    ////////////////////////////////////////////////////////////
    ///////////////* SIBLING *//////////////
    ////////////////////////////////////////////////////////////
    
    #if HIERARCHY
    
    
    class SiblingChanger {
    
        const int MP = 65;
        
        
        /*  [MenuItem("Window/Hierarchy Plugin/Set Previous Sibling Index &UP", true, MP)]
          [MenuItem("Window/Hierarchy Plugin/Set Next Sibling Index &DOWN", true, MP)]
          [MenuItem("Window/Hierarchy Plugin/Set As First Sibling &LEFT", true, MP)]
          [MenuItem("Window/Hierarchy Plugin/Set As Last Sibling &RIGHT", true, MP)]
          public static bool SiblingPrevV()
          {
              return Selection.transforms.Length != 0;
          }*/
        
        /*
        [MenuItem( "Window/Hierarchy Plugin/Set Previous Sibling Index &%UP", false, Hierarchy_GUIEditorWindow.P +  MP )]
        public static void SiblingPrev( )
        {   var obs = Selection.gameObjects.Select(g => g.transform).ToArray();
            if ( obs.Length == 0 ) return;
            obs = obs.OrderBy( o => o.GetSiblingIndex() ).ToArray();
            List<Transform> moveBack = new List<Transform>();
            foreach ( var item in obs.Select( o => new { sib = o.GetSiblingIndex(), transform = o } ).ToArray() )
            {   var sib = item.sib - 1;
                Undo.SetTransformParent( item.transform, item.transform.parent, "Set Previous Sibling Index" );
                if ( sib < 0 ) moveBack.Add( item.transform );
                item.transform.SetSiblingIndex( sib );
            }
            foreach ( var transform in moveBack )
            {   transform.SetAsFirstSibling();
            }
        }
        [MenuItem( "Window/Hierarchy Plugin/Set Next Sibling Index &DOWN", false, Hierarchy_GUIEditorWindow.P + MP )]
        public static void SiblingNext( )
        {   var obs = Selection.gameObjects.Select(g => g.transform).ToArray();
            if ( obs.Length == 0 ) return;
            obs = obs.OrderByDescending( o => o.GetSiblingIndex() ).ToArray();
            List<Transform> moveBack = new List<Transform>();
            foreach ( var item in obs.Select( o => new { sib = o.GetSiblingIndex(), transform = o } ).ToArray() )
            {   var sib = item.sib + 1;
                Undo.SetTransformParent( item.transform, item.transform.parent, "Set Next Sibling Index" );
                var nned = sib;
                item.transform.SetSiblingIndex( sib );
                if ( nned != item.transform.GetSiblingIndex() ) moveBack.Add( item.transform );
            }
            foreach ( var transform in moveBack )
            {   transform.SetAsLastSibling();
            }
        }
        
        [MenuItem( "Window/Hierarchy Plugin/Set As First Sibling &LEFT", false, Hierarchy_GUIEditorWindow.P +  MP )]
        public static void SiblingFirst( )
        {   var obs = Selection.gameObjects.Select(g => g.transform).ToArray();
            if ( obs.Length == 0 ) return;
            obs = obs.OrderByDescending( o => o.GetSiblingIndex() ).ToArray();
            foreach ( var item in obs )
            {   Undo.SetTransformParent( item, item.parent, "Set As First Sibling" );
                item.SetAsFirstSibling();
            }
        }
        
        [MenuItem( "Window/Hierarchy Plugin/Set As Last Sibling &RIGHT", false, Hierarchy_GUIEditorWindow.P +  MP )]
        public static void SiblingLast( )
        {   var obs = Selection.gameObjects.Select(g => g.transform).ToArray();
            if ( obs.Length == 0 ) return;
            obs = obs.OrderBy( o => o.GetSiblingIndex() ).ToArray();
            foreach ( var item in obs )
            {   Undo.SetTransformParent( item, item.parent, "Set As Last Sibling" );
                item.SetAsLastSibling();
            }
        }*/
    }
    #endif
    
    
    
    
    
    
    
    ////////////////////////////////////////////////////////////
    ///////////////* LOCK *//////////////
    ////////////////////////////////////////////////////////////
    #if HIERARCHY
    
    //[MenuItem( "Window/Hierarchy Plugin/Toggle Lock State &#l", false, _S___Hierarchy_GUIEditorWindow.P + 85 )]
    public static void ToggleFreeze()
    {
    
        var obs = HierAdapter.SELECTED_GAMEOBJECTS();
        if ( obs.Length == 0 ) return;
        // var top = obs.Where(g => g.GetComponentsInParent<Transform>(true).Count(p => obs.Contains(p.gameObject)) == 1).Select(g => g.gameObject).ToArray();
        var top = Utilities.GetOnlyTopObjects( obs, Hierarchy.HierarchyAdapterInstance );
        foreach ( var item in top )
        {   var targetO = item.go;
        
            /*var old = o.hideFlags & HideFlags.NotEditable;
            if ( old != 0 ) {
                o.hideFlags &= ~old;
            } else {
                o.hideFlags |= HideFlags.NotEditable;
            }*/
            
            var old = targetO.hideFlags & HideFlags.NotEditable;
            var asd = Initializator.AdaptersByID[0].modules.FirstOrDefault( m => m is EModules.EModulesInternal.Hierarchy.M_Freeze );
            if ( asd != null )
            {   var fr = asd as Hierarchy.M_Freeze;
                var checkValue = targetO.hideFlags;
                if ( old != 0 ) targetO.hideFlags &= ~old;
                else targetO.hideFlags |= HideFlags.NotEditable;
                if ( checkValue != targetO.hideFlags )
                {   if ( (targetO.hideFlags & HideFlags.NotEditable) != 0 )
                        fr.getDoubleList( targetO.scene.GetHashCode() ).SetByKey( new GoGuidPair() { go = targetO.gameObject }, true );
                    else
                        fr.getDoubleList( targetO.scene.GetHashCode() ).RemoveAll( new GoGuidPair() { go = targetO.gameObject } );
                        
                    fr.SetLockToggle( EditorSceneManager.GetActiveScene().GetHashCode() );
                }
                
                
                var stateForDrag = targetO.hideFlags;
                
                foreach ( var VARIABLE in targetO.GetComponentsInChildren<Transform>( true ) )
                    VARIABLE.gameObject.hideFlags = stateForDrag & HideFlags.NotEditable | VARIABLE.gameObject.hideFlags & ~HideFlags.NotEditable;
            }
            
        }
        HierAdapter.RepaintAllViews();
    }
    //  [MenuItem( "Window/Hierarchy Plugin/Unlock All &%#l", false, _S___Hierarchy_GUIEditorWindow.P + 89 )]
    public static void UnclockALl()
    {   /* var obs = Selection.gameObjects;
             if (obs.Length == 0) return;*/
        var asd = Initializator.AdaptersByID[0].modules.FirstOrDefault( m => m is EModules.EModulesInternal.Hierarchy.M_Freeze );
        if ( asd != null )
        {   var fr = asd as Hierarchy.M_Freeze;
            var list = fr.getDoubleList( EditorSceneManager.GetActiveScene().GetHashCode() );
            for ( int i = 0 ; i < list.Count ; i++ )
            {   var p  = list[i].Key;
                var o = Initializator.AdaptersByID[0].GetHierarchyObjectByPair(ref p);
                o.go.hideFlags &= ~HideFlags.NotEditable;
                o.go.transform.hideFlags &= ~HideFlags.NotEditable;
            }
            list.Clear();
            fr.SetLockToggle( EditorSceneManager.GetActiveScene().GetHashCode() );
        }
        /*     foreach ( var item in Resources.FindObjectsOfTypeAll<Transform>() )
             {   item.gameObject.hideFlags &= ~HideFlags.NotEditable;
             }*/
        HierAdapter.RepaintAllViews();
    }
    
    #endif
    
    
    
    
    ////////////////////////////////////////////////////////////
    ///////////////* PRESETS FOR HIERARCHY OBJECTS AND COMPONENTS *//////////////
    ////////////////////////////////////////////////////////////
    
    internal static Adapter static_adapter
    {   get
        {
            #if !HIERARCHY
            return null;
            #else
            return Hierarchy.HierarchyAdapterInstance;
            #endif
        }
    }
    
    
    const int POS = 150;
    /*  #if HIERARCHY
      [MenuItem( "Window/Hierarchy Plugin/Quickly Create a Preset in Last Set &#s", true, 2030 )]
      [MenuItem( "CONTEXT/Component/Quickly Create a Preset in Last Set &#s", true, POS )]
      public static bool INTERNAL_STATIC_VALID(MenuCommand menuCommand)
      {   if (!static_adapter.par.DataKeeperParams.ENABLE) return false;
          / *  foreach (var gameObject in Hierarchy.SELECTED_GAMEOBJECTS())
            {
                if (!gameObject.scene.IsValid()) continue;
                var getted = DataKeeperCache.GetValue(gameObject.scene, gameObject);
                if (getted != null && getted.list.Count > 0 && getted.list[0] == 1) return true;
            }* /
    
          GameObject o = null;
          var comp = menuCommand.context as Component;
          if (comp) o = comp.gameObject;
          else
          {   if (Selection.gameObjects.Length == 0) return false;
              o = Selection.gameObjects[0];
          }
          return IconData.CHECK_ADD_PRESET_TOLAST( o, static_adapter );
      }
    
    
      [MenuItem( "Window/Hierarchy Plugin/Quickly Create a Preset in Last Set &#s", false, 2030 )]
      [MenuItem( "CONTEXT/Component/Quickly Create a Preset in Last Set &#s", false, POS )]
      public static void INTERNAL_STATIC(MenuCommand menuCommand)
      {   GameObject o = null;
          var comp = menuCommand.context as Component;
          if (comp) o = comp.gameObject;
          else
          {   if (Selection.gameObjects.Length == 0) return;
              o = Selection.gameObjects[0];
          }
          IconData. TRY_ADD_PRESET_TOLAST( o, static_adapter );
          static_adapter.RepaintWindowInUpdate();
      }
      #endif
    */
    
    
    
    
    
    
    
    
    
    
    
    
    ////////////////////////////////////////////////////////////
    ///////////////* BOTTOM INTERFACE *//////////////
    ////////////////////////////////////////////////////////////
    
    #if PROJECT
    [MenuItem( dir + "/Project Plugin/Toggle BookMarks Manager State %#m" + (Adapter.LITE ? " (Pro Only)" : ""), false,
               _S___Project_GUIEditorWindow.P + 45 )]
    public static void projectgrapgh()
    {   ProjAdapter.bottomInterface.favorGraph.SWITCH_ACTIVE();
    }
    #endif
    
    #if HIERARCHY
    //  [MenuItem( "Window/Hierarchy Plugin/Toggle HyperGraph State %#x" + (Adapter.LITE ? " (Pro Only)" : ""), false, _S___Hierarchy_GUIEditorWindow.P + 45 )]
    public static void hipergrapgh()
    {   HierAdapter.bottomInterface.hyperGraph.SWITCH_ACTIVE();
    }
    #endif
    
    
    #if PROJECT
    [MenuItem( dir + "/Project Plugin/Toggle BookMarks Manager State %#m" + (Adapter.LITE ? " (Pro Only)" : ""), true,
               _S___Project_GUIEditorWindow.P + 45 )]
    #endif
    public static bool projgrapghvalidate()
    {   if ( ProjAdapter == null ) return false;
        return ProjAdapter.ENABLE_BOTTOMDOCK_PROPERTY && !Adapter.LITE;
    }
    #if HIERARCHY
    // [MenuItem( "Window/Hierarchy Plugin/Toggle HyperGraph State %#x" + (Adapter.LITE ? " (Pro Only)" : ""), true, _S___Hierarchy_GUIEditorWindow.P + 45 )]
    #endif
    public static bool hipergrapghvalidate()
    {   if ( HierAdapter == null ) return false;
        return HierAdapter.ENABLE_BOTTOMDOCK_PROPERTY && !Adapter.LITE;
    }
    
    
    
}
}
