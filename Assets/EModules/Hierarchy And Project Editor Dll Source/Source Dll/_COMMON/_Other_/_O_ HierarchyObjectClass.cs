#if UNITY_EDITOR
	#define HIERARCHY
	#define PROJECT
#endif

using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PROJECT
	using EModules.Project;
#endif
//namespace EModules


namespace EModules.EModulesInternal


{

class NameSpace {

}


/* internal interface ISelectionBehaviour
 {

 }

 internal interface ICallBackModule
 {
     object obj { get; set; }
     Type typeFillter { get; set; }
     FillterData.FillterData_Inputs callFromExternal_objects { get; set; }
     void Draw( Rect rect , GameObject gameObject );
 }*/

internal interface ISettings {

}

internal interface IMethodsInterface {
	/*  Dictionary<string , int> sortType2108193717 { get; set; }
	  Dictionary<string , bool> SortDescending151046911 { get; set; }
	  float FONTSIZE { get; }
	  float LINE_HEIGHT { get; }
	  bool ENABLE_ALL { get; set; }
	  bool ENABLE_BOTTOMDOCK { get; }
	  bool ENABLE_LEFTDOCK_FIX { get; }
	  bool ENABLE_RIGHTDOCK_FIX { get; }
	  bool PLAYMODE_HideRightPanel { get; }
	  bool PIN_FILLTERWIN_BYDEFAULT { get; }
	  Adapter.Module[] modules { get; }
	  bool BOTTOM_AUTOHIDE { get; }
	  int BOTTOM_MAXSCENECOUNT { get; }
	  bool ENABLE_WINDOW_ANIMATION { get; }
	  bool TEMP_DISABLE_CACHE { get; set; }
	  bool USE_CUSTOMMODULES { get; }
	  bool LOCK_SELECTION { get; }*/
	
	void InitModules();
	IHashProperty des( Scene scene );
	IHashProperty des( int scene );
	
	void RegistrateDescription( IDescriptionRegistrator o );
	
	IModuleOnnector_M_Vertices M_Vertices { get; }
	// Adapter.M_Colors M_Colors { get; }
	//  IModuleOnnector_M_CustomIcons M_CustomIcons { get; }
	IModuleOnnector_M_PlayModeKeeper M_PlayModeKeeper { get; }
	//   IModuleOnnector_M_Decription M_Descript { get; }
	IModuleOnnector_M_Freeze M_Freeze { get; }
	
	void CONTEXTMENU_STATICMODULES( GenericMenu menu );
}

/*
internal interface IModuleOnnector_M_Decription {
    // void Clear( );
    //A void CalcBroadCast( );
    void TrySaveCustomRegistrator<TValue>( Adapter.HierarchyObject o, TValue value, Adapter.CacherType type ) where TValue : class;
    bool HasKey(int scene, Adapter.HierarchyObject o );
    void SetValue( string res, int scene, Adapter.HierarchyObject needRestoreGameObjectName );
    string GetValue(int scene, Adapter.HierarchyObject needRestoreGameObjectName );
    void UpdateSwitchRegistratorEnable( );
    void RegistrateDescription(IDescriptionRegistrator reg, Adapter adapter);
    void TrySaveHiglighterRegistrator(Adapter.HierarchyObject o, Adapter.TempColorClass colors32);

}*/

internal interface IModuleOnnector_M_Vertices {
	void Clear();
	Dictionary<int, double> updateTimer { get; }
	void CalcBroadCast();
}

internal interface IModuleOnnector_M_Freeze {
}

/* internal interface IModuleOnnector_M_Color {
     Color32? GetLabelColor(Adapter.HierarchyObject activeGameObject);
     TempColorClass needdrawGetColor( Adapter.HierarchyObject activeGameObject );
     Color GetColorForObject(Adapter.HierarchyObject activeGameObject);
     Texture GetImageForObject_OnlyCacher( Adapter.HierarchyObject activeGameObject );
     //    TempColorClass MCOLOR_NEEDGETCOLOR( GameObject gameObject );
     //ObjectCacheHelper<Adapter.HierarchyObject , SingleList> IconColorCacher { get; }
     void IconColorCacherSetValue(  SingleList list, int scene, Adapter.HierarchyObject o, bool saveRegistrator);
     string IconColorCacherGetValueToString(int scene, Adapter.HierarchyObject o );

     void SetValue( TempColorClass c, int scene, Adapter.HierarchyObject needRestoreGameObjectName );
     string GetValueToString(int scene, Adapter.HierarchyObject needRestoreGameObjectName );
     void DrawBackground(EditorWindow w, Rect selectionRect, Adapter.HierarchyObject o);
     void DrawBackground(EditorWindow w, Rect selectionRect, Adapter.HierarchyObject o, int labelOverride);
 }*/

internal interface IModuleOnnector_M_CustomIcons {
	EventType? useEvent { get; set; }
	Dictionary<int, double> updateTimer { get; }
}
internal interface IModuleOnnector_M_PlayModeKeeper {
}














internal partial class Adapter {

