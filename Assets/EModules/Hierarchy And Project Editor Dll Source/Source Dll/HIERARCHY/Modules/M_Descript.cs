

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

namespace EModules.EModulesInternal {


internal partial class Hierarchy {


    internal partial class M_Descript : Adapter.M_DescriptionCommon {
    
        override internal void TrySaveCustomRegistrator<TValue>( Adapter.HierarchyObject o, TValue value, Adapter.CacherType type )
        {   static_TrySaveCustomRegistrator( o, value, type );
        }
        
        
        override internal void UpdateSwitchRegistratorEnable()
        {   UpdateSwitchRegistratorEnable( Hierarchy.HierarchyAdapterInstance );
        }
        
        
        
        
        
        public M_Descript( int restWidth, int sibbildPos, bool enable, Adapter adapter ) : base( restWidth, sibbildPos, enable, adapter )
        {   adapter.DescriptionModule = this;
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
    
    
    
    
    
    
    internal partial class M_Descript : Adapter.M_DescriptionCommon {
    
    
    
        ///////////////////////////  BASE  /////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////  BASE  /////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////  BASE  /////////////////////////////////////////////////////////////////////////////////
        
        
        static RegistratorCache ReadData( IDescriptionRegistrator reg )
        {   var o = reg.gameObject;
            if ( !o.gameObject.scene.IsValid() ) return null;
            
            try
            {   return Adapter.DESERIALIZE_SINGLE<RegistratorCache>( reg.cachedData );
            }
            catch
            {   return null;
            }
        }
        
        
        /*  internal GoGuidPair tempPair = new GoGuidPair();
          internal GoGuidPair SetPair(Adapter.HierarchyObject _o)
          {
              tempPair.go = _o.go;
              tempPair.guid = "";
              return tempPair;
          }*/
        
        
        
        internal override void RegistrateDescription( IDescriptionRegistrator reg, Adapter adapter )
        {   var o = Hierarchy.HierarchyAdapterInstance.GetHierarchyObjectByInstanceID( reg.gameObject );
            if ( Hierarchy.HierarchyAdapterInstance.DISABLE_DESCRIPTION( o ) ) return;
            
            RegistratorCache readData = ReadData(reg);
            
            
            if ( readData == null ) return;
            
            
            /////  DESCIPRION  ////////////////////////////////// READ
            var list = getDoubleList(o.scene);
            var d = des(o.scene);
            AddDescriptionToList( ref list, o, o.scene, readData.description, false );
            /////  ...  ///////////////////////////////////////////////////////////////////////////////////////////////////
            
            
            
            /////  HIGLIGHTER  ///////////////////////////////// READ
            if ( readData.higlightcolor != null && readData.higlightcolor.Length > 0 || readData.texthiglightcolor != null && readData.texthiglightcolor.Length > 0 )
            {   var black1 = readData.higlightcolor.Length < 4 || readData.higlightcolor[0] == 0 && readData.higlightcolor[1] == 0 && readData.higlightcolor[2] == 0 && readData.higlightcolor[3] == 0;
                var color1 = !black1 ? new Color(readData.higlightcolor[0], readData.higlightcolor[1], readData.higlightcolor[2], readData.higlightcolor[3]) : Adapter.TRANSP_COLOR;
                var black2 = readData.texthiglightcolor.Length < 4 || readData.texthiglightcolor[0] == 0 && readData.texthiglightcolor[1] == 0 && readData.texthiglightcolor[2] == 0
                             && readData.texthiglightcolor[3] == 0;
                var color2 = !black2 ? new Color(readData.texthiglightcolor[0], readData.texthiglightcolor[1], readData.texthiglightcolor[2], readData.texthiglightcolor[3]) : Adapter.TRANSP_COLOR;
                if ( !black1 || !black2 )
                {   tempColor.Reset( new SingleList() { list = new int[9].ToList() } );
                    tempColor.BGCOLOR = (Color32)color1;
                    tempColor.LABELCOLOR = (Color32)color2;
                    tempColor.child = readData.applycolortoChilds;
                    adapter.ColorModule.SetHighlighterValue( tempColor, o.scene, o, false );
                }
                
            }
            
            /////  ...  ////////////////////////////////////////////////////////////////////////////////////////////////////
            
            
            
            /////  ICONCOLOR  ///////////////////////////////// READ
            if ( readData.iconColor != null && readData.iconColor.Length > 0 )
            {   var white = readData.iconColor.Length < 4 || readData.iconColor[0] == 1 && readData.iconColor[1] == 1 && readData.iconColor[2] == 1 && readData.iconColor[3] == 1;
                var black = readData.iconColor.Length < 4 || readData.iconColor[0] == 0 && readData.iconColor[1] == 0 && readData.iconColor[2] == 0 && readData.iconColor[3] == 0;
                if ( !white && !black ) adapter.ColorModule.IconColorCacher.SetValue( new SingleList( readData.iconColor ), o.scene, SetPair( o ), false );
            }
            /////  ...  ////////////////////////////////////////////////////////////////////////////////////////////////////
            
            
            
            if ( readData.keeperdata != null && readData.keeperdata.Count > 0 )
            {   M_PlayModeKeeper.DataKeeperCache.SetValue( new SingleList() { list = readData.keeperdata }, o.scene, o.go, false );
            }
            
            
            
            // //////////// FIND_ALL_CACHE /////////////
            
            
            if ( !Application.isPlaying )
            {   /* Hierarchy.SetDirty(d.component);
                 Hierarchy.SetDirty(d.gameObject);*/
                adapter.SetDirtyDescription( d, o.scene );
                
            }
            
            
            
        }
        
        internal override void TrySaveHiglighterRegistrator( Adapter.HierarchyObject o, Adapter.TempColorClass colors32 )
        {   if ( Hierarchy_GUI.Instance( adapter ).GETUSE_REGISTRATOR && o.go )
            {   RegistratorCache readData = InitCache(o.go);
                /***/
                if ( colors32 != null && (colors32.HAS_BG_COLOR || colors32.HAS_LABEL_COLOR) )//#tag SHOULD FIX OR REMOVE AT ALL // list > 9 dont save
                {   var color = (Color)colors32.BGCOLOR;
                    var color2 = (Color)colors32.LABELCOLOR;
                    readData.higlightcolor = new float[4] { color.r, color.g, color.b, color.a };
                    readData.texthiglightcolor = new float[4] { color2.r, color2.g, color2.b, color2.a };
                    readData.applycolortoChilds = colors32.child;
                }
                else
                {   readData.higlightcolor = new float[4] { 0, 0, 0, 0 };
                    readData.texthiglightcolor = new float[4] { 0, 0, 0, 0 };
                }
                /***/
                SaveCache( readData );
            }
        }
        
        
        static IDescriptionRegistrator reg;
        static bool tempDisable = false;
        static RegistratorCache InitCache( GameObject o )
        {   if ( tempDisable ) return new RegistratorCache();
            reg = o.GetComponents<Component>().FirstOrDefault( c => c is IDescriptionRegistrator ) as IDescriptionRegistrator;
            if ( reg == null )
            {   reg = o.AddComponent( DR ) as IDescriptionRegistrator;
                if ( reg == null )
                {   if ( Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).GETUSE_REGISTRATOR )
                    {   tempDisable = true;
                        Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).SaveToScriptableObject = "SCENE";
                        Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).GETUSE_REGISTRATOR = false;
                        Hierarchy.HierarchyAdapterInstance.SavePrefs();
                        Adapter.RequestScriptReload();
                        
                        EditorUtility.DisplayDialog( Hierarchy.HierarchyAdapterInstance.pluginname + " Plugin", "Cannot enable registrator 'DescriptionRegistrator' is inside Editor folder", "Ok" );
                    }
                    
                    return new RegistratorCache();
                }
                
                //MonoBehaviour.print( DR + " " + reg + "  " + reg);
                Adapter.SetDirty( o );
                Adapter.SetDirty( reg.component );
            }
            return ReadData( reg ) ?? new RegistratorCache();
        }
        
