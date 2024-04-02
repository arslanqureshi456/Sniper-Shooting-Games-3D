using System;
using System.Collections.Generic;
using System.IO;
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






        float? tileCalcKeeperIcons;








        int DRAW_PLAYMODEKEEPER( int I , float start_X , float wOffset , ref float outY ) {
            var R = GET_OFFSETRECT(0, 0, 0);
            /*  if (Event.current.type == EventType.repaint && EditorGUIUtility.isProSkin)
                  DrawTiled(new Rect(R.x, R.y, R.width + 7, (DRAW__CATEGSCalcAttribute ?? (R.y + 200)) - R.y), Hierarchy.GetIcon(ICONID.SETUP_NOISE), 171);*/
            //BEGIN_PADDING(5);
            {

                /*  DrawHeader("Functions and Vars displaying:");
                  DRAW_WIKI_BUTTON("Right Panel", "Components");



                  // DrawHeader("'SHOW_IN_HIER' attribute:");
                  // DrawNew(32);
                  HelpBox("Display of variables and functions in the hierarchy window", MessageType.None);
                  DrawNew();
                  R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
                  Hierarchy.par.DISPLAYING_NULLSISRED = TOGGLE_LEFT(R, "Red color for unassigned variables", Hierarchy.par.DISPLAYING_NULLSISRED);

                  HelpBox("Use '[SHOW_IN_HIER]' attribute in your code", MessageType.Info);

                  DRAW_HELP_TEXTURE("HELP_ATTRIBUTES", 47, true);
                  //END_PADDING();

                  //  DrawNew(20);
                  R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
                  var LAST_GREEN = BEGIN_GREENLINE(R, true);
                  Hierarchy.par.COMP_ATTRIBUTES_BUTTONS = TOGGLE_LEFT(R, "Enable display of functions", Hierarchy.par.COMP_ATTRIBUTES_BUTTONS);
                  //  DrawNew(20);
                  R = DRAW_GREENLINE_ANDGETRECT(ref LAST_GREEN, true, true);
                  Hierarchy.par.COMP_ATTRIBUTES_FIELDS = TOGGLE_LEFT(R, "Enable display of variables", Hierarchy.par.COMP_ATTRIBUTES_FIELDS);


                  //  BEGIN_PADDING(20);
                  HelpBox("Here's a simple example of using the attribute declared", MessageType.None);
                  DrawNew(47);
                  DRAW_HELP_TEXTURE("HELP_ATTRIBUTES_VISUAL", 47, true);
                  HelpBox("There's Code Example inside 'FunctionsDisplaying_Example.cs", MessageType.Info);

                  Space(10);*/






                // Space(30);






                /*  Space(20);*/

                ///   BEGIN_CATEGORY(ref calcRIGHT[I], true, /*new Color32(0, 0, 0, 40)*/null, 5);

                R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
                var last_rect = BEGIN_GREENLINE(R, A.par.DataKeeperParams.ENABLE);
                A.par.DataKeeperParams.ENABLE = A.TOGGLE_LEFT( R , "☰ Enable <b>PlayMode Data Keeper</b> Module ☰" , A.par.DataKeeperParams.ENABLE , defaultStyle: true );
                BEGIN_PADDING( 20 );
                // DRAW_HELP_TEXTURE("HELP_FIXED_KEEPER");
                HelpBox( "Turn off this category to disable persisting for all objects" , MessageType.None );
                var e = GUI.enabled;
                GUI.enabled &= A.par.DataKeeperParams.ENABLE;
                END_PADDING();
                DRAW_KEEPER( last_rect );
                GUI.enabled = e;

                ///  END_CATEGORY(ref calcRIGHT[I++]);



                /*
                                Space(20);

                                BEGIN_CATEGORY(ref calcRIGHT[I], true, /*new Color32(0, 0, 0, 40)#1#null, 5);

                               // DrawNew(40);
                                DrawHeader("Fixed 'Presets Manager'");
                                DRAW_WIKI_BUTTON("Right Panel", "Presets Manager");
                                R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);

                                END_CATEGORY(ref calcRIGHT[I++]);*/
                //  Space(15);

                if ( GetControlRect( 0 ).y > outY ) outY = GetControlRect( 0 ).y;


                // if (Event.current.type == EventType.repaint) DRAW__CATEGSCalcAttribute = GetLastRect().y + GetLastRect().height;
                //R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            }
            // END_PADDING();
            return I;
        }





        void DRAW_KEEPER( Vector2 LAST_RECT ) {
            var R = new Rect();
            BEGIN_PADDING( 20 );

            var asdasdasd = GUI.color;
            GUI.color *= new Color32( 0 , 0 , 0 , 40 );
            if ( Event.current.type == EventType.Repaint ) GUI.DrawTexture( new Rect( R.x - 3 , R.y , R.width + 7 + 4 , (tileCalcKeeperIcons ?? (R.y + 200)) - R.y ) , Texture2D.whiteTexture );
            GUI.color = asdasdasd;


            //  if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(GetLastRect(), Hierarchy.GetIcon("MY"));
            END_PADDING();


            Space( EditorGUIUtility.singleLineHeight );
            R = DRAW_GREENLINE_ANDGETRECT( ref LAST_RECT , A.par.DataKeeperParams.USE_SCRIPTS , false );
            A.par.DataKeeperParams.SAVE_UNITYOBJECT = A.TOGGLE_LEFT( R , "Save Variables typeof UnityEngine.Object" , A.par.DataKeeperParams.SAVE_UNITYOBJECT );
            TOOLTIP( R ,
                    @"Classes references, such as MonoBehaviour or other Unity Components, needs enabled special serialization,
 this feature allows you to save references to all serialized Classes inherited from Unity.Object." );
            //  DrawNew();
            R = DRAW_GREENLINE_ANDGETRECT( ref LAST_RECT , A.par.DataKeeperParams.USE_SCRIPTS , false );
            A.par.DataKeeperParams.SAVE_ENABLINGDISABLING_GAMEOBJEST = A.TOGGLE_LEFT( R , "Save Active State for GameObjects" , A.par.DataKeeperParams.SAVE_ENABLINGDISABLING_GAMEOBJEST );
            TOOLTIP( R , "if any script changes the active state in PlayMode, the changes will be saved after stopping also PlayMode" );

            //  DrawNew();
            /*  R = DRAW_GREENLINE_ANDGETRECT( ref LAST_RECT , A.par.DataKeeperParams.USE_SCRIPTS , false );
              A.par.DataKeeperParams.SAVE_GAMEOBJET_HIERARCHY = A.TOGGLE_LEFT( R , "Save position in hierarchy (Experimental)" , A.par.DataKeeperParams.SAVE_GAMEOBJET_HIERARCHY );*/
            //  DrawNew();
            R = DRAW_GREENLINE_ANDGETRECT( ref LAST_RECT , A.par.DataKeeperParams.USE_SCRIPTS , false );
            A.par.DataKeeperParams.USE_ADD_REMOVE = A.TOGGLE_LEFT( R , "Save Added/Removed components (Experimental)" , A.par.DataKeeperParams.USE_ADD_REMOVE );
            TOOLTIP( R , "if you delete or add the components selected for persisting, the changes will be saved after stopping also PlayMode" );


            // BEGIN_PADDING( 20 );
            // END_PADDING();

            Space( EditorGUIUtility.singleLineHeight );

            R = DRAW_GREENLINE_ANDGETRECT( ref LAST_RECT , A.par.DataKeeperParams.ENABLE , !A.par.DataKeeperParams.USE_SCRIPTS );
            //  R = GET_OFFSETRECT(16);
            A.par.DataKeeperParams.USE_SCRIPTS = A.TOGGLE_LEFT( R , "☰ Enable Keeper for scripts: ☰" , A.par.DataKeeperParams.USE_SCRIPTS , defaultStyle: true );

            // BEGIN_PADDING( 20 );
            //  HelpBox( "Selected scripts will be automatically added to 'PlayMode Data Keeper' after play mode start", MessageType.Info );



            var e = GUI.enabled;
            GUI.enabled &= A.par.DataKeeperParams.USE_SCRIPTS;



            // R = GetControlRectAndOffset(25, 25);


            //END_PADDING();


            //  R = DRAW_GREENLINE_ANDGETRECT(ref LAST_RECT, Hierarchy.par.DataKeeperParams.USE_SCRIPTS, true);
            var boxRect = DRAW_GREENLINE_ANDGETRECT( ref LAST_RECT, A.par.DataKeeperParams.USE_SCRIPTS, true );
            boxRect.x += 4;
            //  boxRect.y += boxRect.height;
            boxRect.height = PKC.KEEPER_HEIGHT + 16;

            R = boxRect;
            R.height = 25;
            R.y += 10;
            // GUI.Label(R, "Assigned User Auto Scripts:");
            //  GUI.Label( R, "Use Auto Keeper for following scripts:" );


            /*       GUILayout.BeginHorizontal();
                   var P = 25;
                   GUILayout.Space(P);
                   GUILayout.Label("Assigned User Icons:");
                   var boxRect = new Rect(GetLastRect().x - (P - 14), GetLastRect().y, W + 12, KEEPER_HEIGHT + 46);
                   GUILayout.EndHorizontal();*/
            //  var r = GetLastRect();
            //  print(r);
            //new Rect(0, r.y + r.height, W, KEEPER_HEIGHT)


            boxRect.width -= 10;
            Adapter.INTERNAL_BOX( boxRect , "" );

            /*
            R = GetControlRect( EditorGUIUtility.singleLineHeight );
            R.width /= 2;
            // GUILayout.BeginHorizontal(GUILayout.MaxWidth(W));
            // GUILayout.Space(40);
            var oldA = Adapter.GET_SKIN().label.alignment;
            Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;
            GUI.Label( R, "MonoBehaviour Script" );
            Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleRight;
            // R.x += R.width;
            //  GUI.Label(R, "Texture2D Icon");
            Adapter.GET_SKIN().label.alignment = oldA;
            //  GUILayout.EndHorizontal();*/


            Space( 10 );
            R = GetControlRect( PKC.KEEPER_HEIGHT );
            R = SHRINK( boxRect , 8 );
            //  GUILayout.BeginHorizontal();
            //  GUILayout.Space(11);
            // GUI.BeginScrollView( R, Vector2.zero, new Rect( 0, 0, R.width, H * (GET_ARRAY().Count + 1) ), false, false/*, GUILayout.Width(W), GUILayout.ExpandWidth(true)*/);
            PKC.DRAW_KEEPER_SCRIPTS( win , R );
            // GUI.EndScrollView();
            //  GUILayout.EndHorizontal();

            //     if (Event.current.type == EventType.repaint) KEEPER_MOUSE_Y = GetLastRect().position.y;

            GUI.enabled = e;


            // Label("");
            //  HelpBox( "After you stop the game, you can undo the persist from some or from all objects", MessageType.Info );
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if ( Event.current.type == EventType.Repaint ) tileCalcKeeperIcons = GetLastRect().y + GetLastRect().height;

        }


        DRAW_KEEPER_SCRIPTS_Class __PKC;
        internal DRAW_KEEPER_SCRIPTS_Class PKC {
            get {
                var res = __PKC ?? (__PKC = new DRAW_KEEPER_SCRIPTS_Class( ));
                res.A = A;
                return res;
            }
        }
        public class DRAW_KEEPER_SCRIPTS_Class
        {
            public float KEEPER_HEIGHT { get { return GET_ARRAY().Count * IC_H + IC_H; } }
            GUIContent KEEPER_PlusContent = new GUIContent()
        {
                text = "+",
                tooltip = "Drag MonoBehaviour Script "
            };
            GUIContent KEEPER_PlusContentEmpty = new GUIContent()
        {
                text = "",
                tooltip = "Drag MonoBehaviour Script"
            };
            public const float IC_H = 36;
            public   float MouseY = -1;
            EditorWindow win;
            List<Hierarchy_GUI.DataKeeperValue> GET_ARRAY() {
                return Hierarchy_GUI.Instance( A ).m_DataKeeper_Values;
            }
            public Adapter A;
            public void DRAW_KEEPER_SCRIPTS( EditorWindow win , Rect lr ) {
                this.win = win;

                A.ChangeGUI();
                int i;
                // content.tooltip = "User Icons";
                // GUILayout.Label("");
                //  var lr = GetControlRect( EditorGUIUtility.singleLineHeight );
                Adapter.INTERNAL_BOX( new Rect( lr.x , lr.y , lr.width , lr.height ) , KEEPER_PlusContentEmpty );

                KEEPER_MOUSE_Y = EditorGUIUtility.GUIToScreenPoint( Vector2.zero ).y;
                /*if (dragContent)
                {
                    Adapter. INTERNAL_BOX(new Rect(0, 0, W, KEEPER_HEIGHT), content);
                }*/
                //    ExampleDragDropGUI(new Rect(0, 0, W, KEEPER_HEIGHT), null);

                if ( KEEPER_UPDATE_currentY.Length != GET_ARRAY().Count ) {
                    KEEPER_UPDATE_currentY = new float[GET_ARRAY().Count];
                    for ( i = 0 ; i < GET_ARRAY().Count ; i++ )
                        KEEPER_UPDATE_currentY[i] = i * IC_H;
                }

                // var lineRect = new Rect(0, 0, W, H);
                for ( i = 0 ; i < GET_ARRAY().Count ; i++ ) {

                    // var customIcon = GET_ARRAT()[i];
                    var r = new Rect( lr.x, lr.y + KEEPER_UPDATE_currentY[i], lr.width, IC_H );

                    if ( dragIndex == i ) {
                        r.x = Event.current.mousePosition.x - IC_H / 2;
                        r.y = Event.current.mousePosition.y - IC_H / 2;
                    }

                    // GUI.BeginClip(r);
                    DRAW_KEEPER_LINE( i , lr , r.x , r.y );
                    // GUI.EndClip();
                    // lineRect.y += H;
                }


                var lineRect = new Rect( lr.x, lr.y + GET_ARRAY().Count * IC_H, lr.width, IC_H );
                SETUPROOT.ExampleDragDropGUI( A , lineRect , null , SETUPROOT.DRAG_VALIDATOR_ONLYMONO , KEEREPR_DRAG_PERFORM );
                /*  if (lineRect.Contains(Event.current.mousePosition))
                  {
                      if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lineRect,Hierarchy.sec);
                  }*/
                var olds = A.button.fontSize;
                A.button.fontSize = 20;
                var butres = GUI.Button( lineRect , KEEPER_PlusContent, A.button );
                A.button.fontSize = olds;

                if ( butres ) {
                    if ( Event.current.button == 0 ) KEEPER_CREATE_LINE( null , int.MaxValue );
                }
                A.RestoreGUI();



            }
            void KEEREPR_DRAG_PERFORM() {
                KEEPER_CREATE_LINE( DragAndDrop.objectReferences[0] as MonoScript , int.MaxValue );
            }
            int dragIndex = -1;

            void DRAW_KEEPER_LINE( int i , Rect lr , float xOffset , float yOffset ) {   // ExampleDragDropGUI(lineRect, new CustomDragData() { index = i });

                MonoScript script = GET_ARRAY()[i].value;
                /*  if (!string.IsNullOrEmpty(GET_ARRAY()[i].Key))
                  {
                      var scriptPath = AssetDatabase.GUIDToAssetPath(GET_ARRAY()[i].Key);
                      if (!string.IsNullOrEmpty(scriptPath)) script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
                  }*/


                var r = new Rect( 0, 0, IC_H, IC_H );

                r.x += xOffset;
                r.y += yOffset;

                var oldsl = A.label.fontSize;
                A.label.fontSize = 16;
                //GUI.Label(r, "■");
                //  if (GUI.Button(r, "▲"))
                GUI.Label( r , "=" , A.label );
                A.label.fontSize = oldsl;

                if ( r.Contains( Event.current.mousePosition ) ) win.Repaint();

                if ( r.Contains( Event.current.mousePosition ) && Event.current.type == EventType.MouseDown ) {
                    if ( Event.current.button == 0 ) {
                        dragIndex = i;
                        /*InternalEditorUtility.repa*/
                        win.Repaint();

                        A.RepaintWindowInUpdate();
                    }
                }
                EditorGUIUtility.AddCursorRect( r , MouseCursor.Link );
                /* if (dragContent)*/

                // ExampleDragDropGUI(r, new CustomDragData() { index = i });

                A.RestoreGUI();

                r.Set( r.width + 10 , (r.height - EditorGUIUtility.singleLineHeight) / 2 , lr.width - IC_H * 2 , EditorGUIUtility.singleLineHeight );

                r.x += xOffset;
                r.y += yOffset;





                MonoScript newScript = script;
                try {
                    newScript = (MonoScript)EditorGUI.ObjectField( r , script , typeof( MonoScript ) , false );
                } catch {
                    newScript = script;
                }



                A.ChangeGUI();

                if ( newScript != script ) {   //var v = GET_ARRAY()[i];
                                               // v.Key = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(newScript));
                                               // GET_ARRAY()[i] = v;
                    Hierarchy_GUI.Instance( A ).DataKeeper_SetScript( i , newScript );

                    A.RepaintWindowInUpdate();
                    // Hierarchy.RepaintAllView();
                }


                r.Set( lr.width - IC_H + 5 , 0 , IC_H - 10 , IC_H );
                r.x += xOffset;
                r.y += yOffset;
                if ( GUI.Button( r , "X" , A.button) ) {
                    if ( Event.current.button == 0 ) {
                        KEEPER_REMOVE_LINE( i );
                        //CreateLine(null, null, int.MaxValue);
                    }
                    //dragContent = true;
                }
            }
            float KEEPER_MOUSE_Y;
            float[] KEEPER_UPDATE_currentY = new float[0];

            public void KEEPER_UPDATE( EditorWindow win ) {
                if ( Event.current.type == EventType.Repaint && KEEPER_UPDATE_currentY.Length != 0 ) {

                    var tempDragIndex = dragIndex == -1 ? -1 : Mathf.Clamp( Mathf.RoundToInt( (EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition ).y - KEEPER_MOUSE_Y - IC_H / 2) / (float)IC_H ), 0,
                                     KEEPER_UPDATE_currentY.Length - 1 );


                    for ( int i = 0, sib = 0 ; i < KEEPER_UPDATE_currentY.Length ; i++, sib++ ) {   // if (tempDragIndex == i && i > dragIndex) sib--;
                                                                                                    //if (tempDragIndex == i && i < dragIndex) sib++;
                        if ( dragIndex != -1 && i > dragIndex && i <= tempDragIndex ) sib = i - 1;
                        else if ( dragIndex != -1 && i < dragIndex && i >= tempDragIndex ) sib = i + 1;
                        else sib = i;
                        KEEPER_UPDATE_currentY[i] = Mathf.Lerp( KEEPER_UPDATE_currentY[i] , sib * IC_H , 0.5f );
                    }
                    //print(tempDragIndex);
                    if ( dragIndex != -1 ) {
                        win.Repaint();
                        //Hierarchy.RepaintAllView();
                    }
                }

                if ( Event.current.rawType == EventType.MouseUp ) {
                    var tempDragIndex = dragIndex == -1 ? -1 : Mathf.Clamp( Mathf.RoundToInt( (EditorGUIUtility.GUIToScreenPoint( Event.current.mousePosition ).y - KEEPER_MOUSE_Y - IC_H / 2) / (float)IC_H ), 0,
                                    KEEPER_UPDATE_currentY.Length - 1 );
                    if ( dragIndex != -1 && tempDragIndex != -1 && tempDragIndex != dragIndex ) {
                        KEEPER_SWITCH_POSES( dragIndex , tempDragIndex );
                        KEEPER_UPDATE_currentY = new float[0];
                        A.RepaintWindowInUpdate();
                        win.Repaint();
                        // Hierarchy.RepaintWindow();
                    }
                    dragIndex = -1;
                }
            }


            void KEEPER_SWITCH_POSES( int i1 , int i2 ) {
                Hierarchy_GUI.Undo( A , "Change PlayMode Data Keeper" );


                // var min = Math.Min(i1, i2);
                // var max = Math.Max(i1, i2);
                var v1 = GET_ARRAY()[i1].value;
                Hierarchy_GUI.Instance( A ).DataKeeper_RemoveAt( i1 );

                if ( i2 >= GET_ARRAY().Count ) Hierarchy_GUI.Instance( A ).DataKeeper_AddScript( v1 );
                else Hierarchy_GUI.Instance( A ).DataKeeper_InsertScript( i2 , v1 );

                Hierarchy_GUI.SetDirtyObject( A );
                A.RepaintWindowInUpdate();
                // Hierarchy.RepaintAllView();



            }

            void KEEPER_REMOVE_LINE( int index ) {
                if ( index < 0 || index >= GET_ARRAY().Count ) return;
                Hierarchy_GUI.Undo( A , "Change PlayMode Data Keeper" );

                Hierarchy_GUI.Instance( A ).DataKeeper_RemoveAt( index );
                Hierarchy_GUI.SetDirtyObject( A );
                A.RepaintWindowInUpdate();
                //  Hierarchy.RepaintAllView();
            }

            void KEEPER_CREATE_LINE( MonoScript script , int index ) {   //  string key = null;
                                                                         //Hierarchy_GUI.CustomIconParams value = new Hierarchy_GUI.CustomIconParams();
                                                                         // if (script != null) key = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(script));

                //   var init = Hierarchy_GUI.Initialize();
                Hierarchy_GUI.Undo( A , "Change PlayMode Data Keeper" );
                if ( index >= GET_ARRAY().Count ) Hierarchy_GUI.Instance( A ).DataKeeper_AddScript( script );
                else Hierarchy_GUI.Instance( A ).DataKeeper_InsertScript( index , script );
                Hierarchy_GUI.SetDirtyObject( A );
                A.RepaintWindowInUpdate();
                // Hierarchy.RepaintAllView();
            }
        }
    }
}
