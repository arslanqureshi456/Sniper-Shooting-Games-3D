#if !UNITY_EDITOR
#define NAMESPACE
#endif

using System;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EModules;
using EModules.EModulesInternal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EModules.EModulesInternal
{




    [CustomEditor( typeof( HierarchyDescriptionHelper ) )]
    class HierarchyDescriptionHelperEditor : Editor
    {


        static HierarchyDescriptionHelper copiedScriptableObject;
        public override void OnInspectorGUI() {
            if ( Application.isPlaying ) return;

            if ( GUILayout.Button( "Copy" , GUILayout.Height( 30 ) ) ) copiedScriptableObject = target as HierarchyDescriptionHelper;
            GUI.enabled = copiedScriptableObject != null;
            var name = "Paste";
            if ( copiedScriptableObject ) name += " " + copiedScriptableObject.name;
            if ( GUILayout.Button( name , GUILayout.Height( 30 ) ) ) {
                var temp  = (target) as HierarchyDescriptionHelper;
                if ( temp ) {
                    Hierarchy.M_Descript.RemoveIHashPropertY( temp );
                    Undo.RecordObject( temp , "Paste DescriptionHelper" );
                    Adapter.SET_HASH_WITHOUT_LOCALID( copiedScriptableObject , temp );
                    Adapter.SET_HASH_LOCALIDONLY( copiedScriptableObject , temp );
                    EditorUtility.SetDirty( temp );

                    Hierarchy.HierarchyAdapterInstance.Again_Reloder_UsingWhenCopyPastOrAssets();

                    EditorUtility.SetDirty( temp );
                    Hierarchy.HierarchyAdapterInstance.MarkSceneDirty( SceneManager.GetActiveScene() );
                }
                /* var path = AssetDatabase.GetAssetPath(target);
                 if (temp && !string.IsNullOrEmpty( path ))
                 {
                   AssetDatabase.DeleteAsset( path );
                   Hierarchy.M_Descript.RemoveIHashPropertY( temp );
                   AssetDatabase.CreateAsset( copiedScriptableObject, path );
                 }*/
            }

            if ( Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ) && Hierarchy_GUI.Instance( Hierarchy.HierarchyAdapterInstance ).SaveToScriptableObject == "SCENE" ) {
                GUI.enabled = true;
                var temp  = (target) as HierarchyDescriptionHelper;
                GUILayout.Space( 10 );
                if ( temp && GUILayout.Button( "Apply to current Scene" , GUILayout.Height( 30 ) ) ) {
                    for ( int i = 0 ; i < EditorSceneManager.sceneCount ; i++ ) {
                        var s = EditorSceneManager.GetSceneAt(i);
                        if ( !s.IsValid() || !s.isLoaded ) continue;
                        var d =  Hierarchy.M_Descript.des(s.GetHashCode());
                        var assetPath = AssetDatabase.GetAssetPath(target);
                        assetPath = assetPath.Remove( assetPath.LastIndexOf( '.' ) );
                        var scenePath = Adapter.GetScenePath(s);
                        scenePath = scenePath.Remove( scenePath.LastIndexOf( '.' ) );
                        if ( d == null || !assetPath.EndsWith( scenePath ) ) continue;

                        Hierarchy.M_Descript.RemoveIHashPropertY( d );
                        Hierarchy.HierarchyAdapterInstance.SET_UNDO( d , "Apply to current Scene" );
                        Adapter.SET_HASH_WITHOUT_LOCALID( temp , d );
                        Hierarchy.HierarchyAdapterInstance.SetDirtyDescription( d , s );

                        Hierarchy.HierarchyAdapterInstance.EditorSceneManagerOnSceneOpening( null , OpenSceneMode.Single );
                    }

                }
            }
        }
    }

}


#if NAMESPACE


//namespace EModules.EModulesInternal {

#endif

[Serializable]
class LOCAL_BOOKMARKS_IntArray
{
    [SerializeField]
    public List<LOCAL_HASH34_IntArray> list;

    public LOCAL_BOOKMARKS_IntArray Clone() {
        var target = list ?? new List<LOCAL_HASH34_IntArray>();
        var result = target.Select(s => s.Clone()).ToList();
        return new LOCAL_BOOKMARKS_IntArray() { list = result };
    }
}

