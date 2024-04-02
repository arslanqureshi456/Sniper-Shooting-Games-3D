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


    internal class M_Descript : Adapter.M_DescriptionCommon {
    
    
    
        #if UNITY_EDITOR
        internal override void RegistrateDescription(IDescriptionRegistrator reg, EModules.EModulesInternal.Adapter adapter)
        {
        }
        #else
        internal override void override_RegistrateDescription(IDescriptionRegistrator reg, Adapter adapter)
        {
        }
        #endif
        
        internal override void TrySaveHiglighterRegistrator(Adapter.HierarchyObject o, Adapter.TempColorClass colors32)
        {
        }
        
        public M_Descript(int restWidth, int sibbildPos, bool enable, Adapter adapter) : base( restWidth, sibbildPos, enable, adapter )
        {   adapter.DescriptionModule = this;
        }
        
    }
}
}
