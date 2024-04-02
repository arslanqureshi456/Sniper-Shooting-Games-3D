#define HIERARCHY
#if UNITY_EDITOR
	#define PROJECT
#endif

//#define LITE




#if UNITY_2017_1_OR_NEWER || !UNITY_EDITOR
	#define USE2017
#endif
#if UNITY_2018_1_OR_NEWER || !UNITY_EDITOR
	#define USE2018
#endif
#if UNITY_2018_2_OR_NEWER || !UNITY_EDITOR
	#define USE2018_2
#endif
#if UNITY_2018_3_OR_NEWER || !UNITY_EDITOR
	#define USE2018_3
#endif





using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using EModules.EModulesInternal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using UnityEditorInternal;
using System.Reflection;
#if PROJECT
	using EModules.Project;
#endif


namespace EModules.EModulesInternal



{

partial class Adapter {
	internal const string HIERARCHY_VERSION = "2019.3p8";
	internal static string[] HIERARCHY_OLDER_VERSIONS = new string[] {  "2019.3",  "2019.3p2", "2019.3p4", "2019.3p6" };
	
	/* internal const string PROJECT_VERSION = "20.1";
	 internal static string[] PROJECT_OLDER_VERSIONS = new string[] {"v2018.0.9b", "v2018.1p1" };*/
	
	internal const string SOURCE_FOLDER = "Hierarchy And Project Editor Dll Source";
	
	
	
	
	
#if USE2018
	internal const bool USE2018 = true;
#else
	internal const bool USE2018 = false;
#endif
	
	
#if LITE
	internal const bool LITE = true;
#else
	internal const bool LITE = false;
#endif
	
	
#if USE2018_3
	internal const bool USE2018_3 = true;
#else
	internal const bool USE2018_3 = false;
#endif
	
	
	internal const bool ALLOW_FOLDER_SAVER = true;
	/*#if UNITY_EDITOR
	#else
	        internal const bool ALLOW_FOLDER_SAVER = false;
	#endif*/
	
	
	
	internal bool NEW_PERFOMANCE
	{	get
		{	if ( pluginID != 0 ) return false;
		
			return UNITY_CURRENT_VERSION >= UNITY_2019_VERSION;
		}
	}
	
	
	
	static MethodInfo _RequestScriptReload;
	internal static void
	EditorUtilityRequestScriptReload()
	{	// #if USE2017
		/**/
		
		if ( UNITY_CURRENT_VERSION >= UNITY_2019_3_0_VERSION )
		{	if ( _RequestScriptReload == null )
			{	_RequestScriptReload = (typeof( EditorUtility )).GetMethod( "RequestScriptReload", (BindingFlags)(-1) );
			}
			
			if ( _RequestScriptReload != null ) _RequestScriptReload.Invoke( null, null );
		}
		
		else
		{
#pragma warning disable
			InternalEditorUtility.RequestScriptReload();
#pragma warning restore
		}
		
		//EditorUtility.RequestScriptReload();
		//  #else
		// AssetDatabase.Refresh();
		// #endif
	}
	
	
	//   internal const bool NEW_RELOAD = USE2018_3;
#pragma warning disable
	internal static bool __NEW_RELOAD = false;
	internal static bool NEW_RELOAD
	{	get
		{	return true;
			//return UNITY_CURRENT_VERSION >= UNITY_2019_2_0_VERSION || USE2018_3 && __NEW_RELOAD;
		}
		
		set { __NEW_RELOAD = value; }
	}
#pragma warning restore
	
	
	
	// static class asd {
	
	internal static void
	staic_reload_a()
	{
#if USE2018_3
		EditorSceneManager.activeSceneChangedInEditMode -= SCN2;
		EditorSceneManager.activeSceneChangedInEditMode += SCN2;
#endif
	}
	
	public GameObject
	FindPrefabRoot( GameObject o )
	{	if ( !o || pluginID != 0) return null;
	
#if USE2018_3
		return UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot( o );
#else
		return UnityEditor.PrefabUtility.FindPrefabRoot( o );
#endif
	}
	
	public GameObject
	GetPrefabInstanceHandleGameObject( GameObject prefab_root )
	{	return GetPrefabInstanceHandle( prefab_root ) as GameObject;
	}
	public UnityEngine.Object
	GetPrefabInstanceHandle( UnityEngine.Object prefab_root )
	{	if ( !prefab_root ) return null;
	
		return GetCorrespondingObjectFromSource( prefab_root );
	}
	UnityEngine.Object _tempP;
	public UnityEngine.Object
	GetCorrespondingObjectFromSource( UnityEngine.Object
	                                  prefab_root )     //Debug.Log((prefab_root == null) + " " + (Hierarchy_GUI.Instance(this) == null) + " " + ( Hierarchy_GUI.Instance(this).PrefabsDic == null));
	{	if ( !prefab_root ) return null;
	
		//  Hierarchy_GUI.Instance(this);
		if ( !Hierarchy_GUI.Instance( this ).GET_prefab.GetValueByKey( prefab_root.GetInstanceID(), out _tempP ) )
		{
#if USE2018_3 || USE2018_2
			_tempP = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource( prefab_root );
#else
			_tempP = UnityEditor.PrefabUtility.GetPrefabParent( prefab_root );
#endif
			Hierarchy_GUI.Instance( this ).GET_prefab.PrefabsDicAdd( prefab_root.GetInstanceID(), _tempP );
		}
		
		return _tempP;
		
	}
	
	public GameObject
	ReplacePrefab( GameObject prefab_root, UnityEngine.Object prefab_src )
	{
#if USE2018_3
		return UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect( prefab_root, AssetDatabase.GetAssetPath( prefab_src ), InteractionMode.AutomatedAction );
#else
		return UnityEditor.PrefabUtility.ReplacePrefab( prefab_root, prefab_src, ReplacePrefabOptions.ConnectToPrefab );
#endif
	}
	
	public enum PrefabInstanceStatus
	{	NotAPrefab = 0,
		Connected = 1,
		Disconnected = 2,
		MissingAsset = 3
	}
	
	public PrefabInstanceStatus
	GetPrefabType( GameObject prefab_root )
	{
#if USE2018_3
		return (PrefabInstanceStatus)UnityEditor.PrefabUtility.GetPrefabInstanceStatus( prefab_root );
#else
		var t = UnityEditor.PrefabUtility.GetPrefabType(prefab_root);
		
		if ( t == UnityEditor.PrefabType.None ) return PrefabInstanceStatus.NotAPrefab;
		
		if ( t == UnityEditor.PrefabType.DisconnectedModelPrefabInstance || t == UnityEditor.PrefabType.DisconnectedPrefabInstance ) return PrefabInstanceStatus.Disconnected;
		
		if ( t == UnityEditor.PrefabType.MissingPrefabInstance ) return PrefabInstanceStatus.MissingAsset;
		
		return PrefabInstanceStatus.Connected;
#endif
	}
	
	
	// }
}

[Serializable]
public class ASD2 {

}








[InitializeOnLoad]
internal class Initializator {

	internal const string HIERARCHY_NAME = "Hierarchy";
	internal const string PROJECT_NAME = "Project";
	
	internal const int HIERARCHY_ID = 0;
	internal const int PROJECT_ID = 1;
	
	static Dictionary<string, Adapter> __AdaptersByName = new Dictionary<string, Adapter>();
	internal static Dictionary<string, Adapter> AdaptersByName
	{	get
		{	if ( !wasInitializator ) InitializatorVoid();
		
			return __AdaptersByName;
		}
	}
	static Dictionary<int, Adapter> __AdaptersByID = new Dictionary<int, Adapter>();
	internal static Dictionary<int, Adapter> AdaptersByID
	{	get
		{	if ( !wasInitializator ) InitializatorVoid();
		
			return __AdaptersByID;
		}
	}
	
	
	internal static string[] plugins =
	{
#if HIERARCHY
		HIERARCHY_NAME,
#endif
#if PROJECT
		PROJECT_NAME,
#endif
	};
	
	static int[] pluginsid =
	{
#if HIERARCHY
		0,
#endif
#if PROJECT
		1,
#endif
	};
	
	
	static bool wasInitializator = false;
	static
	Initializator()
	{	InitializatorVoid();
	}
	
	static void
	InitializatorVoid()     //AA.testc();
	{
	
		wasInitializator = true;
		
		// var namespacestring = Activator.CreateInstance<Initializator>().GetType().Namespace;
		AdaptersByName.Clear();
		AdaptersByID.Clear();
		string[] c1 =  new string[2] { HIERARCHY_NAME + "_KEY_#1", PROJECT_NAME + "_KEY_#1" };
		
		
		for ( int i = 0 ; i < plugins.Length ; i++ )
			//foreach ( var plugin in plugins )
		{	var plugin = plugins[i];
		
		
			/* #if UNITY_EDITOR
			 var adapter = plugin == PROJECT_NAME ? new EModules.EProjectInternal.Adapter() : new Adapter();
			#else
			 var adapter = new Adapter();
			#endif*/
			var adapter = new Adapter();
			// Debug.Log( i );
			
			adapter.pluginID = i;
			adapter.pluginname = plugin;
			
			
			if ( !AdaptersByName.ContainsKey( plugin ) ) AdaptersByName.Add( plugin, adapter );
			else AdaptersByName[plugin] = adapter;
			
			if ( !AdaptersByID.ContainsKey( i ) ) AdaptersByID.Add( i, adapter );
			else AdaptersByID[i] = adapter;
			
			//var flags = (System.Reflection.BindingFlags)int.MaxValue;
			
#if UNITY_EDITOR
			
			if ( plugin == PROJECT_NAME ) EProjectInternal.Project.AttachAdapter( adapter );
			
#endif
			
			if ( plugin == HIERARCHY_NAME ) EModulesInternal.Hierarchy.AttachAdapter( adapter );
			
			// t.GetMethod( "AttachAdapter", flags ).Invoke( null, new object[] { adapter } );
			adapter.InitializateAdapter( plugin, pluginsid[i] );
			
			
			
			
		}
		
		foreach ( var tex in Resources.FindObjectsOfTypeAll<Texture2D>() )
			if ( tex && (tex.name == c1[0] || tex.name == c1[1]) ) UnityEngine.Object.DestroyImmediate( tex );
	}
	
}



public partial class M_Colors_Window : _W___IWindow {

#if USE2018
	private void
	OnEnable()
	{	EditorApplication.quitting -= _apquite;
		EditorApplication.quitting += _apquite;
	}
	void
	_apquite()
	{	if ( this ) base.Close();
	}
#endif
	