[Serializable]
class LOCAL_HASH34_IntArray
{
    [SerializeField]
    public serlong activeObject;
    [SerializeField]
    public List<serlong> list;

    public LOCAL_HASH34_IntArray Clone() {
        var result = new LOCAL_HASH34_IntArray();
        result.activeObject = activeObject;
        result.list = list != null ? list.ToList() : new List<serlong>();
        return result;
    }
}

class HierarchyDescriptionHelper : ScriptableObject, IHashProperty
{
    serlong sl;




    /*
    [MenuItem( "PluginEditor/savesl")]
    static void ks()
    {
    
        var ss = Resources.FindObjectsOfTypeAll<HierarchyDescriptionHelper>().First(s => s.name == EditorSceneManager.GetActiveScene().name);
        // var a = Initializator.Adapters[Initializator.HIERARCHY_NAME];
        //
        // var result = (GameObject)typeof(UnityEditor. PrefabUtility).GetMethod("GetGameObject", (System.Reflection.BindingFlags)(-1)).Invoke(null, new[] {Selection.gameObjects[0]});
        // Debug.Log(result.scene.name);
    
    
    
        string fakestring;
        long result;
        AssetDatabase.TryGetGUIDAndLocalFileIdentifier( Selection.gameObjects[0].transform, out fakestring, out result );
    //  AssetDatabase.file
        var sp =   Adapter.GetLocalIdentifierInFile( Selection.gameObjects[0] );
        Debug.Log(fakestring);
        Debug.Log(result);
    
        ss.  sl = sp;
        //Debug.Log(ss.LocalIdToGameObjecT(ss.  sl, EditorSceneManager.GetActiveScene()));
    }
    */

    // [MenuItem( "PluginEditor/laodsl")]
    static void ksasd() {   /* Debug.Log(Selection.gameObjects[0].GetInstanceID());
         Debug.Log(ContainsKey(Selection.gameObjects[0]));
        
         string ld;
         long ll;
         AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.gameObjects[0], out ld, out ll);
         Debug.Log(ll);
         Debug.Log(UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(Selection.gameObjects[0]));
         Debug.Log(UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(Selection.gameObjects[0]));
         Debug.Log(UnityEditor.PrefabUtility.GetPrefabObject(Selection.gameObjects[0]));
         Debug.Log(UnityEditor.PrefabUtility.GetPrefabInstanceHandle(Selection.gameObjects[0]));
         Debug.Log(UnityEditor.PrefabUtility.IsAnyPrefabInstanceRoot(Selection.gameObjects[0]));
         Debug.Log(UnityEditor.PrefabUtility.GetNearestPrefabInstanceRoot(Selection.gameObjects[0]));
         //Debug.Log(Adapter.GetLocalIdentifierInFile(Selection.gameObjects[0]));
         Debug.Log(Adapter.HierAdapter.GetHierarchyObjectByInstanceID(Selection.gameObjects[0].GetInstanceID()).go.name);
         Debug.Log(Adapter.HierAdapter.GetHierarchyObjectByInstanceID(Selection.gameObjects[0].GetInstanceID()).fileID);
         Debug.Log(Adapter.HierAdapter.GetHierarchyObjectByInstanceID(Selection.gameObjects[0].GetInstanceID()).GetHashCode());
         if (!ContainsKey(Selection.gameObjects[0])) Add(Selection.gameObjects[0], false);*/

        // Debug.Log(Hierarchy_GUI.Initialize(Adapter.HierAdapter).tempDDD.Count);
        //Hierarchy_GUI.Initialize(Adapter.HierAdapter).tempDDD.Add("ASD", 1);

        /* ObjectCacheHelper<Adapter.HierarchyObject, GameObject> asd = new ObjectCacheHelper<Adapter.HierarchyObject, GameObject>(Adapter.HierAdapter, "ASD");
         asd.Add(Adapter.HierAdapter.GetHierarchyObjectByInstanceID(Selection.gameObjects[0].GetInstanceID()), null);
         Debug.Log(asd.Contains(Adapter.HierAdapter.GetHierarchyObjectByInstanceID(Selection.gameObjects[0].GetInstanceID())));*/

        /***********
        Debug.Log(Adapter.HierAdapter.GetHierarchyObjectByInstanceID(Selection.gameObjects[0].GetInstanceID()).go);
        Debug.Log(Adapter.HierAdapter.GetHierarchyObjectByInstanceID(Selection.gameObjects[0].GetInstanceID()).fileID);
        Debug.Log(Adapter.HierAdapter.GetCorrespondingObjectFromSource(Selection.gameObjects[0]).name);
        Debug.Log(Adapter.HierAdapter.GetFileIDWithOutPrefabChecking(Adapter.HierAdapter.GetPrefabInstanceHandleGameObject(Selection.gameObjects[0]),
                  Selection.gameObjects[0]));*/

        /* var ss = Resources.FindObjectsOfTypeAll<HierarchyDescriptionHelper>().First(s => s.name == "test scene");
        var o = ss.LocalIdToGameObjecT(ss.  sl, EditorSceneManager.GetActiveScene());
        Selection.objects = new [] {o };*/
    }


