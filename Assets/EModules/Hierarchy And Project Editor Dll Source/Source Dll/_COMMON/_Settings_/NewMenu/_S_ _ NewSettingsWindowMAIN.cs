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


	void NEW_HEADER( string key, string searchString, int yOffset = -10 )
	{	if ( string.IsNullOrEmpty( searchString ) )
		{	//DRAW_STACK.HELP_TEXTURE( "HELP3" , 50, yOffset: -60 );
			DRAW_STACK.HELP_TEXTURE_WIDE( key, 40, yOffset: yOffset );
			DRAW_STACK.SPACE( 6 );
			
		}
	}
	
	void NEW_HEADER( Texture key, string searchString, int yOffset = -10 )
	{	if ( string.IsNullOrEmpty( searchString ) )
		{	//DRAW_STACK.HELP_TEXTURE( "HELP3" , 50, yOffset: -60 );
			DRAW_STACK.HELP_TEXTURE_WIDE( key, 40, yOffset: yOffset );
			DRAW_STACK.SPACE( 6 );
		}
	}
	
	
	
	
	
	
	
	void TRACK_HEIGHT_CHANGES()
	{	ResetScroll();
		RESET_SMOOTH_HEIGHT();
	}
	void TRACK_BOTTOM_HEIGHT_CHENGES()
	{	RESET_SMOOTH_HEIGHT();
	}
	void DrawGameOjbects()
	{	DRAW_STACK.TOGGLE( "Enable SetActive module for GameObjects ", GetSetter( "ENABLE_ACTIVEGMAOBJECTMODULE", this, true ) );
		DRAW_STACK.TOGGLE( "Smooth Focusing", GetSetter( "SMOOTH_FRAME", this, true ) ).TOOLTIP( "To focus on an object in 'SceneView', use Right-CLICK" );
		DRAW_STACK.HR();
		DRAW_STACK.LABEL( "SetActive Module Style" );
		DRAW_STACK.TOOLBAR( new[] { "Right Default", "Left Small", "Right Small" }, GetSetter( "SETACTIVE_POSITION", this ) );
		DRAW_STACK.TOGGLE_FROM_INT( "Contrast Style", GetSetter( "SETACTIVE_STYLE", this ), 1, 0 );
	}
	//   DRAW_STACK.HEADER( "Main Settings" );
	internal void SETTINGS_MAIN( string searchString )
	{
	
	
		if (string.IsNullOrEmpty(searchString) && pluginID == 0)
		{	using ( GRO.UP( ) )
			{	DRAW_STACK.TOGGLE( "Display List With New Options", GetSetter( "DISP_NEW", this ) );
			
				if (DISP_NEW)
				{	DRAW_STACK.HEADER( "New Features:" );
					DRAW_STACK.PADDING_SET(20);
					
					if (hashoveredItem)
					{	DRAW_STACK.TOGGLE( "Hide background for hovered item", GetSetter( "HIDE_HOVER_BG", this ) );
						DRAW_STACK.ICON( "NEW", null );
					}
					
					DRAW_STACK.HR();
					DRAW_STACK.TOGGLE( "Use multi-line selection for arrow keys", GetSetter( "USE_NEW_MULTYLINE_SELECTION_BEGHAVIOUR", this ) ).
					TOOLTIP("You can select many identical objects and use the up/down arrow keys to select their child.");
					DRAW_STACK.ICON( "NEW", null );
					
					if (hashoveredItem)
					{	DRAW_STACK.TOGGLE( "Expand the hovered item instead the selected item using the arrow keys", GetSetter( "USE_HOVER_EXPANDING", this ) ).
						TOOLTIP("To expand many selected items keep the mouse hover of any selected item and then press the arrow key.");
						DRAW_STACK.ICON( "NEW", null );
					}
					
					DRAW_STACK.HR();
					
					
					DRAW_STACK.TOGGLE_GROUP( "Draw BG for Mono Icon", GetSetter( "DRAW_ICONS_MONO_BG", this ) );
					DRAW_STACK.ICON( "NEW", null );
					
					using ( ENABLE.USE( GetSetter( "DRAW_ICONS_MONO_BG_INVERCE", this) ) )
					{	DRAW_STACK.TOGGLE_GROUP( "Draw Second Char for MonoBehaviour Names", GetSetter( "DRAW_SECOND_CHAR_FOR_MONO", this ) );
						DRAW_STACK.ICON( "NEW", null );
					}
					
					DRAW_STACK.PADDING_SET(0);
				}
				
			}
		}
		
		DRAW_STACK.SPACE(10);
		
		
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE( "Perfomance Bars Caching",  GetSetter( "CACHING_TEXTURES_STACKS", this  ) ).
			TOOLTIP( "this performance feature caches thextures and their positions, however, this is an experimental feature, but the cleaning interval is set to 2 seconds, so the problems with caching should last no more than two seconds" );
		}
		
		
		using ( GRO.UP( "auto hide modules bar press autohide" ) )
		{
		
		
			DRAW_STACK.HEADER( "Hiding Bars & Modules or Blocking" );
			
			
			using ( ENABLE.USE( GetSetter( "HAS_HOVER", this ) ) )
			{
			
				DRAW_STACK.LABEL( "Hover Behaviour:" );
				
				DRAW_STACK.TOGGLE( "Show modules only when selected or mouse is hover", GetSetter( "SHOW_ONLY_HOVERED_RAW", this ) ).
				TOOLTIP( "Use alt key to temporarily disaply all modules" );
			}
			
			DRAW_STACK.HR();
			
			DRAW_STACK.ACTION( RightAutoHider, false );
			
			
			if (USE_BUTTON_TO_INTERACT_AHC_BOOL)
			{	DRAW_STACK.TOOLTIP( "If a module already has a content, you shouldn't use a key to change them." );
			
				using ( ENABLE.USE( GetSetter( "USE_BUTTON_TO_INTERACT_AHC_BOOL", this ) ) )
				{	DRAW_STACK.LABEL( "Hide <b>Right Bar</b> if " + USE_BUTTON_TO_INTERACT_AHC_KEY + " not pressed" );
					DRAW_STACK.PADDING_SET( EditorGUIUtility.singleLineHeight * 4 );
					DRAW_STACK.TOOLBAR_FOR_BOOL( new[] { "Show Always", "Hide" }, GetSetter( "_S_HideRightIfNoFunction", this ) );
					DRAW_STACK.PADDING_SET( 0 );
					DRAW_STACK.LABEL( "Hide <b>Bottom Bar</b> if " + USE_BUTTON_TO_INTERACT_AHC_KEY + " not pressed" );
					DRAW_STACK.PADDING_SET( EditorGUIUtility.singleLineHeight * 4 );
					DRAW_STACK.TOOLBAR_FOR_BOOL( new[] { "Show Always", "Hide" }, GetSetter( "_S_HideBttomIfNoFunction", this ) );
					DRAW_STACK.PADDING_SET( 0 );
				}
				
			}
			
			
			
			
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.HEADER( "Main Settings" );
		}
		
		using ( GRO.UP() )
		{	if (pluginID == 0  )
			{	DRAW_STACK.SLIDER( "<b>Font Size</b> - internal '"+lastFontSize +"'", 5, 30, GetSetter( "HIERARCHY_FONT_SIZE", this ) );
				DRAW_STACK.TOGGLE( "Font Size for editor affect other windows", GetSetter( "EDITOR_FONT_AFFECTOTHERWINDOWS", this ) );      //defaultStyle: true
			}
			
			DRAW_STACK.SLIDER( "<b>Font Size</b> - plugin '11'", 10, 17, GetSetter( "FONTSIZENEW", this, true, TRACK_HEIGHT_CHANGES ), 10 ).
			TOOLTIP( "Now, if the height differs from 16, the bottom bar will hide for a moment after compilation. it will be fixed in the future" );
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.VAR_FIELD( "↕ <b>Height</b> of Lines '", 12, 32, GetSetter( "parLINE_HEIGHT", this, ac: TRACK_HEIGHT_CHANGES ) );
			DRAW_STACK.TOGGLE( "Height affect the bottom panel", GetSetter( "HEIGHT_APPLY_TOBOTTOM", this, true, TRACK_BOTTOM_HEIGHT_CHENGES ) );
			DRAW_STACK.VAR_FIELD( "Additional Height of Bookmarks", -10, 20, GetSetter( "ADDITIONAL_BOOKMARKS_HEIGHT", this, true ) );
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.COLOR_PICKER( "Labels Color", GetSetter( "_S_TextColor", this ) );
		
			if (hashoveredItem)
			{	DRAW_STACK.TOGGLE( "Hide background for hovered item", GetSetter( "HIDE_HOVER_BG", this ) );
				DRAW_STACK.ICON( "NEW", null );
			}
		}
		
		using ( GRO.UP() )
		{	if ( par.HEADER_OPACITY == null ) par.HEADER_OPACITY = DefaultBGOpacity;
		
			DRAW_STACK.VAR_FIELD( "Modules Background Opacity:", 0, 1, GetSetter( "HEADER_OPACITY", this, true ) ).
			TOOLTIP( "How strong the background will fade behind the right modules." );
			DRAW_STACK.PADDING_RIGHT_HARDSET( 90 );
			DRAW_STACK.TOGGLE_FROM_INT( "Background Chess Lines", GetSetter( "HIER_LINES_CHESSE", this, true ), 1, 0 );      //defaultStyle: true
			DRAW_STACK.PADDING_RIGHT_HARDSET( 0 );
			DRAW_STACK.COLOR_PICKER_LASTRECT( GetSetter( "CHESS_COLOR", this ) );
			DRAW_STACK.PADDING_RIGHT_HARDSET( 90 );
			DRAW_STACK.TOGGLE_FROM_INT( "Background Separating Lines", GetSetter( "HIER_LINES_HR", this, true ), 1, 0 );      //defaultStyle: true
			DRAW_STACK.PADDING_RIGHT_HARDSET( 0 );
			DRAW_STACK.COLOR_PICKER_LASTRECT( GetSetter( "HR_COLOR", this ) );
			DRAW_STACK.ENABLE_SET( GetSetter( "HIER_LINES_HR", this, true ), GetSetter( "HIER_LINES_CHESSE", this, true ) );     //ENABLE
			DRAW_STACK.TOGGLE_FROM_INT( "Background Lines Clamp", GetSetter( "HierarhchyLines_Fix", this, true ), 1, 2 );      //defaultStyle: true
			DRAW_STACK.ENABLE_RESTORE();                                                                                          // DRAW_STACK.ENABLE_RESTORE();//ENABLE
		}
		
		using ( GRO.UP() )
		{	if ( par.DEEP_WIDTH == null ) par.DEEP_WIDTH = IS_HIERARCHY() ? 14 : 16;
		
			DRAW_STACK.VAR_FIELD( "Indentation in children '" + (IS_HIERARCHY() ? "14" : "16") + "'", 4, 30, GetSetter( "DEEP_WIDTH", this, true ) );
			DRAW_STACK.HELP_TEXTURE( "HELP_INDENT", 30, yOffset: -100 );
			DRAW_STACK.PADDING_RIGHT_HARDSET( 90 );
			DRAW_STACK.TOGGLE_FROM_INT( "Child Lines", GetSetter( "DRAW_HIERARHCHY_LINES_V2", this, true ), 1, 0 );      //defaultStyle: true
			DRAW_STACK.PADDING_RIGHT_HARDSET( 0 );
			DRAW_STACK.COLOR_PICKER_LASTRECT( GetSetter( "CHILDREN_LINE_COLOR", this ) );
		}
		
		using ( GRO.UP( "Draw ChildCount next to GameObject's name Offset X Offset Y" ) )
		{	DRAW_STACK.TOGGLE_GROUP( "Draw ChildCount", GetSetter( "DRAW_CHILDS_COUNT", this, true ) );
		
			using ( ENABLE.USE( GetSetter( "DRAW_CHILDS_COUNT", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
			{	DRAW_STACK.TOOLBAR( new[] { "Align Left", "Align Middle", "Align Right" }, GetSetter( "_S_CountNumber_Align", this ), 1 );
				DRAW_STACK.VAR_FIELD( "Offset X", GetSetter( "_S_CountNumber_OffsetX", this ), -10000, 10000 );
				DRAW_STACK.VAR_FIELD( "Offset Y", GetSetter( "_S_CountNumber_OffsetY", this ), -10000, 10000 );
				//  DRAW_STACK.HELP_TEXTURE( "CHILD_COUNT" , xOffset: 10 );
				DRAW_STACK.TOGGLE( "Hide ChildCount if line expanded", GetSetter( "HIDE_CHILDCOUNT_IFEXPANDED", this, true ) );    //defaultStyle: true
				DRAW_STACK.TOGGLE( "Hide ChildCount if root", GetSetter( "HIDE_CHILDCOUNT_IFROOT", this, true ) )    //defaultStyle: true
				.TOOLTIP( "Do not show the number for the topmost objects." );
			}
		}
		
		if (pluginID == 0)
			using ( GRO.UP() )
			{	DrawGameOjbects();
			}
			
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE( "<b>Double-click</b>: Expand hierarchy item", GetSetter( "DOUBLECLICK_IS_EXPAND", this, true ) ).
			TOOLTIP( "You can enable the ability to expand items by double-clicking. By default, double-click to move camera to an object in the scene window." );
			DRAW_STACK.TOGGLE( "Use multi-line selection for arrow keys", GetSetter( "USE_NEW_MULTYLINE_SELECTION_BEGHAVIOUR", this ) ).
			TOOLTIP("You can select many identical objects and use the up/down arrow keys to select their child.");
			DRAW_STACK.ICON( "NEW", null );
			
			if (hashoveredItem)
			{	DRAW_STACK.TOGGLE( "Expand the hovered item instead the selected item using the arrow keys", GetSetter( "USE_HOVER_EXPANDING", this ) ).
				TOOLTIP("To expand many selected items keep the mouse hover of any selected item and then press the arrow key.");
				DRAW_STACK.ICON( "NEW", null );
			}
		}
		
		if ( IS_HIERARCHY() )
		{	using ( GRO.UP() ) using ( ENABLE.USE( GetSetter( "hasShowingPrefabHeader", this ) ) )
				{	DRAW_STACK.TOGGLE_GROUP( "Escape: Close <b>Prefab Mode</b>", GetSetter( "ESCAPE_CLOSE_PREFAB", this, true ) );
				
					using ( ENABLE.USE( GetSetter( "ESCAPE_CLOSE_PREFAB", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
					{	DRAW_STACK.LABEL( "For which windows does Escape work?" );
						DRAW_STACK.TOOLBAR_FOR_BOOL( new[] { "Hierarchy/Scene/Inspector", "All" }, GetSetter( "PREFAB_ESCAPE_ALL_WINDOWS", this ) );
					}
				}
		}
		
		
		using ( GRO.UP() )
		{	DRAW_STACK.SLIDER( "Input Windows Width", 1, 3, GetSetter( "ADDITIONA_INPUT_WINDOWS_WIDTH", this )  ).
			TOOLTIP( "Additional width of popup input (int/float/string) windows" );
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE( "Enable ping if changing Object's parameters", GetSetter( "ENABLE_PING_Fix", this, true ) )    //defaultStyle: true
			.TOOLTIP( "If object not selected it will be highlighted when changing the parameters." );
		}
		
		if ( IS_HIERARCHY() )
		{	using ( GRO.UP() )
			{	DRAW_STACK.TOGGLE( "Enable 'SceneView' blocking for locked objects", GetSetter( "LOCK_SELECTION", this, true ) )    //defaultStyle: true
				.TOOLTIP( "You cannot select locked objects in the scene window." );
			}
		}
	}
	
	void RightAutoHider()
	{	Rect R;
		R = EditorGUILayout.GetControlRect();
		R.width -= 80;
		LABEL( R, "Special Key to block actions or hide bar until pressed:" );
		//  R.x += R.width;
		//  R.width = 80;
		R = EditorGUILayout.GetControlRect();
		
		var old_i = par.USE_BUTTON_TO_INTERACT_AHC & 3;
		// var new_i = EditorGUI.Popup(R,  par., new[] { "Disabled", "Alt", "Shift", "Ctrl" });
		
		var new_i = GUI.Toolbar(R, old_i, new[] { "Disabled", "Alt", "Shift", "Ctrl" }, EditorStyles.miniButton) ;
		
		//  TOOLTIP( R2 , "You can block interaction with modules and use a special key." );
		var on = GUI.enabled;
		GUI.enabled &= new_i != 0;
		
		
		var new_8 = USE_BUTTON_TO_INTERACT_AHC_BOOL ? TOGGLE_LEFT( EditorGUILayout.GetControlRect( ), TOGGLE_PTR + "Block only modules without content", ( par.USE_BUTTON_TO_INTERACT_AHC & 8) == 0,
		            defaultStyle:  true) :( ( par.USE_BUTTON_TO_INTERACT_AHC & 8) == 0);
		            
		GUI.enabled = on;
		
		var new_A = new_i | (new_8 ? 0 : 8);
		
		if ( new_A != par.USE_BUTTON_TO_INTERACT_AHC )
		{	par.USE_BUTTON_TO_INTERACT_AHC = new_A;
			DRAW_STACK.ValueChanged();
		}
		
		//    TOOLTIP( R2 , "If a module already has a content, you shouldn't use a key to change them." );
		
		//             var   lineRect = EditorGUILayout.GetControlRect( );
		//             var new_S_HideRightIfNoFunction = TOOGLE_POP( ref lineRect , "Hide <b>Right Bar</b> if " + key + " not pressed" , _S_HideRightIfNoFunction ? 1 : 0 , "Show Always" , "Hide" ) == 1;
		//             GUILayout.Space( EditorGUIUtility.singleLineHeight );
		//             lineRect = EditorGUILayout.GetControlRect();
		//             var new_S_HideBttomIfNoFunction = TOOGLE_POP( ref lineRect , "Hide <b>Bottom Bar</b> if " + key + " not pressed" , _S_HideBttomIfNoFunction ? 1 : 0 , "Show Always" , "Hide" ) == 1;
		//             GUILayout.Space( EditorGUIUtility.singleLineHeight );
		//             GUI.enabled = on;
		//
		//             if ( new_A != par.USE_BUTTON_TO_INTERACT_AHC || _S_HideRightIfNoFunction != new_S_HideRightIfNoFunction || _S_HideBttomIfNoFunction != new_S_HideBttomIfNoFunction ) {
		//                 par.USE_BUTTON_TO_INTERACT_AHC = new_A;
		//                 _S_HideRightIfNoFunction = new_S_HideRightIfNoFunction;
		//                 _S_HideBttomIfNoFunction = new_S_HideBttomIfNoFunction;
		//                 DRAW_STACK.ValueChanged();
		//             }
		
	}
	
	
	
	
	
	
	
	
	
	
	
	internal void SETTINGS_SEARCH( string searchString )
	{
	
		NEW_HEADER( IS_HIERARCHY() ? "HELP_SEARCH" : "SETUP_SEARCH PROJECT", searchString, -40 );
		DRAW_STACK.DRAW_WIKI_BUTTON( "Other", "Search Box" );
		
		
		DRAW_STACK.HELP_BOX( "Right-CLICK on the content of one of the found objects, to search among already found objects\nYou can drag and drop objects from this window." );
		DRAW_STACK.HELP_TEXTURE( "NEW_BOTTOM_SEARTCHHELP" );
		DRAW_STACK.HELP_BOX( "If you want to search within a parent object, select the parent object, hold the 'CONTROL', and 'Right-CLICK' the component." );
		
		
		using ( GRO.UP() )
		{
		
		
			if (pluginID == 0)  DRAW_STACK.TOGGLE( "Include disabled objects", GetSetter( "SEARCH_SHOW_DISABLED_OBJECT", this ) );
			
			DRAW_STACK.TOGGLE( "Search uses only the root object of the clicked object", GetSetter( "SEARCH_USE_ROOT_ONLY", this ) );
		}
		
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE( "Double-click - <b>Close</b> Search Window", GetSetter( "SEARCH_DOUBLECLICK_CLOSE", this, true ) ).
			TOOLTIP( "If it is disabled then a single click will close the unpinned search window." );
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE( "Bind Search Window to <b>Left</b> by default", GetSetter( "_S_searchBintToLeft", this ) ).
			TOOLTIP( "If disabled, the window will be located at the cursor." );
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.VAR_FIELD( "Default <b>Width</b> multiply '1':", GetSetter( "_S_searchWidthMulty", this ), 0.1f, 5 );
			// TOOLTIP( "You can increase the width of the window to your liking." );
		}
		
		//  DRAW_STACK.HR();
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE( "Turn on Pin button by default", GetSetter( "PIN_FILLTERWIN_BYDEFAULT", this, true ) );
			//TOOLTIP( "The window will automatically hide when lost focus, if you click on the pin button at the top of the 'search window', window will not to be automatically hide." );
		}
		
		//  DRAW_STACK.HELP_TEXTURE( IS_HIERARCHY() ? "HELP_SEARCH" : "SETUP_SEARCH PROJECT" , 100 , yOffset: -40 );
		
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	void DrawHotkeysWindows()
	{
	
		GUILayout.BeginHorizontal();
		GUILayout.Space( 30 );
		GUILayout.BeginVertical();
		var htks = CUSTOMMENU_HOTKEYS_WINDOWS;
		//  internal string[] AllWindows = { "SceneView", "Inspector", "GameView", "SceneHierarchy"/*, "ProjectBrowser"*/};
		var R = EditorGUILayout. GetControlRect( GUILayout.Height(EditorStyles.toolbarButton.fixedHeight) );
		GUI.enabled = false;
		
		if ( IS_HIERARCHY() )
		{	GUI.Toolbar( R, 1, new[] { "No", "Scene Hierarchy" }, EditorStyles.toolbarButton );
		}
		
		else
		{	GUI.Toolbar( R, 1, new[] { "No", "Project Browser" }, EditorStyles.toolbarButton );
		}
		
		GUI.enabled = true;
		R = EditorGUILayout.GetControlRect( GUILayout.Height( EditorStyles.toolbarButton.fixedHeight ) );
		var oldw1 = htks.ContainsKey("SceneView");
		var w1 =  GUI.Toolbar(R, oldw1 ? 1 : 0, new[] {   "No", "SceneView"}, EditorStyles.toolbarButton) == 1;
		R = EditorGUILayout.GetControlRect( GUILayout.Height( EditorStyles.toolbarButton.fixedHeight ) );
		var oldw2 = htks.ContainsKey("GameView");
		var w2 =  GUI.Toolbar(R, oldw2 ? 1 : 0, new[] {   "No", "GameView"}, EditorStyles.toolbarButton) == 1;
		R = EditorGUILayout.GetControlRect( GUILayout.Height( EditorStyles.toolbarButton.fixedHeight ) );
		var oldw3 = htks.ContainsKey("Inspector");
		var w3 =  GUI.Toolbar(R, oldw3 ? 1 : 0, new[] {   "No", "Inspector"}, EditorStyles.toolbarButton) == 1;
		
		if ( w1 != oldw1 || w2 != oldw2 || w3 != oldw3 )
		{	var res = new Dictionary<string, bool>();
		
			if ( w1 ) res.Add( "SceneView", true );
			
			if ( w2 ) res.Add( "GameView", true );
			
			if ( w3 ) res.Add( "Inspector", true );
			
			CUSTOMMENU_HOTKEYS_WINDOWS = res;
			DRAW_STACK.ValueChanged();
		}
		
		GUILayout.EndVertical();
		GUILayout.Space( 30 );
		GUILayout.EndHorizontal();
	}
	Vector2 sp;
	void DrawInterface()
	{	var rect = EditorGUILayout. GetControlRect( GUILayout.Height(221) );
		sp = GUI.BeginScrollView( rect, sp, new Rect( 0, 0, 850, 185 ) );
		
		if ( IS_HIERARCHY() ) EditorGUI.TextArea( new Rect( 0, 0, 850, 185 ), HIERARCHY_MENU_HELP );
		else EditorGUI.TextArea( new Rect( 0, 0, 850, 185 ), PROJECT_MENU_HELP );
		
		GUI.EndScrollView();
	}
	internal void SETTINGS_GENERIC( string searchString )
	{
	
	
		NEW_HEADER( IS_HIERARCHY() ? "HELP_RIGHTMENU" : "HELP_RIGHTMENU PROJECT", searchString );
		DRAW_STACK.DRAW_WIKI_BUTTON( "Other", "Custom Extensible Menu" );
		DRAW_STACK.TOOLTIP( "Right-click to the left of the object name to show the menu" );
		
		using ( GRO.UP() )
		{	DRAW_STACK.LABEL( "For which windows does Custom Menu's HotKeys work?" );
			DRAW_STACK.ACTION( DrawHotkeysWindows, false );
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.HELP_BOX( "Source code of all items are placed into: '" + PluginInternalFolder + "/RightClickObjectLeftMenu_Example.cs'", MessageType.None );
		}
		
		//  DRAW_STACK.HELP_TEXTURE( IS_HIERARCHY() ? "HELP_RIGHTMENU" : "HELP_RIGHTMENU PROJECT" /*,60*/).TOOLTIP( "Right-CLICK to the left of the object" );
		
		using ( GRO.UP() )
		{	DRAW_STACK.HELP_BOX( "You should inherit the interface to create menu item" );
			DRAW_STACK.ACTION( DrawInterface, true );
		}
		
		
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
#pragma warning disable
	
	GUIContent C1 = new GUIContent(
	    @"- The plugin will automatically add scripts with the cache to the objects, if necessary.

- You can 'Duplicate' or create 'Instantiate' without losing the parameters of the hierarchy plugin, you can also save the settings for the prefab.

- Costs of this mode, are that you will have many scripts associated with the plugin.");

	GUIContent C2 = new GUIContent(@"- In default mode, in each scene will be created an object with plugin settings, you can hide or show it.

- You can 'Duplicate' object without losing the parameters of the hierarchy plugin, the plugin will store the entire cache in one place (DescriptionHelper object).

- The downside of this mode - presence of an object in the scene. If you send the scene to another machine which does not have plugin, you will have to send 'SharedFolder' also.
");
	GUIContent C3
	{	get
		{	if ( __C3 == null ) __C3 = new GUIContent( @"- If you save data in ScriptableObject files, your data will be stored apart from the scenes, inside the plugin folder in the """ +
				        PluginInternalFolder + @"/_ SAVED DATA""

- Benefits of this mode, are that you can keep clean scenes, and send it to those who do not have hierarchy plugin.

- The downside is, you will have to fix files names with parameters if you rename or move or duplicate scenes. Another shortcoming is that the initialization will take a little longer, although it takes milliseconds, you still have to know about it.

"

				                                         );
				                                         
#pragma warning restore
				                                         
			return __C3;
		}
	}
	GUIContent __C3;
	
	internal void SETTINGS_CACHE( string searchString )
	{
	
		NEW_HEADER( "NEW_BOTTOM_GITHELP2", searchString );
		DRAW_STACK.DRAW_WIKI_BUTTON( "Other", "Cache and Data Storage" );
		
		if ( IS_HIERARCHY() )
		{	DRAW_STACK.HEADER( "Prefabs" );
		
			using ( ENABLE.USE( GetSetter( "EXTERNAL_FOLDER_DISABLE", this) ) )
			{	using ( GRO.UP( "Prefab SettingsSeparate InstancesMerged Instances" ) )
				{	DRAW_STACK.LABEL( "<b>Prefab Settings:</b>" );
					bool[] HLS = new [] { Hierarchy_GUI.Instance( this ).PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.SeparateInstances,
					                      Hierarchy_GUI.Instance(this ).PrefabIDMode == Hierarchy_GUI.PrefabIDModeEnum.MergedInstances
					                    };
					DRAW_STACK.PADDING_SET( 40 );
					DRAW_STACK.BUTTON( new[] { "Separate Instances", "Merged Instances" }, new[] { (Action)CacheEditorPrefab0, (Action)CacheEditorPrefab1 }, 40, HLS );
					DRAW_STACK.PADDING_SET( 0 );
					DRAW_STACK.HELP_BOX( "Merged means all data for all instances of one corresponding object will stored in sync. Changes applied to one instance are automatically applied to others." );
				}
			}
		}
		
		DRAW_STACK.HEADER( "Auto Filters Presets & Icons" );
		
		using ( GRO.UP() )
		{	DRAW_STACK.HELP_BOX( "You can save Auto Highlighter and Icons settings local to Library, or to Assets and push it to other developers", MessageType.None );
			DRAW_STACK.SEARCH_SET( "Auto-HighLighter</b> FiltersSave To Library( Local )Save folder To Asset( Public" );
			DRAW_STACK.LABEL( "<b>Auto Filters</b> folder:" );
			DRAW_STACK.PADDING_SET( 40 );
			DRAW_STACK.TOOLBAR_FOR_BOOL( new[] { "Save To Library (Local)", "Save To Asset (Public)" }, GetSetter( "SaveSettingsHighLighterToAsset", this ) ).
			ON_CHANGE_CONFORM( LibraryFiltersDialog );
			DRAW_STACK.PADDING_SET( 0 );
			DRAW_STACK.SEARCH_SET( null );
			
			if ( IS_HIERARCHY() )
			{	DRAW_STACK.SEARCH_SET( "Components Icons</b> FiltersSave To Library( Local )Save To folder Asset( Public" );
				DRAW_STACK.LABEL( "<b>Components Icons</b> folder:" );
				DRAW_STACK.PADDING_SET( 40 );
				DRAW_STACK.TOOLBAR_FOR_BOOL( new[] { "Save To Library (Local)", "Save To Asset (Public)" }, GetSetter( "SaveSettingsCustomIconsToAsset", this ) ).
				ON_CHANGE_CONFORM( LibraryIconsDialog );
				DRAW_STACK.PADDING_SET( 0 );
				DRAW_STACK.SEARCH_SET( null );
			}
			
			DRAW_STACK.HELP_BOX( "If save into Library you may share the settings, just copy 'Library/EModules' to another machine", MessageType.None );
		}
		
		if ( IS_HIERARCHY() )
		{	DRAW_STACK.HEADER( "Scenes Cache & Storage" );
		
			using ( GRO.UP( "Data SettingsExternalFoldersavecacheCache in Scenes" ) )
			{	DRAW_STACK.LABEL( "<b>Data Settings:</b>" );
			
				if ( tempAdapterDisableCache )
					DRAW_STACK.HELP_BOX( "Please Fix Compile Errors To Enable Cache", MessageType.Error );
				else
				{	DRAW_STACK.ACTION( CacheGUI, false );
				}
			}
		}
		
		
		
		
		
		
		
		
		
		using ( GRO.UP() )
		{	DRAW_STACK.HEADER( "Editor Settings" );
			//  DRAW_STACK.LABEL( "<i>Editor Settings:</i>" );
			DRAW_STACK.BUTTON( new[] { "Import Editor Settings", "Export Editor Settings" }, new[] { (Action)CacheEditorSetBut0, (Action)CacheEditorSetBut1 }, 20 );
			DRAW_STACK.BUTTON( new[] { "Reset to Default Editor Settings" }, new Action[] { (Action)CacheEditorSetBut2 }, 20 );
		}
		
		if ( IS_HIERARCHY() )
		{	DRAW_STACK.HEADER( "Remove" );
		
			using ( GRO.UP( "Remove" ) )
			{	DRAW_STACK.HELP_BOX( "If you used <B>Cache in Folder</b> mode you can simply\n remove the plugin folder" );
				DRAW_STACK.HR();
				DRAW_STACK.HELP_BOX( "If you used <B>Cache in Scenes</b> you can choose\n one of these methods:" );
			}
			
			using ( GRO.UP( "Remove" ) )
			{	DRAW_STACK.PADDING_SET( 40 );
				DRAW_STACK.BUTTON( "Clean Cache and Remove Immediately", RemoveVer3, 40 )   //REMOVER.CreateWizard( A , 0 );
				.TOOLTIP( "This will remove all hierarchy data that was saved is scenes using Clear Cache Manager." );
				DRAW_STACK.BUTTON( "Save Remove Partially", RemoveVer1, 40 )   //REMOVER.CreateWizard( A , 0 );
				.TOOLTIP( "This option saves 'SharedFolder' with DLL library if you have a cache in any scene." );
				DRAW_STACK.BUTTON( "Remove and Create 'ClearDataHelper.cs'", RemoveVer2, 40 )   //REMOVER.CreateWizard( A , 1 );
				.TOOLTIP( "This option create a script that will automatically remove the cache in opened scenes, if you have a cache in any scene." );
				DRAW_STACK.PADDING_SET( 0 );
				
			}
		}
		
	}
	void RemoveVer1()
	{	SETUPROOT.REMOVER.CreateWizard( this, 0 );
	}
	void RemoveVer2()
	{	SETUPROOT.REMOVER.CreateWizard( this, 1 );
	}
	void RemoveVer3()
	{	if ( EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() )
		{	par.ENABLE_ALL = false;
			OnEnableChange();
			_S___ScenesScannerWindow.Init( this, () =>
			{	SETUPROOT.REMOVER.CreateWizard( this, 3 );
			} );
		}
	}
	
	
	
	
	
	
	
	
	
	
	internal void SETTINGS_HIGHLIGHTER_MAIN( string searchString )
	{
	
		NEW_HEADER( "USE_HIGLIGHT", searchString );
		DRAW_STACK.DRAW_WIKI_BUTTON( "Left Panel" );
		
		
		
		//☰
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE_GROUP( "Enable HighLighter", GetSetter( "ENABLE_LEFTDOCK_FIX", this, true ) );
			//  TOOLTIP( "To open the HighLighter, click the dot at the left of the object" );
			DRAW_STACK.HELP_BOX( "To open the HighLighter, left click at the left of the object" );
			
			using ( ENABLE.USE( GetSetter( "ENABLE_LEFTDOCK_FIX", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
			{	DRAW_STACK.LABEL( "<b>HighLighter Button</b> Placement" );
#pragma warning disable
			
				if ( IS_PROJECT() )
					DRAW_STACK.TOOLBAR( new[] { "-Icon-" }, GetSetter( "_S_bgButtonForIconsPlace", this ), 1 );
				//nv = adapter.TOOGLE_POP( ref lineRect , "<b>HighLighter Window Button</b> Placement" , adapter._S_bgButtonForIconsPlace - 1 , "-Icon-" ) + 1;
				else
					if ( Adapter.USE2018_3 )
						DRAW_STACK.TOOLBAR( new[] { "Left", "-Icon-", "<Left and -Icon-" }, GetSetter( "_S_bgButtonForIconsPlace", this ) );
					//nv = adapter.TOOGLE_POP( ref lineRect , "<b>HighLighter Window Button</b> Placement" , adapter._S_bgButtonForIconsPlace , "<Left" , "-Icon-" , "<Left and -Icon-" );
					else
						DRAW_STACK.TOOLBAR( new[] { "<Left" }, GetSetter( "_S_bgButtonForIconsPlace", this ) );
						
				// nv = adapter.TOOGLE_POP( ref lineRect , "<b>HighLighter Window Button</b> Placement" , adapter._S_bgButtonForIconsPlace , "<Left" );
				if ( Adapter.USE2018_3 )
				{	DRAW_STACK.LABEL( "<b>Draw Little Dot</b> for HighLighter Button Hover" );
					DRAW_STACK.TOOLBAR( new[] { "None", "Window Open Only", "Window and Hover" }, GetSetter( "_S_hoverState", this ) );
				}
				
#pragma warning restore
			}
		}
		
		using ( ENABLE.USE( GetSetter( "ENABLE_LEFTDOCK_FIX", this, true ) ) )
		{	DRAW_STACK.HEADER( "Colors" );
		
			using ( GRO.UP() )
			{	DRAW_STACK.TOGGLE_GROUP( "Use HighLighter Colors", GetSetter( "USE_HIGLIGHT", this, true ) );
			
				using ( ENABLE.USE( GetSetter( "USE_HIGLIGHT", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
				{	DRAW_STACK.VAR_FIELD( "HighLighter Colors Opacity:", GetSetter( "highligterOpacity", this, true ), 0, 1 );
					DRAW_STACK.VAR_FIELD( "HighLighter Colors Vertical Padding '1':", GetSetter( "_S_BottomPaddingForBgColor", this ), -16, 16 );
					DRAW_STACK.HR();
					DRAW_STACK.TOGGLE_FROM_INT( "Use Grouping For Child Toggles", GetSetter( "GROUPING_CHILD_MODE", this ), 1, 0 );
					
					if (Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_VERSION )
					{	DRAW_STACK.HR();
						DRAW_STACK.TOGGLE_FROM_INT( "Overlap left unity column", GetSetter( "HIGHLIGHTER_LEFT_OVERFLOW", this ), 1, 0 );
					}
					
					DRAW_STACK.HR();
					DRAW_STACK.SEARCH_SET( "Highlighter external Texture for Colors" );
					DRAW_STACK.LABEL( "Highlighter BG Texture" );
					DRAW_STACK.TOOLBAR( new[] { "None'", "Box", "TextArea", "External" }, GetSetter( "HIGHLIGHTER_TEXTURE_STYLE", this ) );
					
					using ( ENABLE.USE( GetSetter( "HIGHLIGHTER_TEXTURE_GUID_ALLOW", this ) ) )
					{	DRAW_STACK.ACTION( DrawExternalTextureObject, false );
					}
					
					using ( ENABLE.USE( GetSetter( "HIGHLIGHTER_TEXTURE_BORDER_ALLOW", this ) ) )
					{	DRAW_STACK.VAR_FIELD( "Texture Borders", GetSetter( "HIGHLIGHTER_TEXTURE_BORDER", this ), 0, 16 );
					}
					
					DRAW_STACK.SEARCH_SET( null );
					
					using ( ENABLE.USE( GetSetter( "HIGHLIGHTER_TEXTURE_BORDER_ALLOW", this ) ) )
					{	DRAW_STACK.HR();
						DRAW_STACK.SEARCH_SET( "Highlighter Special Drawing Shaderuse" );
						DRAW_STACK.TOGGLE( "Use Special Shader", GetSetter( "HIGHLIGHTER_USE_SPECUAL_SHADER", this ) );
						
						using ( ENABLE.USE( GetSetter( "HIGHLIGHTER_USE_SPECUAL_SHADER", this ) ) )
						{	DRAW_STACK.LABEL( "Highlighter Special Drawing Shader" );
						
							DRAW_STACK.TOOLBAR( new[] { "Normal", "Additive" }, GetSetter( "HIGHLIGHTER_USE_SPECUAL_SHADER_TYPE", this ) );
							DRAW_STACK.ACTION( DrawExternalShaderObject, false );
						}
						
						DRAW_STACK.SEARCH_SET( null );
					}
				}
			}
			
			
			DRAW_STACK.HEADER( "Icons" );
			
			
			using ( GRO.UP() )
			{
			
				DRAW_STACK.TOGGLE( "Use Default Icons size:", GetSetter( "_S_USEdefaultIconSize", this ) );
				
				using ( ENABLE.USE( GetSetter( "_S_USEdefaultIconSize", this ) ) )
				{	DRAW_STACK.VAR_FIELD( "<b>Default Icons</b> size '" + EditorGUIUtility.singleLineHeight + "'", GetSetter( "_S_defaultIconSize", this ), 10, 30 );
				}
			}
			
			using ( GRO.UP() )
			{	DRAW_STACK.VAR_FIELD( "<b>Custom Icons</b> size '" + EditorGUIUtility.singleLineHeight + "'", GetSetter( "COLOR_ICON_SIZE", this, true ), 10, 30 ).
				TOOLTIP( "You can use right button to search by icons" );
				DRAW_STACK.PADDING_SET( EditorGUIUtility.singleLineHeight * 4 );
				DRAW_STACK.LABEL( "Custom Icons Placement" );
				DRAW_STACK.TOOLBAR( new[] { "Next to 'Label'", "Next to 'Foldout'", "Align 'Left'" }, GetSetter( "_S_bgIconsPlace", this ) );
				
				// Debug.Log( HierAdapter._S_bgIconsPlace );
				
				
				if ( HAS_LABEL_ICON() )
				{	DRAW_STACK.TOGGLE( "Draw yellow marks next to the Custom Icons", GetSetter( "DRAW_FOLDER_STARMARK", this ) );
					/* DRAW_STACK.LABEL( "Draw yellow marks next to the Custom Icons" );
					 DRAW_STACK.TOOLBAR_FOR_BOOL( new[] { "Disable" , "Enable" } , GetSetter( "DRAW_FOLDER_STARMARK" , this ) );*/
				}
				
				DRAW_STACK.PADDING_SET( 0 );
				
			}
			
			/*   using ( GRO.UP() ) {
			       DRAW_STACK.INT_FIELD( "<b>Custom Icons</b> size '" + EditorGUIUtility.singleLineHeight + "'" , GetSetter( "COLOR_ICON_SIZE" , this , true ) , 10 , 30 ).
			           TOOLTIP( "You can use right button to search by icons" );
			
			   }
			   using ( GRO.UP() ) {
			       DRAW_STACK.LABEL( "Custom Icons Placement" );
			       DRAW_STACK.TOOLBAR( new[] { "Next to 'Label'" , "Next to 'Foldout'" , "Align 'Left'" } , GetSetter( "_S_bgIconsPlace" , this ) );
			   }
			   if ( HAS_LABEL_ICON() ) {
			       using ( GRO.UP() ) {
			           DRAW_STACK.LABEL( "Draw yellow marks next to the Custom Icons" );
			           DRAW_STACK.TOOLBAR_FOR_BOOL( new[] { "Disable" , "Enable" } , GetSetter( "DRAW_FOLDER_STARMARK" , this ) );
			       }
			   }*/
			if ( IS_HIERARCHY() )
			{	using ( GRO.UP() )
				{
				
					DRAW_STACK.TOGGLE( "Show Locator for Object without Component", GetSetter( "SHOW_NULLS", this, true ) );
					DRAW_STACK.ICON( "NULL", GetSetter( "SHOW_NULLS", this, true ) );
					//  using ( ENABLE.USE( GetSetter( "IS_UNITY_2019_2_ABOVE", this) ) )
					{	DRAW_STACK.TOGGLE( "Show Prefab icon", GetSetter( "SHOW_PREFAB_ICON", this, true ) );
						DRAW_STACK.ICON( "PREF", GetSetter( "SHOW_PREFAB_ICON", this, true ) );
					}
					DRAW_STACK.TOGGLE( "Show Warning if Object has missing Component", GetSetter( "SHOW_MISSINGCOMPONENTS", this, true ) );
					DRAW_STACK.ICON( "WARNING", GetSetter( "SHOW_MISSINGCOMPONENTS", this, true ) );
				}
				
			}
			
			
			
			
			
		}
		
		
		/*  DRAW_STACK.HEADER( "Auto Mode" );
		
		  using ( ENABLE.USE( GetSetter( "ENABLE_LEFTDOCK_FIX", this, true ) ) )
		  {
		
		      //  HIGHLIGHTER_HEADER( searchString );
		
		      using ( GRO.UP( "HighLighter Auto Filters" ) )
		      {   // DRAW_STACK.LABEL( "HighLighter Filters" );
		          DRAW_STACK.TOGGLE( "<i>Enable HighLighter Auto Filters:</i>", GetSetter( "_S_autorFiltersEnable", this ) ).
		          TOOLTIP( "Customized filters will be automatically applied to objects. You can disable auto mode." );
		          DRAW_STACK.SPACE();
		          using ( ENABLE.USE( GetSetter( "_S_autorFiltersEnable", this ) ) )
		          {   DRAW_STACK.ACTION( SETTINGS_DRAW_FILTERS_ACTION, true );
		          }
		      }
		  }*/
	}
	
	
	
	
	
	
	
	void SETTINGS_DRAW_FILTERS_ACTION()
	{	var f = M_Colors_Window.GetFH(prefWindow);
		var F_RECT = EditorGUILayout.GetControlRect( GUILayout.Height(0) );
		F_RECT.height = 330;
		F_RECT.x += 10;
		F_RECT.width -= 40;
		M_Colors_Window.CHANGE_GUI( this );
		f.source = null;
		var h =  f.DrawFilts(F_RECT, this);
		EditorGUILayout.GetControlRect( GUILayout.Height( h ) );
		M_Colors_Window.RESTORE_GUI();
	}
	internal void SETTINGS_HIGHLIGHTER_AUTO( string searchString )
	{
	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	string ROWS_CONTENT  = "Category Name Sort Order Cells and Rows Amount Buttons Cells Amount Apply Highlighter Colors Background Color";
	GUIContent CatName = new GUIContent() { tooltip = "Category Name" };
	GUIContent EnableDisable = new GUIContent() { tooltip = "Enable/Disable" };
	GUIContent Sorting = new GUIContent() { tooltip = "Sort Order" };
	GUIContent SortingUP = new GUIContent() { tooltip = "Sort Order", text = "▲" };
	GUIContent SortingDOWN = new GUIContent() { tooltip = "Sort Order", text = "▼" };
	//    GUIContent CellsRowsCount = new GUIContent() { tooltip = "Cells and Rows Amount" };
	GUIContent ButtonsCount = new GUIContent() { tooltip = "Buttons Cells Amount" };
	GUIContent RowsCount = new GUIContent() { tooltip = "Rows Amount" };
	GUIContent higlighterColor = new GUIContent() { tooltip = "Apply Highlighter Colors" };
	GUIContent backgroundColor = new GUIContent() { tooltip = "Background Color" };
	internal void SETTINGS_BOTTOM_MAIN( string searchString )
	{
	
	
		NEW_HEADER( "BOTTOMHELP", searchString );
		DRAW_STACK.DRAW_WIKI_BUTTON( "Bottom Panel", "Navigation Module" );
		
		
		
		
		
		
		
		
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE_GROUP( "Enable Bottom Bar", GetSetter( "ENABLE_BOTTOMDOCK", this, true ) );
		
		
		
		
		
			using ( ENABLE.USE( GetSetter( "ENABLE_BOTTOMDOCK", this, true ) ) )
			{	//  using ( GRO.UP( "Bookmarks Ordering:" ) ) {
				DRAW_STACK.SLIDER( "Bottom Bar Contrast", 0, 1, GetSetter( "BOTTOM_CONTRAST", this ) );
			}
		}
		
		
		using ( GRO.UP() )
		{	using ( ENABLE.USE( GetSetter( "ENABLE_BOTTOMDOCK", this, true ) ) )
			{	if (pluginID == 0)
				{	DRAW_STACK.TOGGLE( "Shift to Instantiate Clicked Object", GetSetter( "SHIFT_TO_INSTANTIATE_BOTTOM", this ) ).
					TOOLTIP("Hold down the shift and click the bookmark you want to duplicate");
					
					using ( ENABLE.USE( GetSetter( "SHIFT_TO_INSTANTIATE_BOTTOM", this, true ) ) )
					{	DRAW_STACK.TOGGLE_FROM_INT( "Instantiate And Move To View", GetSetter( "INSTANTIATE_MODE", this ), 1, 0 );
					}
				}
			}
		}
		
		using ( GRO.UP() )
		{	using ( ENABLE.USE( GetSetter( "ENABLE_BOTTOMDOCK", this, true ) ) )
			{	DRAW_STACK.SEARCH_SET( "Bookmarks Ordering:" );
				DRAW_STACK.LABEL( "Bookmarks Ordering:" );
				DRAW_STACK.ACTION( DRAW_BOTTOM_BUTTONS, false );
				DRAW_STACK.SEARCH_SET( null );
				DRAW_STACK.HR();
				// }
				DRAW_STACK.PADDING_SET( EditorGUIUtility.singleLineHeight * 2 );
				DRAW_STACK.VAR_FIELD( "Additional Height of Bookmarks", -10, 20, GetSetter( "ADDITIONAL_BOOKMARKS_HEIGHT", this, true ) );
				DRAW_STACK.TOGGLE( "Bookmarks <b>Tooltips</b>", GetSetter( "BOTTOM_TOOLTIPES", this ) ).
				TOOLTIP( "Hover mouse cursor to get a tooltip." );
				DRAW_STACK.PADDING_SET( 0 );
				DRAW_STACK.HR();
				//  }
				// using ( GRO.UP( "Rows Ordering:" + ROWS_CONTENT ) ) {
				DRAW_STACK.SEARCH_SET( "Rows Ordering:" + ROWS_CONTENT );
				DRAW_STACK.LABEL( "Rows Ordering:" );
				DRAW_STACK.SEARCH_SET( null );
				DRAW_STACK.ACTION( DRAW_ROWS, false );
				
			}
		}
		
		using ( ENABLE.USE( GetSetter( "ENABLE_BOTTOMDOCK", this, true ) ) )
		{	//                 using ( GRO.UP() ) {
			//
			//
			//                 }
			if ( string.IsNullOrEmpty( searchString ) )
			{	DRAW_STACK.HR();
				DRAW_STACK.BUTTON( "Open Bottom Quick Help Tab", () =>
				{	overrideIndex = 4;
					overrideLastValue = 3;
				}, (int)EditorGUIUtility.singleLineHeight * 2 );
				DRAW_STACK.HR();
				
			}
			
			/*  using ( GRO.UP() )
			  {   DRAW_STACK.TOGGLE_GROUP( "Draw <b>Parents List</b> for Current Selection", GetSetter( "SHOW_PARENT_TREE", this, true ) );
			      DRAW_STACK.HELP_TEXTURE( "NEW_BOTTOM_SETUP_PARENTS" );
			      using ( ENABLE.USE( GetSetter( "SHOW_PARENT_TREE", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
			      {   DRAW_STACK.TOGGLE( "Don't Draw <b>Current Selection</b>", GetSetter( "DONTSHOW_PARENT_TREE_CURRENTOBJECT", this ) );
			      }
			  }*/
			
			using ( GRO.UP() )
			{	DRAW_STACK.TOGGLE( "Always save the last selections", GetSetter( "SAVE_LASTSELECTION_BEFORESCENEWILLSCLOSE", this ) ).
				TOOLTIP( "If this option is enabled, the scene will be marked dirty if the selection will change." );
			}
			
			
			DRAW_STACK.HEADER( "HyperGraph" );
			//  DrawHeader( "HyperGraph" + (Adapter.LITE ? " (Pro Only)" : "") );
			DRAW_STACK.DRAW_WIKI_BUTTON( "Bottom Panel", "HyperGraph" );
			
			
			using ( GRO.UP() )
			{	DRAW_STACK.TOGGLE( "<b>Auto-Hide</b> when changing selection", GetSetter( "AUTOHIDE", this ) );
			
				using ( ENABLE.USE( GetSetter( "NOTAUTOHIDE", this ) ) )
				{	DRAW_STACK.TOGGLE( "<b>Auto-Reload</b> when changing selection", GetSetter( "AUTOCHANGE", this ) ).
					TOOLTIP( "You can drag and drop objects in and out of the HyperGraph" );
				}
			}
			
			using ( GRO.UP( "Reloading Performance" ) )
			{	DRAW_STACK.LABEL( "<i>Reloading Performance:</i>" );
				DRAW_STACK.ACTION( HYPER_RELOADPERFOMANCE, false );
			}
			
			using ( GRO.UP() )
			{	DRAW_STACK.TOGGLE( "Show 'Spinner' while Loading", GetSetter( "SHOWUPDATINGINDICATOR", this ) );
			}
			
			using ( GRO.UP() )
			{	DRAW_STACK.TOGGLE( "Red color for unassigned variables", GetSetter( "RED_HIGKLIGHTING", this ) );
			}
			
		}
		
		
		
		
	}
	
	
	
	internal void SETTINGS_BOTTOM_HELP( string searchString )
	{	// DRAW_HELP_TEXTURE( "ITEMS_PER_ROW", 137, true, 0 );
		//   Space( 10 );
		//  HelpBox( "Use Control+CLICK or Alt+CLICK to Merge or to Except selections", MessageType.Info );
		
		
		
		if ( string.IsNullOrEmpty( searchString ) )
		{	DRAW_STACK.BUTTON( "Close Bottom Quick Help Tab", () =>
			{	overrideIndex = null;
			}, (int)EditorGUIUtility.singleLineHeight * 2 );
			
			DRAW_STACK.HR();
		}
		
		
		
		using ( GRO.UP() )
		{	DRAW_STACK.HEADER( "Manipulations" );
			DRAW_STACK.HELP_TEXTURE( "BOTTOMHELP" );
			
			if ( IS_HIERARCHY() )
			{	DRAW_STACK.HELP_BOX( "Use Ctrl+Shift+Z / Ctrl+Shift+Y to quickly switch between recent selections" );
			}
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.HEADER( "Selection" );
			DRAW_STACK.PADDING_SET( 30 );
			DRAW_STACK.HELP_TEXTURE( "HOT_CONTROL" );
			DRAW_STACK.HELP_BOX( "CLICK to add/remove object from current selection" );
			DRAW_STACK.HELP_TEXTURE( "HOT_SHIFT" );
			DRAW_STACK.HELP_BOX( "CLICK to add object to current selection" );
			DRAW_STACK.HELP_TEXTURE( "HOT_ALT" );
			DRAW_STACK.HELP_BOX( "CLICK to select object and keep scroll position for hierarchy window" );
			DRAW_STACK.PADDING_SET( 0 );
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.HEADER( "Scenes" );
			DRAW_STACK.HELP_TEXTURE( "HELP_SCENEPIN" );
			DRAW_STACK.HELP_BOX( " - Use the '+' button to add more than one scene" );
			DRAW_STACK.HELP_BOX( " - Tap on the scene icon to pin button" );
			DRAW_STACK.HELP_BOX( " - Use 'shift+CLICK' to load scene as additional" );
			DRAW_STACK.HELP_BOX( " - Use 'ctrl+CLICK' to revival scene in project view" );
		}
		
		/*if ( IS_HIERARCHY() ) {
		    // DRAW_HELP_TEXTURE( "ITEMS_PER_ROW" , 40 , true , 0 , -137 );
		    DRAW_STACK.HELP_TEXTURE( "ITEMS_PER_ROW" , 40 , null , -137 ).HELP_BOX( "Use Ctrl+Shift+Z / Ctrl+Shift+Y to quickly switch between recent selections" );
		}*/
		
		//  DrawHeader("Hierarchy SnapShots");
		using ( GRO.UP() )
		{	DRAW_STACK.HEADER( "Expanded items" );
			DRAW_STACK.HELP_TEXTURE( "HIER_HELP" );
			DRAW_STACK.HELP_BOX( "Use the '+' button to save expanded items in hierarchy" );
		}
		
		using ( GRO.UP() )
		{	DRAW_STACK.HEADER( "HyperGraph" );
			DRAW_STACK.HELP_TEXTURE( "HELP_HIPERGRAPH" /*, 221*/ );
			DRAW_STACK.HELP_BOX( "You can track unity events, for that activate the corresponding toggle" );
		}
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	bool RightEnableChanged( object value )
	{	var newR = (bool) value;
	
		if ( newR ) par.RIGHTDOCK_TEMPHIDE = false;
		
		return true;
	}
	
	bool RoundToIntRightWidhththt( object value )
	{	par.RIGHTDOCK_TEMPHIDEMINWIDTH = Mathf.RoundToInt( (float)value );
		DRAW_STACK.ValueChanged();
		return false;
	}
	
	internal void SETTINGS_RIGHTMAIN( string searchString )
	{
	
		// DRAW_HELP_TEXTURE( A.par.COMPONENTS_NEXT_TO_NAME ? "COMPONENTNEXTTO" : "COMPONENTNEXTTO2", 100, /*Hierarchy.par.COMPONENTS_NEXT_TO_NAME*/ true );
		
		NEW_HEADER( par.COMPONENTS_NEXT_TO_NAME ? "COMPONENTNEXTTO" : "COMPONENTNEXTTO2", searchString );
		DRAW_STACK.DRAW_WIKI_BUTTON( "Right Panel", "Components" );
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE_GROUP( "Enable Right Bar", GetSetter( "ENABLE_RIGHTDOCK_FIX", this, true ) ).ON_CHANGE_CONFORM( RightEnableChanged );
		
			using ( ENABLE.USE( GetSetter( "ENABLE_RIGHTDOCK_FIX", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
			{
			
				DRAW_STACK.TOGGLE( "Bind Header To The Top", GetSetter( "HEADER_BIND_TO_SCENE_LINE", this, true ) );    //defaultStyle: true
				
				DRAW_STACK.SLIDER( "Right Padding", 200, -100, GetSetter( "PADDING_RIGHT_SETTINGS", this ) );
				DRAW_STACK.TOGGLE( "Right Padding affect non-draggable Categories", GetSetter( "PADDING_RIGHT_MoveSetActiveToo", this, true ) );
				
				
				//DRAW_HELP_TEXTURE( "BINDHEADERHELP" , 41 , A.par.HEADER_BIND_TO_SCENE_LINE );
				if ( IS_PROJECT() )
				{	DRAW_STACK.TOGGLE( "'*.*' Extension Next To The Object Name", GetSetter( "COMPONENTS_NEXT_TO_NAME", this, true ) );    //defaultStyle: true
				}
				
				DRAW_STACK.HR();
				
				
				DRAW_STACK.TOGGLE( "Show 'Right Bar' in PlayMode", GetSetter( "PLAYMODE_ShowRightPanel", this ) );
				
				using ( ENABLE.USE( GetSetter( "PLAYMODE_ShowRightPanel", this ) ) )
				{	if ( !IS_PROJECT() )
					{	DRAW_STACK.TOGGLE( "Show 'Components Icons' in PlayMode", GetSetter( "PLAYMODE_ShowComponents2", this ) );
					
						if ( !NEW_PERFOMANCE )
						{	using ( ENABLE.USE( GetSetter( "PLAYMODE_ShowComponents2", this ) ) )
							{	DRAW_STACK.TOGGLE( "Use 'Baked Components' in PlayMode", GetSetter( "PLAYMODE_UseBakedComponents", this, true ) ).
								TOOLTIP( "Using baked components improves performance in PlayMode" );
							}
						}
					}
				}
			}
		}//if - Enable Right Bar
		
		
		
		using ( ENABLE.USE( GetSetter( "ENABLE_RIGHTDOCK_FIX", this, true ) ) )
		{
		
			using ( GRO.UP() )
			{
			
			
				DRAW_STACK.TOGGLE( "Display labels for empty contents", GetSetter( "DRAW_LABEL_FOR_EMPTY_CONTENT", this ) );
				
				DRAW_STACK.TOGGLE_GROUP( "Hide Modules with a small window size", GetSetter( "RIGHTDOCK_TEMPHIDE", this, true ) );
				
				using ( ENABLE.USE( GetSetter( "RIGHTDOCK_TEMPHIDE", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
				{	DRAW_STACK.VAR_FIELD( "If " + pluginname + " Width < ", GetSetter( "RIGHTDOCK_TEMPHIDEMINWIDTH", this ), 100, 500 );
				}
				
				DRAW_STACK.HR();
				
				DRAW_STACK.BUTTON( "Goto Other hide settings", () => { SHOW_HIER_SETTINGS_DEFAULT( pluginID ); }, Mathf.RoundToInt( EditorGUIUtility.singleLineHeight * 1.5f ) );
				
			}
		}
		
		if ( !IS_PROJECT() )
		{	using ( ENABLE.USE( GetSetter( "ENABLE_RIGHTDOCK_FIX", this, true ) ) )
			{
			
				using ( GRO.UP("") )
				{	DRAW_STACK.HEADER( "Module - SetActive" );
					DrawGameOjbects();
				}
				
				DRAW_STACK.HEADER( "Module - Icons" );
				DRAW_STACK.DRAW_WIKI_BUTTON( "Right Panel", "Components" );
				
				using ( GRO.UP() )
				{	DRAW_STACK.TOGGLE_GROUP( "Draw BG for Mono Icon", GetSetter( "DRAW_ICONS_MONO_BG", this ) );
					DRAW_STACK.ICON( "MONOCLEAN", GetSetter( "DRAW_ICONS_MONO_BG", this ) );
					
					using ( ENABLE.USE( GetSetter( "DRAW_ICONS_MONO_BG_INVERCE", this) ) )
					{	DRAW_STACK.TOGGLE_GROUP( "Draw Second Char for MonoBehaviour Names", GetSetter( "DRAW_SECOND_CHAR_FOR_MONO", this ) );
						DRAW_STACK.ICON( "NEW", null );
					}
					
					DRAW_STACK.TOGGLE_GROUP( "Draw Shadows for Icons", GetSetter( "DRAW_ICONS_SHADOW", this ) );
				}
				
				using ( GRO.UP() )
				{	DRAW_STACK.TOGGLE_GROUP( "Component's Icons Next To The Object Name", GetSetter( "COMPONENTS_NEXT_TO_NAME", this, true ) ).TOOLTIP
					( "(Use Ctrl+LEFT DRAG to drag component to inspector, for example" );
					DRAW_STACK.PADDING_SET( EditorGUIUtility.singleLineHeight * 4 );
					
					using ( ENABLE.USE( GetSetter( "COMPONENTS_NEXT_TO_NAME", this, true ) ) )
					{	DRAW_STACK.VAR_FIELD( "Left Padding:", GetSetter( "COMPONENTS_NEXT_TO_NAME_PADDING", this, true ), -40, 100 );
					}
					
					DRAW_STACK.VAR_FIELD( "Space Between Icons:", GetSetter( "ICONSPACE", this, true ), -8, 8 );
					DRAW_STACK.PADDING_SET( 0 );
				}
				
				
				
				// DRAW_STACK.HR();
				using ( GRO.UP() )
				{	DRAW_STACK.SEARCH_SET( "Unity IconsUDefault Icons Hidden Icons" );
					DRAW_STACK.TOGGLE( "☰ Enable Unity <b>Default</b> Icons ☰", GetSetter( "FD_Icons_default", this, true ) );
					DRAW_STACK.ICON( EditorGUIUtility.ObjectContent( null, typeof( BoxCollider ) ).image, null );
					
					using ( ENABLE.USE( GetSetter( "FD_Icons_default", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
					{	DRAW_STACK.LABEL( "<i>Hidden Unity Components Icons:</i>" );
						DRAW_STACK.ACTION( DrawHidenIcons, false );
					}
					
					DRAW_STACK.SEARCH_SET( null );
					DRAW_STACK.HR();
					
					DRAW_STACK.SEARCH_SET( "MonoBehaviour IconsUMono Icons Grouping" );
					DRAW_STACK.TOGGLE( "☰ Enable <b>MonoBehaviour</b> Scripts Icons ☰", GetSetter( "FD_Icons_mono", this, true ) );
					DRAW_STACK.ICON( "MONO", null );
					
					using ( ENABLE.USE( GetSetter( "FD_Icons_mono", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
					{	DRAW_STACK.LABEL( "<i>Grouping MonoBehaviour Icons:</i>" );
						DRAW_STACK.ACTION( DrawGroupMonoIcons, false );
					}
					
					DRAW_STACK.SEARCH_SET( null );
					DRAW_STACK.HR();
					
					DRAW_STACK.SEARCH_SET( "User IconsUsers Icons" );
					DRAW_STACK.TOGGLE( "☰ Enable <b>User's</b> Icons ☰", GetSetter( "FD_Icons_user", this, true ) );
					DRAW_STACK.ICON( "MY", null );
					
					using ( ENABLE.USE( GetSetter( "FD_Icons_user", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
					{	//  DRAW_STACK.LABEL( "<i>Grouping MonoBehaviour Icons:</i>" );
						DRAW_STACK.HEADER( "Additional Custom MonoScript Icons" );
						DRAW_STACK.ACTION( DrawUserIcons, false );
					}
					
					DRAW_STACK.SEARCH_SET( null );
					
					// CHECK
					var newD =  par.FD_Icons_default  ;
					var newM =   par.FD_Icons_mono ;
					var newU =  par.FD_Icons_user  ;
					
					if ( !newD && !newU && !newM )
					{	if ( par.FD_Icons_default || par.FD_Icons_user )
						{	par.FD_Icons_user = !(par.FD_Icons_default = par.FD_Icons_user);
							par.FD_Icons_mono = false;
						}
						
						else
						{	par.FD_Icons_default = true;
							par.FD_Icons_mono = false;
							par.FD_Icons_user = true;
						}
					}
					
					else
					{	par.FD_Icons_default = newD;
						par.FD_Icons_mono = newM;
						par.FD_Icons_user = newU;
					}
					
					// CHECK
				}
				
				
			}//if - Enable Right Bar
			
		}
		
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	internal void SETTINGS_RIGHTMAIN_MODULE0( string searchString )
	{
	
		NEW_HEADER( "HELP_ATTRIBUTES", searchString );
		DRAW_STACK.DRAW_WIKI_BUTTON( "Right Panel", "Components" );
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE( "☰ Enable display of <b>Functions</b> ☰", GetSetter( "COMP_ATTRIBUTES_BUTTONS", this, true ) );
			DRAW_STACK.TOGGLE( "☰ Enable display of <B>Variables</b> ☰", GetSetter( "COMP_ATTRIBUTES_FIELDS", this, true ) );
			
			using ( ENABLE.USE( GetSetter( "COMP_ATTRIBUTES_FIELDS", this, true ), EditorGUIUtility.singleLineHeight * 4 ) )
			{	DRAW_STACK.TOGGLE( "Red color for unassigned variables", GetSetter( "DISPLAYING_NULLSISRED", this, true ) );
			}
		}
		
		
		// DRAW_STACK.HELP_TEXTURE( "HELP_ATTRIBUTES" , 47 );
		using ( GRO.UP() )
		{	DRAW_STACK.HELP_BOX( "Use '[SHOW_IN_HIER]' attribute in your code" );
			DRAW_STACK.HELP_TEXTURE( "HELP_ATTRIBUTES_VISUAL", 47 );
			DRAW_STACK.HELP_BOX( "There's Code Example inside 'FunctionsDisplaying_Example.cs" );
		}
		
	}
	
	
	
	void DrawDataKeeperScripts()
	{	//  var boxRect = DRAW_GREENLINE_ANDGETRECT( ref LAST_RECT, A.par.DataKeeperParams.USE_SCRIPTS, true );
		var boxRect =  EditorGUILayout.GetControlRect(GUILayout.Height(0));
		boxRect.x += 4;
		//  boxRect.y += boxRect.height;
		boxRect.height = PKC.KEEPER_HEIGHT + 16;
		
		var  R = boxRect;
		R.height = 25;
		R.y += 10;
		
		boxRect.width -= 10;
		Adapter.INTERNAL_BOX( boxRect, "" );
		
		// R = EditorGUILayout.GetControlRect( GUILayout.Height( PKC.KEEPER_HEIGHT ) );
		R = boxRect;
		R.x += 8;
		R.y += 8;
		R.width -= 16;
		R.height -= 16;
		
		PKC.DRAW_KEEPER_SCRIPTS( prefWindow, R );
		PKC.KEEPER_UPDATE( prefWindow );
		
		GUILayout.Space( boxRect.height );
	}
	internal void SETTINGS_RIGHTMAIN_MODULE1( string searchString )
	{	NEW_HEADER( GetIcon( "HELP_FIXED_KEEPER" ), searchString );
		DRAW_STACK.DRAW_WIKI_BUTTON( "Right Panel", "PlayMode Data Keeper" );
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE_GROUP( "☰ Enable <b>PlayMode Data Keeper</b> Module ☰", GetSetter( "DataKeeperParamsENABLE", this ) ).
			TOOLTIP( "Turn off this module to disable persisting for all objects" );
			
			using ( ENABLE.USE( GetSetter( "DataKeeperParamsENABLE", this ), EditorGUIUtility.singleLineHeight * 4 ) )
			{
			
			
				DRAW_STACK.TOGGLE( "Save Variables typeof UnityEngine.Object", GetSetter( "DataKeeperParamsSAVE_UNITYOBJECT", this ) );
				DRAW_STACK.HELP_BOX(
				    @"Classes references, such as MonoBehaviour or other Unity Components, needs enabled special serialization,
 this feature allows you to save references to all serialized Classes inherited from Unity.Object." );

				DRAW_STACK.TOGGLE( "Save Active State for GameObjects", GetSetter( "DataKeeperParamsSAVE_ENABLINGDISABLING_GAMEOBJEST", this ) );
				DRAW_STACK.HELP_BOX( "if any script changes the active state in PlayMode, the changes will be saved after stopping also PlayMode" );
				
				DRAW_STACK.TOGGLE( "Save Added/Removed components (Experimental)", GetSetter( "DataKeeperParamsSAVE_GAMEOBJET_HIERARCHY", this ) );
				DRAW_STACK.HELP_BOX( "if you delete or add the components selected for persisting, the changes will be saved after stopping also PlayMode" );
				
				
			}
		}
		
		using ( ENABLE.USE( GetSetter( "DataKeeperParamsENABLE", this ) ) )
		{	using ( GRO.UP() )
			{	DRAW_STACK.TOGGLE( "Always Enable Keeper for scripts below", GetSetter( "DataKeeperParamsUSE_SCRIPTS", this ) );
			
				using ( ENABLE.USE( GetSetter( "DataKeeperParamsUSE_SCRIPTS", this ) ) )
				{	DRAW_STACK.ACTION( DrawDataKeeperScripts, false );
				}
			}
		}
	}
	
	
	internal void SETTINGS_RIGHTMAIN_MODULE2( string searchString )
	{	if ( IS_HIERARCHY() )
		{	NEW_HEADER( "HELP_OPTIMIZER", searchString, -16 );
			DRAW_STACK.DRAW_WIKI_BUTTON( "Right Panel", "Memory Optimizer" );
			
			// DRAW_STACK.HELP_TEXTURE( "HELP_OPTIMIZER" );
			
			using ( GRO.UP( "Reloading Performance" ) )
			{	DRAW_STACK.LABEL( "<i>Broadcasting Performance:</i>" );
				DRAW_STACK.VAR_FIELD( "Reloading Performance:", GetSetter( "BROADCASTING_PREFOMANCE01", this ), 5, 95, "%" );
			}
			
			DRAW_STACK.HELP_TEXTURE( "BROADCASTHELP", 48 );
			
			DRAW_STACK.HELP_BOX( "(High values may reduce performance)", MessageType.Warning );
		}
		
		else
		{	DRAW_STACK.HELP_BOX( "This module is not available right now", MessageType.Warning );
		}
	}
	
	
	
	void DrawCustomModules()
	{	var  HH = 95;
		var  R = EditorGUILayout.GetControlRect( GUILayout.Height( HH ) );
		GUILayout.Space( EditorGUIUtility.singleLineHeight );
		var H3 = 325;
		var R2 = EditorGUILayout.GetControlRect(GUILayout.Height(H3));
		_W__ModulesWindow.DrawCustomModules1( this, R, R2 );
		DRAW_STACK.HELP_BOX( "Source code of default modules are placed into: '" + PluginInternalFolder + "/CustomModule_Example.cs'", MessageType.None );
		_W__ModulesWindow.DrawCustomModules2( this, R, R2 );
	}
	internal void SETTINGS_RIGHTMAIN_MODULE3( string searchString )
	{
	
		NEW_HEADER( "HELP_ATTRIBUTES", searchString );
		DRAW_STACK.DRAW_WIKI_BUTTON( "Right Panel", "Custom Modules" );
		
		using ( GRO.UP() )
		{	DRAW_STACK.TOGGLE_GROUP( "Use Custom Modules:", GetSetter( "USE_CUSTOMMODULES", this, true ) );
			DRAW_STACK.ACTION( DrawCustomModules, false );
		}
		
	}
}




}