	private void
	OnDestroySwitcher()
	{
#if USE2018
		EditorApplication.quitting -= _apquite;
#endif
	}
}

public partial class _W__SearchWindow : _W___IWindow {
#if USE2018
	private void
	OnEnable()
	{	EditorApplication.quitting -= _apquite;
		EditorApplication.quitting += _apquite;
	}
	private void
	OnDestroy()
	{	EditorApplication.quitting -= _apquite;
	}
	void
	_apquite()
	{	CloseThis();
	}
#endif
}



class MyInternalItem {

}

class MyInternalProjectItem {

}





internal partial class Adapter {




#if !USE2017
	static  GUIStyle helperStyle = new GUIStyle();
#endif
	internal static void
	DrawTexture( Rect rect, Texture whiteTexture, ScaleMode StretchToFill, bool al, float ac, Color col, float border, float rad )
	{
#if USE2017
		GUI.DrawTexture( rect, whiteTexture, StretchToFill,  al,  ac,  col,  border,  rad );
#else
		
		if ( border  == 0 )
		{	var c = GUI.color;
			GUI.color *= col;
			GUI.DrawTexture( rect, whiteTexture, StretchToFill  );
			GUI.color = c;
		}
		
		else
		{	if (Event.current.type == EventType.Repaint )
			{	var c = GUI.color;
				GUI.color *= col;
				helperStyle.border.bottom = helperStyle.border.top = helperStyle.border.left = helperStyle.border.right = (int)border;
				helperStyle.normal.background = (Texture2D)whiteTexture;
				helperStyle.Draw( rect, "", false, false, false, false );
				GUI.color = c;
			}
		
		}
		
		
#endif
	}
	
	//#pragma warning disable
	//Vector4 bordersWidth , radiusWidth ;
	//  static Rect nR = new Rect(0,0,1,1);
	internal static void
	DrawTexture( Rect rect, Color col, float A )
	{	if ( Event.current.type != EventType.Repaint ) return;
	
		col.a *= A;
		//  EditorGUI.DrawRect( rect, col );
#if USE2017
		GUI.DrawTexture( rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1, col, 0, 0 );
#else
		DrawTexture( rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1, col, 0, 0 );
#endif
		
		// Graphics.DrawTexture( rect, Texture2D.whiteTexture, sourceRect, 0, 0, 0, 0, col, def_mat, 0 );
		//  Graphics.DrawTexture( rect , Texture2D.whiteTexture , nR , 0 , 0 , 0 , 0 , col , null );
	}
	internal static void
	DrawTexture( Rect rect, Color col )       //  EditorGUI.DrawRect( rect, col );
	{	if ( Event.current.type != EventType.Repaint ) return;
	
#if USE2017
		GUI.DrawTexture( rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1, col, 0, 0 );
#else
		DrawTexture( rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 1, col, 0, 0 );
#endif
		//  Graphics.DrawTexture( rect, Texture2D.whiteTexture, sourceRect, 0, 0, 0, 0, col, def_mat, 0 );
		//  Graphics.DrawTexture( rect , Texture2D.whiteTexture , nR , 0 , 0 , 0 , 0 , col , null );
	}
	internal static void
	DrawTexture( Rect rect, Texture tex, Color col )
	{	/*var oc = GUI.color;
		GUI.color *= col;*/
		// Graphics.DrawTexture( rect , tex );
		// GUI.DrawTexture( rect, tex, ScaleMode.ScaleToFit );
		if ( Event.current.type != EventType.Repaint ) return;
		
#if USE2017
		GUI.DrawTexture( rect, tex, ScaleMode.ScaleToFit, true, 1, col, 0, 0 );
#else
		DrawTexture( rect, tex, ScaleMode.ScaleToFit, true, 1, col, 0, 0 );
#endif
		// Graphics.DrawTexture( rect, tex, sourceRect, 0, 0, 0, 0, col, def_mat, 0 );
		// GUI.color = oc;
		/* GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit, true, 1, col, 0, 0);*/
	}
#pragma warning disable
	/*  Material _def_mat;
	 internal Material def_mat
	 {   get
	     {   if ( !_def_mat )
	         {   _def_mat = DEFAULT_SHADER_SHADER.HIghlighterExternalMaterial;
	         }
	         return _def_mat;
	     }
	 }*/
	static  Rect sourceRect = new Rect(0, 0, 1, 1);
#pragma warning restore
	internal static void
	DrawTexture( Rect rect, Texture tex )        /*  GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit, true, 1, Color.white, 0, 0);*/
	{	// GUI.DrawTexture( rect, tex, ScaleMode.ScaleToFit );
		if ( Event.current.type != EventType.Repaint ) return;
		
#if USE2017
		GUI.DrawTexture( rect, tex, ScaleMode.ScaleToFit, true, 1, GUI.color, 0, 0 );
#else
		DrawTexture( rect,  tex, ScaleMode.ScaleToFit, true, 1, GUI.color, 0, 0 );
#endif
		//  Graphics.DrawTexture( rect, tex, sourceRect, 0, 0, 0, 0, GUI.color, def_mat, 0 );
		// Graphics.DrawTexture( rect , tex );
	}
	internal static void
	DrawTexture_StretchToFill( Rect rect, Texture tex )         /*  GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit, true, 1, Color.white, 0, 0);*/
	{	// GUI.DrawTexture( rect, tex, ScaleMode.ScaleToFit );
		if ( Event.current.type != EventType.Repaint ) return;
		
#if USE2017
		GUI.DrawTexture( rect, tex, ScaleMode.StretchToFill, true, 1, GUI.color, 0, 0 );
#else
		DrawTexture( rect, tex, ScaleMode.StretchToFill, true, 1, GUI.color, 0, 0 );
#endif
		//  Graphics.DrawTexture( rect, tex, sourceRect, 0, 0, 0, 0, GUI.color, def_mat, 0 );
		
		// Graphics.DrawTexture( rect , tex );
	}
	