	Vector2 __HierWinScrollPos;
	internal Vector2 HierWinScrollPos
	{	get { return __HierWinScrollPos; }
	
		set
		{	__HierWinScrollPos = value;
			// __HierWinScrollPos.x = Mathf.CeilToInt( __HierWinScrollPos.x );
			// __HierWinScrollPos.y = Mathf.CeilToInt( __HierWinScrollPos.y );
			
		}
	}
	Vector2? __oldScroll;
	Vector2? oldScroll
#pragma warning disable
	{	get { return Adapter.NEW_RELOAD ? null : __oldScroll; }
	
		set { __oldScroll = value; }
	}
#pragma warning restore
	
	
	
	
	internal bool PLAYMODE_RELOADED = false;
	bool __PLAYMODECHANGE = false;
	internal bool PLAYMODECHANGE
	{	set
		{	if ( !value )
			{	oldScroll = null;
				PLAYMODE_RELOADED = false;
				
			}
			
			// Debug.Log( value + " " + EditorApplication.isCompiling);
			__PLAYMODECHANGE = value;
			
		}
		
		get
		{	return __PLAYMODECHANGE;
		}
	}
	
	
	
	internal int M_SetActive_WIDTH
	{	get { return SETACTIVE_POSITION == 1 || !ENABLE_ACTIVEGMAOBJECTMODULE ? 0 : (SETACTIVE_POSITION == 2 ? 8 : 20); }
	}
	
	
	
	private bool guichange = false;
	internal float deltaTime = 0;
	internal GUISkin DEFAUL_SKIN;
	
	
	
