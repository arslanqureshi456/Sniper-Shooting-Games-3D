using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
#if PROJECT
	using EModules.Project;
#endif
//namespace EModules



namespace EModules.EModulesInternal

{



internal partial class Adapter {

	void CheckProjectDefines()
	{	if (pluginID == 0) return;
	
		if (!par.ENABLE_ALL)
		{	//foreach (var item in Enum.GetValues(typeof(BuildTargetGroup)))
			{	var item = EditorUserBuildSettings.activeBuildTarget;
			
				var d = PlayerSettings.GetScriptingDefineSymbolsForGroup((BuildTargetGroup)item).Split(';').ToList();
				
				if (!d.Contains("EMX_PROJECT_DISABLE"))
				{	d.Add("EMX_PROJECT_DISABLE");
					PlayerSettings.SetScriptingDefineSymbolsForGroup( (BuildTargetGroup)item, d.Count > 0 ?d.Aggregate((a, b)=>a+";"+b) : "");
				}
				
			}
		}
		
		else
		{	foreach (var item in Enum.GetValues(typeof(BuildTargetGroup)))
			{	//var item = EditorUserBuildSettings.activeBuildTarget;
			
				var d = PlayerSettings.GetScriptingDefineSymbolsForGroup((BuildTargetGroup)item).Split(';').ToList();
				//Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup((BuildTargetGroup)item));
				
				if (d.Any(s=>s=="EMX_PROJECT_DISABLE"))
				{	d.RemoveAll(c=>c=="EMX_PROJECT_DISABLE");
					PlayerSettings.SetScriptingDefineSymbolsForGroup( (BuildTargetGroup)item, d.Count > 0 ? d.Aggregate((a, b)=>a+";"+b) : "");
				}
			}
		}
	}
	static  string TOGGLE_PTR = "• ";
	internal void OnEnableChange()
	{	RepaintAllViews();
		NeedBottomPositionUpdate = true;
		tempAdapterBlock = false;
		RepaintAllViews();
		
		if (   pluginID != 0 )
		{	CheckProjectDefines();
		}
		
		// InternalEditorUtility.RepaintAllViews();
		if ( !par.ENABLE_ALL )
		{	OnDisablePlugin();
		}
		
		else
		{	if ( parLINE_HEIGHT != EditorGUIUtility.singleLineHeight ) ResetScroll();
		}
		
		Adapter.RequestScriptReload();
	}
	
	const int LAST = 12;
	
	static void IndexAction( Adapter A, int ind, string searchContex )
	{	switch ( ind )
		{	case 0: A.SETTINGS_MAIN( searchContex ); break;
		
			case 1: A.SETTINGS_HIGHLIGHTER_MAIN( searchContex ); break;
			
			case 2: /*A.SETTINGS_HIGHLIGHTER_AUTO( searchContex );*/ break;
			
			case 3: A.SETTINGS_BOTTOM_MAIN( searchContex ); break;
			
			case 4:  A.SETTINGS_BOTTOM_HELP( searchContex ); break;
			
			case 5: A.SETTINGS_SEARCH( searchContex ); break;
			
			case 6: A.SETTINGS_GENERIC( searchContex ); break;
			
			case 7: A.SETTINGS_CACHE( searchContex ); break;
			
			case 8: A.SETTINGS_RIGHTMAIN( searchContex ); break;
			
			case 9: if ( A.IS_HIERARCHY() ) A.SETTINGS_RIGHTMAIN_MODULE0( searchContex ); break;
			
			case 10: if ( A.IS_HIERARCHY() ) A.SETTINGS_RIGHTMAIN_MODULE1( searchContex ); break;
			
			case 11: if ( A.IS_HIERARCHY() ) A.SETTINGS_RIGHTMAIN_MODULE2( searchContex ); break;
			
			case LAST: if ( A.IS_HIERARCHY() ) A.SETTINGS_RIGHTMAIN_MODULE3( searchContex ); break;
			
			default: A.SETTINGS_MAIN( searchContex ); break;
		}
	}
	
	static bool IndexActionValidate( Adapter A, int ind, string searchContex )
	{	switch ( ind )
		{	case 0: return true;
		
			case 1: return true;
			
			case 2: return false;
			
			case 3: return true;
			
			case 4: return false;
			
			case 5: return true;
			
			case 6: return true;
			
			case 7: return true;
			
			case 8: return true;
			
			case 9: return A.IS_HIERARCHY();
			
			case 10: return A.IS_HIERARCHY();
			
			case 11: return A.IS_HIERARCHY();
			
			case LAST: return A.IS_HIERARCHY();
			
			default: return true;
		}
	}
	
	
	static string IndexToString( int ind )
	{	switch ( ind )
		{	case 0: return "Main Settings";
		
			case 1: return "HighLighter";
			
			case 2: return "Auto Filters";
			
			case 3: return "Bottom Bar";
			
			case 4: return "Quick Help";
			
			case 5: return "Search Window";
			
			case 6: return "Custom Click Menu";
			
			case 7: return "Cache";
			
			case 8: return "Right Bar";
			
			case 9: return "Display of Functions and Vars";
			
			case 10: return "PlayMode Data Keeper";
			
			case 11: return "Memory Optimizer";
			
			case LAST: return "Custom Modules";
			
			default: return "Main Settings";
		}
	}
	
	
	
