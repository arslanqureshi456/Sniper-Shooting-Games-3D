﻿//#define BROADCAST

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEditor.Sprites;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
//namespace EModules

namespace EModules.EModulesInternal

{
internal partial class Hierarchy {


	internal class M_Vertices : Adapter.Module, IModuleOnnector_M_Vertices {
	
		void IModuleOnnector_M_Vertices.Clear() { M_Vertices.Clear(); }
		Dictionary<int, double> IModuleOnnector_M_Vertices.updateTimer { get { return M_Vertices.GetDescript().updateTimer; } }
		void IModuleOnnector_M_Vertices.CalcBroadCast() { CalcBroadCast(); }
		
		
		/* internal override bool enableOverride( )
		 {
		     return !Adapter.LITE;
		 }*/
		// internal override string enableOverrideMessage( ) { return " (Pro Only)"; }
		
		
		static Dictionary<Scene, M_VerticesHelper> d = new Dictionary<Scene, M_VerticesHelper>();
		internal static M_VerticesHelper GetDescript()
		{	if ( !d.ContainsKey( EditorSceneManager.GetActiveScene() ) ) d.Add( EditorSceneManager.GetActiveScene(), new M_VerticesHelper() );
		
			return d[EditorSceneManager.GetActiveScene()];
		}
		
		static GUIContent content = new GUIContent();
		public M_Vertices( int restWidth, int sib, bool enable, Adapter adapter ) : base( restWidth, sib, enable, adapter )
		{	Clear();
		
			/*  long vv = ((long)1) << 33;
			  MonoBehaviour.print(((int)vv));*/
		}
		
		
		
		
		internal static void Clear()     // var s = EditorSceneManager.GetActiveScene();
		{	var dsc = GetDescript();
		
			dsc.BroadcastingInitializeAllObjects = false;
			dsc.Eroor = false;
			dsc.WasFirst = false;
			dsc.Broadcasting = false;
			dsc.md.Clear();
			dsc.shaderTextures.Clear();
			dsc.TEXTUREobjects.Clear();
			dsc.OBJECTtexture.Clear();
			dsc.cacheValue.Clear();
			dsc.broadCastValue.Clear();
			dsc.updateTimer.Clear();
		}
		// static Dictionary<GameObject, long> singleValue = new Dictionary<GameObject, long>();
		
		
		void WriteMF( M_VerticesHelper dsc, GameObject o )
		{	if ( adapter.par.VerticesModuleType == VerticesModuleTypeEnum.ChildCount ) return;
		
			if ( !dsc.md.ContainsKey( o.GetInstanceID() ) ) CHECK_MESHFILTER( ref dsc, o );
			
			if (!dsc.md.ContainsKey( o.GetInstanceID() ) )
			{	// Debug.LogError( o.name + " hasn't MeshFilter" );
				//D mf = null;
				return;
			}
			
			else
			{	mf = dsc.md[o.GetInstanceID()];
			}
		}
		
		bool CHECK_MESHFILTER( GameObject o )     //  var s = EditorSceneManager.GetActiveScene();
		{	if ( adapter.par.VerticesModuleType == VerticesModuleTypeEnum.ChildCount ) return false;
		
			var dsc = GetDescript();
			return CHECK_MESHFILTER( ref dsc, o );
		}
		bool CHECK_MESHFILTER( ref M_VerticesHelper dsc, GameObject o )       //  var s = EditorSceneManager.GetActiveScene();
		{	if ( adapter.par.VerticesModuleType == VerticesModuleTypeEnum.ChildCount ) return false;
		
			// var dsc = GetDescript();
			// if ( dsc.)
			if ( !dsc.md.ContainsKey( o.GetInstanceID() ) )
			{	/* if (md.Any(meshFilter => !meshFilter.Value))
				     foreach (var source in md.Keys.ToArray().Where(source => md[source] == null))
				         md.Remove(source);*/
				
				dsc.md.Add( o.GetInstanceID(), new M_VerticesHelper.MeshGetter() );
			}
			
			//  if (!Validate(o)) return width;
			
			mf = dsc.md[o.GetInstanceID()];
			
			if ( !mf.mesh )
			{	mf.SetMesh = o;
			
				/*= o.GetComponent<MeshFilter>();
				if (!mf.f) mf.s*/
				if ( !mf.mesh )
				{	dsc.md.Remove( o.GetInstanceID() );
					return false;
				}
				
				dsc.md[o.GetInstanceID()] = mf;
			}
			
			return mf.mesh;
		}
		
		bool CHECK_TEXTURE( GameObject o )     // var mr = o.GetComponent<MeshRenderer>();
		{	var mr = o.GetComponent<Renderer>();
			var pr = o.GetComponent<ParticleSystemRenderer>();
			var sr = mr ? null : o.GetComponent<SkinnedMeshRenderer>();
			var sharedMat = mr ? mr.sharedMaterials : pr ? pr.sharedMaterials : sr ? sr.sharedMaterials : null;
			
			
			if ( sharedMat != null && sharedMat.Length != 0 )
			{	if ( sharedMat.Length != 0 ) return true;
			
			}
			
			else
			{	var I = o.GetComponent<Image>();
				var SR = o.GetComponent<SpriteRenderer>();
				var sprite = I ? I.sprite : SR ? SR.sprite : null;
				
				// MonoBehaviour.print(sprite + " " + (sprite != null));
				if ( sprite != null/* && i.sprite != null*/) return true;
			}
			
			/*     var i = o.GetComponent<Image>();
			     if (i != null && i.sprite != null) return true;*/
			return false;
		}
		
		/*  string[] contexthelper =
		  {
		          "Optimizer - Triangles count",
		          "Optimizer - Vertices count",
		          "Optimizer - ChildCount",
		          "Optimizer - Texture Memory Factor (Experimental)"
		      };*/
		
		string BuildFullName( M_VerticesHelper des )
		{	return BuildFullName( adapter.par.VerticesModuleType, des );
		}
		string BuildFullName( VerticesModuleTypeEnum type, M_VerticesHelper des )
		{	return HeaderText + " - " + nameRight( type, des );
		}
		
