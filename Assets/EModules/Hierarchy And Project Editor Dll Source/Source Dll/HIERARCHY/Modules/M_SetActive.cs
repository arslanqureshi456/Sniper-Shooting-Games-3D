using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;

//namespace EModules

namespace EModules.EModulesInternal {
internal partial class Hierarchy {
	internal class M_SetActive : Adapter.Module {
		internal override int STATIC_WIDTH()
		{	return adapter.M_SetActive_WIDTH;
		}
		internal override bool SKIP()
		{	return false;
		}
		static void scenegui( SceneView sceneView )
		{	if ( stateForDrag_B1 != null )
			{
			
				if ( Event.current.type == EventType.Repaint ) Handles.PositionHandle( stateForDrag_B1.transform.position,
					        Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : stateForDrag_B1.transform.rotation );
			}
		}
		
		public M_SetActive( int restWidth, int sib, bool enable, Adapter adapter ) : base( restWidth, sib, enable, adapter )
		{
		
		
			if ( Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_VERSION )
			{	var duringSceneGui = typeof(SceneView).GetField("duringSceneGui", (System.Reflection.BindingFlags)(-1));
				var eventInfo = (Action<SceneView>)duringSceneGui.GetValue(null);
				eventInfo -= scenegui;
				eventInfo += scenegui;
				duringSceneGui.SetValue( eventInfo, eventInfo );
			}
			
			else
			{
#pragma warning disable
				SceneView.onSceneGUIDelegate -= scenegui;
				SceneView.onSceneGUIDelegate += scenegui;
#pragma warning restore
			}
			
			
			
		}
		
		Color   t1A =  new Color( .1f, .1f, .1f, .3f ),
		t1B =  new Color( .3f, .35f, .4f, .36f ),
		t2A /*=  B_ACTIVE*/,
		t2B =  new Color( 200, 200, 200, 255 ),
		t3A/* =  B_PASSIVE*/,
		t3B =  new Color32( 55, 55, 55, 35 ) ;
		/*  t1A = adapter.GET_TEXTURE( 0x007, new Color( .1f, .1f, .1f, .3f ) );
		    t1B = adapter.GET_TEXTURE( 0x008, new Color( .3f, .35f, .4f, .45f ) );
		    t2A = adapter.GET_TEXTURE( 0x009, B_ACTIVE );
		    t2B = adapter.GET_TEXTURE( 0x00A, new Color( 200, 200, 200, 255 ) );
		    t3A = adapter.GET_TEXTURE( 0x00B, B_PASSIVE );
		    t3B = adapter.GET_TEXTURE( 0x00C, new Color( 55, 55, 55, 35 ) );*/
		internal override void InitializeModule()
		{	t2A = B_ACTIVE;
			t3A = B_PASSIVE;
			/*   t1A = new  Texture2D(1, 1, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.DontSave };
			
			   t1A.SetPixel(0, 0, new Color(.1f, .1f, .1f, .3f));
			   t1A.filterMode = FilterMode.Point;
			   t1A.Apply();
			
			   t1B = new  Texture2D(1, 1, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.DontSave };
			   t1B.SetPixel(0, 0, new Color(.3f, .35f, .4f, .45f));
			   t1B.filterMode = FilterMode.Point;
			   t1B.Apply();
			
			   t2A = new  Texture2D(1, 1, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.DontSave };
			   t2A.SetPixel(0, 0, B_ACTIVE);
			   t2A.filterMode = FilterMode.Point;
			   t2A.Apply();
			
			   t2B = new  Texture2D(1, 1, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.DontSave };
			   t2B.SetPixel(0, 0, new Color32(200, 200, 200, 255));
			   t2B.filterMode = FilterMode.Point;
			   t2B.Apply();
			
			   t3A = new  Texture2D(1, 1, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.DontSave };
			   t3A.SetPixel(0, 0, B_PASSIVE);
			   t3A.filterMode = FilterMode.Point;
			   t3A.Apply();
			
			   t3B = new  Texture2D(1, 1, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.DontSave };
			   t3B.SetPixel(0, 0, new Color32(55, 55, 55, 35));
			   t3B.filterMode = FilterMode.Point;
			   t3B.Apply();*/
			
			base.InitializeModule();
			
			// EditorGUIUtility.isProSkin
		}
		//static internal int IW = 22;
		internal const int  ONE_POS_IW = 10;
		internal static int  ONE_POS_IW_FORCOLOR =  (ONE_POS_IW- 4) *2;
		int rawIW
		{	get
			{	return adapter.SETACTIVE_POSITION > 0 ? ONE_POS_IW : 22;
			}
		}
		void FRAME( GameObject o )
		{	if ( SceneView.lastActiveSceneView == null ) return;
		
			var combinedBounds = new Bounds() { center = o.transform.position };
			var rr = o.GetComponent<Renderer>();
			
			if ( rr != null ) combinedBounds.Encapsulate( rr.bounds );
			
			/*  foreach (var render in o.GetComponentsInChildren<Renderer>())
			      if (render != rr) combinedBounds.Encapsulate(render.bounds);*/
			
			var pars = adapter.frame_method.GetParameters().Select( p =>
			{	return p.DefaultValue;
				/*  if (p.ParameterType.IsValueType)
				  {
				      return Activator.CreateInstance(p.ParameterType);
				  }
				  return null;*/
			}
			                                                      ).ToArray();
			pars[0] = combinedBounds;
			
			if ( pars.Length > 1 ) pars[1] = !adapter.par.SMOOTH_FRAME;
			
			adapter.frame_method.Invoke( SceneView.lastActiveSceneView, pars );
			//SceneView.lastActiveSceneView.Frame(combinedBounds, false);
		}
		
		
		
		
		
		
		
		
		