	static string[] HierarchySettingGUI( string searchContex, int ind, bool draw = true )
	{	// if ( HierAdapter == null ) return new string[0];
		if ( HierAdapter == null  ) return new string[0];
		
		return __commonDrawer( HierAdapter, searchContex, ind, draw );
	}
	static string[] ProjectSettingGUI( string searchContex, int ind, bool draw = true )
	{	// if ( HierAdapter == null ) return new string[0];
		if ( ProjAdapter == null ) return new string[0];
		
		return __commonDrawer( ProjAdapter, searchContex, ind, draw, drawAll: false );
	}
	public bool wasInit;
	static   int overrideLastValue;
	static int? overrideIndex;
	static string[] __commonDrawer( Adapter A, string searchContex, int ind, bool draw = true, bool drawAll = false )
	{	if ( A == null ) return new string[0];
	
		if ( !A.wasInit || A.SETUP_TITLEHOGLIGHT == null )
		{	var checkFilesResult = Adapter.CheckFiles(A);
		
			if ( checkFilesResult != null )
			{	if ( !checkFilesResult.EndsWith( ".cs" ) )
				{	A.wasInit = false;
					A.logProxy.Log( "'" + checkFilesResult + "' is Missing" );
				}
				
				else
				{	A.tempAdapterDisableCache = true;
					A.wasInit = true;
				}
			}
			
			else
			{	A.wasInit = true;
			}
			
		}
		
		if ( draw ) A.InitializeStyles();
		
		if ( !A.wasInit || draw && A.SETUP_TITLEHOGLIGHT == null )
		{	if ( draw ) GUILayout.Label( "Please Reinstall Plugin" );
		
			// A.Remove();
			return new string[0];
		}
		
		if ( ind != -1 && string.IsNullOrEmpty( searchContex ) && overrideIndex.HasValue )
		{	if ( overrideLastValue  != ind )
			{	overrideIndex = null;
			}
			
			else
			{	ind = overrideIndex.Value;
			}
		}
		
		if (ind == -1 || !string.IsNullOrEmpty( searchContex ) )
		{	overrideIndex = null;
		}
		
		//if ( draw ) HierAdapter.InitializeStyles();
		
		
		if ( string.IsNullOrEmpty( searchContex ) )
		{	A.DRAW_STACK.ResetStack( draw );
			A.DRAW_STACK.searchString = searchContex;
			
			if ( !drawAll )
			{	if ( ind == -1 )
				{	for ( int i = 0 ; i < LAST ; i++ )
					{	if ( !IndexActionValidate(A, i, searchContex) ) continue;
					
						IndexAction( A, i, searchContex );
					}
				}
				
				else
				{	/* if ( IndexActionValidate( A, ind, searchContex ) )*/ IndexAction( A, ind, searchContex );
				
				}
			}
			
			else
			{	for ( int i = 0 ; i < LAST ; i++ )
				{	if ( !IndexActionValidate( A, i, searchContex ) ) continue;
				
					var header = A.DRAW_STACK.HEADER_SEPARATOR( "" + A.pluginname + " - "  + IndexToString(i) + ":" );
					IndexAction( A, i, searchContex );
					header.skip = false;
				}
			}
			
		}
		
		else
		{
#pragma warning disable
		
			if ( lastIndexMenu != ind || lastSearchMenu != searchContex || A.pluginID != plgIdMenu || true )
			{	plgIdMenu = A.pluginID;
				lastIndexMenu = ind;
				lastSearchMenu = searchContex;
				A.DRAW_STACK.ResetStack( draw );
				A.DRAW_STACK.searchString = searchContex;
				IndexAction( A, ind, searchContex );
				
				//  var header= A.DRAW_STACK.HEADER_SEPARATOR( "Found in other categories:" );
				// var pos = A.DRAW_STACK.Count;
				for ( int i = 0 ; i < LAST ; i++ )
				{	if ( i == ind ) continue;
				
					if ( !IndexActionValidate( A, i, searchContex ) ) continue;
					
					//   A.DRAW_STACK.SPACE();
					var header = A.DRAW_STACK.HEADER_SEPARATOR( "Found in " + A.pluginname + " - "  + IndexToString(i) + ":" );
					var pos = A.DRAW_STACK.Count;
					IndexAction( A, i, searchContex );
					
					if ( A.DRAW_STACK.Count == pos ) header.skip = true;
				}
			}
			
			else
			{	A.DRAW_STACK.SoftResetStack( draw );
			}
			
		}
		
		
		if ( draw ) A.DRAW_STACK.Draw( ind );
		
		if ( draw ) return null;
		
		return A.DRAW_STACK.keywordMemory.Keys.ToArray();
	}
	static int plgIdMenu = -1;
	static int lastIndexMenu = -10;
	static string lastSearchMenu = null;
#pragma warning restore
	
	
	DRW_STC __DRAW_STACK;
	DRW_STC DRAW_STACK
	{	get
		{	if ( __DRAW_STACK == null ) __DRAW_STACK = new DRW_STC();
		
			__DRAW_STACK.A = this;
			//  __DRAW_STACK.S = this;
			return __DRAW_STACK;
		}
	}
	public class DRW_STC {
	
		public int Count = 0;
		
		public Adapter A;
		//  public SETUPROOT S;
		
		//STYLES
		internal GUIStyle GetStyle( string styleName )
		{	GUIStyle s = GUI.skin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
		
			if ( s == null )
			{	//   Debug.LogError( "Missing built-in guistyle " + styleName );
				s = GUI.skin.box;
			}
			
			return s;
		}
#pragma warning disable
		GUIStyle    m_ToggleGroup;
		GUIStyle      m_Tooltip;
		GUIStyle      m_InspectorTitlebar;
		GUIStyle      m_OptionsButtonStyle;
		GUIStyle      m_FoldoutPreDrop ;
		GUIStyle      m_FoldoutHeader;
		GUIStyle      m_FoldoutHeaderIcon;
		GUIStyle      m_ProgressBarBack;
		GUIStyle      m_ProgressBarBar;
		GUIStyle      m_ProgressBarText ;
		GUIStyle      m_AssetLabel;
		GUIStyle      m_HelpBox;
		GUIStyle      m_ViewBg;
#pragma warning restore
		
		bool stylesInit = false;
		void InitStyles()
		{	stylesInit = true;
			m_ToggleGroup = GetStyle( "BoldToggle" );
			m_Tooltip = GetStyle( "Tooltip" );
			m_InspectorTitlebar = GetStyle( "IN Title" );
			m_OptionsButtonStyle = GetStyle( "PaneOptions" );
			m_FoldoutPreDrop = GetStyle( "FoldoutPreDrop" );
			m_FoldoutHeader = GetStyle( "FoldoutHeader" );
			m_FoldoutHeaderIcon = GetStyle( "FoldoutHeaderIcon" );
			m_ProgressBarBack = GetStyle( "ProgressBarBack" );
			m_ProgressBarBar = GetStyle( "ProgressBarBar" );
			m_ProgressBarText = GetStyle( "ProgressBarText" );
			m_AssetLabel = GetStyle( "AssetLabel" );
			m_HelpBox = GetStyle( "HelpBox" );
			m_ViewBg = GetStyle( "TabWindowBackground" );
		}
		//STYLES
		
		
		// DRAWING
		public  string searchString;
		string lastKeywordsText;
		bool DODRAW_SEARCH( string keywords )
		{	if ( !draw && keywords != null && !keywordMemory.ContainsKey( keywords ) ) keywordMemory.Add( keywords, false );
		
			if ( string.IsNullOrEmpty( searchString ) ) return true;
			
			if ( DoKeyWordsFOrGroup ) keyWordkForGroup += keywords;
			
			lastKeywordsText = keywords;
			return DODRAW_SEARCH();
		}
		bool DODRAW_SEARCH()
		{	if ( string.IsNullOrEmpty( searchString ) ) return true;
		
			return !string.IsNullOrEmpty( lastKeywordsText ) && lastKeywordsText.IndexOf( searchString, StringComparison.InvariantCultureIgnoreCase ) >= 0 ||
			       !string.IsNullOrEmpty( keywordsSettedString ) && keywordsSettedString.IndexOf( searchString, StringComparison.InvariantCultureIgnoreCase ) >= 0;
		}
		bool HAS_SEARCH_STRING()
		{	return !string.IsNullOrEmpty( searchString );
		}
		public void SEARCH_SET( string v )
		{	if ( !draw && v != null && !keywordMemory.ContainsKey( v ) ) keywordMemory.Add( v, false );
		
			if ( DoKeyWordsFOrGroup ) keyWordkForGroup += v;
			
			keywordsSettedString = v;
		}
		public void SEARCH_SET_REMOVE(  )
		{	SEARCH_SET( null );
		}
		DRAW_STACK_ITEM empty = new DRAW_STACK_ITEM();
		
		public DRAW_STACK_ITEM ENABLE_SET( FIELD_SETTER setter )
		{	var s = PUSH(STACKTYPE.Enabler);
			s.setter = setter;
			s.setter2 = null;
			return s;
		}
		public DRAW_STACK_ITEM ENABLE_SET( FIELD_SETTER setter, FIELD_SETTER setter2 )
		{	var s = PUSH(STACKTYPE.Enabler);
			s.setter = setter;
			s.setter2 = setter2;
			return s;
		}
		public DRAW_STACK_ITEM ENABLE_RESTORE()
		{	var s = PUSH(STACKTYPE.Enabler);
			s.setter = null;
			return s;
		}
		public DRAW_STACK_ITEM HEADER( string text )
		{	if ( HAS_SEARCH_STRING() ) return empty;
		
			lastKeywordsText = null;
			var s = PUSH(STACKTYPE.Header);
			s.text = text;
			return s;
		}
		public DRAW_STACK_ITEM HEADER_SEPARATOR( string text )
		{	//  if ( HAS_SEARCH_STRING() ) return empty;
			var s = PUSH(STACKTYPE.HeaderSeparator);
			s.text = text;
			s.skip = false;
			return s;
		}
		
