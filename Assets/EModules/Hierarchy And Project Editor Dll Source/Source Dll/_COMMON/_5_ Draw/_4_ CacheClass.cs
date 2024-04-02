#if UNITY_EDITOR
    #define HIERARCHY
    #define PROJECT
#endif
#if PROJECT
    using EModules.Project;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

//namespace EModules

//% (ctrl on Windows, cmd on macOS), # (shift), &


namespace EModules.EModulesInternal


{


internal partial class Adapter {
    internal static Color TRANSP_COLOR = new Color( 0, 0, 0, 0 );
    public enum CacherType { IconColor, KeeperData, ImageIcon };
    
    
    
    internal static void SWITCH_TO_SCENE_MODE( Adapter A )
    {   Hierarchy_GUI.Instance( A ).SaveToScriptableObject = "SCENE";
        Hierarchy_GUI.Instance( A ).GETUSE_REGISTRATOR = false;
        Hierarchy_GUI.SetDirtyObject( A );
        Adapter.RequestScriptReload();
    }
    
    
    
    
    internal static void SET_HASH_WITHOUT_LOCALID( IHashProperty from, IHashProperty to )
    {   to.SaveInFolder = from.SaveInFolder;
        to.EnableRegistrator = from.EnableRegistrator;
        to.FavoritCategorySelected = from.FavoritCategorySelected;
        
        to.SetFreezeHashKeys( from.GetFreezeHashKeys().ToList() );
        to.SetFreezeHashValues( from.GetFreezeHashValues().ToList() );
        
        to.SetBookMarks( from.GetBookMarks().Select( s => s.Clone() ).ToList() );
        to.SetHash10( from.GetHash10().Select( s => s.Clone() ).ToList() );
        to.SetHash1_Fix2_0( from.GetHash1_Fix2_0().ToList() );
        to.SetHash2( from.GetHash2().ToList() );
        to.SetHash3( from.GetHash3().Select( s => s.Clone() ).ToList() );
        to.SetHash4( from.GetHash4().Select( s => s.Clone() ).ToList() );
        to.SetHash5_Fix2_0( from.GetHash5_Fix2_0().ToList() );
        to.SetHash6( from.GetHash6().Select( s => s.Clone() ).ToList() );
        to.SetHash7_Fix2_0( from.GetHash7_Fix2_0().ToList() );
        to.SetHash8( from.GetHash8().Select( s => s.Clone() ).ToList() );
        to.SetHash9( from.GetHash9().ToList() );
        to.HierarchyCache( from.HierarchyCache().Select( s => s.Clone() ).ToList() );
        to.SetHash_IconImageKey( from.GetHash_IconImageKey().ToList() );
        to.SetHash_IconImageValue( from.GetHash_IconImageValue().Select( s => s.Clone() ).ToList() );
        
    }
    
    
    internal static void SET_HASH_LOCALIDONLY( HierarchyDescriptionHelper from, HierarchyDescriptionHelper to )
    {
    
        to.LOCALIDINFILE_Hash1 = from.LOCALIDINFILE_Hash1.ToList();
        to.LOCALIDINFILE_Hash5 = from.LOCALIDINFILE_Hash5.ToList();
        to.LOCALIDINFILE_Hash7 = from.LOCALIDINFILE_Hash7.ToList();
        
        to.LOCALIDINFILE_HashFreeze = from.LOCALIDINFILE_HashFreeze.ToList();
        to.LOCALIDINFILE_Hash1_Fix2_0 = from.LOCALIDINFILE_Hash1_Fix2_0.ToList();
        to.LOCALIDINFILE_Hash5_Fix2_0 = from.LOCALIDINFILE_Hash5_Fix2_0.ToList();
        to.LOCALIDINFILE_Hash7_Fix2_0 = from.LOCALIDINFILE_Hash7_Fix2_0.ToList();
        to.LOCALIDINFILE_Hash9 = from.LOCALIDINFILE_Hash9.ToList();
        
        to.LOCALIDINFILE_Hash_BookMarks = from.LOCALIDINFILE_Hash_BookMarks.Select( b => b.Clone() ).ToList();
        to.LOCALIDINFILE_Hash3 = from.LOCALIDINFILE_Hash3.Select( b => b.Clone() ).ToList();
        to.LOCALIDINFILE_Hash4 = from.LOCALIDINFILE_Hash4.Select( b => b.Clone() ).ToList();
        
        to.LOCALIDINFILE_m_Hash_IconImageKey = from.LOCALIDINFILE_m_Hash_IconImageKey.ToList();
    }
    
#pragma warning disable
    internal static bool CHECK_UNITYEDITOR_SERIALIZATION_TYPE()
    {   return true;
    
        if ( Hierarchy.HierarchyAdapterInstance == null ) throw new Exception( "Hierarchy didnt assign" );
        
        if ( EditorSettings.serializationMode != SerializationMode.ForceText )
        {   var yes = EditorUtility.DisplayDialog( "Editor Settings",
                                                   "in order to use the folder data you should switch unity editor serialization mode to SerializationMode.ForceText",
                                                   "Switch to ForceText",
                                                   "Cancel" );
                                                   
            if ( yes )
            {   EditorSettings.serializationMode = SerializationMode.ForceText;
                Adapter.RequestScriptReload();
                return true;
            }
            else
            {   SWITCH_TO_SCENE_MODE( Hierarchy.HierarchyAdapterInstance );
                return false;
            }
        }
        return true;
    }
#pragma warning restore
    
    
    internal static IHashProperty GetProjectHash( Scene s )
    {   var scene_path =  Adapter.GetScenePath(s);
    
        var path = Hierarchy.HierarchyAdapterInstance.PluginInternalFolder + "/_ SAVED DATA/" + scene_path.Remove(scene_path.LastIndexOf('.')) + ".asset";
        
        var result = AssetDatabase.LoadAssetAtPath<ScriptableObject>( path ) as IHashProperty;
        
        return result;
    }
    