    /*
    internal void TO_HASH(IHashProperty hash)
    {
      hash.EnableRegistrator = EnableRegistrator;
      hash.FavoritCategorySelected = FavoritCategorySelected;
    
      hash.SetBookMarks( GetBookMarks().ToList() );
      hash.SetHash10( GetHash10().ToList() );
      hash.SetHash1_Fix2_0( GetHash1_Fix2_0().ToList() );
      hash.SetHash2( GetHash2().ToList() );
      hash.SetHash3( GetHash3().ToList() );
      hash.SetHash4( GetHash4().ToList() );
      hash.SetHash5_Fix2_0( GetHash5_Fix2_0().ToList() );
      hash.SetHash6( GetHash6().ToList() );
      hash.SetHash7_Fix2_0( GetHash7_Fix2_0().ToList() );
      hash.SetHash8( GetHash8().ToList() );
      hash.SetHash9( GetHash9().ToList() );
      hash.HierarchyCache( HierarchyCache() );
    }*/

    /*   [SerializeField] List<int> LASTINSTID_Hash1_Fix2_0 = new List<int>();
       [SerializeField] List<int> LASTINSTID_Hash5_Fix2_0 = new List<int>();
       [SerializeField] List<int> LASTINSTID_Hash7_Fix2_0 = new List<int>();
       [SerializeField] List<int> LASTINSTID_Hash9 = new List<int>();*/
    [SerializeField] internal List<serlong> LOCALIDINFILE_HashFreeze = new List<serlong>();


    [SerializeField] internal List<serlong> LOCALIDINFILE_Hash1 = new List<serlong>();
    [SerializeField] internal List<serlong> LOCALIDINFILE_Hash5 = new List<serlong>();
    [SerializeField] internal List<serlong> LOCALIDINFILE_Hash7 = new List<serlong>();

    [SerializeField] internal List<serlong> LOCALIDINFILE_Hash1_Fix2_0 = new List<serlong>();
    [SerializeField] internal List<serlong> LOCALIDINFILE_Hash5_Fix2_0 = new List<serlong>();
    [SerializeField] internal List<serlong> LOCALIDINFILE_Hash7_Fix2_0 = new List<serlong>();
    [SerializeField] internal List<serlong> LOCALIDINFILE_Hash9 = new List<serlong>();

    [SerializeField] internal List<LOCAL_BOOKMARKS_IntArray> LOCALIDINFILE_Hash_BookMarks = new List<LOCAL_BOOKMARKS_IntArray>();
    [SerializeField] internal List<LOCAL_HASH34_IntArray> LOCALIDINFILE_Hash3 = new List<LOCAL_HASH34_IntArray>();
    [SerializeField] internal List<LOCAL_HASH34_IntArray> LOCALIDINFILE_Hash4 = new List<LOCAL_HASH34_IntArray>();


    [SerializeField] internal List<serlong> LOCALIDINFILE_m_Hash_IconImageKey = new List<serlong>();