		public DRAW_STACK_ITEM LABEL( string text )
		{	if ( !DODRAW_SEARCH( text ) ) return empty;
		
			var s = PUSH(STACKTYPE.Label);
			s.text = text;
			return s;
		}
		
		public DRAW_STACK_ITEM TOGGLE( string text, FIELD_SETTER setter )
		{	if ( !DODRAW_SEARCH( text ) ) return empty;
		
			var s = PUSH(STACKTYPE.Toggle);
			s.text = text;
			s.setter = setter;
			return s;
		}
		public DRAW_STACK_ITEM SPACE( float? space = null )
		{	if ( HAS_SEARCH_STRING() ) return empty;
		
			var s = PUSH(STACKTYPE.Space);
			s.offset = space ?? EditorGUIUtility.singleLineHeight;
			return s;
		}
		
		public DRAW_STACK_ITEM ACTION( Action action, bool skipSearch )
		{	if ( skipSearch && HAS_SEARCH_STRING() ) return empty;
		
			if ( !DODRAW_SEARCH() ) return empty;
			
			var s = PUSH(STACKTYPE.Action);
			s.action = action;
			return s;
		}
		
		public DRAW_STACK_ITEM TOGGLE_FROM_INT( string text, FIELD_SETTER setter, int enable, int disable )
		{	if ( !DODRAW_SEARCH( text ) ) return empty;
		
			var s = PUSH(STACKTYPE.ToggleFromInt);
			s.text = text;
			s.setter = setter;
			s.X = enable;
			s.Y = disable;
			return s;
		}
		
		bool DoKeyWordsFOrGroup;
		string keyWordkForGroup;
		public DRAW_STACK_ITEM BEGIN_GROUP()
		{	if ( DoKeyWordsFOrGroup ) throw new Exception( "GroupingGroup" );
		
			DoKeyWordsFOrGroup = true;
			keyWordkForGroup = "";
			// if ( HAS_SEARCH_STRING() ) return empty;
			var s = PUSH(STACKTYPE.BeginGroup);
			s.X = writingGroupPos;
			return s;
		}
		public DRAW_STACK_ITEM END_GROUP()
		{	//  if ( HAS_SEARCH_STRING() ) return empty;
			DoKeyWordsFOrGroup = false;
			
			if ( writingGroupPos >= groupKeyWords.Count ) groupKeyWords.Add( "" );
			
			groupKeyWords[writingGroupPos] = keyWordkForGroup;
			//                 if ( !DODRAW_SEARCH( keyWordkForGroup ) ) {
			//                     var pop = POP();
			//                     Debug.Log( pop.type );
			//                     return empty;
			//                 } else {
			var s = PUSH( STACKTYPE.EndGroup );
			s.X = writingGroupPos;
			writingGroupPos++;
			return s;
			//}
		}
		public DRAW_STACK_ITEM TOGGLE_GROUP( string text, FIELD_SETTER setter )
		{	if ( !DODRAW_SEARCH( text ) ) return empty;
		
			var s = PUSH(STACKTYPE.ToggleGroup);
			s.text = text;
			s.setter = setter;
			return s;
		}
		
		public DRAW_STACK_ITEM TOOLBAR( string[] textArray, FIELD_SETTER setter, int? offset = null )
		{	if ( !DODRAW_SEARCH() ) return empty;
		
			var s = PUSH(STACKTYPE.Toolbar);
			s.textArray = textArray;
			s.setter = setter;
			s.offset = offset ?? 0;
			return s;
		}
		public DRAW_STACK_ITEM TOOLBAR_FOR_BOOL( string[] textArray, FIELD_SETTER setter )
		{	if ( !DODRAW_SEARCH() ) return empty;
		
			var s = PUSH(STACKTYPE.ToolbarForBool);
			s.textArray = textArray;
			s.setter = setter;
			return s;
		}
		static Type intType = typeof(int);
		static Type intNType = typeof(int? );
		static Type floatNType = typeof(float? );
		//GUIContent tempContent = new GUIContent();
		public DRAW_STACK_ITEM SLIDER( string text, float min, float max, FIELD_SETTER setter, float offset = 0 )
		{	if ( !DODRAW_SEARCH( text ) ) return empty;
		
			var t = setter.isprop ? setter.prop.PropertyType : setter.field.FieldType;
			var isInt = t == intType || t == intNType;
			var s = PUSH( isInt ? STACKTYPE.SliderInt : STACKTYPE.SliderFloat);
			s.IsNullable = t == intNType || t == floatNType;
			s.text = text;
			s.setter = setter;
			s.min = min;
			s.max = max;
			s.offset = offset;
			return s;
		}
		public void HR()
		{	if ( HAS_SEARCH_STRING() ) return;
		
			PUSH( STACKTYPE.HR );
		}
		
		public DRAW_STACK_ITEM COLOR_PICKER( string text, FIELD_SETTER setter )
		{	if ( text == null ? !DODRAW_SEARCH() : !DODRAW_SEARCH( text ) ) return empty;
		
			var s = PUSH(STACKTYPE.ColorPicker);
			s.UseLastRect = false;
			s.text = text;
			s.setter = setter;
			return s;
		}
		public DRAW_STACK_ITEM COLOR_PICKER_LASTRECT( FIELD_SETTER setter )
		{	if ( !DODRAW_SEARCH() ) return empty;
		
			var s = PUSH(STACKTYPE.ColorPicker);
			s.UseLastRect = true;
			s.text = null;
			s.setter = setter;
			return s;
		}
		public DRAW_STACK_ITEM HELP_TEXTURE( string textire, int? height = null, int? xOffset = null, int? yOffset = null /*, string searchString = null */)
		{	// if ( !string.IsNullOrEmpty( searchString ) ? !IS_SEARCH( searchString ) : !IS_SEARCH() ) return empty;
			if ( HAS_SEARCH_STRING() ) return empty;
			
			// if ( !DODRAW_SEARCH() ) return empty;
			var s = PUSH(STACKTYPE.HelpTexture);
			s.text = textire;
			s.height = height;
			s.icon = null;
			s.X = xOffset;
			s.Y = yOffset;
			s.offset = 0;
			return s;
		}
		public DRAW_STACK_ITEM HELP_TEXTURE_WIDE( string textire, int? height = null, int? xOffset = null, int? yOffset = null /*, string searchString = null */)
		{	// if ( !string.IsNullOrEmpty( searchString ) ? !IS_SEARCH( searchString ) : !IS_SEARCH() ) return empty;
			if ( !DODRAW_SEARCH() ) return empty;
			
			var s = PUSH(STACKTYPE.HelpTexture);
			s.text = textire;
			s.height = height;
			s.icon = null;
			s.X = xOffset;
			s.Y = yOffset;
			s.offset = 1;
			return s;
		}
		public DRAW_STACK_ITEM HELP_TEXTURE_WIDE( Texture textire, int? height = null, int? xOffset = null, int? yOffset = null /*, string searchString = null */)
		{	// if ( !string.IsNullOrEmpty( searchString ) ? !IS_SEARCH( searchString ) : !IS_SEARCH() ) return empty;
			if ( !DODRAW_SEARCH() ) return empty;
			
			var s = PUSH(STACKTYPE.HelpTexture);
			s.text = null;
			s.height = height;
			s.icon = textire;
			s.X = xOffset;
			s.Y = yOffset;
			s.offset = 1;
			return s;
		}
		public DRAW_STACK_ITEM PADDING_SET( float v )
		{	if ( !DODRAW_SEARCH() ) return empty;
		
			var s = PUSH(STACKTYPE.PaddingSet);
			s.offset = v;
			return s;
		}
		public DRAW_STACK_ITEM PADDING_RIGHT_HARDSET( float v )
		{	var s = PUSH(STACKTYPE.PaddingRightHardSet);
			s.offset = v;
			return s;
		}
		