    internal void CACHE_CKECK( IHashProperty d, Scene s )
    {   CHECK_UNITYEDITOR_SERIALIZATION_TYPE();
    
        if ( d.SaveInFolder != Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).SaveToScriptableObject ||
                d.SaveInFolder == "FOLDER" && d.gameObject ||
                d.SaveInFolder == "SCENE" && d.unityobject )
        {   var root = s.GetRootGameObjects();
            var sceneHash = root.Select(r => HierarchyExtensions.Utilities.GetComponentFast<Component>.GetAll(r).FirstOrDefault(c => c is IHashProperty) as IHashProperty ).FirstOrDefault(c => c != null
                            && c.gameObject);
            var folderHash = GetProjectHash(s);
            
            
            
            if ( sceneHash != null )
            {   sceneHash.SaveInFolder = Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).SaveToScriptableObject;
                EditorUtility.SetDirty( sceneHash.component );
            }
            if ( folderHash != null )
            {   folderHash.SaveInFolder = Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).SaveToScriptableObject;
                EditorUtility.SetDirty( folderHash.unityobject );
            }
            
            if ( sceneHash != null && folderHash != null )
            {
            
                try
                {   if ( Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).SaveToScriptableObject == "FOLDER" )
                    {   Adapter.SET_HASH_WITHOUT_LOCALID( sceneHash, folderHash );
                        ((HierarchyDescriptionHelper)folderHash).SetDirty( s );
                        Hierarchy.HierarchyAdapterInstance.EditorSceneManagerOnSceneOpening( null, OpenSceneMode.Single );
                        UnityEngine.Object.DestroyImmediate( sceneHash.gameObject, true );
                    }
                    else
                    {   Adapter.SET_HASH_WITHOUT_LOCALID( folderHash, sceneHash );
                        EditorUtility.SetDirty( sceneHash.component );
                        EditorUtility.SetDirty( sceneHash.gameObject );
                        var assetPath = AssetDatabase.GetAssetPath(folderHash.unityobject);
                        if ( folderHash != null && folderHash.unityobject )
                        {   var path = Adapter.UNITY_SYSTEM_PATH;
                            if ( !path.EndsWith( "/" ) ) path += '/';
                            var oldName = path +  AssetDatabase.GetAssetPath( folderHash.unityobject );
                            var newName = oldName + ".backup";
                            if ( System.IO.File.Exists( newName ) ) System.IO.File.Delete( newName );
                            if ( System.IO.File.Exists( oldName ) )
                                System.IO.File.Move( oldName, newName );
                            if ( System.IO.File.Exists( newName + ".meta" ) ) System.IO.File.Delete( newName + ".meta" );
                            if ( System.IO.File.Exists( oldName + ".meta" ) )
                                System.IO.File.Move( oldName + ".meta", newName + ".meta" );
                                
                        }
                        if ( !string.IsNullOrEmpty( assetPath ) ) AssetDatabase.DeleteAsset( assetPath );
                        AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
                    }
                }
                catch ( Exception ex )
                {   if ( !ex.Message.Contains( "Sharing violation on path" ) )
                        Debug.Log( ex.Message + "\n\n" + ex.StackTrace );
                }
                
                
                
