using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PROJECT
    using EModules.Project;
#endif
//namespace EModules


/*
 * You can use version control systems.

   To maintain compatibility between people who do not use the plug-in hierarchy, you may copy few *.cs files to other machines.

   You should copy 'DescriptionHelper.cs', 'DescriptionRegistrator.cs' and 'Hierarchy (MAYA outliner) Plugin.dll' from ../Resource folder.
 */









namespace EModules.EModulesInternal

{



















public class _S___ScenesScannerWindow : _W___IWindow {


    internal class TreeData {
        internal class TreeItem { /* : IEquatable<TreeItem>*/
            /* internal string name;
                     public override string ToString()
                     {
                         return name;
                     }*/
            internal bool Expand = true;
            internal bool Select
            {   get { return BROAD( this ); }
                set
                {   BROAD( this, value );
                }
            }
            internal void BROAD(Action<TreeItem> ac)
            {   // foreach (var sceneData in item.items) sceneData.Value.Select = value;
                foreach (var sceneData in treeItems)
                {   ac( sceneData.Value );
                    sceneData.Value.BROAD( ac );
                }
            }
            internal void BROAD(Action<SceneData> ac)
            {   // foreach (var sceneData in item.items) sceneData.Value.Select = value;
                foreach (var sceneData in treeItems)
                    sceneData.Value.BROAD( ac );
                foreach (var sceneData in items)
                    ac( sceneData.Value );
                    
            }
            void BROAD(TreeItem item, bool value)
            {   foreach (var sceneData in item.items) sceneData.Value.Select = value;
                foreach (var sceneData in item.treeItems) BROAD( sceneData.Value, value );
            }
            bool BROAD(TreeItem item)
            {   foreach (var sceneData in item.items) if (!sceneData.Value.Select) return false;
                foreach (var sceneData in item.treeItems) return BROAD( sceneData.Value );
                return true;
            }
            internal Dictionary<string, TreeItem> treeItems = new Dictionary<string, TreeItem>();
            internal Dictionary<string, SceneData> items = new Dictionary<string, SceneData>();
            
            /* public bool Equals(TreeItem other)
                     {
                         return other.name == name;
                     }
            
                     public override bool Equals(object obj)
                     {
                         return name == ((TreeItem)obj).name;
                     }*/
        }
        internal TreeItem root = new TreeItem();
        
        internal TreeData(string[] input)
        {   // MonoBehaviour.print(input[0]);
            foreach (var path in input)
            {   var currentCatalog = root;
                foreach (var seg in path.Split( '/' ))
                {   if (seg.Contains( '.' ) && seg.EndsWith(".unity", StringComparison.OrdinalIgnoreCase))
                    {   currentCatalog.items.Add( seg, new SceneData() { name = seg, path = path, Select = !path.StartsWith( "Assets/Plugins" ) && !path.StartsWith( "Assets/Editor" ) } );
                    }
                    else
                    {   TreeItem newTree = null;
                        if (currentCatalog.treeItems.ContainsKey( seg )) newTree = currentCatalog.treeItems[seg];
                        else
                        {   newTree = new TreeItem();
                            currentCatalog.treeItems.Add( seg, newTree );
                        }
                        currentCatalog = newTree;
                    }
                }
            }
        }
    }
    
    
    
    
    static Action lateAction;
    internal static void Init(Adapter adapter, Action _lateAction = null)
    {   if (adapter.IS_PROJECT()) return;
    
        lateAction = _lateAction;
        // var rect = new Rect(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, 0, 0);
        /*var scr = UnityStats.screenRes.Split('x');
          if (scr.Length > 1)
          {
              var X = 0;
              var Y = 0;
              if (int.TryParse(scr[0], out X)) rect.x = X / 2;
              if (int.TryParse(scr[1], out Y)) rect.y = Y / 2;
              MonoBehaviour.print(X+ " " + Y);
          }*/
        //var res = new Rect( 1920 / 2, Screen.currentResolution.height / 2, 0, 0 );
        var res = new Rect( Adapter.MAX_WINDOW_WIDTH .y / 2, Adapter.MAX_WINDOW_HEIGHT.y / 2, 0, 0 );
        
        res.width = (res.x * 2) * 0.5f;
        res.height = (res.y * 2) * 0.7f;
        /* res.x -= res.width / 2;
         res.y -= res.height / 2;*/
        MousePos.w_params[MousePos.Type.SceneScanner_X_X] = new WindowParams() { Width = res.width, Height = res.height };
        var rect = new MousePos(null, MousePos.Type.SceneScanner_X_X, false, adapter);
        
        _W___IWindow.private_Init( rect, typeof( _S___ScenesScannerWindow ), adapter, "Search for stored hierarchy data (Experimental)", utils: true, useAnim: false );
        
        if (loadedData == null) laodData();
    }
    
