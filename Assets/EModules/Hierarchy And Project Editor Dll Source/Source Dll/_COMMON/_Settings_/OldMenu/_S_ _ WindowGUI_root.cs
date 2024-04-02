#if UNITY_EDITOR
    #define HIERARCHY
    #define PROJECT
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
#if PROJECT
    using EModules.Project;
#endif
//namespace EModules


namespace EModules.EModulesInternal {





[CustomEditor( typeof( Hierarchy_GUI ) )]
public class Hierarchy_Internal : Editor {

    public override void OnInspectorGUI()
    {   var setup = target.name.StartsWith("Hierarchy") ? (SETUPROOT)HIER_SETUP.module : PROJ_SETUP.module;
        if ( setup == null ) return;
        // serializedObject.Update();
        
        //   var myAsset = serializedObject.FindProperty("MemberAsset");
        //  CreateCachedEditor(myAsset.objectReferenceValue, null, ref _editor);
        //  _editor.OnInspectorGUI();
        
        //serializedObject.ApplyModifiedProperties();
        //DrawDefaultInspector();
        /*    if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Application.isPlaying", MessageType.Warning);
                return;
            }*/
        
        GUILayout.Label( setup.NAME + " plugin" );
        //  GUILayout.Label( "Hierarchy Plugin" );
        GUILayout.Label( "- Window/" + setup.NAME + " Plugin/Settings" );
        if ( GUILayout.Button( "Open Plugin Settings" ) )
        {
        
        
            #if HIERARCHY
            if ( setup.A.IS_HIERARCHY() ) Adapter.SHOW_HIER_SETTINGS_DEFAULT( 0 );
            #endif
            #if PROJECT
            if ( setup.A.IS_PROJECT() ) Adapter.SHOW_HIER_SETTINGS_DEFAULT( 1 );
            #endif
        }
        
        /* if (!Hierarchy_GUI.Initialize( setup.A ))
         {
             GUILayout.Label( "Please Reinstall Plugin" );
             return;
         }
         setup.DrawHeader( setup.NAME + " plugin" );
         setup.drawGUIcaller( false, setup );*/
        //SETUPROOT.drawGUIcaller( false );
        
    }
}


internal class HIER_SETUP : SETUPROOT {
    public override string NAME { get { return Initializator.HIERARCHY_NAME; } }
    public override string VERSION { get { return Adapter.HIERARCHY_VERSION; } }
    static HIER_SETUP m_module;
    
    internal static HIER_SETUP module
    {   get
        {   if ( !Initializator.AdaptersByName.ContainsKey( Initializator.HIERARCHY_NAME ) ) return null;
            return m_module ?? (m_module = new HIER_SETUP( Initializator.AdaptersByName[Initializator.HIERARCHY_NAME], Initializator.HIERARCHY_NAME ));
        }
    }
    
    internal HIER_SETUP( Adapter A, string AdapterName ) : base( A, AdapterName )
    {
    }
}
internal class PROJ_SETUP : SETUPROOT {
    public override string NAME { get { return Initializator.PROJECT_NAME; } }
    public override string VERSION { get { return Adapter.HIERARCHY_VERSION; } }
    static PROJ_SETUP m_module;
    
    internal static PROJ_SETUP module
    {   get
        {   if ( !Initializator.AdaptersByName.ContainsKey( Initializator.PROJECT_NAME ) ) return null;
            return m_module ?? (m_module = new PROJ_SETUP( Initializator.AdaptersByName[Initializator.PROJECT_NAME], Initializator.PROJECT_NAME ));
        }
    }
    