	internal static void
	DrawRect( Rect rect, Color color )
	{	DrawTexture( rect, color );
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	internal   int firstFrame = 0;
	float? _TOTAL_LEFT_PADDING;
	internal float raw_old_leftpadding
	{	get
		{	if ( pluginID != 0 ) return 0;
		
			if ( UNITY_CURRENT_VERSION >= UNITY_2019_VERSION )
			{	if ( !_TOTAL_LEFT_PADDING.HasValue )
				{	_TOTAL_LEFT_PADDING =
					    (float)SceneHierarchyWindowRoot.Assembly.GetType( "UnityEditor.SceneVisibilityHierarchyGUI" ).GetField( "utilityBarWidth", (BindingFlags)(-1) )
					    .GetValue( null );
					    
					// Debug.Log( UNITY_CURRENT_VERSION + " " + UNITY_2019_2_0_VERSION );
				}
				
				return _TOTAL_LEFT_PADDING.Value;
			}
			
			return 0;
		}
	}
#pragma warning disable
	internal float? TEMP_LEFT_CACHE_FOR_BOTTOM_REMOVED_IF_REPAINT;
	internal float? TEMP_LEFT_CACHE_FOR_BOTTOM_AFTER_REPAINT;
	internal float? TEMP_LEFT_CACHE_FOR_BOTTOM;
#pragma warning restore
	internal float TOTAL_LEFT_PADDING_FORBOTTOM
	{	get
		{
		
			if ( pluginID != Initializator.HIERARCHY_ID ) return 0;
			
			/*  if ( TEMP_LEFT_CACHE_FOR_BOTTOM_REMOVED_IF_REPAINT.HasValue )
			  {   var res  = TEMP_LEFT_CACHE_FOR_BOTTOM_REMOVED_IF_REPAINT.Value;
			      if ( Event.current.type == EventType.Repaint ) TEMP_LEFT_CACHE_FOR_BOTTOM_REMOVED_IF_REPAINT = null;
			      else return res;
			  }
			  if ( TEMP_LEFT_CACHE_FOR_BOTTOM.HasValue )
			  {   var res = TEMP_LEFT_CACHE_FOR_BOTTOM.Value;
			      if ( Event.current.type == EventType.Repaint )
			      {   TEMP_LEFT_CACHE_FOR_BOTTOM_AFTER_REPAINT = TEMP_LEFT_CACHE_FOR_BOTTOM;
			          TEMP_LEFT_CACHE_FOR_BOTTOM = null;
			      }
			      else return res;
			  }
			  if ( TEMP_LEFT_CACHE_FOR_BOTTOM_AFTER_REPAINT .HasValue )
			  {   var res  = TEMP_LEFT_CACHE_FOR_BOTTOM_AFTER_REPAINT.Value;
			      if ( Event.current.type != EventType.Repaint ) TEMP_LEFT_CACHE_FOR_BOTTOM_AFTER_REPAINT = null;
			      else  return res;
			  }*/
			
			/*
			
			  Debug.Log( visualRect + " " + ContentSize + " " + bottomInterface.HEIGHT);*/
			
			if ( UNITY_CURRENT_VERSION >= UNITY_2019_2_0_VERSION  ) //1001
			{	/*if ( firstFrame < 5 ) return 0;
				if ( RedrawInit ) return 0;*/
				
				var visualRect = (Rect)m_VisibleRect.GetValue( m_TreeView( window() ) );
				
				if ( ContentSize.y >= visualRect.height + bottomInterface.HEIGHT - 5) return 0;
				
				/* if ( bottomInterface.lastBottomRectSelectLine.HasValue &&
				         bottomInterface.lastBottomRectSelectLine.Value.y + parLINE_HEIGHT + 2 >= bottomInterface.lastBottomRectUI.Value.y - bottomInterface.lastBottomRectUI.Value.height / 2 )
				     return 0;*/
			}
			
			return raw_old_leftpadding;
			
		}
	}
	internal float TOTAL_LEFT_PADDING
	{	get
		{	if ( UNITY_CURRENT_VERSION >= UNITY_2019_2_0_VERSION ) return 0;  //1001
		
			return TOTAL_LEFT_PADDING_FORBOTTOM;
		}
	}
	
#if USE2018_3
	//  internal  float PREFAB_BUTTON_SIZE = 0;
	internal float PREFAB_BUTTON_SIZE
	{	get
		{	//if ( UNITY_CURRENT_VERSION >= UNITY_2019_1_1_VERSION ) return 0;
		
			return pluginID == Initializator.HIERARCHY_ID ? EditorGUIUtility.singleLineHeight : 0;
		}
	}
#else
	internal  float PREFAB_BUTTON_SIZE = 0;
#endif
	
	
	//[MenuItem("___/CreateObject")] static void Cob() {new GameObject();}
	void
	INIT_HEIGHT_FIX()
	{
#if USE2018
		//if (NEW_RELOAD)
		{	EditorApplication.hierarchyChanged -= OnHierarchyChanged;
			EditorApplication.hierarchyChanged += OnHierarchyChanged;
			
			
		}
#endif
	}
	
	internal  void
	SupscribeToHierarchyChanged(Action ac)
	{
#if USE2018
		//if (NEW_RELOAD)
		{	EditorApplication.hierarchyChanged -= ac;
			EditorApplication.hierarchyChanged += ac;
			
			
		}
#endif
	}
	
	
	public void
	OnHierarchyChanged()
	{	HeightFixIfDrag();
	
		if ( pluginID == 0 && bottomInterface != null && bottomInterface.hyperGraph != null ) bottomInterface.hyperGraph.RefreshWithCurrentSelection();
		
		if (  ColorModule != null ) ColorModule.ClearCache();
		
		/* if ( !NEW_PERFOMANCE ) return;
		 //#todo check cache
		 if ( ColorModule != null ) ColorModule.ClearCacheAdditional();*/
	}
	
	public void
	CLearAdditionalCache()
	{	// if ( !NEW_PERFOMANCE ) return;
		ColorModule.ClearCacheAdditional();
	}
	
	/* [MenuItem( "ASDASDA/asdadsa" )]
	 static void ASDASD() {
	     new GameObject( "ASD" );
	 }*/
	internal static bool AGAIM_HEIGHT_FIXER
	{	get
		{	return false;
			//return NEW_RELOAD || USE2018 && !USE2018_3; // USE2018
			//return USE2018; // USE2018
		}
	}
	
	internal static float UNITY_2019_VERSION
	{	get { return 2019; }
	}
	internal static float UNITY_2019_2_0_VERSION
	{	get { return 2019.101f; }
	}
	internal static float UNITY_2019_3_0_VERSION
	{	get { return 2019.3f; }
	}
	internal static float UNITY_2019_1_1_VERSION
	{	get { return UNITY_2019_VERSION; }
	
		// { get { return 2019.1001f; }
	}
	static float? __UNITY_CURRENT_VERSION;
	internal static float UNITY_CURRENT_VERSION
	{	get
		{	if ( __UNITY_CURRENT_VERSION.HasValue ) return __UNITY_CURRENT_VERSION.Value;
		
			var v = Application.unityVersion.Replace('f', '.').Replace('b', '.').Replace('a', '.') .ToCharArray().Where(IsNumb).Select(c => c.ToString()).Aggregate((a, b) => a + b).Split('.');
			var year = int.Parse( v[0]);
			var quart = int.Parse( v[1]) / 10f;
			var revis = 0f;
			
			try
			{	revis = int.Parse( v[2] ) / 1000f;
			}
			
			catch
			{
			}
			
			__UNITY_CURRENT_VERSION = year + quart + revis;
			//	Debug.Log( Application.unityVersion + " " + __UNITY_CURRENT_VERSION + " " + UNITY_2019_1_1_VERSION );
			return __UNITY_CURRENT_VERSION.Value;
		}
	}
	static bool
	IsNumb( char c )
	{	if ( c == '0' ) return true;
	
		if ( c == '1' ) return true;
		
		if ( c == '2' ) return true;
		
		if ( c == '3' ) return true;
		
		if ( c == '4' ) return true;
		
		if ( c == '5' ) return true;
		
		if ( c == '6' ) return true;
		
		if ( c == '7' ) return true;
		
		if ( c == '8' ) return true;
		
		if ( c == '9' ) return true;
		
		if ( c == '.' ) return true;
		
		return false;
	}
	
	internal static Color
	COLOR_FIELD( Rect cRect, GUIContent gUIContent, Color oldCol, bool v1, bool v2, bool v3 )
	{
	
#if USE2018
		return EditorGUI.ColorField( cRect, new GUIContent(), oldCol, false, true, false );
#else
		return EditorGUI.ColorField( cRect, new GUIContent(), oldCol, false, true, false, null );
		// return EditorGUI.ColorField( cRect, new GUIContent(), oldCol, false, true, false, null );
#endif
		
	}
	
	
#if USE2017
	void
	EditorApplication_playModeStateChanged( PlayModeStateChange obj )
	{	PlayModeChanged = true;
		wasRedrawGoups.Clear();
		
		
		/* if ( IMGUI() )
		  {   PushActionToUpdate( () =>
		      {   PushActionToUpdate( () =>
		          {   bottomInterface.NeedRefreshBOttom = true;
		              if ( hierarchy_windows == null ) return;
		              foreach ( var hierarchyWindow in hierarchy_windows )
		              {   if ( hierarchyWindow == null ) continue;
		                  var tree = m_TreeView(hierarchyWindow as EditorWindow);
		                  if ( tree == null ) continue;
		                  GUIUtility.keyboardControl = (int)m_KeyboardControlIDField.GetValue( tree );
		                  bottomInterface.BOTTOM_UPDATE_POSITION( (EditorWindow)hierarchyWindow );
		
		                  ((EditorWindow)hierarchyWindow).Repaint();
		              }
		              RepaintWindow( true );
		          } );
		      } );
		  }*/
		
		
		if ( obj == PlayModeStateChange.EnteredPlayMode || obj == PlayModeStateChange.EnteredEditMode )
			PlayModeFix = false;
			
		if (/*obj == PlayModeStateChange.ExitingEditMode ||*/ obj == PlayModeStateChange.ExitingPlayMode )
			PlayModeFix = true;
	}
	
	
	
	internal  IEnumerator<UnityEngine.Networking.UnityWebRequestAsyncOperation>   videoRequest = null;
	internal void
	VideoRequest()
	{	videoRequest = __VideoRequest().GetEnumerator();
	}
#pragma warning disable
	
	internal IEnumerable<UnityEngine.Networking.UnityWebRequestAsyncOperation>
	__VideoRequest()
	{
	
		using ( var w = new WWW( "http://emem.store/requestvideourl.json" ) )
		{	while ( w.MoveNext() ) yield return null;
		
			Application.OpenURL( w.text );
		}
		
		/*
		using (var wwwSignin =   UnityEngine.Networking.UnityWebRequest.Get("http://emem.store/requestvideourl.json"))
		{
		
		    wwwSignin.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
		
		    yield return wwwSignin.SendWebRequest();
		
		
		    Debug.Log(wwwSignin.downloadHandler.data);
		    Debug.Log(wwwSignin.downloadHandler.isDone);
		    Debug.Log(wwwSignin.downloadHandler.text);
		
		    if (!wwwSignin.isNetworkError )
		    {   Application.OpenURL( wwwSignin.downloadHandler.text );
		    }
		}*/
		//  var wwwSignin = new UnityEngine.Networking.UnityWebRequest.Post("http://emem.store/requestvideourl.php",);
		//  wwwSignin.downloadHandler = (UnityEngine.Networking.DownloadHandler) new UnityEngine.Networking.DownloadHandlerBuffer();
		
	}
#pragma warning restore
	
	
	internal void
	SubcripePlayModeStateChange()
	{
	
		//  SubcripeSceneLoader_method = method;
		//#if !USE2017
		EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
		EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
#pragma warning disable
		EditorApplication.playmodeStateChanged -= PlayModeStateChanged;
		EditorApplication.playmodeStateChanged += PlayModeStateChanged;
#pragma warning restore
		//#else
		//			EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
		//			EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
		//EditorApplication.playmodeStateChanged += EditorApplication_playModeStateChanged;
		//#endif
	}
#else
	
#pragma warning disable
	internal  IEnumerator videoRequest;
	internal void
	VideoRequest()
	{
	}
#pragma warning restore
	internal void
	SubcripePlayModeStateChange()
	{
	
		//  SubcripeSceneLoader_method = method;
		//#if !USE2017
#pragma warning disable
		EditorApplication.playmodeStateChanged -= PlayModeStateChanged;
		EditorApplication.playmodeStateChanged += PlayModeStateChanged;
#pragma warning restore
		//#else
		//			EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
		//			EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
		//EditorApplication.playmodeStateChanged += EditorApplication_playModeStateChanged;
		//#endif
	}
	void
	EditorApplication_playModeStateChanged()
	{	PlayModeChanged = true;
		wasRedrawGoups.Clear();
	
		if ( IMGUI() )
		{	PushActionToUpdate( () =>
			{	PushActionToUpdate( () =>
				{	bottomInterface.NeedRefreshBOttom = true;
	
					if ( hierarchy_windows == null ) return;
	
					foreach ( var hierarchyWindow in hierarchy_windows )
					{	if ( hierarchyWindow == null ) continue;
	
						var tree = m_TreeView(hierarchyWindow as EditorWindow);
	
						if ( tree == null ) continue;
	
						GUIUtility.keyboardControl = (int)m_KeyboardControlIDField.GetValue( tree );
						bottomInterface.BOTTOM_UPDATE_POSITION( (EditorWindow)hierarchyWindow );
	
						((EditorWindow)hierarchyWindow).Repaint();
					}
	
					RepaintWindow( true );
				} );
			} );
		}
	}
#endif
	
	
	internal static void
	ON_ASSET_IMPORT()
	{
#if PROJECT
	
		if ( Initializator.AdaptersByName.Count == 0 ) return;
		
		Initializator.AdaptersByName[Initializator.PROJECT_NAME].need_ClearHierarchyObjects1 = true;
		Initializator.AdaptersByName[Initializator.PROJECT_NAME].ClearTree(true);
#endif
		Adapter.ALL_ASSETS_PATHS = null;
	}
	
	Type ot = typeof(object);
	Type
	SWITCHER_GET_TYPE( long id )
	{	if ( IS_PROJECT() )
		{	try
			{
#if USE2017
				return InternalEditorUtility.GetTypeWithoutLoadingObject( (int)id );
#else
				var asd = InternalEditorUtility.GetObjectFromInstanceID( (int)id );
				
				if ( !asd ) return null;
				
				return asd.GetType();
#endif
			}
			
			catch
			{	return ot;
			}
		}
		
		else
		{	return EditorUtility.InstanceIDToObject( (int)id ).GetType();
		}
	}
	
	
	
	
	
	[NonSerialized] internal static  Dictionary<int, cache_scene> ltg = new Dictionary<int, cache_scene>();
	
	
	internal class cache_scene {
		[NonSerialized] internal Dictionary<long, GameObject> local_to_gameobject = new Dictionary<long, GameObject>();
		[NonSerialized]internal Dictionary<int, long> gameobject_to_local = new Dictionary<int, long>();
	}
	
	
#pragma warning disable
	static Dictionary<UnityEngine.Object, SerializedObject > cached_so = new Dictionary<UnityEngine.Object, SerializedObject>();
	static string fakestring;
#pragma warning restore
	static SerializedProperty localIdProp;
	static SerializedObject serializedObject;
	static PropertyInfo inspectorModeInfo;
	// static FieldInfo m_SerializedObject;
	// static long result;
	
	
	
	static internal long
	m_TryGetGUIDAndLocalFileIdentifier( UnityEngine.Object gameObject )     // #if USE2018
	{	// AssetDatabase.TryGetGUIDAndLocalFileIdentifier( gameObject, out fakestring, out result );
		// return result;
		// #else
		if ( !gameObject ) return 0;
		
		if ( !cached_so.ContainsKey( gameObject ) )
		{	cached_so.Add( gameObject, new SerializedObject( gameObject ) );
		}
		
		//  serializedObject = new SerializedObject( gameObject );
		if ( inspectorModeInfo == null ) inspectorModeInfo = typeof( SerializedObject ).GetProperty( "inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance );
		
		serializedObject = cached_so[gameObject];
		
		
		inspectorModeInfo.SetValue( serializedObject, InspectorMode.Debug, null );
		localIdProp = serializedObject.FindProperty( "m_LocalIdentfierInFile" );   //note the misspelling!
		
		//#todo FIX FOR MODELS, now it retunr a 100000 always for all objets
		if ( localIdProp.longValue == 100000 )
		{
		
			var path  = AssetDatabase.GetAssetPath(gameObject);
			
			if ( !string.IsNullOrEmpty( path ) ) return AssetDatabase.AssetPathToGUID( path ).GetHashCode();
			
			//AssetDatabase.TryGetGUIDAndLocalFileIdentifier( gameObject, out fakestring, out result );
			//return result;
		}
		
		
		/*  var offset = (int)typeof(UnityEngine.Object).GetField("OffsetOfInstanceIDInCPlusPlusObject", (BindingFlags)(-1)).GetValue(null);
		*/
		
		
		/*var gptr = (IntPtr)typeof(UnityEngine.Object).GetField("m_CachedPtr", (BindingFlags)(-1)).GetValue(gameObject);
		var asdasd = gptr.ToInt32();*/
		/*   var gptr = (IntPtr)typeof(SerializedObject).GetField("m_NativeObjectPtr", (BindingFlags)(-1)).GetValue(serializedObject);
		   var asdasd = gptr.ToInt32();
		   Debug.Log(( gameObject) + " " + asdasd + " " +  localIdProp.longValue);*/
		
		
#if USE2017
		
		if ( localIdProp.longValue != 0 ) return localIdProp.longValue;
		
#endif
		
		return localIdProp.intValue;
		// #endif
	}
	
	
	
	
	internal long
	GetFileID( UnityEngine.GameObject go )
	{	if ( pluginID != 0 ) throw new Exception( "GetFileID not implimented" );
		else return go ? Adapter.HierAdapter.GetFileIDWithOutPrefabChecking( Adapter.HierAdapter.GetPrefabInstanceHandle( go ) as GameObject, go ) : 0;
	}
	
	
	[NonSerialized]
	bool?  cacheSaveToScriptableObject;
	internal long
	GetFileIDWithOutPrefabChecking( UnityEngine.GameObject prefab, UnityEngine.GameObject gameObject )
	{	if ( pluginID != 0 ) throw new Exception( "GetFileIDWithOutPrefabChecking not implemented" );
	
		if ( cacheSaveToScriptableObject ?? (cacheSaveToScriptableObject = Hierarchy_GUI.Instance( this ).SaveToScriptableObject == "FOLDER").Value )
		{	if ( Hierarchy_GUI.HierarchySettings.PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances )
				return m_TryGetGUIDAndLocalFileIdentifier( prefab ?? gameObject );
				
			return m_TryGetGUIDAndLocalFileIdentifier( prefab ?? gameObject );
			
		}
		
		else
		{	if ( Hierarchy_GUI.HierarchySettings.PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances && prefab )     //  Debug.Log(prefab.name + " "  + m_TryGetGUIDAndLocalFileIdentifier(prefab ));
			{	return m_TryGetGUIDAndLocalFileIdentifier( prefab );
			}
			
			return gameObject.GetInstanceID();
		}
		
		/*
		if (Hierarchy_GUI.HierarchySettings.PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances) m_TryGetGUIDAndLocalFileIdentifier(prefab ?? gameObject);
		// return m_TryGetGUIDAndLocalFileIdentifier( gameObject);
		return m_TryGetGUIDAndLocalFileIdentifier( gameObject);*/
	}
	
	/*  internal static long GetLocalIdentifierInFile( UnityEngine.GameObject gameObject ) {
	      return GetLocalIdentifierInFile( gameObject , gameObject.scene );
	  }
	  internal static long GetLocalIdentifierInFile( UnityEngine.Component gameObject ) {
	      return GetLocalIdentifierInFile( gameObject , gameObject.gameObject.scene );
	  }
	  internal static long GetLocalIdentifierInFile( UnityEngine.Object gameObject , Scene s ) {
	      return GetLocalIdentifierInFile( gameObject );
	  }*/
	internal static long
	GetLocalIdentifierInFile( UnityEngine.Object gameObject )
	{
	
	
		try
		{
		
			/*  var offset = (int)typeof(UnityEngine.Object).GetField("OffsetOfInstanceIDInCPlusPlusObject", (BindingFlags)(-1)).GetValue(null);
			*/
			
			/*   var gptr = (IntPtr)typeof(UnityEngine.Object).GetField("m_CachedPtr", (BindingFlags)(-1)).GetValue(gameObject);
			   return gptr.ToInt32();*/
			
			// if (!CHECK_UNITYEDITOR_SERIALIZATION_TYPE()) return 0;
			
			
			if ( !m_GET_FILEID_FOR_PFEFAB_CACHE.TryGetValue( gameObject.GetInstanceID(), out templong ) )
			{
			
				if ( Hierarchy_GUI.HierarchySettings.PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances )
				{	var prfp = HierAdapter.GetPrefabInstanceHandle(gameObject);
				
					if ( !prfp ) templong = m_TryGetGUIDAndLocalFileIdentifier( gameObject );
					else
					{	templong = m_TryGetGUIDAndLocalFileIdentifier( prfp );
					}
					
					// GLOBAL PREFAB SOLUTION
				}
				
				else
				{	templong = m_TryGetGUIDAndLocalFileIdentifier( gameObject );
				}
				
				m_GET_FILEID_FOR_PFEFAB_CACHE.Add( gameObject.GetInstanceID(), templong );
				
			}
			
			return templong;
			
			
			//   return UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(o);
			/*
			AssetDatabase.TryGetGUIDAndLocalFileIdentifier( gameObject, out fakestring, out result );
			return result;*/
			
			/* repeat:
			 localIdProp.longValue =  localIdProp.intValue = UnityEngine.Random.Range(1, int.MaxValue);
			 if (ltg.ContainsKey(s.GetHashCode()))
			 {   var cc =  ltg[s.GetHashCode()];
			     if (cc.local_to_gameobject.ContainsKey(localIdProp.longValue)) goto repeat;
			     var old = cc.gameobject_to_local.ContainsKey(gameObject.GetInstanceID()) ? cc.gameobject_to_local[gameObject.GetInstanceID()] : -1;
			     cc.gameobject_to_local.Remove(gameObject.GetInstanceID());
			     cc.local_to_gameobject.Remove(old);
			 }
			 serializedObject.ApplyModifiedProperties();
			 EditorSceneManager.MarkSceneDirty(s);
			
			
			 return localIdProp.intValue;*/
			/*#endif*/
			
			
			
			
			/* if (m_SerializedObject == null)
			{
			  m_SerializedObject = typeof( SerializedProperty ).GetField( "m_SerializedObject", BindingFlags.NonPublic | BindingFlags.Instance );
			}
			m_SerializedObject.SetValue( localIdProp, serializedObject );
			*/
			
		}
		
		catch ( Exception ex )
		{	Debug.Log( "GET OB " + ex.Message + "\n\n" + ex.StackTrace );
			return -1;
		}
	}
	
	
	
	
#pragma warning disable
	
	class IFP {
		public List<long> Childrens;
		public long GameObjectFileID;
		public long RootOrder;
		public long Father;
		public bool IsPrefab;
	}
	
	class PrefabFileID {
		public Dictionary<long, GameObject> FILEID_TO_PREFAB = new Dictionary<long, GameObject>();
		public Dictionary<GameObject, long> PREFAB_TO_FILEID = new Dictionary<GameObject, long>();
	}
	
	static  Dictionary<int, PrefabFileID> IFP_CACHE = new Dictionary<int, PrefabFileID>();
	static     Dictionary<long, IFP> IFP_PTR = new Dictionary<long, IFP>();
	static       SortedDictionary<long, IFP> root = new SortedDictionary<long, IFP>();
	static       Dictionary<long, string> go_names = new Dictionary<long, string>();
	
	internal static void
	m_INITIALIZE_FILEID_FORPREFABS( Scene s )
	{
	
	
		// GLOBAL PREFAB SOLUTION
		
		return;
		
		// GLOBAL PREFAB SOLUTION
		
		
		
		if ( !IFP_CACHE.ContainsKey( s.GetHashCode() ) ) IFP_CACHE.Add( s.GetHashCode(), new PrefabFileID() );
		
		var pid = IFP_CACHE[s.GetHashCode()];
		pid.FILEID_TO_PREFAB.Clear();
		pid.PREFAB_TO_FILEID.Clear();
		root.Clear();
		IFP_PTR.Clear();
		go_names.Clear();
		//  return;
		string line = "_";
		long lastID = -1;
		
		try
		{	using ( var file = File.OpenText( Adapter.UNITY_SYSTEM_PATH + s.path ) )     // file.read
			{	// int mID = "--- !u!4 &".Length;
				int m_ = "  - {fileID: ".Length;
				int m_Name = "  m_Name: ".Length;
				int m_GameObject = "  m_GameObject: {fileID: ".Length;
				int m_Father = "  m_Father: {fileID: ".Length;
				int m_RootOrder = "  m_RootOrder: ".Length;
				int value = "      value:  ".Length;
				int m_target = "    - target: {fileID: ".Length;
				
				while ( !file.EndOfStream )
				{	line = file.ReadLine();
				
				
					if ( line.StartsWith( "--- !u!1" ) ) // GameObject:
					{
					
						if ( line.StartsWith( "---" ) ) if ( !line.EndsWith( "stripped" ) )
								lastID = long.Parse( (line.Substring( line.IndexOf( ' ', 4 ) + 1 ).Trim( '&' )) );
							else
								lastID = long.Parse( (line.Remove( line.LastIndexOf( ' ' ) ).Substring( line.IndexOf( ' ', 4 ) + 1 ).Trim( '&' )) );
								
						while ( !(line = file.ReadLine()).StartsWith( "---" ) || !file.EndOfStream )
						{	if ( line.StartsWith( "  m_Name" ) ) go_names.Add( lastID, line.Substring( m_Name ) );
						}
					}
					
					else
						if ( line.StartsWith( "--- !u!4" ) && !line.EndsWith( "stripped" ) )   //Transform:
						{	bool children = false;
						
						
							if ( line.StartsWith( "---" ) && !line.EndsWith( "stripped" ) )
								lastID = long.Parse( (line.Substring( line.IndexOf( ' ', 4 ) + 1 ).Trim( '&' )) );
								
							long ptr = -1;
							long father = -1;
							long order = -1;
							List<long> childrens = new List<long>();
							
							while ( !(line = file.ReadLine()).StartsWith( "---" ) || !file.EndOfStream )
							{	if ( children )
								{	if ( line.StartsWith( "  -" ) )
									{	childrens.Add( long.Parse( (line.Remove( line.Length - 1 ).Substring( m_ )) ) );
									}
									
									else
									{	children = false;
									}
								}
								
								if ( line.StartsWith( "  m_GameObject" ) ) ptr = long.Parse( (line.Remove( line.Length - 1 ).Substring( m_GameObject )) );
								else
									if ( line.StartsWith( "  m_Father" ) ) father = long.Parse( (line.Remove( line.Length - 1 ).Substring( m_Father )) );
									else
										if ( line.StartsWith( "  m_RootOrder" ) ) order = long.Parse( line.Substring( m_RootOrder ) );
										else
											if ( line.StartsWith( "  m_Children" ) ) children = true;
							}
							
							
							if ( ptr == -1 || father == -1 || order == -1 )
								continue;
								
							//throw new Exception("Error read '" + s.name + "' '" + lastID + "' " + ptr + " " + father + " " + order);
							var result = new IFP()
							{	Childrens = childrens,
								    GameObjectFileID = ptr,
								    RootOrder = order,
								    Father = father,
							};
							IFP_PTR.Add( lastID, result );
							
							if ( father == 0 ) root.Add( order, result );
							
						}
						
						else
							if ( line.StartsWith( "--- !u!1001" ) && !line.EndsWith( "stripped" ) )   //PrefabInstance:
							{
							
								if ( line.StartsWith( "---" ) && !line.EndsWith( "stripped" ) )
									lastID = long.Parse( (line.Substring( line.IndexOf( ' ', 4 ) + 1 ).Trim( '&' )) );
									
								long ptr = -1;
								long father = 0;
								long order = 0;
								List<long> childrens = new List<long>();
								long lastTarget = 0;
								long? goID = null, transformID = null;
								
								while ( !(line = file.ReadLine()).StartsWith( "---" ) || !file.EndOfStream )
								{
								
									m_Modifications:
									
									if ( line.StartsWith( "      propertyPath: m_Name" ) )
									{	if ( !goID.HasValue ) goID = lastTarget;
									
										if ( goID == lastID )
											go_names.Add( lastID, file.ReadLine().Substring( value ) );
									}
									
									else
										if ( line.StartsWith( "    - target: {fileID: " ) ) { lastTarget = long.Parse( line.Remove( line.IndexOf( ',', m_target ) ).Substring( m_target ) ); }
										
										else
											if ( line.StartsWith( "      propertyPath: m_RootOrder" ) )
											{	if ( transformID == lastID ) order = long.Parse( file.ReadLine().Substring( value ) );
											}
											
											else
												if ( line.StartsWith( "      propertyPath: m_LocalPosition" ) ) { if ( !transformID.HasValue ) transformID = lastTarget; }
												
												else
													if ( line.StartsWith( "      propertyPath: m_Name" ) )
													{	if ( transformID == lastID )
														{	father = long.Parse( file.ReadLine().Substring( value ) );
														}
													}
								}
								
								if ( ptr == -1 || father == -1 || order == -1 )
									continue;
									
								//throw new Exception("Error read '" + s.name + "' '" + lastID + "' " + ptr + " " + father + " " + order);
								var result = new IFP()
								{	Childrens = childrens,
									    GameObjectFileID = ptr,
									    RootOrder = order,
									    Father = father,
									    IsPrefab = true
								};
								IFP_PTR.Add( lastID, result );
								
								if ( father == 0 ) root.Add( order, result );
							}
							
							
				}
			}
		}
		
		catch ( Exception ex )
		{	Debug.LogError( line + "\n" + "\n" + ex.Message + "\n" + "\n" + ex.StackTrace );
		}
		
		var allObjects = GET_FILEID_FOR_PFEFAB_AllSceneObjectsAll( s );
		
		List<IFP> sceneData = new List<IFP>();
		
		foreach ( var g in root ) WriteTSceneDataAll( ref sceneData, g.Value );
		
		Debug.Log( go_names[sceneData[0].GameObjectFileID] + " " + allObjects[0] );
		Debug.Log( go_names[sceneData[1].GameObjectFileID] + " " + allObjects[1] );
		Debug.Log( go_names[sceneData[2].GameObjectFileID] + " " + allObjects[2] );
		
		if ( sceneData.Count != allObjects.Length )
		{	throw new Exception( "sceneData and allObjects are not equals " + sceneData.Count + " " + allObjects.Length );
		}
		
		//  int
		for ( int i = 0, l = allObjects.Length ; i < l ; i++ )
		{	pid.FILEID_TO_PREFAB.Add( sceneData[i].GameObjectFileID, allObjects[i] );
			pid.PREFAB_TO_FILEID.Add( allObjects[i], sceneData[i].GameObjectFileID );
		}
	}
	
	static void
	WriteTSceneDataAll( ref List<IFP> list, IFP t )
	{	list.Add( t );
	
		for ( int i = 0, len = t.Childrens.Count ; i < len ; i++ ) WriteTSceneDataAll( ref list, IFP_PTR[t.Childrens[i]] );
	}
	
	
	static List<GameObject> GET_FILEID_FOR_PFEFAB_WriteTAll_List = new List<GameObject>();
	static void
	GET_FILEID_FOR_PFEFAB_WriteTAll( Transform t )
	{
#if USE2018_3
	
		if ( UnityEditor.PrefabUtility.IsPartOfAnyPrefab( t.gameObject ) ) return;
		
#else
		
		if ( UnityEditor.PrefabUtility.FindPrefabRoot( t.gameObject ) ) return;
		
#endif
		GET_FILEID_FOR_PFEFAB_WriteTAll_List.Add( t.gameObject );
		
		for ( int i = 0, len = t.childCount ; i < len ; i++ ) GET_FILEID_FOR_PFEFAB_WriteTAll( t.GetChild( i ) );
	}
	internal static GameObject[]
	GET_FILEID_FOR_PFEFAB_AllSceneObjectsAll( Scene scene )
	{	GET_FILEID_FOR_PFEFAB_WriteTAll_List.Clear();
	
		foreach ( var g in scene.GetRootGameObjects() ) GET_FILEID_FOR_PFEFAB_WriteTAll( g.transform );
		
		return GET_FILEID_FOR_PFEFAB_WriteTAll_List.ToArray();
	}
	
	static long templong;
	static Dictionary<int, long> m_GET_FILEID_FOR_PFEFAB_CACHE = new Dictionary<int, long>();
	static long
	m_GET_FILEID_FOR_PFEFAB( UnityEngine.Object gameObject, Scene s )
	{
	
		throw new Exception( "m_GET_FILEID_FOR_PFEFAB not implimented" );
		
		// GLOBAL PREFAB SOLUTION
		if ( Hierarchy_GUI.HierarchySettings.PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances )
		{
		
		
			if ( !m_GET_FILEID_FOR_PFEFAB_CACHE.TryGetValue( gameObject.GetInstanceID(), out templong ) )
			{	var prfp = HierAdapter.GetPrefabInstanceHandle(gameObject);
			
				if ( !prfp ) templong = 0;
				else templong = m_TryGetGUIDAndLocalFileIdentifier( prfp );
				
				m_GET_FILEID_FOR_PFEFAB_CACHE.Add( gameObject.GetInstanceID(), templong );
			}
			
			return templong;
			
			// GLOBAL PREFAB SOLUTION
			
		}
		
		
		var go = gameObject as GameObject;
		
		//  Debug.Log(go);
		if ( !go ) return 0;
		
		if ( !IFP_CACHE.ContainsKey( s.GetHashCode() ) ) m_INITIALIZE_FILEID_FORPREFABS( s );
		
		var pid = IFP_CACHE[s.GetHashCode()];
		
		if ( !pid.PREFAB_TO_FILEID.ContainsKey( go ) )
		{	Debug.LogWarning( "Cannot find FileID for prefab " + gameObject.name + ", please save scene and try again" );
			return 0;
		}
		
		return pid.PREFAB_TO_FILEID[go];
		
		
		
		
		/*   PrefabInstance:
		m_ObjectHideFlags: 0
		serializedVersion: 2
		m_Modification:
		m_TransformParent: {fileID: 0}
		m_Modifications:
		- target: {fileID: 3132327336879175704, guid: 7e70960e117cb1047b83574c0d97a4d0,
		 type: 3}
		propertyPath: m_Name
		value: Directional Light (1)
		objectReference: {fileID: 0}
		- target: {fileID: 3132327336879175710, guid: 7e70960e117cb1047b83574c0d97a4d0,
		 type: 3}
		
		propertyPath: m_RootOrder
		value: 3
		objectReference: {fileID: 0}
		
		m_RemovedComponents: []
		m_SourcePrefab: {fileID: 100100000, guid: 7e70960e117cb1047b83574c0d97a4d0, type: 3}
		--- !u!1 &518544643
		
		         518544643
		         518544643
		
		         756665547
		
		         534063903
		
		         1192387440
		
		PrefabInstance:
		m_ObjectHideFlags: 0
		serializedVersion: 2
		m_Modification:
		m_TransformParent: {fileID: 0}
		m_Modifications:
		- target: {fileID: 3132327336879175704, guid: 7e70960e117cb1047b83574c0d97a4d0,
		 type: 3}
		propertyPath: m_Name
		value: Directional Light (2)
		objectReference: {fileID: 0}
		- target: {fileID: 3132327336879175710, guid: 7e70960e117cb1047b83574c0d97a4d0,
		 type: 3}
		
		propertyPath: m_RootOrder
		value: 4
		objectReference: {fileID: 0}
		
		m_RemovedComponents: []
		m_SourcePrefab: {fileID: 100100000, guid: 7e70960e117cb1047b83574c0d97a4d0, type: 3}
		--- !u!1 &756665547 */
		
		
		/*  #if USE2018_3
		  AssetDatabase.TryGetGUIDAndLocalFileIdentifier( gameObject, out fakestring, out result );
		  return result;
		#elif USE2018
		  int res;
		  AssetDatabase.TryGetGUIDAndLocalFileIdentifier( gameObject, out fakestring, out res );
		  result = res;
		  return result;
		#else*/
		
	}
#pragma warning restore
	
	
	
	
	
	
	
	
	
	
	void
	INIT_CustomMenuModules()
	{
#if HIERARCHY
	
		if ( pluginID == Initializator.HIERARCHY_ID )
		{	EditorSceneManager.sceneSaved -= OnSceneSaved;
			EditorSceneManager.sceneSaved += OnSceneSaved;
			customMenuModules = new Adapter.CustomMenuAndModule(
			    typeof( HierarchyExtensions.IGenericMenu ), typeof( MyInternalItem ), typeof( EModules.EModulesInternal.Hierarchy.M_UserModulesRoot ), this );
		}
		
		// Adapter.CustomMenuAndModules<HierarchyExtensions.IGenericMenu , MyInternalItem , EModules.EModulesInternal.Hierarchy.M_UserModulesRoot>.InitExtensionMenu( this );
#endif
#if PROJECT
		
		if ( pluginID == Initializator.PROJECT_ID )
			customMenuModules = new Adapter.CustomMenuAndModule(
			    typeof( ProjectExtensions.IGenericMenu ), typeof( MyInternalProjectItem ), typeof( EModules.EProjectInternal.Project.M_UserModulesRoot ), this );
			    
#endif
			    
	}
	
	
	
	
	
	internal Type
	GetUserModuleType()
	{
#if HIERARCHY
	
		if ( pluginID == Initializator.HIERARCHY_ID ) return typeof( EModules.EModulesInternal.Hierarchy.M_UserModulesRoot );
		
#endif
#if PROJECT
		
		if ( pluginname == Initializator.PROJECT_NAME ) return typeof( EModules.EProjectInternal.Project.M_UserModulesRoot );
		
#endif
		return null;
	}
	
	
	
	
	
	
	
	
	
	
	internal void
	SubcripeSceneLoader( Action method )
	{	SubcripeSceneLoader_method -= method;
		SubcripeSceneLoader_method += method;
#if !USE2017
		EditorSceneManager.sceneLoaded -= EditorSceneManagerOnSceneUnloaded;
		EditorSceneManager.sceneLoaded += EditorSceneManagerOnSceneUnloaded;
#else
		EditorSceneManager.sceneOpening -= EditorSceneManagerOnSceneOpening;
		EditorSceneManager.sceneOpening += EditorSceneManagerOnSceneOpening;
#endif
	}
	
	
	internal long
	GetMemorySize( UnityEngine.Object o )
	{
#if !USE2017
#pragma warning disable
		return Profiler.GetRuntimeMemorySize( o ) / 2;
#pragma warning restore
#else
		return Profiler.GetRuntimeMemorySizeLong( o ) / 2;
#endif
	}
	
	internal static void
	FixLoadImage( ref Texture2D tex, byte[] bytes )
	{
#if !USE2017
		tex.LoadImage( bytes );
#else
		ImageConversion.LoadImage( tex, bytes );
#endif
	}
	
	internal void
	GetTriangle( Mesh m, ref List<int> triangles, int submesh )
	{
	
#if !USE2017
		triangles = m.GetTriangles( submesh ).ToList();
#else
		m.GetTriangles( triangles, submesh );
		// ImageConversion.LoadImage(tex, bytes);
#endif
	}
}
}


































#if HIERARCHY
#if !LITE
namespace HierarchyExtensions {

public static class Styles {
	public static GUIStyle button;
	public static GUIStyle label;
}
public abstract class CustomModule_Slot1 : EModules.EModulesInternal.CustomModule { }
public abstract class CustomModule_Slot2 : EModules.EModulesInternal.CustomModule { }
public abstract class CustomModule_Slot3 : EModules.EModulesInternal.CustomModule { }
public abstract class CustomModule_Slot4 : EModules.EModulesInternal.CustomModule { }
public abstract class CustomModule_Slot5 : EModules.EModulesInternal.CustomModule { }
public abstract class CustomModule_Slot6 : EModules.EModulesInternal.CustomModule { }
public abstract class CustomModule_Slot7 : EModules.EModulesInternal.CustomModule { }
public abstract class CustomModule_Slot8 : EModules.EModulesInternal.CustomModule { }
public abstract class CustomModule_Slot9 : EModules.EModulesInternal.CustomModule { }

}
#else
namespace HierarchyExtensions {
public abstract class CustomModule_Slot1 : FakeModule { }
public abstract class CustomModule_Slot2 : FakeModule { }
public abstract class CustomModule_Slot3 : FakeModule { }
public abstract class CustomModule_Slot4 : FakeModule { }
public abstract class CustomModule_Slot5 : FakeModule { }
public abstract class CustomModule_Slot6 : FakeModule { }
public abstract class CustomModule_Slot7 : FakeModule { }
public abstract class CustomModule_Slot8 : FakeModule { }
public abstract class CustomModule_Slot9 : FakeModule { }
public abstract class FakeModule {
	public abstract string NameOfModule { get; }
	public abstract void Draw(Rect drawRect, GameObject o);
	public abstract string ToString(GameObject o);
	public static void
	SHOW_IntInput(int defaultValue, Action<int> OnValueChanged) { }
	public static void
	SHOW_StringInput(string defaultValue, Action<string> OnValueChanged) { }
	public static void
	SHOW_DropDownMenu(int defaultIndex, string[] Items, Action<int> OnIndexChanged, Action<string> OnItemAdded = null) { }
}
}

#endif


namespace EModules.EModulesInternal {

public abstract class CustomModule {
	public abstract string NameOfModule { get; }
	public abstract void Draw( Rect drawRect, GameObject o, bool mouseHover);
	public abstract string ToString( GameObject o );
	