		public DRAW_STACK_ITEM VAR_FIELD( string text, float min, float max, FIELD_SETTER setter, string posfix = null )
		{	return VAR_FIELD( text, setter, min, max );
		}
		public DRAW_STACK_ITEM VAR_FIELD( string text, FIELD_SETTER setter, float min, float max, string posfix = null )
		{	if ( !DODRAW_SEARCH( text ) ) return empty;
		
			var t = setter.isprop ? setter.prop.PropertyType : setter.field.FieldType;
			var isInt = t == intType || t == intNType;
			var s = PUSH( isInt ? STACKTYPE.IntField : STACKTYPE.FloatField);
			s.IsNullable = t == intNType || t == floatNType;
			s.text = text;
			s.setter = setter;
			s.min = min;
			s.max = max;
			s.str = posfix;
			return s;
		}
		public DRAW_STACK_ITEM ICON( Texture icon, FIELD_SETTER setter )
		{	if ( !DODRAW_SEARCH() ) return empty;
		
			var s = PUSH(STACKTYPE.Icon);
			s.icon = icon;
			s.setter = setter;
			return s;
		}
		public DRAW_STACK_ITEM ICON( string str, FIELD_SETTER setter )
		{	if ( !DODRAW_SEARCH() ) return empty;
		
			var s = PUSH(STACKTYPE.Icon);
			s.icon = null;
			s.str = str;
			s.setter = setter;
			return s;
		}
		public DRAW_STACK_ITEM DRAW_WIKI_BUTTON( string category, string note = null )
		{	if ( HAS_SEARCH_STRING() ) return empty;
		
			var s = PUSH(STACKTYPE.Tutorial);
			s.str = category;
			s.text = note;
			return s;
		}
		public DRAW_STACK_ITEM TOOLTIP( string str )
		{	if ( HAS_SEARCH_STRING() ) return empty;
		
			var s = PUSH(STACKTYPE.ToolTip);
			s.text = str;
			return s;
		}
		public DRAW_STACK_ITEM HELP_BOX( string text, MessageType type = MessageType.None )
		{	if ( HAS_SEARCH_STRING() ) return empty;
		
			var s = PUSH(STACKTYPE.HelpBox);
			s.text = text;
			s.messageType = type;
			return s;
		}
		public DRAW_STACK_ITEM BUTTON( string textArray, Action buttonActionArray, int? height )
		{	return BUTTON( new[] { textArray }, new[] { buttonActionArray }, height );
		}
		public DRAW_STACK_ITEM BUTTON( string[] textArray, Action[] buttonActionArray, int? height, bool[] higlights = null )
		{	if ( HAS_SEARCH_STRING() ) return empty;
		
			var s = PUSH(STACKTYPE.Buttons);
			s.X = textArray.Length;
			s.Y = height;
			s.textArray = textArray;
			s.actionArray = buttonActionArray;
			s.boolArray = higlights;
			return s;
		}
		
		int currentStackPos = -1;
		List<DRAW_STACK_ITEM> stack = new List<DRAW_STACK_ITEM>();
		public DRAW_STACK_ITEM PUSH( STACKTYPE type )
		{	if ( type != STACKTYPE.BeginGroup &&
			        type != STACKTYPE.EndGroup &&
			        type != STACKTYPE.Enabler &&
			        type != STACKTYPE.PaddingSet && type != STACKTYPE.PaddingRightHardSet ) Count++;
			        
			currentStackPos++;
			
			if ( currentStackPos >= stack.Count ) stack.Add( new DRAW_STACK_ITEM() );
			
			stack[currentStackPos].Clear( type );
			return stack[currentStackPos];
		}
		public DRAW_STACK_ITEM POP()
		{	var res = stack[currentStackPos];
			currentStackPos--;
			return res;
		}
		