		string nameRight( VerticesModuleTypeEnum type, M_VerticesHelper des )
		{	switch ( type )
			{	case VerticesModuleTypeEnum.Triangles:
					return "Triangles Count";
					
				case VerticesModuleTypeEnum.Vertices:
					return "Vertices Count";
					
				case VerticesModuleTypeEnum.ChildCount:
					return "Child Count";
					
				case VerticesModuleTypeEnum.TextureMemory:
					return adapter.par.BroadCastOptimizer ? "Textures In Memory (TextureSize\\ObjectsCount)" : "Textures In Memory (TextureSize Only)";
					
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
		
		
		static int frameSkip = 1;
		
		M_VerticesHelper.MemoryData calcValue( M_VerticesHelper des, GameObject o )
		{	/*  if (par.BroadCastOptimizer)
			  {*/
			if ( des.broadCastValue.ContainsKey( o.GetInstanceID() ) ) return des.broadCastValue[o.GetInstanceID()];
			
			tempData.Clear();
			/*    tempData.memory = 0;
			    tempData.postfix = ' ';*/
			return tempData;
			
			/*   }
			   else
			   {
			       return __calcValue(o);
			   }*/
		}
		List<string> tempProps = new List<string>();
		
		List<int> tempTri = new List<int>();
		M_VerticesHelper.MemoryData tempData;
		
		
		M_VerticesHelper.MemoryData __fakeCalc( M_VerticesHelper des, GameObject o )
		{	/*if (!WriteMF( des, o))
			     {
			         tempData.Clear();
			         return tempData;
			     }*/
			WriteMF( des, o );
			return __calcValue( o );
		}
		M_VerticesHelper.MemoryData __calcValue( GameObject o )
		{	tempData.Clear();
		
			/* tempData.memory = 0;
			 tempData.postfix = ' ';*/
			switch ( Hierarchy.HierarchyAdapterInstance.par.VerticesModuleType )
			{	case VerticesModuleTypeEnum.Triangles:
					var mm = mf.mesh;
					var sb = mm.subMeshCount;
					tempData.memory = 0;
					
					for ( int i = 0 ; i < sb ; i++ )
					{	adapter.GetTriangle( mf.mesh, ref tempTri, i );
						tempData.memory += tempTri.Count / 3;
					}
					
					tempData.instance = true;
					break;
					
				case VerticesModuleTypeEnum.Vertices:
					tempData.memory = mf.mesh.vertexCount;
					tempData.instance = true;
					break;
					
				case VerticesModuleTypeEnum.ChildCount:
					tempData.memory = o.transform.childCount;
					tempData.instance = true;
					break;
					
				case VerticesModuleTypeEnum.TextureMemory:
					// Texture targetTexture = null;
					bool wasTexture = false;
					
					/*   var mr = o.GetComponent<Renderer>();
					   if (!mr) mr = o.GetComponent<ParticleSystemRenderer>();*/
					
					var mr = o.GetComponent<Renderer>();
					var pr = mr ? null : o.GetComponent<ParticleSystemRenderer>();
					var sr = mr ? null : o.GetComponent<SkinnedMeshRenderer>();
					var sharedMat = mr ? mr.sharedMaterials : pr ? pr.sharedMaterials : sr ? sr.sharedMaterials : null;
					
					if ( sharedMat != null && sharedMat.Length != 0 )
					{	/* if (sharedMat.Length != 0) return true;
					
						}
						
						if (mr)
						{*/
						if ( sharedMat.Length != 0 )
						{	for ( int j = 0 ; j < sharedMat.Length ; j++ )
							{	if ( sharedMat[j] != null )
								{	var shader = sharedMat[j].shader;
								
									/* if (!dsc.shaderTextures.ContainsKey(shader))
									 {
									     tempProps.Clear();
									     Shader s;
									     //s.name
									     for (int k = 0, len = ShaderUtil.GetPropertyCount(shader); k < len; k++)
									         if (ShaderUtil.GetPropertyType(shader, k) == ShaderUtil.ShaderPropertyType.TexEnv && !ShaderUtil.IsShaderPropertyHidden(shader, k))
									             tempProps.Add(ShaderUtil.GetPropertyName(shader, k));
									
									     dsc.shaderTextures.Add(shader, tempProps.ToArray());
									 }
									
									 var arr = dsc.shaderTextures[shader];
									*/
									
									tempProps.Clear();
									
									for ( int k = 0, len = ShaderUtil.GetPropertyCount( shader ) ; k < len ; k++ )
										if ( ShaderUtil.GetPropertyType( shader, k ) == ShaderUtil.ShaderPropertyType.TexEnv && !ShaderUtil.IsShaderPropertyHidden( shader, k ) )
											tempProps.Add( ShaderUtil.GetPropertyName( shader, k ) );
											
									var texts = new Texture[tempProps.Count];
									bool haveANyTexture = false;
									
									for ( int i = 0 ; i < texts.Length ; i++ )
									{	texts[i] = sharedMat[j].GetTexture( tempProps[i] );
									
										if ( !haveANyTexture && texts[i] ) haveANyTexture = true;
									}
									
									//   var mem =
									if ( haveANyTexture )
									{	tempData.memory += TextureOperator( o, new M_VerticesHelper.TextureSplitter( texts ), j );
										bool wasFirst = false;
										
										for ( int i = 0 ; i < texts.Length ; i++ )
										{	if ( !texts[i] ) continue;
										
											if ( !wasFirst )
											{	wasFirst = true;
												tempData.addparams = "";
											}
											
											else
											{	tempData.addparams += '\n';
											}
											
											tempData.addparams += "Texture '" + texts[i].name + "' " + MemoryToDisapley( rawMemory[i] );
										}
										
										
										tempData.instance = true;
										wasTexture = true;
									}
									
								}
							}
							
							/*       Shader s;
							       s.
							       targetTexture = mr.sharedMaterial.get;*/
							
						}
					}
					
					else
					{	var I = o.GetComponent<Image>();
						var SR = o.GetComponent<SpriteRenderer>();
						//  MonoBehaviour.print(SR);
						var sprite = I ? I.sprite : SR ? SR.sprite : null;
						
						//  MonoBehaviour.print(sprite);
						if ( sprite != null/* && i.sprite != null*/)
						{	Packer.GetAtlasDataForSprite( sprite, out atlas, out atlasTexture );
						
							if ( atlasTexture != null )
							{	tempData.postfix = 'A';
								// var mem =
								tempData.memory += TextureOperator( o, new M_VerticesHelper.TextureSplitter( atlasTexture ), 0 );
								tempData.addparams = "Atlas '" + atlas + "' " + MemoryToDisapley( rawMemory[0] );
							}
							
							else
							{	tempData.memory += TextureOperator( o, new M_VerticesHelper.TextureSplitter( sprite.texture ), 0 );
								tempData.addparams += "Texture '" + sprite.texture.name + "' " + MemoryToDisapley( rawMemory[0] );
								tempData.addparams += "\nWarning '" + sprite.texture.name + "' Not included in the atlas";
								tempData.addparams += "\nYou must assign a Packing Tag or Repack your atlases";
								tempData.postfix = '!';
							}
							
							tempData.instance = true;
							wasTexture = true;
						}
						
					}
					
					if ( !wasTexture ) tempData.memory += TextureOperator( o, null, 0 );
					
					
					
					// blow off the pass at the first round
					// make an error to show if it doesn't work
					// make the backlight red and for textures
					break;
					
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			return tempData;
		}
#pragma warning disable
		string atlas;
		Texture2D atlasTexture;
#pragma warning restore
		
		
		long TextureOperator( GameObject o, M_VerticesHelper.TextureSplitter? targetTexture, int index )         //  var s = EditorSceneManager.GetActiveScene();
		{	var dsc = GetDescript();
		
		
			var id = o.GetInstanceID();
			
			if ( dsc.OBJECTtexture.ContainsKey( id ) )
			{	var get = dsc.OBJECTtexture[id];
			
				if ( targetTexture == null )     // MonoBehaviour.print("ASD");
				{	foreach ( var t in get[index].textures )
						{	if ( !t ) continue;
						
							if ( dsc.TEXTUREobjects.ContainsKey( t ) ) dsc.TEXTUREobjects[t].Remove( id );
							
							if ( dsc.TEXTUREobjects[t].Count == 0 ) dsc.TEXTUREobjects.Remove( t );
						}
						
					dsc.OBJECTtexture.Remove( id );
				}
				
				else
				{	if ( get.ContainsKey( index ) && !get[index].Equals( targetTexture.Value ) ) //changeTexture
						{
						
							//var t = get[index];
							foreach ( var t in get[index].textures )
								{	if ( !t ) continue;
								
									if ( dsc.TEXTUREobjects.ContainsKey( t ) ) dsc.TEXTUREobjects[t].Remove( id );
									
									if ( dsc.TEXTUREobjects[t].Count == 0 ) dsc.TEXTUREobjects.Remove( t );
									
								}
								
							get.Remove( index );
							
							if ( get.Count == 0 ) dsc.OBJECTtexture.Remove( id );
						}
				}
				
				
			}
			
			
			if ( targetTexture != null )
			{	if ( !dsc.OBJECTtexture.ContainsKey( id ) || !dsc.OBJECTtexture[id].ContainsKey( index ) )
				{	if ( dsc.OBJECTtexture.ContainsKey( id ) ) dsc.OBJECTtexture.Remove( id );
				
					foreach ( var texturEobject in dsc.TEXTUREobjects ) texturEobject.Value.Remove( id );
					
					// if (OBJECTtexture.Any(meshFilter => !meshFilter.Value))
					/*   foreach (var source in OBJECTtexture.Keys.ToArray().Where(source => OBJECTtexture[source] == null))
					       OBJECTtexture.Remove(source);*/
					
					if ( !dsc.OBJECTtexture.ContainsKey( id ) ) dsc.OBJECTtexture.Add( id, new Dictionary<int, M_VerticesHelper.TextureSplitter>() );
					
					dsc.OBJECTtexture[id].Add( index, targetTexture.Value );
					
					for ( int i = 0 ; i < targetTexture.Value.textures.Length ; i++ )
					{	if ( !targetTexture.Value.textures[i] ) continue;
					
						if ( !dsc.TEXTUREobjects.ContainsKey( targetTexture.Value.textures[i] ) ) dsc.TEXTUREobjects.Add( targetTexture.Value.textures[i], new Dictionary<int, bool>() );
						
						if ( !dsc.TEXTUREobjects[targetTexture.Value.textures[i]].ContainsKey( id ) ) dsc.TEXTUREobjects[targetTexture.Value.textures[i]].Add( id, false );
					}
					
				}
				
				//OBJECTtexture[id] = targetTexture;
				
				long sum = 0;
				
				if ( rawMemory.Length < targetTexture.Value.textures.Length ) Array.Resize( ref rawMemory, targetTexture.Value.textures.Length );
				
				for ( int i = 0 ; i < targetTexture.Value.textures.Length ; i++ )
				{	if ( !targetTexture.Value.textures[i] ) continue;
				
					// MonoBehaviour.print(dsc.TEXTUREobjects[targetTexture.Value.textures[i]].Count);
					//#if UNITY_5_5
					// rawMemory[i] = Profiler.GetRuntimeMemorySize(targetTexture.Value.textures[i]);
					rawMemory[i] = adapter.GetMemorySize( targetTexture.Value.textures[i] );
					
					/* Texture.
					 ((Texture2D)targetTexture.Value.textures[i].i).is
					 if ()*/
					/*#else
					                        rawMemory[i] = Profiler.GetRuntimeMemorySizeLong(targetTexture.Value.textures[i]);
					#endif*/
					if ( dsc.Broadcasting ) sum += rawMemory[i] / dsc.TEXTUREobjects[targetTexture.Value.textures[i]].Count;
					else sum += rawMemory[i];
				}
				
				return sum;
				
				/* if (!singleValue.ContainsKey(o)) singleValue.Add(o, 0);
				 singleValue[o] = v;*/
			}
			
			return 0;
		}
		
		long[] rawMemory = new long[0];
		
		bool Allow( GameObject o )
		{	if ( Hierarchy.HierarchyAdapterInstance.par.VerticesModuleType == VerticesModuleTypeEnum.Triangles ||
			        Hierarchy.HierarchyAdapterInstance.par.VerticesModuleType == VerticesModuleTypeEnum.Vertices
			   )
				if ( !CHECK_MESHFILTER( o ) ) return false;
				
			if ( Hierarchy.HierarchyAdapterInstance.par.VerticesModuleType == VerticesModuleTypeEnum.TextureMemory )
				if ( !CHECK_TEXTURE( o ) ) return false;
				
			return true;
			
		}
		
		private static M_VerticesHelper.MeshGetter mf;
		
		
		void broadCastAction( M_VerticesHelper dsc, GameObject get_o )
		{
		
		
			if ( !dsc.updateTimer.ContainsKey( get_o.GetInstanceID() ) ) dsc.updateTimer.Add( get_o.GetInstanceID(), 0 );
			
			if ( Math.Abs( EditorApplication.timeSinceStartup - dsc.updateTimer[get_o.GetInstanceID()] ) < 0.5 ) return;
			
			
			tempData.Clear();
			
			if ( !dsc.broadCastValue.ContainsKey( get_o.GetInstanceID() ) )
			{	adapter.RepaintWindowInUpdate();
				ResetStack( get_o.GetInstanceID() );
				// adapter.RESET_DRAW_STACKS();
				dsc.broadCastValue.Add( get_o.GetInstanceID(), tempData );
			}
			
			dsc.updateTimer[get_o.GetInstanceID()] = EditorApplication.timeSinceStartup;
			
			/*   tempData.memory = 0;
			   tempData.postfix = ' ';*/
			if ( Allow( get_o ) ) tempData = __calcValue( get_o );
			
			for ( int i = 0 ; i < get_o.transform.childCount ; i++ )
			{	var c = get_o.transform.GetChild( i );
			
				if ( dsc.broadCastValue.ContainsKey( c.gameObject.GetInstanceID() ) )
				{	var g = dsc.broadCastValue[c.gameObject.GetInstanceID()];
					tempData.memory += g.memory;
					
					if ( g.postfix == '!' ) tempData.postfix = '!';
				}
				
				// if (!updateTimer.ContainsKey(c.gameObject.GetInstanceID())) oldval = -1;
			}
			
			var oldval = dsc.broadCastValue[get_o.GetInstanceID()];
			dsc.broadCastValue[get_o.GetInstanceID()] = tempData;
			
			if ( oldval.memory != dsc.broadCastValue[get_o.GetInstanceID()].memory )     //           lastUpdate = 0;
			{	dsc.updateTimer[get_o.GetInstanceID()] = 0;
				adapter.RepaintWindowInUpdate();
				ResetStack( get_o.GetInstanceID() );
				//  adapter.RESET_DRAW_STACKS();
			}
		}
		void broadCastActionOnlyCaclulate( M_VerticesHelper dsc, GameObject get_o )
		{
		
		
			if ( !dsc.updateTimer.ContainsKey( get_o.GetInstanceID() ) ) dsc.updateTimer.Add( get_o.GetInstanceID(), 0 );
			
			// if (Math.Abs(EditorApplication.timeSinceStartup - dsc.updateTimer[get_o.GetInstanceID()]) < 0.5) return;
			
			
			//   if (!dsc.broadCastValue.ContainsKey(get_o.GetInstanceID())) dsc.broadCastValue.Add(get_o.GetInstanceID(), tempData);
			dsc.updateTimer[get_o.GetInstanceID()] = EditorApplication.timeSinceStartup;
			
			var oldval = !dsc.broadCastValue.ContainsKey( get_o.GetInstanceID() ) ? 0 : dsc.broadCastValue[get_o.GetInstanceID()].memory;
			long newVal = 0;
			
			if ( Allow( get_o ) ) newVal = __calcValue( get_o ).memory;
			
			if ( newVal != oldval )
			{	dsc.updateTimer[get_o.GetInstanceID()] = 0;
				adapter.RepaintWindowInUpdate();
				ResetStack( get_o.GetInstanceID() );
				// adapter.RESET_DRAW_STACKS();
			}
		}
		
		
		//bool initFlag  = false;
		
		
		
		//  static bool broadcasting = false;
		static IEnumerator<Adapter.HierarchyObject> currentList = null;
		static int currentIndex = 0;
		static int interator = 0;
		static Action<M_VerticesHelper, GameObject> brc;
		static List<Texture> removeList = new List<Texture>();
		static void StartBroadcasting()
		{	if ( frameSkip != 0 )
			{	frameSkip--;
				return;
			}
			
			var s = EditorSceneManager.GetActiveScene();
			
			if ( !s.isLoaded || !s.IsValid() ) return;
			
			var dsc = GetDescript();
			
			if ( dsc.Broadcasting ) return;
			
			dsc.Broadcasting = true;
			dsc.shaderTextures.Clear();
			
			foreach ( var texturEobject in dsc.TEXTUREobjects )
			{
			
				/*   if (texturEobject.Key == null)
				   {
				       continue;
				   }*/
				if ( dsc.TEXTUREobjects[texturEobject.Key].Any( o => !EditorUtility.InstanceIDToObject( o.Key ) ) )
				{	foreach ( var source in dsc.TEXTUREobjects[texturEobject.Key].Keys.ToArray() )
					{	if ( !EditorUtility.InstanceIDToObject( source ) )
						{	dsc.TEXTUREobjects[texturEobject.Key].Remove( source );
							dsc.OBJECTtexture.Remove( source );
						}
					}
					
					if ( dsc.TEXTUREobjects[texturEobject.Key].Count == 0 )
					{	removeList.Add( texturEobject.Key );
					}
				}
			}
			
			if ( removeList.Count != 0 )     // MonoBehaviour.print("ASD" + removeList.Count);
			{	foreach ( var texture in removeList )
				{	dsc.TEXTUREobjects.Remove( texture );
				}
				
				removeList.Clear();
			}
			
			
			currentList = Utilities.AllSceneObjectsInterator( Hierarchy.HierarchyAdapterInstance ).GetEnumerator();
			//currentList.Reverse();
			
			interator = currentIndex = 0;
		}
		
		
		internal  void CalcBroadCast()     // var s = EditorSceneManager.GetActiveScene();
		{	var dsc = GetDescript();
		
			//  MonoBehaviour.print("AS");
			if ( !dsc.Broadcasting || !Hierarchy.HierarchyAdapterInstance.ENABLE_RIGHTDOCK_PROPERTY || !Hierarchy.HierarchyAdapterInstance.par.ENABLE_ALL || currentList == null ) return;
			
			interator = 0;
			
			//foreach (var current in currentList)
			while ( currentList.MoveNext() )
				//while (currentIndex < currentList.Count)
			{	var current = currentList.Current;
			
				if ( !/*currentList[currentIndex]*/current.go )
				{	currentIndex++;
					continue;
				}
				
				if ( (current.go.hideFlags & HideFlags.HideInInspector) != 0 )
				{	currentIndex++;
					continue;
				}
				
				
				
				try
				{	brc( dsc, current.go );
				}
				
				catch
				{	Clear();
					currentIndex = 0;
					dsc.Broadcasting = false;
					dsc.Eroor = true;
					dsc.WasFirst = false;
					return;
				}
				
				currentIndex++;
				interator++;
				
				if ( interator > Hierarchy.HierarchyAdapterInstance.par.BROADCASTING_PREFOMANCE ) break;
			}
			
			//   MonoBehaviour.print(dsc.TEXTUREobjects.Count + " " + dsc.OBJECTtexture.Count);
			if ( interator <= Hierarchy.HierarchyAdapterInstance.par.BROADCASTING_PREFOMANCE )
			{	dsc.Broadcasting = false;
				dsc.Eroor = false;
				
				if ( !dsc.WasFirst )
				{	dsc.WasFirst = true;
					Hierarchy.HierarchyAdapterInstance.RepaintWindowInUpdate();
					ResetStack();
					
					if ( !Hierarchy.HierarchyAdapterInstance.par.BroadCastOptimizer && Hierarchy.HierarchyAdapterInstance.par.VerticesModuleType == VerticesModuleTypeEnum.TextureMemory )
					{	dsc.broadCastValue.Clear();
						/*foreach (var memoryData in dsc.broadCastValue)
						{
						    var v = memoryData.Value;
						    v.addparams
						}*/
					}
				}
				
				else
				{	if ( !dsc.BroadcastingInitializeAllObjects )
					{	dsc.BroadcastingInitializeAllObjects = true;
					}
					
				}
				
			}
			
			else
			{	Hierarchy.HierarchyAdapterInstance.RepaintWindowInUpdate();
				ResetStack();
				//  Hierarchy.HierarchyAdapterInstance.RESET_DRAW_STACKS();
			}
		}
		
		
		//  Color labelColor = new Color(0.9f, 0.5f, 0.2f, 1);
		Color labelWarningColor = new Color( 1f, 0.5f, 0.4f, 1 );
		string MemoryToDisapley( long v )
		{	if ( v > 1000000 ) return (Mathf.RoundToInt( v / 100000f ) / 10f).ToString( CultureInfo.InvariantCulture ) + "M";
			else
				if ( v > 1000 ) return (Mathf.RoundToInt( v / 100f ) / 10f).ToString( CultureInfo.InvariantCulture ) + "k";
				
			return v.ToString();
		}
		
		internal override float Draw( Rect drawRect, Adapter.HierarchyObject _o )       //Profiler.GetRuntimeMemorySizeLong( !!!!!!!!!!!!!!!
		{
		
			//if (OPT_EV_BR(Event.current)) return 0;
			
			// base.ContextHelper = "Optimizer set to " + Hierarchy.par.VerticesModuleType + " count";
			//  base.ContextHelper = contexthelper[(int)Hierarchy.par.VerticesModuleType];
			//  base.ContextHelper = BuildFullName(Hierarchy.par.VerticesModuleType);
			
			if ( Hierarchy.HierarchyAdapterInstance.DISABLE_DESCRIPTION( _o ) )
			{	/* var fs = Adapter.GET_SKIN().label.fontSize;
				var al = Adapter.GET_SKIN().label.alignment;
				Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleRight;
				if ( !callFromExternal() ) Adapter.GET_SKIN().label.fontSize = Hierarchy.HierarchyAdapterInstance.FONT_8();
				else Adapter.GET_SKIN().label.fontSize = Hierarchy.HierarchyAdapterInstance.WINDOW_FONT_8();*/
				
				GUI.Label( drawRect, Adapter.CacheDisableConten, !callFromExternal() ? adapter.STYLE_LABEL_8_right : adapter.STYLE_LABEL_8_WINDOWS_right );
				
				//                 Adapter.GET_SKIN().label.alignment = al;
				//                 Adapter.GET_SKIN().label.fontSize = fs;
				return width;
			}
			
			/* if ( Event.current.type == EventType.Layout && initFlag )
			 {   ResetStack();
			     initFlag = false;
			     Debug.Log( "ASD" );
			 }*/
			
			if ( !START_DRAW( drawRect, _o ) ) return 0;
			
			var o = _o.go;
			
			
			base.ContextHelper = HeaderText;
			
			if ( adapter.par.BroadCastOptimizer && Application.isPlaying )
			{	adapter.par.BroadCastOptimizer = false;
			}
			
#if BROADCAST
			
			if (Application.isPlaying && par.VerticesModuleType == VerticesModuleTypeEnum.TextureMemory)
			{	par.VerticesModuleType = VerticesModuleTypeEnum.Triangles;
			}
			
#endif
			//  var s = EditorSceneManager.GetActiveScene();
			var dsc = GetDescript();
			bool needhide = true;
			
			if ( Event.current.type == EventType.Repaint )
			{
			
				headOverrideTexture = adapter.par.BroadCastOptimizer /*|| par.VerticesModuleType == VerticesModuleTypeEnum.TextureMemory*/ ? adapter.redTTexure : (Color? )null;
				// try
				// {
				
				
				
				
				
				//   if (!singleValue.ContainsKey(o)) singleValue.Add(o, 0);
				if ( adapter.par.BroadCastOptimizer )     //  if (Math.Abs(EditorApplication.timeSinceStartup - lastUpdate) > 0.5)
				{	//   {
					//      lastUpdate = EditorApplication.timeSinceStartup;
					//  bool needRepaing = false;
					// foreach (var get_o in Utilites.AllSceneObjects())
					// foreach (var get_o in Utilites.AllSceneObjects())
					/*  {
					
					  }*/
					//  }
					
					//  Utilites.BroadCastActionReverse(o, broadCastAction);
					
					brc = broadCastAction;
					StartBroadcasting();
					
				}
				
				else
				{
				
#if BROADCAST
				
					if (par.VerticesModuleType == VerticesModuleTypeEnum.TextureMemory)
					{	brc = broadCastActionOnlyCaclulate;
						StartBroadcasting();
					}
					
#endif
					
					
					if ( !dsc.updateTimer.ContainsKey( o.GetInstanceID() ) ) dsc.updateTimer.Add( o.GetInstanceID(), 555 );
					
					if ( Math.Abs( EditorApplication.timeSinceStartup - dsc.updateTimer[o.GetInstanceID()] ) > 0.5 )
					{	if ( !dsc.broadCastValue.ContainsKey( o.GetInstanceID() ) ) dsc.broadCastValue.Add( o.GetInstanceID(), tempData );
					
						if ( Allow( o ) )
						{	var old = dsc.broadCastValue[o.GetInstanceID()].memory;
							dsc.broadCastValue[o.GetInstanceID()] = __calcValue( o );
							
							if ( old != dsc.broadCastValue[o.GetInstanceID()].memory )
							{	adapter.RepaintWindowInUpdate();
							
								//ResetStack( o.GetInstanceID() );
								//  adapter.RESET_DRAW_STACKS();
								if (adapter.firstFrame < 4) adapter.RESET_DRAW_STACKS();///////////////////////
								else ResetStack();
								
								//initFlag = true;
								//
							}
						}
						
						else
						{	tempData.Clear();
						
							/*   tempData.memory = 0;
							   tempData.postfix = ' ';*/
							dsc.broadCastValue[o.GetInstanceID()] = tempData;
						}
						
						dsc.updateTimer[o.GetInstanceID()] = EditorApplication.timeSinceStartup;
						
						if ( !dsc.WasFirst
#if BROADCAST
						        && par.VerticesModuleType != VerticesModuleTypeEnum.TextureMemory
#endif
						   )
						{	dsc.WasFirst = true;
							// initFlag = true;
							dsc.BroadcastingInitializeAllObjects = true;
							adapter.RepaintWindowInUpdate();
							
							if ( adapter.firstFrame < 4 )  adapter.RESET_DRAW_STACKS(); ///////////////////////
							else   ResetStack();
							
						}
					}
					
					
				}
				
				
				/*   brc = broadCastAction;
				   StartBroadcasting();*/
				if ( Allow( o ) || adapter.par.BroadCastOptimizer )
				{	if ( (dsc.WasFirst
#if BROADCAST
					        || par.VerticesModuleType != VerticesModuleTypeEnum.TextureMemory
#endif
					     ) && dsc.broadCastValue.ContainsKey( o.GetInstanceID() ) )
					{	var v = dsc.broadCastValue[o.GetInstanceID()];
					
						content.text = MemoryToDisapley( v.memory );
						
						
						content.tooltip = BuildFullName( dsc ) + " " + content.text;
						
						if ( v.memory == 0 ) content.text = "-";
						else
							if ( v.postfix != ' ' )
							{	content.text = string.Concat( "", v.postfix, "  ", content.text );
							
								if ( v.postfix == 'A' ) content.tooltip += " (Atlas)";
							}
							
						if ( v.addparams != null )
						{	content.tooltip += "\n" + v.addparams;
						}
						
						needhide = !v.instance;
					}
					
					else
					{	if ( dsc.Eroor )
						{	content.tooltip = BuildFullName( dsc ) + " error";
							content.text = "error";
						}
						
						else
						{	content.tooltip = BuildFullName( dsc ) + " ...";
							content.text = "...";
							needhide = false;
						}
					}
					
					
				}
				
				else
				{	content.tooltip = BuildFullName( dsc ) + " 0";
					content.text = "-";
				}
				
				
				
				
				// }
				/*     catch
				     {
				         content.tooltip = BuildFullName() + " error";
				         content.text = "error";
				     }*/
				
				
				if ( !dsc.cacheValue.ContainsKey( o.GetInstanceID() ) ) dsc.cacheValue.Add( o.GetInstanceID(), new GUIContent() );
				
				dsc.cacheValue[o.GetInstanceID()].tooltip = content.tooltip;
				dsc.cacheValue[o.GetInstanceID()].text = content.text;
			}
			
			if ( dsc.cacheValue.ContainsKey( o.GetInstanceID() ) )
			{	content.tooltip = dsc.cacheValue[o.GetInstanceID()].tooltip;
				content.text = dsc.cacheValue[o.GetInstanceID()].text;
			}
			
			else
			{	content.tooltip = "Updating";
				content.text = "...";
				needhide = false;
			}
			
			
			/*  var oldl = Adapter.GET_SKIN().label.fontSize;
			var olda = Adapter.GET_SKIN().label.alignment;
			var oldss = Adapter.GET_SKIN().label.fontStyle;
			Adapter.GET_SKIN().label.alignment = TextAnchor.MiddleRight;
			Adapter.GET_SKIN().label.fontSize = adapter.FONT_8();*/
#if BROADCAST
			|| par.VerticesModuleType == VerticesModuleTypeEnum.TextureMemory
#endif
			
			
			/*  if ( adapter.par.BroadCastOptimizer )
			  {   var asd = GUI.color;
			      GUI.color *= new Color32( 200, 90, 50, 15 );
			      GUI.DrawTexture( drawRect, Texture2D.whiteTexture );
			      GUI.color = asd;
			  }
			
			  var L = adapter.STYLE_LABEL_8_right;
			
			
			  if ( content.text.EndsWith( "M" ) ) L.fontStyle = FontStyle.Bold;
			  GUI.enabled = o.activeInHierarchy;
			  var hasContent = true;
			
			  if ( content.text[0] == '!' )
			  {   var oldC = L.normal.textColor;
			      L.normal.textColor = Color.black;
			      r.Set( drawRect.x + 0.5f, drawRect.y, drawRect.width, drawRect.height );
			      GUI.Label( r, content, L );
			      L.normal.textColor = labelWarningColor;
			      GUI.Label( drawRect, content, L );
			      L.normal.textColor = oldC;
			  }
			  else
			  {   var oldstate = GUI.enabled;
			      GUI.enabled &= !needhide;
			      var al = L.alignment;
			      if ( content.text == "-" )
			      {   L.alignment = __Align;
			          hasContent = false;
			      }
			      if ( EditorGUIUtility.isProSkin && GUI.enabled )
			      {   var oldC = L.normal.textColor;
			          L.normal.textColor = Color.black;
			          r.Set( drawRect.x + 1, drawRect.y, drawRect.width, drawRect.height );
			          GUI.Label( r, content, L );
			          L.normal.textColor = oldC;
			      }
			      GUI.Label( drawRect, content, L );
			      L.alignment = al;
			      GUI.enabled = oldstate;
			  }
			
			  GUI.enabled = true;
			  L.fontStyle = FontStyle.Normal;
			  */
			
			if ( adapter.par.BroadCastOptimizer )
			{	Draw_AdapterTexture( drawRect, new Color32( 200, 90, 50, 15 ));
			}
			
			if ( STYLE_M_BLACKCOLOR == null )
			{	STYLE_M_BLACKCOLOR = new GUIStyle[4];
				STYLE_M_WARMCOLOR = new GUIStyle[4];
				STYLE_M_NORMALCOLOR = new GUIStyle[4];
				
				for ( int i = 0 ; i < 2 ; i++ )
				{	STYLE_M_BLACKCOLOR[i] = new GUIStyle( adapter.STYLE_LABEL_8_right );
					STYLE_M_BLACKCOLOR[i].normal.textColor = Color.black;
					STYLE_M_WARMCOLOR[i] = new GUIStyle( adapter.STYLE_LABEL_8_right );
					STYLE_M_WARMCOLOR[i].normal.textColor = labelWarningColor;
					STYLE_M_NORMALCOLOR[i] = new GUIStyle( adapter.STYLE_LABEL_8_right );
				}
				
				STYLE_M_BLACKCOLOR[1].fontStyle = FontStyle.Bold;
				STYLE_M_WARMCOLOR[1].fontStyle = FontStyle.Bold;
				STYLE_M_NORMALCOLOR[1].fontStyle = FontStyle.Bold;
			}
			
			var BOLD = content.text.EndsWith( "M" );
			var _SI = BOLD ? 1 : 0;
			//if ( content.text.EndsWith( "M" ) ) L.fontStyle = FontStyle.Bold;
			var hasContent = true;
			
			if ( content.text[0] == '!' )
			{	Draw_Label( new Rect( drawRect.x + 0.5f, drawRect.y, drawRect.width, drawRect.height ), content, STYLE_M_BLACKCOLOR[_SI ], true );
				Draw_Label( drawRect, content, STYLE_M_WARMCOLOR[_SI ], true);
			}
			
			else
			{	var enableOverride = !needhide;
				enableOverride = true;
				var USE_ALIGN__ = content.text == "-";
				
				if ( USE_ALIGN__ )
				{	//  L.alignment = __Align;
					hasContent = false;
				}
				
				if ( EditorGUIUtility.isProSkin && GUI.enabled )
				{	Draw_Label( new Rect( drawRect.x + 1, drawRect.y, drawRect.width, drawRect.height ), content, STYLE_M_BLACKCOLOR[_SI], true, ADDITIONAL_ENABLE: enableOverride );
				}
				
				Draw_Label( drawRect, content, STYLE_M_NORMALCOLOR[_SI], true, ADDITIONAL_ENABLE: enableOverride );
			}
			
			
			
			/* Adapter.GET_SKIN().label.fontSize = oldl;
			 Adapter.GET_SKIN().label.fontStyle = oldss;
			 Adapter.GET_SKIN().label.alignment = olda;*/
			
			drawRect.y -= 2;
			
			Draw_ModuleButton( drawRect, content, BUTTON_ACTION_HASH, hasContent, dsc );
			
			
			END_DRAW( _o );
			return width;
		}
		// interna struct
		GUIStyle[] STYLE_M_BLACKCOLOR, STYLE_M_WARMCOLOR, STYLE_M_NORMALCOLOR;
		
		
		Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null;
		Adapter.DrawStackMethodsWrapper BUTTON_ACTION_HASH { get { return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION )); } }
		void BUTTON_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
		{	var o = _o.go;
			var dsc = (M_VerticesHelper)data.args;
#pragma warning disable
			var content = data.content;
#pragma warning restore
			
			if ( Event.current.button == 0 )
			{	var menu = new GenericMenu();
			
				menu.AddItem( new GUIContent( "Use Broadcast To Children" ), adapter.par.BroadCastOptimizer, () =>
				{	Clear();
					adapter.par.BroadCastOptimizer = !adapter.par.BroadCastOptimizer;
					adapter.SavePrefs();
					headOverrideTexture = adapter.par.BroadCastOptimizer ? adapter.redTTexure : (Color? )null;
					
					ResetStack(  );
					//   adapter.RESET_DRAW_STACKS();
					adapter.RepaintWindow();
				} );
				
				menu.AddSeparator( "" );
				
				VerticesModuleTypeEnum currentType = adapter.par.VerticesModuleType;
				
				foreach ( var type in (VerticesModuleTypeEnum[])Enum.GetValues( typeof( VerticesModuleTypeEnum ) ) )
				{	var targetType = type;
					var c = new GUIContent();
					c.text = nameRight( type, dsc );
					
					menu.AddItem( c, type == currentType, () =>
					{	Clear();
					
						adapter.par.VerticesModuleType = targetType;
						adapter.SavePrefs();
						ResetStack(  );
						//adapter.RESET_DRAW_STACKS();
						adapter.RepaintWindow();
					} );
				}
				
				menu.ShowAsContext();
				Adapter.EventUse();
			}
			
			
			if ( Event.current.button == 1 && content.text != "error" )
			{	Adapter.EventUse();
			
				/*  int[] contentCost = new int[0];
				  GameObject[] obs = new GameObject[0];*/
				// if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeaderFillter(out obs, out contentCost, md[o.GetInstanceID()].sharedMesh.vertexCount);
				//   Debug.Log( content.text );
				if ( content.text == "-" )
				{	var result = CallHeader();
				
					if ( result != null )     // FillterData.Init(Event.current.mousePosition, SearchHelper, Hierarchy.par.VerticesModuleType + " All", obs, contentCost, null, this);
					{	var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
						_W__SearchWindow.Init( mp, SearchHelper, Hierarchy.HierarchyAdapterInstance.par.VerticesModuleType + " All",
						                       result, this, adapter, _o );
					}
					
					/*  } else
					  {
					     // FillterData.Init(Event.current.mousePosition, SearchHelper, Hierarchy.par.VerticesModuleType + " All", obs, contentCost, null, this);
					
					         FillterData.Init(Event.current.mousePosition, SearchHelper, Hierarchy.par.VerticesModuleType + " All",
					              CallHeaderFiltered(calc.memory), this);
					  }*/
				}
				
				else
				{	if ( !dsc.BroadcastingInitializeAllObjects )     // var pos = InputData.WidnwoRect(!callFromExternal(), Event.current.mousePosition, 128, 68, adapter );
					{	var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_128_68, !callFromExternal(), adapter);
						_W__InputWindow.Init( pos, "", adapter, null, null, "Waiting for refresh..." );
					}
					
					else
					{	var calc = calcValue( dsc, o );
					
						if ( !calc.instance )     // var pos = InputData.WidnwoRect(!callFromExternal(), Event.current.mousePosition, 128, 68, adapter );
						{	// InputData.Init(pos, "", null, null, "hasn't own value");
							var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_128_68, !callFromExternal(), adapter);
							_W__InputWindow.Init( pos, "", adapter, null, null, "Hasn't value, click to one of the children" );
							
						}
						
						else
							if ( Validate( o ) )
							{	/* if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeaderFillter(out obs, out contentCost, calc.memory);
							
								 FillterData.Init(Event.current.mousePosition, SearchHelper, Hierarchy.par.VerticesModuleType + " " + content.text, obs, contentCost, null, this);*/
								
								var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
								_W__SearchWindow.Init( mp, SearchHelper, Hierarchy.HierarchyAdapterInstance.par.VerticesModuleType + " " + content.text,
								                       CallHeaderFiltered( calc.memory ), this, adapter, _o );
							}
					}
				}
				
				
				
				// EditorGUIUtility.ic
			}
			