	/// <summary>Opens window with input field.
	/// </summary>
	/// <param name="defaultValue">value that will be in the input field.</param>
	/// <param name="OnValueChanged">will be invoked when the value is changed.</param>
	public static void
	SHOW_IntInput( string windowName, int defaultValue, Action<int> OnValueChanged )
	{	m_OpenIntInput_W( windowName, defaultValue, OnValueChanged );
	}
	/// <summary>Opens window with input field.
	/// </summary>
	/// <param name="defaultValue">value that will be in the input field.</param>
	/// <param name="OnValueChanged">will be invoked when the value is changed.</param>
	public static void
	SHOW_IntInput( int defaultValue, Action<int> OnValueChanged )
	{	m_OpenIntInput( defaultValue, OnValueChanged );
	}
	
	/// <summary>Opens window with input field.
	/// </summary>
	/// <param name="defaultValue">value that will be in the input field.</param>
	/// <param name="OnValueChanged">will be invoked when the value is changed.</param>
	public static void
	SHOW_StringInput( string windowName, string defaultValue, Action<string> OnValueChanged )
	{	m_OpenStringInput_W( windowName, defaultValue, OnValueChanged );
	}
	/// <summary>Opens window with input field.
	/// </summary>
	/// <param name="defaultValue">value that will be in the input field.</param>
	/// <param name="OnValueChanged">will be invoked when the value is changed.</param>
	public static void
	SHOW_StringInput( string defaultValue, Action<string> OnValueChanged )
	{	m_OpenStringInput( defaultValue, OnValueChanged );
	}
	