    static void laodData()
    {   loadedData = new TreeData( AssetDatabase.GetAllAssetPaths().Where( p => p.EndsWith( ".unity", StringComparison.OrdinalIgnoreCase ) ).OrderBy( s => s ).ToArray() );
    }
    
    void OnDestroy()
    {   StopLoad();
    }
    
    internal override bool PIN
    {   get { return true; }
        set { }
    }
    
    void StartLoad()
    {   /*  if (Loading) return;
                Loading = true;
                lastScenes = EditorSceneManager.GetActiveScene();
                scenes = AssetDatabase.GetAllAssetPaths().Where(p => p.EndsWith(".unity")).ToArray();
                interator = 0;
                asinc = null;
                loadedData.Clear();
                EditorApplication.update -= LoadData;
                EditorApplication.update += LoadData;
        
                _inputWindow.Repaint();*/
        
    }
    
    void StopLoad()
    {   /* Loading = false;
               EditorApplication.update -= LoadData;
               loadedData.Clear();*/
    }
    
    internal class SceneData {
#pragma warning disable
        internal bool wasScanned;
        internal string path;
        internal string name;
        internal bool hasData;
        internal bool dataWasRemove;
        public bool Select;
#pragma warning restore
        
        public override string ToString()
        {   return name;
        }
        
    }
    static TreeData loadedData = null;
    int interator = 0;
    bool Loading = false;
    void LoadData()
    {   if (asinc != null && !asinc.isDone || !Loading) return;
        for (; interator < scenes.Length ; interator++)
        {   _inputWindow.Repaint();
        
            var path = scenes[interator];
            try
            {   var loaded = EditorSceneManager.OpenScene( path, OpenSceneMode.Single );
                DoScene( loaded );
                EditorSceneManager.SaveScene( loaded );
#pragma warning disable
                asinc = EditorSceneManager.UnloadSceneAsync( loaded );
                if (!asinc.isDone) return;
                
#pragma warning restore
            }
            catch
            {
            }
        }
        
        try
        {   if (!string.IsNullOrEmpty( lastScenes.path )) EditorSceneManager.OpenScene( lastScenes.path, OpenSceneMode.Single );
        }
        catch
        {
        }
        StopLoad();
    }
    
    void DoScene(Scene loaded)
    {   /* loadedData.Add(new SceneData() {
                   name = loaded.name,
                   path = loaded.path,
                   haveData = loaded.GetRootGameObjects().Any(t => t.name == "DescriptionHelperObject")
               });*/
        /* foreach (var t in loaded.GetRootGameObjects().Where(t => t.name == "DescriptionHelperObject"))
                   UnityEngine.Object.DestroyImmediate(t.gameObject, true);*/
    }
    
#pragma warning disable
    Vector2 scrollPos = Vector2.zero;
    private Scene lastScenes;
    private string[] scenes;
    private AsyncOperation asinc;
#pragma warning restore
    float H = 14;
    //int ComplexID;
    