        static void SaveCache( RegistratorCache readData )
        {   if ( reg == null || !reg.component || reg.gameObject == null ) return;
            var go = reg.gameObject;
            
            /* MonoBehaviour.print(string.IsNullOrEmpty(readData.description) + " " +
                 readData.higlightcolor.All(d => d == 0) + " " +
                 readData.iconColor.All(d => d == 1) + " " +
                 readData.iconColor.All(d => d == 0));
             MonoBehaviour.print(readData.iconColor[0]);
            */
            if ( string.IsNullOrEmpty( readData.description ) &&
                    readData.texthiglightcolor.All( d => d == 0 ) &&
                    readData.higlightcolor.All( d => d == 0 ) &&
                    (readData.iconColor.All( d => d == 1 ) || readData.iconColor.All( d => d == 0 )) &&
                    (readData.keeperdata.Count == 0 || readData.keeperdata.All( k => k == 0 )) )
            {   UnityEngine.Object.DestroyImmediate( reg.component );
            
            }
            else
                reg.cachedData = Adapter.SERIALIZE_SINGLE( readData );
                
            EditorUtility.SetDirty( go );
        }
        
        
        
        internal static void TrySaveDescriptionRegistrator( GameObject o, string str )
        {
        
            if ( Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).GETUSE_REGISTRATOR && o )
            {   RegistratorCache readData = InitCache(o);
                /***/
                readData.description = str;
                /***/
                SaveCache( readData );
            }
        }
        
