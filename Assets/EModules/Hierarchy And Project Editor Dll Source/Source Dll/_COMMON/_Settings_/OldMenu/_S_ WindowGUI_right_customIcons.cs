using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PROJECT
    using EModules.Project;
#endif
//namespace EModules



namespace EModules.EModulesInternal

{
    internal partial class SETUPROOT
    { /*, ISerializable, IDeserializationCallback*/

        internal void TOOLTIP( Rect R , string text , bool bold = false ) {
            Adapter.TOOLTIP( R , text , bold );
        }


        void DRAW_CAT_CUSTOMICS( float start_X , float wOffset , ref float outY ) {   ////////////////////////icon params
                                                                                      //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            BEGIN_PADDING( 3 , 3 );

            var R = GET_OFFSETRECT(0, 0, 0);


            // if (Event.current.type == EventType.repaint && EditorGUIUtility.isProSkin) DrawTiled(new Rect(R.x, R.y, R.width + 7, (tileCalcCustomIcons ?? (R.y + 200)) - R.y), Hierarchy.GetIcon(ICONID.SETUP_NOISE), 171);
            var asdasdasd = GUI.color;
            GUI.color *= new Color32( 0 , 0 , 0 , 40 );
            //  if (Event.current.type == EventType.Repaint && EditorGUIUtility.isProSkin) GUI.DrawTexture( new Rect( R.x - 3, R.y, R.width + 3, (tileCalcCustomIcons ?? (R.y + 200)) - R.y ), Texture2D.whiteTexture );
            GUI.color = asdasdasd;

            DrawHeader( "ICONS (Module)" , true );
            DRAW_WIKI_BUTTON( "Right Panel" , "Components" );


            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            A.par.COMPONENTS_NEXT_TO_NAME = A.TOGGLE_LEFT( R , "Component's Icons Next To The Object Name:" , A.par.COMPONENTS_NEXT_TO_NAME );

            // DRAW_HELP_TEXTURE( A.par.COMPONENTS_NEXT_TO_NAME ? "COMPONENTNEXTTO" : "COMPONENTNEXTTO2", 100, /*Hierarchy.par.COMPONENTS_NEXT_TO_NAME*/ true );


            var og = GUI.enabled;
            GUI.enabled &= A.par.COMPONENTS_NEXT_TO_NAME;
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            var newPad = SLIDER(R, "Left Padding:", A.par.COMPONENTS_NEXT_TO_NAME_PADDING, -40, 100);
            if ( A.par.COMPONENTS_NEXT_TO_NAME_PADDING != newPad ) {
                A.par.COMPONENTS_NEXT_TO_NAME_PADDING = newPad;
            }
            GUI.enabled = og;


            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            var newSpace = SLIDER(R, "Space Between Icons:", A.par.ICONSPACE, -8, 8);
            if ( newSpace != A.par.ICONSPACE ) {
                A.par.ICONSPACE = newSpace;
                //  Hierarchy.RepaintWindow();
            }





            //  BEGIN_PADDING(20);
            /*  Space( 20 );
            
              HelpBox( "(Use ctrl+Left CLICK - Fast enable/disable components)", MessageType.Info );
            
              R = GetLastRect();
              //DrawTiled(new Rect(R.x - 3, R.y + R.height + 2, R.width + 8 + 3, 160), GetSetubBGTexture(), 64);
              DRAW_HELP_TEXTURE( "HELP_COMPONENTMENU" );*/
            HelpBox( "(Use Ctrl+LEFT DRAG to drag component to inspector, for example" , MessageType.None );
            END_PADDING();

            Space( 20 );

            //     if (Event.current.type == EventType.Repaint) tileCalcCustomIcons = GetLastRect().y + GetLastRect().height;


            // if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
            // Y = start_X;
            // X += wOffset;

            //  if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(GetLastRect(), EditorGUIUtility.ObjectContent(null, typeof(BoxCollider)).image);
            // R = DRAW_GREENLINE_ANDGETRECT(ref LAST_GREEN, true, false);
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            var LAST_GREEN = BEGIN_GREENLINE(R, true);
            DRAW_PRE_ICON( ref R , EditorGUIUtility.ObjectContent( null , typeof( BoxCollider ) ).image , true );
            var newD = A.TOGGLE_LEFT(R, "☰ Enable Unity <b>Default</b> Icons ☰", A.par.FD_Icons_default, defaultStyle: true);




            var wasEn2 = GUI.enabled;
            GUI.enabled &= A.par.FD_Icons_default;

            var RECT = GetLastRect();
            RECT.y += RECT.height;
            RECT.height = 75;

            R = GetControlRectAndOffset( EditorGUIUtility.singleLineHeight , 25 );
            LABEL( R , "<i>Hidden Icons:</i>" );

            GetControlRect( 50 );

            var list = Hierarchy_GUI.Instance(A).HiddenComponents.ToList();
            RECT = GetLastRect();
            //  RECT.y -= 10;
            RECT.x += 20;
            RECT.width = Math.Min( W + 10 , RECT.width ) - 20;
            scrollPos = GUI.BeginScrollView( RECT , scrollPos , new Rect( 0 , 0 , list.Count * 32 , 32 ) , true , false );
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            //Assembly asm = typeof(Image).Assembly;
            Adapter.INTERNAL_BOX( new Rect( 0 , 0 , RECT.width , 32 ) , "" );
            if ( list.Count == 0 ) Adapter.INTERNAL_BOX( new Rect( 0 , 0 , RECT.width , 32 ) , "Use the left click in the hierarchy on the icon to hide" );
            for ( int i = 0 ; i < list.Count ; i++ ) {
                RECT = new Rect( i * 32 , 0 , 32 , 32 );
                //print(list[i] + " " + asm.GetType(list[i]));
                Type target = null;
                foreach ( var assembly in asms ) {
                    target = assembly.GetType( list[i] );
                    if ( target != null ) break;
                }

                //  if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lastRect, Utilites.ObjectContent(null, asm.GetType(list[i])).image);
                var find = EditorGUIUtility.ObjectContent(null, target);
                if ( Event.current.type.Equals( EventType.Repaint ) && find.image != null ) GUI.DrawTexture( RECT , find.image );

                if ( !GUI.enabled ) Adapter.FadeRect( RECT , 0.7f );

                // if (!GUI.enabled) if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lastRect,Hierarchy.sec);
                RECT.x += RECT.width / 2;
                RECT.height = RECT.width = RECT.width / 2;
                if ( GUI.Button( RECT , "X" ) ) {
                    Hierarchy_GUI.Undo( A , "Restore Icon" );
                    Hierarchy_GUI.Instance( A ).HiddenComponents.RemoveAt( i );
                    Hierarchy_GUI.SetDirtyObject( A );
                }

            }
            GUI.EndScrollView();

            GUI.enabled = wasEn2;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////






            Space( 20 );



            /////////////////////////SPLIT ICONS
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(GetLastRect(), Hierarchy.GetIcon("MONO"));
            //  R = GET_OFFSETRECT(16);
            R = DRAW_GREENLINE_ANDGETRECT( ref LAST_GREEN , true , false );
            DRAW_PRE_ICON( ref R , "MONO" , true );
            // R = DRAW_GREENLINE_ANDGETRECT(ref LAST_GREEN, true, false);
            var newM = A.TOGGLE_LEFT(R, "☰ Enable <b>MonoBehaviour</b> Scripts Icons ☰", A.par.FD_Icons_mono, defaultStyle: true);

            var qwe = GUI.enabled;
            GUI.enabled &= newM;

            R = GetControlRectAndOffset( EditorGUIUtility.singleLineHeight , 25 );
            LABEL( R , "<i>Grouping Monobehaviour Icons:</i>" );

            R = GetControlRect( 46 );
            R.x += 20;
            R.width -= 20;
            Adapter.INTERNAL_BOX( R , "" );
            R.width /= 3;

            // GUILayout.BeginHorizontal(GUILayout.Height(46));
            // Space(40);
          /*  var B_normal = Adapter.GET_SKIN().button.normal.background;
            var B_active = Adapter.GET_SKIN().button.active.background;
            var B_focused = Adapter.GET_SKIN().button.focused.background;
            var B_hover = Adapter.GET_SKIN().button.hover.background;
            Adapter.GET_SKIN().button.normal.background = null;
            Adapter.GET_SKIN().button.hover.background = Adapter.GET_SKIN().button.focused.background = Adapter.GET_SKIN().button.active.background = A.GetIcon( "BUTBLUE" );
*/

            GUI.Label( R , "" );
            if ( A.par.COMPS_SplitMode == 0 ) GUI.DrawTexture( R , A.GetIcon( "BUTBLUE" ) );
            var lr = R;
            lr.height = 25;
            lr.y += 4 + EditorGUIUtility.singleLineHeight;
            GUI.DrawTexture( lr , A.GetIcon( "COMP_SPLIT1" ) , ScaleMode.ScaleToFit , true , 1 / 0.3f );
            EditorGUIUtility.AddCursorRect( lr , MouseCursor.Link );
            lr = R;
            lr.height = EditorGUIUtility.singleLineHeight;
            LABEL( lr , "All in <b>One</b>" );
            if ( GUI.Button( R , "" , A.STYLE_DEFBUTTON ) ) A.par.COMPS_SplitMode = 0;

            R.x += R.width;
            GUI.Label( R , "" );
            if ( A.par.COMPS_SplitMode == 1 ) GUI.DrawTexture( R , A.GetIcon( "BUTBLUE" ) );
            if ( GUI.Button( R , "" , A.STYLE_DEFBUTTON ) ) A.par.COMPS_SplitMode = 1;
            lr = R;
            lr.height = 25;
            lr.y += 4 + EditorGUIUtility.singleLineHeight;
            GUI.DrawTexture( lr , A.GetIcon( "COMP_SPLIT2" ) , ScaleMode.ScaleToFit , true , 1 / 0.3f );
            EditorGUIUtility.AddCursorRect( lr , MouseCursor.Link );
            lr = R;
            lr.height = EditorGUIUtility.singleLineHeight;
            //if (Event.current.type == EventType.repaint) EditorStyles.largeLabel.Draw(lr, new GUIContent("Merge\nEnable/Disable"), false, false, false, false);
            LABEL( lr , "<b>Two</b> <i>(On & Off)</i>" );


            R.x += R.width;
            GUI.Label( R , "" );
            if ( A.par.COMPS_SplitMode == 2 ) GUI.DrawTexture( R , A.GetIcon( "BUTBLUE" ) );
            if ( GUI.Button( R , "" , A.STYLE_DEFBUTTON ) ) A.par.COMPS_SplitMode = 2;
            lr = R;
            lr.height = 25;
            lr.y += 4 + EditorGUIUtility.singleLineHeight;
            GUI.DrawTexture( lr , A.GetIcon( "COMP_SPLIT3" ) , ScaleMode.ScaleToFit , true , 1 / 0.3f );
            EditorGUIUtility.AddCursorRect( lr , MouseCursor.Link );
            lr = R;
            lr.height = EditorGUIUtility.singleLineHeight;
            LABEL( lr , "Show <b>All</b>" );
           /* Adapter.GET_SKIN().button.normal.background = B_normal;
            Adapter.GET_SKIN().button.active.background = B_active;
            Adapter.GET_SKIN().button.focused.background = B_focused;
            Adapter.GET_SKIN().button.hover.background = B_hover;*/
            // Space(40);
            //  GUILayout.EndHorizontal();
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            GUI.enabled = qwe;


            Space( 20 );




            /*merge into one icon*/
            ///////////////////////////////////user icons
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //  if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(GetLastRect(), Hierarchy.GetIcon("MY"));
            // R = GET_OFFSETRECT(16);
            R = DRAW_GREENLINE_ANDGETRECT( ref LAST_GREEN , true , true );
            DRAW_PRE_ICON( ref R , "MY" , true );
            //R = DRAW_GREENLINE_ANDGETRECT(ref LAST_GREEN, true, false);
            var newU = A.TOGGLE_LEFT(R, "☰ Enable <b>User's</b> Icons ☰", A.par.FD_Icons_user, defaultStyle: true);

            if ( !newD && !newU && !newM ) {
                if ( A.par.FD_Icons_default || A.par.FD_Icons_user ) {
                    A.par.FD_Icons_user = !(A.par.FD_Icons_default = A.par.FD_Icons_user);
                    A.par.FD_Icons_mono = false;
                } else {
                    A.par.FD_Icons_default = true;
                    A.par.FD_Icons_mono = false;
                    A.par.FD_Icons_user = true;
                }
            } else {
                A.par.FD_Icons_default = newD;
                A.par.FD_Icons_mono = newM;
                A.par.FD_Icons_user = newU;
            }
            var on4 = GUI.enabled;
            GUI.enabled &= A.par.FD_Icons_user;

            Space( 3 );
            R = GetControlRectAndOffset( 0 , 25 );

            var boxRect = GetLastRect();
            //boxRect.x += 4;
            //   boxRect.y += boxRect.height;
            boxRect.height = CI.CusomIconsHeight + 12;


            /*       GUILayout.BeginHorizontal();
                   var P = 25;
                   GUILayout.Space(P);
                   GUILayout.Label("Assigned User Icons:");
                   var boxRect = new Rect(GetLastRect().x - (P - 14), GetLastRect().y, W + 12, CusomIconsHeight + 46);
                   GUILayout.EndHorizontal();*/
            //  var r = GetLastRect();
            //  print(r);
            //new Rect(0, r.y + r.height, W, CusomIconsHeight)

            Adapter.INTERNAL_BOX( boxRect , "" );

            /*
            R = GetControlRect( EditorGUIUtility.singleLineHeight, 40 );
            R.width /= 2;
            // GUILayout.BeginHorizontal(GUILayout.MaxWidth(W));
            // GUILayout.Space(40);
            var oldA = Adapter.GET_SKIN().label.alignment;
            Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
            GUI.Label( R, "MonoBehaviour Script" );
            Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleRight;
            R.x += R.width;
            GUI.Label( R, "Texture2D Icon" );
            Adapter.GET_SKIN().label.alignment = oldA;*/
            //  GUILayout.EndHorizontal();



            R = GetControlRect( CI.CusomIconsHeight );
            R.x += 7;
            R.y += 6;
            //  GUILayout.BeginHorizontal();
            //  GUILayout.Space(11);
            GUI.BeginScrollView( R , Vector2.zero , new Rect( 0 , 0 , R.width , DrawCustomIconsClass.IC_H * (customIcons.Count + 1) ) , false , false/*, GUILayout.Width(W), GUILayout.ExpandWidth(true)*/);

            CI.DrawCustomIcons( win , GetControlRect( EditorGUIUtility.singleLineHeight ) );
            GUI.EndScrollView();



            //  GUILayout.EndHorizontal();

            //     if (Event.current.type == EventType.repaint) MouseY = GetLastRect().position.y;


            Label( "" );
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if ( GetControlRect( 0 ).y > outY ) outY = GetControlRect( 0 ).y;

            GUI.enabled = on4;
        }
    }
}