		List<string> groupKeyWords = new List<string>();
		int writingGroupPos;
		bool draw;
		public void ResetStack( bool draw )
		{	Count = 0;
			currentStackPos = -1;
			
			if ( keywordMemory.Count != 0 ) keywordMemory.Clear();
			
			SoftResetStack( draw );
		}
		public void SoftResetStack( bool draw )
		{	this.draw = draw;
			writingGroupPos = 0;
			DoKeyWordsFOrGroup = false;
			enableStack.Clear();
			paddingValue = 0;
			rightPaddingValue = 0;
			keywordsSettedString = null;
		}
		internal  Dictionary<string, bool> keywordMemory = new Dictionary<string, bool>();
		List<bool> enableStack = new List<bool>();
		float paddingValue = 0;
		float rightPaddingValue = 0;
		string keywordsSettedString = null;
		internal void ValueChanged()
		{	A.SavePrefs();
			A.RESET_DRAW_STACKS();
			A.CLearAdditionalCache();
			//  A.RepaintWindowInUpdate();
			UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
		}
		float ASD = 15;
		bool groupBegin = false;
		Rect GetControlRect()
		{	var R = EditorGUILayout.GetControlRect();
			R.x += paddingValue;
			R.width -= paddingValue * 2;
			R.width -= rightPaddingValue;
			lastRect = R;
			
			if ( groupBegin )
			{	// GUI.Label( R , "" , m_FoldoutHeader );
				groupBegin = false;
			}
			
			return R;
		}
		Rect GetControlRect( float height )
		{	var R = EditorGUILayout.GetControlRect( GUILayout.Height( height ) );
			R.x += paddingValue;
			R.width -= paddingValue * 2;
			R.width -= rightPaddingValue;
			lastRect = R;
			return R;
		}
		Rect lastRect;
		void OnEnableChange()
		{	A.OnEnableChange();
		}
		GUIStyle buttonStyle;
		GUIStyle offerStyleA, offerStyleB;
		Texture hierarchyoffertextureA, hierarchyoffertextureB;
		public void Draw( int drawIndex )
		{
		
		
		
		
		
			if ( !HAS_SEARCH_STRING() && drawIndex <= 0 )
			{
			
				var logo =  A.GetIcon("LOGO");
				
				if ( A.IS_PROJECT() ) logo = A.GetIcon( "LOGO PROJECT" );
				
				var R = GetControlRect( 20  );
				var LOGO_R = R;
				//LOGO_R.x = (LOGO_R.width - 384) / 2;
				// LOGO_R.width = 384;
				LOGO_R.x = LOGO_R.width - logo.width + 80;
				GUI.BeginClip( LOGO_R );
				LOGO_R.x = 0;
				LOGO_R.y = -6;
				LOGO_R.width = logo.width;
				LOGO_R.height = logo.height;
				GUI.DrawTexture( LOGO_R, logo, ScaleMode.ScaleToFit );
				GUI.EndClip();
				
				/* var asd = GUI.color;
				 LOGO_R.y = LOGO_R.height + LOGO_R.y - EditorGUIUtility.singleLineHeight;
				 LOGO_R.height = EditorGUIUtility.singleLineHeight;
				 GUI.color *= new Color( 1 , 1 , 1 , 0.3f );
				 GUI.Label( LOGO_R , Adapter.HIERARCHY_VERSION );
				 GUI.color = asd;*/
				
				
				R.height = EditorGUIUtility.singleLineHeight;
				// R.x = R.width - 150;
				var oldval = A.TOGGLE_LEFT(R, " ☰ Use " + A.pluginname + " Plugin " + Adapter.HIERARCHY_VERSION + " ☰", A.par.ENABLE_ALL, defaultStyle: null);
				
				if ( oldval != A.par.ENABLE_ALL )
				{	A.par.ENABLE_ALL = oldval;
					ValueChanged();
					OnEnableChange();
				}
				
				GUILayout.Space( 6 );
				
				R = GetControlRect();
				var R2 = GetControlRect();
				R.height = R2.y + R2.height - R.y;
				
				if ( GUI.Button( R, "Open Welcome Screen" ) )
				{	WelcomeScreen.Init( prefWindow.position );
				}
				
				EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
				GUILayout.Space( 6 );
				
				R = GetControlRect(   );
				
				if ( buttonStyle  == null )
				{	buttonStyle = new GUIStyle( GUI.skin.button );
					buttonStyle.alignment = TextAnchor.MiddleLeft;
				}
				
				
				// if ( GUI.Button( R, "OFFER FOR FANS - https://emem.store/faq", buttonStyle ) )
				if ( GUI.Button( R, "Help to improve the asset - https://emem.store/faq", buttonStyle ) )
				{	Application.OpenURL( "https://emem.store/faq" );
				}
				
				EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
				/*  R = GetControlRect();
				  GUI.Label( R, "If you like the asset you can take part in the draw of 5 vouchers" );*/
				R = GetControlRect();
				R.y += R.height;
				
				
				
				
				R = GetControlRect();
				
				A.HR( R );
				/*if (!hierarchyoffertextureA )
				{   hierarchyoffertextureA =   AssetDatabase.LoadAssetAtPath<Texture>( A.PluginInternalFolder + "/Documentations/Hierarchy Offer A.png" );
				    hierarchyoffertextureB = AssetDatabase.LoadAssetAtPath<Texture>( A.PluginInternalFolder + "/Documentations/Hierarchy Offer B.png" );
				}
				if ( hierarchyoffertextureA )
				{   if ( offerStyleA == null )
				    {   offerStyleA = new GUIStyle();
				        offerStyleA.border = new RectOffset( 10, hierarchyoffertextureA .width - 10, 0, 0 );
				        offerStyleA.normal.background = hierarchyoffertextureA as Texture2D;
				        offerStyleB = new GUIStyle();
				        offerStyleB.border = new RectOffset( hierarchyoffertextureB .width - 12, 12, 0, 0 );
				        offerStyleB.normal.background = hierarchyoffertextureB as Texture2D;
				    }
				    var R2 = GetControlRect( 65 );
				    if ( Event.current.type == EventType.Repaint )
				    {   R2.width /= 2;
				        offerStyleA.Draw( R2, "", false, false, false, false );
				        R2.x += R2.width;
				        offerStyleB.Draw( R2, "", false, false, false, false );
				    }
				}*/
			}
			
			
			
			
			GUILayout.BeginHorizontal();
			GUILayout.Space( ASD );
			GUILayout.BeginVertical();
			
			
			if ( !stylesInit ) InitStyles();
			
			for ( int i = 0 ; i <= currentStackPos ; i++ )
			{	switch ( stack[i].type )
				{	case STACKTYPE.Header:
					{	var R = GetControlRect((EditorGUIUtility.singleLineHeight * 1.2f));
						// Adapter.LABEL( R , stack[i].text , m_ProgressBarBar );
						//Adapter.LABEL( R , stack[i].text , m_InspectorTitlebar );
						//  GUILayout.Label( stack[i].text , m_ProgressBarBar );
						Adapter.LABEL( R, stack[i].text, m_ProgressBarBack );
						
						
						break;
					}
					
					case STACKTYPE.HeaderSeparator:
					{	if ( !stack[i].skip )
						{	//   EditorGUILayout.GetControlRect( GUILayout.Height( height ) );
							EditorGUILayout.GetControlRect( GUILayout.Height( EditorGUIUtility.singleLineHeight ) );
							EditorGUILayout.GetControlRect( GUILayout.Height( EditorGUIUtility.singleLineHeight ) );
							var R = EditorGUILayout.GetControlRect(GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f));
							// Adapter.LABEL( R , stack[i].text , m_ProgressBarBar );
							//Adapter.LABEL( R , stack[i].text , m_InspectorTitlebar );
							//  GUILayout.Label( stack[i].text , m_ProgressBarBar );
							Adapter.LABEL( R, stack[i].text, m_ProgressBarBar );
							/*GUI.BeginClip( R );
							                               R.x = 0;
							                       R.y = 0;
							                       Adapter.LABEL( R, " ", m_InspectorTitlebar );
							GUI.EndClip(); */
						}
						
						
						
						break;
					}
					
					case STACKTYPE.Label:
					{	var R = GetControlRect();
						Adapter.LABEL( R, stack[i].text );
						break;
					}
					
					case STACKTYPE.Toggle:
					{	var R = GetControlRect();
						var b = (bool)stack[i].setter.value;
						var newb = A.TOGGLE_LEFT( R, TOGGLE_PTR + stack[i].text, b );
						
						if ( b != newb )
						{	if ( stack[i].conform == null || stack[i].conform( newb ) )
							{	stack[i].setter.value = newb;
								ValueChanged();
							}
						}
						
						break;
					}
					
					case STACKTYPE.ToggleFromInt:
					{	var R = GetControlRect();
						var b = (int)stack[i].setter.value == stack[i].X.Value;
						var newb = A.TOGGLE_LEFT(R,  TOGGLE_PTR + stack[i].text, b );
						
						if ( b != newb )
						{	ValueChanged();
							var res = newb ? stack[i].X.Value : stack[i].Y.Value;
							
							if ( stack[i].conform == null || stack[i].conform( res ) )
							{	stack[i].setter.value = res;
								ValueChanged();
							}
						}
						
						break;
					}
					
					case STACKTYPE.ToggleGroup:
					{	var R = GetControlRect();
					
						if (UNITY_CURRENT_VERSION < UNITY_2019_3_0_VERSION)  GUI.Label( R, "", m_FoldoutHeader );
						
						//  GUI.Label( R , "" , m_ProgressBarBack );
						var b = (bool)stack[i].setter.value;
						var newb = A.TOGGLE_LEFT(R, TOGGLE_PTR + stack[i].text, b );
						
						if ( b != newb )
						{	if ( stack[i].conform == null || stack[i].conform( newb ) )
							{	stack[i].setter.value = newb;
								ValueChanged();
							}
						}
						
						break;
					}
					
					case STACKTYPE.SliderFloat:
					{	var R = GetControlRect();
						var b = (stack[i].IsNullable ? ((float ? )stack[i].setter.value).Value : (float)stack[i].setter.value) + stack[i].offset;
						var newb = ( A.S_SLIDER( R, TOGGLE_PTR + stack[i].text, b, stack[i].min, stack[i].max ) );
						
						if ( b != newb )
						{	if ( stack[i].IsNullable ) stack[i].setter.value = (float? )(newb - stack[i].offset);
							else stack[i].setter.value = newb - stack[i].offset;
							
							ValueChanged();
						}
						
						break;
					}
					
					case STACKTYPE.SliderInt:
					{	var R = GetControlRect();
						var b = (stack[i].IsNullable ? ((int ? )stack[i].setter.value).Value : (int)stack[i].setter.value) +  (int)stack[i].offset;
						var newb = ( A.S_SLIDER( R, TOGGLE_PTR + stack[i].text, b, (int)stack[i].min, (int)stack[i].max ) );
						
						if ( b != newb )
						{	if ( stack[i].IsNullable ) stack[i].setter.value = (int? )(newb - (int)stack[i].offset);
							else stack[i].setter.value = newb - (int)stack[i].offset;
							
							ValueChanged();
						}
						
						break;
					}
					
					case STACKTYPE.HR:
					{	/*  GUILayout.Space( 10 );
						var  R = EditorGUILayout.GetControlRect( GUILayout.Height(1) );
						Adapter.INTERNAL_BOX( R );
						GUILayout.Space( 10 );*/
						GUILayout.Space( 3 );
						var R = EditorGUILayout.GetControlRect(GUILayout.Height(3));
						var HRP = ASD;
						HRP -= R.width / 4;
						R.x -= (HRP - 5);
						R.width += (HRP - 5) * 2;
						// Adapter.LABEL( R , stack[i].text , m_ProgressBarBar );
						GUI.BeginClip( R );
						R.x = 0;
						R.y = 0;
						Adapter.LABEL( R, " ", m_InspectorTitlebar );
						GUI.EndClip();
						
						break;
					}
					
					case STACKTYPE.ColorPicker:
					{	var R = stack[i].UseLastRect ? lastRect : GetControlRect();
					
						if ( stack[i].UseLastRect ) R.width += 90;
						
						R.width -= 85;
						
						if ( stack[i].text != null ) Adapter.LABEL( R, TOGGLE_PTR + stack[i].text );
						
						var b = (Color)stack[i].setter.value ;
						var newb = M_Colors_Window.PICKER(new Rect(R.x + R.width, R.y - 3, 85, 23), GUIContent.none, b);
						
						if ( b != newb )
						{	stack[i].setter.value = newb;
							ValueChanged();
						}
						
						break;
					}
					
					case STACKTYPE.HelpTexture:
					{
					
						if ( stack[i].icon ) A.S_DRAW_HELP_TEXTURE( stack[i].icon, height: stack[i].height, xOffset: stack[i].X ?? 0, yOffset : stack[i].Y, bgstyle : m_ProgressBarBack,
							        bgStyleW : stack[i].offset == 1 );   //m_ProgressBarBack  m_Tooltip
						else A.S_DRAW_HELP_TEXTURE( stack[i].text, height: stack[i].height, xOffset: stack[i].X ?? 0, yOffset : stack[i].Y, bgstyle : m_ProgressBarBack,
							                            bgStyleW : stack[i].offset == 1 );   //m_ProgressBarBack  m_Tooltip
							                            
						lastRect = GUILayoutUtility.GetLastRect();
						
						break;
					}
					
					case STACKTYPE.FloatField:
					{	var R = GetControlRect();
						var _r = R;
						R.width /= 1.5f;
						Adapter.LABEL( R, TOGGLE_PTR + stack[i].text );
						R.x += R.width;
						R.width = _r.width - R.width;
						var v =  (stack[i].IsNullable ? ((float ? )stack[i].setter.value).Value : (float)stack[i].setter.value) ;
						var newv = Mathf.Clamp (A.S_FLOAT_FIELD( R, v, stack[i].str), stack[i].min, stack[i].max);
						
						if ( v != newv )
						{	if ( stack[i].IsNullable ) stack[i].setter.value = (float? )(newv);
							else stack[i].setter.value = newv;
							
							ValueChanged();
						}
						
						break;
					}
					
					case STACKTYPE.IntField:
					{	var R = GetControlRect();
						var _r = R;
						R.width /= 1.5f;
						Adapter.LABEL( R, TOGGLE_PTR + stack[i].text );
						R.x += R.width;
						R.width = _r.width - R.width;
						
						var v = (stack[i].IsNullable ? ((int ? )stack[i].setter.value).Value : (int)stack[i].setter.value) ;
						var newv = Mathf.Clamp (A.S_INT_FIELD( R, v, stack[i].str ), (int)stack[i].min, (int)stack[i].max);
						
						if ( v != newv )
						{	if ( stack[i].IsNullable ) stack[i].setter.value = (int? )(newv);
							else stack[i].setter.value = newv;
							
							ValueChanged();
						}
						
						break;
					}
					
					case STACKTYPE.ToolTip:
					{	if ( !HAS_SEARCH_STRING() )
						{	var R = GetControlRect();
							R.x += EditorGUIUtility.singleLineHeight;
							R.width -= EditorGUIUtility.singleLineHeight * 2;
							__TOOLTIP.text = stack[i].text;
							__TOOLTIP.tooltip = stack[i].text;
							GUI.Label( R, __TOOLTIP, m_AssetLabel );
						}
						
						break;
					}
					
					case STACKTYPE.Buttons:
					{	var R = stack[i].Y.HasValue ? GetControlRect(stack[i].Y.Value) : GetControlRect();
						R.width /= stack[i].X.Value;
						
						for ( int asd = 0 ; asd < stack[i].X.Value ; asd++ )
						{	if ( GUI.Button( R, stack[i].textArray[asd] ) ) stack[i].actionArray[asd]();
						
							if ( stack[i].boolArray != null && stack[i].boolArray[asd] )
							{	GUI.DrawTexture( R, A.GetIcon( "BUTBLUE" ) );
							}
							
							R.x += R.width;
						}
						
						break;
					}
					
					case STACKTYPE.Tutorial:
					{	var r = lastRect;
						// r.y -= EditorGUIUtility.singleLineHeight;
						r.height = EditorGUIUtility.singleLineHeight;
						A.__DRAW_WIKI_BUTTON( r, stack[i].str, stack[i].text );
						break;
					}
					
					case STACKTYPE.Enabler:
						if ( stack[i].setter == null )
						{	GUI.enabled = enableStack[enableStack.Count - 1];
							enableStack.RemoveAt( enableStack.Count - 1 );
						}
						
						else
						{	enableStack.Add( GUI.enabled );
							var t = stack[i].setter.isprop ? stack[i].setter.prop.PropertyType : stack[i].setter.field.FieldType;
							bool res = false;
							
							if ( t == typeof( bool ) )
							{	res |= (bool)stack[i].setter.value;
							
								if ( stack[i].setter2 != null ) res |= (bool)stack[i].setter2.value;
							}
							
							else
								if ( t == typeof( bool? ) )
								{	res |= (bool? )stack[i].setter.value ?? true;
								
									if ( stack[i].setter2 != null ) res |= (bool? )stack[i].setter2.value ?? true;
								}
								
								else
									if ( t == typeof( int ) )
									{	res |= (int)stack[i].setter.value == 1;
									
										if ( stack[i].setter2 != null ) res |= (int)stack[i].setter2.value == 1;
									}
									
									else
										if ( t == typeof( int? ) )
										{	res |= ((int? )stack[i].setter.value ?? 0) == 1;
										
											if ( stack[i].setter2 != null ) res |= ((int? )stack[i].setter2.value ?? 0) == 1;
										}
										
							GUI.enabled &= res;
							
							
						}
						
						break;
						
					case STACKTYPE.Toolbar:
					{	var b = (int)stack[i].setter.value + (int)stack[i].offset;
						var R = GetControlRect();
						var newb = GUI.Toolbar(R, b, stack[i].textArray, EditorStyles.miniButton) ;
						EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
						
						if ( b != newb )
						{	var res = newb - (int)stack[i].offset;
						
							if ( stack[i].conform == null || stack[i].conform( res ) )
							{	stack[i].setter.value = res;
								ValueChanged();
							}
						}
						
						break;
					}//EditorStyles.toolbarButton
					
					case STACKTYPE.ToolbarForBool:
					{	var b = (bool)stack[i].setter.value;
						var R = GetControlRect();
						var newb = GUI.Toolbar(R, b ? 1 : 0, stack[i].textArray, EditorStyles.miniButton) == 1;
						EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
						
						if ( b != newb )
						{	if ( stack[i].conform == null || stack[i].conform( newb ) )
							{	stack[i].setter.value = newb;
								ValueChanged();
							}
						}
						
						break;
					}
					
					case STACKTYPE.PaddingSet:
					{	paddingValue = stack[i].offset;
						break;
					}
					
					case STACKTYPE.PaddingRightHardSet:
					{	rightPaddingValue = stack[i].offset;
						break;
					}
					
					case STACKTYPE.Space:
					{	GUILayout.Space( stack[i].offset );
						break;
					}
					
					case STACKTYPE.BeginGroup:
					{	if ( DODRAW_SEARCH( groupKeyWords[stack[i].X.Value] ) )
						{	var c = GUI.color;
							GUI.color *= new Color( 1, 1, 1, 0.5f );
							GUILayout.BeginVertical( m_Tooltip ); //m_ProgressBarBack  m_Tooltip
							GUI.color = c;
							groupBegin = true;
						}
						
						//  EditorGUILayout.BeginFoldoutHeaderGroup( true,"", EditorStyles.helpBox );
						break;
					}
					
					case STACKTYPE.EndGroup:
					{	if ( DODRAW_SEARCH( groupKeyWords[stack[i].X.Value] ) )
						{	GUILayout.EndVertical();
						}
						
						if ( !HAS_SEARCH_STRING() )
						{	GUILayout.Space( 3 );
							var R = EditorGUILayout.GetControlRect(GUILayout.Height(3));
							R.x -= (ASD - 5);
							R.width += (ASD - 5) * 2;
							// Adapter.LABEL( R , stack[i].text , m_ProgressBarBar );
							
							GUI.BeginClip( R );
							R.x = 0;
							R.y = 0;
							Adapter.LABEL( R, " ", m_InspectorTitlebar );
							GUI.EndClip();
						}
						
						
						//  EditorGUILayout.EndFoldoutHeaderGroup();
						break;
					}
					
					case STACKTYPE.Action:
					{	if ( paddingValue != 0 )
						{	GUILayout.BeginHorizontal();
							GUILayout.Space( paddingValue );
							GUILayout.BeginVertical();
						}
						
						stack[i].action();
						
						if ( paddingValue != 0 )
						{	GUILayout.EndVertical();
							GUILayout.Space( paddingValue );
							GUILayout.EndHorizontal();
						}
						
						break;
					}
					
					case STACKTYPE.Icon:
					{	var R = lastRect;
						R.x = R.width + R.x - EditorGUIUtility.singleLineHeight;
						
						if ( stack[i].icon ) A.DRAW_PRE_ICON( ref R, stack[i].icon, (stack[i].setter == null || (bool)stack[i].setter.value) && GUI.enabled );
						else A.DRAW_PRE_ICON( ref R, stack[i].str, (stack[i].setter == null || (bool)stack[i].setter.value) && GUI.enabled );
						
						break;
					}
					
					case STACKTYPE.HelpBox:
					{	if ( !HAS_SEARCH_STRING() )
						{
						
						
							var text = "☢ " + stack[i].text;
							var type = stack[i].messageType;
							
							/* var rect = GetControlRect( 0 );
							 tempContent.text = text;
							 var h = EditorStyles.helpBox.CalcHeight( tempContent, rect.width - (type == MessageType.None ? 0 : 40) );
							 if ( type != MessageType.None && h < 32 ) h = 32;
							 rect = GetControlRect( h );*/
							if ( paddingValue != 0 )
							{	GUILayout.BeginHorizontal();
								GUILayout.Space( paddingValue );
								GUILayout.BeginVertical();
							}
							
							var  r = EditorStyles.helpBox.richText;
							EditorStyles.helpBox.richText = true;
							EditorGUILayout.HelpBox( text, type );
							EditorStyles.helpBox.richText = r;
							
							if ( paddingValue != 0 )
							{	GUILayout.EndVertical();
								GUILayout.Space( paddingValue );
								GUILayout.EndHorizontal();
							}
						}
						
						break;
					}
					
						//                         case STACKTYPE.SearchSet: {
						//                             paddingValue = stack[i].offset;
						//                             break;
						//                         }
				}
				
				
				if ( !string.IsNullOrEmpty( stack[i].tip ) )
				{	/* __TOOLTIP.tooltip = stack[i].tip;
					 GUI.Label( GUILayoutUtility.GetLastRect() , __TOOLTIP );*/
					
					var R = GetControlRect();
					R.x += EditorGUIUtility.singleLineHeight;
					R.width -= EditorGUIUtility.singleLineHeight * 2;
					var t = stack[i].tip;
					t = t.Trim( '.' ) + '.';
					__TOOLTIP.tooltip = __TOOLTIP.text = t;
					GUI.Label( R, __TOOLTIP, m_AssetLabel );
					
				}
				
			}
			
			
			/*  GUILayout.Box( "2" , m_Tooltip );
			  GUILayout.Box( "2" , m_Tooltip );
			  GUILayout.Label( "6" , m_FoldoutHeader );
			  GUILayout.Label( "Additional" , m_FoldoutHeaderIcon );
			  GUILayout.Label( "8" , m_ProgressBarBack );
			  // GUILayout.Label( "0" , m_ProgressBarText );
			  GUILayout.Label( "12" , m_AssetLabel );
			  GUILayout.Label( "14" , m_HelpBox );*/
			
			
			GUILayout.EndVertical();
			GUILayout.Space( ASD );
			GUILayout.EndHorizontal();
		}
		// DRAWING
	}
	
	
	public enum STACKTYPE { Empty, HelpBox, ToolTip, Icon, Action, BeginGroup, EndGroup, Header, HeaderSeparator, Label, Toggle, ToggleFromInt, ToggleGroup, FloatField, IntField, Buttons, Tutorial, Enabler, Toolbar, ToolbarForBool, SliderInt, SliderFloat, HR, ColorPicker, HelpTexture, PaddingRightHardSet, PaddingSet, Space/*, SearchSet*/ }
	public class DRAW_STACK_ITEM {
	