        internal static void static_TrySaveCustomRegistrator<T>( Adapter.HierarchyObject o, T value, Adapter.CacherType type )
        {   if ( Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).GETUSE_REGISTRATOR )
            {   RegistratorCache readData = InitCache(o.go);
                /***/
                var color = value as SingleList;
                switch ( type )
                {   case Adapter.CacherType.IconColor:
                        //  if (color != null) readData.iconColor = new float[4] { color.Value.r, color.Value.g, color.Value.b, color.Value.a };
                        if ( color != null && color.list != null && color.list.Count != 0 )
                        {   readData.iconColor = new float[4] { color.list[0] / 255f, color.list[1] / 255f, color.list[2] / 255f, color.list[3] / 255f };
                        }
                        else
                        {   readData.iconColor = new float[4] { 1, 1, 1, 1 };
                        }
                        break;
                    // //////////// FIND_ALL_CACHE /////////////
                    case Adapter.CacherType.KeeperData:
                        if ( color != null && color.list != null && color.list.Count != 0 )
                        {   readData.keeperdata = color.list.ToList();
                        }
                        else
                        {   readData.keeperdata = new List<int>();
                        }
                        break;
                        
                        // //////////// FIND_ALL_CACHE /////////////
                }
                
                
                SaveCache( readData );
            }
            /***/
        }
        
        
        
        Adapter.TempColorClass tempColor = new Adapter.TempColorClass();
        internal void UpdateSwitchRegistratorEnable( Adapter adapter )
        {   if ( Hierarchy.HierarchyAdapterInstance.DISABLE_DES() ) return;
        
            for ( int i = 0 ; i < SceneManager.sceneCount ; i++ )
            {   var s = SceneManager.GetSceneAt(i);
            
            
            
                if ( !s.isLoaded || !s.IsValid() ) continue;
                var d = des(s.GetHashCode());
                
                if ( d == null ) return;
                
                if ( d.EnableRegistrator != Hierarchy_GUI.Instance( adapter ).GETUSE_REGISTRATOR )
                {
                
                    d.EnableRegistrator = Hierarchy_GUI.Instance( adapter ).GETUSE_REGISTRATOR;
                    // var s = SceneManager.GetSceneAt(i);
                    var list = Utilities.AllSceneObjects(s);
                    //var list = getDoubleList(s);
                    if ( !Application.isPlaying )
                    {   /* Hierarchy.SetDirty(d.component);
                         Hierarchy.SetDirty(d.gameObject);*/
                        adapter.SetDirtyDescription( d, s );
                        
                    }
                    
                    if ( d.EnableRegistrator )
                    {   for ( int j = 0 ; j < list.Length ; j++ )
                        {   if ( !list[j] ) continue;
                        
                            //MonoBehaviour.print(list[j].Key.name + " " +list[j].Key.transform.root.name );
                            var __o = list[j];
                            var o = Hierarchy.HierarchyAdapterInstance.GetHierarchyObjectByInstanceID( __o.GetInstanceID() );
                            /** DESCRIPTION **/
                            var scene = s.GetHashCode();
                            if ( HasKey( scene, o ) )
                            {   TrySaveDescriptionRegistrator( __o, GetValue( scene, o ) );
                            }
                            
                            /** HIGLIGHTER **/
                            
                            if ( adapter.ColorModule.HighlighterHasKey( scene, o ).IsTrue( false ) )
                            {
                            
                            
                                var sl = adapter.ColorModule.GetHighlighterValue(scene, o);
                                if ( sl != null )
                                {   tempColor.AssignFromList( sl );
                                
                                
                                    adapter.DescriptionModule.TrySaveHiglighterRegistrator( o, tempColor );
                                }
                                
                                
                            }
                            
                            /** ICON COLOR **/
                            if ( adapter.ColorModule.IconColorCacher.HasKey( scene, o ) )
                            {   TrySaveCustomRegistrator < Color? >( o, adapter.ColorModule.ConverterSingle( adapter.ColorModule.IconColorCacher.GetValue( scene, o ) ), Adapter.CacherType.IconColor );
                            }
                            
                            
                            
                            if ( M_PlayModeKeeper.DataKeeperCache.HasKey( scene, o ) )
                            {   TrySaveCustomRegistrator( o, M_PlayModeKeeper.DataKeeperCache.GetValue( scene, o ), Adapter.CacherType.KeeperData );
                            }
                            // //////////// FIND_ALL_CACHE /////////////
                            
                        }
                    }
                    else
                    {   for ( int j = 0 ; j < list.Length ; j++ )
                        {   var o = list[j];
                            if ( !o ) continue;
                            var reg = HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll(o).FirstOrDefault(c => c is IDescriptionRegistrator) as IDescriptionRegistrator;
                            
                            // MonoBehaviour.print(reg.component);
                            if ( reg != null && reg.component ) UnityEngine.Object.DestroyImmediate( reg.component );
                        }
                    }
                }//if
                
                // Debug.Log( d.SaveInFolder + " " + (Hierarchy_GUI.Initialize( Hierarchy.adapter ).SaveToScriptableObject == "FOLDER") );
                if ( s.IsValid() )
                    adapter.CACHE_CKECK( d, s );
                    
            }
        }
        
        
        
        
        
        
    }
}

}