    protected override void OnGUI()
    {   if (adapter == null) Close();
        if (adapter.IS_PROJECT()) return;
        
        base.OnGUI();
        
        if (Loading && scenes != null && scenes.Length != 0)
        {   var m = GUI.matrix;
            GUI.matrix = Matrix4x4.Scale( new Vector3( 7, 7, 7 ) );
            
            var InterfaceRect = new Rect( _inputWindow.position.width / 2, _inputWindow.position.height / 2, 0, 0 );
            InterfaceRect.width = InterfaceRect.height = 44;
            InterfaceRect.x -= 22;
            InterfaceRect.y -= 22;
            
            GUI.DrawTexture( InterfaceRect, adapter.LOADING_TEXTURE() );
            InterfaceRect.y += InterfaceRect.height + 3;
            InterfaceRect.height = 20;
            var prog = InterfaceRect;
            var val = interator / (scenes.Length - 1f);
            
            prog.width *= val;
            GUI.DrawTexture( prog, adapter.GetIcon( "BUTBLUE" ) );
            GUI.Label( InterfaceRect, Mathf.RoundToInt( val * 100 ) + "%" );
            
            GUI.matrix = m;
            
            _inputWindow.Repaint();
        }
        else
        {   // WIDTH = EditorGUILayout.GetControlRect(false, GUILayout.Height(0)).width;
            //if (WIDTH < 2) WIDTH = _inputWindow.position.width;
            WIDTH = _inputWindow.position.width - 40;
            GUILayout.BeginHorizontal();
            
            var R = EditorGUILayout.GetControlRect( false, GUILayout.Height( H * 2 ), GUILayout.Width( WIDTH * columns[0] ) );
            var drawR = R;
            drawR.width = (Math.Min( R.width, (500) + 40 ) - 40) / 5;
            if (GUI.Button( drawR, "Collapse None", adapter.SETUP_BUTTON )) loadedData.root.BROAD( b => b.Expand = false );
            drawR.x += drawR.width;
            if (GUI.Button( drawR, "Expand All", adapter.SETUP_BUTTON )) loadedData.root.BROAD( (Action<TreeData.TreeItem>)(b => b.Expand = true) );
            drawR.x += drawR.width + 20;
            if (GUI.Button( drawR, "Select None", adapter.SETUP_BUTTON )) loadedData.root.Select = false;
            drawR.x += drawR.width;
            if (GUI.Button( drawR, "Select All", adapter.SETUP_BUTTON )) loadedData.root.Select = true;
            drawR.x += drawR.width + 20;
            if (GUI.Button( drawR, "Refresh Scenes", adapter.SETUP_BUTTON ))
            {   if (EditorUtility.DisplayDialog( "Refresh Scenes", "this is to remove the scanned information. are you sure?", "Yes", "Cancel" ))
                {   laodData();
                }
                /* conform*/
            }
            
            
            if (lateAction == null)
            {   R = EditorGUILayout.GetControlRect( false, GUILayout.Height( H * 2 ), GUILayout.Width( WIDTH * columns[1] ) );
                if (GUI.Button( R, "Scan All", adapter.SETUP_BUTTON ))
                {   if (EditorUtility.DisplayDialog( "Scan All", "it may take a time, are you sure?", "Yes", "Cancel" ))
                    {   loadedData.root.BROAD( (Action<SceneData>)(b =>
                        {   if (b.Select) SCAN_SCENE( b );
                        }) );
                    }
                    
                    /*scan*/
                }
                
                EditorGUILayout.GetControlRect( false, GUILayout.Height( H * 2 ), GUILayout.Width( WIDTH * columns[2] ) );
                
                R = EditorGUILayout.GetControlRect( false, GUILayout.Height( H * 2 ), GUILayout.Width( WIDTH * columns[3] ) );
                if (GUI.Button( R, "Remove All", adapter.SETUP_BUTTON ))
                {   if (EditorUtility.DisplayDialog( "Remove All", "it may take a time, are you sure?", "Yes", "Cancel" ))
                    {   REMOVE_ALL();
                    }
                    /*remove*/
                }
            }
            else
            {   R = EditorGUILayout.GetControlRect( false, GUILayout.Height( H * 2 ), GUILayout.Width( WIDTH * columns[2] + WIDTH * columns[3] ) );
                if (GUI.Button( R, "Remove Cache and Plugin", adapter.SETUP_BUTTON ))
                {   if (EditorUtility.DisplayDialog( "Remove Cache and Plugin", "it may take a time, are you sure?", "Yes", "Cancel" ))
                    {   REMOVE_ALL( false );
                        lateAction();
                    }
                }
            }
            
            
            
            GUILayout.EndHorizontal();
            
            scrollPos = EditorGUILayout.BeginScrollView( scrollPos );
            
            /* foreach (var sceneData in )
                     {
                         GUILayout.Label(sceneData.name);
                         GUILayout.Label(sceneData.path);
                         GUILayout.Label(sceneData.haveData ? "Have Data" : "");
                     }*/
            line = 0;
            GUILayout.BeginVertical();
            if (loadedData.root.treeItems.Count != 0) DrawTreeView( loadedData.root.treeItems.First().Value, 0 );
            GUILayout.EndVertical();
            
            EditorGUILayout.EndScrollView();
        }
        
        
        
    }
    