		public DRAW_STACK_ITEM TOOLTIP( string tip )
		{	if ( type == STACKTYPE.Empty ) return this;
		
			this.tip = tip;
			return this;
		}
		public DRAW_STACK_ITEM ON_CHANGE_CONFORM( Func<object, bool> conform )
		{	if ( type != STACKTYPE.Toolbar && type != STACKTYPE.ToolbarForBool && type != STACKTYPE.ToggleGroup && type != STACKTYPE.Toggle && type != STACKTYPE.ToggleFromInt )
			{	if ( type == STACKTYPE.Empty ) return this;
			
				throw new Exception( "ON_CHANGE_CONFORM" );
			}
			
			this.conform = conform;
			return this;
		}
		public void Clear( STACKTYPE type )
		{	if ( tip != "" ) tip = "";
		
			this.type = type;
			
			if ( conform != null ) conform = null;
		}
		public bool UseLastRect;
		public STACKTYPE type = STACKTYPE.Empty;
		public string str;
		public string text;
		public MessageType messageType;
		public string tip;
		public string[] textArray;
		public  FIELD_SETTER setter;
		public  FIELD_SETTER setter2;
		public float min, max, offset;
		public int? height, X, Y;
		public bool IsNullable;
		public Action action;
		public Action[] actionArray;
		public Func<object, bool> conform;
		public bool[] boolArray;
		public bool skip;
		public Texture icon;
	}
	
	
	public class FIELD_SETTER {
		public   bool isprop;
		public FieldInfo field;
		public PropertyInfo prop;
		public Adapter A;
		public bool UsePar;
		public Action onChange;
		
