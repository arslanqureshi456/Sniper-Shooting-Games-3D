#if UNITY_EDITOR
	#define HIERARCHY
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
//namespace EModules
#if PROJECT
	using EModules.Project;
#endif
namespace EModules.EModulesInternal

{
internal partial class Adapter {
	internal partial class M_Colors : Adapter.Module {
	
		DrawStackAdapter _colorAdditionalStackAdapter;
		internal DrawStackAdapter ICON_STACK
		{	get { return _colorAdditionalStackAdapter ?? (_colorAdditionalStackAdapter = new DrawStackAdapter() { adapter = adapter }); }
		}
		internal override float Draw( Rect selectionRect, Adapter.HierarchyObject _o )
		{	//if (OPT_EV_BR(Event.current)) return 0;
		
			if ( !ICON_STACK.START_DRAW( selectionRect, _o ) ) return 0;
			
			
			if ( !_o.Validate() || adapter.MOI.des( _o.scene ) == null || adapter.IS_PREFAB_MOD_OPENED() )
			{	ICON_STACK.END_DRAW( _o );
				return width;
			}
			
			if ( callFromExternal() || !_o.internalIcon ) _o.drawIcon = GET_CONTENT( _o );
			
			_o.internalIcon = false;
			var labelPlace = _o.switchType == 1 &&  USE2018_3;
			var icon_place = labelPlace ? 0  : adapter._S_bgIconsPlace;
			
			if ( callFromExternal() ) icon_place = 2;
			
			var  buttonRectLabel = selectionRect;
			buttonRectLabel.width = EditorGUIUtility.singleLineHeight;
			var  buttonRectLeft = selectionRect;
			
			
			buttonRectLeft.width = selectionRect.x - adapter.foldoutStyleWidth;
			
			if ( _o.ChildCount( adapter ) == 0 ) buttonRectLeft.width += adapter.foldoutStyleWidth / 2;
			
			var left_padding = adapter.raw_old_leftpadding;
			
			// var left_padding = 0;
			//if ( UNITY_CURRENT_VERSION < UNITY_2019_2_0_VERSION ) left_padding = 0;
			if ( adapter.SETACTIVE_POSITION == 1 && (adapter._S_bgButtonForIconsPlace == 2 || adapter._S_bgButtonForIconsPlace == 0)/* && Adapter.UNITY_CURRENT_VERSION < Adapter.UNITY_2019_VERSION*/
			        && adapter.ENABLE_ACTIVEGMAOBJECTMODULE
			   )
			{	left_padding += EModules.EModulesInternal.Hierarchy.M_SetActive.ONE_POS_IW_FORCOLOR ;
			
				if (!  (_o.parent(adapter) != null || _o.ChildCount(adapter)== 0))
				{	left_padding-=4;
				
				}
				
				if ( UNITY_CURRENT_VERSION < UNITY_2019_2_0_VERSION ) buttonRectLeft.width -= 8;
			}
			
			
			buttonRectLeft.x = left_padding;
			// EditorGUI.DrawRect( buttonRectLeft, Color.white );
			
			if ( UNITY_CURRENT_VERSION >= UNITY_2019_2_0_VERSION ) buttonRectLeft.width -= left_padding;
			
			var  icon_rect = labelPlace ? GetIconRect(selectionRect, overrideSBGIconPlace: 0) : GetIconRect(selectionRect);
			
			EditorGUIUtility.SetIconSize( Vector2.zero );
			
			
			bool auto = _o.switchType > 0 || _o.cache_prefab;
			// float __IS = adapter.par.COLOR_ICON_SIZE;
			float __IS = adapter.par.COLOR_ICON_SIZE - ( adapter. DEFAULT_ICON_SIZE - EditorGUIUtility.singleLineHeight) / 2;
			
			// if  (__o. cache_prefab )__IS = adapter.DEFAULT_ICON_SIZE;
			if ( _o.switchType == 1 ) __IS = adapter.DEFAULT_ICON_SIZE;
			
			if ( _o.switchType == 2 ) __IS = EditorGUIUtility.singleLineHeight;
			
			var ICON_SIZE = Mathf.CeilToInt(__IS  - EditorGUIUtility.singleLineHeight );
			
			if ( icon_place != 2 ) icon_rect.x -= Mathf.CeilToInt(ICON_SIZE / 2f);
			
			icon_rect.y -= ICON_SIZE / 2;
			icon_rect.width += ICON_SIZE;
			icon_rect.height += ICON_SIZE;
			
			
			/* if ( adapter._S_bgIconsPlace == 1 )
			 {   var centerX = icon_rect.x + icon_rect.width / 2;
			     var centerY = icon_rect.y + icon_rect.height / 2;
			     buttonRectLeft.x = centerX - buttonRectLeft.width / 2 ;
			     buttonRectLeft.y = centerY - buttonRectLeft.height / 2;
			 }*/
			
			
			
			if ( !callFromExternal()  /* && adapter.IsSelected(__o.id)*/)
			{
			
			
			
			
			
				//GUI.DrawTexture(label_icon_rect, Texture2D.whiteTexture, ScaleMode.ScaleToFit, true, 0.0f, active ? Color.white : inactiveColor, 0.0f, 0.0f);
				// if ( HighlighterHasKey(__o.scene, __o))
				if ( _o.BACKGROUNDED != 0 && _o.switchType != 1 )
				{
				
				
					var  label_icon_rect = GetIconRectIfNextToLabel(selectionRect, GetIconRectIfNextToLabelType.DefaultIcon);
					var treeItem =  !adapter.IS_SEARCH_MOD_OPENED() && !callFromExternal() ?  _o.GetTreeItem(adapter) : null;
					var active = _o.Active();
					
					if ( (!_o.drawIcon.add_icon || adapter._S_bgIconsPlace != 0) && adapter.HAS_LABEL_ICON() )
					{
					
						/*  var  label_icon_rect = GetIconRectIfNextToLabel(selectionRect, GetIconRectIfNextToLabelType.DefaultIcon);
						  var treeItem = __o.GetTreeItem(adapter);
						  var active = __o.Active();*/
						//  EditorUtility.GetIconInActiveState( this.GetIconForItem( item ) );
						var targetIcon = treeItem != null ? treeItem.icon : null;
						var skipoverlay = !targetIcon;
						
						//if ( !targetIcon && adapter.HAS_LABEL_ICON()  ) targetIcon = (Texture2D)Utilities.__internal_ObjectContent(adapter, __o.GetHardLoadObject(), __o.GET_TYPE(adapter)).add_icon;
						if ( !targetIcon && adapter.HAS_LABEL_ICON() )
						{
						
							/* MethodInfo gi = null;
							 if ( gi == null )
							 {   var ty = typeof(EditorGUIUtility);
							     gi = ty.GetMethod( "GetIconForObject",  (BindingFlags) (-1));
							
							 }
							 targetIcon = gi.Invoke( null, new object[1] { __o.GetHardLoadObject() } ) as Texture2D;*/
							if ( adapter.IS_PROJECT() )
							{	targetIcon = EditorGUIUtility.ObjectContent( _o.GetHardLoadObject(), _o.GET_TYPE( adapter ) ).image as Texture2D;
							
							}
							
							else
							{	var loadObject = _o.go;
							
								if ( adapter.FindPrefabRoot( loadObject ) != loadObject ) loadObject = null;
								
								targetIcon = EditorGUIUtility.ObjectContent( loadObject, _o.GET_TYPE( adapter ) ).image as Texture2D;
								
								if ( _o.drawIcon.add_icon && _o.drawIcon.add_icon == targetIcon )
								{	____SetIconOnlyInternal( _o, null );
									targetIcon = EditorGUIUtility.ObjectContent( loadObject, _o.GET_TYPE( adapter ) ).image as Texture2D;
									____SetIconOnlyInternal( _o, _o.drawIcon.add_icon as Texture2D );
								}
							}
							
						}
						
						if ( targetIcon && active )
						{
						
						
						
							// if ( adapter.DRAW_ICONS_SHADOW )
							{	var S = 4;
								var R = label_icon_rect;
								R.x -= S;
								R.y -= S;
								R.width += S * 2;
								R.height += S * 2;
								
								
								//  Adapter.DrawTexture( R, adapter.GetIcon( "HIPERUI_BUTTONGLOW" ), Color.black );
								ICON_STACK.Draw_AdapterTexture( R, adapter.GetIcon( "HIPERUI_BUTTONGLOW" ), Color.black, true );
							}
							
							if ( !_o.Active() )
							{	/*var oldC = GUI.color;
								GUI.color *= inactiveColor;
								
								Adapter.DrawTexture( label_icon_rect, targetIcon, active ? Color.white : inactiveColor );
								GUI.color = oldC;*/
								ICON_STACK.Draw_AdapterTexture( label_icon_rect, targetIcon, active ? Color.white : inactiveColor, false );
							}
							
							else
							{	// Adapter.DrawTexture( label_icon_rect, targetIcon, active ? Color.white : inactiveColor );
								ICON_STACK.Draw_AdapterTexture( label_icon_rect, targetIcon, active ? Color.white : inactiveColor, false );
							}
							
							
							
							if ( !skipoverlay )
							{
							
								overlayButStr.treeItem = treeItem;
								overlayButStr.active = active;
								ICON_STACK.Draw_Action( label_icon_rect, ICON_OVERLAY_ACTION_HASH, overlayButStr );
							}
						}
					}
					
					if ( treeItem != null )
					{
					
					
						//LabelOverlayGUI( selectionRect, treeItem );
						overlayButStr.treeItem = treeItem;
						ICON_STACK.Draw_Action( selectionRect, LABEL_OVERLAY_ACTION_HASH, overlayButStr );
						
						if (  /*treeItem.hasChildren &&*/ active && (_o.BACKGROUNDED != 2 || (_o.FLAGS & 1) != 1) && _o.ChildCount( adapter ) != 0 && !adapter.IS_SEARCH_MOD_OPENED() )
						{
						
						
							/*   if ( Adapter.UNITY_CURRENT_VERSION < Adapter.UNITY_2019_3_0_VERSION )
							   {   label_icon_rect.x -= adapter.foldoutStyleWidth + 1;
							       label_icon_rect.width = adapter.foldoutStyle.fixedWidth;
							       var d = label_icon_rect.height - adapter.foldoutStyleWidth;
							       label_icon_rect.y += d / 2 - 1;
							       / *   if ( adapter.USE_LABEL_OFFSET )* /
							       label_icon_rect.y -= 3;
							       label_icon_rect.y = Mathf.FloorToInt( label_icon_rect.y );
							       label_icon_rect.height = adapter.foldoutStyle.fixedWidth;
							   }
							   else*/
							{	label_icon_rect.x -= adapter.foldoutStyleWidth + 1;
								label_icon_rect.width = adapter.foldoutStyle.fixedWidth;
								var d = label_icon_rect.height - adapter.foldoutStyleHeight;
								label_icon_rect.y += d / 2;
								//label_icon_rect.y -= EditorGUIUtility.singleLineHeight / 2;
								label_icon_rect.y = Mathf.FloorToInt( label_icon_rect.y );
								label_icon_rect.height = adapter.foldoutStyleHeight;
							}
							
							//EditorGUI.DrawRect(label_icon_rect, Color.white);
							
							overlayButStr.treeItem = treeItem;
							ICON_STACK.Draw_Action( label_icon_rect, FOLD_ACTION_HASH, overlayButStr );
							
							
						}
					}
					
					
				}
				
			}
			
			
			
			/* var defaultX = icon_rect.x;
			 var defaultY = icon_rect.y;
			 var defaultHeight = icon_rect.height;
			 var HHH = Mathf.Min(EditorGUIUtility.singleLineHeight, icon_rect.height);
			 var WWW = icon_rect.x - HHH;*/
			
			// icon_rect.x -= adapter.par.COLOR_ICON_SIZE + 20;
			
			/* var  buttonRect = icon_rect;
			 buttonRect.height -= 2;
			 if (!adapter.IS_PROJECT())
			 {   buttonRect.x = 0;
			     buttonRect.width = WWW;
			 }*/
			
			/*  icon_rect.width = icon_rect.height = adapter.par.COLOR_ICON_SIZE;
			  icon_rect.x += (icon_rect.width - icon_rect.width) / 2 + (adapter.par.COLOR_ICON_SIZE - 12) / 2f;
			  icon_rect.y += (icon_rect.height - icon_rect.height) / 2;*/
			
			/* standrardRect.width = standrardRect.height = EditorGUIUtility.singleLineHeight;
			 standrardRect.x += (oldW - standrardRect.width) / 2 + (EditorGUIUtility.singleLineHeight - 12) / 2f;
			 standrardRect.y += (oldH - standrardRect.height) / 2;*/
			
			
			
			
			
			
			
			
			
			
			if ( _o.drawIcon.add_icon )
			{
			
			
			
			
				//if (__o.switchType == 0)  Debug.Log(__o.drawIcon.add_hasiconcolor);
				var COLOR = Color.white;
				
				if ( _o.switchType == 0 && _o.drawIcon.add_hasiconcolor )
				{	//backCol = GUI.color;
					/*if (!adapter.DISABLE_DES() && IconColorCacher.HasKey( __o.scene, __o ))
					{   var get = IconColorCacher.GetValue(__o.scene, __o );
					    tc.r = get.list[0] / 255f;
					    tc.g = get.list[1] / 255f;
					    tc.b = get.list[2] / 255f;
					    tc.a = Mathf.Clamp01( get.list[3] / 255f  );
					    GUI.color *= tc;
					}*/
					COLOR *= _o.drawIcon.add_iconcolor;
					// if ( IconImageCacher.HasKey( __o.scene , __o ) ) Debug.Log( GUI.color );
				}
				
				
				/*  if (!__o.Active())
				  {   var oldC = GUI.color;
				      GUI.color *= inactiveColor;
				      GUI.DrawTexture( icon_rect, drawIcon, ScaleMode.ScaleToFit);
				      GUI.color = oldC;
				  }
				  else
				  {   GUI.DrawTexture( icon_rect, drawIcon, ScaleMode.ScaleToFit);
				      {
				  }*/
				/*if ( adapter.SETACTIVE_POSITION == 1 && Adapter.UNITY_CURRENT_VERSION < Adapter.UNITY_2019_VERSION
				        && adapter.par.ENABLE_ACTIVEGMAOBJECTMODULE && adapter._S_bgIconsPlace == 2 ) icon_rect.x += EModules.EModulesInternal.Hierarchy.M_SetActive.ONE_POS_IW_FORCOLOR;
				    */
				var c = (_o.switchType == 2 || _o.Active() ? Color.white : inactiveColor) * COLOR;
				
				if ( _o.switchType == 1 )
				{	var dw = (icon_rect.width - 7) / 2;
					var dh = (icon_rect.height - 7) / 2;
					// var d = (icon_rect.width - adapter.parLINE_HEIGHT) / 2;
					//   Adapter.DrawTexture( new Rect( icon_rect.x + dw, icon_rect.y + dh, 7, 7 ), _o.drawIcon.add_icon, _o.switchType == 2 || _o.Active() ? Color.white : inactiveColor );
					ICON_STACK.Draw_AdapterTexture( new Rect( icon_rect.x + dw, icon_rect.y + dh, 7, 7 ), (Texture2D)_o.drawIcon.add_icon, c, false );
				}
				
				else
				{	//Adapter.DrawTexture( icon_rect, _o.drawIcon.add_icon, _o.switchType == 2 || _o.Active() ? Color.white : inactiveColor );
					ICON_STACK.Draw_AdapterTexture( icon_rect, (Texture2D)_o.drawIcon.add_icon, c, false );
					
					if ( adapter._S_bgIconsPlace == 2 )
					{	ICON_STACK.Draw_Action( icon_rect, SKIP_CHILD_COUNT_ACTION_HASH, null );
					}
				}
				
				if ( !adapter.IS_SEARCH_MOD_OPENED() && !callFromExternal() )
				{	//IconOverlayGUI( icon_rect, _o.GetTreeItem( adapter ) );
					overlayButStr.treeItem = _o.GetTreeItem( adapter );
					ICON_STACK.Draw_Action( icon_rect, SECOND_ICON_OVERLAY_ACTION_HASH, overlayButStr );
				}
				
				if ( !auto && /*adapter.IS_PROJECT() &&*/ adapter.par.BottomParams.DRAW_FOLDER_STARMARK )
				{	//Adapter.DrawTexture( icon_rect, adapter.GetIcon( "FOLDER_STARMARK" ), Color.white );
					ICON_STACK.Draw_AdapterTexture( icon_rect, adapter.GetIcon( "FOLDER_STARMARK" ), Color.white, true );
					
				}
				
				/*   if (!o.activeInHierarchy)
				   {
				       Hierarchy.FadeRect(drawRect, 0.7f);
				   }*/
				if ( _o.switchType == 0 && _o.drawIcon.add_hasiconcolor )
				{	// GUI.color = backCol;
				}
				
				
				switch ( _o.switchType )
				{	case 0: switchedConten.tooltip = !_o.drawIcon.add_icon ? "" : _o.drawIcon.add_icon.name; break;
				
					case 1: switchedConten.tooltip = "" /*contentNull.tooltip*/; break;
					
					case 2: switchedConten.tooltip = contentMis.tooltip; break;
				}
			}
			
			else
				if ( !adapter.IS_PROJECT() )
				{	/* tRr = oldRect;
					     tRr.width -= 10;
					     tRr.y += (tRr.height - tRr.width - 4) / 2;
					     tRr.height = tRr.width;
					     tRr.y += 2;
					     if (!ICON_LEFT) tRr.x += adapter.par.COLOR_ICON_SIZE / 2 - tRr.width / 2;
					     var c = GUI.color;
					     GUI.color = alpha;
					     GUI.DrawTexture( tRr, adapter.GetIcon( "UNLOCK" ) );
					     GUI.color = c;*/
					
					//  if ( __o.name == "Directional Light (6)" ) Debug.Log( "ASD" );
					// CHeckIcon( __o ,)
					switchedConten.tooltip = content.tooltip;
					
				}
				
				
				
				
				
			// content.tooltip = base.ContextHelper;
			// oldRect.width = Math.Max(16,par.COLOR_ICON_SIZE);
			/*if (switchedConten.image) {
			     switchedConten.image = null;
			     GUI.DrawTexture( oldRect, switchedConten.image );
			   }*/
			var DIF = /*(adapter.parLINE_HEIGHT - 16) / 2*/ + 2;
			buttonRectLeft.y += DIF;
			buttonRectLeft.height -= DIF * 2;
			
			//Rect currentRect = Rect.zero;
			//   var but = false;
			if ( adapter._S_bgButtonForIconsPlace == 0 || adapter._S_bgButtonForIconsPlace == 2 )
			{	/*if ( Button( buttonRectLeft, switchedConten ) )
				{   but = true;
				    // currentRect = buttonRectLeft;
				}*/
				colButStr.localSelectionRect = ICON_STACK.ConvertToLocal( selectionRect);
				ICON_STACK.Draw_SimpleButton( buttonRectLeft, switchedConten, BUTTON_ACTION_HASH, colButStr, true );
			}
			
			if ( adapter._S_bgButtonForIconsPlace == 1 || adapter._S_bgButtonForIconsPlace == 2 )
			{
			
			
			
			
				//   var fixedRect = adapter.HoverRect(selectionRect, 1, 0);
				var fixedRect = adapter.HoverFullRect(selectionRect);
				buttonRectLabel.x = fixedRect.x;
				buttonRectLabel.width = fixedRect.width - 2;
				
				colButStr.localSelectionRect = ICON_STACK.ConvertToLocal(selectionRect);
				ICON_STACK.Draw_SimpleButton( buttonRectLabel, switchedConten, BUTTON_ACTION_HASH,  colButStr, true );
				//ICON_STACK.Draw_AdapterTexture(buttonRectLabel, Color.white);
				/* if ( Button( buttonRectLabel, switchedConten ) )
				 {   but = true;
				     // currentRect = buttonRectLabel;
				 }*/
			}
			
			//EditorGUI.DrawRect( buttonRectLeft, Color.white );
			//EditorGUI.DrawRect( buttonRectLabel, Color.white );
			
			
			
			/* if ( but && )
			 {
			 }*/
			
			ICON_STACK.END_DRAW( _o );
			return width;
		}
		
		
		