    internal PROJ_SETUP( Adapter A, string AdapterName ) : base( A, AdapterName )
    {
    }
}


internal partial class SETUPROOT {
    public virtual string NAME { get; set; }
    public virtual string VERSION { get; set; }
    internal string Aname;
    internal Adapter A;
    
    
    internal SETUPROOT( Adapter A, string AdapterName )
    {   this.A = A;
    
        this.Aname = AdapterName;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    EditorWindow win;
    
    Rect BOTTOM = new Rect(),
    RIGHT = new Rect(),
    LEFT = new Rect();
    // bool TreeWarning = false;
    //  bool? TreeChange = null;
    
    
    
    float calcedHeight = 0;
    
    
#pragma warning disable
    float calcMAIN = 0;
    float calHELP = 0;
    float calcPLAY = 0;
#pragma warning restore
    
    float calcGEN = 0;
    float calcMM = 0;
    
    //  float calcRIGHT = 0;
    float calcBOTTOM = 0;
    float calcBOTTOM1 = 0;
    float calcBOTTOM2 = 0;
    
    float calcREMOVE = 0;
    float[] calcLEFT = new float[10];
    
    
    
    bool wasInit = false;
    
    float SCH;
    
    
    bool DrawHelpImage = true;
    
    
    
    string HIERARCHY_MENU_HELP = @"
class MyMenu_SelectAllChildren : HierarchyExtensions.IGenericMenu
{
    public string Name { get { return ""Select All Children""; } }
    public int PositionInMenu { get { return 200; } }

    public bool IsEnable(GameObject clickedObject) { return clicedkObject.transform.childCount != 0; }
    public bool NeedExcludeFromMenu(GameObject clickedObject) { return false; }

    public void OnClick(GameObject[] affectedObjectsArray)
    {
        Selection.objects = selectedObjects.SelectMany(s => s.GetComponentsInChildren<Transform>(true)).Select(s => s.gameObject).ToArray();
    }
}";



    string PROJECT_MENU_HELP = @"
    class MyMenu : ProjectExtensions.IGenericMenu
    {
        public string Name { get { return ""MySubItem/MyMenuItem %k""; } }
        public int PositionInMenu { get { return 0; } }

        public bool IsEnable(string clickedObjectPath, string clickedObjectGUID, int instanceId, bool isFolder, bool isMainAsset) { return true; }
        public bool NeedExcludeFromMenu(string clickedObjectPath, string clickedObjectGUID, int instanceId, bool isFolder, bool isMainAsset) { return false; }
    
        public void OnClick(string[] affectedObjectsPathArray, string[] affectedObjectsGUIDArray, int[] affectedObjectsInstanceId, bool[] affectedObjectIsFolder, bool[] isMainAsset)
        {
          throw new System.NotImplementedException();
        }
    }";



    Vector2 sp;
    
    
    
    // whatever concatenation you think looks the best can be used here,
    // since it's guarded...
    
    
    Vector2 scrollPos;
    
    
    
    
    //  GUIContent content = new GUIContent();
    
    
    internal Color redTTexure
    {   get
        {   return A.redTTexure;
        }
    }
    
    
    
    DoubleList<string, Hierarchy_GUI.CustomIconParams> customIcons { get { return Hierarchy_GUI.Get( A ); } }
    
    
    
    void Repaint( bool updateHeight = false )     //  Hierarchy.RepaintAllViews();
    {   // A.bottomInterface.HEIGHT_FUNCTIUON( A.window(), null );
    
        if ( updateHeight && A.m_UseExpansionAnimation != null )
        {   var treeView = A.m_TreeView(A.window());
            A.m_UseExpansionAnimation.SetValue( treeView, true );
            A.bottomInterface.NEED_READ_LIST.Clear();
        }
        
        
        if ( EditorWindow.focusedWindow != null ) EditorWindow.focusedWindow.Repaint();
        A.RepaintWindowInUpdate();
        //A.
        foreach ( var w in A.bottomInterface.WindowController )
        {   if ( w.REFERENCE_WINDOW ) w.REFERENCE_WINDOW.Repaint();
        }
        foreach ( var w in A.bottomInterface.FavoritControllers )
        {   if ( w.REFERENCE_WINDOW ) w.REFERENCE_WINDOW.Repaint();
        }
        
        if ( A.bottomInterface.hyperGraph.editorWindow ) A.bottomInterface.hyperGraph.editorWindow.Repaint();
        
        /* A.NeedApplyMod = true;
         A.Mody += () =>
         {
             A.INIT_IF_NEDDED();
         };*/
    }
    
    
    void BeginHorizontal() { }
    void EndHorizontal() { }
    
    
    
    void OnEnableChange()
    {   A.RepaintAllViews();
        A.NeedBottomPositionUpdate = true;
        A.tempAdapterBlock = false;
        A.RepaintAllViews();
        // InternalEditorUtility.RepaintAllViews();
        if ( !A.par.ENABLE_ALL )
        {   A.OnDisablePlugin();
        }
        else
        {   if ( A.parLINE_HEIGHT != EditorGUIUtility.singleLineHeight ) A.ResetScroll();
        }
        Adapter.RequestScriptReload();
    }
    
    internal bool TOGGLE_LEFT( string title, bool value )
    {   var rect = GetControlRect(EditorGUIUtility.singleLineHeight);
        return A.TOGGLE_LEFT( rect, title, value );
    }
    
    internal float DRAW_MAIN( bool inspector )     // GUI.matrix = Matrix4x4.Scale(new Vector3(0.8f, 1f, 1));
    {
    
    
        X = 0;
        Y = 0;
        
        
        if ( inspector )     //if (Event.current.type == EventType.Repaint) A.SETUP_BOXCAT.Draw(new Rect(0, 0, W + 200, 100), "", false, false, false, false);
        {   // if (Event.current.type == EventType.repaint) SETUP_NOISE.Draw(new Rect(0, 0, W + 200, 110), "", false, false, false, false);
            // if (Event.current.type == EventType.repaint) SETUP_TITLEHOGLIGHT.Draw(new Rect(0, 0, W + 200, 110), "", false, false, false, false);
#pragma warning disable
            
            var logo = Adapter.LITE ? A.GetIcon("LOGO LITE") : A.GetIcon("LOGO");
            if ( A.IS_PROJECT() ) logo = A.GetIcon( "LOGO PROJECT" );
#pragma warning restore
            
            GUI.DrawTexture( new Rect( 10, 0, 384, logo.height ), logo );
            var asd = GUI.color;
            GUI.color *= new Color( 1, 1, 1, 0.3f );
            GUI.Label( new Rect( 384 - 60, 5, 70, EditorGUIUtility.singleLineHeight ), VERSION );
            GUI.color = asd;
            // GUI.DrawTexture(new Rect(20, 0, 384, 110), Hierarchy.GetIcon("LOGO"));
            
            
            //  GetControlRect((inspector ? 20 : 110));
            GetControlRect( logo.height );
        }
        else
        {   GetControlRect( 10 );
        }
        
        
        UpdateHi();
        Space( 10 );
        
        
        
        var R = GetControlRect((84));
        
        var PDD = 8;
        if ( Event.current.type == EventType.Repaint )
        {   var sbr = R;
            sbr.y -= PDD;
            sbr.height += PDD * 2;
            var c = GUI.color;
            if ( !EditorGUIUtility.isProSkin )
            {   var cc = GUI.color;
                cc.a = 0.3f;
                GUI.color = cc;
            }
            A.SETUP_BUTTON.Draw( sbr, "", false, false, false, false );
            GUI.color = c;
        }
        R.x += PDD;
        var w = R.width + 8;
        var OFFSET = 0.78f;
#pragma warning disable
        
        //  if (Adapter.LITE || A.IS_PROJECT())
        OFFSET = 0.55f;
#pragma warning restore
        R.width = R.width * OFFSET;
        
        /*if (A.IS_HIERARCHY())*/
        Draw_HIERERCHY_YouTube( R );
        /* else GUI.Label( R, VERSION );*/
        
        R.x += R.width + 5;
        R.width = w - R.x;
        R.height = 41 * 2;
        EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
        /* if (GUI.Button(R, new GUIContent("ASSETSTORE", null, "http://www.u3d.as/PSP"), SETUP_BUTTON))
         {
             Application.OpenURL("http://www.u3d.as/PSP");
         }*/
        
        var style = EditorStyles.miniButtonRight;
        var tt = style.border.top;
        var bb = style.border.bottom;
        if ( !EditorGUIUtility.isProSkin )
        {   style.border.top = 5;
            style.border.bottom = 5;
        }
        if ( GUI.Button( R, new GUIContent( "Documentation\n\nwww.emem.store/wiki", null, "http://emem.store/wiki?=..." ),
                         EditorStyles.miniButtonRight ) )
        {   if ( A.pluginID == 0 )
                Application.OpenURL( "http://emem.store/wiki?=Hierarchy&Left%20Panel&Highlighter%20Window" );
            else
                Application.OpenURL( " http://emem.store/wiki?=Project&Bottom%20Panel&BookMarks%20Manager" );
        }
        style.border.top = tt;
        style.border.bottom = bb;
        // R = GetControlRect(false, GUILayout.Height(42));
        //R.x += R.width;
        R.y += R.height + 3;
        EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
        
        /* var asd11 = GUI.color;
         var c2 = asd11;
         c2.a = 0.5f;
         GUI.color = c2;
         style = EditorStyles.miniButtonRight;
         tt = style.border.top;
         bb = style.border.bottom;
         if (!EditorGUIUtility.isProSkin)
         {   style.border.top = 5;
             style.border.bottom = 5;
         }
         if (GUI.Button( R, new GUIContent( "contact", null, "http://emem.store/contact" ),
                         EditorStyles.miniButtonRight ))
         {   Application.OpenURL( "http://emem.store/contact" );
         }
         GUI.color = asd11;
         style.border.top = tt;
         style.border.bottom = bb;*/
        
        
        //   Space(28);
        var START_Y = GetControlRect(0).y + 20;
        
        
        
        
        
        
        /*  R = GetControlRect(16);
          DrawHelpImage = A.TOGGLE_LEFT(R, "Show Help", DrawHelpImage);*/
        
        
        
        /**   MAIN */
        /**   MAIN */
        /**   MAIN */
        /**   MAIN */
        
        bool updateHeight = false;
        
        
        var max = 0f;
        var next = 0f;
        
        Y = EditorGUIUtility.singleLineHeight;
        X = W;
        
        
        R = GetControlRect( EditorGUIUtility.singleLineHeight );
        var oldval = A.TOGGLE_LEFT(R, "☰ Use " + A.pluginname + " Plugin ☰", A.par.ENABLE_ALL, defaultStyle: null);
        if ( oldval != A.par.ENABLE_ALL )
        {   A.par.ENABLE_ALL = oldval;
            OnEnableChange();
        }
        Space( 10 );
        
        var gg = GUI.enabled;
        GUI.enabled &= A.par.ENABLE_ALL;
        //  updateHeight = DRAW_MAIN_TOGGLES( Y, W, START_Y + 66 );
        DRAW_CATEGORIES_BUTS_V200( new Rect( X, Y, W * 2, START_Y + 66 ) );
        GUI.enabled = gg;
        
        
        
        
        Y = START_Y;
        X = 0;
        
        
        
        //DRAW_CATEGORIES_BUTS_VERTICAL();
        next = GetControlRect( 0 ).y;
        if ( next > max ) max = next;
        
        //  START_Y += 20 ;
        
        Y = START_Y;
        X = 0;
        /*LEFT.y = GetLastRect().y + GetLastRect().height;
        DOCK_LEFT();
        LEFT.height = GetLastRect().y + GetLastRect().height - LEFT.y;
        next = GetControlRect(0).y;
        if (next > max) max = next;*/
        
        
        /*
        
        DRAW_CATEGORIES_BUTS();*/
        
        NEW_DISPLAY( START_Y, ref max );
        // OLD_MENU(START_Y, max);
        
        /*  DRAW_CATEGORIES_BUTS();
        
          */
        
        
        
        /*   Y = 0;
           X = W*2;
           BOTTOM.y = GetLastRect().y + GetLastRect().height;
           DOCK_BOTTOM();
           BOTTOM.height = GetLastRect().y + GetLastRect().height - BOTTOM.y;
           next = GetControlRect(0).y;
           if (next > max) max = next;
        
           Y = 0;
           X = W*3;
           RIGHT.y = GetLastRect().y + GetLastRect().height;
           DOCK_RIGHT();
           RIGHT.height = GetLastRect().y + GetLastRect().height - RIGHT.y;
           next = GetControlRect(0).y;
           if (next > max) max = next;
        
           Y = 0;
           X = W*4;
           DRAW_CACHE();
           Space(15);
           DrawRemove();
           next = GetControlRect(0).y;
           if (next > max) max = next;*/
        
        
        
        LEFT.width = RIGHT.width = BOTTOM.width = EditorGUIUtility.singleLineHeight;
        
        
        if ( GUI.changed )
        {   A.SavePrefs();
            Repaint( updateHeight );
        }
        GUI.enabled = A.par.ENABLE_ALL;
        
        return max;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    Color32[] colors = new[] {new Color32(244, 67, 54, 127), new Color32(25, 118, 210, 127), new Color32(76, 175, 80, 127), new Color32(255, 158, 34, 127), new Color32(161, 136, 127, 127)};
    
    
    void DRAW_CATEGORIES_BUTS_V200( Rect rect )
    {
    
        GUI.enabled = A.par.ENABLE_ALL;
        
        
        
        
        var r = GetControlRect(50);
        var startYY = r.y;
        
        // List<string> additional_temp = new List<string>();
#pragma warning disable
        /*    if (!Adapter.LITE) additional_temp.Add("Custom Generic Menu - Other");
            else additional_temp.Add(null);
            if (!Adapter.LITE) additional_temp.Add("Search Box - Other");
            else additional_temp.Add(null);*/
        /*  if (A.IS_HIERARCHY()) additional_temp.Add( "Cache / Export Settings / GIT - Other" );
          else additional_temp.Add( "Export Settings / GIT - Other" );←→↓*/
        
        var names = new[] { "MAIN SETTINGS", "LEFT ◀BAR◀", "BOTTOM ▼BAR▼", "RIGHT ▶BAR▶",/* "SEARCH WINDOW", "CUSTOM MENU", */ "☰CACHE☰ UNINSTALL" } .Select(s => s.Replace(" ",
                "\n\n")).ToArray();
                
        /* string[]  cats = names;
         var ov = EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item");
         ov = Mathf.Clamp(ov, 0, cats.Length - 1);
         var nv = GUI.Toolbar(new Rect(r.x, r.y, r.width * 2, r.height * 4),   ov, cats, EditorStyles.toolbarButton);
         if (ov != nv)
         {   EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item", nv);
         }
         return;*/
        
        
        /*var categories = new string[][] {new string[0], new string[0], A.IS_HIERARCHY() && Adapter.LITE ? new string[0] : new[]{  A.IS_HIERARCHY() ? "HyperGraph - Extra" : "BookMarks Manager - Extra" },
            //new[] { "Extra - PlayMode Data Keeper", "Extra - Functions and Vars displaying", "Extra - Memory Optimizer", "Extra - Custom Icons" } ,
            A.IS_HIERARCHY() ?   new[] { "PlayMode Data Keeper - Extra", "Functions and Vars displaying - Extra", "Memory Optimizer - Extra", "Custom Icons - Extra" }
            : new[] { null, null, "Memory Optimizer - Extra", null }
        
            ,
            additional_temp.ToArray(),
            new string[0]
        };*/
#pragma warning restore
        var L = names.Length;
        Rect RECT_R;
        // Rect[] buttons = new Rect[names.Length];
        r.width *= 0.4f;
        r.height *= 1.5f;
        // var curS = EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 );
        for ( int i = 0 ; i < L ; i++ )
        {   var R = r;
            //  if ( i >= 4 ) R = new Rect(R.x + W, R.y - r.height * 4, R.width, R.height);
            //   var active = curS == i;
            if ( Event.current.type == EventType.Repaint )     //var label = "●●●";
            {
            
                ///  EditorStyles.toolbarButton.Draw(R, label, active, active, false, active);
                // EditorStyles.toolbarButton.Draw(new Rect(R.x, R.y + EditorGUIUtility.singleLineHeight + R.height, R.width, EditorGUIUtility.singleLineHeight), label, active, active, false, active);
            }
            R.y += EditorGUIUtility.singleLineHeight;
            R.height += EditorGUIUtility.singleLineHeight;
            EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
            
            if ( Event.current.type == EventType.Repaint )
            {
            
                if ( EditorGUIUtility.isProSkin )
                    GUI.skin.box.Draw( SHRINK( R, -3 ), "", false, false, false, false );
                else
                    A.SETUP_BOXCAT.Draw( SHRINK( R, -3 ), "", false, false, false, false );
            }
            if (/*Event.current.type == EventType.Repaint &&*/
                EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 ) == i )
                Adapter.DrawRect( SHRINK( R, EditorGUIUtility.isProSkin ? 10 : 0 ), colors[i] );
            BOX( R );
            
            
            A.ChangeGUI();
            
            var FS = Adapter.GET_SKIN().button.fontSize;
            Adapter.GET_SKIN().button.fontSize = Adapter.GET_SKIN().box.fontSize;
            if ( GUI.Button( R, names[i] ) )
            {   EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item", i );
                EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item/" + i, 0 );
            }
            Adapter.GET_SKIN().button.fontSize = FS;
            
            // if (EditorPrefs.GetInt(A.pluginname + "/" + "Plugin Menu Item", 0) == i)
            /*  {   for (int j = 0 ; j < categories[i].Length ; j++)
                  {   if (categories[i][j] == null) continue;
            
                      var RR = r;
                      RR.height -= EditorGUIUtility.singleLineHeight;
                      //RR.x += 40;
                      //RR.width -= 80;
            
                      EditorGUIUtility.AddCursorRect( RR, MouseCursor.Link );
                      //  BOX(RR, true);
                      var oldAligment = Adapter.GET_SKIN().button.alignment;
                      var oldPad = Adapter.GET_SKIN().button.padding.right;
                      Adapter.GET_SKIN().button.alignment = TextAnchor.MiddleRight;
                      Adapter.GET_SKIN().button.padding.right = 10;
                      FS = Adapter.GET_SKIN().button.fontSize;
                      Adapter.GET_SKIN().button.fontSize = Adapter.GET_SKIN().box.fontSize;
                      if (GUI.Button( RR, categories[i][j] ))
                      {   EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item", i );
                          EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item/" + i, j + 1 );
                      }
                      Adapter.GET_SKIN().button.fontSize = FS;
                      Adapter.GET_SKIN().button.alignment = oldAligment;
                      Adapter.GET_SKIN().button.padding.right = oldPad;
            
                      RECT_R = RR;
                      Adapter.DrawRect( new Rect( r.x + RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color(0, 0, 0) );
            
                      if  EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 ) == i
                              && EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item/" + i, 0 ) == j + 1)
                      {   var c = GUI.color;
                          GUI.color *= new Color( 0.2f, 0.7f, 1f );
                          BOX( RR, false, true );
                          GUI.color = c;
            
                          Adapter.DrawRect( new Rect( r.x + RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color( 0.2f, 0.7f, 1f ) );
            
                      }
            
                      r.y += r.height - EditorGUIUtility.singleLineHeight;
            
                  }
              }*/
            
            
            A.RestoreGUI();
            {   var r2 = r;
                r2.y -= 1;
                //  r2.x -= 4;
                var Rasd = 5;
                var SIZE = 5;
                var _S = SIZE - 2;
                RECT_R = R;
                Adapter.DrawRect( new Rect( r2.x + RECT_R.width / 2 - SIZE / 2 + Rasd, RECT_R.y + RECT_R.height / 2 - SIZE / 2, SIZE, SIZE ), Color.black );
                Adapter.DrawRect( new Rect( r2.x + RECT_R.width / 2 - SIZE / 2 - Rasd, RECT_R.y + RECT_R.height / 2 - SIZE / 2, SIZE, SIZE ), Color.black );
                // r2.x += 1;
                
                if (/*Event.current.type == EventType.Repaint &&*/
                    EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 ) == i )
                {   var c = GUI.color;
                    GUI.color *= new Color( 0.2f, 0.7f, 1f );
                    //  BOX( R, false, true );
                    // GUI.Label(R, names[i]);
                    //style.Draw(R, names[i] /*.ToUpper()*/, true, true, false, true);
                    GUI.color = c;
                    // Adapter.DrawRect(SHRINK(R, 5), colors[i]);
                    
                    Adapter.DrawRect( new Rect( r2.x + RECT_R.width / 2 - _S / 2 + Rasd, RECT_R.y + RECT_R.height / 2 - _S / 2, _S, _S ), new Color( 0.2f, 0.7f, 1f ) );
                    Adapter.DrawRect( new Rect( r2.x + RECT_R.width / 2 - _S / 2 - Rasd, RECT_R.y + RECT_R.height / 2 - _S / 2, _S, _S ), new Color( 0.2f, 0.7f, 1f ) );
                    
                }
                
                
            }
            
            
            if ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 ) != i )   //colors[i].a /= 3;
            {
            
                if ( EditorGUIUtility.isProSkin )
                {   var rrr = SHRINK(R, 10);
                    rrr.height = 3;
                    Adapter.DrawRect( (rrr), colors[i] );
                }
                else
                {   Adapter.DrawRect( SHRINK( R, 5 ), colors[i] );
                
                }
                
                //   Adapter.DrawRect(R, colors[i]);
            }
            