		// GUIContent content = new GUIContent();
		internal override float Draw( Rect drawRect, Adapter.HierarchyObject _o )
		{	if ( !START_DRAW( drawRect, _o ) ) return 0;
		
			var o = _o.go;
			
			var ww = drawRect.width + drawRect.x;
			
			if ( Hierarchy.HierarchyAdapterInstance.par.PADDING_RIGHT_MoveSetActiveToo ) drawRect.width -= Hierarchy.HierarchyAdapterInstance.par.PADDING_RIGHT;
			
			
			var IW = (float) rawIW;
			
			
			if ( adapter.SETACTIVE_POSITION == 1 )
				drawRect.x = adapter.raw_old_leftpadding;
			else
				drawRect.x = drawRect.x + drawRect.width - IW + 4;
				
			if ( adapter.SETACTIVE_POSITION == 2 ) drawRect.x -= 3;
			
			
			//if ( adapter.SETACTIVE_POSITION == 2 && Adapter.UNITY_CURRENT_VERSION < Adapter.UNITY_2019_1_1_VERSION ) drawRect.x -= 4;
			/* if (drawRect.y == 0)
			 {
			     drawRect.y += 1;
			     drawRect.height -= 1;
			 }*/
			
			if ( adapter.SETACTIVE_POSITION != 2)
				drawRect.width = IW - 3;
			else
				drawRect.width = IW ;
				
			var  tR = drawRect;
			
			//MonoBehaviour.print(drawRect);
			// drawRect.y += 1;
			
			//if ( adapter.SETACTIVE_POSITION  !=  1 )
			{	var bgrect = drawRect;
			
			
				if ( adapter.SETACTIVE_POSITION != 1 ) bgrect.width = (ww ) - bgrect.x + 40;
				
				if (adapter.SETACTIVE_STYLE == 0 )
				{	if ( EditorGUIUtility.isProSkin )
					{	if ( adapter.SETACTIVE_POSITION == 0 || !o.activeInHierarchy )
						{	var c = t1A;
							c.a *= adapter.SETACTIVE_POSITION == 2 ? 0.8f : 1;
							Draw_AdapterTexture( bgrect, c  );
						}
					}
					
					else
					{	if ( o.activeInHierarchy )
						{	var c = Color.white;
						
							if ( Adapter.UNITY_CURRENT_VERSION >= Adapter.UNITY_2019_3_0_VERSION)  c.a *= 0.75f;
							
							if ( adapter.SETACTIVE_POSITION == 2) c.a *=  0.5f;
							
							Draw_AdapterTexture( bgrect,  t1B, c);
							// GUI.color = c;
						}
					}
				}
				
				else
				{	Draw_AdapterTexture( bgrect, new Color32( 255, 255, 255, 15 ) );
				
				}
			}
			
			/*    tR.width += 40;
			    tR.x = tR.x + tR.width - 18;*/
			
			
			//  tR.y += 1;
			
			// var oldW = drawRect.width;
			var oldH = drawRect.height;
			
			
			if ( adapter.SETACTIVE_POSITION  == 0 )
			{	drawRect.width = 12;
				drawRect.height = 10;
				drawRect.x += 3;
			}
			
			else
			{	drawRect.height = 10;
			
				if ( adapter.SETACTIVE_POSITION != 2 )
					drawRect.width = IW - 4;
				else
				{
				
					drawRect.width = IW - 1;
					
					/*   if ( adapter.hashoveredItem && adapter.hoverID == _o.id)
					   drawRect.width = IW ;
					else
					   drawRect.width = IW - 6;*/
				}
			}
			
			/**/
			//////////////////
			
			
			// drawRect.x += (oldW - drawRect.width) / 2;
			drawRect.y += (oldH - drawRect.height) / 2;
			
			// float t=  0;
			
			// var oldBoxT = Adapter.GET_SKIN().box.normal.background;
			bool skip = false;
			
			if ( adapter.SETACTIVE_POSITION == 0 )
			{	if ( !o.activeSelf ) drawRect.x += drawRect.width / 2;
				else drawRect.width = EditorGUIUtility.singleLineHeight - (Adapter.USE2018_3 ? 3 : 0);
			}
			
			else
				if ( adapter.SETACTIVE_POSITION == 2 )
				{
				
					if ( adapter.SETACTIVE_STYLE == 1 )
					{	// if ( !o.activeSelf ) drawRect.width /= 1.5f;
						if ( !o.activeSelf && (!o.transform.parent || o.transform.parent.gameObject.activeInHierarchy) ) drawRect.width /= 1.5f;
						else
							if ( !o.activeSelf ) skip = true;
							else
								if ( o.activeSelf && !o.activeInHierarchy )
								{	drawRect.width /= 1.5f;
									//drawRect.height /= 3f;
								}
								
								else
									if ( !o.activeInHierarchy ) skip = true;
					}
					
					else
					{	//if ( !(o.activeSelf && o.activeInHierarchy || !o.activeSelf && !o.activeInHierarchy) )
						//  drawRect.width /= 1.5f;
						if (  !o.activeSelf && (!o.transform.parent || o.transform.parent.gameObject.activeInHierarchy)) drawRect.width /= 1.5f;
						else
							if ( o.activeSelf && !o.activeInHierarchy )
							{	drawRect.width /= 1.5f;
								//drawRect.height /= 3f;
							}
							
							else
								if ( !o.activeInHierarchy ) skip = true;
					}
					
					//  {   if ( !o.activeSelf ) drawRect.width += drawRect.width / 2;
				}
				
			var rr = drawRect;
			
			if ( adapter.SETACTIVE_POSITION == 2 )
			{	rr.x += 2;
				rr.width -= 4;
			}
			
			if ( adapter.SETACTIVE_POSITION == 0 )
				if ( adapter.SETACTIVE_STYLE == 1 ) rr.x += 2;
				
			if ( adapter.SETACTIVE_POSITION == 1 )
				if ( adapter.SETACTIVE_STYLE == 0 ) rr.width -= 2;
				
			if ( !skip )
			{	if ( o.activeSelf )
				{
				
				
					//////////////////
					/**/
					//////////////////
					
					if ( adapter.SETACTIVE_STYLE == 0 )
					{
					
					
						if ( EditorGUIUtility.isProSkin ) Draw_AdapterTexture( rr, o.activeInHierarchy ? t2A : t3A );
						else Draw_AdapterTexture( rr, o.activeInHierarchy ? t2B : t3B );
					}
					
					else
					{
					
						Draw_AdapterTexture( rr, new Color( 0, 0, 0, o.activeInHierarchy ? 0.3f : 0.0f ) );
					}
					
					
					
					
				}
				
				else
				{	var c =  Color.white;
				
					if ( adapter.SETACTIVE_POSITION != 2 && adapter.SETACTIVE_STYLE != 0 )
					{	rr.width /= 1.5f;
					}
					
					if ( adapter.SETACTIVE_POSITION != 2 || adapter.SETACTIVE_STYLE != 0 )
					{	c *= new Color( 1, 1, 1, 0.4f );
					}
					
					if ( adapter.SETACTIVE_STYLE == 0 )
					{	if ( adapter.SETACTIVE_STYLE == 0 )
						{	if ( EditorGUIUtility.isProSkin ) Draw_AdapterTexture( rr, o.activeInHierarchy ? t2A : t3A, c);
							else Draw_AdapterTexture( rr, o.activeInHierarchy ? t2B : t3B, c);
						}
					}
					
					else
					{	Draw_AdapterTexture( rr, new Color( 0, 0, 0, o.activeInHierarchy ? 0.3f : 0.0f ), c );
					}
					
					//  GUI. color = c;
				}
			}
			
			
			if ( adapter.SETACTIVE_STYLE == 1 )
			{
			
				if ( adapter.SETACTIVE_POSITION == 2 )
				{	// if ( (o.activeSelf && o.activeInHierarchy || !o.activeSelf && !o.activeInHierarchy) )
					if (!skip)
					{	if ( o.activeInHierarchy )
							Draw_GUITexture( drawRect, adapter.GetIcon( "SETUP_BUTTON_HOVER" ) );
						else
						{	/*var c = GUI .color;
							GUI .color *= new Color( 1, 1, 1, 0.3f );*/
							Draw_GUITexture( drawRect, adapter.GetIcon( "SETUP_BUTTON_HOVER" ), new Color( 1, 1, 1, 0.3f ) );
							// GUI .color = c;
						}
					}
					
					
				}
				
				else
				{	if ( o.activeInHierarchy )
						Draw_AdapterTexture( drawRect, adapter.GetIcon( "SETUP_BUTTON_HOVER" ) );
					else
					{
					
						if ( adapter.SETACTIVE_POSITION == 0 && adapter.SETACTIVE_STYLE == 1 && !o.activeSelf )
						{	/*var c = GUI .color;
							GUI .color *= new Color( 1, 1, 1, 0.5f );*/
							Draw_AdapterTexture( drawRect, adapter.GetIcon( "SETUP_BUTTON_HOVER" ), new Color( 1, 1, 1, 0.5f ) );
							// GUI .color = c;
							
						}
						
						else
						{	/* var c = GUI .color;
							 GUI .color *= new Color( 1, 1, 1, 0.25f );*/
							Draw_AdapterTexture( drawRect, adapter.GetIcon( "SETUP_BUTTON_HOVER" ), new Color( 1, 1, 1, 0.25f ) );
							//GUI .color = c;
						}
						
						
					}
				}
				
				
				
			}
			
			tR.height = adapter.parLINE_HEIGHT;
			
			if ( adapter.SETACTIVE_POSITION != 2 )
				tR.width = IW - 4;
			else
				tR.width = IW;
				
			if ( adapter.PREFAB_BUTTON_SIZE != 0 && adapter.SETACTIVE_POSITION != 1 && adapter.FindPrefabRoot( _o.go ) != _o.go ) tR.width += adapter.PREFAB_BUTTON_SIZE;
			
			
			
			
			/* content.text = adapter.SETACTIVE_STYLE == 1 || adapter.SETACTIVE_POSITION != 0 ? "" : "O";
			 //  tR.height -= 2;( Ctrl+Left CLICK - All selected objects )\n
			 if ( tR.Contains( Event.current.mousePosition ) )
			     content.tooltip = objectIsHiddenAndLock ? "Object hided" : "Left CLICK/Left DRAG Show/Hide GameObject \n( Right CLICK/Right DRAG - Focus on the object in the SceneView )";*/
			//  if (Event.current.rawType != EventType.repaint && Event.current.rawType != EventType.layout) MonoBehaviour.print(Event.current.rawType);
			/*if ( Event.current.rawType == EventType.MouseUp )     // if (stateForDrag_B0 != null) Hierarchy.RepaintWindow();
			{   if ( stateForDrag_B1 != null )     // Hierarchy.RepaintWindow();
			    {   // SceneView.RepaintAll();
			    }
			    stateForDrag_B0 = null;
			    stateForDrag_B1 = null;
			
			}*/
			if (adapter.SETACTIVE_POSITION == 1/* && adapter._S_bgButtonForIconsPlace != 0*/  )
			{	tR.width *= 2;
			
				if (!(_o.parent(adapter) != null || _o.ChildCount(adapter)== 0)) tR.width -=4;
			}
			
			Draw_Action( tR, SET_ACTIVE_ACTION_HASH, null);
			
			
			
			
			
			
			//                 var oldF = Adapter.GET_SKIN().button.fontSize;
			//                 var al = Adapter.GET_SKIN().button.padding.bottom;
			//                 var al2 = Adapter.GET_SKIN().button.padding.top;
			//                 var al3 = Adapter.GET_SKIN().button.alignment;
			
			/*    if (Event.current.type == EventType.Repaint )
			    {   style.Draw( tR, content, false, false, false, false );
			    }*/
			//  DrawStyle( tR, style, content );
			
			/* if ( GUI.Button( tR, content, style ) ) {
			
			     }*/
			//                 Adapter.GET_SKIN().button.padding.bottom = al;
			//                 Adapter.GET_SKIN().button.padding.top = al2;
			//                 Adapter.GET_SKIN().button.alignment = al3;
			//                 Adapter.GET_SKIN().button.fontSize = oldF;
			
			/*  if (adapter.ModuleButton(tR, content))
			  {
			      Undo.RecordObject(o, "GameObject SetActive");
			      //EditorUtility.SetObjectEnabled( markedObjects[ instanceID ], !markedObjects[ instanceID ].activeInHierarchy );
			      if (objectIsHiddenAndLock)
			      {
			               //#tag TODO if you need the ability to turn off objects inside disabled you need to uncomment
			          // o.SetActive(!o.activeSelf);
			      }
			      else
			          o.SetActive(!o.activeInHierarchy);
			      Hierarchy.SetDirty(o);
			      //ScriptExtensions.SetDirty(o);
			  }
			*/
			
			
			
			END_DRAW(_o);
			return width;
		}
		GUIStyle style;
		static bool? stateForDrag_B0 = null;
		static GameObject stateForDrag_B1 = null;
		
