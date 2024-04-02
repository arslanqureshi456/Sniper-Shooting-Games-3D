
#if UNITY_EDITOR
	#define HIERARCHY
	#define PROJECT
#endif


using System;
using System.Collections;
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




	internal sealed partial class BottomInterface {
	
	
	
	
	
		internal List<Adapter.BottomInterface.BottomController> WindowController = new List<Adapter.BottomInterface.BottomController>();
		internal List<Adapter.BottomInterface.BottomController> FavoritControllers = new List<Adapter.BottomInterface.BottomController>();
		
		
		
		internal BottomInterface( Adapter adapter, bool USE_HOTKEYS )
		{
		
		
		
			this.adapter = adapter;
			this.hyperGraph = new HyperGraph( adapter, this );
			this.favorGraph = new FavorGraph( adapter, this );
			ContentSelBackLabel = new GUIContent() { tooltip = "Selection Backward" + (USE_HOTKEYS ? " Ctrl+Shift+Z" : "") };
			ContentSelForwLabel = new GUIContent() { tooltip = "Selection Forward" + (USE_HOTKEYS ? " Ctrl+Shift+Y" : "") };
		}
		
		void PLAY_MODE_CHANGE()
		{	if ( onSceneChange != null ) onSceneChange();
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		internal int[] DRAW_INDEX = new int[4];
		
		internal void SORT_DRAW_ROWS()
		{	if ( DRAW_INDEX.Length != RowsParams.Length ) DRAW_INDEX = new int[RowsParams.Length];
		
			for ( int i = 0, l = RowsParams.Length ; i < l ; i++ ) DRAW_INDEX[i] = -1;
			
			for ( int i = 0, l = RowsParams.Length ; i < l ; i++ )
			{	var c = RowsParams[i].IndexPos;
			
				if ( c < 0 || c > l - 1 ) continue;
				
				DRAW_INDEX[c] = i;
			}
			
			for ( int i = 0, l = RowsParams.Length ; i < l ; i++ ) if ( DRAW_INDEX[i] == -1 )     // Debug.LogWarning("ASD");
				{	for ( int J = 0 ; J < l ; J++ )
					{	if ( DRAW_INDEX.Contains( J ) )
						{	continue;
						}
						
						DRAW_INDEX[i] = J;
					}
				}
		}
		
		
		
		
		
		
		internal class PLUGIN_ID {
			internal const int BOOKMARKS = 0;
			internal const int LAST = 1;
			internal const int HIER = 2;
			internal const int SCENES = 3;
		}
		
		
		ISET_ROW[] _mRowsParams = null;
		internal ISET_ROW[] RowsParams
		{	get
			{	if ( _mRowsParams == null ) _mRowsParams = new ISET_ROW[4]
				{	new BOOKMARKS(adapter, 0),
					new LAST(adapter, 1),
					new HIERARCHY(adapter, 2),
					new SCENES(adapter, 3)
				};
				
				return _mRowsParams;
			}
		}
		
		
		internal abstract class ISET_ROW {
			protected Adapter a;
			internal int PluginID;
			
			internal int MaxItems { get { return a.par.BottomParams.BottomRows[PluginID].MaxItems; } set { a.par.BottomParams.BottomRows[PluginID].MaxItems = value; } }
			internal int Rows { get { return a.par.BottomParams.BottomRows[PluginID].Rows; } set { a.par.BottomParams.BottomRows[PluginID].Rows = value; } }
			internal bool HiglighterValue { get { return a.par.BottomParams.BottomRows[PluginID].HiglighterValue; } set { a.par.BottomParams.BottomRows[PluginID].HiglighterValue = value; } }
			internal int IndexPos { get { return a.par.BottomParams.BottomRows[PluginID].IndexPos; } set { a.par.BottomParams.BottomRows[PluginID].IndexPos = value; } }
			
			abstract internal float FULL_HEIGHT { get; }
			abstract internal int RowHeight { get; }
			abstract internal bool Enable { get; set; }
			abstract internal bool AllowHiglighter { get; }
			abstract internal bool AllowBgColor { get; }
			abstract internal Color BgColorValue { get; set; }
			abstract internal string Name { get; }
			
		}
		
		internal class BOOKMARKS : ISET_ROW {
			internal BOOKMARKS( Adapter a, int paramIndex ) { this.a = a; this.PluginID = paramIndex; }
			
			override internal bool Enable { get { return a.par.SHOW_BOOKMARKS_ROWS; } set { a.par.SHOW_BOOKMARKS_ROWS = value; } }
			override internal bool AllowHiglighter { get { return true; } }
			override internal bool AllowBgColor { get { return true; } }
			override internal string Name { get { return "Bookmarks"; } }
			override internal float FULL_HEIGHT { get { return !Enable ? 0 : a.par.BottomParams.BottomRows[PluginID].Rows * RowHeight; } }
			override internal int RowHeight { get { return Mathf.RoundToInt( a.bottomInterface.LINE_HEIGHT( true, true ) ); } }
			override internal Color BgColorValue
			{	get
					//  {   if ( a.par.BottomParams.BottomRows[PluginID].BgColorValue == null ) a.par.BottomParams.BottomRows[PluginID].BgColorValue = new SerializeColor( 47 / 255f, 75 / 255f, 114 / 255f, 47 / 255f );
				{	//if ( a.par.BottomParams.BottomRows[PluginID].BgColorValue == null ) a.par.BottomParams.BottomRows[PluginID].BgColorValue = new SerializeColor( 0 / 255f, 115 / 255f, 158 / 255f, 164 / 255f );
					if ( a.par.BottomParams.BottomRows[PluginID].BgColorValue == null ) a.par.BottomParams.BottomRows[PluginID].BgColorValue = new SerializeColor( 63f / 255f, 132f / 255f, 158f / 255f, 164 / 255f );
					
					return a.par.BottomParams.BottomRows[PluginID].BgColorValue.Value;
				}
				
				set { a.par.BottomParams.BottomRows[PluginID].BgColorValue = value; }
			}
		}
		
		internal class LAST : ISET_ROW {
			internal LAST( Adapter a, int paramIndex ) { this.a = a; this.PluginID = paramIndex; }
			
			override internal bool Enable { get { return a.par.SHOW_LAST_ROWS; } set { a.par.SHOW_LAST_ROWS = value; } }
			override internal bool AllowHiglighter { get { return true; } }
			override internal bool AllowBgColor { get { return true; } }
			override internal string Name { get { return "Last Selections"; } }
			override internal float FULL_HEIGHT { get { return !Enable ? 0 : a.par.BottomParams.BottomRows[PluginID].Rows * RowHeight; } }
			override internal int RowHeight { get { return a.bottomInterface.LINE_HEIGHT( true ); } }
			override internal Color BgColorValue
			{	get
				{	if ( a.par.BottomParams.BottomRows[PluginID].BgColorValue == null ) a.par.BottomParams.BottomRows[PluginID].BgColorValue = new SerializeColor( .15f, .15f, .15f, .15f );
				
					return a.par.BottomParams.BottomRows[PluginID].BgColorValue.Value;
				}
				
				set { a.par.BottomParams.BottomRows[PluginID].BgColorValue = value; }
			}
		}
		
		internal class HIERARCHY : ISET_ROW {
			internal HIERARCHY( Adapter a, int paramIndex ) { this.a = a; this.PluginID = paramIndex; }
			
			override internal bool Enable { get { return a.par.SHOW_HIERARCHYSLOTS_ROWS; } set { a.par.SHOW_HIERARCHYSLOTS_ROWS = value; } }
			override internal bool AllowHiglighter { get { return false; } }
			override internal bool AllowBgColor { get { return false; } }
			override internal string Name { get { return "Expand States"; } }
			override internal float FULL_HEIGHT { get { return !Enable ? 0 : a.par.BottomParams.BottomRows[PluginID].Rows * RowHeight; } }
			override internal int RowHeight { get { return a.bottomInterface.LINE_HEIGHT( true ); } }
			override internal Color BgColorValue
			{	get
				{	if ( a.par.BottomParams.BottomRows[PluginID].BgColorValue == null ) a.par.BottomParams.BottomRows[PluginID].BgColorValue = new SerializeColor();
				
					return a.par.BottomParams.BottomRows[PluginID].BgColorValue.Value;
				}
				
				set { a.par.BottomParams.BottomRows[PluginID].BgColorValue = value; }
			}
		}
		
		class SCENES : ISET_ROW {
			internal SCENES( Adapter a, int paramIndex ) { this.a = a; this.PluginID = paramIndex; }
			
			override internal bool Enable { get { return a.par.SHOW_SCENES_ROWS; } set { a.par.SHOW_SCENES_ROWS = value; } }
			override internal bool AllowHiglighter { get { return false; } }
			override internal bool AllowBgColor { get { return false; } }
			override internal string Name { get { return "Scene Buttons"; } }
			override internal float FULL_HEIGHT { get { return !Enable ? 0 : a.par.BottomParams.BottomRows[PluginID].Rows * RowHeight; } }
			override internal int RowHeight { get { return Mathf.RoundToInt( a.bottomInterface.LINE_HEIGHT( true ) * 1.35f ); } }
			override internal Color BgColorValue
			{	get
				{	if ( a.par.BottomParams.BottomRows[PluginID].BgColorValue == null ) a.par.BottomParams.BottomRows[PluginID].BgColorValue = new SerializeColor();
				
					return a.par.BottomParams.BottomRows[PluginID].BgColorValue.Value;
				}
				
				set { a.par.BottomParams.BottomRows[PluginID].BgColorValue = value; }
			}
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		internal FavorGraph favorGraph;
		internal HyperGraph hyperGraph;
		Adapter adapter;
		internal Action onSelectionChange;
		internal Action onSceneChange;
		
		
		internal void INIT_ON_LOAD()     // Application.isPlaying = false;
		{	cacheInit = false;
			hyperGraph.HierHyperController.wasInit = false;
			hyperGraph.WindowHyperController.wasInit = false;
			onSelectionChange = null;
			//  if (onSceneChange != null) onSceneChange();
			onSceneChange = null;
			
			HierarchyController.selection_action = null;
			
			foreach ( var windowController in WindowController )
				windowController.selection_action = null;
				
			foreach ( var windowController in FavoritControllers )
				windowController.selection_action = null;
				
			/* EditorApplication.playmodeStateChanged -= PLAY_MODE_CHANGE;
			 EditorApplication.playmodeStateChanged += PLAY_MODE_CHANGE;*/
		}
		
		//  float HEIGHT = 56;
		private const int BOTTOMPADDINGOFFSET = 0;
		private const int HYPER_OFFSET = 900000;
		private const int SPACE = 2;
		
		private bool SelectChange = false;
		private Int32List newIds;
#pragma warning disable
		private UnityEngine.Object[] lastSel = new UnityEngine.Object[0];
		
		//private float? WINH;
		//  int[] prevIds = new int[0];
		private int lastSceneID = -1;
		private string lastScenePath = null;
		private string[] lastSceneGUID_ALL = new string[0];
		private string lastSceneGUID = null;
		private bool SkipRemove = false;
		private bool SkipRemoveFix = false;
		public int LastIndex = -1;
		internal bool NeedRefreshBOttom = false;
#pragma warning restore
		
		private GameObject[] ignoreLock = new GameObject[0];
		private float lastHeight = -1;
		internal float? defaultextraInsertionMarkerIndent;
		private Rect oldr;
		
		
		internal void GET_BOOKMARKS( ref List<Int32ListArray> list, int scene )      //  var scene = EditorSceneManager.GetActiveScene();
		{	if ( adapter.IS_HIERARCHY() && (!Adapter.GET_SCENE_BY_ID( scene ).isLoaded || !Adapter.GET_SCENE_BY_ID( scene ).IsValid()) || adapter.MOI.des( scene ) == null )
			{	list = new List<Int32ListArray>();
				return;
			}
			
			list = adapter.MOI.des( scene ).GetBookMarks();
			
			//  refBookmarks = controller.GetCategoryIndex( scene ) == 0 ? adapter.MOI.des( scene ).GetHash4() : adapter.MOI.des( scene ).GetBookMarks()[controller.GetCategoryIndex( scene )].array;
			//list = controller.GetCategoryIndex( scene ) == 0 ? adapter.MOI.des( scene ).GetHash4() : adapter.MOI.des( scene ).GetBookMarks()[controller.GetCategoryIndex( scene )].array;
			if ( list.Count == 0 )     // var b = adapter.MOI.des(scene).GetBookMarks();
			{	var result = new Int32ListArray()
				{	name = "Default",
					    array = new List<Int32List>()
				};
				result.SET_COLOR( adapter.bottomInterface.RowsParams[0].BgColorValue );
				list.Add( result );
				//adapter.MOI.des(scene).SetBookMarks(b);
			}
			
			if ( adapter.MOI.des( scene ).GetHash4() == null ) adapter.MOI.des( scene ).SetHash4( new List<Int32List>() );
			
			list[0].array = adapter.MOI.des( scene ).GetHash4();
			
			for ( int i = 0 ; i < list.Count ; i++ )
				CheckUniqueFavoritID( ref list[i].InstanceID );
		}
		
		Dictionary<int, int> _mFavIdCache =  new Dictionary<int, int>();
		internal void CheckUniqueFavoritID( ref int id )
		{	if ( id != 0 )
			{	if ( !_mFavIdCache.ContainsKey( id ) ) _mFavIdCache.Add( id, -1 );
			
				return;
			}
			
			int result = 0;
			
			do result = UnityEngine.Random.Range( int.MinValue, int.MaxValue );
			
			while ( _mFavIdCache.ContainsKey( result ) );
			
			_mFavIdCache.Add( result, -1 );
			id = result;
			//return result;
		}
		
		
		
		/*  internal  EditorWindow selection_window;
		  internal  int? selection_button;
		  internal  bool selection_info;*/
		
		// GUIContent emptyContent = new GUIContent() { tooltip = "" };
		internal GUIContent FoldContent = new GUIContent() { tooltip = "Minimize The Dock" };
		// internal GUIContent plusContentLabel = new GUIContent() { tooltip = "Add GameObject" };
		internal GUIContent plusContentLabel = new GUIContent() { tooltip = "Create a snapshot of objects that have been expanded" };
		internal GUIContent plusContentSceneLabel = new GUIContent() { tooltip = "Create a multi-scenes buttons from open scenes" };
		
		internal GUIContent plusContent = new GUIContent() { text = "+" };
		internal GUIContent hierCollapce = new GUIContent() { text = "►" }; //-
		internal GUIContent hierCollapceLabel = new GUIContent() { tooltip = "Collapse all expanded items" };
		
		internal GUIContent ContentSelBackLabel = null;
		private GUIContent ContentSelForwLabel = null;
		
		internal GUIContent ContentSelBack = new GUIContent() { text = "◄" };//
		internal GUIContent ContentSelForw = new GUIContent() { text = "►" };
		
		internal GUIContent ___GETTOOLTIPPEDCONTENT = new GUIContent();
		// internal int LINE_REFERENCE_HEIGHT {get {return (int)EditorGUIUtility.singleLineHeight;}}
		internal int LINE_REFERENCE_HEIGHT = (int)EditorGUIUtility.singleLineHeight;
		
		
		float? ___heightcache;
		internal float? _HEIGHT
		{	get
			{	if ( !___heightcache.HasValue ) ___heightcache = EditorPrefs.GetFloat( adapter.pluginname + "/" + "cached_height", -1 );
			
				return ___heightcache.Value == -1 ? (float ? )null : ___heightcache.Value;
			}
			
			set
			{	if ( value == null ) value = -1;
			
				if ( ___heightcache != value ) EditorPrefs.SetFloat( adapter.pluginname + "/" + "cached_height", value.Value );
				
				___heightcache = value;
			}
		}
		
		
		internal List<int> Keys = new List<int>();
		internal Dictionary<int, bool> NEED_READ_LIST = new Dictionary<int, bool>();
		
		// internal  bool PLAYMODECHANGE = false;
		internal Rect hiperRect;
		internal int DRAW_FOLD_ICONS_CONTROLID;
		internal GUIContent iconsContent = new GUIContent();
		internal Rect? lastRect;
		//  private  Color ca5 = new Color(1, 1, 1, 0.5f);
		
		
		/*     private  GUIContent tooltipLastHelp = new GUIContent() { tooltip = "You can switch between recently selected GameObjects" };
		     private  GUIContent tooltipCustomHelp = new GUIContent() { tooltip = "You can store these GameObjects in this line" };
		     private  GUIContent tooltipScenesHelp = new GUIContent() { tooltip = "You can switch between recently selected Scenes" };*/
		
		/* GUIContent tooltipLast = new GUIContent() { tooltip = "Left-Clilck to Select or Drag to Inspector\nRight-Click to Remove" };
		 GUIContent tooltipCustom = new GUIContent() { tooltip = "Left-Clilck to Select or Drag to Inspector\nRight-Click to Remove" };
		 GUIContent tooltipScenes = new GUIContent() { tooltip = "Left-Clilck to Select or Drag to Inspector\nRight-Click to Remove" };*/
		
		private GUIContent content = new GUIContent();
		
		
		//   Hierarchy_GUI.SetDirtyObject();
		//   Utilites.ObjectContent(//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?//?
		
		private GUIStyle refStyle;
		private Color refColor;
#pragma warning disable
		private Color tst = new Color32(240, 240, 255, 255);
		private Color ts2t = new Color32(248, 240, 225, 255);
#pragma warning restore
		
		private bool cacheInit = false;
		private Dictionary<MemType, Rect[][]> m_memPosition = new Dictionary<MemType, Rect[][]>();
		
		internal Dictionary<MemType, List<MemoryRoot>[]> m_memCache =
		    new Dictionary<MemType, List<MemoryRoot>[]>();
		    
		private MemType[] MemTypes;
		private IHashProperty lastDes = null;
		
		/*     float REFERENCE_HEIGHT {
		        get { return ((par.BOTTOM_AUTOHIDE ? ___REFERENCE_HEIGHT_AUTOHIDE() : ___REFERENCE_HEIGHT_FULL()) - LINE_REFERENCE_HEIGHT) * HALFFACTOR_8() + LINE_REFERENCE_HEIGHT; }
		    }*/
		
		internal float REFERENCE_HEIGHT
		{	get
			{	var result = (adapter.par.BOTTOM_AUTOHIDE ? ___REFERENCE_HEIGHT_AUTOHIDE() : ___REFERENCE_HEIGHT_FULL());
				// if (adapter.par.USE_HORISONTAL_SCROLL) result += EditorGUIUtility.singleLineHeight;
				return result;
			}
		}
		
		internal int HEIGHT
		{	get
			{	/*   MonoBehaviour.print(m_SearchFilter);
				       MonoBehaviour.print(window());
				       MonoBehaviour.print(m_SearchFilter.GetValue(window()));*/
				//MonoBehaviour.print((string)m_SearchFilter.GetValue(window()));
				if ( adapter.DISABLE_DES() ) return 0;
				
				// if ( adapter.m_SearchFilterString != null && !string.IsNullOrEmpty( (string)adapter.m_SearchFilterString.GetValue( adapter.window() ) ) ) return 0;
				if ( adapter.IS_SEARCH_MODE_OR_PREFAB_OPENED() ) return 0;
				
				if ( !_HEIGHT.HasValue ) _HEIGHT = adapter.ENABLE_BOTTOMDOCK_PROPERTY ? REFERENCE_HEIGHT : 0;
				
				return Mathf.FloorToInt( !adapter.ENABLE_BOTTOMDOCK_PROPERTY ? 0 : (_HEIGHT ?? 0 /*+ HIPER_HEIGHT()*/) );
				//return !adapter.par.ENABLE_ALL ? 0 : (_HEIGHT.Value /*+ HIPER_HEIGHT()*/);
			}
			
			set { _HEIGHT = value; }
		}
		
		
		
		public void Init()     //RefreshMemCache();
		{	//  MonoBehaviour.print("!@#");
			Selection.selectionChanged -= SelectionChanged;
			Selection.selectionChanged += SelectionChanged;
			
			adapter.SubcripeSceneLoader( EditorSceneManagerOnSceneOpening );
			
			/*EditorApplication.playmodeStateChanged -= PLAY_MODE_CHANGE_ACTION;
			EditorApplication.playmodeStateChanged += PLAY_MODE_CHANGE_ACTION;*/
			
			/*  EditorSceneManager.GetActiveScene(). += (s1, s2) => {
			      MonoBehaviour.print("ASD");
			  };*/
		}
		
		private void EditorSceneManagerOnSceneOpening()     // MonoBehaviour.print("ASD");
		{	//  adapter.ClearHierarchyObjects();
			Hierarchy.HierarchyAdapterInstance.need_ClearHierarchyObjects1 = true;
			
			lastScenePath = null;
			lastSceneID = -1;
			lastSceneGUID = null;
		}
		
		
		private void Scene_WriteLastScene( int _s )
		{	Scene s;
		
			if ( _s == -1 ) s = SceneManager.GetActiveScene();
			else s = Adapter.GET_SCENE_BY_ID( _s );
			
			lastSceneID = _s;
			lastScenePath = s.path;
			
			if ( !string.IsNullOrEmpty( s.path ) )
			{	lastSceneGUID = AssetDatabase.AssetPathToGUID( s.path );
				Array.Resize( ref lastSceneGUID_ALL, SceneManager.sceneCount );
				
				for ( int i = 0 ; i < lastSceneGUID_ALL.Length ; i++ )
					lastSceneGUID_ALL[i] = AssetDatabase.AssetPathToGUID( SceneManager.GetSceneAt( i ).path );
			}
			
			else lastSceneGUID = null;
		}
		
		
		private void Scene_SetDityMemory( int _scene, Scene[] additiona_scenes, bool skipPlay = false )       // print("123");
		{	if ( Application.isPlaying && !skipPlay ) return;
		
		
			if ( _scene == -1 ) _scene = SceneManager.GetActiveScene().GetHashCode();
			
			var GUID = AssetDatabase.AssetPathToGUID(Adapter.GET_SCENE_BY_ID(_scene).path);
			
			if ( !string.IsNullOrEmpty( GUID ) )
			{	var newScene = new SceneId( GUID, (additiona_scenes ?? new Scene[0]).Select(s => AssetDatabase.AssetPathToGUID(s.path)).Where(guid => !string.IsNullOrEmpty(guid)).ToArray());
			
				var lastScenes = Hierarchy_GUI.GetLastScenes(adapter);
				var haveScene = lastScenes.Take(adapter.par.BottomParams.BottomRows[1].MaxItems).FirstOrDefault(s => s == newScene);
				
				if ( haveScene != null )
				{	if ( haveScene.pin ) return;
				}
				
				var pinned = lastScenes.Select((s, i) => new { scene = s, index = i })
				.Where(r => r.scene.pin)
				.OrderBy(r => r.index)
				.ToArray();
				lastScenes.RemoveAll( s => s.pin );
				
				lastScenes.RemoveAll( s => s == newScene );
				
				
				
				if ( lastScenes.Count == 0 ) lastScenes.Add( newScene );
				else lastScenes.Insert( 0, newScene );
				
				for ( int i = 0 ; i < pinned.Length ; i++ )
				{	if ( lastScenes.Count == pinned[i].index ) lastScenes.Add( pinned[i].scene );
					else lastScenes.Insert( pinned[i].index, pinned[i].scene );
				}
				
				while ( lastScenes.Count > adapter.MAX20 ) lastScenes.RemoveAt( adapter.MAX20 );
				
				Hierarchy_GUI.SetDirtyObject( adapter );
				/* MonoBehaviour.print("SetUp");*/
			}
		}
		
		private void Scene_RefreshGUIAndClearActions( int scene )
		{	RefreshMemCache( scene );
			ClearAction();
		}
		
		/* List<Object> parents = new List<Object>();*/
		//            private bool FixIMGUISelectionChangeMarker = false;
		private void SelectionChanged()
		{	if ( !Hierarchy_GUI.Instance( adapter ) ) return;
		
#if HIERARCHY
			/* if (adapter.pluginID == Initializator.HIERARCHY_ID)
			 {   if (adapter.modules.Length != 0 && adapter.modules[3].enable && adapter.par.LOCK_SELECTION)
			     {   foreach (var gameObject in Selection.gameObjects)
			         {   if (!gameObject.scene.IsValid()) continue;
			             if ((gameObject.hideFlags & HideFlags.NotEditable) != 0)
			             {   //Selection.objects = new GameObject[0];
			                 //MonoBehaviour.print("ASD");
			                 return;
			             }
			         }
			     }
			 }*/
#endif
			//   FixIMGUISelectionChangeMarker = true;
			/* parents.Clear();
			 if (Selection.activeTransform)
			 {
			     var p = Selection.activeTransform.parent;
			     while (p)
			     {
			
			     }
			 }*/
			
			if ( onSelectionChange != null ) onSelectionChange();
			
			/*if (Application.isPlaying)  /////// *****  DELETED TO ENABLE CHANGING FOR PLAYING MODE
			{
			    SkipRemoveFix = SkipRemove = false;
			    return;
			}*/   /////// *****  DELETED TO ENABLE CHANGING FOR PLAYING MODE
			
			//	adapter.SELECTED_GAMEOBJECTS
			var temp =
			    adapter.IS_HIERARCHY() ? Selection.objects.Where(g => g is GameObject && ((GameObject)g).scene.IsValid()).ToArray() :
			    Selection.objects.Where(o => o && Adapter.isProjectObjectBool(o)).ToArray();
			    
			    
			    
			if ( temp.Length > 500 )
			{	SkipRemoveFix = SkipRemove = false;
				return;
			}
			
			SelectChange = temp.Length != 0;
			
			if ( adapter.IS_HIERARCHY() )
			{	foreach ( var o in temp )
				{	var gameObject = (GameObject)o;
				
					if ( !gameObject.scene.isLoaded || (gameObject.hideFlags & HideFlags.NotEditable) != 0 ||
					        !gameObject.transform )
					{	SelectChange = false;
						break;
					}
				}
			}
			
			
			
			
			
			if ( adapter.pluginID == Initializator.HIERARCHY_ID )
			{	if ( SelectChange && Selection.activeGameObject && Selection.activeGameObject.scene.IsValid() )
				{	newIds = new Int32List { };
					SET_INT32_AS_HIERARCHY( newIds, temp.Select( t => t.GetInstanceID() ).ToArray(), Selection.activeGameObject.GetInstanceID() );
					lastSel = temp;
					
				}
				
				else
				{	LastIndex = -1;
				}
			}
			
			if ( adapter.pluginID == Initializator.PROJECT_ID )
			{	if ( SelectChange && Selection.activeObject && Adapter.isProjectObjectBool( Selection.activeObject ) )
				{	newIds = new Int32List { };
					SET_INT32_AS_PROJECT( newIds, temp.Select( t => t.GetInstanceID() ).ToArray(), Selection.activeObject.GetInstanceID() );
					lastSel = temp;
				}
				
				else
				{	LastIndex = -1;
				}
			}
			
			
			
			
			
			
			if ( adapter.bottomInterface.hyperGraph.editorWindow ) adapter.bottomInterface.hyperGraph.editorWindow.Repaint();
			
			
			ClearAction();
		}
		
		
		internal void ClearAction()
		{
		
		
			foreach ( var windowController in WindowController )
			{	windowController.wasDrag = false;
				windowController.selection_action = null;
				windowController.selection_button = null;
				windowController.selection_window = null;
			}
			
			foreach ( var windowController in FavoritControllers )
			{	windowController.wasDrag = false;
				windowController.selection_action = null;
				windowController.selection_button = null;
				windowController.selection_window = null;
			}
			
			HierarchyController.wasDrag = false;
			HierarchyController.selection_action = null;
			HierarchyController.selection_button = null;
			HierarchyController.selection_window = null;
			
			
			hyperGraph.currentActionID = -1;
			favorGraph.currentActionID = -1;
			// HiperGraph.currentActionWindow = null;
			hyperGraph.WindowHyperController.currentAction = null;
			hyperGraph.HierHyperController.currentAction = null;
			favorGraph.WindowFavorController.currentAction = null;
			favorGraph.HierFavorController.currentAction = null;
		}
		
		
		internal string INSTANCEID_TOGUID( int instanceId )
		{	return AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( instanceId ) );
			/* var ob = UnityEditor.EditorUtility.InstanceIDToObject(instanceId);
			 if (!ob) return "";
			 return AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( ob ) );*/
		}
		
		internal void SET_INT32_AS_PROJECT( Int32List target, int[] IDs, int active )
		{	target.GUIDsList = IDs.Select( INSTANCEID_TOGUID ).ToList();
			target.GUIDsActiveGameObject_CheckAndGet = INSTANCEID_TOGUID( active );
			// target.FavParams = 1;
		}
		internal void SET_INT32_AS_HIERARCHY( Int32List target, int[] IDs, int active )
		{	target.list = IDs.Select( id => EditorUtility.InstanceIDToObject( id ) as GameObject ).ToList();
			target.ActiveGameObject = EditorUtility.InstanceIDToObject( active ) as GameObject;
		}
		
		
		
		
		
		internal string INT32_TOSTRING( Int32List target )
		{	if ( target == null ) return "";
		
			if ( adapter.IS_HIERARCHY() ) return target.GET_ActiveGameObject ? target.GET_ActiveGameObject.name : "";
			
#pragma warning disable
			target.TryToCheckAndGet();
			
			var result = adapter.GetHierarchyObjectByGUID(ref target.GUIDsActiveGameObject, ref target.PATHsActiveGameObject).project.assetName;
			return result ?? "";
#pragma warning restore
			/*    var result = adapter.GetHierarchyObjectByGUID(ref target.GUIDsActiveGameObject, ref target.PATHsActiveGameObject).GetHardLoadObject();
			    if ( !result ) return "";
			
			return result.name;*/
		}
		
		
		internal bool INT32_ACTIVE( Int32List target )
		{	if ( target == null ) return false;
		
		
			if ( adapter.IS_HIERARCHY() ) return target.GET_ActiveGameObject;
			
#pragma warning disable
			target.TryToCheckAndGet();
			return adapter.GetHierarchyObjectByGUID( ref target.GUIDsActiveGameObject, ref target.PATHsActiveGameObject ).Validate();
#pragma warning restore
			/*if ( target.wasInit ) return target.CashedBool;
			target.wasInit = true;
			target.CashedBool = !string.IsNullOrEmpty( AssetDatabase.GUIDToAssetPath( target.GUIDsActiveGameObject ) );
			return target.CashedBool;*/
		}
		
		internal bool INT32_ISVALID( Int32List target )
		{	if ( target == null ) return false;
		
			if ( adapter.IS_HIERARCHY() ) return target.GET_ActiveGameObject && target.GET_ActiveGameObject.scene.IsValid() && target.GET_ActiveGameObject.scene.isLoaded;
			
			return INT32_ACTIVE( target );
		}
		
		internal Adapter.HierarchyObject INT32__ACTIVE_TOHIERARCHYOBJECT( Int32List target )
		{	if ( target == null ) return null;
		
			if ( adapter.IS_HIERARCHY() ) return adapter.GetHierarchyObjectByInstanceID( target.GET_ActiveGameObject );
			
#pragma warning disable
			target.TryToCheckAndGet();
			return adapter.GetHierarchyObjectByGUID( ref target.GUIDsActiveGameObject, ref target.PATHsActiveGameObject );
#pragma warning restore
		}
		
		internal Object[] INT32_TOOBJECTASLISTCT( Int32List target )
		{	if ( target == null ) return new Object[0];
		
			if ( adapter.IS_HIERARCHY() ) return target.list.ToArray();
			
			var PATHids = target.GET_PATHsList(adapter.pluginname);
			var result = new Object[target.GUIDsList.Count];
			var activeObject =  target.GUIDsActiveGameObject_CheckAndGet;
			var activeIndex = -1;
			
			for ( int i = 0 ; i < target.GUIDsList.Count ; i++ )
			{	if ( activeObject == target.GUIDsList[i] ) activeIndex = i;
			
				var ts = target.GUIDsList[i];
				//Debug.Log( target.GUIDsList.Count + " " + PATHids.Count );
				var getted = adapter.GetHierarchyObjectByGUID(ref ts,  PATHids[i]);
				
				if ( ts != target.GUIDsList[i] )
				{	target.GUIDsList[i] = ts;
					adapter.ON_GUID_BACKCHANGED();
				}
				
				result[i] = getted.GetHardLoadObject();
			}
			
			if ( activeIndex != -1 )
			{	var a = result[activeIndex];
				UnityEditor.ArrayUtility.RemoveAt( ref result, activeIndex );
				
				///  UnityEditor.ArrayUtility.Add( ref result, a );
				if ( result.Length != 0 ) UnityEditor.ArrayUtility.Insert( ref result, 0, a );
				else UnityEditor.ArrayUtility.Add( ref result, a );
			}
			
			return result;
			//return target.GUIDsList.Select(g=>AssetDatabase.GUIDToAssetPath(g)).Where(p=>!string.IsNullOrEmpty(p)).Select(p=>AssetDatabase.LoadMainAssetAtPath(p)).ToArray();
		}
		
		internal int INT32_COUNT( Int32List target )
		{	if ( target == null ) return 0;
		
			if ( adapter.IS_HIERARCHY() ) return target.list.Count;
			
			return target.GUIDsList.Count;
			
		}
		
		
		internal int INT32_SCENE( Int32List target )
		{	if ( target == null ) return adapter.GET_ACTIVE_SCENE;
		
			return adapter.IS_HIERARCHY() ? target.GET_ActiveGameObject ? target.GET_ActiveGameObject.scene.GetHashCode() : adapter.GET_ACTIVE_SCENE : adapter.GET_ACTIVE_SCENE;
		}
		
		internal bool INT32_ISNULL( Int32List target )
		{	if ( target == null ) return true;
		
			if ( adapter.IS_HIERARCHY() ) return target.list == null;
			
			return target.GUIDsList == null;
		}
		
		
		internal string[] INT32_TOSTRINGARRAY( Int32List target )
		{	if ( target == null ) return new string[0];
		
			if ( adapter.IS_HIERARCHY() ) return target.list.Where( o => o ).Select( o => o.name ).ToArray();
			
			return adapter.bottomInterface.INT32_TOOBJECTASLISTCT( target ).Where( o => o ).Select( o => o.name ).ToArray();
		}
		
		
		
		private int GetLineHeight()
		{	var res = EditorGUIUtility.singleLineHeight;
		
			if ( adapter.par.ENABLE_ALL && !adapter.tempAdapterBlock ) res = adapter.parLINE_HEIGHT;
			
			return Mathf.RoundToInt( res );
		}
		float lastLineHeight = -1;
		FieldInfo lastLineHeightField ;
		FieldInfo lastLineHeightField_m_Value ;
		object astsvcvalue ;
		bool lastLineHeightFieldTryGet;
		internal void BOTTOM_UPDATE_POSITION( EditorWindow IN_FitteringWindow, bool log = false )     // MonoBehaviour.print(GETID());
		{
		
			// if ( Event.current != null && Event.current.type != EventType.Layout ) return;
			// log = true;
			if ( IN_FitteringWindow != null )
			{	var treeView = adapter.m_TreeView(IN_FitteringWindow);
			
				if ( treeView == null ) return;
				
				var gui = adapter.guiProp.GetValue(treeView, null);
				
				if ( adapter.k_LineHeight != null )
				{	var H = GetLineHeight();
				
				
					// adapter.HEIGHT_RIX_FUNCTIUON( IN_FitteringWindow, treeView );
					adapter.k_LineHeight.SetValue( gui, H );
					
					// Debug.Log( "ASD" );
					adapter.m_UseHorizontalScroll.SetValue( gui, adapter.par.USE_HORISONTAL_SCROLL );
					
					// MonoBehaviour.print(H);
					if ( H != EditorGUIUtility.singleLineHeight || adapter.NeedBottomPositionUpdate )
					{	adapter.NeedBottomPositionUpdate = true;
						adapter.foldoutYOffset.SetValue( gui, Mathf.RoundToInt( (H - EditorGUIUtility.singleLineHeight) / 2 ) );
						
						//MonoBehaviour.print(Event.current);
						
						if ( Event.current != null )
						{	var ping = adapter.m_Ping.GetValue(gui);
							var style = adapter. m_PingStyle.GetValue(ping);
							
							if ( style != null ) adapter.fixedHeight.SetValue( style, H, null );
							
							
							var gostyle = adapter.s_GOStyles != null ? adapter.s_GOStyles.GetValue(null) : null;
							// var gostyle = s_GOStyles.GetValue(null);
							// if (gostyle != null)
							{
							
							
								if ( Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_2_0_VERSION )
								{	var sceneStyle = adapter.sceneHeaderBg.GetValue(gostyle) as GUIStyle;
									sceneStyle.fixedHeight = adapter.parLINE_HEIGHT;
									sceneStyle.alignment = TextAnchor.MiddleLeft;
									
									if ( adapter.gs != null  )
									{
									
										if ( !lastLineHeightFieldTryGet )
										{	lastLineHeightFieldTryGet = true;
											lastLineHeightField = adapter.gs.GetField( "sceneHeaderWidth", (BindingFlags)(-1) );
											
											if ( lastLineHeightField  != null )
											{	lastLineHeightField_m_Value =
												    lastLineHeightField.FieldType.GetField( "m_Value", (BindingFlags)(-1) );
											}
										}
										
										var targetOffset = ( 1 - (adapter.parLINE_HEIGHT - 16) / 2 );
										
										if ( lastLineHeightField != null && (lastLineHeight != adapter.parLINE_HEIGHT || !ReferenceEquals( lastLineHeightField.GetValue( null ), astsvcvalue)
										                                     || (float)lastLineHeightField_m_Value.GetValue( lastLineHeightField.GetValue( null ) ) != targetOffset)
										   )
										{	/* var newval = Activator.CreateInstance( width .FieldType, new object[] { "SceneTopBarBg", "border-bottom-width", 0f, new long[0] } );    //new StyleState
											     width.SetValue( null, newval );*/
											astsvcvalue =     lastLineHeightField.GetValue( null  );
											lastLineHeightField_m_Value.SetValue( astsvcvalue, targetOffset);
											lastLineHeightField.SetValue( null, astsvcvalue );
											
											/*
											var v =    width .FieldType.GetProperty( "value", (BindingFlags)(-1) );
											Debug.Log((float) v.GetValue( width.GetValue( null ) ));*/
											lastLineHeight = adapter.parLINE_HEIGHT;
										}
									}
									
									
									// var prefabLabel = adapter. GetValue_Field(gs, "prefabLabel") as GUIStyle;
								}
								
								else
								{	var sceneStyle = adapter.sceneHeaderBg.GetValue(gostyle);
									adapter.fixedHeight.SetValue( sceneStyle, H == EditorGUIUtility.singleLineHeight ? EditorGUIUtility.singleLineHeight : 0, null );
									adapter.alignment.SetValue( sceneStyle, TextAnchor.MiddleLeft, null );
								}
								
								
								
								
								if ( lastHeight != H )
								{	lastHeight = H;
								
									if (UNITY_CURRENT_VERSION < UNITY_2019_1_1_VERSION )
									{	foreach ( var treestyle in adapter.treestyles )
										{	var st = treestyle.GetValue(gostyle);
											adapter.fixedHeight.SetValue( st, H == EditorGUIUtility.singleLineHeight ? EditorGUIUtility.singleLineHeight : 0, null );
										}
									}
									
									if ( adapter.lineStyle != null )
									{	var getst = adapter.s_Style != null ? adapter.s_Style.GetValue(null) : null;
										var st = adapter.lineStyle.GetValue(getst);
										adapter.alignment.SetValue( st, TextAnchor.MiddleLeft, null );
										adapter.alignment.SetValue( adapter.lineBoldStyle.GetValue( getst ), TextAnchor.MiddleLeft,
										                            null );
										// MonoBehaviour.print(fixedHeight.GetValue(st,null));
										//fixedHeight.SetValue(st, parLINE_HEIGHT == 16 ? 16 : 0, null);
										
										var pad3 = (RectOffset)adapter.paddingProp.GetValue(st, null);
										pad3.top = 0;
									}
								}
							}
							
							
							if ( H == EditorGUIUtility.singleLineHeight && Event.current.type == EventType.Repaint )
								adapter.NeedBottomPositionUpdate = false;
						}
					}
					
					/* for ( int i = 1 ; i < adapter.hierarchy_windows.Count ; i++ )
					 {   var t2 = adapter.m_TreeView(adapter.hierarchy_windows[i] as EditorWindow);
					     if ( t2 == null ) continue;
					     var g2 = adapter.guiProp.GetValue(t2, null);
					     adapter.k_BaseIndent.SetValue( g2, 2 );
					     adapter.m_UseHorizontalScroll.SetValue( gui, false );
					     adapter.k_IndentWidth.SetValue( g2, 14 );
					     adapter.k_LineHeight.SetValue( g2, EditorGUIUtility.singleLineHeight );
					     adapter.foldoutYOffset.SetValue( g2, 0 );
					
					     if ( Event.current != null )
					     {   var ping = adapter.m_Ping.GetValue(g2);
					         var style = adapter.m_PingStyle.GetValue(ping);
					         if ( style != null ) adapter.fixedHeight.SetValue( style, EditorGUIUtility.singleLineHeight, null );
					     }
					 }*/
				}
				
				if ( adapter.k_BottomRowMargin != null )
				{	adapter.k_BottomRowMargin.SetValue( gui, Mathf.RoundToInt( Mathf.RoundToInt( HEIGHT ) ) );
				}
				
				if ( !defaultextraInsertionMarkerIndent.HasValue ) defaultextraInsertionMarkerIndent = (float)adapter.k_BaseIndent.GetValue( gui );
				
				var addIndent = 0;
				
				if ( adapter.par.DEEP_WIDTH != null )
				{	addIndent = Mathf.RoundToInt( 14 - adapter.par.DEEP_WIDTH.Value );
					adapter.k_IndentWidth.SetValue( gui, adapter.par.DEEP_WIDTH.Value );
					//k_BaseIndent.SetValue(gui, Math.Max(2, 2 + 14 - par.DEEP_WIDTH.Value));
					//  var old = (k_BaseIndent.GetValue(gui));
					// k_BaseIndent.SetValue(gui, 2 + 14 - par.DEEP_WIDTH.Value);
					//MonoBehaviour.print(old + " " + k_BaseIndent.GetValue(gui));
					//
				}
				
				// if ( log ) Debug.Log( "ASD" );
				if ( adapter.par.USEdefaultIconSize ) adapter.k_IconWidth.SetValue( gui, adapter.par.defaultIconSize );
				else adapter.k_IconWidth.SetValue( gui, EditorGUIUtility.singleLineHeight );
				
				//  k_BaseIndent.SetValue( gui, defaultextraInsertionMarkerIndent + addIndent + (adapter.par.COLOR_ICON_SIZE - 12) );
				adapter.k_BaseIndent.SetValue( gui, defaultextraInsertionMarkerIndent + addIndent );
				
				/*  if (extraSpaceBeforeIconAndLabel != null)
				{
				    extraSpaceBeforeIconAndLabel.SetValue(gui, (parLINE_HEIGHT - 16f) / 2, null);
				}*/
			}
		}
		
		// Rect RR;
		internal Rect GetNavigatorRect( /*object treeView,*/ float width )
		{	/*  var isAnim = false;
			    if (Eve
			
			      try {
			          isAnim = (bool)m_Animating.GetValue(treeView, null);
			      }
			      catch {
			      }
			      var h = window().position.height;
			      if (!isAnim) {
			          //   h -= ((Rect)m_CurrentClipRect.GetValue(m_FramingAnimFloat.GetValue(treeView))).height;
			          // if ((bool)isExpanding.GetValue(m_FramingAnimFloat.GetValue(treeView), null)) h -= HEIGHT;
			          // h += HEIGHT;
			       //   RR.Set(0, h /*- HEIGHT#1# - (EditorGUIUtility.singleLineHeight - BOTTOMPADDINGOFFSET), width, HEIGHT + (EditorGUIUtility.singleLineHeight - BOTTOMPADDINGOFFSET));
			
			           oldr.Set(0, h /*- HEIGHT#1# + scrollPos.y - (EditorGUIUtility.singleLineHeight - BOTTOMPADDINGOFFSET), width, HEIGHT + (EditorGUIUtility.singleLineHeight - BOTTOMPADDINGOFFSET));
			      } else {
			
			      }*/
			// oldr = RR;
			// oldr.y += scrollPos.y;
			//  EditorGUIUtility.ScreenToGUIRect(
			// MonoBehaviour.print(isAnim);
			// if (isAnim && oldr.HasValue) return oldr.Value;
			
			
			/* if (adapter.NEW_INITIALIZER())
			 {   oldr.Set( adapter.TOTAL_LEFT_PADDING + adapter. PREFAB_BUTTON_SIZE,
			               adapter.window().position.height - HEIGHT + adapter.HierWinScrollPos.y - (EditorGUIUtility.singleLineHeight - BOTTOMPADDINGOFFSET),
			               width,
			               (EditorGUIUtility.singleLineHeight - BOTTOMPADDINGOFFSET) );
			 }
			 else*/
			
			// if ( Event.current.type != EventType.Repaint ) return lastRectGetNavigatorRect;
			
			{	oldr.Set( adapter.TOTAL_LEFT_PADDING_FORBOTTOM,
				          Mathf.RoundToInt( adapter.window().position.height + adapter.HierWinScrollPos.y -
				                            (EditorGUIUtility.singleLineHeight - BOTTOMPADDINGOFFSET) ),
				          width - adapter.TOTAL_LEFT_PADDING_FORBOTTOM,
				          (HEIGHT) + (EditorGUIUtility.singleLineHeight - BOTTOMPADDINGOFFSET) );
			}
			
			/* if ( UNITY_CURRENT_VERSION >= UNITY_2019_VERSION ) {
			if (adapter.pluginID == 0)    Debug.Log( Event.current.type + " " + oldr.y );
			     //if ( Mathf.RoundToInt( adapter.HierWinScrollPos.y ) % 2 != 1 ) oldr.y -= 1;
			     //if ( Mathf.RoundToInt( adapter.par.LINE_HEIGHT ) % 2 != 1 ) oldr.y -= 1;
			 }*/
			
			return oldr;
			
			/*  var wp = window().position;
			  wp.x+= wp.height -= HEIGHT;
			  wp.height = HEIGHT;
			wp.y += scrollPos.y;
			  return wp;*/
		}
		
		// Vector2 p1, p2;
		/*    void SetNavigationRect(object treeView, float width)
		   {
		       var isAnim = false;
		      // try {
		           isAnim = (bool)m_Animating.GetValue(m_FramingAnimFloat.GetValue(treeView));
		     /*  }
		       catch {
		       }#1#
		       var h = window().position.height;
		     //  if (isAnim) h += HEIGHT;
		     MonoBehaviour.print(isAnim);
		       GetNavigatorRect = new Rect(0, h /*- HEIGHT#1# + scrollPos.y - (16 - BOTTOMPADDINGOFFSET), width, HEIGHT + (16 - BOTTOMPADDINGOFFSET));
		   }*/
		
		private Rect GetLineRect( Rect rect )
		{	var line = rect;
			line.height = LINE_HEIGHT( true );
			//  line.y -= 2;
			
			line.x += 2;
			line.width -= 5;
			return line;
		}
		
		private Rect GetFoldOutRect( ref Rect lineRect )
		{	var foldRect = lineRect;
			foldRect.y += 1;
			foldRect.height = LINE_REFERENCE_HEIGHT - 2;
			//  foldRect.height = EditorGUIUtility.singleLineHeight - 2;
			
			lineRect.y += foldRect.height + 2;
			
			
			
			return foldRect;
		}
		
		private GUIContent GETTOOLTIPPEDCONTENT( MemType type, string upline, BottomController controller )
		{	___GETTOOLTIPPEDCONTENT.text = "";
			___GETTOOLTIPPEDCONTENT.tooltip = "";
			
			// if ( type == MemType.Custom ) Debug.Log( (controller.selection_action != null) + " " + IsValidDrag() + " " + (upline != null).ToString() );
			if ( controller.selection_action != null /*|| IsValidDrag() */) return ___GETTOOLTIPPEDCONTENT;
			
			if ( upline != null ) ___GETTOOLTIPPEDCONTENT.tooltip += upline + "\n";
			
			switch ( type )
			{	case MemType.Last:
					// ___GETTOOLTIPPEDCONTENT.tooltip += "You can switch between recently selected GameObjects";
					return ___GETTOOLTIPPEDCONTENT;
					
				case MemType.Custom:
					// ___GETTOOLTIPPEDCONTENT.tooltip += "You can store these GameObjects in this line";
					return ___GETTOOLTIPPEDCONTENT;
					
				case MemType.Scenes:
				
					// content.tooltip = objectIsHiddenAndLock ? "Object hided" : "Left CLICK/Left DRAG Show/Hide GameObject \n( Right CLICK/Right DRAG - Focus on the object in the SceneView )";
					//   ___GETTOOLTIPPEDCONTENT.tooltip += "Left CLICK - to load Scene\nShift+Left CLICK - to additive load Scene\nCtrl+Left CLICK - to select Scene in Project";
					return ___GETTOOLTIPPEDCONTENT;
					
				case MemType.Hier:
					___GETTOOLTIPPEDCONTENT.tooltip += "You can create a snapshot of objects that have been expanded";
					return ___GETTOOLTIPPEDCONTENT;
					
				default:
					throw new ArgumentOutOfRangeException( "type", type, null );
			}
		}
		
		
		internal int DESCRIPTION_MULTY( bool main )
		{	if ( main ) return adapter.FONT_8() + 2;
			else return (int)(adapter.FONT_8() * 1.5f);
		}
		
		internal int LINE_HEIGHT( bool? main, bool custom = false )
		{	int result;
		
			if ( adapter.par.HEIGHT_APPLY_TOBOTTOM )
				result =
				    Mathf.RoundToInt( ((adapter.parLINE_HEIGHT - LINE_REFERENCE_HEIGHT) / 2 + LINE_REFERENCE_HEIGHT) *
				                      adapter.HALFFACTOR_8() );
			else
				result =
				    Mathf.RoundToInt( ((LINE_REFERENCE_HEIGHT - LINE_REFERENCE_HEIGHT) / 2 + LINE_REFERENCE_HEIGHT) *
				                      adapter.HALFFACTOR_8() );
				                      
			if ( !custom || !main.HasValue ) return result;
			
			result = (int)(result * 1.35f + adapter.par.ADDITIONAL_BOOKMARKS_HEIGHT);
			
			if ( main.Value && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INHIER ) result = (int)(result + DESCRIPTION_MULTY( true ));
			
			if ( !main.Value && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN ) result = (int)(result + DESCRIPTION_MULTY( false ));
			
			return result;
		}
		
		/*  float ___REFERENCE_HEIGHT_AUTOHIDE = 64 + BOTTOMPADDINGOFFSET - 32 + 1;
		  float ___REFERENCE_HEIGHT_FULL = 64 + BOTTOMPADDINGOFFSET + 1;*/
		
		private int ___REFERENCE_HEIGHT_AUTOHIDE()     //var h = LINE_HEIGHT(true, true);
		{	return /*h * adapter.par.BOTTOM_MAXCUSTOMROWS +*/ Mathf.RoundToInt( LINE_REFERENCE_HEIGHT + BOTTOMPADDINGOFFSET + 1 + GRAPH_HEIGHT() );
		}
		
		private int ___REFERENCE_HEIGHT_FULL()     //   var h1 = LINE_HEIGHT(true, true);
		{	//  var h = LINE_HEIGHT(true);
			var otherH = BOTTOMPADDINGOFFSET + 1 + GRAPH_HEIGHT() + LINE_REFERENCE_HEIGHT;
			return Mathf.RoundToInt( RowsParams[0].FULL_HEIGHT + RowsParams[1].FULL_HEIGHT + RowsParams[2].FULL_HEIGHT + RowsParams[3].FULL_HEIGHT + otherH );
			// return h1 * adapter.par.BOTTOM_MAXCUSTOMROWS + LINE_REFERENCE_HEIGHT + h +  +                       h * adapter.par.BOTTOM_MAXLASTROWS +
		}
		
		
		
		/*    private  void PLAY_MODE_CHANGE_ACTION()
		    {
		        PLAYMODECHANGE = true;
		       /* if (window() != null)
		        {
		            MonoBehaviour.print("A" + (Rect)m_Pos.GetValue(window()));
		            var winrect = (Rect)m_Pos.GetValue(window());
		            // WINH = winrect.height - Math.Max(winrect.height - 140, 0);
		            //winrect.height -= WINH.Value;
		            // winrect.height -= 150;
		            m_Pos.SetValue(window(), winrect);
		
		
		            var treeView = m_TreeView.GetValue(window(), null);
		            // if (Event.current.type == EventType.layout) MonoBehaviour.print(mVisibleRect);
		            m_VisibleRect.SetValue(treeView, winrect);
		            //  print("B" + (Rect)m_Pos.GetValue(window()));
		        }#1#
		        //   print("A" + winrect);
		    }
		*/
		//  object[] args = new object[1] { false };
		//  bool applicationIsPlaying = false;
		
		Type I_ITreeViewDataSource;
		MethodInfo I_DATA_GetExpandedIDs;
		
		internal void EXPAND_SWITCHER( HierarchyObject o, bool? expandOverride = null)
		{	var tree = adapter.m_TreeView(adapter.window());
		
			var data = adapter.m_data.GetValue(tree, null);
			
			try
			{	if ( adapter.IS_HIERARCHY() )
				{	if ( o.go.transform.childCount != 0 )
					{
#if HIERARCHY
						var expanded = expandOverride ?? !(bool)adapter.m_dataIsExpanded.Invoke(data, new[] { (object)o.go.GetInstanceID() });
						HierarchyExtensions.Utilities.SetExpanded( o.go.GetInstanceID(), expanded );
						Adapter.EventUseFast();
#endif
					}
				}
				
				else
				{	if ( o.project.IsFolder )     //    Debug.Log( o.id );
					{
#if PROJECT
						var expanded = expandOverride ?? !(bool)adapter.m_dataIsExpanded.Invoke(data, new[] { (object)o.id });
						ProjectExtensions.Utility.SetExpandedInProjectWindow( o.id, expanded );
						Adapter.EventUseFast();
#endif
					}
					
				}
			}
			
			catch ( Exception ex )
			{	Debug.LogError( ex.Message + "\n\n" + ex.StackTrace );
			}
			
			
			
			adapter.RepaintWindowInUpdate();
			/*var tree = adapter.m_TreeView.GetValue(adapter.window());
			
			var data = adapter.m_data.GetValue(tree, null);
			
			//MonoBehaviour.print(wasMethod);
			if(I_ITreeViewDataSource== null) I_ITreeViewDataSource = data.GetType().GetInterfaces().FirstOrDefault(i => i.Name.Contains("ITreeViewDataSource"));
			
			
			    if (I_DATA_GetExpandedIDs == null) I_DATA_GetExpandedIDs = data.GetType().GetInterfaceMap(I_ITreeViewDataSource).InterfaceMethods.First(m => m.Name == "GetExpandedIDs");
			    var expandedIds = I_DATA_GetExpandedIDs.Invoke(data, null) as int[];
			
			    var expand = expandedIds.Contains(id);
			
			HierarchyExtensions.Utilities.SetExpanded(id,! expand);
			    Debug.Log("dfg");*/
			//WINDOW_expandMethod.Invoke(adapter.window(), new object[] { id, expand });
		}
		
		
	}
}
}