		Adapter. DrawStackMethodsWrapper __FOLD_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper FOLD_ACTION_HASH { get { return __FOLD_ACTION_HASH ?? (__FOLD_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( FOLD_ACTION )); } }
		void FOLD_ACTION(Rect worldOffset, Rect label_icon_rect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{
		
			var overlayButStr = ( OverlayButtonStr)data. args;
			var treeItem = overlayButStr.treeItem;
			//label_icon_rect.x += worldOffset.x;
			//label_icon_rect.y += worldOffset.y;
			
			if ( EditorGUIUtility.isProSkin )
			{
			
			
				var c = GUI.color;
				var CCC = _o. BACKGROUNDEsourceBgColorD;
				var l = (CCC.r + CCC.g +  CCC.b) ;
				CCC.g = CCC.r = CCC.b = 1 - l / 2;
				CCC.a = 1;
				var expandedState =  DoFoldout(label_icon_rect, treeItem, _o.id);
				
				/* GUI.color = Color.black ;
				 if (Event.current.type == EventType.Repaint) adapter.foldoutStyle.Draw(new Rect(label_icon_rect.x - 1, label_icon_rect.y, label_icon_rect.width, label_icon_rect.height)
				             , GUIContent.none, 0,
				             expandedState);
				 if (Event.current.type == EventType.Repaint) adapter.foldoutStyle.Draw(new Rect(label_icon_rect.x + 1, label_icon_rect.y, label_icon_rect.width, label_icon_rect.height)
				             , GUIContent.none, 0, expandedState);*/
				//if (!label_icon_rect.Contains( Event.current.mousePosition ) || Event.current.keyCode != KeyCode.Mouse0)
				GUI.color *= CCC;
				
				//EditorGUI.DrawRect(label_icon_rect, Color.white);
				//if ( Event.current.type == EventType.Repaint ) adapter.foldoutStyle.Draw( label_icon_rect,
				//	        GUIContent.none, false, label_icon_rect.Contains( Event.current.mousePosition ) && Event.current.button == 0, expandedState, false );
				
				
				GUI.Toggle( label_icon_rect, expandedState, GUIContent.none, adapter.foldoutStyle );
				
				GUI.color = c;
			}
			
			else
			{	var expandedState =  DoFoldout(label_icon_rect, treeItem, _o.id);
				GUI.Toggle( label_icon_rect, expandedState, GUIContent.none, adapter.foldoutStyle );
				
			}
		}
		
		Adapter. DrawStackMethodsWrapper __SKIP_CHILD_COUNT_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper SKIP_CHILD_COUNT_ACTION_HASH { get { return __SKIP_CHILD_COUNT_ACTION_HASH ?? (__SKIP_CHILD_COUNT_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( SKIP_CHILD_COUNT_ACTION )); } }
		void SKIP_CHILD_COUNT_ACTION( Rect worldOffset, Rect label_icon_rect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{	adapter.SKIP_CHILDCOUNT = true;
		}
		
		
		
		internal struct OverlayButtonStr
		{	internal UnityEditor.IMGUI.Controls.TreeViewItem treeItem;
			internal bool active;
		}
		OverlayButtonStr overlayButStr;
		Adapter. DrawStackMethodsWrapper __ICON_OVERLAY_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper ICON_OVERLAY_ACTION_HASH { get { return __ICON_OVERLAY_ACTION_HASH ?? (__ICON_OVERLAY_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( ICON_OVERLAY_ACTION )); } }
		void ICON_OVERLAY_ACTION( Rect worldOffset, Rect label_icon_rect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{
		
			var overlayButStr = ( OverlayButtonStr) data.args;
			IconOverlayGUI( label_icon_rect, overlayButStr.treeItem );
			OverlayIconGUI( label_icon_rect, overlayButStr.treeItem, overlayButStr.active );
		}
		Adapter. DrawStackMethodsWrapper __SECOND_ICON_OVERLAY_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper SECOND_ICON_OVERLAY_ACTION_HASH { get { return __SECOND_ICON_OVERLAY_ACTION_HASH ?? (__SECOND_ICON_OVERLAY_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( SECOND_ICON_OVERLAY_ACTION )); } }
		void SECOND_ICON_OVERLAY_ACTION( Rect worldOffset, Rect label_icon_rect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o)
		{
		
			var overlayButStr = ( OverlayButtonStr)data. args;
			IconOverlayGUI( label_icon_rect, overlayButStr.treeItem );
			//   OverlayIconGUI( label_icon_rect , overlayButStr.treeItem , overlayButStr.active );
		}
		Adapter. DrawStackMethodsWrapper __LABEL_OVERLAY_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper LABEL_OVERLAY_ACTION_HASH { get { return __LABEL_OVERLAY_ACTION_HASH ?? (__LABEL_OVERLAY_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( LABEL_OVERLAY_ACTION )); } }
		void LABEL_OVERLAY_ACTION( Rect worldOffset, Rect selectionRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{	var overlayButStr = ( OverlayButtonStr) data.args;
			LabelOverlayGUI( selectionRect, overlayButStr.treeItem );
		}
		
		
		
		internal struct ColorButtonStr
		{	internal Rect localSelectionRect;
		
		}
		ColorButtonStr colButStr;
		
		Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper BUTTON_ACTION_HASH { get { return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION )); } }
		void BUTTON_ACTION(Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{
		
		
			if ( adapter.IsSceneHaveToSave( _o ) ) return;
			
			
			if ( Event.current.button == 0 /*&& !Application.isPlaying*/)
			{	/*var pos = InputData.WidnwoRect( !callFromExternal(),
				                                            //  Event.current.mousePosition - new Vector2(0, EditorGUIUtility.singleLineHeight * 2)
				                                            new Vector2(Event.current.mousePosition.x, currentRect.y - EditorStyles.foldout.lineHeight * 1.25f)
				                                            , 0, 0, adapter, lockPos: true);*/
				
				
				var colButStr = (ColorButtonStr)data.args;
				var selectionRect = colButStr.localSelectionRect;
				selectionRect.x += worldOffset.x;
				selectionRect.y += worldOffset.y;
				//  var mp = Event.current.mousePosition;
				var mp = new Vector2(Event.current.mousePosition.x, selectionRect.y + selectionRect.height * 1.4f);
				// - new Vector2(0, EditorGUIUtility.singleLineHeight * 1.5f)
				var pos = new MousePos( mp, MousePos.Type.Highlighter_410_0, false, adapter);
				// var pos = new MousePos( mp, MousePos.Type.Highlighter_410_0, !callFromExternal(), adapter);
				
				/*  mp = GUIUtility.GUIToScreenPoint( mp );
				  var pos = new Rect(mp.x, mp.y, 0, 0);*/
				
				Action<Texture, string> SET_TEXTURE = (currentSelection, undoStr) =>
				{
				
				
					if (currentSelection == null)
					{	SetIcon(_o, (Texture2D)null);
					}
					
					else
					{	var library = AssetDatabase.GetAssetPath(currentSelection).StartsWith("Library/");
						string texName = library
						                 ? currentSelection.name
						                 : "GUID=" + AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(currentSelection));
						                 
						if (!library)
						{	Hierarchy_GUI.Undo(adapter, undoStr);
							Hierarchy_GUI.GetLastList(adapter).RemoveAll(t => t == texName);
							
							if (Hierarchy_GUI.GetLastList(adapter).Count == 0) Hierarchy_GUI.GetLastList(adapter).Add(texName);
							else Hierarchy_GUI.GetLastList(adapter).Insert(0, texName);
							
							while (Hierarchy_GUI.GetLastList(adapter).Count > 20) Hierarchy_GUI.GetLastList(adapter).RemoveAt(20);
							
							Hierarchy_GUI.SetDirtyObject(adapter);
						}
						
						SetIcon(_o, (Texture2D)currentSelection);
					}
					
					
					
				};
				//
				
				var capture_o = _o;
				var GET_TEXTURE = Utilities.ObjectContent_NoCacher(adapter, EditorUtility.InstanceIDToObject(_o.id), adapter.t_GameObject).add_icon;
				
				//"Change Highlighter Color"
				
				/*    Action<SingleList> SET_TEXT_COLOR = (singleList) =>
				  {   SetAction(capture_o, (_o) =>
				      {   if (singleList == null || !_o.Validate(true)) return;
				          var c = singleList.list;
				          // var crs = Converter(singleList);
				          var d = adapter.MOI.des( _o.scene );
				          adapter.SET_UNDO(d, "Change Text Color");
				          cacheDic.Clear();
				          / *
				                                          if (c.All(ca => ca == 0))
				                                          {
				                                              var list = getDoubleList(_o.scene);
				                                              list.RemoveAll(_o);
				                                          } else
				                                          {
				                                              SetValue(new Color32((byte)c[0], (byte)c[1], (byte)c[2], (byte)c[3]), c[4] == 1, _o.scene, _o);
				                                          }* /
				        Color32 color1 = c[0] == 0 & c[1] == 0 & c[2] == 0 & c[3] == 0 ? (Color32)Adapter.TRANSP_COLOR : new Color32((byte)c[0], (byte)c[1], (byte)c[2], (byte)c[3]);
				          Color32 color2 = c.Count < 9 || c[5] == 0 & c[6] == 0 & c[7] == 0 & c[8] == 0 ? (Color32)Adapter.TRANSP_COLOR : new Color32((byte)c[5], (byte)c[6], (byte)c[7], (byte)c[8]);
				
				          SetValue(color1 == Adapter.TRANSP_COLOR && color2 == Adapter.TRANSP_COLOR ? null : new[] { color1, color2 }, c[4] == 1, _o.scene, _o);
				      });
				  };*/
				
				/*   SingleList tempList;
				   Func<SingleList, SingleList> GetCopyedList*/
				
				Func<TempColorClass> GET_HIGLIGHTER_COLOR = () =>
				{	var l_o = capture_o;
					TempColorClass _tempColor = new TempColorClass();
					
					if (!l_o.Validate(true)) return _tempColor.empty;
					
					var c = GetHighlighterValue(l_o.scene, l_o);
					
					if (c != null)
					{	/*var ass = _tempColor.AssignFromList( c );
						Debug.Log(ass.LABELCOLOR);*/
						return _tempColor.AssignFromList( c );
					}
					
					// if (c != null) return c;
					else return _tempColor.empty;
				};
				
				Action<TempColorClass, string> SET_HIGLIGHT_COLOR = (el, undoName) =>
				{	SetAction(capture_o, (in_o) =>
					{	if ( !in_o.Validate(true) ) return;
					
						//  var c = singleList.list;
						// var crs = Converter(singleList);
						var d = adapter.MOI.des( in_o.scene );
						adapter.SET_UNDO(d, undoName);
						//Debug.Log(GET_HIGLIGHTER_COLOR().BG_ALIGMENT_LEFT_CONVERTED + "\n" + GET_HIGLIGHTER_COLOR().HAS_BG_COLOR);
						// Undo.RecordObject(d.gameObject, "Change Highlighter Color");
						
						// MonoBehaviour.print(c[4] == 1);
						SetHighlighterValue( el, in_o.scene, in_o);
						// adapter.SetDirtyDescription(d, in_o.scene);
					});
				};
				
				
				/*  Func<SingleList> GET_TEXT_COLOR = () =>
				  {   var _o = capture_o;
				      if (!_o.Validate(true)) return new SingleList() { list = new int[9].ToList() };
				      var c = GetValue(_o.scene, _o);
				      if (c != null) return new SingleList() { list = c.list.ToList() };
				      // if (c != null) return c;
				      else return new SingleList() { list = new int[9].ToList() };
				  };*/
				
				
				M_Colors_Window.Init( pos, "Select Icon", SET_TEXTURE, _o.drawIcon != null && _o.drawIcon.add_icon ? GET_TEXTURE : null, SET_HIGLIGHT_COLOR, GET_HIGLIGHTER_COLOR
				                      ,
				                      ( singleList, undoStr ) =>
				{	SetAction( capture_o, ( l_o ) =>
					{	if ( singleList == null || !l_o.Validate( true ) ) return;
					
						var c = singleList.list;
						
						var d = adapter.MOI.des(l_o.scene);
						adapter.SET_UNDO( d, undoStr );
						IconColorCacher.cacheDic.Clear();
						
						/* if (c.All(ca => ca == 255) || c.All(ca => ca == 0))
						 {
						     var list = IconColorCacher.getDoubleList(_o.scene);
						     list.RemoveAll(_o);
						 } else
						 {
						     // var c2 = singleList.list;
						     IconColorCacher.SetValue(singleList, _o.scene, _o, true);
						 }*/
						// MonoBehaviour.print(c[0]);
						/* if ( adapter.pluginID == 0 && adapter.HAS_LABEL_ICON() ) {
						     var ic = GET_TEXTURE;
						     if ( ic ) {
						         if ( AssetDatabase.GetAssetPath( ic ).StartsWith( "Library/" )){
						             SetIcon
						         }
						         / *) guid = ;
						         if ( !string.IsNullOrEmpty( guid ) && guid.StartsWith( "Assets" ) ) guid = AssetDatabase.AssetPathToGUID( guid );
						         if ( !string.IsNullOrEmpty( guid ) ) {   // SetImageForObject_OnlyCacher(_o, guid);
						
						             IconImageCacher.SetValue( new Int32List() {
						                 GUIDsActiveGameObject_CheckAndGet = guid ,
						                 PATHsActiveGameObject = AssetDatabase.GUIDToAssetPath( guid )
						             } , _o.scene , SetPair( _o ) , true );
						
						         }* /
						     }
						 }*/
						
						
						
						IconColorCacher.SetValue( c.All( ca => ca == 255 ) || c.All( ca => ca == 0 ) ? null : singleList, l_o.scene, SetPair( l_o ), true );
						Hierarchy_GUI.SetDirtyObject( adapter );
						ClearCache();
						adapter.RepaintWindowInUpdate();
					} );
				}
				,
				() =>
				{	var l_o = capture_o;
				
					if ( !l_o.Validate( true ) ) return new SingleList() { list = new List<int>() { 255, 255, 255, 255 } };
					
					var c = IconColorCacher.GetValue(l_o.scene, l_o);
					
					if ( c != null ) return new SingleList() { list = c.list.ToList() };
					else return new SingleList() { list = new List<int>() { 255, 255, 255, 255 } };
				}, _o
				, adapter
				
				                    );
			}
			
			if ( Event.current.button == 1 )
			{	Adapter.EventUse();
				/*    int[] contentCost = new int[0];
				    GameObject[] obs = new GameObject[0];
				
				    if (drawIcon)
				    {
				        if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeader(out obs, out contentCost, drawIcon);
				        FillterData.Init(Event.current.mousePosition, SearchHelper + " '" + drawIcon.name + "' icon", "'" + drawIcon.name + "' icon", obs, contentCost, null, this);
				
				    } else
				    {
				        CallHeader(out obs, out contentCost);
				        FillterData.Init(Event.current.mousePosition, SearchHelper + " user's icons", "any user's icons", obs, contentCost, null, this);
				
				    }*/
				
				var icon = _o.drawIcon.add_icon;
				
				//if ( adapter.IS_PROJECT() ) __o.drawIcon.add_icon = GetImageForObject_OnlyCacher( __o ).add_icon;
				if ( adapter.IS_PROJECT() )     //var treeItem = __o.GetTreeItem(adapter);
				{	if ( !icon )
					{	icon = AssetDatabase.GetCachedIcon( _o.project.assetPath );
					
						if ( AssetDatabase.GetAssetPath( icon ) == "Library/unity editor resources" && icon.name == "Folder Icon" ) icon = null;
					}
				}
				
				var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
				
				if ( icon )
				{	var ttt = icon.name.Replace(adapter.pluginname + "_KEY_#1", "default");
				
					_W__SearchWindow.Init( mp, SearchHelper + " '" + ttt + "' icon", "'" + ttt + "' icon",
					                       CallHeaderFiltered( icon ),
					                       this, adapter, _o, adapter.IS_HIERARCHY() ? null : _o.project.fileExtension );
				}
				
				else
				{	_W__SearchWindow.Init( mp, SearchHelper + " User's icons Only", "any user's icons",
					                       CallHeader(),
					                       this, adapter, _o );
				}
				
				
				// EditorGUIUtility.ic
			}
		}
		
	}
}
}

