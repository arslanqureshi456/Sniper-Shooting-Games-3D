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
    
    
    
    
        void DRAW_SEARCHWINDOW(float start_X, float wOffset, ref float outY)
        {
        
        
#pragma warning disable
            if (Adapter.LITE) return;
#pragma warning restore
            
            
            
            
            
            
            
            BEGIN_CATEGORY( ref caclCACHE[0], false, new Color32( 0, 0, 0, 40 ) );
            DrawHeader( "Search Box (Right Click)" );
            DRAW_WIKI_BUTTON( "Other", "Search Box" );
            
            
            
            DrawNew();
            var  R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            A.par.SEARCH_DOUBLECLICK_CLOSE = A.TOGGLE_LEFT( R, "Double-click - <b>Close</b> Search Window", A.par.SEARCH_DOUBLECLICK_CLOSE );
            TOOLTIP(R, "Double-clicking will close the unpinned search window when select objects, if it off then a single click will close the unpinned window search.");
            
            
            R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            A._S_searchBintToLeft = A.TOGGLE_LEFT( R, "Bind Search Window to <b>Left</b> by default",                A._S_searchBintToLeft );
            TOOLTIP(R,   "If enabled, by default, the window will be located at the left of hierarchy window, to hide the names of objects that may be distracting.");
            
            
            R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            A._S_searchWidthMulty = SLIDER( R, "Default <b>Width</b> multiply '1':", A._S_searchWidthMulty, 0.1f, 5 );
            TOOLTIP(R,   "you can increase the width of the window to your liking.");
            
            
            R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
            A.par.PIN_FILLTERWIN_BYDEFAULT = A.TOGGLE_LEFT( R, "Turn on Pin button by default",                A.par.PIN_FILLTERWIN_BYDEFAULT );
            TOOLTIP(R,   "The window will automatically hide when lost focus, if you click on the pin button at the top of the 'search window', window will not to be automatically hide.");
            
            
            
            HR();
            //Space( 20 );
            Space(EditorGUIUtility.singleLineHeight );
            
            
            // R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight * 3);
            // EditorGUI.TextArea(R, "Right-CLICK on the content of one of the found objects, to search among already found objects\nYou can drag and drop objects from this window");
            HelpBox( "Right-CLICK on the content of one of the found objects, to search among already found objects\nYou can drag and drop objects from this window", MessageType.None   );
            DRAW_HELP_TEXTURE( A.IS_HIERARCHY() ? "HELP_SEARCH" : "SETUP_SEARCH PROJECT", height : 100, yOffset : -40);
            HelpBox( "If you want to search within a parent object, select the parent object, hold the 'CONTROL', and 'Right-CLICK' the component", MessageType.None );
            DRAW_HELP_TEXTURE( "NEW_BOTTOM_SEARTCHHELP" );
            //   HelpBox( "You can drag and drop objects from this window", MessageType.Warning );
            HelpBox( "Escape to close", MessageType.None );
            
            
            
            END_CATEGORY( ref caclCACHE[0] );
            
            
            
            if (GetControlRect( 0 ).y > outY) outY = GetControlRect( 0 ).y;
            
        }//ADD_SeartchBox
        
        
        
        
        
    }
}