		// object cachedValue
		public object value
		{	get
			{	if ( UsePar ) return isprop ? prop.GetValue( A.par, null ) : field.GetValue( A.par );
				else return isprop ? prop.GetValue( A, null ) : field.GetValue( A );
			}
			
			set
			{	if ( this.value == value ) return;
			
				if ( UsePar )
				{	object t = A.par;
				
					if ( isprop ) prop.SetValue( t, value, null );
					else field.SetValue( t, value );
					
					A.par = (HierParams)t;
				}
				
				else
				{	if ( isprop ) prop.SetValue( A, value, null );
					else field.SetValue( A, value );
				}
				
				if ( onChange != null ) onChange();
			}
		}
	}
	Dictionary<string, FIELD_SETTER> _setterCache = new Dictionary<string, FIELD_SETTER>();
	FIELD_SETTER GetSetter( string name, Adapter A, bool UsePar = false, Action ac = null )
	{	if ( !_setterCache.ContainsKey( name ) )
		{	var type = UsePar ? A.par.GetType() : A.GetType();
			var p = type.GetProperty(name, (BindingFlags)(-1));
			var res = new FIELD_SETTER();
			
			if ( p != null )
			{	res.isprop = true;
				res.prop = p;
			}
			
			else
			{	var f = type.GetField(name, (BindingFlags)(-1));
				res.field = f;
			}
			
			if ( res.field == null && res.prop == null ) throw new Exception( name + " field not found\n" );
			
			if ( ac != null ) res.onChange = ac;
			
			res.UsePar = UsePar;
			_setterCache.Add( name, res );
		}
		
		_setterCache[name].A = A;
		return _setterCache[name];
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	CLASS_ENALBE __ENABLE;
	CLASS_ENALBE ENABLE
	{	get { return __ENABLE ?? (__ENABLE = new CLASS_ENALBE() { A = this }); }
	}
	public class CLASS_ENALBE {
		public class dsp : IDisposable {
			public  Adapter A;
			public bool usePadding;
			public void Dispose()
			{	if ( usePadding ) A.DRAW_STACK.PADDING_SET( 0 );
			
				A.DRAW_STACK.ENABLE_RESTORE();//ENABLE
			}
		}
		public   Adapter A;
		public dsp USE( FIELD_SETTER setter )
		{	A.DRAW_STACK.ENABLE_SET( setter );//ENABLE
			return new dsp() { A = A };
		}
		public dsp USE( FIELD_SETTER setter, float padding )
		{	A.DRAW_STACK.ENABLE_SET( setter );//ENABLE
			A.DRAW_STACK.PADDING_SET( padding );
			return new dsp() { A = A, usePadding = true };
		}
	}
	
	
	
	CLASS_GROUP __GROUP;
	CLASS_GROUP GRO
	{	get { return __GROUP ?? (__GROUP = new CLASS_GROUP() { A = this }); }
	}
	public class CLASS_GROUP {
		public class dsp : IDisposable {
			public bool UseSearchSet;
			public  Adapter A;
			public void Dispose()
			{	A.DRAW_STACK.END_GROUP();
			
				if ( UseSearchSet ) A.DRAW_STACK.SEARCH_SET( null );//SEARCH
			}
		}
		public   Adapter A;
		public dsp UP()
		{	A.DRAW_STACK.BEGIN_GROUP();
			return new dsp() { A = A };
		}
		public dsp UP( string searchSet )
		{	A.DRAW_STACK.SEARCH_SET( searchSet );//SEARCH
			A.DRAW_STACK.BEGIN_GROUP();
			return new dsp() { A = A, UseSearchSet = true };
		}
		
	}
	
	
	
	
	
	
	
}
}


