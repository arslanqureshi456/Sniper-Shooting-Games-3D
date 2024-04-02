#define DISABLE_PING

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using UnityEngineInternal;
using Object = UnityEngine.Object;

//namespace EModues

namespace EModules.EModulesInternal {
internal partial class Hierarchy {


	internal partial class M_CustomIcons : Adapter.Module, IModuleOnnector_M_CustomIcons {
	
#pragma warning disable
		internal class IconPosClass { internal float rightPos; }
		internal Dictionary<int, IconPosClass> iconRightPosition = new Dictionary<int, IconPosClass>();
		void PutIconPos(int id, float rightPos )
		{	return;
		
			if ( !iconRightPosition.ContainsKey( id ) ) iconRightPosition.Add( id, new IconPosClass() { rightPos = rightPos } );
			else
			{	if ( iconRightPosition[id].rightPos < rightPos ) iconRightPosition[id].rightPos = rightPos;
			}
		}
		void ClearIconPos( int id )
		{	/* foreach ( var item in iconRightPosition )
			 {   item.Value.rightPos = 0;
			 }*/
			return;
			
			if ( !iconRightPosition.ContainsKey( id ) ) iconRightPosition.Add( id, new IconPosClass() { rightPos = -1 } );
			else
			{	iconRightPosition[id].rightPos = -1;
			}
		}
		internal float GetIconPos(int id )
		{	return -1;
		
			if ( !iconRightPosition.ContainsKey( id ) ) return -1;
			
			return iconRightPosition[id].rightPos;
		}
#pragma warning restore
		