                MarkSceneDirty( s );
                MarkSceneDirty( s );
                Hierarchy.HierarchyAdapterInstance.EditorSceneManagerOnSceneOpening( null, OpenSceneMode.Single );
            }
            
        }//if
    }
}




internal class ObjectCacheHelper<TObject, TValue> where TValue : class {
    Func<IHashProperty, List<TObject>> get1;
    Func<IHashProperty, List<TValue>> get2;
    Adapter.CacherType type;
    bool gameOjbect;
    bool HOGoGuidPair;
    bool LONG;
    Adapter adapter;
    string prefKey;
    internal ObjectCacheHelper( Func<IHashProperty, List<TObject>> get1, Func<IHashProperty, List<TValue>> get2, Adapter.CacherType type, Adapter adapter, string name )
    {
    
        prefKey = adapter.pluginname + "/" + "Cache " + name + " LastScene";
        
        this.adapter = adapter;
        this.get1 = get1;
        this.get2 = get2;
        this.type = type;
        
        gameOjbect = typeof( TObject ) == typeof( GameObject );
        HOGoGuidPair = !gameOjbect && typeof( TObject ) == typeof( GoGuidPair );
        if ( !HOGoGuidPair && !gameOjbect ) throw new Exception( "Unknowed ObjectCacheHelper Type" );
        LONG = !gameOjbect && !HOGoGuidPair && typeof( TObject ) == typeof( long );
        if ( LONG ) throw new Exception( "not awaliable anymore" );
        
        if ( !gameOjbect && !HOGoGuidPair ) throw new Exception( typeof( TObject ) + " error" );
        
        int fixInd = 0;
        Action undo_ac = null;
        undo_ac = () =>
        {   if ( fixInd > 0 ) return;
            if ( cacheDic == null )
            {   adapter.onUndoAction -= undo_ac;
                fixInd++;
            }
            else
                cacheDic.Clear();
        };
        adapter.onUndoAction += undo_ac;
        
        int fixInd2 = 0;
        Action aceneo_ac = null;
        aceneo_ac = () =>     //  Debug.Log( EditorSceneManager.GetActiveScene().GetHashCode() );
        {   if ( fixInd2 > 0 ) return;
            if ( cacheDic == null )
            {   adapter.bottomInterface.onSceneChange -= aceneo_ac;
                fixInd2++;
            }
            else
                cacheDic.Clear();
        };
        adapter.bottomInterface.onSceneChange += aceneo_ac;
        // += aceneo_ac;
    }
    class GOComparer : IEqualityComparer<Adapter.HierarchyObject> {
        public bool Equals( Adapter.HierarchyObject b1, Adapter.HierarchyObject b2 )
        {   if ( ReferenceEquals( b1, null ) && ReferenceEquals( b2, null ) ) return true;
            if ( ReferenceEquals( b1, null ) || ReferenceEquals( b2, null ) ) return false;
            return b1.go == b2.go;
        }
        public int GetHashCode( Adapter.HierarchyObject bx )
        {   if ( ReferenceEquals( bx, null ) || !bx.go ) return -1;
            return bx.go.GetInstanceID();
        }
    }
    internal Dictionary<int, Dictionary<Adapter.HierarchyObject, int>> cacheDic = new Dictionary<int, Dictionary<Adapter.HierarchyObject, int>>();
    
