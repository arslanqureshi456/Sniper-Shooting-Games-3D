using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using EModules.EModulesInternal;
//namespace EModules

namespace EModules.EProjectInternal {
internal partial class Project {


    internal class M_UserModulesRoot : Adapter.Module {
    
    
        internal override bool enableOverride()
        {   return !Adapter.LITE && Assigned && adapter.par.USE_CUSTOMMODULES;
        }
        internal override string enableOverrideMessage() { return Adapter.LITE ? " - Not Assigned (Pro Only)" : adapter.par.USE_CUSTOMMODULES ? " - Not Assigned" : " - Disabled in settings"; }
        
        public M_UserModulesRoot( int restWidth, int sibbildPos, bool enable, Adapter adapter ) : base( restWidth, sibbildPos, enable, adapter )
        {   CustomModule.m_OpenIntInput = SHOW_INT;
            CustomModule.m_OpenStringInput = SHOW_STRING;
            CustomModule.m_OpenDropDownMenu = SHOW_DROPDOWN;
        }
        
        CustomModule customModule;
        internal bool Assigned;
        internal string HeaderText_pref;
        internal string ContextHelper_pref;
        Rect? stateForDrag_B0;
        
        
        
        internal override object SetCustomModule( object _customModule )
        {   var customModule = _customModule as CustomModule;
            Assigned = customModule != null;
            this.customModule = customModule;
            if ( Assigned )
            {   HeaderText = "[C]" + customModule.NameOfModule;
                ContextHelper = ContextHelper_pref + " - " + customModule.NameOfModule;
            }
            else
            {   HeaderText = HeaderText_pref;
                ContextHelper = ContextHelper_pref;
            }
            
            return this;
        }
        
        
        Adapter.HierarchyObject D_o;
        string fillter;
        
        void RawOnUP()
        {   if ( stateForDrag_B0.HasValue && stateForDrag_B0.Value.Contains( Event.current.mousePosition ) && D_o != null && D_o .Validate())
            {   Adapter.EventUse();
                //Debug.Log( customModule.ToString( o.project.assetPath, o.project.guid, o.id, o.project.IsFolder ) );
                var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                
                _W__SearchWindow.Init( mp, HeaderText + " '" + (customModule.ToString( D_o.project.assetPath, D_o.project.guid, D_o.id, D_o.project.IsFolder, D_o.project.IsMainAsset ) ?? "-") + "'",
                                       (customModule.ToString( D_o.project.guid, D_o.project.assetPath, D_o.id, D_o.project.IsFolder, D_o.project.IsMainAsset ) ?? "-"),
                                       !string.IsNullOrEmpty( fillter ) ?
                                       CallHeaderFiltered( fillter ) :
                                       CallHeader(),
                                       this, adapter, D_o );
            }
            stateForDrag_B0 = null;
        }
        
        internal override float Draw( Rect drawRect, Adapter.HierarchyObject o )
        {
        
            if ( !Assigned ) return width;
            
            if ( Event.current.rawType == EventType.MouseUp )
            {   RawOnUP();
            }
            
            //  Undo.RecordObject(o, "GameObject SetActive");
            
            if ( stateForDrag_B0.HasValue && stateForDrag_B0.Value.Contains( Event.current.mousePosition ) )
            {   if ( EditorGUIUtility.isProSkin ) GUI.DrawTexture( stateForDrag_B0.Value, adapter.STYLE_DEFBUTTON.active.background );
                else GUI.DrawTexture( stateForDrag_B0.Value, Texture2D.whiteTexture );
            }
            
            if ( Event.current.type == EventType.MouseDown && Event.current.button == 1 && drawRect.Contains( Event.current.mousePosition ) )
            {   if ( stateForDrag_B0.HasValue )
                {   fillter = customModule.ToString( o.project.assetPath, o.project.guid, o.id, o.project.IsFolder, o.project.IsMainAsset );
                    stateForDrag_B0 = drawRect;
                    D_o = o;
                    adapter.PUSH_ONMOUSEUP( RawOnUP );
                }
                
                Adapter.EventUse();
            }
            
            
            
            
            
            
            /* if (tR.Contains(Event.current.mousePosition))
             {
            
                 if (tR.Contains(Event.current.mousePosition) && Event.current.type == EventType.mouseDown)
                 {
                     //EditorUtility.SetObjectEnabled( markedObjects[ instanceID ], !markedObjects[ instanceID ].activeInHierarchy );
                     /*   if (objectIsHiddenAndLock)
                        {
                            // o.SetActive(!o.activeSelf);
                        }
                        else#1#
            
                     if (Event.current.button == 0)
                     {
                         var targetO = new[] { o };
                         var sel = SELECTED_GAMEOBJECTS();
                         if (sel.Contains(o) /*&& Event.current.control#1#)
                         {
                             // targetO = sel.Where(g => g.GetComponentsInParent<Transform>(true).Count(p => sel.Contains(p.gameObject)) == 1).Select(g => g.gameObject).ToArray();
                             targetO = Utilities.GetOnlyTopObjects(sel);
                             /*  for (int i = 0; i < targetArray.Count; i++)
                               {
            
                               } #1#
                         }
                     }
                 }
             }*/
            
            try
            {   //              var b = Adapter.GET_SKIN().button.normal.textColor;
                //                 var a = Adapter.GET_SKIN().button.alignment;
                //                 Adapter.GET_SKIN().button.normal.textColor = Adapter.GET_SKIN().label.normal.textColor;
                //                 Adapter.GET_SKIN().button.alignment = TextAnchor.MiddleLeft;
                
                if ( HierarchyExtensions.Styles.button == null )
                {   HierarchyExtensions.Styles.button = new GUIStyle( adapter.STYLE_DEFBUTTON );
                    HierarchyExtensions.Styles.button.alignment = TextAnchor.MiddleLeft;
                    HierarchyExtensions.Styles.button.clipping = TextClipping.Clip;
                }
                HierarchyExtensions.Styles.button.normal.textColor = adapter.button.normal.textColor;
                //customModule.Draw( drawRect , o.project.assetPath , o.project.guid , o.id , o.project.IsFolder , o.project.IsMainAsset );
                GUI.BeginClip( drawRect );
                drawRect.x = 0;
                drawRect.y = 0;
                if ( Event.current.button != 1 || !Event.current.isMouse )
                {   try
                    {   customModule.Draw( drawRect, o.project.assetPath, o.project.guid, o.id, o.project.IsFolder, o.project.IsMainAsset );
                    }
                    catch ( Exception ex )      // adapter.logProxy.LogWarning( "CustomModule: " + ex.Message + " " + ex.StackTrace );
                    {   Debug.LogWarning( "CustomModule: " + ex.Message + " " + ex.StackTrace );
                    }
                }
                GUI.EndClip();
                
                
                
                
                //                 Adapter.GET_SKIN().button.normal.textColor = b;
                //                 Adapter.GET_SKIN().button.alignment = a;
            }
            catch ( Exception ex )
            {   adapter.logProxy.LogError( "CustomModule: " + ex.Message + " " + ex.StackTrace );
            }
            
            
            
            
            
            return width;
        }
        
        
        
        
        static void SHOW_INT( int i, Action<int> action )
        {   if ( Event.current == null )
            {   LOGERROR();
                return;
            }
            
            Action<string> convertAction = ( str ) =>
            {   int reslt;
                if ( int.TryParse( str, out reslt ) ) action( reslt );
            };
            var adapter = Initializator.AdaptersByName[Initializator.PROJECT_NAME];
            
            var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_190_68, true, adapter);
            
