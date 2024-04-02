#if UNITY_EDITOR && PFFPFFFPFF
	#define PROFILERDISABLE
#endif

#if PROJECT
	using EModules.Project;
#endif


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;





namespace EModules.EModulesInternal



{

internal partial class Adapter {
	internal bool ENABLE_BOTTOMDOCK_PROPERTY
	{	get
		{	return par.ENABLE_BOTTOMDOCK && !PROFILER_ENABLE && par.ENABLE_ALL ;
		}
	}
	internal bool ENABLE_LEFTDOCK_PROPERTY
	{	get
			// {   return par.ENABLE_LEFTDOCK_FIX && !PROFILER_ENABLE && par.ENABLE_ALL;
		{	return  !PROFILER_ENABLE && par.ENABLE_ALL;
		}
	}
	internal bool ENABLE_RIGHTDOCK_PROPERTY
	{	get
		{	return par.ENABLE_RIGHTDOCK_FIX && (!DISABLE_PLAY_REPAINT || !par.PLAYMODE_HideRightPanel) && !PROFILER_ENABLE && par.ENABLE_ALL;
		}
	}
	
	internal bool PROFILER_ENABLE
	{	get
		{
#if PROFILERDISABLE
			return false;
#else
			return Profiler.enabled && ProfilerDriver.profileEditor && ProfilerDriver.deepProfiling;
#endif
		}
	}
}
}
