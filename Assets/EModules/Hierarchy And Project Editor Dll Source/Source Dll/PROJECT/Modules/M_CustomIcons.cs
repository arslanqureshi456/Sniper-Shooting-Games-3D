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


    internal class M_CustomIcons : Adapter.Module, IModuleOnnector_M_CustomIcons {
        public M_CustomIcons( int restWidth, int sibbildPos, bool _enable, Adapter adapter ) : base( restWidth, sibbildPos, _enable, adapter )
        {
        
        }
        
        EventType? IModuleOnnector_M_CustomIcons.useEvent
        {   get { return null; }
            set { }
        }
        Dictionary<int, double> updateTimer = new Dictionary<int, double>();
        Dictionary<int, double> IModuleOnnector_M_CustomIcons.updateTimer { get { return updateTimer; } }
        
        
        static GUIContent tempContent = new GUIContent();
        static GUIContent emptyContent = new GUIContent();
        
        internal override float Draw( Rect drawRect, Adapter.HierarchyObject o )      // drawRect.x += drawRect.width / 2;
        {   // if (!o.project.IsMainAsset) return 0;
        
            if ( !o.project.IsMainAsset ) return 0;
            
            /*   var al = Adapter.GET_SKIN().label.alignment;
               Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleLeft;*/
            
            tempContent.text = o.project.fileExtension;
            if ( adapter.par.COMPONENTS_NEXT_TO_NAME ) drawRect.width = adapter.STYLE_LABEL_8.CalcSize( tempContent ).x;
            
            GUI.Label( drawRect, tempContent, adapter.STYLE_LABEL_8 );
            /*   Adapter.GET_SKIN().label.alignment = al;*/
            
            
            
            if ( Event.current.button != 0 && adapter.SimpleButton( drawRect, emptyContent ) )
            {   if ( Event.current.button == 0 )
                {   Selection.objects = new[] { o.GetHardLoadObject() };
                }
                
                if ( Event.current.button == 1 )
                {   Adapter.EventUse();
                    var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                    
                    _W__SearchWindow.Init( mp, SearchHelper, o.project.fileExtension,
                                           Validate( o ) ?
                                           CallHeaderFiltered( o.project.fileExtension ) :
                                           CallHeader(),
                                           this, adapter, o);
                }
            }
            
            
            
            return width;
        }
        
        
        
        
        
        bool Validate( Adapter.HierarchyObject o )
        {   return !string.IsNullOrEmpty( o.project.fileExtension );
        }
        
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   var result = new _W__SearchWindow.FillterData_Inputs(callFromExternal_objects)
            {   Valudator = Validate,
                    SelectCompareString = (d, i) => d.project.fileExtension,
                    SelectCompareCostInt = (d, i) =>
                {   return i;
                }
            };
            return result;
        }
        
        internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered( string filter )
        {   var result = CallHeader();
            result.Valudator = s => Validate( s ) && s.project.fileExtension == filter;
            return result;
        }
    }
}
}