    internal bool HasKey( Adapter.HierarchyObject _o )
    {   return HasKey( _o.scene, _o );
    }
    internal bool HasKey( int scene, Adapter.HierarchyObject _o )
    {   if ( !_o.Validate() ) return false;
        if ( !cacheDic.ContainsKey( scene ) )
        {
        
            var needClear = false;
            
            if ( adapter.IS_HIERARCHY() ) needClear
                    = EditorSceneManager.GetActiveScene().IsValid() && SessionState.GetInt( prefKey, -1 ) != EditorSceneManager.GetActiveScene().GetHashCode();
            SessionState.SetInt( prefKey, EditorSceneManager.GetActiveScene().GetHashCode() );
            
            if ( get1( adapter.MOI.des( scene ) ).Count != get2( adapter.MOI.des( scene ) ).Count )
            {
            
                #if UNITY_EDITOR
            
                if ( EditorUtility.DisplayDialog( "Hierarchy Pro - Critical error", "Cache index out of range\n" + get1( adapter.MOI.des( scene ) ).Count + " "
                                                  + get2( adapter.MOI.des( scene ) ).Count + " : " + _o.name + "\n" + this.GetType().FullName
                                                  + "\n\nDo you want to remove missed element?", "Yes", "Cancel" ) )
                #endif
                {   var l1 = get1( adapter.MOI.des( scene ) );
                    var l2 = get2( adapter.MOI.des( scene ) );
                    var miin = Math.Min(l1.Count, l2.Count);
                    while ( l1.Count > miin ) l1.RemoveAt( l1.Count - 1 );
                    while ( l2.Count > miin ) l2.RemoveAt( l1.Count - 1 );
                }
            }
            // needClear = true;
            var h = get1( adapter.MOI.des( scene ) );
            if ( gameOjbect )
                cacheDic.Add( scene, new Dictionary<Adapter.HierarchyObject, int>( new GOComparer() ) );
            else
                cacheDic.Add( scene, new Dictionary<Adapter.HierarchyObject, int>() );
            for ( int i = 0 ; i < h.Count ; i++ )
                //for (int i = h.Count - 1 ; i >= 0 ; i--)
            {   if ( gameOjbect )
                {   var o = h[i] as GameObject;
                    if ( needClear && (!o || o.scene.GetHashCode() != scene) )     //Debug.Log(h.Count + " ASD");
                    {   if ( i < get1( adapter.MOI.des( scene ) ).Count ) get1( adapter.MOI.des( scene ) ).RemoveAt( i );
                        if ( i < get2( adapter.MOI.des( scene ) ).Count ) get2( adapter.MOI.des( scene ) ).RemoveAt( i );
                        //Debug.Log(h.Count + " ASaaD");
                        i--;
                        continue;
                    }
                    if ( o )
                    {   var h2 =  adapter.GetHierarchyObjectByInstanceID(o.GetInstanceID());
                        if ( !cacheDic[scene].ContainsKey( h2 ) ) cacheDic[scene].Add( h2, i );
                    }
                }
                else if ( HOGoGuidPair )
                {   /* var __o =  (GoGuidPair)(IGoGuidPair) h[i];
                         var o = adapter.GetHierarchyObjectByPair( __o);*/
                    var o = adapter.GetHierarchyObjectByPair( ref h, i );
                    if ( needClear && !o.Validate( scene ) )     //Debug.Log(h.Count + " ASD");
                    {   if (i < get1( adapter.MOI.des( scene ) ).Count)      get1( adapter.MOI.des( scene ) ).RemoveAt( i );
                        if ( i < get2( adapter.MOI.des( scene ) ).Count ) get2( adapter.MOI.des( scene ) ).RemoveAt( i );
                        //Debug.Log(h.Count + " ASDaa");
                        i--;
                        continue;
                    }
                    if ( o.Validate() )
                    {   if ( !cacheDic[scene].ContainsKey( o ) )
                            cacheDic[scene].Add( o, i );
                    }
                }
                /* else
                 {   if (LONG)
                     {   cacheDic[scene].Add( Convert.ToInt64( h[i] ), i );
                     }
                     else
                     {   cacheDic[scene].Add( Convert.ToInt32( h[i] ), i );
                     }
                     if (needClear && !EditorUtility.InstanceIDToObject( Convert.ToInt32( h[i] ) ))
                     {   get1( adapter.MOI.des( scene ) ).RemoveAt( i );
                         get2( adapter.MOI.des( scene ) ).RemoveAt( i );
                         i--;
                         continue;
                     }
                 }*/
                
            }
        }
        return cacheDic[scene].ContainsKey( _o );
    }
    /*  bool HasKey(Scene s, int id)
      {
          if (!cacheDic.ContainsKey(s))
          {
              var h = get1(M_Descript.des(s));
              cacheDic.Add(s, new Dictionary<int, int>());
              for (int i = 0; i < h.Count; i++)
              {
                  if (gameOjbect)
                  {
                      var o = h[i] as GameObject;
                      if (!o) continue;
                      cacheDic[s].Add(o.GetInstanceID(), i);
                  } else
                  {
                      cacheDic[s].Add(Convert.ToInt32(h[i]), i);
                  }
    
              }
          }
          return cacheDic[s].ContainsKey(id);
      }*/
    