	/// <summary>Opens the drop-down menu with the specified parameters.
	/// </summary>
	/// <param name="defaultIndex">designated index of item.</param>
	/// <param name="Items">names of menu items.</param>
	/// <param name="OnIndexChanged">will be invoked when the value is changed.</param>
	/// <param name="OnItemAdded">will be invoked when the value is added.
	/// if OnItemAdded == null then 'add new' menu item won't available.</param>
	public static void
	SHOW_DropDownMenu( int defaultIndex, string[] Items, Action<int> OnIndexChanged, Action<string> OnItemAdded = null )
	{	m_OpenDropDownMenu( defaultIndex, Items, OnIndexChanged, OnItemAdded );
	}
	internal static Action<string, int, Action<int>> m_OpenIntInput_W;
	internal static Action<int, Action<int>> m_OpenIntInput;
	internal static Action<string, string, Action<string>> m_OpenStringInput_W;
	internal static Action<string, Action<string>> m_OpenStringInput;
	internal static Action<int, string[], Action<int>, Action<string>> m_OpenDropDownMenu;
	// bool CreateDefaultUndo();
}


class M_UserModulesRoot_Slot1 : Hierarchy.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot1( int restWidth, int sibbildPos, bool enable, Adapter dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
}
class M_UserModulesRoot_Slot2 : Hierarchy.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot2( int restWidth, int sibbildPos, bool enable, Adapter dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
}
class M_UserModulesRoot_Slot3 : Hierarchy.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot3( int restWidth, int sibbildPos, bool enable, Adapter dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
}
class M_UserModulesRoot_Slot4 : Hierarchy.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot4( int restWidth, int sibbildPos, bool enable, Adapter dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
}
class M_UserModulesRoot_Slot5 : Hierarchy.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot5( int restWidth, int sibbildPos, bool enable, Adapter dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
}
class M_UserModulesRoot_Slot6 : Hierarchy.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot6( int restWidth, int sibbildPos, bool enable, Adapter dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
}

class M_UserModulesRoot_Slot7 : Hierarchy.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot7( int restWidth, int sibbildPos, bool enable, Adapter dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
}
class M_UserModulesRoot_Slot8 : Hierarchy.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot8( int restWidth, int sibbildPos, bool enable, Adapter dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
}
class M_UserModulesRoot_Slot9 : Hierarchy.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot9( int restWidth, int sibbildPos, bool enable, Adapter dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
}
}

#endif






#if PROJECT
#if !LITE
namespace ProjectExtensions {
public abstract class CustomModule_Slot1 : EModules.EProjectInternal.CustomModule { }
public abstract class CustomModule_Slot2 : EModules.EProjectInternal.CustomModule { }
public abstract class CustomModule_Slot3 : EModules.EProjectInternal.CustomModule { }
public abstract class CustomModule_Slot4 : EModules.EProjectInternal.CustomModule { }
public abstract class CustomModule_Slot5 : EModules.EProjectInternal.CustomModule { }
public abstract class CustomModule_Slot6 : EModules.EProjectInternal.CustomModule { }
public abstract class CustomModule_Slot7 : EModules.EProjectInternal.CustomModule { }
public abstract class CustomModule_Slot8 : EModules.EProjectInternal.CustomModule { }
public abstract class CustomModule_Slot9 : EModules.EProjectInternal.CustomModule { }
}
#else
namespace ProjectExtensions {
public abstract class CustomModule_Slot1 : FakeModule { }
public abstract class CustomModule_Slot2 : FakeModule { }
public abstract class CustomModule_Slot3 : FakeModule { }
public abstract class CustomModule_Slot4 : FakeModule { }
public abstract class CustomModule_Slot5 : FakeModule { }
public abstract class CustomModule_Slot6 : FakeModule { }
public abstract class CustomModule_Slot7 : FakeModule { }
public abstract class CustomModule_Slot8 : FakeModule { }
public abstract class CustomModule_Slot9 : FakeModule { }

public abstract class FakeModule {
	public abstract string NameOfModule { get; }
	public abstract void Draw(Rect drawRect, string assetPath, string assetGuid, int instanceId, bool isFolder, bool isMainAsset);
	public abstract string ToString(string assetPath, string assetGuid, int instanceId, bool isFolder, bool isMainAsset);
	public static void
	SHOW_IntInput(int defaultValue, Action<int> OnValueChanged) { }
	public static void
	SHOW_StringInput(string defaultValue, Action<string> OnValueChanged) { }
	public static void
	SHOW_DropDownMenu(int defaultIndex, string[] Items, Action<int> OnIndexChanged, Action<string> OnItemAdded = null) { }
}
}

#endif


namespace EModules.EProjectInternal {

public abstract class CustomModule {
	public abstract string NameOfModule { get; }
	public abstract void Draw( Rect drawRect, string assetPath, string assetGuid, int instanceId, bool isFolder, bool isMainAsset );
	public abstract string ToString( string assetPath, string assetGuid, int instanceId, bool isFolder, bool isMainAsset );
	