			// Undo.RecordObject(o, "GameObject Lock");
			
			// EditorGUIUtility.ExitGUI();
		}
		
		private bool Validate( GameObject o )
		{	return Allow( o );
			/* if (!md.ContainsKey(o.GetInstanceID())) return false;
			 if (md[o.GetInstanceID()] == null) return false;
			 var mf = md[o.GetInstanceID()];
			 if (mf.sharedMesh == null) return false;
			 return true;*/
		}
		
		
		/* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
		              Validate(o) ?
		              CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
		              CallHeader(),
		              this);*/
		/** CALL HEADER */
		_W__SearchWindow.FillterData_Inputs m_CallHeader()
		{	var dsc = GetDescript();
		
			var result = new _W__SearchWindow.FillterData_Inputs( callFromExternal_objects )
			{	Valudator = null,
				    SelectCompareString = ( o, i ) => ( adapter.par.BroadCastOptimizer ? calcValue( dsc, o.go ) : __fakeCalc(dsc, o.go)).memory.ToString(),
				    SelectCompareCostInt = ( o, i ) => (int)Math.Min( ( adapter.par.BroadCastOptimizer ? calcValue( dsc, o.go ) : __fakeCalc(dsc, o.go)).memory, int.MaxValue )
			};
			return result;
		}
		
		internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered( long fillter )
		{	var result = CallHeader();
		
			Func<M_VerticesHelper, GameObject, M_VerticesHelper.MemoryData> calcAction = adapter.par.BroadCastOptimizer ? calcValue :
			        (Func<M_VerticesHelper, GameObject, M_VerticesHelper.MemoryData>)__fakeCalc;
			        
			var dsc = GetDescript();
			
			result.Valudator = o =>
			{	try
				{	var restul = Validate( o.go );
				
					if ( restul )
					{	var c = calcAction( dsc, o.go );
					
						if ( !c.instance ) return false;
						
						var calc = c.memory;
						restul = calc != 0 && calc == fillter;
					}
					
					return restul;
				}
				
				catch ( Exception ex )
				{	Debug.LogError( ex.Message + "\n\n" + ex.StackTrace );
					return false;
				}
			};
			
			return result;
		}
		/** CALL HEADER */
		
		
		internal override _W__SearchWindow.FillterData_Inputs CallHeader()     //  var s = EditorSceneManager.GetActiveScene();
		{	var dsc = GetDescript();
		
			if ( !dsc.BroadcastingInitializeAllObjects )     //var pos = InputData.WidnwoRect( !callFromExternal(), Event.current.mousePosition, 128, 68, adapter );
			{	var pos = new MousePos( Event.current.mousePosition, MousePos.Type.Input_128_68, !callFromExternal(), adapter);
				_W__InputWindow.Init( pos, "", adapter, null, null, "wait for refresh" );
				return null;
			}
			
			
			var result = m_CallHeader();
			
			result.Valudator = o =>
			{	try
				{	if ( !Validate( o.go ) ) return false;
				
					var c = adapter.par.BroadCastOptimizer ? calcValue( dsc, o.go ) : __fakeCalc(dsc, o.go);
					
					if ( !c.instance ) return false;
					
					//  return calcAction( dsc, o.go ).memory != 0;
					return c.memory != 0;
				}
				
				catch     /*(Exception ex)*/
				{	//Debug.LogError(ex.Message + "\n" + o.go.name + " " + o.go.GetInstanceID() + "\n" + ex.StackTrace);
					return false;
				}
			};
			
			return result;
			
			/*   result.SelectCompareString = (d, i) => calcAction(dsc, d).memory.ToString();
			   result.SelectCompareCostInt = (d, i) => calcAction(dsc, d).memory;
			       .OrderBy(d => d.name)
			       .Select((d, i) => new { d.startIndex, cost = i })
			       .OrderBy(d => d.startIndex)
			       .Select(d => d.cost).ToArray();
			   return true;*/
		}
		