		GUIContent _CONTENT_STYLE_1, _CONTENT_STYLE_OTHER;
		GUIContent CONTENT_STYLE_1
		{	get
			{	if ( _CONTENT_STYLE_1 == null )
				{	_CONTENT_STYLE_1 = new GUIContent();
					_CONTENT_STYLE_1.text = "";
					_CONTENT_STYLE_1.tooltip =  "Left CLICK/Left DRAG Show/Hide GameObject \n( Right CLICK/Right DRAG - Focus on the object in the SceneView )";
				}
				
				return _CONTENT_STYLE_1;
			}
		}
		GUIContent CONTENT_STYLE_OTHER
		{	get
			{	if ( _CONTENT_STYLE_OTHER == null )
				{	_CONTENT_STYLE_OTHER = new GUIContent();
					_CONTENT_STYLE_OTHER.text =  "O";
					_CONTENT_STYLE_OTHER.tooltip =   "Left CLICK/Left DRAG Show/Hide GameObject \n( Right CLICK/Right DRAG - Focus on the object in the SceneView )";
				}
				
				return _CONTENT_STYLE_OTHER;
			}
		}
		
		GUIContent _CONTENT_STYLE_1_disabled, _CONTENT_STYLE_OTHER_disabled;
		GUIContent CONTENT_STYLE_1_disabled
		{	get
			{	if ( _CONTENT_STYLE_1_disabled == null )
				{	_CONTENT_STYLE_1_disabled = new GUIContent();
					_CONTENT_STYLE_1_disabled.text = "";
					_CONTENT_STYLE_1_disabled.tooltip =  "Object hided" ;
				}
				
				return _CONTENT_STYLE_1_disabled;
			}
		}
		GUIContent CONTENT_STYLE_OTHER_disabled
		{	get
			{	if ( _CONTENT_STYLE_OTHER_disabled == null )
				{	_CONTENT_STYLE_OTHER_disabled = new GUIContent();
					_CONTENT_STYLE_OTHER_disabled.text = "O";
					_CONTENT_STYLE_OTHER_disabled.tooltip =  "Object hided" ;
				}
				
				return _CONTENT_STYLE_OTHER_disabled;
			}
		}
		
		
		