	/// <summary>Opens window with input field.
	/// </summary>
	/// <param name="defaultValue">value that will be in the input field.</param>
	/// <param name="OnValueChanged">will be invoked when the value is changed.</param>
	public static void
	SHOW_IntInput( int defaultValue, Action<int> OnValueChanged )
	{	m_OpenIntInput( defaultValue, OnValueChanged );
	
	}
	
	/// <summary>Opens window with input field.
	/// </summary>
	/// <param name="defaultValue">value that will be in the input field.</param>
	/// <param name="OnValueChanged">will be invoked when the value is changed.</param>
	public static void
	SHOW_StringInput( string defaultValue, Action<string> OnValueChanged )
	{	m_OpenStringInput( defaultValue, OnValueChanged );
	
	}
	
	/// <summary>Opens the drop-down menu with the specified parameters.
	/// </summary>
	/// <param name="defaultIndex">designated index of item.</param>
	/// <param name="Items">names of menu items.</param>
	/// <param name="OnIndexChanged">will be invoked when the value is changed.</param>
	/// <param name="OnItemAdded">will be invoked when the value is added.
	/// if OnItemAdded == null then 'add new' menu item won't available.</param>
	public static void
	SHOW_DropDownMenu( int defaultIndex, string[] Items, Action<int> OnIndexChanged, Action<string> OnItemAdded = null )
	{	m_OpenDropDownMenu( defaultIndex, Items, OnIndexChanged, OnItemAdded );
	
	}
	
