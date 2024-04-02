using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
//namespace EModules

namespace EModules.EModulesInternal

{
internal partial class Hierarchy {
    class M_Audio : Adapter.Module {
        /*  internal override bool enableOverride()
          {
              return !Adapter.LITE;
          }*/
        //  internal override string enableOverrideMessage() { return " (Pro Only)"; }
        
        public M_Audio( int restWidth, int sibbildPos, bool enable, Adapter adapter ) : base( restWidth, sibbildPos, enable, adapter )
        {
        
        
        
            if ( adapter.NEW_PERFOMANCE )
            {   adapter. SupscribeToHierarchyChanged( Clear );
            }
            else
            {   adapter.onSelectionChanged -= Clear;
                adapter.onSelectionChanged += Clear;
            }
            
        }
        internal static AudioSource currentPlay;
        static GUIContent content = new GUIContent();
        Dictionary<int, AudioSource> audio_cache = new Dictionary<int, AudioSource>();
        void Clear()
        {   audio_cache.Clear();
        }
        
        static Action playingAct;
        static void Updater()
        {   if ( playingAct  != null )
            {   playingAct();
            }
        }
        
        internal  void PlayAudio( AudioSource aud )
        {   if ( aud.isPlaying )
            {   aud.Stop();
                if ( playingAct != null )
                {   playingAct = null;
                    EditorApplication.update -= Updater;
                }
            }
            else if ( aud.clip != null )
            {   if ( !aud.enabled )
                {   Debug.LogWarning( "Can not play a disabled audio source\nUnityEngine.AudioSource:Play()" );
                }
                else
                {   if ( currentPlay != null && currentPlay.isPlaying ) currentPlay.Stop();
                    currentPlay = null;
                    aud.Play();
                    currentPlay = aud;
                    
                    var capturePlay = currentPlay;
                    EditorApplication.update -= Updater;
                    EditorApplication.update += Updater;
                    playingAct = () =>
                    {   if ( capturePlay  == null || !currentPlay.isPlaying )
                        {   playingAct = null;
                            ResetStack();
                            adapter.RepaintWindowInUpdate();
                            EditorApplication.update -= Updater;
                        }
                    };
                }
            }
        }
        
        internal override float Draw( Rect drawRect, Adapter.HierarchyObject _o )
        {
        
        
            if ( !START_DRAW( drawRect, _o ) ) return 0;
            
            var o = _o.go;
            
            if ( !audio_cache.ContainsKey( o.GetInstanceID() ) ) audio_cache.Add( o.GetInstanceID(), o.GetComponent<AudioSource>() );
            // if (audio_cache[o.GetInstanceID()] == null) return width;
            var aud = audio_cache[o.GetInstanceID()];
            if ( aud == null )
            {   END_DRAW( _o );
                return width;
            }
            
            var oldW = drawRect.width;
            var oldH = drawRect.height;
            drawRect.width = drawRect.height = 12;
            drawRect.x += (oldW - drawRect.width) / 2;
            drawRect.y += (oldH - drawRect.width) / 2;
            // Adapter.DrawTexture( drawRect, adapter.GetIcon( aud.clip == null ? "AUDIOPLAYLOCK" : aud.isPlaying ? "AUDIOSTOP" : "AUDIOPLAY" ) );
            Draw_AdapterTexture( drawRect, adapter.GetIcon( aud.clip == null ? "AUDIOPLAYLOCK" : aud.isPlaying ? "AUDIOSTOP" : "AUDIOPLAY" ), USE_GO: true );
            
            //if ( !o.activeInHierarchy ) Adapter.FadeRect( drawRect );
            /* var r = drawRect;
             r.width = 100;
             r.height = EditorGUIUtility.singleLineHeight;
             EditorGUI.HelpBox(r, "GameObject not active", MessageType.Warning);*/
            drawRect.y -= 2;
            content.tooltip = aud.isPlaying ? "Stop AudioClip" : "Play AudioClip";
            if ( aud.playOnAwake ) content.tooltip += "\n (PlayOnAwake Enable)";
            if ( aud.loop ) content.tooltip += "\n (Loop Enable)";
            
            
            /* if ( adapter.ModuleButton( drawRect, content, true ) )
             {
             }*/
            str.aud = aud;
            Draw_ModuleButton( drawRect, content, BUTTON_ACTION_HASH, true, str );
            
            if ( currentPlay != null && currentPlay.isPlaying )
            {   adapter.RepaintWindowInUpdate();
            }
            
            
            END_DRAW( _o );
            return width;
        }
        argsS str;
        struct argsS
        {   internal AudioSource aud;
        }
        