    internal void SetDirty( Scene s ) {
        LOCALIDINFILE_Hash1 = Hash1.Select( p => (serlong)GetLocalIdInFile( p , s ) ).ToList();
        LOCALIDINFILE_Hash5 = Hash5.Select( p => (serlong)GetLocalIdInFile( p , s ) ).ToList();
        LOCALIDINFILE_Hash7 = Hash7.Select( p => (serlong)GetLocalIdInFile( p , s ) ).ToList();

        LOCALIDINFILE_HashFreeze = HashFreezeHashKeys.Select( p => (serlong)GetLocalIdInFile( p.go , s ) ).ToList();
        LOCALIDINFILE_Hash1_Fix2_0 = GetHash1_Fix2_0().Select( p => (serlong)GetLocalIdInFile( p.go , s ) ).ToList();
        LOCALIDINFILE_Hash5_Fix2_0 = GetHash5_Fix2_0().Select( p => (serlong)GetLocalIdInFile( p.go , s ) ).ToList();
        LOCALIDINFILE_Hash7_Fix2_0 = GetHash7_Fix2_0().Select( p => (serlong)GetLocalIdInFile( p.go , s ) ).ToList();
        LOCALIDINFILE_Hash9 = Hash9.Select( p => (serlong)GetLocalIdInFile( p , s ) ).ToList();

        LOCALIDINFILE_Hash_BookMarks.Clear();
        for ( int i = 0 ; i < Hash_BookMarks.Count ; i++ ) {
            LOCALIDINFILE_Hash_BookMarks.Add( new LOCAL_BOOKMARKS_IntArray() { list = new List<LOCAL_HASH34_IntArray>() } );
            if ( Hash_BookMarks[i].array == null ) continue;
            for ( int x = 0 ; x < Hash_BookMarks[i].array.Count ; x++ )
                LOCALIDINFILE_Hash_BookMarks[i].list = INT32_TO_HASH34( Hash_BookMarks[i].array , s );
        }

        LOCALIDINFILE_Hash3 = INT32_TO_HASH34( Hash3 , s );
        LOCALIDINFILE_Hash4 = INT32_TO_HASH34( Hash4 , s );


        LOCALIDINFILE_m_Hash_IconImageKey = m_GetHash_IconImageKey.Select( p => (serlong)GetLocalIdInFile( p.go , s ) ).ToList();

        EditorUtility.SetDirty( this );
    }

    List<LOCAL_HASH34_IntArray> INT32_TO_HASH34( List<Int32List> value , Scene s ) {
        var result = new List<LOCAL_HASH34_IntArray>();
        for ( int i = 0 ; i < value.Count ; i++ ) {
            result.Add( new LOCAL_HASH34_IntArray() { list = new List<serlong>() } );
            result[i].activeObject = GetLocalIdInFile( value[i].ActiveGameObject , s );
            if ( value[i].list == null ) continue;
            result[i].list.Clear();
            for ( int x = 0 ; x < value[i].list.Count ; x++ )
                result[i].list.Add( GetLocalIdInFile( value[i].list[x] , s ) );
        }
        return result;
    }

    void HASH34_TO_INT32( List<LOCAL_HASH34_IntArray> value , ref List<Int32List> result , Scene s ) {
        if ( result == null ) result = new List<Int32List>();

        // if (value.Count == 0) return;

        for ( int i = 0 ; i < value.Count ; i++ ) {
            if ( result.Count <= i ) result.Add( new Int32List() { list = new List<GameObject>() } );

            result[i].ActiveGameObject = LocalIdToGameObjecT( value[i].activeObject , s );
            if ( result[i].list == null ) result[i].list = new List<GameObject>();
            result[i].list.Clear();
            for ( int x = 0 ; x < value[i].list.Count ; x++ )
                result[i].list.Add( LocalIdToGameObjecT( value[i].list[x] , s ) );
        }
    }


