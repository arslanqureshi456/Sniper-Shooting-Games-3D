#if UNITY_EDITOR
	#define HIERARCHY
	#define PROJECT
#endif


using System;
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

	internal void SAVE_SCROLL()
	{	var scroll  = GetHierarchyWindowScrollPos();
		//if (scroll != Vector2.zero)
		{	PushActionToUpdate( () =>
			{	PushActionToUpdate( () =>     /*if (!IMGUI20183())*/
				{	ResetScroll( scroll );
					//Debug.Log(scroll);
				} );
			} );
		}
	}
	
	internal bool OneFrameActionOnUpdate = false;
	internal Action OneFrameActionOnUpdateAC;
	internal void PushActionToUpdate( Action ac )
	{	OneFrameActionOnUpdate = true;
		OneFrameActionOnUpdateAC += ac;
	}
	
	
	
	void UpdateCb()
	{	if ( !wasModulesInitialize ) return;
	
		// if (par.ENABLE_ALL) Loopinit();
		// MonoBehaviour.print(!par.ENABLE_ALL + " " + tempBlock + " " + !Loopinit());
		// if () return;
		// MonoBehaviour.print("123");
		
		if (repaintImmidiately.Count != 0 )
		{	foreach ( var item in repaintImmidiately )
			{	if ( !item ) continue;
			
				(typeof( EditorWindow )).GetMethod( "RepaintImmediately", (BindingFlags)(-1) ).Invoke( item, null );
			}
			
			repaintImmidiately.Clear();
		}
		
		if ( OneFrameActionOnUpdate )
		{	OneFrameActionOnUpdate = false;
			var copy = OneFrameActionOnUpdateAC;
			OneFrameActionOnUpdateAC = null;
			copy();
		}
		
		backedSelectColor = null;
		
		
		
		if ( !applicationIsPlaying.HasValue ) applicationIsPlaying = Application.isPlaying;
		
		if ( applicationIsPlaying != Application.isPlaying )
		{	applicationIsPlaying = Application.isPlaying;
			ReloadEd = true;
			//SWITCHER_RELOAD_DATA( this );
		}
		
		
		if ( CHILD_GROUP_FIX_FLAG ) CHILD_GROUP_FIX_FLAG = false;
		
		// if (WAIT_EDITOR || string.IsNullOrEmpty(EditorSceneManager.GetActiveScene().path)) return;
		
		// if (!window()) return;
		//  if (Input.GetKey(KeyCode.A)) MonoBehaviour.print(Input.anyKey);
		if ( pluginID == 0 )
			Hierarchy. M_CustomIcons.useEvent = null;
			
		paddinguseEvent = null;
		SKIP_PREFAB_ESCAPE = false;
		
		foreach ( var r in bottomInterface.Keys )
		{	bottomInterface.NEED_READ_LIST[r] = true;
		}
		
		if ( bottomInterface.Keys.Count > 100 )
		{	bottomInterface.Keys.Clear();
			bottomInterface.NEED_READ_LIST.Clear();
			
		}
		
		DrawHeadRepaint = null;
		DrawHeaderOther = null;
		
		// tree_cache.Clear();
		//  MonoBehaviour.print("ASD");
		// DrawHeadind = 0;
		try
		{
		
		
			if ( !par.ENABLE_ALL || !INIT_AdapterRepeatableInitializationFunction() || tempAdapterBlock )
			{	bottomInterface.LastIndex = -1;
				return;
			}
			
			/*foreach ( var nc in ColorModule.new_child_cache )
			{   nc.Value.wasInit = false;
			}*/
			
			
			MOI.InitModules();
			Hierarchy_GUI.Instance( this );
			/* if (window() != null)
			 {
			     var field = window().GetType().GetField("m_LastUserInteractionTime", (BindingFlags)(-1));
			     if (field != null)
			     {
			         //                    MonoBehaviour.print(EditorApplication.timeSinceStartup + "  " + field.GetValue(window()));
			         field.SetValue(window(), 0);
			        // MonoBehaviour.print("SET");
			     }
			 }*/
			
			// if (!wasFirstUpdate)
			{	// wasFirstUpdate = true;
				if ( !wasIconsInitialize ) InitializeIcons();
			}
			
			//   Counter();
			if ( lastTime == 0 ) lastTime = EditorApplication.timeSinceStartup;
			
			deltaTime = (float)(EditorApplication.timeSinceStartup - lastTime);
			lastTime = EditorApplication.timeSinceStartup;
			
			if ( deltaTime > 0.033f ) deltaTime = 0.033f;
			
			// MonoBehaviour.print(deltaTime);
			
			m_ToolTipTime += deltaTime;
			// if (Application.isPlaying) return;
			var t = modulesOrdered;
			//__modulesOrdered = modules.Where(m => m.sibbildPos != -1).OrderBy(m => m.sibbildPos).ToArray();
			
			
			MOI.M_Vertices.CalcBroadCast();
			
			
			bottomInterface.Updater();
			CheckMouseEventUpdater();
			
			ENABLE_HOVER_ITEMS = true;
			
			
			
			if ( needRepaint )     //Hierarchy.RepaintAllViews();
			{	RepaintWindow(true);
				needRepaint = false;
			}
		}
		
		catch ( Exception ex )
		{	logProxy.LogError( ex.Message + " " + ex.StackTrace );
		
		}
		
		//  INIT_PADING();
		
		
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	private const int defWDTH = 8;
	internal Color colorStatic = new Color(Color.gray.r, Color.gray.g, Color.gray.b, Color.gray.a / 4);
	/*  private Texture2D secA,
	          secB;*/
	Color  secA =  Adapter.LINE ;
	Color       secB =  new Color( Adapter.LINE.a, Adapter.LINE.r, Adapter.LINE.b, Adapter.LINE.a / 3 ) ;
	
	
	
	internal Color sec { get { return EditorGUIUtility.isProSkin ? secA : secB; } }
	// private Scene _lastScene;
	internal string[] DefaulTypes;
	// private string m_ToolTip = "";
	private float m_ToolTipTime = 0;
	internal double lastTime = 0;
	private int resetcount = 0;
	internal Module[] __modulesOrdered = null;
	private Rect r;
	private Rect r2;
	private Rect r3;
	
	internal Module[] modules;
	
	private bool wasWarning = false;
	
	private int? DrawHeadRepaint,
			DrawHeaderOther;
	// private float RIGHTPAD = 30;
	private float RIGHTPAD { get { return 0; } }
	
	private bool allow = false;
	
	private Dictionary<EditorWindow, Dictionary<Module, Rect>> ___CurrentRect =
		new Dictionary<EditorWindow, Dictionary<Module, Rect>>();
		
	private Rect DragRect;
	private Module curMod;
	
	private EditorWindow mouseEventW;
	
	
	internal float DefaultBGOpacity = 0.3f;
	private GUIContent ObjectNameCOntent = new GUIContent();
	
	Color colorText11 = Color.gray;
	/*internal Texture2D colorText11
	{   get
	    {   if (_colorText11 == null) return Texture2D.blackTexture;
	        return _colorText11;
	    }
	}*/
	
	
	
	private Module[] modulesOrdered
	{	get
		{	if ( __modulesOrdered == null )
			{	__modulesOrdered = modules.Where( m => m.sibbildPos != -1 ).OrderBy( m => m.sibbildPos ).ToArray();
				// MonoBehaviour.print(__modulesOrdered.Length);
				/*foreach ( var item in modules.Where( m => m.disableSib) )
				{   ArrayUtility.Insert( ref __modulesOrdered, 0, item );
				}*/
				var playKeeper = modules.FirstOrDefault(m => m is EModules.EModulesInternal.Hierarchy.M_PlayModeKeeper);
				
				if ( playKeeper != null ) ArrayUtility.Insert( ref __modulesOrdered, 0, playKeeper );
				
				// MonoBehaviour.print(__modulesOrdered.Length);
				// UpdateCb();
			}
			
			return __modulesOrdered;
		}
	}
	
	private bool ENABLED_OPTIMIZER
	{	get
		{	return VerticesModule != null && VerticesModule.enable &&
				   (par.BroadCastOptimizer || par.VerticesModuleType == VerticesModuleTypeEnum.TextureMemory);
		}
	}
	
	internal Module VerticesModule;
	
	
	internal float padding_right
	{
	
		get
		{	if ( IS_HIERARCHY() ) return M_SetActive_WIDTH /*+ (par.DataKeeperParams.ENABLE ? M_PlayModeKeeper.WIDTH : 0)*/ + par.PADDING_RIGHT;
		
			return par.PADDING_RIGHT;
		}
	}
	
	private void WriteT( Transform t )     // markedObjects.Add(t.GetInstanceID(), t.gameObject);
	{	for ( int i = 0,
				len = t.childCount ; i < len ; i++ ) WriteT( t.GetChild( i ) );
	}
	
	
	
	Color c;
	
	internal Color CHESS_COLOR
	{	get
		{	if ( EditorGUIUtility.isProSkin )
			{	if ( par.BG_LINE_CHESS_COLOR_PRO != null )
				{	c.r = par.BG_LINE_CHESS_COLOR_PRO.Value.r;
					c.g = par.BG_LINE_CHESS_COLOR_PRO.Value.g;
					c.b = par.BG_LINE_CHESS_COLOR_PRO.Value.b;
					c.a = par.BG_LINE_CHESS_COLOR_PRO.Value.a;
					return c;
				}
				
				var f = 20 / 255f;
				return new Color( f, f, f, 0.1F );
			}
			
			else
			{	if ( par.BG_LINE_CHESS_COLOR_PERSONAL != null )
				{	c.r = par.BG_LINE_CHESS_COLOR_PERSONAL.Value.r;
					c.g = par.BG_LINE_CHESS_COLOR_PERSONAL.Value.g;
					c.b = par.BG_LINE_CHESS_COLOR_PERSONAL.Value.b;
					c.a = par.BG_LINE_CHESS_COLOR_PERSONAL.Value.a;
					return c;
				}
				
				return new Color( .1f, .1f, .1f, 0.1F / 3 );
			}
		}
		
		set
		{	if ( EditorGUIUtility.isProSkin )
			{	par.BG_LINE_CHESS_COLOR_PRO = new SerializeColor( value.r, value.g, value.b, value.a );
			}
			
			else
			{	par.BG_LINE_CHESS_COLOR_PERSONAL = new SerializeColor( value.r, value.g, value.b, value.a );
			}
		}
	}
	
	internal Color HR_COLOR
	{	get
		{	if ( EditorGUIUtility.isProSkin )
			{	if ( par.BG_LINE_HR_COLOR_PRO != null )
				{	c.r = par.BG_LINE_HR_COLOR_PRO.Value.r;
					c.g = par.BG_LINE_HR_COLOR_PRO.Value.g;
					c.b = par.BG_LINE_HR_COLOR_PRO.Value.b;
					c.a = par.BG_LINE_HR_COLOR_PRO.Value.a;
					return c;
				}
				
				return new Color( .1f, .1f, .1f, 0.1F );
			}
			
			else
			{	if ( par.BG_LINE_HR_COLOR_PERSONAL != null )
				{	c.r = par.BG_LINE_HR_COLOR_PERSONAL.Value.r;
					c.g = par.BG_LINE_HR_COLOR_PERSONAL.Value.g;
					c.b = par.BG_LINE_HR_COLOR_PERSONAL.Value.b;
					c.a = par.BG_LINE_HR_COLOR_PERSONAL.Value.a;
					return c;
				}
				
				return new Color( .1f, .1f, .1f, 0.1F / 3 );
			}
		}
		
		set
		{	if ( EditorGUIUtility.isProSkin )
			{	par.BG_LINE_HR_COLOR_PRO = new SerializeColor( value.r, value.g, value.b, value.a );
			}
			
			else
			{	par.BG_LINE_HR_COLOR_PERSONAL = new SerializeColor( value.r, value.g, value.b, value.a );
			}
		}
	}
	
	
	
	private void DrawChess( Rect selectionRect, float content )
	{
	
		var rect = (selectionRect);
		rect.x = TOTAL_LEFT_PADDING;
		rect.width = content;
		rect.width += PREFAB_BUTTON_SIZE;
		
		
		if ( par.HIER_LINES_CHESSE == 1 )
		{	var cc = Mathf.RoundToInt(selectionRect.y / selectionRect.height) % 2;
		
			if ( cc == 0 && selectionRect.y > HierWinScrollPos.y + rect.height )
				//  GUI.DrawTexture(rect, sec);
			{	// var oc = GUI.color;
				// var c = GUI.color;
				//  GUI.color = CHESS_COLOR;
				//  GUI.DrawTexture
				Graphics.DrawTexture( rect, Texture2D.whiteTexture, rect, 0, 0, 0, 0, CHESS_COLOR );
				//GUI.DrawTexture(rect, Texture2D.whiteTexture);
				// GUI.color = c;
				// Graphics.DrawTexture( rect , sec );
				//  GUI.color = oc;
			}
		}
		
		if ( par.HIER_LINES_HR == 1 )
		{	rect.y -= 1;
			rect.height = 2;
			//rect.height = 1;
			
			var c = GUI.color;
			GUI.color = HR_COLOR;
			//  GUI.DrawTexture( rect , Texture2D.whiteTexture );
			GUI.DrawTexture( rect, Texture2D.whiteTexture );
			GUI.color = c;
		}
		
		
		
		/* if (ENABLED_OPTIMIZER) {
		     var o = GUI.color;
		     GUI.color = new Color32(200, 90, 50, 25);
		     GUI.DrawTexture(rect, Texture2D.whiteTexture);
		     GUI.color = o;
		 }*/
		
		/* rect.x = rect.width;
		 rect.width = defWidth - content;
		 rect.height = 1;
		 GUI.DrawTexture(rect, sec);*/
	}
	Color tempColor, activeColor;
	private void DrawLines( Adapter.HierarchyObject o, Rect selectionRect )
	{	var OFF = par.DEEP_WIDTH ?? 10;
	
		if ( IS_SEARCH_MOD_OPENED() ) return;
		
		selectionRect.width -= 40;
		r = (selectionRect);
		r.x -= 8;
		r.x += TOTAL_LEFT_PADDING;
		
		if ( !o.ParentIsNull() && (IS_HIERARCHY() || o.project.IsMainAsset) )
		{	if ( o.Active() )
			{	tempColor = GUI.color;
				activeColor = CHILDREN_LINE_COLOR;
				
				r2 = r;
				r2.x -= OFF;
				r2.y += r2.height / 2f;
				r2.height = 1;
				
				if ( IS_HIERARCHY() ) r2.width = OFF;
				else r2.width = OFF / 2;
				
				r2.width -= 3;
				// if ( !o.Active() ) activeColor.a /= 4;
				
				GUI.color = activeColor;
				
				GUI.DrawTexture( r2, Texture2D.whiteTexture );
				
				if ( o.ChildCount( this ) == 0 )
				{	r2.x += 5 + 7 + OFF - 14;
					r2.y -= 2;
					r2.width = r2.height = 4;
					GUI.DrawTexture( r2, Texture2D.whiteTexture );
				}
				
				GUI.color = tempColor;
			}
			
			
		}
		
		r3 = r;
		
		
		if ( pluginID == Initializator.HIERARCHY_ID )
		{	bool first = true;
			var _parent = o.go.transform;
			
			while ( _parent != null )
			{	if ( _parent.parent == null ) break;
			
				r3.width = 1;
				r3.x -= OFF;
				r3.y = r.y;
				r3.height = r.height;
				
				if ( _parent.parent != null && _parent.GetSiblingIndex() == _parent.parent.childCount - 1 )
				{	if ( !first )
					{	_parent = _parent.parent;
						continue;
					}
					
					r3.height /= 2;
				} /*else if (o.transform.childCount != 0) {
				
                }*/

				tempColor = GUI.color;
				activeColor = CHILDREN_LINE_COLOR;
				
				if ( _parent.parent && !_parent.parent.gameObject.activeInHierarchy ) activeColor.a /= 4;
				
				GUI.color = activeColor;
				
				GUI.DrawTexture( r3, Texture2D.whiteTexture );
				
				GUI.color = tempColor;
				
				
				first = false;
				_parent = _parent.parent;
				// parCount++;
			}
		}
		
		if ( pluginID == Initializator.PROJECT_ID )
		{	bool first = true;
			var _parent = o;
			
			while ( _parent != null )
			{	if ( _parent.parent( this ) == null ) break;
			
				r3.width = 1;
				r3.x -= OFF;
				r3.y = r.y;
				r3.height = r.height;
				
				if ( IS_PROJECT() && !_parent.project.IsMainAsset )
				{	_parent = _parent.parent( this );
					continue;
				}
				
				
				// if (_parent.parent( this ) != null && _parent.GetSiblingIndex( this ) == _parent.parent( this ).ChildCount( this ) - 1)
				if ( _parent.parent( this ) != null && _parent.IsLastSibling( this ) )
				{	if ( !first )
					{	_parent = _parent.parent( this );
						continue;
					}
					
					r3.height /= 2;
				} /*else if (o.transform.childCount != 0) {
				
                }*/


				tempColor = GUI.color;
				activeColor = CHILDREN_LINE_COLOR;
				
				if ( !_parent.Active() ) activeColor.a /= 4;
				
				GUI.color = activeColor;
				
				GUI.DrawTexture( r3, Texture2D.whiteTexture );
				
				GUI.color = tempColor;
				
				
				first = false;
				_parent = _parent.parent( this );
				// parCount++;
			}
		}
	}
	
	internal EventType? paddinguseEvent = null;
	float bakepad = 0;
	private void CurrentRectClear()
	{	___CurrentRect.Remove( window() );
	}
	private bool CurrentRectContainsKey( EditorWindow w, Module m )
	{	if ( !___CurrentRect.ContainsKey( w ) ) return false;
	
		if ( !___CurrentRect[w].ContainsKey( m ) ) return false;
		
		return true;
	}
	private void CurrentRectInit( EditorWindow w, Module m, Rect r )
	{	if ( !___CurrentRect.ContainsKey( w ) ) ___CurrentRect.Add( w, new Dictionary<Module, Rect>() );
	
		if ( !___CurrentRect[w].ContainsKey( m ) ) ___CurrentRect[w].Add( m, r );
		
		/*if ( hierarchy_windows.Count > 1 )*/ ___CurrentRect[w][m] = r;
	}
	private void CurrentRectSet( EditorWindow w, Module m, Rect r )       //MonoBehaviour.print(hierarchy_windows.Count);
	{	if ( !___CurrentRect.ContainsKey( w ) ) ___CurrentRect.Add( w, new Dictionary<Module, Rect>() );
	
		if ( !___CurrentRect[w].ContainsKey( m ) ) ___CurrentRect[w].Add( m, r );
		
		___CurrentRect[w][m] = r;
	}
	private Rect CurrentRect( EditorWindow w, Module m )      //if (!___CurrentRect.ContainsKey(w))___CurrentRect.Add(w, new Dictionary<Module, Rect>());
	{	return ___CurrentRect[w][m];
	}
	private Rect ClipMINSizeRect( Rect mod, float fullWidth )
	{	float leftPad = par.PADDING_LEFT;
	
		// if (window() != null && MIN > window().position.width - RIGHTADD_PadLeft) MIN = window().position.width - RIGHTADD_PadLeft;
		if ( /*window() != null &&*/ leftPad > fullWidth - RIGHTPAD ) leftPad = fullWidth - RIGHTPAD;
		
		if ( mod.x < leftPad )
		{	var difToRight = leftPad - mod.x;
			mod.width = Math.Max( 0, mod.width - difToRight );
			mod.x += difToRight;
		}
		
		return mod;
	}
	private float GET_PADING( float width )
	{	if ( paddinguseEvent == Event.current.type ) return bakepad;
	
		var w = window();
		
		foreach ( var drawModule in modules )
		{	if ( drawModule.sibbildPos == -1 || !drawModule.enable ) continue;
		
			if ( CurrentRectContainsKey( w, drawModule ) )
			{	if ( !CurrentRectContainsKey( w, drawModule ) ) continue;
			
				var target = CurrentRect(w, drawModule);
				
				if ( target.x < width ) width = target.x;
			}
		}
		
		paddinguseEvent = Event.current.type;
		bakepad = width;
		return width;
	}
	
	
	
	private void _DG( Rect selectionRect )
	{	try
		{	var headRect = selectionRect;
		
			if ( !par.HEADER_BIND_TO_SCENE_LINE && EditorSceneManager.sceneCount < 2 ) headRect.y = Mathf.RoundToInt( HierWinScrollPos.y );
			else headRect.y = 0;
			
			if ( ENABLE_RIGHTDOCK_PROPERTY ) DrawHeader( headRect );
		}
		
		catch ( Exception ex )
		{	logProxy.LogError( "DrawHeaderError " + ex.Message + " " + ex.StackTrace );
		}
	}
	
	
	
	
	private void ShowCategoryList( bool addMenu = false, Action<GenericMenu> topBuilder = null )
	{	var menu = new GenericMenu();
	
		//  if (addMenu)
		
		
		if ( topBuilder != null ) topBuilder( menu );
		
		GUIContent cont = null;
		bool haveDisable = false;
#pragma warning disable
		
		if ( Adapter.LITE ) haveDisable = true;
		
#pragma warning restore
		
		foreach ( var mod in modules )
		{	if ( mod.sibbildPos == -1 ) continue;
		
			var wasEnable = mod.enable;
			var wasMod = mod;
			// if (mod.HeaderTexture2D != null) cont = new GUIContent(GetIcon(mod.HeaderTexture2D));
			/*else*/
			
			if ( !mod.enableOverride() )
			{	if ( string.IsNullOrEmpty( mod.enableOverrideMessage() ) )
				{	continue;
					/*menu.AddDisabledItem(new GUIContent(mod.ContextHelper.ToString() + " (Pro Only)")); */
				}
				
				else
				{	if ( !haveDisable ) menu.AddDisabledItem( new GUIContent( mod.ContextHelper.ToString() + " " + mod.enableOverrideMessage() ) );
				
					haveDisable = true;
				}
			}
			
			else
			{	var userType = GetUserModuleType();
				var txt = userType.IsAssignableFrom(mod.GetType())/* is Adapter.M_UserModulesRoot*/ ? mod.ContextHelper : mod.HeaderText;
				
				if ( mod.enable || userType.IsAssignableFrom( mod.GetType() )/* is M_UserModulesRoot */) cont = new GUIContent( txt.ToString() );
				else cont = new GUIContent( "[ " + txt.ToString() + " ]" );
				
				menu.AddItem( cont, mod.enable, () =>
				{	wasMod.CreateUndo();
					wasMod.enable = !wasEnable;
					wasMod.SetDirty();
					RepaintWindow(true);
				} );
			}
		}
		
		menu.AddSeparator( "" );
		//  var mp = EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition);
		var pos = new MousePos( null, MousePos.Type.ModulesListWindow_380_700, false, this); //new Rect(Event.current.mousePosition.x - 190, Event.current.mousePosition.y, 0, 0)
		
		menu.AddItem( new GUIContent( "[ Open Modules Table ☷ ]" ), false, () => { _W__ModulesWindow.Init( pos, this ); } );
		
		//* Static Members **/
		MOI.CONTEXTMENU_STATICMODULES( menu );
		
		menu.AddSeparator( "" );
		
		ADD_LAYOUTS( ref menu );
		
		menu.AddSeparator( "" );
		
		{	menu.AddItem( new GUIContent( "Auto-Hide If Width < " + par.RIGHTDOCK_TEMPHIDEMINWIDTH ),
						  par.RIGHTDOCK_TEMPHIDE, () =>
			{	par.RIGHTDOCK_TEMPHIDE = !par.RIGHTDOCK_TEMPHIDE;
				SavePrefs();
			} );
			
			if ( IS_PROJECT() )
			{	menu.AddItem( new GUIContent( "'*.*' Extension Next To The Object Name" ), par.COMPONENTS_NEXT_TO_NAME, () =>
				{	par.COMPONENTS_NEXT_TO_NAME = !par.COMPONENTS_NEXT_TO_NAME;
					SavePrefs();
				} );
			}
			
			if ( EditorSceneManager.sceneCount < 2 )
				menu.AddItem( new GUIContent( "Bind Header To The Top" ), par.HEADER_BIND_TO_SCENE_LINE, () =>
			{	par.HEADER_BIND_TO_SCENE_LINE = !par.HEADER_BIND_TO_SCENE_LINE;
				SavePrefs();
			} );
			else
				menu.AddDisabledItem( new GUIContent( "Bind Header To The Top" ) );
				
				
			menu.AddSeparator( "" );
		}
		menu.AddItem( new GUIContent( "Open Settings" ), false, () =>
		{	/*EditorPrefs.SetInt( pluginname + "/" + " Plugin Menu Item", 3 );
		
			if
			( pluginID == Initializator.HIERARCHY_ID )
			{
			    #if HIERARCHY
			    _S___Hierarchy_GUIEditorWindow.showWindow();
			    #endif
			}
			else
			{
			    #if PROJECT
			    _S___Project_GUIEditorWindow.showWindow();
			    #endif
			}*/
			SHOW_HIER_SETTINGS_GENERICMENU();
		} );
		
		
		
		if ( Initializator.plugins.Length > 1 )
		{	menu.AddSeparator( "" );
		
			if ( Hierarchy_GUI.Instance( this ).SaveToScriptableObject == "FOLDER" )
			{	menu.AddItem( new GUIContent( "Revival Cache in Project" ), false, () =>
				{	var d = MOI.des(-1);
					Selection.objects = new[] { d as UnityEngine.Object };
				} );
				
			}
			
			else
			{	menu.AddDisabledItem( new GUIContent( "Revival Cache in Project" ) );
			}
		}
		
		
		
		/*menu.AddItem(new GUIContent("[ Use Horisontal Scroll ] (Experimental)"), par.USE_HORISONTAL_SCROLL, () =>
		{
		    par.USE_HORISONTAL_SCROLL = !par.USE_HORISONTAL_SCROLL;
		    SavePrefs();
		});*/
		
		if ( !addMenu )
		{	/*   menu.AddSeparator("");
			       // menu.AddDisabledItem(new GUIContent("Click on the item for more options"));
			       // menu.AddDisabledItem(new GUIContent("Click below to get more options.."));
			       menu.AddDisabledItem(new GUIContent("Click on the object's line to configure"));*/
		}
		else
		{	menu.AddSeparator( "" );
			menu.AddDisabledItem( new GUIContent( "Drag this icon to overall width" ) );
		}
		
		
		menu.ShowAsContext();
		// EventUse();
	}
	
	
	
	
	string[] TakeModulesSnapShot()
	{	var modules =            Hierarchy_GUI.Instance( this ).modulesParams;
	
		if ( modules.Count == 0 ) return null;
		
		var final = "";
		
		for ( int i = 0 ; i < modules.Count ; i++ )
		{	var result = "";
			result += modules[i].Key + "\0";
			result += modules[i].Value.enable + "\0";
			result += modules[i].Value.sib + "\0";
			result += modules[i].Value.width + "\n";
			final += result;
		}
		
		final = final.Trim( '\n' );
		return new[] { "", final };
	}
	
	void SetDirtyModulesSnapShots( string name, string snapShot )
	{	var modules =  Hierarchy_GUI.Instance( this ).modulesParams;
	
		if ( modules.Count == 0 ) return;
		
		Hierarchy_GUI.Undo( this, "Apply " + name );
		
		foreach ( var item in snapShot.Split( '\n' ) )
		{	try
			{	var data =  item.Split('\0');
				var type = data[0];
				
				if ( !modules.ContainsKey( type ) ) continue;
				
				modules[type].enable = bool.Parse( data[1] );
				modules[type].sib = int.Parse( data[2] );
				modules[type].width = int.Parse( data[3] );
			}
			
			catch { }
			
		}
		
		Hierarchy_GUI.SetDirtyObject( this );
		__modulesOrdered = null;
		wasModulesInitialize = false;
		MOI.InitModules();
		RepaintAllViews();
	}
	
	
	/* int GetCountSnapShots()
	 {
	     return EditorPrefs.GetInt( pluginname + "/Layouts/SnapsCount", 0 );
	 }*/
	
	string[][] snapsArray
	{	get
		{	var snaps = EditorPrefs.GetString( pluginname + "/Layouts/SnapsSer", null );
		
			if ( snaps != null )
			{	try { return DESERIALIZE_SINGLE<string[][]>( snaps ); }
			
				catch { }
			}
			
			return new string[0][];
		}
		
		set
		{	var ser = SERIALIZE_SINGLE( value );
			EditorPrefs.SetString( pluginname + "/Layouts/SnapsSer", ser );
		}
	}
	
	//      s.Split( new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries );
	//
	
	void ADD_LAYOUTS( ref GenericMenu menu )
	{	var currentSnap = TakeModulesSnapShot();
		var snaps = snapsArray;
		var findIndex = snaps.ToList().FindIndex(s => s[1] == currentSnap[1]);
		
		const string CATEGORY = "- LAYOUTS/";
		
		var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_128_68, false, this);
		
		//  var pos = InputData.WidnwoRect(FocusRoot.WidnwoRectType.Clamp, Event.current.mousePosition, 128, 68, this);
		if ( findIndex != -1 ) menu.AddDisabledItem( new GUIContent( CATEGORY + "Add" ) );
		else menu.AddItem( new GUIContent( CATEGORY + "Add" ), false, () =>
		{	_W__InputWindow.Init( pos, "New Item", this, ( str ) =>
			{	if ( string.IsNullOrEmpty( str ) ) return;
			
				str = str.Trim();
				var r = snaps.ToList();
				currentSnap[0] = str;
				r.Add( currentSnap );
				snapsArray = r.ToArray();
			}, textInputSet: "MyWorkflow" );
		} );
		
		if ( findIndex == -1 ) menu.AddDisabledItem( new GUIContent( CATEGORY + "Remove" ) );
		else menu.AddItem( new GUIContent( CATEGORY + "Remove" ), false, () =>
		{	var r = snaps.ToList();
			r.RemoveAt( findIndex );
			snapsArray = r.ToArray();
		} );
		
		if ( snaps.Length != 0 ) menu.AddSeparator( CATEGORY );
		
		for ( int i = 0 ; i < snaps.Length ; i++ )
		{	var name = snaps[i][0];
			var captureName = snaps[i][0];
			var captureSnap = snaps[i][1];
			menu.AddItem( new GUIContent( CATEGORY + name ), findIndex == i, () =>
			{	SetDirtyModulesSnapShots( captureName, captureSnap );
			} );
		}
		
		
		menu.AddSeparator( CATEGORY );
		
		menu.AddItem( new GUIContent( CATEGORY + "- Default" ),
					  modules.All(
						  m =>
						  m.sibbildPos == -1 || DefaulTypes.Contains( m.GetType().FullName ) && (m.enable || !m.enableOverride()) ||
						  !DefaulTypes.Contains( m.GetType().FullName ) && !m.enable ), () =>
		{	SET_TO_DEFAULT();
			/*   modules.First( m => m is M_CustomIcons )
			       .enable = true;
			   modules.First( m => m is M_Audio ).enable =
			       true;
			   modules.First( m => m is M_Descript ).enable
			       = true;
			   modules.First( m => m is IModuleOnnector_M_Vertices ).enable
			       = true;*/
			SavePrefs();
			RepaintWindow(true);
		} );
		
		menu.AddItem( new GUIContent( CATEGORY + "- Show All" ), modules.All( m => m.sibbildPos == -1 || m.enable ), () =>
		{	foreach (
				var
				module
				in
				modules
			)
			{	if (
					module
					.sibbildPos ==
					-1 )
					continue;
					
				module
				.enable
					=
						true;
			}
			
			SavePrefs();
			RepaintWindow
			(true);
		} );
		menu.AddItem( new GUIContent( CATEGORY + "- Hide All" ), modules.All( m => m.sibbildPos == -1 || !m.enable ), () =>
		{	foreach (
				var
				module
				in
				modules
			)
			{	if (
					module
					.sibbildPos ==
					-1 )
					continue;
					
				module
				.enable
					=
						false;
			}
			
			modules.First( m => m is IModuleOnnector_M_CustomIcons ).enable = true;
			SavePrefs();
			RepaintWindow
			(true);
		} );
		
	}
	
	internal void SET_TO_DEFAULT()
	{	foreach ( var module in modules )
		{	if ( module.sibbildPos == -1 ) continue;
		
			// Debug.Log( DefaulTypes[0] + " " + module.GetType().FullName );
			if ( DefaulTypes.Any( d => d == (module.GetType().FullName) ) ) module.enable = true;
			else module.enable = false;
		}
	}
	
}
}