		/*  internal void CallHeaderFillter(out GameObject[] obs, out int[] contentCost, long fillter)
		  {
		      //   obs = Utilites.AllSceneObjects().Where(d => Validate(d) && calcValue(d) == fillter).ToArray();
		      //  var s = EditorSceneManager.GetActiveScene();
		      Func<M_VerticesHelper, GameObject, M_VerticesHelper.MemoryData> calcAction = par.BroadCastOptimizer ? calcValue :
		          (Func<M_VerticesHelper, GameObject, M_VerticesHelper.MemoryData>)__fakeCalc;
		
		      var dsc = GetDescript();
		      obs = Utilities.AllSceneObjects().Where(o => {
		          try
		          {
		              var restul = Validate(o);
		              if (restul)
		              {
		                  var c = calcAction(dsc, o);
		                  if (!c.instance) return false;
		                  var calc = c.memory;
		                  restul = calc != 0 && calc == fillter;
		              }
		              return restul;
		          }
		          catch
		          {
		              return false;
		          }
		      }).ToArray();
		
		      contentCost = obs.Select((d, i) => new { name = calcAction(dsc, d).memory, startIndex = i }).OrderBy(d => d.name).Select((d, i) => new { d.startIndex, cost = i }).OrderBy(d => d.startIndex).Select(d => d.cost).ToArray();
		  }*/
	}
}
}