            //   var pos = InputData.WidnwoRect(EModulesInternal.FocusRoot.WidnwoRectType.Clamp, Event.current.mousePosition, 190, 68, adapter );
            _W__InputWindow.InitTeger( pos, "New Value", adapter, convertAction, null, i.ToString() );
            
        }/** SHOW_INT */
        
        static void SHOW_STRING( string s, Action<string> action )
        {   if ( Event.current == null )
            {   LOGERROR();
                return;
            }
            
            var adapter = Initializator.AdaptersByName[Initializator.PROJECT_NAME];
            var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_190_68, true, adapter);
            
            //var pos = InputData.WidnwoRect( EModulesInternal.FocusRoot.WidnwoRectType.Clamp, Event.current.mousePosition, 190, 68, adapter );
            _W__InputWindow.Init( pos, "New Value", adapter, action, null, s );
        }/** SHOW_STRING */
        static void LOGERROR()
        {   Project.adapter.logProxy.LogWarning( "Input windows can only be called from the OnGUI method" );
        }
        
        static void SHOW_DROPDOWN( int i1, string[] strings, Action<int> arg3, Action<string> arg4 )
        {   if ( Event.current == null )
            {   LOGERROR();
                return;
            }
            
            
            GenericMenu menu = new GenericMenu();
            
            for ( int i = 0 ; i < strings.Length ; i++ )
            {   var ind = i;
                menu.AddItem( new GUIContent( strings[i] ), i == i1, () =>
                {   try
                    {   arg3( ind );
                    }
                    catch ( Exception ex )
                    {   Project.adapter.logProxy.LogError( "Changing Index: " + ex.Message + " " + ex.StackTrace );
                    }
                    
                } );
            }
            
            var adapter = Initializator.AdaptersByName[Initializator.PROJECT_NAME];
            
            if ( arg4 != null )
            {   menu.AddSeparator( "" );
                var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_128_68, true, adapter);
                // var pos = InputData.WidnwoRect( EModulesInternal.FocusRoot.WidnwoRectType.Clamp, Event.current.mousePosition, 128, 68, adapter );
                menu.AddItem( new GUIContent( "New Item" ), false, () =>
                {
                
                
                    _W__InputWindow.Init( pos, "New Item", adapter, ( str ) =>
                    {   if ( string.IsNullOrEmpty( str ) ) return;
                        str = str.Trim();
                        try
                        {   arg4( str );
                        }
                        catch ( Exception ex )
                        {   adapter.logProxy.LogError( "Adding Item: " + ex.Message + " " + ex.StackTrace );
                        }
                    } );
                } );
                
            }
            
            
            
            
            
            
            menu.ShowAsContext();
            Adapter.EventUse();
        }
        
        
        
        
        
        
        
        bool Validate( Adapter.HierarchyObject o )
        {   return !string.IsNullOrEmpty( o.project.assetPath ) && !string.IsNullOrEmpty( customModule.ToString( o.project.assetPath, o.project.guid, o.id, o.project.IsFolder, o.project.IsMainAsset ) );
        }
        
        
        
        /** CALL HEADER */
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   var result = new _W__SearchWindow.FillterData_Inputs( callFromExternal_objects )
            {   Valudator = Validate,
                    SelectCompareString = ( d, i ) => customModule.ToString( d.project.assetPath, d.project.guid, d.id, d.project.IsFolder, d.project.IsMainAsset ),
                    SelectCompareCostInt = ( d, i ) =>
                {   var cost = i;
                    //cost += d.activeInHierarchy ? 0 : 100000000;
                    return cost;
                }
            };
            return result;
        }
        
        internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered( string filter )
        {   var result = CallHeader();
            result.Valudator = s => Validate( s ) && customModule.ToString( s.project.assetPath, s.project.guid, s.id, s.project.IsFolder, s.project.IsMainAsset ) == filter;
            return result;
        }
        /** CALL HEADER */
        
    }
}
}

