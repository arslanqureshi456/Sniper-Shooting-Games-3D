using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//namespace EModules

namespace EModules.EModulesInternal {
internal partial class Hierarchy {
    class M_Warning : Adapter.Module {
        public M_Warning(int restWidth, int sib, bool enable, Adapter adapter ) : base(restWidth, sib, enable,  adapter )
        {
        }
        
        
        internal override float Draw(Rect drawRect, Adapter.HierarchyObject o)
        {
        
            return width;
        }
        
        /*   bool Validate(GameObject o)
        {
            return o.GetComponents<Component>().Any(c => GetComponentType(c) != -1);
        }*/
        
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   //return Utilites.AllSceneObjects().Where(Validate).ToArray();
        
            return null;
        }
    }
}
}