	private Rect __currentClipRect;
	private Vector2 v2;
	public Rect currentClipRect
	{	get { return __currentClipRect; }
	
		set
		{	v2.Set( value.x, value.y );
			v2 = GUIUtility.GUIToScreenPoint( v2 );
			__currentClipRect.Set( v2.x, v2.y, value.width, value.height );
			// MonoBehaviour.print("SET " + __currentClipRect);
			
		}
	}
	private Vector2 GUIToScreenPoint( float x, float y )
	{	v2.Set( x, y );
		return GUIUtility.GUIToScreenPoint( v2 );
	}
	private bool DISABLE_PLAY_REPAINT
	{	get { return Application.isPlaying; }
	}
	
	
	internal void Destroy()
	{	EditorApplication.update -= UpdateCb;
	
		if ( pluginID == Initializator.HIERARCHY_ID ) EditorApplication.hierarchyWindowItemOnGUI -= hier_Main;
		
		if ( pluginID == Initializator.PROJECT_ID ) EditorApplication.projectWindowItemOnGUI -= proj_Main;
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	internal class HierarchyObject_ProjectExt : ICloneable {
		//internal UnityEngine.Object obj;
		internal string guid;
		internal string assetPath;
		internal string assetFolder;
		internal string assetName;
		internal string fileExtension;
		internal bool IsFolder;
		
		internal Dictionary<int, HierarchyObject> nonMainAssets;
		//#pragma warning disable
		internal bool IsMainAsset = true;
		// internal int? childCount;
		//#pragma warning restore
		
		internal int? parentCount;
		public int sibling;
		
		public object Clone()
		{	return MemberwiseClone();
		}
	}
	
	
	
	
	
	
	
	
	
	
	internal class HierarchyObject : IEqualityComparer<HierarchyObject>, IComparable<HierarchyObject>, IEquatable<HierarchyObject>, ICloneable {
	
	
	
	
	
		public object Clone()
		{	var result = (HierarchyObject)MemberwiseClone();
			//  result.localTempColor = new TempColorClass().AssignFromList( new SingleList() { list = Enumerable.Repeat( 0, 9 ).ToList() }, true );
			// result.localTempColor = new TempColorClass().AssignFromList( 0, true );
			result.m_fileID = null;
			result.project = (HierarchyObject_ProjectExt)result.project.Clone();
			return result;
		}
		
		internal HierarchyObject( int pluginID )
		{	this.pluginID = pluginID;
		
		}
		
		internal bool internalIcon;
		internal TempColorClass drawIcon;
		internal bool cache_prefab;
		internal int switchType;
		internal    Color BACKGROUNDEsourceBgColorD;
		internal int BACKGROUNDED = 0;
		internal int FLAGS = 0;
		internal Rect? BG_RECT = null;
		internal UnityEngine.Object _GetHardLoadObject;    // return InternalEditorUtility.GetLoadedObjectFromInstanceID( id );
		internal UnityEngine.Object GetHardLoadObject()     // return InternalEditorUtility.GetLoadedObjectFromInstanceID( id );
		{
		
			if ( pluginID == 0 ) return go;
			
			if ( _GetHardLoadObject  == null) _GetHardLoadObject = EditorUtility.InstanceIDToObject( id );
			
			return _GetHardLoadObject;
		}
		internal TempColorClass localTempColor;
		internal int pluginID;
		internal int id;
		/*internal long fileID
		{   get
		    {   if (noFileID) return id;
		        if (!_fileID.HasValue)
		        {   if ( Hierarchy_GUI.HierarchySettings.PrefabIDMode != Hierarchy_GUI.PrefabIDModeEnum.MergedInstances) _fileID = id;
		            else   _fileID = Adapter.GetLocalIdentifierInFile(go);
		        }
		        return _fileID.Value;
		    }
		}*/
		
		long? m_fileID;
		internal long fileID
		{	get
			{
			
				/*if (go && go.name == "PACKER")
				{
				
				  id = id;
				}*/
				
				if ( m_fileID.HasValue ) return m_fileID.Value;
				
				if ( pluginID != 0 ) m_fileID = project.guid.GetHashCode();
				else
					m_fileID = go ? Adapter.HierAdapter.GetFileIDWithOutPrefabChecking( Initializator.AdaptersByID[pluginID].GetPrefabInstanceHandle( go ) as GameObject, go ) : 0;
					
				// Debug.Log()
				return m_fileID.Value;
			}
			
			set
			{	if ( value == 0 ) return;
			
				m_fileID = value;
			}
		}
		
		
		internal HierarchyObject_ProjectExt project;
		
		internal GameObject go;
		
		
		internal int scene
		{	get
			{	if ( pluginID == Initializator.HIERARCHY_ID ) return go.scene.GetHashCode();
			
				return -1;
			}
		}
		
		
		internal HierarchyObject[] GetAllParents( Adapter adapter )
		{	var result = new List<HierarchyObject>();
			var current = parent( adapter );
			
			while ( current != null )
			{	result.Add( current );
				current = current.parent( adapter );
			}
			
			return result.ToArray();
		}
		
		internal HierarchyObject root( Adapter adapter )
		{	return GetAllParents( adapter ).LastOrDefault() ?? this;
		}
		internal HierarchyObject parent( Adapter adapter )
		{	if ( pluginID == Initializator.HIERARCHY_ID )
			{	if ( !go ) return null;
			
				var p = go.transform.parent;
				
				if ( !p ) return null;
				
				return adapter.GetHierarchyObjectByInstanceID( p.gameObject );
			}
			
			if ( project.assetFolder == null || project.assetFolder == "Assets" ) return null;
			
			if ( !project.IsMainAsset ) return adapter.m_PathToObject[project.assetPath];
			
			if ( !adapter.m_PathToObject.ContainsKey( project.assetFolder ) )
			{	var guid = AssetDatabase.AssetPathToGUID(project.assetFolder);
			
				if ( string.IsNullOrEmpty( guid ) ) return null;
				
				var load = adapter.GetHierarchyObjectByGUID(ref guid, ref project.assetFolder);
				
				if ( !adapter.m_PathToObject.ContainsKey( project.assetFolder ) ) adapter.m_PathToObject.Add( project.assetFolder, load );
				
				if ( load == null ) return null;
			}
			
			return adapter.m_PathToObject[project.assetFolder];
		}
		
		internal int GetSiblingIndex( Adapter adapter )
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go.transform.GetSiblingIndex();
		
		
		
			/*if ( project.childCount.HasValue ) return project.childCount.Value;
			//if ( project.assetName != null ) return (project.childCount = 0).Value;
			if ( !project.IsFolder ) return 0;
			return (project.childCount = .Count).Value;*/
			//if ( project.assetName == "BARRACK WOOD 1.mat" ) Debug.Log( adapter.m_PathToChildrens[project.assetFolder].Count );
			return project.sibling;
			
			//return adapter.m_PathToChildrens[project.assetFolder].IndexOfKey( project.assetName );
			
			/*return project.assetFolder
			
			
			if ( project.childCount.HasValue ) return project.childCount.Value;
			//if ( project.assetName != null ) return (project.childCount = 0).Value;
			if ( !project.IsFolder ) return 0;
			return (project.childCount = adapter.m_PathToChildrens[project.guid].Count).Value;*/
		}
		
		int? __mBackedLastSib;
		int backedLastSib( Adapter adapter )
		{	if ( !__mBackedLastSib.HasValue )
			{	var item = GetTreeItem( adapter );
			
				if ( item == null )
				{	return 0;
				}
				
				var children = adapter.m_data_treeitem_children.GetValue( item, null ) as IList;
				
				if ( children == null || children.Count == 0 ) __mBackedLastSib = 0;
				else __mBackedLastSib = (int)adapter.m_data_treeitem_m_ID.GetValue( children[children.Count - 1] );
			}
			
			return __mBackedLastSib.Value;
		}
		
		
		
		
		internal bool IsLastSibling( Adapter adapter )
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go.transform.parent && go.transform.GetSiblingIndex() == go.transform.parent.childCount - 1;
		
			var p = parent(adapter);
			return p != null && p.backedLastSib( adapter ) == id;
		}
		
		int treeReloadId;
		static UnityEditor.IMGUI.Controls.TreeViewItem tiv;
		Dictionary<object,  UnityEditor.IMGUI.Controls.TreeViewItem> __ti = new
		Dictionary<object,  UnityEditor.IMGUI.Controls.TreeViewItem>();
		UnityEditor.IMGUI.Controls.TreeViewItem emptyreeitem = new UnityEditor.IMGUI.Controls.TreeViewItem();
		
		static MethodInfo  FindItem;
		static PropertyInfo  m_RootItem;
		static object[] ob_arr2 = new object[2];
		
