using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PROJECT
    using EModules.Project;
#endif



namespace EModules.EModulesInternal

{




internal partial class SETUPROOT {







    void ADDITIONAL_FEATURES(float start_X, float wOffset, ref float outY)
    {
    
    
    
    
    
        /*  BEGIN_CATEGORY(ref calHELP, false);
          Space(10);
          HelpBox("Hierarchy interface has three built-in panels", MessageType.None);
          DRAW_HELP_TEXTURE("HELP_PANELS", 127, GUI.enabled, 0);
          END_CATEGORY(ref calHELP);*/
        
        
        
        
        
        
        
        
        
        
        
        
        if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
        Y = start_X;
        X += wOffset;
        
        if (A.IS_HIERARCHY())
        {   //  Space(10);
        
        
            BEGIN_CATEGORY( ref calcPLAY );
            DrawHeader( "Play Mode" );
            // DrawNew();
            
            var enn = GUI.enabled;
            
            R = GET_OFFSETRECT( EditorGUIUtility.singleLineHeight );
            var lastr = BEGIN_GREENLINE(R, !A.par.PLAYMODE_HideRightPanel);
            // if (Event.current.type == EventType.repaint) SETUP_GREENLINE.Draw(new Rect(R.x + 8, R.y + R.height / 2, 4, R.height), false, false, false, false);
            A.par.PLAYMODE_HideRightPanel = !A.TOGGLE_LEFT( R, "Show 'Right Panel' in PlayMode", !A.par.PLAYMODE_HideRightPanel );
            GUI.enabled &= !A.par.PLAYMODE_HideRightPanel;
            // DrawNew();
            R = DRAW_GREENLINE_ANDGETRECT( ref lastr, true, !A.par.PLAYMODE_HideRightPanel && A.par.PLAYMODE_HideComponents2 );
            A.par.PLAYMODE_HideComponents2 = !A.TOGGLE_LEFT( R, "Show 'Components Module' in PlayMode", !A.par.PLAYMODE_HideComponents2 );
            BEGIN_PADDING( 20 );
            HelpBox( "( Baked components work fine but you can hide components module to improve performance )", MessageType.None );
            END_PADDING();
            GUI.enabled &= !A.par.PLAYMODE_HideComponents2;
            
            R = DRAW_GREENLINE_ANDGETRECT( ref lastr, !A.par.PLAYMODE_HideComponents2, true );
            A.par.PLAYMODE_UseBakedComponents = A.TOGGLE_LEFT( R, "Use 'Baked Components' in PlayMode", A.par.PLAYMODE_UseBakedComponents );
            HelpBox( "( Using baked components improves performance in PlayMode )", MessageType.None );
            Space( 15 );
            
            
            GUI.enabled = enn;
            
            /*  A.par.FIX_IMGUI_Controls = TOGGLE_LEFT("Fix Strange Exeption 'UnityEditor.IMGUI.Controls'", A.par.FIX_IMGUI_Controls);
              R = GET_OFFSETRECT(106);
              EditorGUI.HelpBox(R, "Beginning with the new 2017 version of the Unity, during the starting or stopping of the game, sometimes strange exception appeared:" +
                      "'objectController.IsItemDragSelectedOrSelected' " +
            ".\nTurn on this option if you get this exception. This option manipulatin the scroll position in the " +
            "hierarchy window if playmode changing.\n" +
                      "Perhaps in some new version of the unity we can avoid it.", MessageType.Info);*/
            
            END_CATEGORY( ref calcPLAY );
            Space( 20 );
            
            
        }
        
        
        
        
        
        
        
        
        
    }
    
    
    
    
    
}
}