		void DrawIcon(Component[] drawComps, Adapter.HierarchyObject __o, Rect drawRect, Texture2D overImage, Type callbackType, bool allowHide, ref string MenuText )
		{
		
			// var drawComps = get_drawComps( __o.id);
			
			//  PutIconPos( __o.id, drawRect.x + drawRect.width );
			
			if (/*GUID == null && */drawComps.Length == 0 ) return;
			
			drawIcon = true;
			var r = drawRect;
			r.width += 3;
			//  r.width += 2;
			//  r.height += 1;
			r.x -= 1;
			
			
			/*
			
			r.y = 0;
			r.height = height;
			GUI.DrawTexture(r, Hierarchy.colorText11half);*/
			
			//  if (context.image == null) return width;
			//   var clip = r;
			//  if (clip.x < 0) clip.x = 0;
			//EditorGUI.DrawTextureTransparent(
			//   if (clip.y < 25) clip.y = (25 -clip.y)/clip.height;
			/* if (Event.current.type.Equals(EventType.Repaint))
			 {
			     /*          GUIClip.Push(this.clientRect);
			      DrawInnerControls();
			      GUIClip.Pop();#1#
			
			     var wp = GUIToScreenPoint(r.x, r.y);
			     // if (wp.x < currentClipRect.x) r.x = currentClipRect.x;
			     var clip = new Rect(0, 0, 1, 1);
			     if (wp.y < currentClipRect.y + 25)
			     {
			       //  MonoBehaviour.print(wp.y + " " +currentClipRect.y);
			         var difY = Mathf.Clamp(currentClipRect.y + 25 - wp.y, 0, r.height);
			         clip.y = difY / r.height;
			         clip.height -= clip.y;
			     }
			
			     Graphics.DrawTexture(r, context.image, clip, 0, 0, 0, 0);
			 }*/
			
			/*    var oldC = GUI.color;
			    GUI.color = oldC * color;
			    if ( !o.activeInHierarchy )
			    {   var c1 = GUI.color;
			        c1.a *= 0.2f;
			        GUI.color = oldC * c1;
			    }*/
			var o = __o.go;
			var col = color;
			
			if ( !o.activeInHierarchy ) col.a *= 0.2f;
			
			// Graphics.DrawTexture(r, context.image);
			//if ( (__o.FLAGS & 1) != 0 ) Debug.Log( __o.BG_RECT );
			if ( adapter.DRAW_ICONS_SHADOW || (__o.FLAGS & 1) != 0 && __o.BG_RECT.HasValue && __o.BG_RECT.Value.x + __o.BG_RECT.Value.width > r.x + r.width )
			{	var S = 4;
				var R = r;
				R.x -= S;
				R.y -= S;
				R.width += S * 2;
				R.height += S * 2;
				//   Adapter.DrawTexture( R, adapter.GetIcon( "HIPERUI_BUTTONGLOW" ), Color.black );
				Draw_AdapterTexture( R, adapter.GetIcon( "HIPERUI_BUTTONGLOW" ), col * Color.black );
			}
			
			//  Adapter.DrawTexture( r, context.image );
			var hasText = drawName && /*///Event.current.type == EventType.Repaint &&///*/ adapter.par.COMPS_SplitMode == 2 && callbackType != null;
			var drawBg=  adapter.DRAW_ICONS_MONO_BG || !hasText;
			
			if (drawBg)	Draw_GUITexture( r, overImage, col  );
			
			if ( hasText )
			{	/* labelStyle.fontSize = FONT_8();
				 r.y += r.height - labelStyle.fontSize / 2;
				 r.height = labelStyle.fontSize;
				 var c = Adapter.GET_SKIN().label.normal.textColor;
				 c.r = 1 - c.r;
				 c.g = 1 - c.g;
				 c.b = 1 - c.b;
				 labelStyle.normal.textColor = c;
				 r.x -= 1;
				 labelStyle.Draw(r, callbackType.Name, false, false, false, false);
				 r.x += 2;
				 r.y += 1;
				 labelStyle.Draw(r, callbackType.Name, false, false, false, false);
				
				 r.x -= 1;
				 labelStyle.normal.textColor = Adapter.GET_SKIN().label.normal.textColor;
				 labelStyle.Draw(r, callbackType.Name, false, false, false, false);*/
				// labelStyle.Draw(r, GET_STRING(callbackType.Name, 1), false, false, false, false);
				
				var n = callbackType.Name;
				/*	r.x += 1;
					r.y += 1;
					r.width -= 2;
					r.height -=2;*/
				
				labelStyleWhite.fontSize = drawBg ? 10 : 8;
				labelStyle.fontSize = drawBg ? 10 : 8;
				
				if ( n.Length != 0 )
				{	//labelStyle.Draw( r, n[0].ToString(), false, false, false, false );
					var texxt =  n[0].ToString().ToUpper();
					
					if (n.Length > 1 && !drawBg && adapter.DRAW_SECOND_CHAR_FOR_MONO)  texxt+= "<size="+(labelStyle.fontSize - 1) + ">" + n[1].ToString().ToLower() +"</size>";
					
					if (drawBg) Draw_Style( r, o.activeInHierarchy ? labelStyle : labelStyleWhite, texxt );
					else Draw_Style( r, /*EditorGUIUtility.isProSkin ? labelStyleWhite :*/ labelStyle, texxt, USE_GO: true );
				}
			}
			
			//  GUI.color = oldC;
			{	i = 0;
			
				for ( int j = 0 ; j < drawComps.Length ; j++ )
				{	if ( DrawDisable( drawComps[j] ) ) i++;
				}
				
				if ( i != 0 )
					//Adapter.DrawTexture( drawRect, adapter.GetIcon( i == drawComps.Length ? "DISABLE" : "DISABLEHALF" ) );
					Draw_AdapterTexture( drawRect, adapter.GetIcon( i == drawComps.Length ? "DISABLE" : "DISABLEHALF" )  );
			}
			
			
			
			/*   if (!o.activeInHierarchy)
			   {
			       /*    var col = Hierarchy.LINE;
			           col.r = col.g = col.b = 0.2f;
			           col.a = .8f;
			           Hierarchy.colorText11.SetPixel(0, 0, col);
			           Hierarchy.colorText11.Apply();
			           GUI.DrawTexture(drawRect, Hierarchy.colorText11, ScaleMode.ScaleAndCrop, true, 1);#1#
			
			       Hierarchy.FadeRect(r, 0.9f);
			
			   }*/
			
			//  if (drawRect.Contains(Event.current.mousePosition)) MyHierarchyIcon.ToolTip(context.tooltip);.
			r = drawRect;
			r.x -= 1;
			r.width += 2;
			r.y -= 1;
			r.height *= 0.75f;
			r.height += 2;
			
			r.y = firstRect.y;
			r.height = firstRect.height;
			r.width += 2;
			
			/* ------ */
			args.drawComps = drawComps.ToArray();
			Draw_Action( r, SET_ACTIVE_ACTION_HASH, args );
			
			/*if (Event.current.button == 1)
			{
			
			  if (!objectIsHiddenAndLock)
			  {
			      if (SceneView.lastActiveSceneView == null)
			      {
			          var pos = InputData.WidnwoRect(Event.current.mousePosition, 128, 68);
			          InputData.Init(pos, "", null, null, "Please open 'SceneView' window");
			      } else
			      {
			          stateForDrag_B1 = o;
			          //   SceneView.RepaintAll();
			          FRAME(o);
			      }
			
			  }
			
			}
			if (Event.current.isMouse) EventUse();*/
			/* ------ */
			
			
			// GUIUtility.all
			//
			
			/*   if ( adapter.ModuleButton( r, context, true ) )
			   {
			   }*/
			
			sendContent.text = "";
			sendContent.image = null;
			
			if (drawComps.Length > 1)
			{	sendContent.tooltip = /*comps.Where(comp => comp is MonoBehaviour)*/drawComps.Select( Adapter.GetTypeName ).Aggregate( ( a, b ) => a + '\n' + b );
			}
			
			else
			{	sendContent.tooltip = drawComps[0].GetType().Name;
			}
			
			args.drawComps = drawComps.ToArray();
			args.callbackType = callbackType;
			args.allowHide = allowHide;
			args.MenuText = MenuText;
			Draw_ModuleButton( r, sendContent, BUTTON_ACTION_HASH, true, args, true);
		}
		
