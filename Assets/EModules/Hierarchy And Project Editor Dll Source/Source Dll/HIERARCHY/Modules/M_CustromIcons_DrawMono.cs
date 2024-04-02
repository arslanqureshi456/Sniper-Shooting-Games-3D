#define DISABLE_PING

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using UnityEngineInternal;
using Object = UnityEngine.Object;

//namespace EModues

namespace EModules.EModulesInternal {
internal partial class Hierarchy {


	internal partial class M_CustomIcons : Adapter.Module, IModuleOnnector_M_CustomIcons {
	
		/*  static string TransformTypeName = typeof(Transform).Name;
		static string CanvasRendererTypeName = typeof(CanvasRenderer).Name;*/
		
		//  static Texture monotexturedefault = null;
		static string monotexturedefaultjs = "js Script Icon";
		static string monotexturedefaultdll = "dll Script Icon";
		static string monotexturedefaultcs = "cs Script Icon";
		static string monotexturedefaultjs2 = "d_js Script Icon";
		static string monotexturedefaultdll2 = "d_dll Script Icon";
		static string monotexturedefaultcs2 = "d_cs Script Icon";
		
		
		
		void MonDrawer(Adapter.HierarchyObject _o, Component[] cc, Type callbackType, bool allowHide)
		{	if ( cc == null || cc.Length == 0 ) return;
		
			//  tempContent.image = Hierarchy.GetIcon("MONO");
			
			if ( !DRAW_NEXTTONAME ) drawRect.x -= drawRect.width + Hierarchy.HierarchyAdapterInstance.par.ICONSPACE;
			
			//if ( drawRect.Contains( Event.current.mousePosition ) )
			
			Texture image = null;
			
			
			//  MenuText = "Open The Settings To Hide MonoBehaviour Icon...";
			var MenuText = "Hide 'MonoBehaviour' icon";
			
			callbackType = cc.Length == 1 ? Adapter.GetType_( cc[0] ) : null;
			//allowHide = false;
			// drawComps = cc;
			/////   set_drawComps( _o.id, cc );
			//  allComps = comps;
			color = Color.white;
			
			if ( adapter.par.COMPS_SplitMode == 2 )
			{	var getted = Utilities.ObjectContent_NoCacher( adapter, cc[0], type );
				image = getted.add_icon ? getted.add_icon : null;
				
				if ( image )
				{	var n = image.name;
				
					if ( n.Equals( monotexturedefaultcs ) ||
					        n.Equals( monotexturedefaultdll ) ||
					        n.Equals( monotexturedefaultjs ) ||
					        n.Equals( monotexturedefaultcs2 ) ||
					        n.Equals( monotexturedefaultdll2 ) ||
					        n.Equals( monotexturedefaultjs2 )
					   ) image = null;
				}
			}
			
			drawName = !image;
			
			if ( !image ) image = adapter.par.COMPS_SplitMode == 2 && callbackType != null ? adapter.GetIcon( "MONOCLEAN" ) : adapter.GetIcon( "MONO" );
			
			
			
			DrawIcon( cc, _o, drawRect, (Texture2D)image, callbackType, allowHide, ref MenuText);
			
			if ( DRAW_NEXTTONAME ) drawRect.x += drawRect.width + Hierarchy.HierarchyAdapterInstance.par.ICONSPACE - 0.65f;
			
			bool wasDraw = false;
			
			for ( int i = 0 ; i < cc.Length ; i++ )     //  if (!wasDraw && !DRAW_NEXTTONAME) drawRect.x -= Hierarchy.par.ICONSPACE;
			{	if ( DrawAttributes(_o,  cc[i] ) ) wasDraw = true;
			}
			
			if ( wasDraw )
			{	if ( DRAW_NEXTTONAME ) drawRect.x += Hierarchy.HierarchyAdapterInstance.par.ICONSPACE;
			}
			
			
		}
		
		// Component[] emptyArr = new Component[0];
		// GUIContent tempContent = new GUIContent();
		Component[] tempArr = new Component[1];
		
		Dictionary<int, bool> IsMonoBehaviour_helper = new Dictionary<int, bool>();
		
		//  static Type monoType = typeof()
		bool _IsMonoBehaviour( Component comp )
		{	if ( !comp ) return false;
		
			if ( !IsMonoBehaviour_helper.ContainsKey( comp.GetInstanceID() ) )
				IsMonoBehaviour_helper.Add( comp.GetInstanceID(), comp is MonoBehaviour && Utilities.ObjectContent_NoCacher( adapter, (UnityEngine.Object)null, Adapter.GetType_( comp ) ).add_icon == null );
				
			return IsMonoBehaviour_helper[comp.GetInstanceID()];
		}
	}
}
}