		internal UnityEditor.IMGUI.Controls.TreeViewItem GetTreeItem( Adapter adapter )
		{	var tree = adapter.m_TreeView( adapter.window() );
		
			/* if (treeReloadId != adapter.TreeReloaderID)
			 {   treeReloadId = adapter.TreeReloaderID;
			     __ti.Clear();
			 }*/
			if ( tree == null ) return null;
			
			if ( !__ti.TryGetValue( tree, out tiv ) || tiv == null || tiv.id != id )
			{	var data = adapter.m_data.GetValue(tree, null );
			
			
			
				if (FindItem == null )
				{	var asemb = typeof( EditorWindow ).Assembly;
					FindItem = asemb.GetType( "UnityEditor.IMGUI.Controls.TreeViewUtility" ).GetMethod( "FindItem", (BindingFlags)(-1) );
					/*  var bases = data.GetType();
					  while ( bases.BaseType != null ) bases = bases.BaseType;
					  m_RootItem = bases.GetProperty( "root", (BindingFlags)(-1) );*/
					m_RootItem = asemb.GetType( "UnityEditor.IMGUI.Controls.ITreeViewDataSource" ).GetProperty( "root", (BindingFlags)(-1) );
				}
				
				ob_arr2[0] = id;
				ob_arr2[1] = m_RootItem.GetValue(data, null);
				var res =   FindItem.Invoke( null, ob_arr2  ) as UnityEditor.IMGUI.Controls.TreeViewItem;
				
				/*ob_arr[0] = id;
				var res = adapter.m_dataFindItem.Invoke( data, ob_arr ) Item;
				*/
				
				if ( __ti.ContainsKey( tree ) )
					__ti[tree] = res;
				else
					__ti.Add( tree, res );
			}
			
			
			return tiv ?? emptyreeitem;
		}
		
		public int parentCount()
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go.GetComponentsInParent<Transform>( true ).Length;
		
			if ( project.parentCount.HasValue ) return project.parentCount.Value;
			
			project.parentCount = Math.Max( 0, project.assetPath.ToCharArray().Count( c => c == '/' ) - 1 );
			return project.parentCount.Value;
		}
		
		internal bool ParentIsNull()
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go.transform.parent == null;
		