    internal TValue GetValue( Scene scene, Adapter.HierarchyObject o )
    {   return GetValue( scene.GetHashCode(), o );
    }
    
    bool wasExeption = false;
    
    internal TValue GetValue( int scene, Adapter.HierarchyObject o )
    {   if ( !HasKey( scene, o ) ) return null;
        var d = adapter.MOI.des( scene );
        if ( d == null ) return null;
        var ptr = cacheDic[scene][o];
        // Debug.Log( ptr + " " + get2( d ).Count );
        if ( ptr >= get2( d ).Count )
        {   bool fixedd = false;
        
            if ( !wasExeption )
            {   if (EditorUtility.DisplayDialog( "Hierarchy Pro - Critical error", "Cache index out of range\n" + ptr + " " + get2( d ).Count + " : " + o.name + "\n" + this.GetType().FullName
                                                 + "\n\nDo you want to remove missed element?", "Yes", "Cancel" ) )
                {   cacheDic[scene].Remove( o );
                    //  getDoubleList( scene ).listKeys.RemoveAll( o );
                    fixedd = true;
                    //  SetValue( null, scene, o, false );
                    //   throw new Exception( o.name );
                    //adapter.logProxy.LogError( ptr + "\n" + get2( d ).Count + "\n" + o.name );
                }
            }
            
            if ( !fixedd )
            {   wasExeption = true;
                return null;
            }
            else
            {   return null;
            }
        }
        return get2( d )[ptr];
    }
    
    internal string GetValueToString( int scene, Adapter.HierarchyObject o )
    {   var r = GetValue( scene, o );
        if ( r == null ) return null;
        return r.ToString();
    }
    
    DoubleList<TObject, TValue> dl = new DoubleList<TObject, TValue>();
    public DoubleList<TObject, TValue> getDoubleList( int scene )
    {   dl.listKeys = get1( adapter.MOI.des( scene ) );
        dl.listValues = get2( adapter.MOI.des( scene ) );
        
        // var res = new DoubleList<GameObject, string> { listKeys = des.listKeys, listValues = des.listValues };
        return dl;
    }
    