/*  internal override bool CallHeader(out GameObject[] obs, out int[] contentCost)
  {
      for (int i = 0; i < SceneManager.sceneCount; i++)
      {
          var s = SceneManager.GetSceneAt(i);
          if (!s.IsValid()) continue;
          var g = getDoubleList(s);
          list.listKeys.AddRange(g.listKeys);
          list.listValues.AddRange(g.listValues);
      }

      obs = Utilities.AllSceneObjects().Where(o => list.ContainsKey(o) && !string.IsNullOrEmpty(list[o])).ToArray();
      contentCost = obs
          .Select((d, i) => new { name = list[d], startIndex = i })
          .OrderBy(d => d.name)
          .Select((d, i) => new { d.startIndex, cost = i })
          .OrderBy(d => d.startIndex)
          .Select(d => d.cost).ToArray();
      return true;
  }

  internal void CallHeaderFilltered(out GameObject[] obs, out int[] contentCost, string filter)
  {
      DoubleList<GameObject, string> list = new DoubleList<GameObject, string>();
      for (int i = 0; i < SceneManager.sceneCount; i++)
      {
          var s = SceneManager.GetSceneAt(i);
          if (!s.IsValid()) continue;
          var g = getDoubleList(s);
          list.listKeys.AddRange(g.listKeys);
          list.listValues.AddRange(g.listValues);
      }

      obs = Utilities.AllSceneObjects().Where(o => list.ContainsKey(o) && !string.IsNullOrEmpty(list[o]) && String.Equals(filter, list[o], StringComparison.CurrentCultureIgnoreCase)).ToArray();
      contentCost = obs
          .Select((d, i) => new { name = list[d], startIndex = i, obj = d })
          .OrderBy(d => d.name)
          .Select((d, i) => {
              var cost = i;
              cost += d.obj.activeInHierarchy ? 0 : 100000000;
              return new { d.startIndex, cost = cost };
          })
          .OrderBy(d => d.startIndex)
          .Select(d => d.cost).ToArray();
  }*/

/* int ToContentCost(GameObject o)
{
    var aud = o.GetComponent<AudioSource>();
    if (aud.clip == null) return 1;
    return 0;
}*/