    internal void InitializeLists( Scene scene ) {
        var allObjects = Utilities.AllSceneObjects( scene );

        if ( !Adapter.CHECK_UNITYEDITOR_SERIALIZATION_TYPE() ) return;

        Adapter.m_INITIALIZE_FILEID_FORPREFABS( scene );


        foreach ( var item in allObjects )
            GetLocalIdInFile( item , scene );

        if ( LOCALIDINFILE_Hash1.Count != 0 ) Hash1 = LOCALIDINFILE_Hash1.Select( id => LocalIdToGameObjecT( id , scene ) ).ToList();
        if ( LOCALIDINFILE_Hash5.Count != 0 ) Hash5 = LOCALIDINFILE_Hash5.Select( id => LocalIdToGameObjecT( id , scene ) ).ToList();
        if ( LOCALIDINFILE_Hash7.Count != 0 ) Hash7 = LOCALIDINFILE_Hash7.Select( id => LocalIdToGameObjecT( id , scene ) ).ToList();

        if ( LOCALIDINFILE_HashFreeze.Count != 0 ) HashFreezeHashKeys = LOCALIDINFILE_HashFreeze.Select( id => Adapter.GoGuidPairConstructor( LocalIdToGameObjecT( id , scene ) ) ).ToList();
        if ( LOCALIDINFILE_Hash1_Fix2_0.Count != 0 ) Hash1_Fix2_0 = LOCALIDINFILE_Hash1_Fix2_0.Select( id => Adapter.GoGuidPairConstructor( LocalIdToGameObjecT( id , scene ) ) ).ToList();
        if ( LOCALIDINFILE_Hash5_Fix2_0.Count != 0 ) Hash5_Fix2_0 = LOCALIDINFILE_Hash5_Fix2_0.Select( id => Adapter.GoGuidPairConstructor( LocalIdToGameObjecT( id , scene ) ) ).ToList();
        if ( LOCALIDINFILE_Hash7_Fix2_0.Count != 0 ) Hash7_Fix2_0 = LOCALIDINFILE_Hash7_Fix2_0.Select( id => Adapter.GoGuidPairConstructor( LocalIdToGameObjecT( id , scene ) ) ).ToList();
        if ( LOCALIDINFILE_Hash9.Count != 0 ) Hash9 = LOCALIDINFILE_Hash9.Select( h => LocalIdToGameObjecT( h , scene ) ).ToList();

        if ( Hash_BookMarks == null ) Hash_BookMarks = new List<Int32ListArray>();
        for ( int i = 0 ; i < LOCALIDINFILE_Hash_BookMarks.Count ; i++ ) {
            if ( Hash_BookMarks[i].array == null ) Hash_BookMarks[i].array = new List<Int32List>();
            if ( Hash_BookMarks[i].array.Count <= i ) Hash_BookMarks[i].array.Add( new Int32List() { list = new List<GameObject>() } );
            HASH34_TO_INT32( LOCALIDINFILE_Hash_BookMarks[i].list , ref Hash_BookMarks[i].array , scene );
        }
        HASH34_TO_INT32( LOCALIDINFILE_Hash3 , ref Hash3 , scene );
        HASH34_TO_INT32( LOCALIDINFILE_Hash4 , ref Hash4 , scene );


        if ( HashFreezeHashKeys.Count != HashFreezeHashValues.Count ) Resize( ref HashFreezeHashKeys , HashFreezeHashValues.Count );
        if ( GetHash1_Fix2_0().Count != Hash2.Count ) Resize( ref Hash1_Fix2_0 , Hash2.Count );
        if ( GetHash5_Fix2_0().Count != Hash6.Count ) Resize( ref Hash5_Fix2_0 , Hash6.Count );
        if ( GetHash7_Fix2_0().Count != Hash8.Count ) Resize( ref Hash7_Fix2_0 , Hash8.Count );
        if ( Hash9.Count != Hash10.Count ) Resize( ref Hash9 , Hash10.Count );


        if ( LOCALIDINFILE_m_Hash_IconImageKey.Count != 0 ) m_GetHash_IconImageKey = LOCALIDINFILE_m_Hash_IconImageKey.Select( id => Adapter.GoGuidPairConstructor( LocalIdToGameObjecT( id , scene ) ) ).ToList();
    }

    void Resize<T>( ref List<T> list , int count ) {
        var ar = list.ToArray();
        Array.Resize( ref ar , count );
        list = ar.ToList();
    }