		ARGS args;
		GUIContent sendContent = new GUIContent();
		
		Adapter. DrawStackMethodsWrapper __SET_ACTIVE_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper SET_ACTIVE_ACTION_HASH { get { return __SET_ACTIVE_ACTION_HASH ?? (__SET_ACTIVE_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( SET_ACTIVE_ACTION )); } }
		// int SET_ACTIVE_ACTION_HASH = "SET_ACTIVE_ACTION".GetHashCode();
		void SET_ACTIVE_ACTION( Rect worldOffset, Rect r, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{
		
		
		
			if (Event.current.rawType == EventType.MouseUp )
			{	RawOnUP();
			}
			
			RawOnUpDragComponents();
			
			if (Event.current.keyCode == KeyCode.Escape )
			{	if (RawOnUpDragComponents_Array != null ) RawOnUpDragComponents();
			}
			
			
			if (Event.current.control && stateForDrag_B0.HasValue && stateForDrag_B1 == _o.go && Event.current.type == EventType.MouseDrag
			        && !stateForDrag_B0.Value.Contains(Event.current.mousePosition ) )     //DragAndDrop.PrepareStartDrag();// reset data
			{	adapter.InternalClearDrag();
			
			
			
				if (Event.current.shift )
				{	var targetList = new List<Component>();
				
					foreach (var item in stateForDrag_B2 )
					{	try
						{	var newC = GameObject.Instantiate( item );// o.AddComponent();
							Hierarchy.Copy(item );
							
							if (Hierarchy.PastValidate(newC ) ) Hierarchy.Paste(newC );
							
							newC.gameObject.hideFlags = RawOnUpDragComponents_Flags;
							Undo.RegisterCreatedObjectUndo(newC, "Move Component" );
							Undo.RegisterCreatedObjectUndo(newC.gameObject, "Move Component" );
							targetList.Add(newC );
						}
						
						catch
						{
						
						}
					}
					
					if (RawOnUpDragComponents_Array != null ) RawOnUpDragComponents();
					
					RawOnUpDragComponents_Array = targetList.ToArray();
					// DragAndDrop.SetGenericData( "MoveComp", RawOnUpDragComponents_Array );
					stateForDrag_B2 = targetList.ToArray();
					adapter.PUSH_ONMOUSEUP(RawOnUpDragComponents );
				}
				
				DragAndDrop.objectReferences = stateForDrag_B2;
				
				
				
				
				// drawComps = emptyArr;
				DragAndDrop.StartDrag( "Dragging Component" );
				// DragAndDrop.
				//  EventUse();
				adapter.RepaintWindowInUpdate();
				Adapter.EventUse();
			}
			
			if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition ) )
			{	if (Event.current.button == 0 )
				{	if ( !stateForDrag_B0.HasValue ) adapter.PUSH_ONMOUSEUP(RawOnUP );
				
					stateForDrag_B0 = r;
					stateForDrag_B1 = _o.go;
					//  var drawComps = get_drawComps(_o.id);
					var drawComps = args.drawComps.Where(c=>c);
					stateForDrag_B2 = drawComps.ToArray();
				}
			}
		}
		
	}
}
}

