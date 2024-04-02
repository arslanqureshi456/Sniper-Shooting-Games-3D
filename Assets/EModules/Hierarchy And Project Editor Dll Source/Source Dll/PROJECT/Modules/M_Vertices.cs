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


    internal class M_Vertices : Adapter.Module, IModuleOnnector_M_Vertices {
        public M_Vertices( int restWidth, int sibbildPos, bool _enable, Adapter adapter ) : base( restWidth, sibbildPos, _enable, adapter )
        {
        }
        
        void IModuleOnnector_M_Vertices.Clear( ) { }
        Dictionary<int, double> IModuleOnnector_M_Vertices.updateTimer { get { return new Dictionary<int, double>(); } }
        void IModuleOnnector_M_Vertices.CalcBroadCast( ) {  }
        
        
        
        
        internal override float Draw(Rect drawRect, Adapter.HierarchyObject o)
        {
        
            return 0;
            //  throw new NotImplementedException();
        }
        
        
        internal override _W__SearchWindow.FillterData_Inputs CallHeader( )
        {   return null;
            //   throw new NotImplementedException();
        }
    }
}
}
