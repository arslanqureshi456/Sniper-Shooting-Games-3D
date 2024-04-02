#if !UNITY_EDITOR
#endif
#define REMOVE_ONLY_FLAGS

#if UNITY_EDITOR
    //  #define LOG
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
//namespace EModules

namespace EModules.EModulesInternal {
/* [RequireComponent(typeof(SETUPROOT))]
 [RequireComponent(typeof(Hierarchy_GUI))]
 [RequireComponent(typeof(DescriptionHelper))]*/
internal partial class Adapter {
    Type __DF;
    internal  Type DT;
    internal  Type DR;
    internal   MonoScript loadedH1;
    internal Type DF
    {   get
        {   if ( __DF == null )
            {   loadedH1 = AssetDatabase.LoadAssetAtPath<MonoScript>( Hierarchy.HierarchyAdapterInstance.PluginInternalFolder + "/SharedFolder/DescriptionHelper.cs" );
            
                var loadedH2 = AssetDatabase.LoadAssetAtPath<MonoScript>( Hierarchy.HierarchyAdapterInstance.PluginInternalFolder + "/SharedFolder/DescriptionRegistrator.cs" );
                
                var loadedH3 = AssetDatabase.LoadAssetAtPath<MonoScript>( Hierarchy.HierarchyAdapterInstance.PluginInternalFolder + "/SharedFolder/DescriptionFlush.cs" );
                
                
                if ( loadedH1 == null || loadedH1.GetClass() == null || !typeof( IHashProperty ).IsAssignableFrom( loadedH1.GetClass() ) ||
                        loadedH2 == null || loadedH2.GetClass() == null || !typeof( IDescriptionRegistrator ).IsAssignableFrom( loadedH2.GetClass() ) ||
                        loadedH3 == null || loadedH3.GetClass() == null || !typeof( IDescriptionFlush ).IsAssignableFrom( loadedH3.GetClass() ) )
                {   Hierarchy.HierarchyAdapterInstance.tempAdapterBlock = true;
                    Hierarchy.HierarchyAdapterInstance.needReinstall = true;
                    return null;
                }
                
                DT = loadedH1.GetClass();
                DR = loadedH2.GetClass();
                __DF = loadedH3.GetClass();
            }
            return __DF;
        }
    }
}

internal partial class Hierarchy {



    internal partial class M_Descript : Adapter.M_DescriptionCommon {
    
    
    
        //static  internal  Type DT;
        // static  internal  Type DR;
        Type DT { get { return adapter.DT; } }
        static Type DR { get { return Hierarchy.HierarchyAdapterInstance.DR; } }
        
        
        
        public class MD4 : HashAlgorithm {
            private uint _a;
            private uint _b;
            private uint _c;
            private uint _d;
            private uint[] _x;
            private int _bytesProcessed;
            
            public MD4()
            {   _x = new uint[16];
            
                Initialize();
            }
            
            public override void Initialize()
            {   _a = 0x67452301;
                _b = 0xefcdab89;
                _c = 0x98badcfe;
                _d = 0x10325476;
                
                _bytesProcessed = 0;
            }
            
            protected override void HashCore( byte[] array, int offset, int length )
            {   ProcessMessage( Bytes( array, offset, length ) );
            }
            
            protected override byte[] HashFinal()
            {   try
                {   ProcessMessage( Padding() );
                
                    return new[] { _a, _b, _c, _d } .SelectMany( word => Bytes( word ) ).ToArray();
                }
                finally
                {   Initialize();
                }
            }
            
            private void ProcessMessage( IEnumerable<byte> bytes )
            {   foreach ( byte b in bytes )
                {   int c = _bytesProcessed & 63;
                    int i = c >> 2;
                    int s = (c & 3) << 3;
                    
                    _x[i] = (_x[i] & ~((uint)255 << s)) | ((uint)b << s);
                    
                    if ( c == 63 )
                    {   Process16WordBlock();
                    }
                    
                    _bytesProcessed++;
                }
            }
            