	//#if UNITY_EDITOR
	internal static Action<int, Action<int>> m_OpenIntInput;
	internal static Action<string, Action<string>> m_OpenStringInput;
	internal static Action<int, string[], Action<int>, Action<string>> m_OpenDropDownMenu;
	//#else
	/*       internal static Action<int , Action<int> , EModules.Adapter> m_OpenIntInput;
	      internal static Action<string , Action<string> , EModules.Adapter> m_OpenStringInput;
	      internal static Action<int , string[] , Action<int> , Action<string> , EModules.Adapter> m_OpenDropDownMenu;*/
	//#endif
	// bool CreateDefaultUndo();
}


class M_UserModulesRoot_Slot1 : Project.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot1( int restWidth, int sibbildPos, bool enable, Adapter adatper ) : base( restWidth, sibbildPos, enable, adatper ) { }
}
class M_UserModulesRoot_Slot2 : Project.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot2( int restWidth, int sibbildPos, bool enable, Adapter adatper ) : base( restWidth, sibbildPos, enable, adatper ) { }
}
class M_UserModulesRoot_Slot3 : Project.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot3( int restWidth, int sibbildPos, bool enable, Adapter adatper ) : base( restWidth, sibbildPos, enable, adatper ) { }
}
class M_UserModulesRoot_Slot4 : Project.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot4( int restWidth, int sibbildPos, bool enable, Adapter adatper ) : base( restWidth, sibbildPos, enable, adatper ) { }
}
class M_UserModulesRoot_Slot5 : Project.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot5( int restWidth, int sibbildPos, bool enable, Adapter adatper ) : base( restWidth, sibbildPos, enable, adatper ) { }
}
class M_UserModulesRoot_Slot6 : Project.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot6( int restWidth, int sibbildPos, bool enable, Adapter adatper ) : base( restWidth, sibbildPos, enable, adatper ) { }
}
class M_UserModulesRoot_Slot7 : Project.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot7( int restWidth, int sibbildPos, bool enable, Adapter adatper ) : base( restWidth, sibbildPos, enable, adatper ) { }
}
class M_UserModulesRoot_Slot8 : Project.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot8( int restWidth, int sibbildPos, bool enable, Adapter adatper ) : base( restWidth, sibbildPos, enable, adatper ) { }
}
class M_UserModulesRoot_Slot9 : Project.M_UserModulesRoot {
	public
	M_UserModulesRoot_Slot9( int restWidth, int sibbildPos, bool enable, Adapter adatper ) : base( restWidth, sibbildPos, enable, adatper ) { }
}
}

#endif













namespace EModules.EModulesInternal



{

partial class Adapter {

	const string PREF_HIER = "Preferences/Hierarchy PRO";
	//  const string PROJ_PREF = "/Project Settings";
	const string PROJ_PREF = "/☰ Project ORG";
	// const string PREF_PREFIX = "/Hierarchy";
	const string PREF_PREFIX = "/";
	
	public static  void
	__ShowSection( int pluginID, string sectionName )
	{
	
#if USE2018_3
		var asm = Assembly.GetAssembly(typeof(EditorWindow));
		var prefType = asm.GetType("UnityEditor.SettingsWindow");
		
		if ( prefType == null )
		{	Debug.LogWarning( $"UnityEditor.SettingsWindow not found" );
			return;
		}
		
		var m = prefType.GetMethod( "Show", (BindingFlags)(-1) );
		
		if ( m == null )
		{	Debug.LogWarning( $"UnityEditor.SettingsWindow.Show not found" );
			return;
		}
		
		m.Invoke( null, new[] { SettingsScope.User, (object)sectionName } );
#else
		
		if (pluginID == 0)
		{	_S___Hierarchy_GUI2.showWindow();
		}
		
		else
		{	_S___Project_GUI2.showWindow();
		}
		
#endif
	}
	
	internal static void
	SHOW_HIER_SETTINGS_DEFAULT(int pluginID )
	{	if ( pluginID == 0 ) __ShowSection( pluginID, PREF_HIER + "" );
	
#if UNITY_EDITOR
		else __ShowSection( pluginID, PREF_HIER + PROJ_PREF + "" );
		
#endif
	}
	
	internal void
	SHOW_HIER_SETTINGS_GENERICMENU()
	{	if ( pluginID == 0 ) __ShowSection( pluginID, PREF_HIER + "" );  //__ShowSection( pluginID, PREF_HIER + PREF_PREFIX + "Right Bar" );
	
#if UNITY_EDITOR
		else __ShowSection( pluginID, PREF_HIER + PROJ_PREF + "" );
		
#endif
	}
	internal void
	SHOW_HIER_SETTINGS_PLAYMODE_KEEPER()
	{	if ( pluginID == 0 ) __ShowSection( pluginID, PREF_HIER + PREF_PREFIX + "Right Bar/PlayMode Data Keeper" );
	
#if UNITY_EDITOR
		// else __ShowSection( pluginID, PREF_HIER + PROJ_PREF + "" );
#endif
	}
}
}







#if USE2018_3





namespace EModules.EModulesInternal



{

partial class Adapter {