    long GetLocalIdInFile( GameObject go , Scene s ) {
        if ( !go ) {
            return -1;
        }
        if ( !Adapter.ltg.ContainsKey( s.GetHashCode() ) ) Adapter.ltg.Add( s.GetHashCode() , new Adapter.cache_scene() );
        var gameobject_to_local = Adapter. ltg[s.GetHashCode()].gameobject_to_local;
        var local_to_gameobject = Adapter.ltg[s.GetHashCode()].local_to_gameobject;
        if ( !gameobject_to_local.ContainsKey( go.GetInstanceID() ) ) gameobject_to_local.Add( go.GetInstanceID() , Adapter.GetLocalIdentifierInFile( go ) );
        var result = gameobject_to_local[go.GetInstanceID()];
        if ( !local_to_gameobject.ContainsKey( result ) ) local_to_gameobject.Add( result , go );
        return result;
    }
    GameObject LocalIdToGameObjecT( long id , Scene s ) {
        if ( !Adapter.ltg.ContainsKey( s.GetHashCode() ) ) Adapter.ltg.Add( s.GetHashCode() , new Adapter.cache_scene() );
        var local_to_gameobject = Adapter. ltg[s.GetHashCode()].local_to_gameobject;
        if ( local_to_gameobject.ContainsKey( id ) ) return local_to_gameobject[id];
        return null;
    }
    bool RefreshIdForObject( GameObject go , Scene s , long newV ) {
        GetLocalIdInFile( go , s );
        var gameobject_to_local = Adapter. ltg[s.GetHashCode()].gameobject_to_local;
        var local_to_gameobject = Adapter.ltg[s.GetHashCode()].local_to_gameobject;
        gameobject_to_local[go.GetInstanceID()] = newV;
        if ( !local_to_gameobject.ContainsKey( newV ) ) local_to_gameobject.Add( newV , go );
        else return false;
        return true;
    }

    [SerializeField] List<GoGuidPair> HashFreezeHashKeys = new List<GoGuidPair>();
    [SerializeField] List<bool> HashFreezeHashValues = new List<bool>();


    [SerializeField] List<HierarchySnapShotArray> Hash_HierarchyCache = new List<HierarchySnapShotArray>();
    [SerializeField] List<Int32ListArray> Hash_BookMarks = new List<Int32ListArray>();

    [SerializeField] List<GameObject> Hash1 = new List<GameObject>();
    [SerializeField] List<GoGuidPair> Hash1_Fix2_0 = new List<GoGuidPair>();
    [SerializeField] List<string> Hash2 = new List<string>();

    [SerializeField] List<Int32List> Hash3 = new List<Int32List>();
    [SerializeField] List<Int32List> Hash4 = new List<Int32List>();

    [SerializeField] List<GameObject> Hash5 = new List<GameObject>();
    [SerializeField] List<GoGuidPair> Hash5_Fix2_0 = new List<GoGuidPair>();
    [SerializeField] List<SingleList> Hash6 = new List<SingleList>();

    [SerializeField] List<GameObject> Hash7 = new List<GameObject>();
    [SerializeField] List<GoGuidPair> Hash7_Fix2_0 = new List<GoGuidPair>();
    [SerializeField] List<SingleList> Hash8 = new List<SingleList>();

    [SerializeField] List<GameObject> Hash9 = new List<GameObject>();
    [SerializeField] List<SingleList> Hash10 = new List<SingleList>();

    [SerializeField] private bool _enableRegistrator;
    [SerializeField] private string _saveInFolder;

    [SerializeField] int _mFavoritCategorySelected = 0;



    public List<HierarchySnapShotArray> HierarchyCache() { return Hash_HierarchyCache; }
    public void HierarchyCache( List<HierarchySnapShotArray> hash ) { Hash_HierarchyCache = hash; }
    public List<Int32ListArray> GetBookMarks() { return Hash_BookMarks; }
    public void SetBookMarks( List<Int32ListArray> hash ) { Hash_BookMarks = hash; }

    public bool EnableRegistrator {
        get { return false; }
        set { }
    }

    public Component component {
        get { return null; ; }
    }

    public GameObject gameObject {
        get { return null; }
    }

    public UnityEngine.Object unityobject {
        get { return this; }
    }

    public int FavoritCategorySelected { get { return _mFavoritCategorySelected; } set { _mFavoritCategorySelected = value; } }


    public string SaveInFolder {
        get { return _saveInFolder; }
        set {

            _saveInFolder = value;
        }
    }


