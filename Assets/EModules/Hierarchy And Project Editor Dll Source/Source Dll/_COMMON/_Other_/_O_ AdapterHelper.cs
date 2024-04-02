
#if UNITY_EDITOR
	#define HIERARCHY
	#define PROJECT
#endif

//#define GETFIELDS_CHECKERRORS

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
	static GUISkin __skin;
	
	internal static GUISkin GET_SKIN()
	{	if ( ReferenceEquals( __skin, null ) ) __skin = GUI.skin;
	
		return __skin;
	}
	
	
	
	internal void ResetStyles()
	{	__labelStyle = null;
		__STYLE_LASTSEL_BUTTON = null;
		__STYLE_HIERSEL_BUTTON = null;
		__STYLE_HIERSEL_PLUS = null;
		__STYLE_InternalBoxStyle = null;
		__STYLE_DEFBUTTON_middle = null;
		__STYLE_DEFBUTTON = null;
		__STYLE_HYPERGRAPH_DEFBUTTON = null;
		__STYLE_LABEL_8 = null;
		__STYLE_LABEL_8_WINDOWS = null;
		__STYLE_LABEL_8_middle = null;
		__STYLE_LABEL_8_WINDOWS_middle = null;
		__STYLE_LABEL_8_right = null;
		__STYLE_LABEL_8_WINDOWS_right = null;
		__STYLE_LABEL_10_middle = null;
		__STYLE_LABEL_10 = null;
		__STYLE_LABEL_10_COLORED = null;
		__STYLE_DEFBOX = null;
		m_InspectorTitlebar = null;
	}
	
	
	
	internal   GUIStyle __labelStyle ;
	internal GUIStyle labelStyle
	{	get
		{	if ( __labelStyle == null ) __labelStyle = new GUIStyle( Adapter.GET_SKIN().label );
		
			if ( __labelStyle.alignment != TextAnchor.MiddleLeft)   __labelStyle.alignment = TextAnchor.MiddleLeft;
			
			if (pluginID == 0) __labelStyle.fontSize = HIERARCHY_FONT_SIZE;
			else
				if (HierAdapter.EDITOR_FONT_AFFECTOTHERWINDOWS) __labelStyle.fontSize = HIERARCHY_FONT_SIZE;
				else __labelStyle.fontSize = lastFontSize;
				
			return __labelStyle;
		}
	}
	internal  GUIStyle __STYLE_LASTSEL_BUTTON;
	internal GUIStyle STYLE_LASTSEL_BUTTON
	{	get
		{	if ( __STYLE_LASTSEL_BUTTON == null )
			{	__STYLE_LASTSEL_BUTTON = new GUIStyle( STYLE_DEFBUTTON );
				__STYLE_LASTSEL_BUTTON.normal.textColor = new Color32( 20, 20, 20, 255 );
				//                     __STYLE_LASTSEL_BUTTON.padding.top = 5;
				//                     __STYLE_LASTSEL_BUTTON.padding.left = 3;
				//                     __STYLE_LASTSEL_BUTTON.padding.right = 1;
				//                     __STYLE_LASTSEL_BUTTON.padding.bottom = 3;
				__STYLE_LASTSEL_BUTTON.alignment = TextAnchor.MiddleCenter;
				// __STYLE_LASTSEL_BUTTON.fontSize = Mathf.RoundToInt( Adapter.GET_SKIN().button.fontSize / 1.5f );
			}
			
			__STYLE_LASTSEL_BUTTON.fontSize = STYLE_LABEL_8.fontSize - 2;
			return __STYLE_LASTSEL_BUTTON;
		}
	}
	internal  GUIStyle __STYLE_HIERSEL_BUTTON;
	internal GUIStyle STYLE_HIERSEL_BUTTON
	{	get
		{	if ( __STYLE_HIERSEL_BUTTON == null )
			{	__STYLE_HIERSEL_BUTTON = new GUIStyle( STYLE_DEFBUTTON );
				__STYLE_HIERSEL_BUTTON.normal.textColor = new Color32( 20, 20, 20, 255 );
				//                     __STYLE_HIERSEL_BUTTON.padding.top = 5;
				//                     __STYLE_HIERSEL_BUTTON.padding.left = 1;
				//                     __STYLE_HIERSEL_BUTTON.padding.right = 1;
				__STYLE_HIERSEL_BUTTON.alignment = TextAnchor.MiddleCenter;
				__STYLE_HIERSEL_BUTTON.fontStyle = FontStyle.Bold;
			}
			
			return __STYLE_HIERSEL_BUTTON;
		}
	}
	
	
	internal  GUIStyle __STYLE_HIERSEL_PLUS;
	internal GUIStyle STYLE_HIERSEL_PLUS
	{	get
		{	if ( __STYLE_HIERSEL_PLUS == null )
			{	__STYLE_HIERSEL_PLUS = new GUIStyle( STYLE_DEFBUTTON );
				__STYLE_HIERSEL_PLUS.normal.textColor = new Color32( 20, 20, 20, 255 );
				__STYLE_HIERSEL_PLUS.alignment = TextAnchor.MiddleCenter;
				// __STYLE_HIERSEL_PLUS.padding.top = 5;
			}
			
			return __STYLE_HIERSEL_PLUS;
		}
	}
	
	
	internal static GUIStyle __STYLE_InternalBoxStyle;
	internal static GUIStyle STYLE_InternalBoxStyle
	{	get
		{	if ( __STYLE_InternalBoxStyle == null )
			{	__STYLE_InternalBoxStyle = new GUIStyle( Adapter.GET_SKIN().textArea );
				__STYLE_InternalBoxStyle.alignment = TextAnchor.MiddleCenter;
			}
			
			return __STYLE_InternalBoxStyle;
		}
	}
	internal  GUIStyle __STYLE_DEFBUTTON_middle;
	internal GUIStyle STYLE_DEFBUTTON_middle
	{	get
		{	if ( __STYLE_DEFBUTTON_middle == null )
			{	__STYLE_DEFBUTTON_middle = new GUIStyle( STYLE_DEFBUTTON );
				__STYLE_DEFBUTTON_middle.alignment = TextAnchor.MiddleCenter;
			}
			
			return __STYLE_DEFBUTTON_middle;
		}
	}
	internal  GUIStyle __STYLE_DEFBUTTON_right;
	internal GUIStyle STYLE_DEFBUTTON_right
	{	get
		{	if ( __STYLE_DEFBUTTON_right == null )
			{	__STYLE_DEFBUTTON_right = new GUIStyle( STYLE_DEFBUTTON );
				__STYLE_DEFBUTTON_right.alignment = TextAnchor.MiddleRight;
			}
			
			return __STYLE_DEFBUTTON_right;
		}
	}
	internal  GUIStyle __STYLE_DEFBUTTON;
	internal GUIStyle STYLE_DEFBUTTON
	{	get
		{	if ( __STYLE_DEFBUTTON == null )
			{	__STYLE_DEFBUTTON = new GUIStyle( Adapter.GET_SKIN().button );
				__STYLE_DEFBUTTON.normal.background = Texture2D.blackTexture;
				__STYLE_DEFBUTTON.hover.background = Texture2D.blackTexture;
				__STYLE_DEFBUTTON.focused.background = GetIcon( "BUTBLUE" );
				__STYLE_DEFBUTTON.active.background = GetIcon( "BUTBLUE" );
				__STYLE_DEFBUTTON.normal.scaledBackgrounds = new[] { Texture2D.blackTexture };
				__STYLE_DEFBUTTON.hover.scaledBackgrounds = new[] { Texture2D.blackTexture };
				__STYLE_DEFBUTTON.focused.scaledBackgrounds = new[] { Texture2D.blackTexture };
				__STYLE_DEFBUTTON.active.scaledBackgrounds = new[] { Texture2D.blackTexture };
				__STYLE_DEFBUTTON.border.top = EditorGUIUtility.isProSkin ? -2 : 0;
				__STYLE_DEFBUTTON.border.right = 0;
				__STYLE_DEFBUTTON.border.bottom = 0;
				__STYLE_DEFBUTTON.border.left = 0;
				__STYLE_DEFBUTTON.clipping = TextClipping.Overflow;
				__STYLE_DEFBUTTON.alignment = TextAnchor.MiddleLeft;
				
			}
			
			__STYLE_DEFBUTTON.fontSize = FONT_8();
			return __STYLE_DEFBUTTON;
		}
	}
	internal  GUIStyle __STYLE_HYPERGRAPH_DEFBUTTON;
	internal GUIStyle STYLE_HYPERGRAPH_DEFBUTTON
	{	get
		{	if ( __STYLE_HYPERGRAPH_DEFBUTTON == null )
			{	__STYLE_HYPERGRAPH_DEFBUTTON = new GUIStyle( STYLE_DEFBUTTON );
				__STYLE_HYPERGRAPH_DEFBUTTON.normal.textColor = Color.black;
				__STYLE_HYPERGRAPH_DEFBUTTON.padding.top = 3;
				__STYLE_HYPERGRAPH_DEFBUTTON.padding.left = 3;
				__STYLE_HYPERGRAPH_DEFBUTTON.padding.right = 2;
				__STYLE_HYPERGRAPH_DEFBUTTON.padding.bottom = 3;
			}
			
			__STYLE_HYPERGRAPH_DEFBUTTON.fontSize = Mathf.RoundToInt( STYLE_DEFBUTTON.fontSize / 1.5f );
			return __STYLE_HYPERGRAPH_DEFBUTTON;
		}
	}
	
	internal  GUIStyle __STYLE_LABEL_8;
	internal GUIStyle STYLE_LABEL_8
	{	get
		{	if ( __STYLE_LABEL_8 == null )
			{	__STYLE_LABEL_8 = new GUIStyle( Adapter.GET_SKIN().label );
				__STYLE_LABEL_8.fontSize = FONT_8();
				__STYLE_LABEL_8.alignment = TextAnchor.MiddleLeft;
			}
			
			return __STYLE_LABEL_8;
		}
	}
	internal  GUIStyle __STYLE_LABEL_8_WINDOWS;
	internal GUIStyle STYLE_LABEL_8_WINDOWS
	{	get
		{	if ( __STYLE_LABEL_8_WINDOWS == null )
			{	__STYLE_LABEL_8_WINDOWS = new GUIStyle( Adapter.GET_SKIN().label );
				__STYLE_LABEL_8_WINDOWS.fontSize = WINDOW_FONT_8();
				__STYLE_LABEL_8_WINDOWS.alignment = TextAnchor.MiddleLeft;
			}
			
			return __STYLE_LABEL_8_WINDOWS;
		}
	}
	//
	internal  GUIStyle __STYLE_LABEL_8_middle;
	internal GUIStyle STYLE_LABEL_8_middle
	{	get
		{	if ( __STYLE_LABEL_8_middle == null )
			{	__STYLE_LABEL_8_middle = new GUIStyle( Adapter.GET_SKIN().label );
				__STYLE_LABEL_8_middle.fontSize = FONT_8();
				__STYLE_LABEL_8_middle.alignment = TextAnchor.MiddleCenter;
			}
			
			return __STYLE_LABEL_8_middle;
		}
	}
	internal  GUIStyle __STYLE_LABEL_8_WINDOWS_middle;
	internal GUIStyle STYLE_LABEL_8_WINDOWS_middle
	{	get
		{	if ( __STYLE_LABEL_8_WINDOWS_middle == null )
			{	__STYLE_LABEL_8_WINDOWS_middle = new GUIStyle( Adapter.GET_SKIN().label );
				__STYLE_LABEL_8_WINDOWS_middle.fontSize = WINDOW_FONT_8();
				__STYLE_LABEL_8_WINDOWS_middle.alignment = TextAnchor.MiddleCenter;
			}
			
			return __STYLE_LABEL_8_WINDOWS_middle;
		}
	}
	//
	internal  GUIStyle __STYLE_LABEL_8_right;
	internal GUIStyle STYLE_LABEL_8_right
	{	get
		{	if ( __STYLE_LABEL_8_right == null )
			{	__STYLE_LABEL_8_right = new GUIStyle( Adapter.GET_SKIN().label );
				__STYLE_LABEL_8_right.fontSize = FONT_8();
				__STYLE_LABEL_8_right.alignment = TextAnchor.MiddleRight;
			}
			
			return __STYLE_LABEL_8_right;
		}
	}
	internal  GUIStyle __STYLE_LABEL_8_WINDOWS_right;
	internal GUIStyle STYLE_LABEL_8_WINDOWS_right
	{	get
		{	if ( __STYLE_LABEL_8_WINDOWS_right == null )
			{	__STYLE_LABEL_8_WINDOWS_right = new GUIStyle( Adapter.GET_SKIN().label );
				__STYLE_LABEL_8_WINDOWS_right.fontSize = WINDOW_FONT_8();
				__STYLE_LABEL_8_WINDOWS_right.alignment = TextAnchor.MiddleRight;
			}
			
			return __STYLE_LABEL_8_WINDOWS_right;
		}
	}
	internal  GUIStyle __STYLE_LABEL_10_middle;
	internal GUIStyle STYLE_LABEL_10_middle
	{	get
		{	if ( __STYLE_LABEL_10_middle == null )
			{	__STYLE_LABEL_10_middle = new GUIStyle( Adapter.GET_SKIN().label );
				__STYLE_LABEL_10_middle.alignment = TextAnchor.MiddleCenter;
				__STYLE_LABEL_10_middle.fontSize = FONT_10();
			}
			
			return __STYLE_LABEL_10_middle;
		}
	}
	
	internal  GUIStyle __STYLE_LABEL_10;
	internal GUIStyle STYLE_LABEL_10
	{	get
		{	if ( __STYLE_LABEL_10 == null )
			{	__STYLE_LABEL_10 = new GUIStyle( Adapter.GET_SKIN().label );
				__STYLE_LABEL_10.alignment = TextAnchor.MiddleLeft;
				__STYLE_LABEL_10.fontSize = FONT_10();
			}
			
			return __STYLE_LABEL_10;
		}
	}
	//
	internal  GUIStyle __STYLE_LABEL_10_COLORED;
	internal GUIStyle STYLE_LABEL_10_COLORED
	{	get
		{	if ( __STYLE_LABEL_10_COLORED == null )
			{	__STYLE_LABEL_10_COLORED = new GUIStyle( Adapter.GET_SKIN().label );
				__STYLE_LABEL_10_COLORED.alignment = TextAnchor.MiddleLeft;
				__STYLE_LABEL_10_COLORED.normal.textColor = _S_TextColor;
				__STYLE_LABEL_10_COLORED.fontSize = FONT_10();
			}
			
			return __STYLE_LABEL_10_COLORED;
		}
	}
	internal static GUIStyle __STYLE_DEFBOX;
	internal static GUIStyle STYLE_DEFBOX
	{	get
		{	if ( __STYLE_DEFBOX == null )
			{	/* __STYLE_DEFBOX = new GUIStyle( Adapter.GET_SKIN().textArea );
				 __STYLE_DEFBOX.alignment = TextAnchor.MiddleCenter;*/
				
				__STYLE_DEFBOX = GUI.skin.FindStyle( "Tooltip" ) ?? EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).FindStyle( "Tooltip" );
				
				if ( __STYLE_DEFBOX == null )
				{	__STYLE_DEFBOX = GUI.skin.box;
				}
				
				__STYLE_DEFBOX = new GUIStyle( __STYLE_DEFBOX );
				__STYLE_DEFBOX.alignment = TextAnchor.MiddleCenter;
			}
			
			return __STYLE_DEFBOX;
		}
	}
	
	internal static GUIStyle m_InspectorTitlebar;
	internal void HR(Rect R, float HR = 0)
	{	if ( m_InspectorTitlebar == null )
		{	m_InspectorTitlebar = GUI.skin.FindStyle( "m_InspectorTitlebar" ) ?? EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).FindStyle( "m_InspectorTitlebar" );
		
			if ( m_InspectorTitlebar == null )
			{	m_InspectorTitlebar = GUI.skin.box;
			}
		}
		
		R.height = 1;
		
		if ( HR  != 0 )
		{	float HRP = HR;
			HRP -= R.width / 4;
			R.x -= (HRP - 5);
			R.width += (HRP - 5) * 2;
		}
		
		R.y -= 8;
		Adapter.DrawRect( R, new Color32(20, 20, 20, 255) );
		
		// Adapter.LABEL( R , stack[i].text , m_ProgressBarBar );
		//Adapter.LABEL( R, " ", m_InspectorTitlebar );
	}
	
	//   private bool? ___UNITY_5_5;
	// private bool? ___UNITY_2017_1;
	
	private bool UNITY_5_5
	{	get     // if (___UNITY_5_5 == null) ___UNITY_5_5 = Application.unityVersion.StartsWith( "5.5" );
		{	return false;
		}
	}
	private bool UNITY_2017_1
	{	get     //if (___UNITY_2017_1 == null) ___UNITY_2017_1 = Application.unityVersion.StartsWith( "2" );
		{	return true;
		}
	}
	
	private Vector2? ___ContentSize;
	private Vector2 ContentSize
	{	get
		{	if ( ___ContentSize == null && window() != null )
			{	var treeView = m_TreeView(window());
			
				if ( treeView == null )
				{	// Debug.Log( "ASD" );
					return Vector2.zero;
				}
				
				___ContentSize = (Vector2)GetTotalSizeMethodInfo.Invoke( treeView, null );
				//  GetTotalSizeMethodInfo = m_TreeView.PropertyType.GetMethod("GetTotalRect", (BindingFlags)(-1));
				//___ContentSize = new Vector2(((Rect)GetTotalSizeMethodInfo.Invoke(treeView, null)).width, ((Rect)GetTotalSizeMethodInfo.Invoke(treeView, null)).height);
			}
			
			return ___ContentSize ?? Vector2.zero;
		}
	}
	
	
	
	
	internal bool NeedBottomPositionUpdate = false;
	private RectOffset padding = new RectOffset(0, 0, 0, 0);
	private RectOffset border = new RectOffset(0, 0, 0, 0);
	
	
	
	private RectOffset AssignMyPadding( RectOffset reference )
	{	padding.bottom = reference.bottom;
		padding.left = reference.left;
		padding.top = reference.top;
		padding.right = reference.right;
		return padding;
	}
	
	private RectOffset AssignMyBorder( RectOffset reference )
	{	border.bottom = reference.bottom;
		border.left = reference.left;
		border.top = reference.top;
		border.right = reference.right;
		return border;
	}
	
	internal void RESET_SMOOTH_HEIGHT()
	{	bottomInterface.HEIGHT = Mathf.RoundToInt( ENABLE_BOTTOMDOCK_PROPERTY ? bottomInterface.REFERENCE_HEIGHT : 0 );
	}
	
	internal void CHECK_SMOOTH_HEIGHT()     //if ( m_SearchFilterString != null && !string.IsNullOrEmpty( (string)m_SearchFilterString.GetValue( window() ) ) ) return;
	{	if ( IS_SEARCH_MODE_OR_PREFAB_OPENED() ) return;
	
		if ( bottomInterface._HEIGHT == null )
			bottomInterface.HEIGHT = Mathf.RoundToInt( ENABLE_BOTTOMDOCK_PROPERTY ? bottomInterface.REFERENCE_HEIGHT : 0 );
	}
	
	
	
	
	
	
	
	
	
	
	
	internal void SET_UNDO( IHashProperty d, string name )
	{
	
	
		if ( !(d as UnityEngine.Object) ) return;
		
#if HIERARCHY
		
		if ( IS_HIERARCHY() )
		{	if ( d.gameObject )
			{	Undo.RecordObject( d.component, name );
				Undo.RecordObject( d.gameObject, name );
			}
			
			else
				if ( d.unityobject )
				{	Undo.RecordObject( d.unityobject, name );
				}
				
			// this is fix for case if active object was removed
			var h = d.GetHash4();
			
			for ( int i = 0 ; i < h.Count ; i++ )
				if ( !h[i].ActiveGameObject )
				{	h[i].ActiveGameObject = h[i].GET_ActiveGameObject;
				}
				
			var b = d .GetBookMarks();
			
			for ( int _b = 0 ; _b < b.Count ; _b++ )
			{	if ( b[_b].array == null ) continue;
			
				for ( int i = 0 ; i < b[_b].array.Count ; i++ )
				{	if ( !b[_b].array[i].ActiveGameObject )
					{	b[_b].array[i].ActiveGameObject = b[_b].array[i].GET_ActiveGameObject;
					}
				}
			}
			
			// this is fix for case if active object was removed
		}
		
#endif
		
		
#if PROJECT
		
		if ( IS_PROJECT() )
		{	EModules.EProjectInternal.Project.SET_UNDO_FOR_HASHPROPERY( name );
		}
		
#endif
	}
	
	internal void SET_DIRTY_PROJECT()
	{
#if PROJECT
	
		if ( IS_PROJECT() )
		{	EModules.EProjectInternal.Project.SET_DIRTY();
		}
		
#endif
	}
	
	
	
	
	
	
	
	internal HierarchyObject[] FILTER_GAMEOBJECTS( UnityEngine.Object[] o )
	{	if ( pluginID == Initializator.HIERARCHY_ID ) return /* Selection.gameObjects == null ? new GameObject[0] :*/
			    o.Select( g => g as GameObject ).Where( g => g && isSceneObject( g ) ).Select( GetHierarchyObjectByInstanceID ).ToArray();
			    
		return o.Where( ob => ob && !string.IsNullOrEmpty( isProjectObject( ob ) ) ).Select( ob => GetHierarchyObjectByInstanceID( ob.GetInstanceID() ) ).ToArray();
	}
	
	internal HierarchyObject[] SELECTED_GAMEOBJECTS()
	{	return FILTER_GAMEOBJECTS( Selection.objects );
	}
	
	internal HierarchyObject activeGameObject()
	{	if ( pluginID == Initializator.HIERARCHY_ID ) return isSceneObject( Selection.activeGameObject ) ? GetHierarchyObjectByInstanceID( Selection.activeGameObject ) : null;
	
		var g = isProjectObject( Selection.activeObject );
		
		if ( string.IsNullOrEmpty( g ) ) return null;
		
		return GetHierarchyObjectByInstanceID( Selection.activeObject.GetInstanceID() );
		
	}
	
	internal static bool isProjectObjectBool( UnityEngine.Object o )
	{	var gameObject = o as GameObject;
	
		if ( gameObject != null && isSceneObject( gameObject ) ) return false;
		
		return true;
	}
	
	
	internal static string isProjectObject( UnityEngine.Object o )
	{	var gameObject = o as GameObject;
	
		if ( !o || gameObject != null && isSceneObject( gameObject ) ) return null;
		
		return AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( o.GetInstanceID() ) );
	}
	
	internal static bool isSceneObject( GameObject o )
	{	return o && o.scene.IsValid();
		/*var prefab_root = PrefabUtility.FindPrefabRoot(o);
		return string.IsNullOrEmpty(AssetDatabase.GetAssetPath(prefab_root));*/
		
		
		/*   MonoBehaviour.print(AssetDatabase.GetAssetPath(prefab_root));
		   Object parentObject = PrefabUtility.GetPrefabParent(prefab_root);
		   MonoBehaviour.print(parentObject);
		   return parentObject != prefab_root;*/
		/*  if (parentObject == null) return true;
		  string path = AssetDatabase.GetAssetPath(parentObject);
		  MonoBehaviour.print(path);
		  return string.IsNullOrEmpty(path);*/
	}
	
	
	
	
	
	
	internal GUIStyle InitializeStyle( string icon )
	{	return InitializeStyle( icon, 0, 0, 0, 0 );
	}
	internal GUIStyle InitializeStyle( string icon, float border )
	{	return InitializeStyle( icon, border, border, border, border );
	}
	internal GUIStyle InitializeStyle( string icon, float LEFT_B, float RIGHT_B, float TOP_B, float BOTTOM_B, TextClipping clipping = TextClipping.Clip, GUIStyle refStyle = null )
	{	var txt = GetIcon( icon );
		GUIStyle result = null;
#if UNITY_EDITOR
		
		if ( txt == null ) Debug.LogError( "txt == null" );
		
#endif
		
		if ( txt != null )
		{	result = refStyle != null ? new GUIStyle( refStyle ) : new GUIStyle();
		
			result.normal = new GUIStyleState() { background = txt };
			result.focused = new GUIStyleState() { background = txt };
			result.hover = new GUIStyleState() { background = txt };
			result.border = new RectOffset( (int)(txt.width * LEFT_B), (int)(txt.width * RIGHT_B), (int)(txt.height * TOP_B), (int)(txt.height * BOTTOM_B) );
			result.padding = new RectOffset( 3, 3, -1, -1 );
			result.alignment = TextAnchor.MiddleLeft;
			
			result.focused.textColor = result.active.textColor = result.hover.textColor = result.normal.textColor = Color.white;
			result.clipping = clipping;
			//  result.overflow = new RectOffset(5,5,5,5);
			//result.wordWrap = true;
		}
		
		return result;
	}
	internal GUIStyle InitializeStyle( string icon, string iconhover, float LEFT_B, float RIGHT_B, float TOP_B, float BOTTOM_B, TextClipping clipping = TextClipping.Clip, GUIStyle refStyle = null )
	{
	
		var result = InitializeStyle( icon, LEFT_B, RIGHT_B, TOP_B, BOTTOM_B, clipping, refStyle );
		// result.hover.background = result.focused.background = result.active.background = GetIcon(iconhover);
		result.active = new GUIStyleState() { background = GetIcon( iconhover ) };
		return result;
	}
	
	
	
	
	
	
	
	/*
	
	#region GET_TEXTURE
	internal Texture2D GET_TEXTURE(int code, Color color)
	{
	
	
	    var redT = new  Texture2D( 1, 1, TextureFormat.ARGB32, false, !Adapter.USE2018 )
	    {   name = pluginname + "_KEY_#1",
	            hideFlags = HideFlags.DontSave
	    };
	    redT.SetPixel( 0, 0, color );
	    redT.Apply();
	
	    EditorPrefs.SetInt( pluginname + "/Textures/" + code, redT.GetInstanceID() );
	
	    return redT;
	}
	
	internal static Texture2D GET_TEXTURE_GLOBAL(int code, Color color, Adapter adapter)
	{
	
	
	    var redT = new  Texture2D( 1, 1, TextureFormat.ARGB32, false, !Adapter.USE2018 )
	    {   name = adapter.pluginname + "_KEY_#1",
	            hideFlags = HideFlags.DontSave
	    };
	    redT.SetPixel( 0, 0, color );
	    redT.Apply();
	
	    EditorPrefs.SetInt( "Textures/" + code, redT.GetInstanceID() );
	
	    return redT;
	}
	
	internal static Texture2D GET_TEXTURE(string pluginname, int code, Color color, out bool oldTextureFound)
	{   oldTextureFound = false;
	
	
	    var redT = new  Texture2D( 1, 1, TextureFormat.ARGB32, false, !Adapter.USE2018 )
	    {   name = pluginname + "_KEY_#1",
	            hideFlags = HideFlags.DontSave
	    };
	    redT.SetPixel( 0, 0, color );
	    redT.Apply();
	
	    EditorPrefs.SetInt( pluginname + "/Textures/" + code, redT.GetInstanceID() );
	
	    return redT;
	}
	
	internal  Texture2D GET_TEXTURE(int code, Color color, out bool oldTextureFound)
	{   oldTextureFound = false;
	
	
	    var redT = new  Texture2D( 1, 1, TextureFormat.ARGB32, false, !Adapter.USE2018 )
	    {   name = pluginname + "_KEY_#1",
	            hideFlags = HideFlags.DontSave
	    };
	    redT.SetPixel( 0, 0, color );
	    redT.Apply();
	
	    EditorPrefs.SetInt( pluginname + "/Textures/" + code, redT.GetInstanceID() );
	
	    return redT;
	}
	
	
	
	
	#endregion
	
	 internal Texture2D GET_TEXTURE(int code, int width, int height, TextureFormat format, out bool oldTextureFound)
	{   oldTextureFound = false;
	
	
	    var redT = new Texture2D( width, height, format, false, !Adapter.USE2018 )
	    {   name = pluginname + "_KEY_#1",
	            hideFlags = HideFlags.DontSave
	    };
	    redT.Apply();
	
	    // EditorPrefs.SetInt( pluginname + "/Textures/" + code, redT.GetInstanceID() );
	
	    return redT;
	}
	internal void SET_TEXTURE_DIRTY(Texture2D tex, int code)
	{   //EditorPrefs.SetInt( pluginname + "/Textures/" + code , tex.GetInstanceID() );
	}
	
	
	
	*/
	
	
	
	
	
	
	
	
	
	internal static GUIContent CacheDisableConten = new GUIContent( "Cache Disabled!", null, "Cache Disabled!" );
	
	static Assembly[] __ass_raw;
	internal static Assembly[] ass_raw
	{	get
		{	if ( __ass_raw == null )
			{	__ass_raw = System.AppDomain.CurrentDomain.GetAssemblies().Where( a => !a.FullName.EndsWith( "Editor" ) && !a.FullName.StartsWith( "UnityEditor" ) )
				            .ToArray();
			}
			
			return __ass_raw;
		}
	}
	static Dictionary<string, Type> ass;
	internal static Type GET_TYPE_BY_STRING( string str )
	{	if ( string.IsNullOrEmpty( str ) ) return null;
	
		if ( ass == null ) ass =
			    ass_raw.SelectMany( a => a.GetTypes() ).ToLookup( pair => pair.FullName, pair => pair )
			    .ToDictionary( group => group.Key, group => group.First() );
			    
		if ( ass.ContainsKey( str ) ) return ass[str];
		
		return null;
	}
	
	
	// static Type serType2 = typeof(ISerializationCallbackReceiver);
	static Type serType2 = typeof(System.NonSerializedAttribute);
	static Type serType = typeof(SerializeField);
	internal static Type unityObjectType = typeof(UnityEngine.Object);
	private static BindingFlags flags = ~BindingFlags.Static;
	static Dictionary<Type, Dictionary<string, FieldAdapter>> _SCAN_FIELDS_CACHE = new Dictionary<Type, Dictionary<string, FieldAdapter>>();
	static Dictionary<Type, Dictionary<string, FieldAdapter>> _SCAN_FIELDS_CACHE_NOARRAYS = new Dictionary<Type, Dictionary<string, FieldAdapter>>();
	static Dictionary<Type, Type> genericTypeCache = new Dictionary<Type, Type>();
	static Type additionalSkipper = typeof(IEnumerable);
	
	/* interface IName {
	     string Name { get; }
	     object GetValue( object o );
	 }*/
	
	public class FieldAdapter {
		//UnityEngine.Events.UnityEvent
		bool isObject;
		bool isField;
		FieldInfo f;
		PropertyInfo p;
		// public Type ObjectType = null;
		public string Name;
		//public FieldAdapter[] childFields;
		bool isClass;
		public Dictionary<string, FieldAdapter> childFields;
		bool isEnumerable;
		bool isList;
		//bool UnityEventMarker;
		static bool? assignFromArray;
		Dictionary<string, object> fastDic;
		
		public static FieldAdapter TryToCreate( FieldInfo f, bool includeArrays )
		{	/* isField = true;
			 this.f = f;
			 Name = f.Name;*/
			var res = _constructor( f.FieldType,  includeArrays );
			
			if ( ReferenceEquals( res, null ) ) return null;
			
			res.isField = true;
			res.f = f;
			res.Name = f.Name;
			return res;
		}
		public static FieldAdapter TryToCreate( PropertyInfo p, bool includeArrays )
		{	/* isField = false;
			 this.p = p;
			 Name = p.Name;*/
			var res = _constructor( p.PropertyType,  includeArrays );
			
			if ( ReferenceEquals( res, null ) ) return null;
			
			res.isField = false;
			res.p = p;
			res.Name = p.Name;
			return res;
		}
		
		static bool HasUnityEvent( ref Dictionary<string, FieldAdapter> arr )
		{	return arr.Any( c => c.Value.f != null && Adapter.UnityEventArgsType == c.Value.f.FieldType );
		}
		static FieldAdapter _constructor( Type type, bool includeArrays /*, GET_FIELDS_TYPE searchType*/ )
		{
		
			if ( type.IsPrimitive )
			{	return null;
			}
			
			if ( unityObjectType.IsAssignableFrom( type ) )
			{	var res = new FieldAdapter();
				res.isObject = true; // OBJECT
				return res;
				//ObjectType = type;
			}
			
			else
			{
			
				assignFromArray = null;
				
				if ( type.IsSerializable || (assignFromArray ?? (assignFromArray = arrayType.IsAssignableFrom( type )).Value) )
				{	if ( !type.IsGenericType && !(assignFromArray ?? (assignFromArray = arrayType.IsAssignableFrom( type )).Value) )   //CLASS
					{	var childFields = GET_FIELDS( type );
					
						if ( childFields.Count == 0 ) return null;
						
						//  ObjectType = type;
						var res = new FieldAdapter();
						res.isClass = true;
						res.childFields = childFields;
						/* if ( searchType == GET_FIELDS_TYPE.UnityEvent && !res.UnityEventMarker ) {
						     res.UnityEventMarker = HasUnityEvent( ref childFields );
						     if ( res.UnityEventMarker ) Debug.Log( "ASD" );
						 }*/
						return res;
					}
					
					else
					{	if ( !includeArrays )
						{	return null;
						}
						
						else
							if ( assignFromArray ?? (assignFromArray = arrayType.IsAssignableFrom( type )).Value )     //[]
							{	if ( type.GetArrayRank() == 1 )
								{	// isEnumerable = true;
									var elType = type.GetElementType();
									
									if ( unityObjectType.IsAssignableFrom( elType ) )
									{	var res = new FieldAdapter();
										res.isObject = true; // OBJECT
										res.isEnumerable = true; // OBJECT
										return res;
									}
									
									else
										if ( additionalSkipper.IsAssignableFrom( elType ) )
										{	return null;
										}
										
										else
										{	var childFields = GET_FIELDS(/* ObjectType =*/ elType );
										
											if ( childFields.Count == 0 ) return null;
											
											var res = new FieldAdapter();
											res.childFields = childFields;
											res.isEnumerable = true; // OBJECT
											/* if ( searchType == GET_FIELDS_TYPE.UnityEvent && !res.UnityEventMarker ) {
											     res.UnityEventMarker = HasUnityEvent( ref childFields );
											     if ( res.UnityEventMarker ) Debug.Log( "ASD" );
											 }*/
											return res;
										}
										
								}
								
								else
								{	return null;
								}
							}
							
							else
							{	if ( !genericTypeCache.ContainsKey( type ) )
								{	genericTypeCache.Add( type, (typeof( List<> ).MakeGenericType( type.GetGenericArguments().First( g => !g.IsGenericParameter ) )) );
								}
								
								if ( genericTypeCache[type].IsAssignableFrom( type ) )   //LIST
								{	// isEnumerable = true;
									var elType = type.GetGenericArguments().First( g => !g.IsGenericParameter );
									
									if ( unityObjectType.IsAssignableFrom( elType ) )
									{	var res = new FieldAdapter();
										res.isObject = true; // OBJECT
										res.isEnumerable = true; // OBJECT
										res.isList = true;
										return res;
									}
									
									else
										if ( additionalSkipper.IsAssignableFrom( elType ) )
										{	return null;
										}
										
										else
										{	var childFields = GET_FIELDS( /*ObjectType = */elType );
										
											if ( childFields.Count == 0 ) return null;
											
											var res = new FieldAdapter();
											res.childFields = childFields;
											res.isEnumerable = true;
											res.isList = true;
											// OBJECT
											/* if ( searchType == GET_FIELDS_TYPE.UnityEvent && !res.UnityEventMarker ) {
											     res.UnityEventMarker = HasUnityEvent( ref childFields );
											     if ( res.UnityEventMarker ) Debug.Log( "ASD" );
											 }*/
											return res;
										}
								}
								
								else
								{	return null;
								}
							}
					}
				}
				
				else
				{	return null;
				}
				
			}
			
			/* if ()
			 else if ( p.PropertyType.I )*/
		}
		
		
		object res = null;
		
		public Dictionary<int, Dictionary<string, object>> GetAllValuesCache = null;
		int oldSelID = -1;
		internal void CheckID(int sel_id, int SEK_INC)
		{	if (oldSelID == (sel_id ^ SEK_INC)) return;
		
			oldSelID = sel_id ^ SEK_INC;
			
			if (GetAllValuesCache!= null) GetAllValuesCache.Clear();
		}
		
		static Dictionary<string, object> emptyObject = new Dictionary<string, object>();
		// static object[] singleObject = new object[1];
		public Dictionary<string, object> GetAllValues( object o, int deep, int searchType )
		{	if ( deep == 20 )
			{	return emptyObject;
			}
			
			deep++;
			// Debug.Log( o.GetType().FullName + " " + Name );
			res = GetValue( o );
			/*if ( isField ) res = f.GetValue( o );
			else res = p.GetValue( o , null );*/
			// if ( Name == "GAGAGA" ) Debug.Log( "ASD" );
			
			
			if ( Adapter.IsObjectNull( res ) && (!isObject || (searchType & 4) == 0) ) return emptyObject;
			
			if ( isEnumerable )
			{	if ( (searchType & 2) != 0 ) return emptyObject;
			
				var dic = new Dictionary<string, object>();
				int i  = 0;
				
				foreach ( var item in (IEnumerable)res )
				{	if ( !isObject )
					{	foreach ( var a in childFields.Values.SelectMany( c => c.GetAllValues( item, deep, searchType ) ) )
						{	dic.Add( Name + '#' + i + "#/" + a.Key, a.Value );
						}
					}
					
					else
					{	if ( (searchType & 1) == 0 )
							dic.Add( Name + '#' + i + '#', item as UnityEngine.Object );
					}
					
					i++;
				}
				
				if ( isObject && (searchType & 1) != 0 ) dic = dic.Where( d => d.Key.EndsWith( "/m_Target" ) &&
					        d.Key.Contains( '#' ) && d.Key.Remove( d.Key.LastIndexOf( '#', d.Key.LastIndexOf( '#' ) - 1 ) ).EndsWith( "/m_Calls" ) ).ToDictionary( d => d.Key, d => d.Value );
					        
				return dic;
			}
			
			if ( isObject )
			{	if ( deep == 1 && (searchType & 1) != 0 )
				{	return emptyObject;
				}
				
				if ( fastDic == null )
				{	fastDic = new Dictionary<string, object>();
					fastDic.Add( Name, null );
				}
				
				fastDic[Name] = res;
				return fastDic;
			}
			
			if ( isClass )
			{	var dic = childFields.Values.SelectMany( c => c.GetAllValues( res, deep, searchType ) )
				          .ToDictionary( v2 => Name + '/' + v2.Key, v2 => v2.Value );
				          
				/* foreach ( var item in dic ) {
				     Debug.Log( item.Key );
				
				 }*/
				if ( (searchType & 1) != 0 ) dic = dic.Where( d => d.Key.EndsWith( "m_Target" ) &&
					                                   d.Key.Contains( '#' ) && d.Key.Remove( d.Key.LastIndexOf( '#', d.Key.LastIndexOf( '#' ) - 1 ) ).EndsWith( "/m_Calls" ) ).ToDictionary( d => d.Key, d => d.Value );
					                                   
				return dic;
			}
			
			throw new Exception( "Unknown field type" );
		}
		
		
		public struct ArrayKey
		{	public object ArrayObject;
			// public string ArrayFieldName;
			public int ArrayIndex;
			public List<string> ChildFieldsNames;
			
			public FieldKey ToFieldKey()
			{	var res = new FieldKey();
				res.ChildFieldsNames = ChildFieldsNames.ToList();
				res.Value = ArrayObject;
				return res;
			}
		}
		public struct FieldKey
		{	public List<string> ChildFieldsNames;
			public object Value;
			public FieldKey Clone()
			{	var resuslt = this;
				resuslt.ChildFieldsNames = ChildFieldsNames.ToList();
				return resuslt;
			}
			public FieldKey TrimFirst()
			{	ChildFieldsNames.RemoveAt( 0 );
				return this;
			}
		}
		public void SetAllValues( object o, Dictionary<string, object> values )
		{	var converted = values.Select( v => new FieldKey() { ChildFieldsNames = v.Key.Split( '/' ).ToList(), Value = v.Value } ).ToArray();
			SetAllValues( o, converted );
		}
		public object SetAllValues( object o, FieldKey[] converted )
		{
		
			/*if ( Name == "as44dasd" )
			    Debug.Log( converted.Length );*/
			
			if ( converted.Length == 0 ) return o;
			
			/*foreach ( var item in converted ) {
			    Debug.Log( Name + " " + (item.ChildFieldsNames.Count == 0 ? "0" : item.ChildFieldsNames.Aggregate( ( a , b ) => a + "/" + b ) ));
			}*/
			
			if ( !isEnumerable )
			{	if ( isObject )
				{	if ( converted.Length != 1 ) throw new Exception( "SetAllValues isObject Length != 1" );
				
					SetValue( o, converted[0].Value );
					return o;
				}
				
				if ( isClass )
				{	var resClass = GetValue( o );
				
					foreach ( var cf in childFields.Values )
					{	var newConv = converted.Where( c => c.ChildFieldsNames.Count > 1 &&
						                               (c.ChildFieldsNames[1][c.ChildFieldsNames[1].Length - 1] == '#' && c.ChildFieldsNames[1].Remove(c.ChildFieldsNames[1].IndexOf('#')) == cf.Name ||
						                                c.ChildFieldsNames[1] == cf.Name)
						                             ).Select( c => c.Clone().TrimFirst() ).ToArray();
						var _resClass = cf.SetAllValues( resClass, newConv );
						
						/* Debug.Log( "----" );
						 foreach ( var item in newConv ) {
						     Debug.Log( cf.Name + " " + (item.ChildFieldsNames.Count == 0 ? "0" : item.ChildFieldsNames.Aggregate( ( a , b ) => a + "/" + b )) );
						 }*/
#if GETFIELDS_CHECKERRORS
						
						if ( _resClass.GetType() != resClass.GetType() ) throw new Exception( "Type missmatch " + _resClass.GetType().Name + " " + resClass.GetType().Name );
						
#endif
						resClass = _resClass;
					}
					
					/* var dic = childFields.Values.SelectMany( c => c.GetAllValues( res , deep , searchType ) )
					 .ToDictionary( v2 => Name + '/' + v2.Key , v2 => v2.Value );*/
					SetValue( o, resClass );
					return o;
				}
				
				throw new Exception( "Unknown field type" );
			}
			
			var resArray = GetValue( o );
			
			var newConverted = new ArrayKey[converted.Length];
			var dicArrayKeys = new Dictionary<int, List< ArrayKey>>();
			
			for ( int i = 0 ; i < converted.Length ; i++ )
			{	if ( converted[i].ChildFieldsNames.Count == 0 ) continue;
			
				/* var c = converted[i];
				 if (
				         (c.ChildFieldsNames[0][c.ChildFieldsNames[0].Length - 1] == '#' && c.ChildFieldsNames[0].Remove( c.ChildFieldsNames[0].IndexOf( '#' ) ) == Name ||
				         c.ChildFieldsNames[0] == Name) ) {
				
				     if ( c.ChildFieldsNames.Count < 2 ) throw new Exception( "ChildFieldsNames.Count < 2" );*/
				var field = converted[i].ChildFieldsNames[0];
				int ind = -1;
				
				if ( field[field.Length - 1] == '#' )
				{	var R1 = field.IndexOf('#');
					var indS = field .Substring(R1 + 1);
					ind = int.Parse( indS.Remove( indS.Length - 1 ) );
					//field = field.Remove( R1 );
				}
				
				newConverted[i] = new ArrayKey();
				newConverted[i].ArrayObject = converted[i].Value;
				// newConverted[i].ArrayFieldName = field;
				newConverted[i].ArrayIndex = ind;
				newConverted[i].ChildFieldsNames = converted[i].ChildFieldsNames.ToList();
				// newConverted[i].ChildFieldsNames.RemoveAt( 0 );
				
				if ( !dicArrayKeys.ContainsKey( ind ) ) dicArrayKeys.Add( ind, new List<ArrayKey>() );
				
				dicArrayKeys[ind].Add( newConverted[i] );
				// }
				
			}
			
			// Debug.Log( converted[0].ChildFieldsNames[0] );
			
			if ( isList )
			{	var arr =  resArray as IList;
			
				if ( isObject )
				{	for ( int i = 0 ; i < arr.Count ; i++ )
					{	arr[i] = null;
					}
				}
				
				foreach ( var dicKey in dicArrayKeys )
				{	var i = dicKey.Key;
				
					if ( i >= arr.Count ) continue;
					
					var resClass = arr[i];
					
					if ( !isObject )
					{	var sendArgs = dicKey.Value.Select(d => d.ToFieldKey()).ToArray();
					
						foreach ( var cf in childFields.Values )
						{	var _sendArgs = sendArgs.Where( c => c.ChildFieldsNames.Count > 1 &&
							                                (c.ChildFieldsNames[1][c.ChildFieldsNames[1].Length - 1] == '#' && c.ChildFieldsNames[1].Remove(c.ChildFieldsNames[1].IndexOf('#')) == cf.Name ||
							                                 c.ChildFieldsNames[1] == cf.Name)
							                              ).Select( c => c.Clone().TrimFirst() ).ToArray();
							var _resClass = cf.SetAllValues( resClass, _sendArgs );
#if GETFIELDS_CHECKERRORS
							
							if ( _resClass.GetType() != resClass.GetType() ) throw new Exception( "Type missmatch " + _resClass.GetType().Name + " " + resClass.GetType().Name );
							
#endif
							resClass = _resClass;
						}
					}
					
					else
					{	var _resClass = dicKey.Value[0].ArrayObject as UnityEngine.Object;
#if GETFIELDS_CHECKERRORS
						//if ( _resClass.GetType() != resClass.GetType() ) throw new Exception( "Type missmatch " + _resClass.GetType().Name + " " + resClass.GetType().Name );
#endif
						resClass = _resClass;
						// dic.Add( Name + "/#" + i , item as UnityEngine.Object );
					}
					
					arr[i] = resClass;
				}
				
				SetValue( o, arr );
				return o;
			}
			
			else
			{	var arr = resArray as Array;
			
				if ( isObject )
				{	for ( int i = 0 ; i < arr.Length ; i++ )
					{	arr.SetValue( null, i );
					}
				}
				
				foreach ( var dicKey in dicArrayKeys )
				{	var i = dicKey.Key;
				
					if ( i >= arr.Length ) continue;
					
					var resClass = arr.GetValue(i);
					
					if ( !isObject )
					{	var sendArgs = dicKey.Value.Select(d => d.ToFieldKey()).ToArray();
					
						foreach ( var cf in childFields.Values )
						{	var _sendArgs = sendArgs.Where(  c => c.ChildFieldsNames.Count > 1 &&
							                                 (c.ChildFieldsNames[1][c.ChildFieldsNames[1].Length - 1] == '#' && c.ChildFieldsNames[1].Remove(c.ChildFieldsNames[1].IndexOf('#')) == cf.Name ||
							                                  c.ChildFieldsNames[1] == cf.Name)
							                              ).Select( c => c.Clone().TrimFirst() ).ToArray();
							var _resClass = cf.SetAllValues( resClass, _sendArgs );
							
							/*  Debug.Log( "----" );
							  foreach ( var item in _sendArgs ) {
							      Debug.Log( cf.Name + " " + (item.ChildFieldsNames.Count == 0 ? "0" : item.ChildFieldsNames.Aggregate( ( a , b ) => a + "/" + b )) );
							  }*/
#if GETFIELDS_CHECKERRORS
							
							if ( _resClass.GetType() != resClass.GetType() ) throw new Exception( "Type missmatch " + _resClass.GetType().Name + " " + resClass.GetType().Name );
							
#endif
							resClass = _resClass;
							/* Debug.Log( Name + " "  + _sendArgs.Length );
							 Debug.Log( converted[0].ChildFieldsNames[0] );
							 Debug.Log( converted[0].ChildFieldsNames[1] );*/
							
						}
					}
					
					else
					{	var _resClass = dicKey.Value[0].ArrayObject as UnityEngine.Object;
#if GETFIELDS_CHECKERRORS
						// if ( _resClass.GetType() != resClass.GetType() ) throw new Exception( "Type missmatch " + _resClass.GetType().Name + " " + resClass.GetType().Name );
#endif
						resClass = _resClass;
						// dic.Add( Name + "/#" + i , item as UnityEngine.Object );
					}
					
					arr.SetValue( resClass, i );
				}
				
				SetValue( o, arr );
				return o;
			}
			
			/*var asda =  (IEnumerable)resArray;
			
			foreach ( var item in (IEnumerable)resArray ) {
			    if ( !isObject ) {
			        foreach ( var a in childFields.Values.SelectMany( c => c.GetAllValues( item , deep , searchType ) ) ) {
			            dic.Add( Name + "/#" + i + '#' + a.Key , a.Value );
			        }
			    } else {
			        dic.Add( Name + "/#" + i , item as UnityEngine.Object );
			    }
			    i++;
			}*/
			
			//throw new Exception( "Unknown field type" );
			
		}
		
		
		
		public object GetValue( object o )
		{	//  if ( !isObject ) return null;
			if ( isField ) res = f.GetValue( o );
			else res = p.GetValue( o, null );
			
			// if ( isObject )
			return res;
		}
		public void SetValue( object o, object value )
		{	//if ( !isObject ) return;
			if ( isField ) f.SetValue( o, value );
			else p.SetValue( o, value, null );
		}
		
		
		
	}
	
	static  object defVal;
	static  Dictionary<Type, object> defaultValueCache = new Dictionary<Type, object>();
	static internal object DefaultValue( Type type )
	{	if ( !defaultValueCache.TryGetValue( type, out defVal ) )
		{	defVal = Activator.CreateInstance( type );
			defaultValueCache.Add( type, defVal );
		}
		
		return defVal;
	}
	static internal bool IsObjectNull( /*Type type ,*/ object result )
	{	//if ( !type.IsClass ) return false;
		if ( result is UnityEngine.Object ) return !(result as UnityEngine.Object);
		
		return result == null;
		//return result == null || result.ToString() == "null" && result == Adapter.DefaultValue( type );
	}
	
	static Type rootType = typeof(object);
	static Type arrayType = typeof( Array );
	
	
	
	/* [MenuItem( "ASDASD/asdads" )]
	 static void aASD() {
	     asd qwe = null;
	     object o = qwe;
	     Debug.Log( o == null );
	     Debug.Log( Adapter.IsObjectNull(typeof(GameObject), o ) );
	     Debug.Log( Adapter.IsObjectNull(typeof(object) , o ) );
	 }
	 class asd
	 {
	
	 }*/
	/*  [Serializable]
	  class asd : List<int>
	  {
	
	  }
	
	  [MenuItem( "ASDASD/asdads" )]
	  static void aASD() {
	      List<int> asd1 = new List<int>() {1 };
	      int[] asd2 = new int[2];
	      ArrayList asd3 = new ArrayList();
	
	      object o1 =  asd1;
	      object o2 = asd2;
	      foreach ( var item in (o1 as System.Collections.IEnumerable) ) {
	          Debug.Log( item );
	
	      }
	
	  }*/
	/*   Debug.Log( asd1.GetType().IsSerializable );
	      var lt = (typeof( List<> ).MakeGenericType( asd1.GetType().GenericTypeArguments[0] ));
	      Debug.Log( asd1.GetType().IsGenericType + " " + lt.IsAssignableFrom( asd1.GetType() ) );
	      Debug.Log( lt.IsSerializable );
	      Debug.Log( arrayType.IsSerializable );
	      Debug.Log( typeof( ArrayList ).IsAssignableFrom( asd2.GetType() ) );
	      Debug.Log( arrayType.IsAssignableFrom( asd2.GetType() ) );*/
	/* void TryReadValue( Type type ) {
	     if ( arrayType.IsAssignableFrom( type ) )
	
	
	 }*/
	//  internal enum GET_FIELDS_TYPE { Default = 0, UnityEvent = 1 , NoArrays = 2}
	
	internal static KeyValuePair<string, KeyValuePair<FieldAdapter, object>>[] GET_FIELDS_AND_VALUES( UnityEngine.Object obj, Type type, bool includeArrays = true, int searchVals = 0 )
	{	var res = new Dictionary<string, KeyValuePair<FieldAdapter, object>>();
		var fff = Adapter.GET_FIELDS(type, includeArrays);
		
		foreach ( var f in fff )
		{	foreach ( var item in f.Value.GetAllValues( obj, 0, searchVals ) )
			{	res.Add( item.Key, new KeyValuePair<FieldAdapter, object>( f.Value, item.Value ) { } );
			}
		}
		
		return res.Select( d => new KeyValuePair<string, KeyValuePair<FieldAdapter, object>>( d.Key, d.Value ) ).ToArray();
	}
	
	
	static BindingFlags propFlags = BindingFlags.Public |  BindingFlags.Instance;
	static   Type monoType = typeof(MonoBehaviour);
	static   Type compType = typeof(Component);
	internal static Dictionary<string, FieldAdapter> GET_FIELDS( Type type, bool includeArrays = true )
	{	var SCAN_FIELDS_CACHE = includeArrays ? _SCAN_FIELDS_CACHE : _SCAN_FIELDS_CACHE_NOARRAYS;
	
		if ( SCAN_FIELDS_CACHE.ContainsKey( type ) ) return SCAN_FIELDS_CACHE[type];
		
		if ( type == rootType || type == unityObjectType )
		{	lock ( SCAN_FIELDS_CACHE) SCAN_FIELDS_CACHE.Add( type, new Dictionary<string, FieldAdapter>() );
		
			return SCAN_FIELDS_CACHE[type];
		}
		
		
		
		
		/* if ( listType == null ) {
		     listType = typeof( List<> );
		     arrayType = typeof( ArrayList );
		 }*/
		/*if (type.Name == "Canvas")
		{
		
		
		    Debug.Log( type.BaseType.Name);
		    Debug.Log( type.GetProperties((BindingFlags)( - 1)).Count());
		
		    Debug.Log( type.GetFields((BindingFlags)( - 1)).First().Name);
		    Debug.Log( type.GetFields(flags).Count(f =>
		    {   if (!unityObjectType.IsAssignableFrom(f.FieldType)) return false;
		        return true;
		    }));
		    Debug.Log( type.GetFields(flags).Count(f =>
		    {   if (!unityObjectType.IsAssignableFrom(f.FieldType)) return false;
		        if (f.IsPublic) return true;
		        return false;
		    }));
		}*/
		var result = new Dictionary<string, FieldAdapter>();
		
		foreach ( var item in type.GetFields( flags ).Where( f =>
	{	// if ( !searchType.IsAssignableFrom( f.FieldType ) ) return false;
		/* if ( Adapter.UnityEventArgsType == f.FieldType  )
		     Debug.Log( "!@#" );asdasd*/
		
		if ( f.FieldType.IsPrimitive ) return false;
		
			if ( !f.IsPublic )
			{	return f.IsDefined( serType, true );
			}
			
			/* if ( f.IsNotSerialized ) return false;
			 var res = f.GetCustomAttributes( serType , false ).Length != 0;
			 if ( !res ) return false;*/
			
			
			
			return !f.IsDefined( serType2, true );
			// return f.GetCustomAttributes( serType2 , false ).Length == 0;
		} ) )
		
		if ( !result.ContainsKey( item.Name ) )
		{	var fa =  FieldAdapter.TryToCreate( item, includeArrays );
		
			if ( !ReferenceEquals( fa, null ) ) result.Add( item.Name, fa );
		}
		
		
		//#todo compType.IsAssignableFrom( type ) added for perfomance, but it may affect to any components if playmodekeeper
		if ( !monoType.IsAssignableFrom( type ) && compType.IsAssignableFrom( type ) )
		{
		
		
			var estimProps = type.GetProperties( propFlags ).Where( f =>
			{	if ( !f.PropertyType.IsClass ) return false;
			
				if ( !f.CanRead || !f.CanWrite ) return false;
				
				// if ( !searchType.IsAssignableFrom( f.PropertyType ) ) return false;
				//if ( f.GetGetMethod() == null || f.GetSetMethod() == null ) return false;
				//if ( !f.GetGetMethod().IsPublic || !f.GetSetMethod().IsPublic ) return false;
				return (f.GetGetMethod().GetMethodImplementationFlags() & MethodImplAttributes.InternalCall) != 0 && (f.GetSetMethod().GetMethodImplementationFlags() & MethodImplAttributes.InternalCall) != 0;
			} ).ToList();
			
			for ( int i = 0 ; i < estimProps.Count ; i++ )
			{	if ( estimProps[i].Name.StartsWith( "shared", StringComparison.InvariantCultureIgnoreCase ) )
				{	var ft = estimProps [i];
					var fname = ft.Name.Substring("shared".Length);
					estimProps.RemoveAll( e => e.Name.Equals( fname, StringComparison.InvariantCultureIgnoreCase ) );
					i = estimProps.IndexOf( ft );
				}
			}
			
			foreach ( var item in estimProps )
			{	if ( !result.ContainsKey( item.Name ) )
				{	var fa =  FieldAdapter.TryToCreate( item, includeArrays );
				
					if ( !ReferenceEquals( fa, null ) ) result.Add( item.Name, fa );
				}
			}
		}
		
		
		var ct = type.BaseType;
		
		while ( ct != rootType )
		{	foreach ( var item in GET_FIELDS( ct, includeArrays ) )
				if ( !result.ContainsKey( item.Key ) ) result.Add( item.Key, item.Value );
				
			ct = ct.BaseType;
		}
		
		
		
		lock ( SCAN_FIELDS_CACHE )
		{	// MonoBehaviour.print(type + " " + result.Count);
			SCAN_FIELDS_CACHE.Add( type, result );
		}
		
		return result;
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	//  M_DescriptionCommon _mdescript;
	internal M_DescriptionCommon DescriptionModule; /*{
            get {
                if ( _mdescript == null ) _mdescript = modules.FirstOrDefault( m => m is M_DescriptionCommon ) as M_DescriptionCommon;
                return _mdescript;
            }
        }*/

	// M_Colors _mColorModule;
	internal M_Colors ColorModule; /*{
            get {
                if ( _mColorModule == null ) _mColorModule = modules.FirstOrDefault( m => m is M_Colors ) as M_Colors;
                return _mColorModule;
            }
        }*/

	static private Color c1 = (Color)new Color32(56, 56, 56, 255);
	static private Color c2 = (Color)new Color32(194, 194, 194, 255);
	static private Color setc2 = (Color)new Color32(222, 222, 222, 255);
	static private Color c1111 = (Color)new Color32(62, 62, 62, 255);
	static private Color c2222 = (Color)new Color32(217, 217, 217, 255);
	static internal Color LINE = new Color(.1f, .1f, .1f, 0.1F);
	
	internal static Color EditorBGColor
	{	get { return EditorGUIUtility.isProSkin ? c1 : c2; }
	}
	internal static Color SettingsBGColor
	{	get { return EditorGUIUtility.isProSkin ? c1 : setc2; }
	}
	
	internal static Color SceneColor
	{	get { return EditorGUIUtility.isProSkin ? c1111 : c2222; }
	}
	
	//internal static Color B_ACTIVE = new Color(.6f, .75f, .8f, 0.6f);
	internal static Color B_ACTIVE = new Color(.55f, .6f, .6f, 0.6f);
	internal static Color B_PASSIVE = new Color(.8f, .8f, .8f, 0.1F);
	
	//#tag Instance id test
	/*
	   [MenuItem("PluginEditor/Try Get ID", false, 51006)]
	   static void PacIconasdss()
	   {   Debug.Log(Adapter.GET_INSTANCE_ID( Selection.gameObjects[0]));
	       Debug.Log(Hierarchy_GUI.HierarchySettings.PrefabIDMode);
	       Debug.Log(UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(Selection.gameObjects[0]).name);
	       PrefabUtility.GetPrefabParent
	       return;
	
	       var go = Selection.gameObjects[0];
	
	       var prfp = PrefabUtility.GetCorrespondingObjectFromSource(go);
	       if (!prfp) Debug.Log("NotPref");
	       string fakestring;
	       long templong;
	
	
	       AssetDatabase.TryGetGUIDAndLocalFileIdentifier(prfp, out fakestring, out templong);
	
	       Debug.Log(templong);
	
	   }*/
	
	internal static UnityEngine.Object GET_OBJECT( long id )
	{	return GET_OBJECT( (int)id );
	}
	internal static UnityEngine.Object GET_OBJECT( int id )     //EditorUtility.InstanceIDToObject( jsonData.fields_value[i] )
	{	return InternalEditorUtility.GetObjectFromInstanceID( id );
	}
	
	
	internal static string GetScenePath( Scene s )
	{	var p = s.path;
	
		if ( string.IsNullOrEmpty( p ) ) return "untitled.unity";
		
		return p;
	}
	internal string GetStoredDataPathInternal( Scene s )
	{	return PluginInternalFolder + "/_ SAVED DATA/" + GetScenePath( s ).Remove( GetScenePath( s ).LastIndexOf( '.' ) ) + ".asset";
	}
	internal string GetStoredDataPathInternal( string p )
	{	if ( !p.ToLower().EndsWith( ".unity" ) ) return null;
	
		p = p.Remove( p.Length - ".unity".Length );
		return PluginInternalFolder + "/_ SAVED DATA/" + p + ".asset";
	}
	
	internal string GetStoredDataPathExternal( Scene s )
	{	var path = Adapter.UNITY_SYSTEM_PATH;
	
		if ( !path.EndsWith( "/" ) ) path += '/';
		
		return path + GetStoredDataPathInternal( s );
	}
	internal string GetStoredDataPathExternal( string s )
	{	var path = Adapter.UNITY_SYSTEM_PATH;
	
		if ( !path.EndsWith( "/" ) ) path += '/';
		
		return path + GetStoredDataPathInternal( s );
	}
	
	internal static long GET_INSTANCE_ID( GameObject go )
	{	if ( !go ) return 0;
	
		if ( Hierarchy_GUI.HierarchySettings.PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances )
		{	var pref = HierAdapter.GetPrefabInstanceHandle(go);
		
			if ( pref )     //  return pref.GetInstanceID();
			{	return Adapter.GetLocalIdentifierInFile( pref );
			}
		}
		
		return go.GetInstanceID();
	}
	
	
	
	
	internal void SavePrefs()
	{	EditorPrefs.SetString( GET_HIER_PARAM_KEY( this ), Adapter.SERIALIZE_SINGLE( par ) );
		// MonoBehaviour.print("SAVE" + par.FD_Icons_default);
	}
	
	
#if UNITY_EDITOR
	/* [StructLayout( LayoutKind.Explicit )]
	 public class UnionHierParams {
	     [FieldOffset(0)]
	     public HierParams hierarchy;
	     [FieldOffset(0)]
	     public EModules.EProjectInternal.HierParams project;
	
	     public UnionHierParams( EModules.EProjectInternal.HierParams project )
	     {   this.hierarchy = default( HierParams );
	         this.project = project;
	     }
	 }
	 */
	
	public void ShallowCopyValues<T1, T2>( ref T1 firstObject, ref T2 secondObject )
	{	const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		var firstFieldDefinitions = firstObject.GetType().GetFields(bindingFlags);
		{	IEnumerable<FieldInfo> secondFieldDefinitions = secondObject.GetType().GetFields(bindingFlags);
		
			foreach ( var fieldDefinition in firstFieldDefinitions )
			{	var matchingFieldDefinition = secondFieldDefinitions.FirstOrDefault(fd => fd.Name == fieldDefinition.Name &&
				                              fd.FieldType == fieldDefinition.FieldType);
				                              
				if ( matchingFieldDefinition == null )
					continue;
					
				var value = fieldDefinition.GetValue(firstObject);
				matchingFieldDefinition.SetValue( secondObject, value );
			}
		}
		{	IEnumerable<PropertyInfo> secondFieldDefinitions = secondObject.GetType().GetProperties(bindingFlags);
		
			foreach ( var fieldDefinition in firstFieldDefinitions )
			{	var matchingFieldDefinition = secondFieldDefinitions.FirstOrDefault(fd => fd.Name == fieldDefinition.Name &&
				                              fd.PropertyType == fieldDefinition.FieldType);
				                              
				if ( matchingFieldDefinition == null )
					continue;
					
				var value = fieldDefinition.GetValue(firstObject);
				matchingFieldDefinition.SetValue( secondObject, value, null );
			}
		}
		
		
		
	}
	
	public static byte[] RawSerialize( object anything )
	{	int rawsize = Marshal.SizeOf(anything);
		byte[] rawdata = new byte[rawsize];
		GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
		Marshal.StructureToPtr( anything, handle.AddrOfPinnedObject(), false );
		handle.Free();
		return rawdata;
	}
	
	public T ReadStruct<T>(/*FileStream fs*/byte[] buffer )     //byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
	{	// fs.Read( buffer, 0, Marshal.SizeOf( typeof( T ) ) );
		GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		T temp = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
		handle.Free();
		return temp;
	}
	
	/* unsafe HierParams CREATE_PROJWECT_OPARAMS()
	 {
	
	             var a = new EModules.EProjectInternal.HierParams()
	             {
	                 ENABLE_ALL = true,
	                 ENABLE_PING_Fix = true,
	                 ENABLE_LEFTDOCK_FIX = true,
	                 ENABLE_RIGHTDOCK_FIX = true,
	                 //M_params = new Dictionary<Type, string>(),
	                 FD_Icons_default = true,
	                 FD_Icons_mono = true,
	                 FD_Icons_user = true,
	             };
	     var result = new HierParams();
	
	     Buffer.MemoryCopy(&a,&result,)
	 }*/
	
	
#endif
	internal static string STR_DATA( Adapter adapter ) { return adapter.pluginID == Initializator.HIERARCHY_ID ? "hierarchydata.editor" : "projectdata.editor"; }
	
	
	
	internal const HideFlags flagsSHOW = /*HideFlags.DontUnloadUnusedAsset |*/ HideFlags.DontSaveInBuild ;
	internal const HideFlags flagsHIDE = /*HideFlags.DontUnloadUnusedAsset |*/ HideFlags.DontSaveInBuild | HideFlags.HideInInspector | HideFlags.HideInHierarchy;
	
	
	
	internal static Adapter HierAdapter
	{	get
		{	if ( Initializator.AdaptersByID.ContainsKey( Initializator.HIERARCHY_ID ) ) return Initializator.AdaptersByID[Initializator.HIERARCHY_ID];
		
			return null;
		}
	}
	
	internal static Adapter ProjAdapter
	{	get
		{	if ( Initializator.AdaptersByID.ContainsKey( Initializator.PROJECT_ID ) ) return Initializator.AdaptersByID[Initializator.PROJECT_ID];
		
			return null;
		}
	}
	
	
	
	/* static Dictionary<int, Type> get_type_helper = new Dictionary<int, Type>();
	static Dictionary<int, string> get_typeName_helper = new Dictionary<int, string>();
	static Dictionary<int, string> get_typeFullName_helper = new Dictionary<int, string>();
	static int id;*/
	internal static Type GetType_( UnityEngine.Object obj )
	{	return obj.GetType();
		/*  id = obj.GetInstanceID();
		  if (!get_type_helper.ContainsKey(id)) get_type_helper.Add(id, obj.GetType());
		  return get_type_helper[id];*/
	}
	internal static string GetTypeName( UnityEngine.Object obj )
	{	return obj.GetType().Name;
		/* id = obj.GetInstanceID();
		 if (!get_typeName_helper.ContainsKey(id))
		 {
		     get_typeName_helper.Add(id, GetType_(obj).Name);
		 }
		 return get_typeName_helper[id];*/
	}
	internal static string GetTypeFullName( UnityEngine.Object obj )
	{	return obj.GetType().FullName;
		/* id = obj.GetInstanceID();
		 if (!get_typeFullName_helper.ContainsKey(id))
		 {
		     get_typeFullName_helper.Add(id, GetType_(obj).FullName);
		 }
		 return get_typeFullName_helper[id];*/
	}
	
	
	
	internal static void EventUseFast()
	{	Event.current.Use();
	}
	
	
	internal static void EventUse()     //   MonoBehaviour.print("USE");
	{	if ( Event.current != null && Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout ) Adapter.EventUseFast();
	}
	
	
	
	internal void SetDirtyDescription( IHashProperty d, int s )
	{	SetDirtyDescription( d, GET_SCENE_BY_ID( s ) );
	}
	
	internal void SetDirtyDescription( IHashProperty d, Scene s )
	{	if ( IS_PROJECT() )
		{	SET_DIRTY_PROJECT();
		}
		
		else
		{
#if HIERARCHY
		
			if ( d is HierarchyDescriptionHelper ) ((HierarchyDescriptionHelper)d).SetDirty( s );
			else
				if ( d.component ) Adapter.SetDirty( d.component );
				
#endif
		}
		
		/*  if (Application.isPlaying) return;
		
		  Hierarchy.SetDirty(d.component);
		  Hierarchy.SetDirty(d.gameObject);
		  // MonoBehaviour.print("SET");
		  //Hierarchy.SetDirty(o);
		  EditorUtility.SetDirty(o);*/
	}
	
	
	
	
	
	
	internal static void SetDirty( UnityEngine.Object o )
	{	if ( Application.isPlaying ) return;
	
		// MonoBehaviour.print("SET");
		//Hierarchy.SetDirty(o);
		EditorUtility.SetDirty( o );
	}
	
	internal void CreateUndoActiveDescription( string name, Scene scene ) { CreateUndoActiveDescription( name, scene.GetHashCode() ); }
	internal void CreateUndoActiveDescription( string name, int _scene )
	{	if ( Application.isPlaying ) return;
	
	
#if HIERARCHY
		// var scene = EditorSceneManager.GetActiveScene();
		{
		}
		
#endif
		IHashProperty d = null;
		
		var scene = _scene;
		
		if ( IS_HIERARCHY() )
			if ( !GET_SCENE_BY_ID( _scene ).IsValid() || !GET_SCENE_BY_ID( _scene ).isLoaded ) return;
			
		d = MOI.des( scene );
		
		if ( d != null )
			SET_UNDO( d, name );
			
		/*  if (IS_PROJECT())
		  {   SET_DIRTY_PROJECT();
		  }*/
	}
	
	internal void SetDirtyActiveDescription( Scene scene ) { SetDirtyActiveDescription( scene.GetHashCode() ); }
	internal void SetDirtyActiveDescription( int _scene )
	{	if ( Application.isPlaying ) return;
	
#if HIERARCHY
	
		if ( IS_HIERARCHY() )     // var scene = EditorSceneManager.GetActiveScene();
		{	var scene = GET_SCENE_BY_ID(_scene);
		
			if ( !scene.IsValid() || !scene.isLoaded ) return;
			
			MarkSceneDirty( scene );
			var d = MOI.des(scene.GetHashCode());
			
			if ( d == null ) return;
			
			if ( d is HierarchyDescriptionHelper ) ((HierarchyDescriptionHelper)d).SetDirty( scene );
			else
				if ( d.component ) Adapter.SetDirty( d.component );
		}
		
#endif
		
		
		if ( IS_PROJECT() )
		{	SET_DIRTY_PROJECT();
		}
		
	}
	
	internal void MarkSceneDirty( Scene s )     //  if (Application.isPlaying || EditorSceneManager.GetActiveScene().isDirty || IS_PROJECT()) return;
	{
	
		MarkSceneDirty( s.GetHashCode() );
	}
	internal void MarkSceneDirty( int s )
	{	if ( Application.isPlaying || IS_PROJECT() ) return;
	
		// MonoBehaviour.print("SET");
		//Hierarchy.SetDirty(o);
		// if ( Hierarchy_GUI.Instance( this ).SaveToScriptableObject == "FOLDER" ) return;
		var getS = GET_SCENE_BY_ID(s);
		
		if ( getS.IsValid() && getS.isLoaded && !getS.isDirty ) EditorSceneManager.MarkSceneDirty( getS );
		
	}
	
	static Scene dasdasd;
	static Dictionary<int, Scene> cache_scenes = new Dictionary<int, Scene>();
	internal static Scene GET_SCENE_BY_ID( int id )
	{
	
		if ( cache_scenes.TryGetValue( id, out dasdasd ) && dasdasd.IsValid() ) return dasdasd;
		
		for ( int i = 0 ; i < SceneManager.sceneCount ; i++ )
		{	if ( SceneManager.GetSceneAt( i ).GetHashCode() == id )
			{	if ( !cache_scenes.ContainsKey( id ) ) cache_scenes.Add( id, SceneManager.GetSceneAt( i ) );
				else cache_scenes[id] = SceneManager.GetSceneAt( i );
				
				return SceneManager.GetSceneAt( i );
			}
		}
		
		if ( EditorSceneManager.GetActiveScene().GetHashCode() == id )
			if ( !cache_scenes.ContainsKey( id ) ) cache_scenes.Add( id, EditorSceneManager.GetActiveScene() );
			else cache_scenes[id] = EditorSceneManager.GetActiveScene();
			
		return EditorSceneManager.GetActiveScene();
	}
	
	
	/*
	
	internal void MarkSceneDirty(Scene scene)
	{
	    if (Application.isPlaying || EditorSceneManager.GetActiveScene().isDirty || IS_PROJECT()) return;
	    // MonoBehaviour.print("SET");
	    //Hierarchy.SetDirty(o);
	    EditorSceneManager.MarkSceneDirty( EditorSceneManager.GetActiveScene() );
	}
	*/
	
	internal static string SERIALIZE_SINGLE( object ob )
	{	return Serializer.SERIALIZE_SINGLE( ob );
	
		// return EditorJsonUtility.ToJson(ob);
		
		/*  using (var stream = new MemoryStream())
		  {
		      EditorJsonUtility.ToJson()
		
		      System.Runtime.Serialization.Formatters.Binary.BinaryFormatter.
		      new DataContractJsonSerializer();
		
		      XmlSerializer formatter = new XmlSerializer(typeof(object));
		      formatter.Serialize(stream, ob);
		      var result = stream.GetBuffer();
		      return Convert.ToBase64String(result);
		  }*/
		
		
		/*  using (var stream = new MemoryStream())
		  {
		      var bin = new BinaryFormatter();
		
		      // bin.FilterLevel = TypeFilterLevel.LowFull
		      bin.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
		      // bin.Binder = new AllowAllAssemblyVersionsDeserializationBinder();
		      //  bin.Binder = new Version1ToVersion2DeserializationBinder();
		      // bin.Binder = new BindChanger();
		      //  bin.Binder = new MyBinder();
		
		      bin.Serialize(stream, ob);
		      var result = stream.GetBuffer();
		      return Convert.ToBase64String(result);
		  }*/
	}
	/*  public static T DESERIALIZE_SINGLE<T>(string ser)
	  {   var bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
	      bin.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
	      bin.Binder = new MyBinder();
	      bin.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low;
	      bin.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.XsdString;
	
	      var bytes = Convert.FromBase64String(ser);
	      using (var stream = new MemoryStream( bytes ))
	      {   Debug.Log(bin);
	          Debug.Log(stream);
	          var result = bin.Deserialize(stream);
	          if (result == null) return default( T );
	          return (T)result;
	      }
	  }*/
	internal static T DESERIALIZE_SINGLE<T>( string ser )
	{
	
	
	
		return Serializer.DESERIALIZE_SINGLE<T>( ser );
		// BinaryFormatterHelper.Write(myObject1, FILENAME);
		
		// Deserialize
		/*try
		{
		    T target = Activator.CreateInstance<T>();
		    EditorJsonUtility.FromJsonOverwrite(ser, target);
		    return target;
		}
		catch
		{
		    return default(T);
		}*/
		
		/* XmlSerializer formatter = new XmlSerializer(typeof(object));
		 var bytes = Convert.FromBase64String(ser);
		 using (var stream = new MemoryStream(bytes)) return formatter.Deserialize(stream);*/
		/*  var bin = new BinaryFormatter();
		  //  bin.Binder = new Version1ToVersion2DeserializationBinder();
		  //  bin.Binder = new AllowAllAssemblyVersionsDeserializationBinder();
		
		  // bin.FilterLevel = TypeFilterLevel.Low;
		  bin.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
		  //bin.Binder = new BindChanger();
		  bin.Binder = new MyBinder();
		
		  var bytes = Convert.FromBase64String(ser);
		  using (var stream = new MemoryStream(bytes))
		  {
		      var result = bin.Deserialize(stream);
		      if (result == null) return default(T);
		      return (T)result;
		  }*/
	}
	
	
	
	
	/*  public void SerializeState(string filename, ProgramState ps)
	  {
	      Stream s = File.Open(filename, FileMode.Create);
	      BinaryFormatter bFormatter = new BinaryFormatter();
	      bFormatter.AssemblyFormat =
	         System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
	      bFormatter.Serialize(s, ps);
	      s.Close();
	  }
	
	public object DeserializeState(string filename)
	  {
	      if (File.Exists(filename))
	      {
	          ProgramState res = new ProgramState();
	          Stream s = File.Open(filename, FileMode.Open);
	          BinaryFormatter bFormatter = new BinaryFormatter();
	          bFormatter.AssemblyFormat =
	            System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
	          bFormatter.Binder = new MyBinder(); // MyBinder class code given below
	          try
	          {
	              res = (ProgramState)bFormatter.Deserialize(s);
	          }
	          catch (SerializationException se)
	          {
	              Debug.WriteLine(se.Message);
	          }
	          s.Close();
	          return res;
	      }
	      else return null;
	  }*/
	
	class BindChanger : System.Runtime.Serialization.SerializationBinder {
		/* public override Type BindToType(string assemblyName, string typeName)
		 {
		     // Define the new type to bind to
		     Type typeToDeserialize = null;
		     // Get the current assembly
		     string currentAssembly = Assembly.GetExecutingAssembly().FullName;
		     // Create the new type and return it
		     typeToDeserialize = Type.GetType(string.Format("{0}, {1}", typeName, currentAssembly));
		     return typeToDeserialize;
		 }*/
		
		static Assembly[] ass;
		
		public override Type BindToType( string assemblyName, string typeName )
		{	Debug.Log( typeName );
		
			if ( ass == null ) ass = AppDomain.CurrentDomain.GetAssemblies().Where( s => s.FullName.StartsWith( "System" ) ).ToArray();
			
			for ( int i = 0 ; i < ass.Length ; i++ )
			{	if ( ass[i].GetType( typeName ) != null ) return ass[i].GetType( typeName );
			}
			
			string currentAssembly = Assembly.GetExecutingAssembly().FullName;
			var typeToDeserialize = Type.GetType(string.Format("{0}, {1}", typeName, currentAssembly));
			return typeToDeserialize;
		}
	}
	
	internal sealed class MyBinder : SerializationBinder {
		static Dictionary<string, Type> cache = new Dictionary<string, Type>();
		public override Type BindToType( string assemblyName, string typeName )
		{	if ( cache.ContainsKey( typeName ) ) return cache[typeName];
		
			Type ttd = null;
			// try
			{	string toassname = assemblyName.Split(',')[0];
				Assembly[] asmblies = AppDomain.CurrentDomain.GetAssemblies();
				
				foreach ( Assembly ass in asmblies )
				{	if ( ass.FullName.Split( ',' )[0] == toassname )
					{	ttd = ass.GetType( typeName );
						break;
					}
				}
			}
			/* catch (System.Exception e)
			 {
			     throw new Exception(e.Message);
			 }*/
			cache.Add( typeName, ttd );
			
			return ttd;
		}
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	[Serializable]
	public class AA : ISerializable {
		[NonSerialized]
		public Dictionary<int, ASD2> records;
		
		public AA( bool a )
		{
		
		}
		
		public void GetObjectData( SerializationInfo info, StreamingContext context )
		{	int[] comp_to_Type_keys = new int[records.Count];
			ASD2[] comp_to_Type_values = new ASD2[records.Count];
			int i = 0;
			
			foreach ( var type in records )
			{	comp_to_Type_keys[i] = type.Key;
				comp_to_Type_values[i] = type.Value;
				i++;
			}
			
			info.AddValue( "records keys", comp_to_Type_keys, typeof( int[] ) );
			info.AddValue( "records values", comp_to_Type_values, typeof( ASD2[] ) );
		}
		
		public AA( SerializationInfo info, StreamingContext context )
		{	records = new Dictionary<int, ASD2>();
		
			try
			{	int[] comp_to_Type_keys = (int[])info.GetValue("records keys", typeof(int[]));
				ASD2[] comp_to_Type_values = (ASD2[])info.GetValue("records values", typeof(ASD2[]));
				
				for ( int i = 0 ; i < comp_to_Type_keys.Length ; i++ )
					records.Add( comp_to_Type_keys[i], comp_to_Type_values[i] );
			}
			
			catch
			{
			
			}
			
		}
		
		
		internal static void testc()
		{	/* var aa = new AA(true);
			     aa.records = new Dictionary<int, ASD2>();
			     aa.records.Add(5, new ASD2());
			
			     var s = Adapter.SERIALIZE_SINGLE(aa);
			     Debug.Log(s);
			     var des = Adapter.DESERIALIZE_SINGLE<AA>(s);
			     Debug.Log(des.records);
			     Debug.Log(des.records.Count);
			     Debug.Log(des.records.FirstOrDefault().Value != null);*/
			
			
			KeeperData data1 = new KeeperData();
			data1.field_records = new Dictionary<long, KeeperDataItem>();
			data1.field_records.Add( 5, new KeeperDataItem() );
			data1.field_records[5].records = new Dictionary<long, KeeperDataUnityJsonData>();
			data1.field_records[5].records.Add( 5, new KeeperDataUnityJsonData() { default_json = "ASD" } );
			
			var s = Adapter.SERIALIZE_SINGLE(data1);
			Debug.Log( s );
			data1 = Adapter.DESERIALIZE_SINGLE<KeeperData>( s );
			Debug.Log( "1- " + data1 );
			Debug.Log( "2- " + data1.field_records );
			Debug.Log( "3- " + data1.field_records[5] );
			Debug.Log( "4- " + data1.field_records[5].records );
			Debug.Log( "5- " + data1.field_records[5].records[5] );
			Debug.Log( "6- " + data1.field_records[5].records[5].default_json );
		}
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	internal static void FadeSceneRect( Rect drawRect, float alpha = 0.6f )
	{	if ( alpha < 0.01f ) return;
	
		var defColor = GUI.color;
		var c = Adapter.SceneColor;
		c.a = alpha;
		GUI.color *= c;
		
		GUI.DrawTexture( drawRect, EditorGUIUtility.whiteTexture );
		GUI.color = defColor;
	}
	
	internal static void FadeRect( Rect drawRect, float alpha = 0.6f )
	{	if ( alpha < 0.01f ) return;
	
		var defColor = GUI.color;
		var c = Adapter.EditorBGColor;
		c.a = alpha;
		GUI.color *= c;
		
		GUI.DrawTexture( drawRect, EditorGUIUtility.whiteTexture );
		GUI.color = defColor;
	}
	
	Color m_HoverColorPersonal = new Color32( 170, 170, 170, 255  );
	Color m_HoverColorPro = new Color32( 51, 51, 51, 255  );
	internal  Color HoverColor
	{	get
		{	if ( hoveredBackgroundColor.HasValue ) return Color.Lerp( EditorBGColor, new Color( hoveredBackgroundColor.Value.r, hoveredBackgroundColor.Value.g, hoveredBackgroundColor.Value.b, 1 ),
				        hoveredBackgroundColor.Value.a );
				        
			if ( EditorGUIUtility.isProSkin ) return m_HoverColorPro;
			else return m_HoverColorPersonal;
		}
	}
	
	// static Color m_PrefabColorPro = new Color32( 76, 128, 217, 255  );
	static Color tttCC;
	static Color m_SelectColorPro = new Color32( 62, 95, 150, 255  );
	//  static Color m_SelectColorProNonFocus = new Color32( 104, 104, 104, 255  );
	static Color m_SelectColorProNonFocus = new Color32( 72, 72, 72, 255  );
	static Color m_SelectColorPersonal = new Color32( 62, 125, 231, 255  );
	static Color m_SelectColorPersonalNonFOcus = new Color32( 143, 143, 143, 255  );
	internal Color? backedSelectColor;
	
	internal Color SelectColor
	{	get
		{	if ( backedSelectColor.HasValue ) return backedSelectColor.Value;
		
			if ( EditorWindow.focusedWindow == window() && (string.IsNullOrEmpty( GUI.GetNameOfFocusedControl() ) || GUI.GetNameOfFocusedControl() != "SearchFilter") )
			{	if ( EditorGUIUtility.isProSkin ) backedSelectColor = m_SelectColorPro;
				else backedSelectColor = m_SelectColorPersonal;
			}
			
			else
			{	if ( EditorGUIUtility.isProSkin ) backedSelectColor = m_SelectColorProNonFocus;
				else backedSelectColor = m_SelectColorPersonalNonFOcus;
			}
			
			return backedSelectColor.Value;
			
		}
	}
	internal Color SelectColorOverrided( bool active )
	{	if ( active )
		{	if ( EditorGUIUtility.isProSkin ) return m_SelectColorPro;
			else return m_SelectColorPersonal;
		}
		
		else
		{	if ( EditorGUIUtility.isProSkin ) return m_SelectColorProNonFocus;
			else return m_SelectColorPersonalNonFOcus;
		}
	}
	
	internal void SelectRect( Rect drawRect, float alpha = 1, bool? overrideSelect = null )
	{	var defColor = GUI.color;
	
		if ( overrideSelect.HasValue )
		{	tttCC = SelectColorOverrided( overrideSelect.Value );
		}
		
		else
		{	tttCC = SelectColor;
		}
		
		tttCC.a = alpha;
		// tttCC.a = (byte)Mathf.Clamp( Mathf.RoundToInt( alpha * 255 ), 0, 255 );
		GUI.color *= tttCC;
		/*  if (EditorGUIUtility.isProSkin)
		    GUI.color *= new Color32( 62, 95, 150, (byte)Mathf.Clamp( Mathf.RoundToInt( alpha * 255 ), 0, 255 ) );
		  else GUI.color *= new Color32( 62, 125, 231, (byte)Mathf.Clamp( Mathf.RoundToInt( alpha * 255 ), 0, 255 ) );*/
		
		GUI.DrawTexture( drawRect, EditorGUIUtility.whiteTexture );
		GUI.color = defColor;
	}
}
}