    internal void SetValue( TValue value, int scene, TObject _o, bool SaveRegistrator )        // var gameObject
    {
    
        // GameObject o = _o as GameObject;
        
        if ( gameOjbect && !(_o as GameObject) ) return;
        
        if ( HOGoGuidPair )
        {   var asd  = (GoGuidPair)(IGoGuidPair)_o;
            if ( !adapter.GetHierarchyObjectByPair( ref asd ).Validate() ) return;
        }
        
        
        //  if (!gameOjbect && !HO && !(EditorUtility.InstanceIDToObject( (_o as int?).Value ) as UnityEngine.Object)) return;
        
        
        
        
        var d = adapter.MOI.des( scene );
        if ( d == null ) return;
        cacheDic.Clear();
        //  Undo.RecordObject(d.component, "Change description");
        
        
        
        var list = getDoubleList( scene );
        if ( value == null )
        {   list.RemoveAll( _o );
        }
        else
        {   if ( list.ContainsKey( _o ) )
            {   list[_o] = value;
            }
            else list.Add( _o, value );
        }
        
        
        #if HIERARCHY
        if ( SaveRegistrator )
        {   if ( gameOjbect ) adapter.DescriptionModule.TrySaveCustomRegistrator( Hierarchy.HierarchyAdapterInstance.GetHierarchyObjectByInstanceID( _o as GameObject ), value, type );
            if ( HOGoGuidPair )
            {   var asd  = (GoGuidPair)(IGoGuidPair)_o;
                adapter.DescriptionModule.TrySaveCustomRegistrator( adapter.GetHierarchyObjectByPair( ref asd ), value, type );
            }
            // if (!gameOjbect && !HO) adapter.DescriptionModule.TrySaveCustomRegistrator( Hierarchy.adapter.GetHierarchyObjectByInstanceID( _o as GameObject ), value, type );
            
        }
        #endif
        
        if ( !Application.isPlaying )
        {   /*  Hierarchy.SetDirty(d.component);
                  Hierarchy.SetDirty(d.gameObject);*/
            adapter.SetDirtyDescription( d, scene );
            
            adapter.MarkSceneDirty( scene );
        }
    }
}












internal class ObjectCacheHelperOld<TObject, TValue> where TValue : class {
    Func<IHashProperty, List<TObject>> get1;
    Func<IHashProperty, List<TValue>> get2;
    Adapter.CacherType type;
    bool gameOjbect;
    bool HOGoGuidPair;
    bool LONG;
    Adapter adapter;
    string prefKey;
    internal ObjectCacheHelperOld( Func<IHashProperty, List<TObject>> get1, Func<IHashProperty, List<TValue>> get2, Adapter.CacherType type, Adapter adapter, string name )
    {
    
        prefKey = adapter.pluginname + "/" + "Cache " + name + " LastScene";
        
        this.adapter = adapter;
        this.get1 = get1;
        this.get2 = get2;
        this.type = type;
        
        gameOjbect = typeof( TObject ) == typeof( GameObject );
        HOGoGuidPair = !gameOjbect && typeof( TObject ) == typeof( GoGuidPair );
        if ( !HOGoGuidPair && !gameOjbect ) throw new Exception( "Unknowed ObjectCacheHelper Type" );
        LONG = !gameOjbect && !HOGoGuidPair && typeof( TObject ) == typeof( long );
        
        if ( !gameOjbect && !HOGoGuidPair ) throw new Exception( typeof( TObject ) + " error" );
        
        int fixInd = 0;
        Action undo_ac = null;
        undo_ac = () =>
        {   if ( fixInd > 0 ) return;
            if ( cacheDic == null )
            {   adapter.onUndoAction -= undo_ac;
                fixInd++;
            }
            cacheDic.Clear();
        };
        adapter.onUndoAction += undo_ac;
        
        int fixInd2 = 0;
        Action aceneo_ac = null;
        aceneo_ac = () =>     //  Debug.Log( EditorSceneManager.GetActiveScene().GetHashCode() );
        {   if ( fixInd2 > 0 ) return;
            if ( cacheDic == null )
            {   adapter.bottomInterface.onSceneChange -= aceneo_ac;
                fixInd2++;
            }
            cacheDic.Clear();
        };
        adapter.bottomInterface.onSceneChange += aceneo_ac;
        // += aceneo_ac;
    }
    