	// [SettingsProvider] static SettingsProvider MyNewPrefCode() { return CreateMenuItem( PREF_HIER + "" , -1 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode0() { return CreateMenuItemHier( PREF_HIER + "", 0 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode1() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "HighLighter", 1 ); }
	// [SettingsProvider] static SettingsProvider MyNewPrefCode2() { return CreateMenuItem( PREF_HIER + "/HighLighter/☰ Auto Filters" , 2 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode3() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Bottom Bar", 3 ); }
	//[SettingsProvider] static SettingsProvider MyNewPrefCode4() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Bottom Bar/Quick Help", 4 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode5() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Search Window", 5 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode6() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Custom Click Menu", 6 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode7() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Cache", 7 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode8() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Right Bar", 8 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode9() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Right Bar/Display of Functions and Vars", 9 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode10() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Right Bar/PlayMode Data Keeper", 10 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode11() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Right Bar/Memory Optimizer", 11 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCode12() { return CreateMenuItemHier( PREF_HIER + PREF_PREFIX + "Right Bar/Custom Modules", 12 ); }
	//         [SettingsProvider] static SettingsProvider MyNewPrefCode1() { return CreateMenuItem( PREF_HIER + "/Search Window" , 1 ); }
	//         [SettingsProvider] static SettingsProvider MyNewPrefCode2() { return CreateMenuItem( PREF_HIER + "/Custom Click Menu" , 2 ); }
	//         [SettingsProvider] static SettingsProvider MyNewPrefCode3() { return CreateMenuItem( PREF_HIER + "/Colors and Icons" , 3 ); }
	//         [SettingsProvider] static SettingsProvider MyNewPrefCode4() { return CreateMenuItem( PREF_HIER + "/Auto Colors List" , 4 ); }
	//         [SettingsProvider] static SettingsProvider MyNewPrefCode5() { return CreateMenuItem( PREF_HIER + "/Bottom Bar" , 5 ); }
	//         [SettingsProvider] static SettingsProvider MyNewPrefCode6() { return CreateMenuItem( PREF_HIER + "/HiperGraph" , 6 ); }
	//         [SettingsProvider] static SettingsProvider MyNewPrefCode7() { return CreateMenuItem( PREF_HIER + "/Right Bar" , 7 ); }
	//         [SettingsProvider] static SettingsProvider MyNewPrefCode8() { return CreateMenuItem( PREF_HIER + "/Right Modules" , 8 ); }
	//         [SettingsProvider] static SettingsProvider MyNewPrefCode9() { return CreateMenuItem( PREF_HIER + "/Cache" , 9 ); }
	
	
	
	
#if UNITY_EDITOR
	
	
	[SettingsProvider] static SettingsProvider MyNewPrefCHierProj0() { return CreateMenuItemProj( PREF_HIER + PROJ_PREF + "", 0 ); }
#if !EMX_PROJECT_DISABLE
	[SettingsProvider] static SettingsProvider MyNewPrefCHierProj1() { return CreateMenuItemProj( PREF_HIER + PROJ_PREF + "/HighLighter", 1 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCHierProj3() { return CreateMenuItemProj( PREF_HIER + PROJ_PREF + "/Bottom Bar", 3 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCHierProj5() { return CreateMenuItemProj( PREF_HIER + PROJ_PREF + "/Search Window", 5 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCHierProj6() { return CreateMenuItemProj( PREF_HIER + PROJ_PREF + "/Custom Click Menu", 6 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCHierProj7() { return CreateMenuItemProj( PREF_HIER + PROJ_PREF + "/Cache", 7 ); }
	[SettingsProvider] static SettingsProvider MyNewPrefCHierProj8() { return CreateMenuItemProj( PREF_HIER + PROJ_PREF + "/Right Bar", 8 ); }
#endif
#endif
	
	
	
	
	private class MyPrefSettingsProvider : SettingsProvider {
		public
		MyPrefSettingsProvider( string path, SettingsScope scopes = SettingsScope.User ) : base( path, scopes ) { }
		public Action<string> onGuiAction;
		public override void
		OnGUI( string searchContext )
		{	onGuiAction( searchContext );
		}
	}
	
	
	static SettingsProvider
	CreateMenuItemProj( string menu, int ind )
	{	var p = new MyPrefSettingsProvider( menu, SettingsScope.User );
		p.onGuiAction = ( search ) => ProjectSettingGUI( search, ind );
		string res = "";
		
		try
		{	res = string.Concat( ProjectSettingGUI( null, ind, false ) );
		}
		
		catch ( Exception ex )
		{	Debug.Log( ex.Message + "\n" + ex.StackTrace );
		}
		
		p.keywords = new[] { res };
		return p;
	}
	static SettingsProvider
	CreateMenuItemHier( string menu, int ind )
	{	var p = new MyPrefSettingsProvider( menu, SettingsScope.User );
		/* p.titleBarGuiHandler = () => {
		     GUILayout.Button( "ASD" );
		 };*/
		// p.keywords = SettingsProvider.GetSearchKeywordsFromGUIContentProperties<Styles>();
		// var kw = new HashSet<string>();
		//                 foreach ( var item in HierarchySettingGUI( null , ind , false ).SelectMany( w => w.Split( ' ' ) ).Where( content => !string.IsNullOrEmpty( content ) )
		//                              .Select( content => content.ToLowerInvariant() )
		//                              .Distinct() ) {
		//                     kw.Add( item );
		//                 }
		/* res = HierarchySettingGUI( null , ind , false ).SelectMany( w => w.Split( ' ' ) ).Where( content => !string.IsNullOrEmpty( content ) )
		                              .Select( content => content.ToLowerInvariant() )
		                              .Distinct().Aggregate( ( a , b ) => a + " " + b );*/
		
		p.onGuiAction = ( search ) => HierarchySettingGUI( search, ind );
		string res = "";
		
		try
		{	res = string.Concat( HierarchySettingGUI( null, ind, false ) );
		}
		
		catch ( Exception ex )
		{	Debug.Log( ex.Message + "\n" + ex.StackTrace );
		}
		
		p.keywords = new[] { res };
		return p;
	}
}
}



#else

namespace EModules.EModulesInternal

{
partial class Adapter {

	/* internal const int Paa = 2000;
	
	 [MenuItem( "Window/Hierarchy Plugin/Settings", false, Paa + 3 )]
	 static void NewSetWinHier()
	 {   _S___Hierarchy_GUI2.showWindow();
	 }
	
	//#if UNITY_EDITOR
	 [MenuItem( "Window/Project Plugin/Settings", false, Paa + 3 )]
	 static void NewSetWinProj()
	 {   _S___Project_GUI2.showWindow();
	 }
	
	//#endif
	*/
	
	
}
}

#endif




































#if LITE && HIERARCHY




/////////////////////////////////////////////////////MENU ITEM TEMPLATE///////////////////////////////////////////////////////////////////////////////
/*

    class MyMenu : HierarchyExtensions.IGenericMenu
    {
        public string Name { get { return "MySubItem/MyMenuItem %k"; } }
        public int PositionInMenu { get { return 0; } }

        public bool IsEnable(GameObject clickedObject) { return true; }
        public bool NeedExcludeFromMenu(GameObject clickedObject) { return false; } // or 'return clickedObject.GetComponent<MyComponent>() == null'

        public void OnClick(GameObject[] affectedObjectsArray)
        {
            throw new System.NotImplementedException();
        }
    }

*/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


namespace EModules.EModulesInternal {



#region ITEM 100-101 - Group/UnGroup

class MyMenu_Group : MyInternalItem, HierarchyExtensions.IGenericMenu {
	public bool
	IsEnable(GameObject clickedObject) { return true; }
	public bool
	NeedExcludeFromMenu(GameObject clickedObject) { return false; }
	
	public int PositionInMenu { get { return 100; } }
	public string Name { get { return "Group %g"; } }
	
	
	public void
	OnClick(GameObject[] affectedObjectsArray)
	{	var onlytop = MyMenu_Utils.GetOnlyTopObjects(affectedObjectsArray).OrderBy(go => go.transform.GetSiblingIndex()).ToArray();
	
		if (onlytop.Length == 0) return;
		
		var groupParent = onlytop[0].transform.parent;
		var groupSiblingIndex = onlytop[0].transform.GetSiblingIndex();
		
		var groupRoot = new GameObject("GROUP " + onlytop[0].name);
		groupRoot.transform.SetParent( groupParent, false );
		groupRoot.transform.localScale = Vector3.one;
		groupRoot.transform.SetSiblingIndex( groupSiblingIndex );
		
		MyMenu_Utils.AssignUniqueName( groupRoot ); // name
		
		if (groupRoot.GetComponentsInParent<Canvas>( true ).Length != 0)
		{	// canvas
			var rect = groupRoot.AddComponent<RectTransform>();
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
		}
		
		Undo.RegisterCreatedObjectUndo( groupRoot, groupRoot.name );
		
		foreach (var gameObject in onlytop)
		{	Undo.SetTransformParent( gameObject.transform, groupRoot.transform, groupRoot.name );
		}
		
		HierarchyExtensions.Utilities.SetExpanded( groupRoot.GetInstanceID(), true );
		
		Selection.objects = onlytop.ToArray();
		//Selection.objects = new[] { groubObject };
	}
	
}


class MyMenu_UnGroup : MyInternalItem, HierarchyExtensions.IGenericMenu {
	public bool
	IsEnable(GameObject clickedObject) { return clickedObject.transform.childCount != 0; }
	public bool
	NeedExcludeFromMenu(GameObject clickedObject) { return false; }
	
	public int PositionInMenu { get { return 101; } }
	public string Name { get { return "UnGroup %#g"; } }
	
	
	public void
	OnClick(GameObject[] affectedObjectsArray)
	{	var ungroupedObjects = new List<GameObject>();
		var onlytop = MyMenu_Utils.GetOnlyTopObjects(affectedObjectsArray);
		
		foreach (var ungroupedRoot in onlytop)
		{	var ungroupSiblinkIndex = ungroupedRoot.transform.GetSiblingIndex();
			var ungroupParent = ungroupedRoot.transform.parent;
			var undoName = ungroupedRoot.name;
			
			for (int i = ungroupedRoot.transform.childCount - 1 ; i >= 0 ; i--)
			{	var o = ungroupedRoot.transform.GetChild(i);
				Undo.SetTransformParent( o.transform, ungroupParent, "Remove " + undoName );
				
				if (!Application.isPlaying) Undo.RegisterFullObjectHierarchyUndo( o, "Remove " + undoName );
				
				o.SetSiblingIndex( ungroupSiblinkIndex );
				
				if (!Application.isPlaying) EditorUtility.SetDirty( o );
				
				ungroupedObjects.Add( o.gameObject );
			}
			
			Undo.DestroyObjectImmediate( ungroupedRoot );
		}
		
		Selection.objects = ungroupedObjects.ToArray();
	}
	
}

#endregion



#region ITEM 500 - DuplicateNextToObject

class MyMenu_DuplicateNextToObject : MyInternalItem, HierarchyExtensions.IGenericMenu {
	public bool
	IsEnable(GameObject clickedObject) { return true; }
	public bool
	NeedExcludeFromMenu(GameObject clickedObject) { return false; }
	
	public int PositionInMenu { get { return 500; } }
	public string Name { get { return "Duplicate Next To Object %#d"; } }
	
	
	public void
	OnClick(GameObject[] affectedObjectsArray)
	{
	
		var onlytop = MyMenu_Utils.GetOnlyTopObjects(affectedObjectsArray).OrderByDescending(o => o.transform.GetSiblingIndex());
		
		List<GameObject> clonedObjects = new List<GameObject>();
		
		foreach (var gameObject in onlytop)
		{	var oldSib = gameObject.transform.GetSiblingIndex();
			Selection.objects = new[] { gameObject };
			HierarchyExtensions.Utilities.DuplicateSelection();
			var clonedObject = Selection.activeGameObject;
			MyMenu_Utils.AssignUniqueName( clonedObject );
			clonedObject.transform.SetSiblingIndex( oldSib + 1 );
			clonedObjects.Add( clonedObject );
		}
		
		Selection.objects = clonedObjects.ToArray();
		
	}
}

#endregion



#region ITEM 1000-1001 - ExpandSelecdedObject/CollapseSelecdedObject

class MyMenu_ExpandSelecdedObject : MyInternalItem, HierarchyExtensions.IGenericMenu {
	public bool
	IsEnable(GameObject clickedObject) { return clickedObject.transform.childCount != 0; }
	public bool
	NeedExcludeFromMenu(GameObject clickedObject) { return false; }
	
	public int PositionInMenu { get { return 1000; } }
	public string Name { get { return "Expand Selection"; } }
	
	
	public void
	OnClick(GameObject[] affectedObjectsArray)
	{	foreach (var result in affectedObjectsArray.Select( o => o.GetInstanceID() ))
			HierarchyExtensions.Utilities.SetExpandedRecursive( result, true );
	}
	
}


class MyMenu_CollapseSelecdedObject : MyInternalItem, HierarchyExtensions.IGenericMenu {
	public bool
	IsEnable(GameObject clickedObject) { return clickedObject.transform.childCount != 0; }
	public bool
	NeedExcludeFromMenu(GameObject clickedObject) { return false; }
	
	public int PositionInMenu { get { return 1001; } }
	public string Name { get { return "Collapse Selection"; } }
	
	
	public void
	OnClick(GameObject[] affectedObjectsArray)
	{	foreach (var result in affectedObjectsArray.Select( o => o.GetInstanceID() ))
			HierarchyExtensions.Utilities.SetExpandedRecursive( result, false );
	}
	
}

#endregion



#region ITEM 2000-2001 - ReverseChildrenOrder/SelectOnlyTopObjects/SelectAllChildren

class MyMenu_ReverseChildrenOrder : MyInternalItem, HierarchyExtensions.IGenericMenu {
	public bool
	IsEnable(GameObject clickedObject) { return clickedObject.transform.childCount > 0; }
	public bool
	NeedExcludeFromMenu(GameObject clickedObject) { return false; }
	
	public int PositionInMenu { get { return 2000; } }
	public string Name { get { return "Reverse Children Order"; } }
	
	
	public void
	OnClick(GameObject[] affectedObjectsArray)
	{	foreach (var gameObject in MyMenu_Utils.GetOnlyTopObjects( affectedObjectsArray ))
		{	var T = gameObject.transform;
		
			for (int i = 0 ; i < gameObject.transform.childCount ; i++)
			{	Undo.SetTransformParent( T.GetChild( i ), T.GetChild( i ).transform.parent, "Reverse Children Order" );
				T.GetChild( i ).SetAsFirstSibling();
			}
		}
	}
	
}

class MyMenu_SelectOnlyTopObjects : MyInternalItem, HierarchyExtensions.IGenericMenu {
	public bool
	IsEnable(GameObject clickedObject) { return Selection.gameObjects.Length >= 2; }
	public bool
	NeedExcludeFromMenu(GameObject clickedObject) { return false; }
	
	public int PositionInMenu { get { return 2001; } }
	public string Name { get { return "Select Only Top Objects"; } }
	
	
	public void
	OnClick(GameObject[] affectedObjectsArray)
	{	Selection.objects = MyMenu_Utils.GetOnlyTopObjects( affectedObjectsArray );
	}
	
}


class MyMenu_SelectAllChildren : MyInternalItem, HierarchyExtensions.IGenericMenu {
	public bool
	IsEnable(GameObject clickedObject) { return clickedObject.transform.childCount != 0; }
	public bool
	NeedExcludeFromMenu(GameObject clickedObject) { return false; }
	
	public int PositionInMenu { get { return 2002; } }
	public string Name { get { return "Select All Children"; } }
	
	
	public void
	OnClick(GameObject[] affectedObjectsArray)
	{	Selection.objects = affectedObjectsArray.SelectMany( s => s.GetComponentsInChildren<Transform>( true ) ).Select( s => s.gameObject ).ToArray();
	}
	
}

#endregion






#region - Utils

static class MyMenu_Utils {
	public static void
	AssignUniqueName(GameObject o)
	{
	
		var usedNames = new SortedDictionary<string, string>();
		var childList = o.transform.parent
		                ? new Transform[o.transform.parent.childCount].Select((t, i) => o.transform.parent.GetChild(i))
		                : o.scene.GetRootGameObjects().Select(go => go.transform);
		                
		foreach (var child in childList.Where( child => child != o.transform ))
		{	if (!usedNames.ContainsKey( child.name )) usedNames.Add( child.name, child.name );
		}// existing names
		
		if (!usedNames.ContainsKey( o.name )) return;
		
		
		
		var number = 1;
		var name = o.name;
		
		var leftBracket = name.IndexOf('(');
		var rightBracket = name.IndexOf(')');
		
		if (leftBracket != -1 && rightBracket != -1 && rightBracket - leftBracket > 1)
		{	int parseResult;
		
			if (int.TryParse( name.Substring( leftBracket + 1, rightBracket - leftBracket - 1 ), out parseResult ))
			{	number = parseResult + 1;
				name = name.Remove( leftBracket );
			}
		}// previous value
		
		
		
		name = name.TrimEnd();
		
		while (usedNames.ContainsKey( name + " (" + number + ")" )) ++number;
		
		o.name = name + " (" + number + ")"; //result
		
	}
	
	public static GameObject[]
	GetOnlyTopObjects(GameObject[] affectedObjectsArray)
	{	return affectedObjectsArray.
		       Where( g => g.GetComponentsInParent<Transform>( true ).Where( p => p != g.transform ).Count( p => affectedObjectsArray.Contains( p.gameObject ) ) == 0 ).
		       Select( g => g.gameObject ).ToArray();
	}
}

#endregion




}//namespace

#endif