		Adapter. DrawStackMethodsWrapper __SET_ACTIVE_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper SET_ACTIVE_ACTION_HASH
		{	get
			{	if (__SET_ACTIVE_ACTION_HASH == null )
				{	__SET_ACTIVE_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( SET_ACTIVE_ACTION );
				}
				
				return __SET_ACTIVE_ACTION_HASH;
			}
		}
		// int SET_ACTIVE_ACTION_HASH = "SET_ACTIVE_ACTION".GetHashCode();
		void SET_ACTIVE_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{
		
		
			var o = _o.go;
			
			
			var objectIsHiddenAndLock = !o.activeInHierarchy && o.transform.parent != null && !o.transform.parent.gameObject.activeInHierarchy;
			var content = objectIsHiddenAndLock
			              ? ( adapter.SETACTIVE_STYLE == 1 || adapter.SETACTIVE_POSITION  != 0 ? CONTENT_STYLE_1_disabled : CONTENT_STYLE_OTHER_disabled)
			              :  ( adapter.SETACTIVE_STYLE == 1 || adapter.SETACTIVE_POSITION  != 0 ? CONTENT_STYLE_1 : CONTENT_STYLE_OTHER);
			              
			              
			              
			              
			var contains =  (inputRect.Contains( Event.current.mousePosition ) || (adapter.hashoveredItem && adapter.hoverID == _o.id));
			
			if ( stateForDrag_B0.HasValue && contains && !objectIsHiddenAndLock )
			{	if ( EditorGUIUtility.isProSkin ) Adapter.DrawTexture_StretchToFill( inputRect, adapter.button.active.background );
				else Adapter.DrawTexture_StretchToFill( inputRect, Texture2D.whiteTexture );
				
				if ( o.activeInHierarchy != stateForDrag_B0.Value )
				{	Undo.RecordObject( o, "GameObject SetActive" );
					stateForDrag_B0 = !o.activeInHierarchy;
					o.SetActive( !o.activeInHierarchy );
					/* Hierarchy.SetDirty(o);
					 Hierarchy.MarkSceneDirty(o.gameObject.scene);*/
				}
				
				if ( Event.current.isMouse ) Adapter.EventUse();
			}
			
			if ( stateForDrag_B1 && contains && !objectIsHiddenAndLock )
			{	if ( EditorGUIUtility.isProSkin ) Adapter.DrawTexture_StretchToFill( inputRect, adapter.button.active.background );
				else Adapter.DrawTexture_StretchToFill( inputRect, Texture2D.whiteTexture );
				
				FRAME( o );
				
				if ( stateForDrag_B1 != o )     //SceneView.RepaintAll();
				{
				}
				
				stateForDrag_B1 = o;
				
				
				if ( Event.current.isMouse ) Adapter.EventUse();
			}
			
			if ( inputRect.Contains( Event.current.mousePosition ) )
			{
			
				if ( inputRect.Contains( Event.current.mousePosition )
				        && Event.current.type == EventType.MouseDown )     //EditorUtility.SetObjectEnabled( markedObjects[ instanceID ], !markedObjects[ instanceID ].activeInHierarchy );
				{	/*   if (objectIsHiddenAndLock)
					{
					   //#tag TODO if you need the ability to turn off objects inside disabled you need to uncomment
					   // o.SetActive(!o.activeSelf);
					}
					else*/
					
					if ( Event.current.button == 0 )
					{	var targetO = new[] { o };
						var sel = adapter.SELECTED_GAMEOBJECTS();
						
						if ( sel.Any( c => c.id ==
						              _o.id ) /*&& Event.current.control*/)     // targetO = sel.Where(g => g.GetComponentsInParent<Transform>(true).Count(p => sel.Contains(p.gameObject)) == 1).Select(g => g.gameObject).ToArray();
						{	targetO = Utilities.GetOnlyTopObjects( sel, adapter ).Select( g => g.go ).ToArray();
							/*  for (int i = 0; i < targetArray.Count; i++)
							  {
							
							  } */
						}
						
						if ( !objectIsHiddenAndLock )
						{
						
							if ( stateForDrag_B0 == null ) adapter.PUSH_ONMOUSEUP( OnMouseUp );
							
							stateForDrag_B0 = !o.activeInHierarchy;
							
							foreach ( var gameObject in targetO )
							{	Undo.RecordObject( gameObject, "GameObject SetActive" );
								gameObject.SetActive( stateForDrag_B0.Value );
								/*  Hierarchy.SetDirty(gameObject);
								  Hierarchy.MarkSceneDirty(gameObject.scene);*/
							}
						}
						
						else
						{	var p = o.GetComponentsInParent<Transform>( true ).FirstOrDefault( a =>
							        !a.gameObject.activeInHierarchy && (a.transform.parent != null && a.transform.parent.gameObject.activeInHierarchy || a.transform.parent == null)
							                                                                 );
							                                                                 
							if ( p != null )
							{	Adapter.EventUse();
								adapter.TRY_PING_OBJECT( p.gameObject );
							}
							
							// MonoBehaviour.print(p);
						}
					}
					
					if ( Event.current.button == 1 )
					{
					
						if ( !objectIsHiddenAndLock )
						{	if ( SceneView.lastActiveSceneView == null )     //var pos = InputData.WidnwoRect(!callFromExternal(), Event.current.mousePosition, 128, 68, adapter );
							{	var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_128_68, !callFromExternal(), adapter);
								_W__InputWindow.Init( pos, "", adapter, null, null, "Please open 'SceneView' window" );
							}
							
							else
							{
							
								if ( stateForDrag_B1 == null ) adapter.PUSH_ONMOUSEUP( OnMouseUp );
								
								stateForDrag_B1 = o;
								//   SceneView.RepaintAll();
								FRAME( o );
							}
							
						}
						
					}
				}
				
				if ( Event.current.isMouse ) Adapter.EventUse();
			}
			
			
			
			
			
			if ( style == null )
			{	style = new GUIStyle( adapter.button );
				style.fontSize = 7;
				style.padding.bottom = 0;
				style.padding.top = 0;
				style.alignment = TextAnchor.MiddleLeft;
			}
			
			style.fontSize = adapter.parLINE_HEIGHT > 18 ? 8 : 7;
			
			if ( Event.current.type == EventType.Repaint )
			{	style.Draw( inputRect, content, false, false, false, false );
			}
		}
		
		
		
		internal override _W__SearchWindow.FillterData_Inputs CallHeader()
		{	throw new Exception( "Error CallHeader in SetActive" );
		}
		
		void OnMouseUp()
		{	if ( stateForDrag_B1 != null )     // Hierarchy.RepaintWindow();
			{	// SceneView.RepaintAll();
			}
			
			stateForDrag_B0 = null;
			stateForDrag_B1 = null;
		}
	}
}
}