/*using System.Collections;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
#if !UNITY_2018_3_OR_NEWER
    public class PreferencesTools
    {
        /// <summary>
        /// Open preferences window and select specific section
        /// </summary>
        /// <param name="sectionName"></param>
        public static void ShowSection(string sectionName)
        {
            const string preferencesType = "UnityEditor.PreferencesWindow";
            const string addCustomSectionsMethodName = "AddCustomSections";
            const string showWindowMethodName = "ShowPreferencesWindow";
            const string sectionsFieldName = "m_Sections";
            const string refreshPreferencesFieldName = "m_RefreshCustomPreferences";
            const string selectedSectionPropertyName = "selectedSectionIndex";
            const string sectionTypeName = "Section";
            const string contentFiledName = "content";

            // find assemble wich contains PreferencesWindow
            var asm = Assembly.GetAssembly(typeof(EditorWindow));
            var prefType = asm.GetType(preferencesType);
            if (prefType == null)
            {
                Debug.LogWarning($"{preferencesType} not found in {asm.FullName}");
                return;
            }
            // find method that runs PreferencesWindow and invoke it
            var showMethod = prefType.GetMethod(showWindowMethodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (showMethod == null)
            {
                Debug.LogWarning($"Methond {showWindowMethodName} not found in {preferencesType}");
                return;
            }
            showMethod.Invoke(null, null);
            var prefEditor = EditorWindow.GetWindow(prefType);
            if (prefEditor == null)
            {
                Debug.LogWarning($"{preferencesType} showed but can't find this window using");
                return;
            }

            // check is custom preferences added
            var refreshField = prefType.GetField(refreshPreferencesFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (refreshField == null)
            {
                Debug.LogWarning($"Field {refreshPreferencesFieldName} not found in {preferencesType}");
                return;
            }
            if ((bool)refreshField.GetValue(prefEditor))
            {
                // find method that runs PreferencesWindow and invoke it
                var refreshMethod = prefType.GetMethod(addCustomSectionsMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (refreshMethod == null)
                {
                    Debug.LogWarning($"Methond {addCustomSectionsMethodName} not found in {preferencesType}");
                    return;
                }
                refreshMethod.Invoke(prefEditor, null);
                refreshField.SetValue(prefEditor, false);
            }

            // find index of Protobuf section
            var sectionType = prefType.GetNestedType(sectionTypeName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (sectionType == null)
            {
                Debug.LogWarning($"{sectionTypeName} not found in {asm.FullName}");
                return;
            }
            var contentField = sectionType.GetField(contentFiledName, BindingFlags.Public | BindingFlags.Instance);
            if (contentField == null)
            {
                Debug.LogWarning($"Field {contentFiledName} not found in {sectionTypeName}");
                return;
            }
            var sectionsField = prefType.GetField(sectionsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (sectionsField == null)
            {
                Debug.LogWarning($"Field {sectionsFieldName} not found in {preferencesType}");
                return;
            }
            var sections = sectionsField.GetValue(prefEditor) as IEnumerable;
            if (sections == null)
            {
                Debug.LogWarning($"Field {sectionsFieldName} is not {typeof(IEnumerable).Name}");
                return;
            }
            int sectionIndex = 0;
            bool found = false;
            foreach (var section in sections)
            {
                GUIContent content = (GUIContent)contentField.GetValue(section);
                if (content.text == sectionName)
                {
                    found = true;
                    break;
                }
                sectionIndex++;
            }
            if (!found)
            {
                Debug.LogWarning($"Section {sectionName} not found in {preferencesType}");
                return;
            }

            // select protobuf section
            var selectedProp = prefType.GetProperty(selectedSectionPropertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (selectedProp == null)
            {
                Debug.LogWarning($"Property {selectedSectionPropertyName} not found in {preferencesType}");
                return;
            }
            selectedProp.SetValue(prefEditor, sectionIndex);
        }
    }
#endif
}
*/
