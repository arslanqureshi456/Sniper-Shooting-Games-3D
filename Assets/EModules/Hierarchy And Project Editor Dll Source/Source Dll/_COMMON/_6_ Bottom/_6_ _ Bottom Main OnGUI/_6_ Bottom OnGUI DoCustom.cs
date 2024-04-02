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
    internal Hierarchy.M_CustomIcons M_CustomIconsModule;
    // internal IModuleOnnector_M_CustomIcons IM_CustomIconsModule;
    
    internal partial class BottomInterface {
    
    
    
    
    
        private void DoCustom( Rect line, int LH, BottomController controller, int scene )       // refStyle = EditorStyles.helpBox;
        {   refStyle = EditorStyles.toolbarButton;
            // refStyle = EditorStyles.helpBox;
            // refStyle = Adapter.GET_SKIN().button;
            //  refColor = ts2t;
            refColor = tst;
            //  refColor = Color.white;
            
            adapter.bottomInterface.GET_BOOKMARKS( ref list, scene );
            
            
            if ( !controller.CheckCategoryIndex( scene ) )
            {   var c = controller.GetCurerentCategoryName();
            
                var tooltip = GETTOOLTIPPEDCONTENT(MemType.Custom, null, controller);
                tooltip.text = "\"" + Adapter.GET_SCENE_BY_ID( scene ).name + "\" doesn't contain a \"" + c + "\" category";
                tooltip.tooltip = "";
                Label( line, tooltip );
                
                return;
            }
            
            
            var BG_COLOR = list[controller.GetCategoryIndex(scene)].GET_COLOR() ?? RowsParams[PLUGIN_ID.BOOKMARKS].BgColorValue;
            if ( controller.GetCategoryIndex( scene ) == 0 ) BG_COLOR = adapter.bottomInterface.RowsParams[0].BgColorValue;
            
            if ( Event.current.type == EventType.Repaint )
            {   if ( BG_COLOR.a != 0 )
                    Adapter.DrawRect( line, BG_COLOR * GUI.color );
                //else
                //  EditorStyles.helpBox.Draw( line, /*new GUIContent(""), */false, false, false, false );
            }
            /* if (Event.current.type == EventType.repaint)
                 EditorStyles.helpBox.Draw(line, /* new GUIContent(""),#1# false, false, false, false);*/
            var l = line;
            
            
            //  l.x = 0;
            l.y -= 1;
            //  l.height -= 4;
            /*var cc = GUI.color;
            if (par.BottomParams.FAVORITS_TINT_BUTTONS && par.BottomParams.FAVORITS_COLOR.HaveColor) GUI.color = GUI.color * Color.Lerp(Color.white, par.BottomParams.FAVORITS_COLOR, par.BottomParams.FAVORITS_COLOR.a);
            */
            if ( !DrawButtons( l, LH, MemType.Custom,
                               // RowsParams[PLUGIN_ID.BOOKMARKS].HiglighterValue && BG_COLOR.a != 0 ? Color.Lerp( WHITE , BG_COLOR , BG_COLOR.a ) : WHITE
                               BG_COLOR.a != 0 ? Color.Lerp( WHITE, BG_COLOR, BG_COLOR.a ) : WHITE
                               
                               , controller, scene ) )
            {   var tooltip = GETTOOLTIPPEDCONTENT(MemType.Custom, null, controller);
                tooltip.text = "Drop object on me";
                Label( l, tooltip );
                /*
                var br = l;
                br.height = LINE_HEIGHT( controller.IS_MAIN , false );
                br.width = br.width / 5;
                // br.x += br.width * 4;
                
                br = adapter.bottomInterface.GET_CELL_RECT( l , l , MemType.Custom , 0 , 10 , 5 );
                DRAW_CATEGORY( br , controller , scene );*/
            }
            // GUI.color = cc;
            
            UpdateDragArea( line, controller );
        }
        
        
        
        
        
        
        
        
        
        
        
        
        void DRAW_CATEGORY( Rect buttonRect, BottomController controller, int scene )
        {   var idOffset = IDOFFSET(MemType.Custom);
            var HHH =   Math.Min( 24, buttonRect.height - 2);
            
            //                buttonRect_label.x += EditorGUIUtility.singleLineHeight;
            //               buttonRect_label.width -= EditorGUIUtility.singleLineHeight + HHH;
            
            /* var buttonRect_label = buttonRect;
             var colorR = buttonRect_label;
             colorR.x -= HHH;
             buttonRect_label.width -= HHH;
            colorR.x += colorR.width - 1;
            
             */
            var colorR = buttonRect;
            
            //                 var buttonRect_favIcon = buttonRect_label;
            //
            //                 buttonRect_favIcon.y += Mathf.RoundToInt( (buttonRect_favIcon.height - EditorGUIUtility.singleLineHeight) / 2 ) + 1;
            //                 buttonRect_favIcon.width = buttonRect_favIcon.height = EditorGUIUtility.singleLineHeight;
            //                 buttonRect_favIcon.x -= EditorGUIUtility.singleLineHeight;
            //
            
            colorR.width = HHH;
            colorR.y += (buttonRect.height - HHH) / 2;
            colorR.y = Mathf.RoundToInt( colorR.y );
            colorR.height = colorR.width;
            colorR.x += (buttonRect.width - HHH) / 2;
            
            var clamp = colorR.x;
            adapter.bottomInterface.GET_BOOKMARKS( ref list, scene );
            
            
            /* var inforR = colorR;
             inforR.x -= Mathf.RoundToInt( inforR.width * 1.2f );*/
            
            
            
            
            
            
            
            /*      faveContent.text = list[controller.GetCategoryIndex( scene )].name;
                  faveContent.tooltip = "Click - to change current category";
                  var ala = Adapter.GET_SKIN().label.alignment;
                  Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
                  GUI.Label( buttonRect_label , faveContent );
                  Adapter.GET_SKIN().label.alignment = ala;*/
            
            
            //                 if (BottomIconSwap)
            //                     GUI.DrawTexture( buttonRect_favIcon, adapter.GetIcon( "STAR" ) );
            //                 else
            //                     GUI.DrawTexture( buttonRect_favIcon, adapter.GetIcon( "FAV" ) );
            //
            //
            
            
            // var newC2 = IconData.PICKER(colorR, new GUIContent(), BG_COLOR, false);
            /* if (BG_COLOR != newC2)
             {
                 if (list[controller.GetCategoryIndex()].GET_COLOR() == null) adapter.bottomInterface.RowsParams[0].BgColorValue = newC2;
                 else list[controller.GetCategoryIndex()].SET_COLOR(newC2);
                 GUI.changed = true;
             }*/
            
            
            
            
            // if (!controller.IS_MAIN) GUI.Label( inforR, categoryDescriptionContent );
            //  EditorGUIUtility.AddCursorRect( colorR, MouseCursor.Link );
            
            
            
            
            
            DO_BUTTON( controller, scene, buttonRect, idOffset + 200, SET_BOOK_2 );
            
            Adapter.INTERNAL_BOX( Shrink( colorR, 4 ), "" );
            // if (!controller.IS_MAIN) Adapter. INTERNAL_BOX( inforR , "" );
            var BG_COLOR = list[controller.GetCategoryIndex(scene)].GET_COLOR() ?? adapter.bottomInterface.RowsParams[0].BgColorValue;
            Adapter.DrawRect( colorR, BG_COLOR );
            GUI.Label( colorR, categoryColorContent, adapter.STYLE_LABEL_8_middle );
            
            //  DO_BUTTON( controller , scene , buttonRect , idOffset + 200 , DO_BUTTON_CUSTOM );
            //  DO_BUTTON( controller, scene, colorR, idOffset + 201, DO_BUTTON_COLOR );
            //                 if (!controller.IS_MAIN)
            //                 {   clamp = inforR.x;
            //                     if (Event.current.type == EventType.Repaint && adapter.par.BottomParams.SHOW_ALL_DESCRIPTIONS_INWIN) Adapter.GET_SKIN().button.Draw( inforR, REALEMPTY_CONTENT, false, false, false, true );
            //                     DO_BUTTON( controller, scene, inforR, idOffset + 202, DO_BUTTON_DESCRIPTION );
            //                 }
            
            
            
            // buttonRect.width -= buttonRect.x + buttonRect.width - clamp;
            EditorGUIUtility.AddCursorRect( buttonRect, MouseCursor.Link );
        }
        
        
    }
}
}