    void REMOVE_ALL(bool removeOnlyFinded = true)
    {   var list = new List<SceneData>();
        loadedData.root.BROAD( (Action<SceneData>)(b =>
        {   if (removeOnlyFinded && b.wasScanned && b.hasData || !removeOnlyFinded && b.Select) list.Add( b );
        }) );
        if (list.Count != 0)
        {   EditorUtility.DisplayProgressBar( "Remove All Hierarchy Cache", "completed: " + (0) + "/" + (list.Count), 0 / (list.Count - 1f) );
            for (int i = 0 ; i < list.Count ; i++)
            {   REMOVE_SCENE( list[i], removeOnlyFinded );
                EditorUtility.DisplayProgressBar( "Remove All Hierarchy Cache", "completed: " + (i + 1) + "/" + (list.Count), i / (list.Count - 1f) );
            }
            EditorUtility.ClearProgressBar();
        }
    }
    
    
    void SCAN_SCENE(SceneData sc)
    {   if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        try
        {   var loaded = EditorSceneManager.OpenScene( sc.path, OpenSceneMode.Single );
            sc.wasScanned = true;
            sc.hasData = loaded.GetRootGameObjects().Any( t => t.name == "DescriptionHelperObject" );
        }
        catch { }
    }
    
    void REMOVE_SCENE(SceneData sc, bool removeOnlyFinded = true)
    {   if (removeOnlyFinded && !sc.hasData || !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        try
        {   var loaded = EditorSceneManager.GetActiveScene().path == sc.path ? EditorSceneManager.GetActiveScene() : EditorSceneManager.OpenScene( sc.path, OpenSceneMode.Single );
        
        
            var ds = loaded.GetRootGameObjects().Where( t => t.name == "DescriptionHelperObject" );
            bool wasChange = false;
            foreach (var c in ds)
            {   var o = c.gameObject;
                DestroyImmediate( o, true );
                wasChange = true;
            }
            foreach (var c in Resources.FindObjectsOfTypeAll<Component>().Where( c => c is IDescriptionRegistrator && c.gameObject.scene.IsValid() ))
            {   var o = c.gameObject;
                if (!o) continue;
                DestroyImmediate( c, true );
                EditorUtility.SetDirty( o );
                wasChange = true;
            }
            sc.dataWasRemove = true;
            sc.Select = false;
            if (wasChange) EditorSceneManager.SaveScene( loaded );
        }
        catch { }
    }
    
    
    
    void DRAWSCENE_GUI(KeyValuePair<string, SceneData> data)
    {   var R = EditorGUILayout.GetControlRect( false, GUILayout.Height( H ), GUILayout.Width( WIDTH * columns[1] ) );
        if (data.Value.wasScanned)
        {   GUI.Label( R, "Scanned" );
        }
        else
        {   if (GUI.Button( R, "Scan Scene", adapter.SETUP_BUTTON ))
            {   SCAN_SCENE( data.Value );
            }
        }
        
        R = EditorGUILayout.GetControlRect( false, GUILayout.Height( H ), GUILayout.Width( WIDTH * columns[2] ) );
        if (data.Value.wasScanned)
        {   if (data.Value.hasData) GUI.Label( R, "Has cache" );
            else GUI.Label( R, "Not found" );
        }
        else
        {   GUI.Label( R, "-" );
        }
        
        R = EditorGUILayout.GetControlRect( false, GUILayout.Height( H ), GUILayout.Width( WIDTH * columns[3] ) );
        if (data.Value.wasScanned && data.Value.hasData)
        {   if (data.Value.dataWasRemove)
            {   GUI.Label( R, "Is removed" );
            }
            else
            {   if (GUI.Button( R, "Remove cache", adapter.SETUP_BUTTON ))
                {   REMOVE_SCENE( data.Value );
                    // data.Value.dataWasRemove = true;
                    //  data.Value.Select = false;
                    /*remove*/
                }
            }
            
        }
        
    }
    
    float WIDTH = 0;
    int line = 0;
    Color[] lineC = { new Color( 0, 0, 0, 0.1f ), new Color( 0, 0, 0, 0.05f ) };
    Color sceneC = new Color( 0.05f, 0.4f, 0.9f, 0.2f );
    Color sceneC5 = new Color( 0.05f, 0.4f, 0.9f, 0.05f );
    float[] columns = { 0.7f, 0.1f, 0.1f, 0.1f };
    
    void DrawColor(Rect R, Color color)
    {   var asd = GUI.color;
        GUI.color *= color;
        GUI.DrawTexture( R, Texture2D.whiteTexture );
        GUI.color = asd;
    }
    
    void DrawTreeView(TreeData.TreeItem treeData, int deep)
    {   foreach (var treeItem in treeData.treeItems)
        {   //GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();
            
            var R = EditorGUILayout.GetControlRect( false, GUILayout.Height( H ), GUILayout.Width( WIDTH * columns[0] ) );
            var drawR = R;
            drawR.width = WIDTH * 2;
            DrawColor( drawR, lineC[line++ % 2] );
            R.x += deep * H;
            R.width -= deep * H;
            drawR = R;
            drawR.width = H;
            treeItem.Value.Expand = EditorGUI.Foldout( drawR, treeItem.Value.Expand, "" );
            drawR.x += drawR.width;
            var newS = EditorGUI.Toggle( drawR, treeItem.Value.Select );
            if (newS != treeItem.Value.Select)
            {   treeItem.Value.Select = newS;
            }
            drawR.x += drawR.width;
            drawR.width = R.width - drawR.x;
            GUI.Label( drawR, new GUIContent( treeItem.Key, adapter.GetIcon( "FOLDER" ) ) );
            
            GUILayout.EndHorizontal();
            
            
            //GUILayout.EndHorizontal();
            if (treeItem.Value.Expand) DrawTreeView( treeItem.Value, deep + 1 );
        }
        
        
        foreach (var treeItem in treeData.items)
        {   //GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal();
            var R = EditorGUILayout.GetControlRect( false, GUILayout.Height( H ), GUILayout.Width( WIDTH * columns[0] ) );
            var drawR = R;
            drawR.width = WIDTH * 2;
            DrawColor( drawR, lineC[line++ % 2] );
            DrawColor( drawR, treeItem.Value.Select ? sceneC : sceneC5 );
            R.x += deep * H + H;
            R.width -= deep * H + H;
            drawR = R;
            drawR.width = H;
            treeItem.Value.Select = EditorGUI.Toggle( drawR, treeItem.Value.Select );
            drawR.x += drawR.width;
            drawR.width = R.width - drawR.x;
            
            GUI.Label( drawR, new GUIContent( treeItem.Key, adapter.GetIcon( "SCENE" ) ) );
            
            DRAWSCENE_GUI( treeItem );
            GUILayout.EndHorizontal();
            //GUILayout.EndHorizontal();
        }
    }
}
}
