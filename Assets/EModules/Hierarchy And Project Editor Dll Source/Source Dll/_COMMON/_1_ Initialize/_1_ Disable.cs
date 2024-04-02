#if UNITY_EDITOR
    #define PROJECT
    #define HIERARCHY
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Profiling;
#if PROJECT
    using EModules.Project;
#endif
namespace EModules.EModulesInternal


{


internal partial class Adapter

{








    internal void RESET_MODULE()
    {   if (tempAdapterBlock) return;
    
        RestoreGUI();
        
        
        //par.ENABLE_ALL = false;
        // SavePrefs();
        if (resetcount > 4)
        {   tempAdapterBlock = true;
            resetcount = 0;
            OnDisablePlugin();
            RepaintAllViews();
            // InternalEditorUtility.RepaintAllViews();
            logProxy.LogWarning( "Apologize - " + pluginname + " plugin is temporarily stopped" );
            return;
        }
        
        modules = null;
        wasModulesInitialize = false;
        MOI.InitModules();
        
        InitializateAdapter( pluginname, pluginID );
        //Constructor();//
        if (MOI.M_Vertices != null) MOI.M_Vertices.Clear();
        
        if (Event.current != null)
        {   if (Event.current.type == EventType.Repaint) resetcount++;
        }
        else
        {   resetcount++;
        }
        OnDisablePlugin();
        // MonoBehaviour.print("RESET");
    }
    
    internal void OnDisablePlugin()
    {   hierarchy_windows = ((IList)strange_field.GetValue( null ));
        //  MonoBehaviour.print(hierarchy_windows.Count);
        for (int i = 0 ; i < hierarchy_windows.Count ; i++)
        {   var t2 = m_TreeView(hierarchy_windows[i] as EditorWindow);
            if (t2 == null) continue;
            /* if (t2 == null)
            {
            hierarchy_windows[i].GetType().GetMethod( "Init" , (BindingFlags)(-1) ).Invoke( hierarchy_windows[i],null );
            t2 = m_TreeView.GetValue( hierarchy_windows[i]  );
            }*/
            var g2 = guiProp.GetValue(t2, null);
            //k_BaseIndent.SetValue( g2, (bottomInterface.defaultextraInsertionMarkerIndent ?? 0) + 2 );
            m_UseHorizontalScroll.SetValue( g2, false );
            k_IndentWidth.SetValue( g2, 14 );
            k_IconWidth.SetValue(g2, EditorGUIUtility.singleLineHeight );
            k_LineHeight.SetValue( g2, EditorGUIUtility.singleLineHeight );
            foldoutYOffset.SetValue( g2, 0 );
            //Debug.Log( "ASD" );
            if (k_BottomRowMargin != null)
                k_BottomRowMargin.SetValue( g2, 0 );
                
            var ping = m_Ping.GetValue(g2);
            var style = m_PingStyle.GetValue(ping);
            if (style != null) fixedHeight.SetValue( style, EditorGUIUtility.singleLineHeight, null );
            
            var gostyle = s_GOStyles != null ? s_GOStyles.GetValue(null) : null;
            var getst = s_Style != null ? s_Style.GetValue(null) : null;
            // var gostyle = s_GOStyles.GetValue(null);
            if (gostyle != null)
            {   /* var sceneStyle = sceneHeaderBg.GetValue(gostyle);
                 fixedHeight.SetValue( sceneStyle, EditorGUIUtility.singleLineHeight, null );
                 alignment.SetValue( sceneStyle, TextAnchor.UpperLeft, null );*/
                
                foreach (var treestyle in treestyles)
                {   var st = treestyle.GetValue(gostyle);
                    fixedHeight.SetValue( st, EditorGUIUtility.singleLineHeight, null );
                }
            }
            if (lineStyle != null && getst != null)
            {   /* var st = lineStyle.GetValue(getst);
                 alignment.SetValue( st, TextAnchor.UpperLeft, null );
                 alignment.SetValue( lineBoldStyle.GetValue( getst ), TextAnchor.UpperLeft, null );*/
                //fixedHeight.SetValue(st, EditorGUIUtility.singleLineHeight, null);
            }
        }
        //  MonoBehaviour.print(hierarchy_windows.Count);
        
        foreach (var w in hierarchy_windows)
        {   bottomInterface.BOTTOM_UPDATE_POSITION( (EditorWindow)w );
        }
        if (window() != null) bottomInterface.BOTTOM_UPDATE_POSITION( window() );
    }
}
}