			return project.assetFolder == "Assets";
		}
		static SortedList<string, HierarchyObject> tl;
		static object[] ob_arr = new object[1];
		int? backedChild;
		internal int ChildCount( Adapter adapter )
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go ? go.transform.childCount : 0;
		
			/*if ( !project.IsFolder ) return 0;
			adapter.GetPathToChildrens( ref project.assetPath , out tl );
			return tl.Count;*/
			
			if ( backedChild.HasValue ) return backedChild.Value;
			
			var data = adapter.m_data.GetValue(adapter.m_TreeView( adapter.window() ), null );
			ob_arr[0] = id;
			var item =  adapter.m_dataFindItem.Invoke(data, ob_arr );
			
			if ( item == null ) return 0;
			
			/* ob_arr[0] = row;
			 var item =  adapter.m_dataGetItem.Invoke(data, ob_arr );
			 TreeViewUtility.*/
			var children = adapter.m_data_treeitem_children.GetValue( item, null ) as IList;
			return (backedChild = (children != null ? children.Count : 0)).Value;
			/*
			if (project.childCount.HasValue) return project.childCount.Value;
			//if ( project.assetName != null ) return (project.childCount = 0).Value;
			if (!project.IsFolder) return 0;
			//adapter.GetPathToChildrens( ref project.assetPath , out tl );
			
			//project.childCount = adapter.m_PathToChildrens.ContainsKey( project.assetPath ) ? adapter.m_PathToChildrens[project.assetPath].Count : 0;
			
			project.childCount =
			    Directory.GetDirectories( UNITY_SYSTEM_PATH + project.assetPath, "*.*", SearchOption.TopDirectoryOnly ).Count( d => File.Exists( d + ".meta" ) ) +
			    Directory.GetFiles( UNITY_SYSTEM_PATH + project.assetPath, "*.*", SearchOption.TopDirectoryOnly )
			        .Count( f => !f.EndsWith( ".meta" ) );
			
			//if (project.assetName == "lightmaps") Debug.Log(project.childCount.Value);
			
			return (project.childCount).Value;*/
		}
		
		public override string ToString()
		{
		
			if ( pluginID == 0 ) return go ? go.name : "";
			
			
			return project != null ? !string.IsNullOrEmpty( project.assetName ) ? project.assetName : project.assetPath : "";
		}
		
		public string name { get { return ToString(); } }
		
		internal bool Validate()
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go;
		
			return true;
			// return !string.IsNullOrEmpty( AssetDatabase.AssetPathToGUID( project.assetPath ) );
		}
		internal bool Validate( int sceneHash )
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go && go.scene.GetHashCode() == sceneHash;
		
			return true;
			// return !string.IsNullOrEmpty( AssetDatabase.AssetPathToGUID( project.assetPath ) );
		}
		
		internal  bool filterAssigned = false;
		internal TempColorClass filterColor;
		
		
		internal  bool invalide = false;
		internal bool Validate( bool checkScene )
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go && go.scene.IsValid();
		
			return true;
			// return !string.IsNullOrEmpty( AssetDatabase.AssetPathToGUID( project.assetPath ) );
		}
		
		Type cachedType = null;
		internal TempColorClass MIXINCOLOR;
		
		internal Type GET_TYPE( Adapter adapter )
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go.GetType();
		
			if ( cachedType == null )     //InternalEditorUtility.GetTypeWithoutLoadingObject
			{	cachedType = adapter.SWITCHER_GET_TYPE( id );
			
				if ( cachedType == null ) cachedType = typeof( UnityEngine.Object );
				
				/* var ob = InternalEditorUtility.GetObjectFromInstanceID(id);
				 if (!ob) cachedType = typeof( UnityEngine.Object );
				 else cachedType = ob.GetType();*/
			}
			
			return cachedType;
		}
		
		
		internal bool Active()
		{	if ( pluginID == Initializator.HIERARCHY_ID ) return go && go.activeInHierarchy;
		
			return true;
		}
		
		public static bool operator ==( HierarchyObject x, HierarchyObject y )
		{	if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) ) return ReferenceEquals( x, null ) && ReferenceEquals( y, null );
		
			return x.Equals( y );
		}
		
		public static bool operator !=( HierarchyObject x, HierarchyObject y )
		{	return !(x == y);
		}
		
		public bool Equals( HierarchyObject x, HierarchyObject y )
		{	if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) ) return ReferenceEquals( x, null ) && ReferenceEquals( y, null );
		
			//if ( x == null || y == null ) return x == null && y == null;
			return x.Equals( y );
		}
		
		public int GetHashCode( HierarchyObject obj )
		{	return obj.GetHashCode();
		}
		
		public int CompareTo( HierarchyObject other )
		{	if ( pluginID == 0 ) return (int)((fileID - other.fileID) % int.MaxValue);
		
			return project.guid.CompareTo( other.project.guid );
		}
		
		public bool Equals( HierarchyObject other )
		{	if ( ReferenceEquals( other, null ) ) return false;
		
			if ( pluginID == 0 ) return fileID == other.fileID;
			
			if ( project == null || other.project == null ) return false;
			
			return project.guid == other.project.guid;
		}
		public override int GetHashCode()
		{
		
			if ( pluginID == 0 ) return (int)(fileID % int.MaxValue);
			else return project.guid.GetHashCode();
		}
		
		public override bool Equals( object obj )
		{	if ( ReferenceEquals( obj, null ) ) return false;
		
			return GetHashCode() == obj.GetHashCode();
		}
		
		
	}
	
	
	
	
	static string m_UNITY_SYSTEM_PATH;
	internal static string UNITY_SYSTEM_PATH { get { return m_UNITY_SYSTEM_PATH ?? (m_UNITY_SYSTEM_PATH = Application.dataPath.Remove( Application.dataPath.Length - "Assets".Length )); } }
	
	
	
	Dictionary<int, string> m_instanceIdToGUID = new Dictionary<int, string>();
	Dictionary<int, string> m_instanceIdToPATH = new Dictionary<int, string>();
	Dictionary<int, HierarchyObject> m_instanceIdToHierObject = new Dictionary<int, HierarchyObject>();
	
	internal class scanneddata {
		internal Dictionary<string, int> extensions;
		internal List<scanneditem> scanneditems;
	}
	
	internal class scanneditem {
		Adapter adapter;
		internal HierarchyObject CurrentObject = null;
		internal scanneditem( Adapter adapter, string name, string rootpath, string fullPath, string[] folders )
		{	if ( name.LastIndexOf( '.' ) == -1 )
			{	m_name = name;
				extension = "";
			}
			
			else
			{	m_name = name.Remove( name.LastIndexOf( '.' ) );
				extension = name.Substring( name.LastIndexOf( '.' ) + 1 ).ToLower();
			}
			
			m_folders = folders;
			Array.Resize( ref m_folders, m_folders.Length - 1 );
			this.fullPath = fullPath.Trim( '/' );
			this.rootPath = rootpath.Trim( '/' );
			this.adapter = adapter;
			
		}
		
		internal HierarchyObject GetFoldByIndex( int index )
		{	if ( foldersObjects == null )
			{	foldersObjects = new HierarchyObject[folders.Length];
				var cp = rootPath;
				
				for ( int i = 0 ; i < folders.Length ; i++ )
				{	cp += '/' + folders[i];
					var guid = AssetDatabase.AssetPathToGUID( cp );
					foldersObjects[i] = adapter.GetHierarchyObjectByGUID( ref guid, null );
				}
				
			}
			
			return foldersObjects[index];
		}
		
		internal int DEEP { get { return folders.Length; } }
		string m_name;
		internal string extension;
		internal string name
		{	get { return m_name; }
		}
		
		internal string fullPath;
		internal string rootPath;
		HierarchyObject[] foldersObjects;
		string[] m_folders;
		internal string[] folders
		{	get
			{	return m_folders;
			}
		}
	}
	internal Dictionary<string, scanneddata> scanned_folder = new Dictionary<string, scanneddata>();
	
	internal void ClearHierarchyObjects(bool clearObjects)
	{	//  Utilities.ObjectContent_cache.Clear();
		//  Utilities.ObjectContent_Objectcache.Clear();
		Utilities.cache_ObjectContent_byType.Clear();
		Utilities.cache_ObjectContent_byId.Clear();
		
		//Debug.Log( "Clea" );
		if ( clearObjects )
		{	m_instanceIdToGUID.Clear();
			m_instanceIdToPATH.Clear();
			m_instanceIdToHierObject.Clear();
			m_GuidToHierObject.Clear();
			m_fakeList.Clear();
			m_PathToChildrens.Clear();
			m_PathToObject.Clear();
			scanned_folder.Clear();
		}
		
		
		if ( OnClearObjects != null ) OnClearObjects();
		
		
		ColorModule.ClearCacheAdditional();
	}
	
	internal Action OnClearObjects = null;
	internal bool IsSceneHaveToSave( HierarchyObject o )
	{	if ( pluginID != 0 || Hierarchy_GUI.Instance( this ).SaveToScriptableObject != "FOLDER" ) return false;
	
		if ( o.fileID != 0 ) return false;
		
		UnityEditor.EditorUtility.DisplayDialog(
		    "External Folder Mode",
		    o.name + "\nIn the external folder mode, you have to save the scene before applying the parameters to the new created objects",
		    "Ok" );
		return true;
	}
	internal void OnSceneSaved( Scene s )
	{	if ( pluginID != 0 || Hierarchy_GUI.Instance( this ).SaveToScriptableObject != "FOLDER" ) return;
	
		ClearHierarchyObjects(true);
		m_GET_FILEID_FOR_PFEFAB_CACHE.Clear();
		cached_so.Clear();
	}
	/* [MenuItem("ASDASD/asd")]
	 static void SASD() {
	     Debug.Log( Initializator.AdaptersByID[0].GetHierarchyObjectByInstanceID( Selection.activeGameObject.GetInstanceID() ).fileID );
	 }*/
	
	internal HierarchyObject GetHierarchyObjectByInstanceID( GameObject o )
	{	return GetHierarchyObjectByInstanceID( o ? o.GetInstanceID() : -1, o );
	}
	string finded = null;
	string f_path = null;
	internal HierarchyObject GetHierarchyObjectByInstanceID( int instanceid, GameObject ___o = null)
	{	if ( !m_instanceIdToHierObject.TryGetValue( instanceid, out gettedObject ) || IS_HIERARCHY() && !gettedObject.go )
		{	m_instanceIdToHierObject.Remove( instanceid );
		
			if ( IS_HIERARCHY() )
			{	gettedObject = new HierarchyObject( pluginID )
				{	go = ___o ?? ( EditorUtility.InstanceIDToObject( instanceid ) as GameObject),
				};
				
				gettedObject.id = instanceid;
				
				if ( !gettedObject.go ) gettedObject.fileID = instanceid;
				else gettedObject.fileID = gettedObject.fileID;
				
				/* if (Hierarchy_GUI.HierarchySettings.PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances)
				 {   if (gettedObject.go)
				     {   gettedObject.  fileID = Adapter.GET_INSTANCE_ID(gettedObject.go);
				     }
				
				 }*/
				
			}
			
			else
			{	if ( !m_instanceIdToPATH.ContainsKey( instanceid ) )
				{	finded = AssetDatabase.GetAssetPath( instanceid );
					/*try { finded = AssetDatabase.GetAssetPath( AssetDatabase.Path. EditorUtility.InstanceIDToObject( instanceid ) ); }
					catch { return null; }*/
					m_instanceIdToPATH.Add( instanceid, finded );
				}
				
				if ( !m_instanceIdToGUID.ContainsKey( instanceid ) ) m_instanceIdToGUID.Add( instanceid, f_path = AssetDatabase.AssetPathToGUID( m_instanceIdToPATH[instanceid] ) );
				
				gettedObject = GetHierarchyObjectByGUID( ref f_path, ref finded );
				
				
				if ( !gettedObject.project.IsFolder && gettedObject.id != instanceid )
				{	if ( gettedObject.project.nonMainAssets == null ) gettedObject.project.nonMainAssets = new Dictionary<int, HierarchyObject>();
				
					if ( !gettedObject.project.nonMainAssets.ContainsKey( instanceid ) )
					{	var target = EditorUtility.InstanceIDToObject( instanceid );
					
						if ( target )
						{	var clone = (HierarchyObject)gettedObject.Clone();
							clone.project.IsMainAsset = false;
							clone.project.nonMainAssets = null;
							clone.project.assetName = target.name;
							gettedObject.project.nonMainAssets.Add( instanceid, clone );
						}
						
					}
					
					if ( gettedObject.project.nonMainAssets.ContainsKey( instanceid ) )
						gettedObject = gettedObject.project.nonMainAssets[instanceid];
				}
			}
			
			
			m_instanceIdToHierObject.Add( instanceid, gettedObject );
		}
		
		//if ( !m_instanceIdToHierObject.ContainsKey( id ) ) return null;
		return m_instanceIdToHierObject[instanceid];
	}
	
	SortedList<int, HierarchyObject> m_fakeList = new SortedList<int, HierarchyObject>();
	Dictionary<string, SortedList<int, HierarchyObject>> m_PathToChildrens = new Dictionary<string, SortedList<int, HierarchyObject>>();
	Dictionary<string, HierarchyObject> m_PathToObject = new Dictionary<string, HierarchyObject>();
	
	
	internal void GetPathToChildrens( ref string key, out SortedList<int, HierarchyObject> result )
	{	if ( !m_PathToChildrens.ContainsKey( key ) )
		{	result = m_fakeList;
			return;
		}
		
		result = m_PathToChildrens[key];
	}
	
	
	bool INVOKE_ON_GUID_BACKCHANGED = false;
	internal void ON_GUID_BACKCHANGED()
	{	if ( INVOKE_ON_GUID_BACKCHANGED ) return;
	
		INVOKE_ON_GUID_BACKCHANGED = true;
		GUI_ONESHOTPUSH( () =>
		{	SET_DIRTY_PROJECT();
			bottomInterface.RefreshMemCache( -1 );
			INVOKE_ON_GUID_BACKCHANGED = false;
		} );
		/* bottomInterface.m_memCache
		 ClearHierarchyObjects();*/
	}
	
	
	string temp_guid;
	MethodInfo SET_GUID_METHOD;
	object[] params_SET_GUID_METHOD = new object[1];
	internal HierarchyObject GetHierarchyObjectByPair<T>( ref List<T> h, int i )
	{	var __o =  (GoGuidPair)(IGoGuidPair) h[i];
	
		if ( IS_HIERARCHY() )
		{	var res = GetHierarchyObjectByInstanceID( __o.go );
			res.fileID = __o.fileID;
			return res;
		}
		
#pragma warning disable
		marker = false;
		temp_guid = __o.guid;
		var result = GetHierarchyObjectByGUID( ref temp_guid,  __o.path );
		
		if ( marker )
		{	if ( SET_GUID_METHOD == null ) SET_GUID_METHOD = __o.GetType().GetMethod( "SET_GUID_NOREF", (BindingFlags)(-1) );
		
			params_SET_GUID_METHOD[0] = temp_guid;
			SET_GUID_METHOD.Invoke( h[i], params_SET_GUID_METHOD );
			ON_GUID_BACKCHANGED();
		}
		
		return result;
#pragma warning restore
	}
	internal HierarchyObject GetHierarchyObjectByPair( ref List<GoGuidPair> pair, int index )
	{	if ( IS_HIERARCHY() )
		{	var res = GetHierarchyObjectByInstanceID( pair[index].go );
			res.fileID = pair[index].fileID;
			return res;
		}
		
#pragma warning disable
		marker = false;
		temp_guid = pair[index].guid;
		var result = GetHierarchyObjectByGUID( ref temp_guid,  pair[index].path );
		
		if ( marker )
		{	pair[index].SET_GUID( ref temp_guid );
			ON_GUID_BACKCHANGED();
		}
		
		return result;
#pragma warning restore
	}
	
	internal HierarchyObject GetHierarchyObjectByPair( ref GoGuidPair pair )
	{	if ( IS_HIERARCHY() )
		{	var res = GetHierarchyObjectByInstanceID( pair.go );
			res.fileID = pair.fileID;
			return res;
		}
		
#pragma warning disable
		return GetHierarchyObjectByGUID( ref pair.guid, ref pair.path );
#pragma warning restore
	}
	
	
	
	
	Dictionary<string, HierarchyObject> m_GuidToHierObject = new Dictionary<string, HierarchyObject>();
	
	
	//static ulong guidID;
	internal HierarchyObject GetHierarchyObjectByGUID( ref string guid, string estimpath )
	{	return GetHierarchyObjectByGUID( ref guid, ref estimpath );
	}
	bool marker;
	internal HierarchyObject GetHierarchyObjectByGUID( ref string guid, ref string estimpath )
	{
	
		if ( !m_GuidToHierObject.TryGetValue( guid, out gettedObject ) )      // if (GetInstanceIDFromGUID == null) return null;
		{
		
		
			//args[0] = guid;
			
			var getted_id = GET_ID_BY_GUID ( ref guid );
			
			if ( getted_id == 0 )
			{	var estimguid = AssetDatabase.AssetPathToGUID(estimpath);
			
				if ( !string.IsNullOrEmpty( estimguid ) )
				{	getted_id = GET_ID_BY_GUID( ref estimguid );
				
					if ( getted_id != 0 )
					{	guid = estimguid;
						ON_GUID_BACKCHANGED();
						marker = true;
					}
				}
			}
			
			
			
			estimpath = getted_id == 0 ? "" : AssetDatabase.GUIDToAssetPath( guid );
			
			//if (string.IsNullOrEmpty( estimpath )) gettedObject = null;
			// else
			{	gettedObject = new HierarchyObject( pluginID )
				{	project = new HierarchyObject_ProjectExt()
					{	assetPath = estimpath == null ? "" : estimpath,
						    guid = guid
					},
					id = getted_id,  //InternalEditorUtility.GetObjectFromInstanceID
					// fileID = getted_id //InternalEditorUtility.GetObjectFromInstanceID
				};
			}
			
			
			
			//Debug.Log( gettedObject.project.assetPath );
			
			//var _id =
			//gettedObject.go = EditorUtility.InstanceIDToObject( gettedObject.id ) as GameObject;
			
			
			
			if ( !string.IsNullOrEmpty( gettedObject.project.assetPath ) )
			{	gettedObject.project.IsFolder = Directory.Exists( UNITY_SYSTEM_PATH + gettedObject.project.assetPath );
			
				var ind = gettedObject.project.assetPath.LastIndexOf( '/' );
				gettedObject.project.assetName = gettedObject.project.assetPath.Substring( ind + 1 );
				//  if (gettedObject.project.IsFolder) gettedObject.project.assetName = "(Folder)" + gettedObject.project.assetName;
				gettedObject.project.assetFolder = ind > -1 ? gettedObject.project.assetPath.Remove( ind ) : "";
				
				if ( !m_PathToChildrens.ContainsKey( gettedObject.project.assetFolder ) ) m_PathToChildrens.Add( gettedObject.project.assetFolder, new SortedList<int, HierarchyObject>() );
				
				gettedObject.project.sibling = m_PathToChildrens[gettedObject.project.assetFolder].Count;
				m_PathToChildrens[gettedObject.project.assetFolder].Add( gettedObject.project.sibling, gettedObject );
				
				if ( !gettedObject.project.IsFolder )
				{	var extInd = gettedObject.project.assetName.LastIndexOf( '.' );
				
					if ( extInd != -1 && extInd < gettedObject.project.assetName.Length - 1 ) { gettedObject.project.fileExtension = '.' + gettedObject.project.assetName.Substring( extInd + 1 ).ToLower(); }
				}
				
				if ( !gettedObject.project.IsFolder )
				{	var dot =  gettedObject.project.assetName.LastIndexOf( '.' );
					var slash =  gettedObject.project.assetName.LastIndexOf( '/' );
					
					if ( dot > 0 && slash < dot ) gettedObject.project.assetName = gettedObject.project.assetName.Remove( dot );
				}
				
				
				//gettedObject.project.obj = AssetDatabase.LoadMainAssetAtPath( gettedObject.project.assetName );
				
				
			}
			
			else
			{	gettedObject.invalide = true;
			}
			
			if ( !m_GuidToHierObject.ContainsKey( guid ) )
				m_GuidToHierObject.Add( guid, gettedObject );
			else m_GuidToHierObject[guid] = gettedObject;
			
			// if (gettedObject != null)
			{	if ( !m_PathToObject.ContainsKey( gettedObject.project.assetPath ) ) m_PathToObject.Add( gettedObject.project.assetPath, gettedObject );
				else m_PathToObject[gettedObject.project.assetPath] = gettedObject;
			}
			
			
			
		}
		
		return gettedObject;
		//	if ( !m_GuidToHierObject.ContainsKey( guid ) ) return null;
		//return m_GuidToHierObject[guid];
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	/*
	#pragma warning disable
	void OldInit()
	{   var allfiles = Directory.GetFiles( PluginExternalFolder, "ASD",//STR_DATA( this ),
	                                    SearchOption.AllDirectories );
	 //MonoBehaviour.print(allfiles.Length);
	
	 if ( allfiles.Length == 0 ) return;
	 var done = false;
	 byte[][] pars;
	 foreach ( var a in allfiles )
	 {   try
	     {   var result = File.ReadAllText( a );
	         if ( string.IsNullOrEmpty( result ) ) continue;
	
	         #if HIERARCHY
	         if ( pluginID == Initializator.HIERARCHY_ID ) pars = (DESERIALIZE_SINGLE<Package>( result )).icons;
	         #endif
	
	         #if PROJECT
	
	         if ( pluginname == Initializator.PROJECT_NAME ) pars = (DESERIALIZE_SINGLE<EModules.EProjectInternal.Package>( result )).icons;
	         #endif
	         // pars = (Package)DESERIALIZE_SINGLE( result );
	         done = true;
	     }
	     catch
	     {   //   MonoBehaviour.print(e.Message + "\n" + e.StackTrace);
	         // ignored
	     }
	     // MonoBehaviour.print(a);
	     /* var file = a.Substring(a.IndexOf("Assets", StringComparison.Ordinal)).Replace('\\', '/');
	      // MonoBehaviour.print(file);
	      try
	      {
	          var result = AssetDatabase.LoadAssetAtPath<TextAsset>(file);
	          //  MonoBehaviour.print((result == null) + " " + string.IsNullOrEmpty(result.text));
	          if (result == null) continue;
	          /*   if (!AssetDatabase.Contains(result))
	                 AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(a), ImportAssetOptions.ForceUpdate);#1#
	          // ClassFileFinder
	          //  MonoBehaviour.print((result == null) + " " + string.IsNullOrEmpty(result.text));
	          pars = (Package)DESERIALIZE_SINGLE(result.text);
	          //  MonoBehaviour.print((pars.icons.Length));
	          done = true;
	      }
	      catch/ * (Exception e)#1#
	      {
	          //   MonoBehaviour.print(e.Message + "\n" + e.StackTrace);
	          // ignored
	      }* /
	 }
	
	}
	private bool IconLoadedLog = false;
	#pragma warning restore
	*/
}
}