    public List<GoGuidPair> GetFreezeHashKeys() { return HashFreezeHashKeys; }
    public void SetFreezeHashKeys( List<GoGuidPair> hash ) { HashFreezeHashKeys = hash; }
    public List<bool> GetFreezeHashValues() { return HashFreezeHashValues; }
    public void SetFreezeHashValues( List<bool> hash ) { HashFreezeHashValues = hash; }


    /*public List<GameObject> GetHash1() { return Hash1; }
    public void SetHash1(List<GameObject> hash) { Hash1 = hash; }*/
    public List<GoGuidPair> GetHash1_Fix2_0() {
        if ( Hash1.Count != 0 ) {
            Hash1_Fix2_0 = Hash1.Select( h => (GoGuidPair)h ).ToList();
            Hash1.Clear();
        }
        return Hash1_Fix2_0;
    }
    public void SetHash1_Fix2_0( List<GoGuidPair> hash ) { Hash1_Fix2_0 = hash; }
    public List<string> GetHash2() { return Hash2; }
    public void SetHash2( List<string> hash ) { Hash2 = hash; }

    public List<Int32List> GetHash3() { return Hash3; }
    public void SetHash3( List<Int32List> hash ) { Hash3 = hash; }
    public List<Int32List> GetHash4() { return Hash4; }
    public void SetHash4( List<Int32List> hash ) { Hash4 = hash; }

    /*public List<GameObject> GetHash5() { return Hash5 ?? (Hash5 = new List<GameObject>()); }
    public void SetHash5(List<GameObject> hash) { Hash5 = hash; }*/
    public List<GoGuidPair> GetHash5_Fix2_0() {
        if ( Hash5.Count != 0 ) {
            Hash5_Fix2_0 = Hash5.Select( h => (GoGuidPair)h ).ToList();
            Hash5.Clear();
        }
        return Hash5_Fix2_0;
    }
    public void SetHash5_Fix2_0( List<GoGuidPair> hash ) { Hash5_Fix2_0 = hash; }
    public List<SingleList> GetHash6() { return Hash6 ?? (Hash6 = new List<SingleList>()); }
    public void SetHash6( List<SingleList> hash ) { Hash6 = hash; }

    /*public List<GameObject> GetHash7() { return Hash7 ?? (Hash7 = new List<GameObject>()); }
    public void SetHash7(List<GameObject> hash) { Hash7 = hash; }*/
    public List<GoGuidPair> GetHash7_Fix2_0() {
        if ( Hash7.Count != 0 ) {
            Hash7_Fix2_0 = Hash7.Select( h => (GoGuidPair)h ).ToList();
            Hash7.Clear();
        }
        return Hash7_Fix2_0;
    }
    public void SetHash7_Fix2_0( List<GoGuidPair> hash ) { Hash7_Fix2_0 = hash; }
    public List<SingleList> GetHash8() { return Hash8 ?? (Hash8 = new List<SingleList>()); }
    public void SetHash8( List<SingleList> hash ) { Hash8 = hash; }

    public List<GameObject> GetHash9() { return Hash9 ?? (Hash9 = new List<GameObject>()); }
    public void SetHash9( List<GameObject> hash ) { Hash9 = hash; }
    public List<SingleList> GetHash10() { return Hash10 ?? (Hash10 = new List<SingleList>()); }
    public void SetHash10( List<SingleList> hash ) { Hash10 = hash; }



    [SerializeField] List<GoGuidPair> m_GetHash_IconImageKey = new List<GoGuidPair>();
    [SerializeField] List<Int32List> m_GetHash_IconImageValue = new List<Int32List>();
    public List<GoGuidPair> GetHash_IconImageKey() { return m_GetHash_IconImageKey ?? (m_GetHash_IconImageKey = new List<GoGuidPair>()); }
    public void SetHash_IconImageKey( List<GoGuidPair> hash ) { m_GetHash_IconImageKey = hash; }
    public List<Int32List> GetHash_IconImageValue() { return m_GetHash_IconImageValue ?? (m_GetHash_IconImageValue = new List<Int32List>()); }
    public void SetHash_IconImageValue( List<Int32List> hash ) { m_GetHash_IconImageValue = hash; }

}



#if NAMESPACE
//}
#endif