    internal Dictionary<int, Dictionary<long, int>> cacheDic = new Dictionary<int, Dictionary<long, int>>();
    
    internal bool HasKey( Adapter.HierarchyObject _o )
    {   return HasKey( _o.scene, _o );
    }
    internal bool HasKey( int scene, Adapter.HierarchyObject _o )
    {   if ( !_o.Validate() ) return false;
        if ( !cacheDic.ContainsKey( scene ) )
        {
        
            var needClear = false;
            
            if ( adapter.IS_HIERARCHY() ) needClear
                    = EditorSceneManager.GetActiveScene().IsValid() && EditorPrefs.GetInt( prefKey, -1 ) != EditorSceneManager.GetActiveScene().GetHashCode();
            EditorPrefs.SetInt( prefKey, EditorSceneManager.GetActiveScene().GetHashCode() );
            
            var h = get1( adapter.MOI.des( scene ) );
            cacheDic.Add( scene, new Dictionary<long, int>() );
            for ( int i = 0 ; i < h.Count ; i++ )
                //for (int i = h.Count - 1 ; i >= 0 ; i--)
            {   if ( gameOjbect )
                {   var o = h[i] as GameObject;
                    if ( needClear && (!o || o.scene.GetHashCode() != scene) )     //Debug.Log(h.Count + " ASD");
                    {   get1( adapter.MOI.des( scene ) ).RemoveAt( i );
                        get2( adapter.MOI.des( scene ) ).RemoveAt( i );
                        //Debug.Log(h.Count + " ASaaD");
                        i--;
                        continue;
                    }
                    if ( o )
                        // {   cacheDic[scene].Add( Adapter.GET_INSTANCE_ID( o), i );
                    {   // cacheDic[scene].Add( Adapter.GetLocalIdentifierInFile( o), i );
                        cacheDic[scene].Add( adapter.GetFileID( o ), i );
                    }
                }
                else if ( HOGoGuidPair )
                {   /* var __o =  (GoGuidPair)(IGoGuidPair) h[i];
                         var o = adapter.GetHierarchyObjectByPair( __o);*/
                    var o = adapter.GetHierarchyObjectByPair( ref h, i );
                    if ( needClear && !o.Validate( scene ) )     //Debug.Log(h.Count + " ASD");
                    {   get1( adapter.MOI.des( scene ) ).RemoveAt( i );
                        get2( adapter.MOI.des( scene ) ).RemoveAt( i );
                        //Debug.Log(h.Count + " ASDaa");
                        i--;
                        continue;
                    }
                    if ( o.Validate() )
                    {   if ( !cacheDic[scene].ContainsKey( o.fileID ) )
                            cacheDic[scene].Add( o.fileID, i );
                    }
                }
                else
                {   if ( LONG )
                    {   cacheDic[scene].Add( Convert.ToInt64( h[i] ), i );
                    }
                    else
                    {   cacheDic[scene].Add( Convert.ToInt32( h[i] ), i );
                    }
                    if ( needClear && !EditorUtility.InstanceIDToObject( Convert.ToInt32( h[i] ) ) )
                    {   get1( adapter.MOI.des( scene ) ).RemoveAt( i );
                        get2( adapter.MOI.des( scene ) ).RemoveAt( i );
                        i--;
                        continue;
                    }
                }
                
            }
        }
        return cacheDic[scene].ContainsKey( _o.fileID );
    }
    /*  bool HasKey(Scene s, int id)
      {
          if (!cacheDic.ContainsKey(s))
          {
              var h = get1(M_Descript.des(s));
              cacheDic.Add(s, new Dictionary<int, int>());
              for (int i = 0; i < h.Count; i++)
              {
                  if (gameOjbect)
                  {
                      var o = h[i] as GameObject;
                      if (!o) continue;
                      cacheDic[s].Add(o.GetInstanceID(), i);
                  } else
                  {
                      cacheDic[s].Add(Convert.ToInt32(h[i]), i);
                  }
    
              }
          }
          return cacheDic[s].ContainsKey(id);
      }*/
    
    
    internal TValue GetValue( Scene scene, Adapter.HierarchyObject o )
    {   return GetValue( scene.GetHashCode(), o );
    }
    internal TValue GetValue( int scene, Adapter.HierarchyObject o )
    {   if ( !HasKey( scene, o ) ) return null;
        var d = adapter.MOI.des( scene );
        if ( d == null ) return null;
        var ptr = cacheDic[scene][o.fileID];
        // Debug.Log( ptr + " " + get2( d ).Count );
        return get2( d )[ptr];
    }
    
    
    internal string GetValueToString( int scene, Adapter.HierarchyObject o )
    {   var r = GetValue( scene, o );
        if ( r == null ) return null;
        return r.ToString();
    }
    