            private static IEnumerable<byte> Bytes( byte[] bytes, int offset, int length )
            {   for ( int i = offset ; i < length ; i++ )
                {   yield return bytes[i];
                }
            }
            
            private IEnumerable<byte> Bytes( uint word )
            {   yield return (byte)(word & 255);
                yield return (byte)((word >> 8) & 255);
                yield return (byte)((word >> 16) & 255);
                yield return (byte)((word >> 24) & 255);
            }
            
            private IEnumerable<byte> Repeat( byte value, int count )
            {   for ( int i = 0 ; i < count ; i++ )
                {   yield return value;
                }
            }
            
            private IEnumerable<byte> Padding()
            {   return Repeat( 128, 1 )
                       .Concat( Repeat( 0, ((_bytesProcessed + 8) & 0x7fffffc0) + 55 - _bytesProcessed ) )
                       .Concat( Bytes( (uint)_bytesProcessed << 3 ) )
                       .Concat( Repeat( 0, 4 ) );
            }
            
            private void Process16WordBlock()
            {   uint aa = _a;
                uint bb = _b;
                uint cc = _c;
                uint dd = _d;
                
                foreach ( int k in new[] { 0, 4, 8, 12 } )
                {   aa = Round1Operation( aa, bb, cc, dd, _x[k], 3 );
                    dd = Round1Operation( dd, aa, bb, cc, _x[k + 1], 7 );
                    cc = Round1Operation( cc, dd, aa, bb, _x[k + 2], 11 );
                    bb = Round1Operation( bb, cc, dd, aa, _x[k + 3], 19 );
                }
                
                foreach ( int k in new[] { 0, 1, 2, 3 } )
                {   aa = Round2Operation( aa, bb, cc, dd, _x[k], 3 );
                    dd = Round2Operation( dd, aa, bb, cc, _x[k + 4], 5 );
                    cc = Round2Operation( cc, dd, aa, bb, _x[k + 8], 9 );
                    bb = Round2Operation( bb, cc, dd, aa, _x[k + 12], 13 );
                }
                
                foreach ( int k in new[] { 0, 2, 1, 3 } )
                {   aa = Round3Operation( aa, bb, cc, dd, _x[k], 3 );
                    dd = Round3Operation( dd, aa, bb, cc, _x[k + 8], 9 );
                    cc = Round3Operation( cc, dd, aa, bb, _x[k + 4], 11 );
                    bb = Round3Operation( bb, cc, dd, aa, _x[k + 12], 15 );
                }
                
                unchecked
                {   _a += aa;
                    _b += bb;
                    _c += cc;
                    _d += dd;
                }
            }
            
            private static uint ROL( uint value, int numberOfBits )
            {   return (value << numberOfBits) | (value >> (32 - numberOfBits));
            }
            
            private static uint Round1Operation( uint a, uint b, uint c, uint d, uint xk, int s )
            {   unchecked
                {   return ROL( a + ((b & c) | (~b & d)) + xk, s );
                }
            }
            
            private static uint Round2Operation( uint a, uint b, uint c, uint d, uint xk, int s )
            {   unchecked
                {   return ROL( a + ((b & c) | (b & d) | (c & d)) + xk + 0x5a827999, s );
                }
            }
            
            private static uint Round3Operation( uint a, uint b, uint c, uint d, uint xk, int s )
            {   unchecked
                {   return ROL( a + (b ^ c ^ d) + xk + 0x6ed9eba1, s );
                }
            }
        }
        