        Adapter. DrawStackMethodsWrapper __BUTTON_ACTION_HASH = null;
        Adapter.DrawStackMethodsWrapper BUTTON_ACTION_HASH { get { return __BUTTON_ACTION_HASH ?? (__BUTTON_ACTION_HASH = new Adapter.DrawStackMethodsWrapper( BUTTON_ACTION )); } }
        void BUTTON_ACTION( Rect worldOffset, Rect inputRect, Adapter.DrawStackMethodsWrapperData data, Adapter.HierarchyObject _o )
        {   audio_cache[_o.id] = _o.go.GetComponent<AudioSource>();
        
            var str = (argsS)data.args;
            var aud = str. aud;
            if ( Event.current.button == 0 && _o.go.activeInHierarchy )
            {   PlayAudio( aud );
            }
            
            if ( Event.current.button == 1 )
            {   Adapter.EventUse();
                var mp = new MousePos( Event.current.mousePosition, MousePos.Type.Search_356_0, !callFromExternal(), adapter);
                
                _W__SearchWindow.Init( mp, SearchHelper, typeof( AudioSource ).Name,
                                       Validate( _o ) ?
                                       CallHeaderFiltered( audio_cache[_o.id].clip ) :
                                       CallHeader(),
                                       this, adapter, _o );
                                       
                                       
                /*  int[] contentCost = new int[0];
                  GameObject[] obs = new GameObject[0];
                  if (EditorSceneManager.GetActiveScene().rootCount != 0) CallHeader(out obs, out contentCost);
                
                
                  FillterData.Init(Event.current.mousePosition, SearchHelper, typeof(AudioSource).Name, obs, contentCost, null, this);*/
            }
            ResetStack();
        }
        
        bool Validate( Adapter.HierarchyObject _o )
        {   var o = _o.go;
            var res = o.GetComponent<AudioSource>();
            if ( res )
            {   if ( !audio_cache.ContainsKey( o.GetInstanceID() ) ) audio_cache.Add( o.GetInstanceID(), o.GetComponent<AudioSource>() );
                else audio_cache[o.GetInstanceID()] = res;
            }
            return res != null;
        }
        int ToContentCost( Adapter.HierarchyObject o, int i )
        {   var aud = o.go.GetComponent<AudioSource>();
            var cost = i;
            if ( aud.clip == null ) cost += 10000;
            if ( !o.go.activeInHierarchy ) cost += 1000000;
            return cost;
        }
        
        
        
        /* FillterData.Init(Event.current.mousePosition, SearchHelper, LayerMask.LayerToName(o.layer),
                     Validate(o) ?
                     CallHeaderFiltered(LayerMask.LayerToName(o.layer)) :
                     CallHeader(),
                     this);*/
        /** CALL HEADER */
        internal override _W__SearchWindow.FillterData_Inputs CallHeader()
        {   var result = new _W__SearchWindow.FillterData_Inputs(callFromExternal_objects)
            {   Valudator = Validate,
                    SelectCompareString = (d, i) => audio_cache[d.go.GetInstanceID()] && audio_cache[d.go.GetInstanceID()].clip != null ? audio_cache[d.go.GetInstanceID()].clip.name : "",
                    SelectCompareCostInt = ToContentCost
            };
            return result;
        }
        
        internal _W__SearchWindow.FillterData_Inputs CallHeaderFiltered( AudioClip filter )
        {   var result = CallHeader();
            result.Valudator = s => Validate( s ) && audio_cache[s.go.GetInstanceID()].clip == filter;
            return result;
        }
        /** CALL HEADER */
        
        
        
        /*   internal override bool CallHeader(out GameObject[] obs, out int[] contentCost)
           {
               obs = Utilities.AllSceneObjects().Where(Validate).ToArray();
               contentCost = obs.Select(ToContentCost).ToArray();
               return true;
           }*/
    }
    
    
    internal static int INT_COMPARE( string str )
    {   return String.Compare( str, "", StringComparison.Ordinal );
    }
}

}