    DoubleList<TObject, TValue> dl = new DoubleList<TObject, TValue>();
    public DoubleList<TObject, TValue> getDoubleList( int scene )
    {   dl.listKeys = get1( adapter.MOI.des( scene ) );
        dl.listValues = get2( adapter.MOI.des( scene ) );
        
        // var res = new DoubleList<GameObject, string> { listKeys = des.listKeys, listValues = des.listValues };
        return dl;
    }
    
    internal void SetValue( TValue value, int scene, TObject _o, bool SaveRegistrator )        // var gameObject
    {
    
        // GameObject o = _o as GameObject;
        
        if ( gameOjbect && !(_o as GameObject) ) return;
        
        if ( HOGoGuidPair )
        {   var asd  = (GoGuidPair)(IGoGuidPair)_o;
            if ( !adapter.GetHierarchyObjectByPair( ref asd ).Validate() ) return;
        }
        
        
        //  if (!gameOjbect && !HO && !(EditorUtility.InstanceIDToObject( (_o as int?).Value ) as UnityEngine.Object)) return;
        
        
        
        
        var d = adapter.MOI.des( scene );
        if ( d == null ) return;
        cacheDic.Clear();
        //  Undo.RecordObject(d.component, "Change description");
        
        
        
        var list = getDoubleList( scene );
        if ( value == null )
        {   list.RemoveAll( _o );
        }
        else
        {   if ( list.ContainsKey( _o ) )
            {   list[_o] = value;
            }
            else list.Add( _o, value );
        }
        
        
        #if HIERARCHY
        if ( SaveRegistrator )
        {   if ( gameOjbect ) adapter.DescriptionModule.TrySaveCustomRegistrator( Hierarchy.HierarchyAdapterInstance.GetHierarchyObjectByInstanceID( _o as GameObject ), value, type );
            if ( HOGoGuidPair )
            {   var asd  = (GoGuidPair)(IGoGuidPair)_o;
                adapter.DescriptionModule.TrySaveCustomRegistrator( adapter.GetHierarchyObjectByPair( ref asd ), value, type );
            }
            // if (!gameOjbect && !HO) adapter.DescriptionModule.TrySaveCustomRegistrator( Hierarchy.adapter.GetHierarchyObjectByInstanceID( _o as GameObject ), value, type );
            
        }
        #endif
        
        if ( !Application.isPlaying )
        {   /*  Hierarchy.SetDirty(d.component);
                  Hierarchy.SetDirty(d.gameObject);*/
            adapter.SetDirtyDescription( d, scene );
            
            adapter.MarkSceneDirty( scene );
        }
    }
}




}