        public static class FileIDUtil {
            public static int Compute( string Namespace, string Name )
            {   string toBeHashed = "s\0\0\0" + Namespace + Name;
            
                using ( HashAlgorithm hash = new MD4() )
                {   byte[] hashed = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(toBeHashed));
                
                    int result = 0;
                    
                    for ( int i = 3 ; i >= 0 ; --i )
                    {   result <<= 8;
                        result |= hashed[i];
                    }
                    
                    return result;
                }
            }
            public static int Compute( Type t )
            {   string toBeHashed = "s\0\0\0" + t.Namespace + t.Name;
            
                using ( HashAlgorithm hash = new MD4() )
                {   byte[] hashed = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(toBeHashed));
                
                    int result = 0;
                    
                    for ( int i = 3 ; i >= 0 ; --i )
                    {   result <<= 8;
                        result |= hashed[i];
                    }
                    
                    return result;
                }
            }
        }
        
        
        
        
        
        internal static HideFlags flags { get { return Hierarchy.HierarchyAdapterInstance.par.HIDE_DES ? Adapter.flagsHIDE : Adapter.flagsSHOW; } }
        
        
        
        internal static void UpdateFlags()
        {   if ( Hierarchy.HierarchyAdapterInstance.DISABLE_DES() ) return;
            //  MonoBehaviour.print(Resources.FindObjectsOfTypeAll<Component>().Where(c => c is IHashProperty).Count());
            for ( int i = 0 ; i < EditorSceneManager.loadedSceneCount ; i++ )
            {   var sc = EditorSceneManager.GetSceneAt(i);
                if ( !sc.isLoaded || !sc.IsValid() ) continue;
                var d = des(sc.GetHashCode());
                if ( d == null || !d.gameObject ) continue;
                //  MonoBehaviour.print(flags & HideFlags.HideInHierarchy + " " + d.gameObject.name);
                d.gameObject.hideFlags = flags /*HideFlags.None*/;
                //  d.component.hideFlags = flags /*HideFlags.None*/;
            }
        }
        GameObject oldDes;
        IHashProperty InitializeDescriptions( Scene s )     // MonoBehaviour.print(typeof(DescriptionHelper).FullName);
        {
        
            if ( adapter.DF == null )
            {
            
            
            
            
            }
            // __go = Resources.FindObjectsOfTypeAll<DescriptionHelper>().FirstOrDefault();
            var root = s.GetRootGameObjects();
            var __go = root.Select(r => r.GetComponent(DT)).FirstOrDefault(d => d && d.gameObject);
            // root = new GameObject[0];
            // if (__go) PrefabUtility.RecordPrefabInstancePropertyModifications(__go);
            #if LOG
            MonoBehaviour.print(root.Length + " " + ( __go == null));
            #endif
            
            if ( Application.isPlaying )
            {
            
            
                if ( !__go )
                {   __go = (new GameObject( "DescriptionHelperObject" )).AddComponent( DT );
                    __go.gameObject.name = "DescriptionHelperObject";
                }
                
                // __go.hideFlags = flags;
                __go.gameObject.hideFlags = flags;
                
                foreach ( var transform in root.Where( t =>
                                                       #if REMOVE_ONLY_FLAGS
                                                       (t.gameObject.hideFlags & HideFlags.DontSaveInBuild) != 0 &&
                                                       #endif
                                                       t.name == "DescriptionHelperObject" &&
                                                       //  (PrefabUtility.GetPrefabParent(t.gameObject) == null || string.IsNullOrEmpty(AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(t.gameObject))))&&
                                                       (!ReferenceEquals( t.gameObject, __go.gameObject )) ) )
                {
                
                    GameObject.Destroy( transform.gameObject );
                }
                
                
                return (IHashProperty)__go;
                
            }
            
            /*
            
                            if (!__go)
                            {
                                var candidates = root.Where(t =>
            #if REMOVE_ONLY_FLAGS
                            (t.gameObject.hideFlags & HideFlags.DontSaveInBuild) != 0 &&
            #endif
                            t.name == "DescriptionHelperObject");
                                foreach (var gameObject in candidates) {
                                    PrefabUtility.insta
                                }
                            }
            */
            
            
            if ( __go )
            {   var comp = __go.GetComponents<Component>();
                if ( comp.Any( c => c == null ) )
                {
                    #if LOG
                    MonoBehaviour.print("GetComponents<Component>().Any(c => c == null)");
                    #endif
                    __go = null;
                }
                else if ( Adapter.HierAdapter.GetCorrespondingObjectFromSource( __go.gameObject ) != null )
                {   __go = null;
                }
                
                
            }
            
            //MonoBehaviour.print(PrefabUtility.GetPrefabParent(__go.gameObject));
            // var finded = Resources.FindObjectsOfTypeAll<Transform>().Where(t =>
            var finded = root.Where(t =>
                                    #if REMOVE_ONLY_FLAGS
                                    (t.gameObject.hideFlags & HideFlags.DontSaveInBuild) != 0 &&
                                    #endif
                                    t.name == "DescriptionHelperObject" &&
                                    //  (PrefabUtility.GetPrefabParent(t.gameObject) == null || string.IsNullOrEmpty(AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(t.gameObject))))&&
                                    (__go == null || !ReferenceEquals(t.gameObject, __go.gameObject))).ToList();
                                    
                                    
                                    
                                    
                                    
            if ( !__go )
            {
            
            
                if ( finded.Count > 0 )
                {
                    #if LOG
                    MonoBehaviour.print("finded.Count > 0");
                    #endif
                    
                    
                    oldDes = finded.FirstOrDefault( f => f.GetComponent( DT ) );
                    if ( !oldDes )
                    {   /* foreach (var f in finded)
                         {   f.hideFlags = HideFlags.None;
                         }*/
                        
                        if ( UnityEditor.EditorUtility.DisplayDialog( "Hierarchy PRO " + s.path, "Found old colors DescriptionHelper settings that may be lost.\n\nDo you want to fix old DescriptionHelper object?", "Yes",
                                "No" ) )
                        {
                        
                        
                            if ( UnityEditor.EditorUtility.DisplayDialog( "Hierarchy PRO " + s.path,
                                    "We recommend to save a copy of the scene before. And also make sure that the Project Settings for serialization are set to ForceText.",
                                    "Yes",
                                    "Cancel" ) )
                            {   var ft = System.IO.File.ReadAllText(Adapter.UNITY_SYSTEM_PATH +  s.path);
                                var ds = ft.IndexOf("DescriptionHelperObject");
                                var fs = ft.IndexOf("m_Script:", ds);
                                var hs = ft.IndexOf("Hash1:", ds);
                                if ( hs < fs || fs == -1 )
                                {   UnityEditor.EditorUtility.DisplayDialog( "Warning", "Scene not fixed. Make sure that the Project Settings for serialization are set to ForceText", "Ok" );
                                }
                                else
                                {   var start = ft.IndexOf("guid:", fs);
                                    var end1 =  ft.IndexOf(",", start);
                                    var end2 =  ft.IndexOf("}", start);
                                    if ( end2 < end1 ) end1 = end2;
                                    var part1 = ft.Remove(start);
                                    var part2 = ft.Substring(end1);
                                    ft = part1 + "guid: " + AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( adapter.loadedH1 ) ) + part2;
                                    System.IO.File.WriteAllText( Adapter.UNITY_SYSTEM_PATH + s.path, ft );
                                    AssetDatabase.ImportAsset( s.path, ImportAssetOptions.ForceUpdate );
                                    AssetDatabase.Refresh();
                                    
                                    if ( EditorSceneManager.sceneCount == 1 )
                                    {   EditorSceneManager.OpenScene( s.path, OpenSceneMode.Single );
                                    }
                                    else
                                    {   var p = s.path;
                                        EditorSceneManager.CloseScene( s, true );
                                        EditorSceneManager.OpenScene( p, OpenSceneMode.Additive );
                                    }
                                    // EditorGUIUtility.ExitGUI();
                                    // return null;
                                    s = EditorSceneManager.GetActiveScene();
                                    
                                    adapter.Again_Reloder_UsingWhenCopyPastOrAssets();
                                }
                                
                            }
                        }
                        
                        // throw new Exception("Cannot read old Hierarchy PRO data");
                    }
                }
                
                var oldS = EditorSceneManager.GetActiveScene();
                if ( oldS != s ) EditorSceneManager.SetActiveScene( s );
                var tg = new GameObject("DescriptionHelperObject");
                
                __go = (tg).AddComponent( DT );
                //MonoBehaviour.print( DT );
                if ( !__go )
                {   GameObject.DestroyImmediate( tg );
                    Hierarchy.HierarchyAdapterInstance.logProxy.LogWarning( "Cannot add component " + DT );
                    Hierarchy.HierarchyAdapterInstance.tempAdapterBlock = true;
                    Hierarchy.HierarchyAdapterInstance.needReinstall = true;
                    return null;
                }
                if ( oldS != s ) EditorSceneManager.SetActiveScene( oldS );
                
                // __go.hideFlags = flags;
                __go.gameObject.hideFlags = flags;
                __go.gameObject.name = "DescriptionHelperObject";
                
                
                
                if ( finded.Count > 0 )
                {
                
                    if ( oldDes )
                    {
                        #if LOG
                        MonoBehaviour.print("finded.  oldDes");
                        #endif
                        
                        
                        if ( oldDes.GetComponent( DT ) is IHashProperty )
                        {   var d = oldDes.GetComponent(DT) as IHashProperty;
                            var parse = (IHashProperty)__go;
                            // parse.SetHash1(d.GetHash1());
                            /* parse.SetHash1_Fix2_0( d.GetHash1_Fix2_0() );
                             #if LOG
                             MonoBehaviour.print(__go.last_custom.Count);
                             #endif
                             parse.SetHash2( d.GetHash2() );
                             parse.SetHash3( d.GetHash3() );
                             parse.SetHash4( d.GetHash4() );
                             parse.SetHash5_Fix2_0( d.GetHash5_Fix2_0() );
                             parse.SetHash6( d.GetHash6() );
                             parse.SetHash7_Fix2_0( d.GetHash7_Fix2_0() );
                             parse.SetHash8( d.GetHash8() );
                             parse.SetHash9( d.GetHash9() );
                             parse.SetHash10( d.GetHash10() );
                             parse.SetBookMarks( d.GetBookMarks() );
                             parse.HierarchyCache( d.HierarchyCache() );
                             parse.EnableRegistrator = d.EnableRegistrator;*/
                            
                            Adapter.SET_HASH_WITHOUT_LOCALID( parse, d );
                        }
                        
                        
                    }
                    
                    
                }
                /* Hierarchy.SetDirty(__go);
                 Hierarchy.SetDirty(__go.gameObject);*/
                Hierarchy.HierarchyAdapterInstance.SetDirtyDescription( (IHashProperty)__go, s );
                
                // Hierarchy.MarkSceneDirty(s);
                // EditorSceneManager.SaveScene(s);
                #if LOG
                MonoBehaviour.print("CREATE");
                
                #endif
                
                
                
            }
            
            
            
            
            adapter.PushActionToUpdate( () =>
            {   foreach ( var transform in finded )
                {   /* transform.gameObject.hideFlags = HideFlags.None;
                     transform.gameObject.gameObject.hideFlags = HideFlags.None;*/
                    if ( transform && transform.gameObject )
                        GameObject.DestroyImmediate( transform.gameObject, true );
                }
            } );
            
            
            return (IHashProperty)__go;
            
            
        }
        
        static internal void RemoveIHashPropertY( IHashProperty prop )
        {   if ( !descriptionPTRPlay.Values.Any( v => v == prop ) ) return;
            var first = descriptionPTRPlay.First(p => p.Value == prop).Key;
            descriptionWasSave.Remove( first );
            descriptionPTRPlay.Remove( first );
            descriptionPTREditor.Remove( first );
        }
        
        
        
        override public void OnSceneLoaded()
        {   if ( Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).SaveToScriptableObject == "FOLDER" )
            {   descriptionWasSave.Clear();
                descriptionPTRPlay.Clear();
                descriptionPTREditor.Clear();
                
                for ( int i = 0 ; i < EditorSceneManager.sceneCount ; i++ )
                {   var s = EditorSceneManager.GetSceneAt(i);
                    var result = Adapter.GetProjectHash(s);
                    if ( result == null ) continue;
                    ((HierarchyDescriptionHelper)result).InitializeLists( s );
                }
            }
        }
        
        static Dictionary<int, bool> descriptionWasSave = new Dictionary<int, bool>();
        static Dictionary<int, IHashProperty> descriptionPTRPlay = new Dictionary<int, IHashProperty>();
        static Dictionary<int, IHashProperty> descriptionPTREditor = new Dictionary<int, IHashProperty>();
        static Dictionary<int, IHashProperty> descriptionPTR()
        {   return Application.isPlaying ? descriptionPTRPlay : descriptionPTREditor;
        }
        
        
        
        internal static bool TryCreateBackupForCache( Scene s )
        {   return TryCreateBackupForCache( s.path );
        }
        internal static bool TryCreateBackupForCache( string scene )
        {   var oldName = Hierarchy.HierarchyAdapterInstance.GetStoredDataPathExternal(scene);
            bool reload = false;
            if ( System.IO.File.Exists( oldName + ".backup" ) )
            {   System.IO.File.Copy( oldName + ".backup", oldName );
                reload = true;
            }
            if ( System.IO.File.Exists( oldName + ".meta" + ".backup" ) )
            {   System.IO.File.Copy( oldName + ".meta" + ".backup", oldName + ".meta" );
                reload = true;
            }
            if ( reload )
            {   Hierarchy.HierarchyAdapterInstance.tempAdapterBlock = true;
            
                return true;
            }
            return false;
        }
        
        
        static   IHashProperty resres;
        
        internal static IHashProperty des( int sceneID )
        {   var t = Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance );
            if ( !t.GETWAS_NEW_INIT )
            {   t.GETWAS_NEW_INIT = true;
                t.SaveToScriptableObject = "SCENE";
                
                
            }
            
            
            if ( !t.TargetChecked ) Hierarchy_GUI.CheckSaveTargetForHierarchy( t );
            
            var scene = Adapter.GET_SCENE_BY_ID(sceneID);
            
            
            
            
            
            
            /* if (Hierarchy.adapter.DISABLE_DESCRIPTION( scene ))
             {
            #if UNITY_EDITOR
               throw new Exception( "CACHE ERROR" );
            #endif
               // throw new Exception( "CACHE ERROR" );
            #pragma warning disable
               //Hierarchy.adapter.logProxy.LogError( "the cache was turned off but you are trying to access it" );
               return null;
            #pragma warning restore
             }*/
            
            
            //var dpt = descriptionPTR();
            
            if ( t.SaveToScriptableObject == "FOLDER" )
            {   var scene_path = Adapter.GetScenePath(scene);
                if ( !descriptionWasSave.ContainsKey( sceneID ) ) descriptionWasSave.Add( sceneID, false );
                
                if ( !descriptionPTR().TryGetValue( sceneID, out resres ) || !resres.unityobject )
                {   /* var path = Hierarchy.adapter.PluginInternalFolder + "/_ SAVED DATA/" + scene_path.Remove(scene_path.LastIndexOf('.')) + ".asset";
                
                     var result = AssetDatabase.LoadAssetAtPath<ScriptableObject>( path ) as IHashProperty;*/
                    
                    descriptionPTR().Remove( sceneID );
                    
                    var result = Adapter.GetProjectHash(scene);
                    if ( result != null ) descriptionWasSave[sceneID] = true;
                    else
                    {
                    
                        /* if (Hierarchy_GUI.Initialize( Hierarchy.adapter ).SaveToScriptableObject == "FOLDER" && folderHash == null)
                        {   var oldName = Hierarchy.adapter.GetStoredDataPathExternal(s);
                        bool reload = false;
                        if (System.IO.File.Exists( oldName + ".backup"))
                        {   System.IO.File.Copy( oldName + ".backup", oldName  );
                         reload = true;
                        }
                        if (System.IO.File.Exists( oldName + ".meta" + ".backup"))
                        {   System.IO.File.Copy( oldName + ".meta" + ".backup", oldName + ".meta" );
                         reload = true;
                        }
                        if (reload)
                        {   Hierarchy.adapter.tempAdapterBlock = true;
                         Adapter.RequestScriptReload();
                         return;
                        }
                        }*/
                        
                        if ( TryCreateBackupForCache( scene ) )
                        {   AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
                            Adapter.RequestScriptReload();
                            return null;
                        }
                        result = HierarchyDescriptionHelper.CreateInstance<HierarchyDescriptionHelper>() as IHashProperty;
                    }
                    
                    descriptionPTR().Add( sceneID, result as IHashProperty );
                    descriptionPTR()[sceneID].SaveInFolder = "SCENE";
                    ((HierarchyDescriptionHelper)result).InitializeLists( scene );
                    
                    
                    resres = result;
                }
                
                
                /* TRY MOVE DATA FROM GAMEOBJET TO FOLDER */
                if ( !descriptionWasSave[sceneID] && !Application.isPlaying && !string.IsNullOrEmpty( scene_path ) )
                {   descriptionWasSave[sceneID] = true;
                
                    // var path = Hierarchy.adapter.PluginInternalFolder + "/_ SAVED DATA/" + scene_path.Remove(scene_path.LastIndexOf('.')) + ".asset";
                    var path = Hierarchy.HierarchyAdapterInstance.GetStoredDataPathInternal(scene);
                    
                    
                    Adapter.CreateFolders( path.Remove( path.LastIndexOf( '/' ) ) );
                    
                    AssetDatabase.CreateAsset( descriptionPTR()[sceneID].unityobject, path );
                    
                    AssetDatabase.ImportAsset( path );
                }
                
                
            }
            else
            {   if ( !descriptionPTR().TryGetValue( sceneID, out resres ) || !resres.component || !resres.gameObject )
                {   descriptionPTR().Remove( sceneID );
                
                
                
                
                    var __go = ((M_Descript)Hierarchy.HierarchyAdapterInstance.DescriptionModule).InitializeDescriptions(scene);
                    
                    if ( __go == null ) return null;
                    // if (__go.gameObject.hideFlags != flags)
                    {   __go.gameObject.hideFlags = flags /*HideFlags.None*/;
                        // __go.component.hideFlags = flags /*HideFlags.None*/;
                        //Hierarchy.SetDirty(__go);
                    }
                    #if UNITY_EDITOR
                    
                    /*    __go.hideFlags = HideFlags.None;
                        __go.gameObject.hideFlags = HideFlags.None;*/
                    #endif
                    
                    /*  Hierarchy.SetDirty(__go.gameObject);
                      Hierarchy.SetDirty(__go);*/
                    
                    if ( descriptionPTR().ContainsKey( sceneID ) ) __go.SaveInFolder = "FOLDER";
                    descriptionPTR().Remove( sceneID );
                    
                    descriptionPTR().Add( sceneID, __go );
                    
                    resres = __go;
                }
                
                
                /* TRY MOVE DATA FROM FOLDER TO GAMEOBJET */
            }
            
            
            
            
            
            if ( resres == null ) return null;
            
            //var r = descriptionPTR()[sceneID];
            var r = resres;
            if ( r.gameObject )     // r.component.hideFlags = flags;
            {   r.gameObject.hideFlags = flags;
            }
            
            return r;
        }
        
        
        
        
        
    }
}
}