            r.x += r.width + 6;
            
            // R = r;
            /* style.border.top = tt;
             style.border.bottom = bb;*/
            /* if (i != 0 & i != L - 1)
             {
                 R.height -= i == L / 2 ? 10 : 5;
                 R.y +=i== L / 2 ? 10 : 5;
             }*/
        }
        
        Space( r.y - startYY - r.height );
    }
    
    
    
    
    void NEW_DISPLAY( float START_Y, ref float max )
    {
    
        if ( A.par.ENABLE_ALL )
        {   var contentY = START_Y;
            switch ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 ) )
            {   /*case 0:
                        updateHeight = DRAW_MAIN();
                        break;*/
                case 0:
                    DRAW_MAIN_TOGGLES( Y, W, (START_Y + 66) * 2 );
                    X = W;
                    Y += EditorGUIUtility.singleLineHeight * 2;
                    if ( Y > contentY ) contentY = Y;
                    Y = START_Y;
                    DRAW_SEARCHWINDOW( Y, W, ref contentY );
                    X = W * 2;
                    Y += EditorGUIUtility.singleLineHeight * 2;
                    if ( Y > contentY ) contentY = Y;
                    Y = START_Y;
                    DRAW_CUSTOMOBJECTSMENU( Y, W, ref contentY );
                    Y += EditorGUIUtility.singleLineHeight * 2;
                    if ( Y > contentY ) contentY = Y;
                    break;
                    
                case 1:
                    LEFT.y = GetLastRect().y + GetLastRect().height;
                    DOCK_LEFT( START_Y, W, ref contentY );
                    LEFT.height = GetLastRect().y + GetLastRect().height - LEFT.y;
                    break;
                    
                case 2:
                    var wasEn2 = GUI.enabled;
                    
                    BOTTOM.y = GetLastRect().y + GetLastRect().height;
                    DOCK_BOTTOM( START_Y, W, ref contentY );
                    BOTTOM.height = GetLastRect().y + GetLastRect().height - BOTTOM.y;
                    
                    
                    X = W * 2;
                    Y = START_Y;
                    var startCalc = GetControlRect(0).y;
                    BEGIN_CATEGORY( ref calcBOTTOM1, false, null, 5 );
                    // DrawHeader( "Bottom Extra" );
                    GUI.enabled &= A.par.ENABLE_RIGHTDOCK_FIX;
                    if ( A.IS_HIERARCHY() ) DRAW_HIPER( START_Y, W, ref contentY );
                    else DRAW_BOOKMARKSMANAGER( START_Y, W, ref contentY );
                    //  contentY += 20;
                    // calcRIGHT[3] = contentY - startCalc;
                    END_CATEGORY( ref calcBOTTOM1 );
                    
                    GUI.enabled = wasEn2;
                    break;
                    
                case 3:
                    var wasEn = GUI.enabled;
                    RIGHT.y = GetLastRect().y + GetLastRect().height;
                    DOCK_RIGHT( START_Y, W, ref contentY );
                    RIGHT.height = GetLastRect().y + GetLastRect().height - RIGHT.y;
                    GUI.enabled = wasEn;
                    
                    break;
                    
                case 4:
                    /* var wasEn4 = GUI.enabled;
                     var startCalc4 = GetControlRect(0).y;
                     BEGIN_CATEGORY( ref calcRIGHT[3], false, null, 5 );
                     DrawHeader( "Extra options" );
                    
                     GUI.enabled &= A.par.ENABLE_RIGHTDOCK_FIX;
                    
                    
                     contentY += 20;
                     calcRIGHT[3] = contentY - startCalc4;
                     END_CATEGORY( ref calcRIGHT[3] );
                     GUI.enabled = wasEn4;*/
                    
                    ADD_CacheGIT( START_Y, W, ref contentY, false );
                    if ( contentY > max ) max = contentY;
                    
                    X = W * 2;
                    Y = START_Y;
                    
                    DrawRemove();
                    
                    
                    
                    break;
                    
                    
                /* case 5:
                     var wasEn5 = GUI.enabled;
                     var startCalc5 = GetControlRect(0).y;
                     BEGIN_CATEGORY( ref calcRIGHT[3], false, null, 5 );
                     DrawHeader( "Extra options" );
                     BEGIN_PADDING( 3 );
                     GUI.enabled &= A.par.ENABLE_RIGHTDOCK_FIX;
                
                     DRAW_CAT_MEMORY( START_Y, W, ref contentY );
                
                
                     END_PADDING();
                     contentY += 20;
                     calcRIGHT[3] = contentY - startCalc5;
                     END_CATEGORY( ref calcRIGHT[3] );
                     GUI.enabled = wasEn5;
                     break;
                
                 case 6:
                     ADDITIONAL_FEATURES( START_Y, W, ref contentY );
                     break;
                
                 case 7:
                
                     var startCalc7 = GetControlRect(0).y;
                     BEGIN_CATEGORY( ref calcRIGHT[4], false, null, 5 );
                     DrawHeader( "Other features" );
                     BEGIN_PADDING( 3 );
                     GUI.enabled &= A.par.ENABLE_RIGHTDOCK_FIX;
                     ADD_CacheGIT( START_Y, W, ref contentY, false);
                
                     END_PADDING();
                     contentY += 20;
                     calcRIGHT[4] = contentY - startCalc7;
                     END_CATEGORY( ref calcRIGHT[4] );
                
                     X = W * 2;
                     Y = START_Y;
                     DrawRemove();
                     break;*/
                default: break;
            }
            
            if ( contentY > max ) max = contentY;
        }
        else
        {   var og = GUI.enabled;
            GUI.enabled = true;
            
            var contentY = START_Y;
            ADD_CacheGIT( START_Y, W, ref contentY, true );
            if ( contentY > max ) max = contentY;
            
            GUI.enabled = og;
            
        }
        
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    // DEPRICATES FUNCS
    // DEPRICATES FUNCS
    // DEPRICATES FUNCS
    
    
    
    void OLD_MENU( float START_Y, float max )
    {
    
        if ( A.par.ENABLE_ALL )
        {   var contentY = START_Y;
            switch ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 ) )
            {   /*case 0:
                        updateHeight = DRAW_MAIN();
                        break;*/
                case 0:
                    LEFT.y = GetLastRect().y + GetLastRect().height;
                    DOCK_LEFT( START_Y, W, ref contentY );
                    LEFT.height = GetLastRect().y + GetLastRect().height - LEFT.y;
                    break;
                case 1:
                
                    var wasEn2 = GUI.enabled;
                    
                    switch ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item/" + 1, 0 ) )
                    {   case 0:
                            BOTTOM.y = GetLastRect().y + GetLastRect().height;
                            DOCK_BOTTOM( START_Y, W, ref contentY );
                            BOTTOM.height = GetLastRect().y + GetLastRect().height - BOTTOM.y;
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            var startCalc = GetControlRect(0).y;
                            BEGIN_CATEGORY( ref calcBOTTOM1, false, null, 5 );
                            DrawHeader( "Bottom Extra" );
                            BEGIN_PADDING( 3 );
                            GUI.enabled &= A.par.ENABLE_RIGHTDOCK_FIX;
                            switch ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item/" + 1, 0 ) )
                            {   case 1:
                            
                                    if ( A.IS_HIERARCHY() ) DRAW_HIPER( START_Y, W, ref contentY );
                                    else DRAW_BOOKMARKSMANAGER( START_Y, W, ref contentY );
                                    break;
                                /*case 2:
                                  DRAW_CAT_VARSFUNKS( START_Y, W, ref contentY );
                                  break;*/
                                default: break;
                            }
                            END_PADDING();
                            contentY += 20;
                            calcRIGHT[3] = contentY - startCalc;
                            END_CATEGORY( ref calcBOTTOM1 );
                            
                            break;
                        default: break;
                    }
                    
                    GUI.enabled = wasEn2;
                    
                    
                    break;
                case 2:
                
                
                    var wasEn = GUI.enabled;
                    
                    switch ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item/" + 2, 0 ) )
                    {   case 0:
                            RIGHT.y = GetLastRect().y + GetLastRect().height;
                            DOCK_RIGHT( START_Y, W, ref contentY );
                            RIGHT.height = GetLastRect().y + GetLastRect().height - RIGHT.y;
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            var startCalc = GetControlRect(0).y;
                            BEGIN_CATEGORY( ref calcRIGHT[3], false, null, 5 );
                            DrawHeader( "Extra options" );
                            BEGIN_PADDING( 3 );
                            GUI.enabled &= A.par.ENABLE_RIGHTDOCK_FIX;
                            switch ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item/" + 2, 0 ) )
                            {   case 1:
                            
                                    DRAW_PLAYMODEKEEPER( 4, START_Y, W, ref contentY );
                                    break;
                                case 2:
                                    DRAW_CAT_VARSFUNKS( START_Y, W, ref contentY );
                                    break;
                                case 3:
                                    DRAW_CAT_MEMORY( START_Y, W, ref contentY );
                                    break;
                                case 4:
                                    DRAW_CAT_CUSTOMICS( START_Y, W, ref contentY );
                                    break;
                                default: break;
                            }
                            END_PADDING();
                            contentY += 20;
                            calcRIGHT[3] = contentY - startCalc;
                            END_CATEGORY( ref calcRIGHT[3] );
                            
                            break;
                        default: break;
                    }
                    
                    GUI.enabled = wasEn;
                    
                    
                    break;
                case 3:
                
                
                    switch ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item/" + 3, 0 ) )
                    {   case 0:
                            ADDITIONAL_FEATURES( START_Y, W, ref contentY );
                            /* RIGHT.y = GetLastRect().y + GetLastRect().height;
                             DOCK_RIGHT(START_Y, W, ref contentY);
                             RIGHT.height = GetLastRect().y + GetLastRect().height - RIGHT.y;*/
                            break;
                        case 1:
                        case 2:
                        case 3:
                            var startCalc = GetControlRect(0).y;
                            BEGIN_CATEGORY( ref calcRIGHT[4], false, null, 5 );
                            DrawHeader( "Other features" );
                            BEGIN_PADDING( 3 );
                            GUI.enabled &= A.par.ENABLE_RIGHTDOCK_FIX;
                            switch ( EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item/" + 3, 0 ) )
                            {   /*  case 1:
                                          ADD_CustomGenericMenu( START_Y, W, ref contentY);
                                          break;
                                      case 2:
                                          ADD_SeartchBox(START_Y, W, ref contentY);
                                          break;*/
                                case 1:
                                    ADD_CacheGIT( START_Y, W, ref contentY, false );
                                    break;
                                default: break;
                            }
                            END_PADDING();
                            contentY += 20;
                            calcRIGHT[4] = contentY - startCalc;
                            END_CATEGORY( ref calcRIGHT[4] );
                            
                            break;
                        default: break;
                    }
                    
                    
                    break;
                case 4:
                    DrawRemove();
                    break;
                default: break;
            }
            
            if ( contentY > max ) max = contentY;
        }
        else
        {   var og = GUI.enabled;
            GUI.enabled = true;
            
            var contentY = START_Y;
            ADD_CacheGIT( START_Y, W, ref contentY, true );
            if ( contentY > max ) max = contentY;
            
            GUI.enabled = og;
            
        }
        
    }
    
    
    
    
    void DRAW_CATEGORIES_BUTS()
    {   var r = GetControlRect(50, 5);
        var names = new[] { "Main", "Left\n-\nHighlighter", "Bottom\n-\nBookmarks", "Right\n-\nModules", "Other" };
        var L = names.Length;
        // Rect[] buttons = new Rect[names.Length];
        for ( int i = 0 ; i < L ; i++ )
        {   var R = r;
            r.x += r.width;
            
            // buttons[i] = R;
            
            var style = i < L / 2 ? EditorStyles.miniButtonLeft : i > L / 2 ? EditorStyles.miniButtonRight
                        : EditorStyles.miniButton;
            var tt = style.border.top;
            var bb = style.border.bottom;
            if ( !EditorGUIUtility.isProSkin )
            {   style.border.top = 5;
                style.border.bottom = 5;
            }
            EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
            if ( GUI.Button( R, names[i], style ) )
            {   EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item", i );
            }
            if ( Event.current.type == EventType.Repaint &&
                    EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 ) == i )
            {   var c = GUI.color;
                GUI.color *= new Color( 1f, 0.7f, 0.6f );
                style.Draw( R, names[i] /*.ToUpper()*/, true, true, false, true );
                GUI.color = c;
            }
            style.border.top = tt;
            style.border.bottom = bb;
            /* if (i != 0 & i != L - 1)
             {
                 R.height -= i == L / 2 ? 10 : 5;
                 R.y +=i== L / 2 ? 10 : 5;
             }*/
        }
    }
    void DRAW_CATEGORIES_BUTS_VERTICAL()
    {   Space( 10 );
    
        R = GetControlRect( EditorGUIUtility.singleLineHeight );
        // if (Event.current.type == EventType.Repaint && A.par.ENABLE_ALL)                A.SETUP_GREENLINE.Draw(new Rect(R.x + 8, R.y + R.height / 2, 4, R.height), false, false, false, false);
        
        //  GUI.DrawTexture(new Rect(0, R.y + 7, 20, 6), A.SETUP_GREENLINE_HORISONTAL.normal.background);
        var oldval = A.TOGGLE_LEFT(R, "Use " + A.pluginname + " Plugin", A.par.ENABLE_ALL);
        if ( oldval != A.par.ENABLE_ALL )
        {   A.par.ENABLE_ALL = oldval;
            OnEnableChange();
        }
        
        Space( 10 );
        
        GUI.enabled = A.par.ENABLE_ALL;
        
        
        
        
        var r = GetControlRect(50);
        var startYY = r.y;
        
        List<string> additional_temp = new List<string>();
#pragma warning disable
        /*    if (!Adapter.LITE) additional_temp.Add("Custom Generic Menu - Other");
            else additional_temp.Add(null);
            if (!Adapter.LITE) additional_temp.Add("Search Box - Other");
            else additional_temp.Add(null);*/
        if ( A.IS_HIERARCHY() ) additional_temp.Add( "Cache / Export Settings / GIT - Other" );
        else additional_temp.Add( "Export Settings / GIT - Other" );
        
        var names = new[] { "[Left] Icons & Highlighter", "[Bottom] Selections & Bookmarks", "[Right] Components & Modules", (Adapter.LITE ? "" : "[Menu/Search] ") + "Additional Features", "Uninstall" };
        var categories = new string[][] { new string[0], A.IS_HIERARCHY() && Adapter.LITE ? new string[0] : new[]{  A.IS_HIERARCHY() ? "HyperGraph - Extra" : "BookMarks Manager - Extra" },
            //new[] { "Extra - PlayMode Data Keeper", "Extra - Functions and Vars displaying", "Extra - Memory Optimizer", "Extra - Custom Icons" } ,
            A.IS_HIERARCHY() ?   new[] { "PlayMode Data Keeper - Extra", "Functions and Vars displaying - Extra", "Memory Optimizer - Extra", "Custom Icons - Extra" }
            : new[] { null, null, "Memory Optimizer - Extra", null }
            
            ,
            additional_temp.ToArray(),
            new string[0]
        };
#pragma warning restore
        var L = names.Length;
        Rect RECT_R;
        // Rect[] buttons = new Rect[names.Length];
        for ( int i = 0 ; i < L ; i++ )
        {   var R = r;
            r.y += r.height;
            
            // buttons[i] = R;
            
            /* var style = i < L / 2 ? EditorStyles.miniButtonLeft : i > L / 2 ? EditorStyles.miniButtonRight
                 : EditorStyles.miniButton;
             var tt = style.border.top;
             var bb = style.border.bottom;
             if (!EditorGUIUtility.isProSkin)
             {
                 style.border.top = 5;
                 style.border.bottom = 5;
             }*/
            EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );
            
            BOX( R );
            
            A.ChangeGUI();
            
            var FS = Adapter.GET_SKIN().button.fontSize;
            Adapter.GET_SKIN().button.fontSize = Adapter.GET_SKIN().box.fontSize;
            if ( GUI.Button( R, names[i] ) )
            {   EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item", i );
                EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item/" + i, 0 );
            }
            Adapter.GET_SKIN().button.fontSize = FS;
            
            // if (EditorPrefs.GetInt(A.pluginname + "/" + "Plugin Menu Item", 0) == i)
            {   for ( int j = 0 ; j < categories[i].Length ; j++ )
                {   if ( categories[i][j] == null ) continue;
                
                    var RR = r;
                    RR.height -= EditorGUIUtility.singleLineHeight;
                    //RR.x += 40;
                    //RR.width -= 80;
                    
                    EditorGUIUtility.AddCursorRect( RR, MouseCursor.Link );
                    //  BOX(RR, true);
                    var oldAligment = Adapter.GET_SKIN().button.alignment;
                    var oldPad = Adapter.GET_SKIN().button.padding.right;
                    Adapter.GET_SKIN().button.alignment = TextAnchor.MiddleRight;
                    Adapter.GET_SKIN().button.padding.right = 10;
                    FS = Adapter.GET_SKIN().button.fontSize;
                    Adapter.GET_SKIN().button.fontSize = Adapter.GET_SKIN().box.fontSize;
                    if ( GUI.Button( RR, categories[i][j] ) )
                    {   EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item", i );
                        EditorPrefs.SetInt( A.pluginname + "/" + "Plugin Menu Item/" + i, j + 1 );
                    }
                    Adapter.GET_SKIN().button.fontSize = FS;
                    Adapter.GET_SKIN().button.alignment = oldAligment;
                    Adapter.GET_SKIN().button.padding.right = oldPad;
                    
                    RECT_R = RR;
                    Adapter.DrawRect( new Rect( r.x + RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color( 0, 0, 0 ) );
                    
                    if (/*Event.current.type == EventType.Repaint &&*/ EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 ) == i
                            && EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item/" + i, 0 ) == j + 1 )
                    {   var c = GUI.color;
                        GUI.color *= new Color( 0.2f, 0.7f, 1f );
                        BOX( RR, false, true );
                        GUI.color = c;
                        
                        Adapter.DrawRect( new Rect( r.x + RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color( 0.2f, 0.7f, 1f ) );
                        
                    }
                    
                    r.y += r.height - EditorGUIUtility.singleLineHeight;
                    
                }
            }
            
            
            A.RestoreGUI();
            
            RECT_R = R;
            Adapter.DrawRect( new Rect( r.x + RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), Color.black );
            Adapter.DrawRect( new Rect( r.x - RECT_R.height / 2 + RECT_R.width, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), Color.black );
            
            if (/*Event.current.type == EventType.Repaint &&*/
                EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 ) == i && EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item/" + i, 0 ) == 0 )
            {   var c = GUI.color;
                GUI.color *= new Color( 0.2f, 0.7f, 1f );
                BOX( R, false, true );
                // GUI.Label(R, names[i]);
                //style.Draw(R, names[i] /*.ToUpper()*/, true, true, false, true);
                GUI.color = c;
                
                Adapter.DrawRect( new Rect( r.x + RECT_R.height / 2, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color( 0.2f, 0.7f, 1f ) );
                Adapter.DrawRect( new Rect( r.x - RECT_R.height / 2 + RECT_R.width, RECT_R.y + RECT_R.height / 2 - 2, 4, 4 ), new Color( 0.2f, 0.7f, 1f ) );
                
            }
            
            // R = r;
            /* style.border.top = tt;
             style.border.bottom = bb;*/
            /* if (i != 0 & i != L - 1)
             {
                 R.height -= i == L / 2 ? 10 : 5;
                 R.y +=i== L / 2 ? 10 : 5;
             }*/
        }
        
        Space( r.y - startYY - r.height );
        
    }
    
    
    // GUIStyle emptyStyle = new GUIStyle();
    
    
    void UpdateHi()
    {   PKC.KEEPER_UPDATE( win );
    
        CI.Updater( win );
    }
    
    
}


//  }
}