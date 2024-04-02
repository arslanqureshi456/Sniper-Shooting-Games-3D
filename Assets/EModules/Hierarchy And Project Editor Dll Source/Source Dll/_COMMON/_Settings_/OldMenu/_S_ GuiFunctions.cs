using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PROJECT
    using EModules.Project;
#endif
//namespace EModules



namespace EModules.EModulesInternal

{
internal partial class SETUPROOT { /*, ISerializable, IDeserializationCallback*/







    internal const float W = 380;
    
    
    
    
    
    internal static GUIContent wikibutcontent = new GUIContent();
    
    internal static void DRAW_WIKI_BUTTON(string pluginname, string category, string note = null)
    {   var rect = GetPreLastRect();
        var P = 0;
        rect.height -= P * 2;
        rect.y += P;
        rect.x += rect.width;
        rect.width = rect.height * 3;
        rect.x -= rect.width - P;
        
        var lr = rect;
        lr.width *= 0.66f;
        LABEL( new Rect(lr.x - lr.width, lr.y, lr.width * 2, lr.height), "<i>online doc</i>" );
        lr.x += lr.width;
        lr.width /= 1.8f;
        // GUI.Label(lr, "?");
        wikibutcontent.text = "?";
        wikibutcontent.tooltip = "www.emem.store/wiki/hierarchy/" + category;
        if (note != null) wikibutcontent.tooltip += "/" + note;
        // Hierarchy.ChangeGUI();
        if (GUI.Button( lr, wikibutcontent ))
        {   OpenWikiUrl( pluginname, category, note );
        }
    }
    
    internal void DRAW_WIKI_BUTTON(string category, string note = null)
    {   DRAW_WIKI_BUTTON( A.pluginname, category, note );
        //  Hierarchy.RestoreGUI();
        
    }
    
    
    internal int DRAW_BUTTONS(string tit, string[] names, int selected, int height, bool usered = false, Texture2D[] textures = null)
    {   var r = GET_OFFSETRECT(height);
        return DRAW_BUTTONS( r, tit, names, selected, usered, textures );
    }
    internal int DRAW_BUTTONS(Rect r, string tit, string[] names, int selected, bool usered = false, Texture2D[] textures = null)
    {   var L = names.Length;
    
        if (tit != null)
        {
        
        
            r.width /= 2;
            GUI.Label( r, tit );
            r.x += r.width;
        }
        r.width /= L;
        // Rect[] buttons = new Rect[names.Length];
        for (int i = 0 ; i < L ; i++)
        {   var R = r;
            r.x += r.width;
            
            // buttons[i] = R;
            
            var style = i < L / 2 ? EditorStyles.miniButtonLeft : i > L / 2 ? EditorStyles.miniButtonRight : EditorStyles.miniButton;
            var tt = style.border.top;
            var bb = style.border.bottom;
            if (!EditorGUIUtility.isProSkin)
            {   style.border.top = 5;
                style.border.bottom = 5;
            }
            
            if (GUI.Button( R, names[i], style ))
            {   return i;
            }
            
            if (Event.current.type == EventType.Repaint && selected == i)
            {   var c = GUI.color;
                if (usered) GUI.color *= new Color( 1f, 0.7f, 0.6f );
                style.Draw( R, names[i]/*.ToUpper()*/, true, true, false, true );
                GUI.color = c;
            }
            
            style.border.top = tt;
            style.border.bottom = bb;
            
            if (textures != null)
            {   GUI.DrawTexture( new Rect( R.x + (R.width - textures[i].width) / 2, R.y + (R.height - textures[i].height) / 2, textures[i].width, textures[i].height ), textures[i] );
            }
            /* if (i != 0 & i != L - 1)
             {
                 R.height -= i == L / 2 ? 10 : 5;
                 R.y +=i== L / 2 ? 10 : 5;
             }*/
            
        }
        return selected;
    }
    
    internal Rect SHRINK(Rect R)
    {   R.x += 5;
        R.y += 5;
        R.width -= 10;
        R.height -= 10;
        return R;
    }
    
    internal Rect SHRINK(Rect R, float v)
    {   R.x += v;
        R.y += v;
        R.width -= v * 2;
        R.height -= v * 2;
        return R;
    }
    
    internal void BOX(Rect R, bool drawBg = true, bool drawFb = true)
    {   if (EditorGUIUtility.isProSkin)
        {   var asd = GUI.color;
            GUI.color = Color.black;
            if (drawBg)
            {   INTERNAL_BOX( R, "" );
            
                INTERNAL_BOX( R, "" );
                INTERNAL_BOX( R, "" );
                INTERNAL_BOX( R, "" );
            }
            
            GUI.color = asd;
            
            R.x += 5;
            R.y += 5;
            R.width -= 10;
            R.height -= 10;
            if (drawFb) INTERNAL_BOX( R, "" );
        }
        else
        {   var asd = GUI.color;
            GUI.color = new Color( 0, 0, 0, 0.1f );
            ////   if (drawBg && Event.current.type == EventType.Repaint) GUI.TextArea( R, "" );
            //if (drawBg && Event.current.type == EventType.Repaint) Adapter. INTERNAL_BOX( R, "" );
            GUI.color = asd;
            
            
            GUI.color *= new Color( 1, 1, 1, 0.6f );
            
            R.x += 5;
            R.y += 5;
            R.width -= 10;
            R.height -= 10;
            ////  if (drawFb && Event.current.type == EventType.Repaint) GUI.TextArea( R, "" );
            // if (drawBg && Event.current.type == EventType.Repaint) GUI.DrawTexture( R, A.GetIcon( "SETUP_BOXCAT" ) );
            if (drawFb) Adapter. INTERNAL_BOX( R, "" );
            
            GUI.color = asd;
            
        }
        
        
        
        
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    internal static void OpenWikiUrl(string plugin, string category, string note = null)
    {   var url = "http://www.emem.store/wiki" + "?=" + plugin + "&" + category;
        if (note != null) url += "&" + note;
        Application.OpenURL( url );
    }
    
    
    internal Texture2D GetSetubBGTexture()
    {   return EditorGUIUtility.isProSkin ? A.GetIcon( "SETUP_BG" ) : A.GetIcon( "SETUP_BG_PERSONAL" );
    }
    
    
    
    
    
    
    // Type type;
    //string name;
    //  EditorWindow win;
    string ScrollKey { get { return A.pluginname + "/ScrollPos"; } }
    
    
    
    
    
    
    
    internal bool WindowWasInit = false;
    internal float scrolxx;
    internal float? scroll = null;
    
    internal void OnGUI(EditorWindow w, SETUPROOT settingModule)
    {   if (!win)
        {   win = w;
            if (!win) return;
        }
        if (!WindowWasInit)
        {   if (Event.current.type == EventType.Repaint)
            {   if (win.position.x < 15 && win.position.y < 50)
                {   var p = win.position;
                    p.x =  Adapter.MAX_WINDOW_WIDTH.x + (Adapter.MAX_WINDOW_WIDTH.y - p.width) / 2;
                    p.y = Adapter.MAX_WINDOW_HEIGHT.x + (Adapter.MAX_WINDOW_HEIGHT.y - p.height) / 2;
                    win.position = p;
                }
                WindowWasInit = true;
            }
        }
        
        
        var newScroll = GUILayout.BeginScrollView(new Vector2(scrolxx, scroll ?? EditorPrefs.GetFloat(ScrollKey, 0)));
        scrolxx = newScroll.x;
        // scrollX = newScroll.x;
        internalDrawer( true, settingModule );
        EditorGUILayout.GetControlRect(GUILayout.Width(W * 3 + 30));
        GUILayout.EndScrollView();
        
        if ((Event.current.type == EventType.Layout || Event.current.isMouse || Event.current.type == EventType.ScrollWheel || Event.current.type == EventType.Used) && newScroll.y != scroll)
        {   scroll = newScroll.y;
            EditorPrefs.SetFloat( ScrollKey, newScroll.y );
        }
    }
    
    
    //float scrollX = 0;
    
    
    internal Vector2 sv;
    //internal EditorWindow prefWin;
    //internal bool wasInit = false;
    //internal float SCH = 0;
    //internal float calcedHeight = 0;
    
    internal void drawGUIcaller(bool itIsPrefs, SETUPROOT settingModule)
    {   // GUI.matrix = Matrix4x4.Scale(new Vector3(0.6f, 0.6f, 0.6f));
    
        try
        {   if (Application.unityVersion.StartsWith( "5.5" ) && itIsPrefs)
            {   win = Resources.FindObjectsOfTypeAll( typeof( EditorWindow ) )
                      .FirstOrDefault( w => w.GetType().FullName == "UnityEditor.PreferencesWindow" ) as EditorWindow;
                      
                if (win != null)
                {   sv = GUILayout.BeginScrollView( sv, false, true, GUILayout.Width( W + 20 ),
                                                    GUILayout.Height( win.position.height - 40 ) );
                    internalDrawer( !itIsPrefs, settingModule );
                    GUILayout.EndScrollView();
                }
            }
            else
            {   internalDrawer( !itIsPrefs, settingModule );
            }
        }
        catch (Exception ex)
        {   A.logProxy.LogError( ex.Message + " " + ex.StackTrace );
        }
    }
    
    internal void internalDrawer(bool inspector, SETUPROOT settingModule)
    {   // if (Event.current.type.ToString().Equals("ScrollWheel", StringComparison.CurrentCultureIgnoreCase)) return;
    
        if (A == null)
        {   if (Initializator.AdaptersByName.ContainsKey( Aname ))
            {   A = Initializator.AdaptersByName[Aname];
            }
        }
        if (A == null) return;
        
        if (!wasInit || A.SETUP_TITLEHOGLIGHT == null)
        {   var checkFilesResult = Adapter.CheckFiles(A);
            if (checkFilesResult != null)
            {   if (!checkFilesResult.EndsWith( ".cs" ))
                {   wasInit = false;
                    A.logProxy.Log( "'" + checkFilesResult + "' is Missing" );
                }
                else
                {   A.tempAdapterDisableCache = true;
                    wasInit = true;
                }
            }
            else
            {   wasInit = true;
            }
            
        }
        A.InitializeStyles();
        
        if (!wasInit || A.SETUP_TITLEHOGLIGHT == null)
        {   GUILayout.Label( "Please Reinstall Plugin" );
            DrawRemove();
            return;
        }
        
        
        
        
        if (Event.current.type == EventType.Repaint)
        {   // DrawTiled(new Rect(0, 0, W + 200, calcedHeight == 0 ? 1500 : calcedHeight), GetSetubBGTexture(), 337);
        }
        
        
        if (Event.current.type == EventType.Repaint || Event.current.type == EventType.ContextClick ||
                Event.current.type == EventType.DragExited || Event.current.type == EventType.DragPerform ||
                Event.current.type == EventType.DragUpdated || Event.current.type == EventType.ExecuteCommand ||
                Event.current.rawType == EventType.MouseUp || Event.current.rawType == EventType.Used ||
                Event.current.rawType == EventType.Ignore || Event.current.isMouse || Event.current.isKey)
        {   BEGIN_PADDING( inspector ? EditorGUIUtility.singleLineHeight : 0 );
            SCH = settingModule.DRAW_MAIN( inspector );
            END_PADDING();
        }
        
        GUILayout.Label( "", GUILayout.Height( SCH )/*, GUILayout.Width(W * 5)*/);
        
        
        if (Event.current.type == EventType.Repaint)
        {   var needRepaint = calcedHeight == 0;
            calcedHeight = GUILayoutUtility.GetLastRect().y + GUILayoutUtility.GetLastRect().height;
            
            if (needRepaint)
            {   settingModule.Repaint();
            }
        }
    }
    
    
    
    
    
    
    
    
    
    internal void DrawNew(float? height = null)
    {   // var oc = GUI.color;
        // GUI.color = Color.red;
        var rect = GetLastRect();
        rect.y += rect.height;
        rect.height = EditorGUIUtility.singleLineHeight;
        rect.x = X + 10;
        rect.y += 2;
        rect.width = EditorGUIUtility.singleLineHeight;
        if (height != null)
        {   rect.y += (height.Value - rect.height) / 2;
        }
        GUI.DrawTexture( rect, A.GetIcon( "NEW" ) );
        // GUI.Label(rect, "NEW");
        // GUI.color = oc;
    }
    internal void DrawNew(Rect rect)
    {   // var oc = GUI.color;
        // GUI.color = Color.red;
        rect.height = EditorGUIUtility.singleLineHeight;
        rect.x = X;
        rect.y += 2;
        rect.width = EditorGUIUtility.singleLineHeight;
        
        GUI.DrawTexture( rect, A.GetIcon( "NEW" ) );
        // GUI.Label(rect, "NEW");
        //  GUI.color = oc;
    }
    
    GUIStyle shadow, glow;
    
    internal Rect DRAW_HELP_TEXTURE(string textureName, int? height = null, bool? enable = null, int xOffset = 0, float? yOffset = null, TextAlignment? align = null, float DDD = 1.2f)
    {   if (!DrawHelpImage) return new Rect( 0, 0, 0, 0 );
        var texture = A.GetIcon(textureName + (EditorGUIUtility.isProSkin ? "" : " PERSONAL"));
        if (texture == Texture2D.whiteTexture) texture = A.GetIcon( textureName );
        float HHH = height ?? texture.height;
        HHH /= DDD;
        if (yOffset.HasValue) yOffset /= DDD;
        var lastRect = GetControlRect((HHH));
        var dx = lastRect.x;
        var dw = lastRect.width;
        if (align == null) lastRect.x += xOffset + (lastRect.width - 327) / 2;
        else lastRect.x += xOffset;
        lastRect.width = texture.width;
        if (align == null) lastRect.x += (327 - lastRect.width) / 2;
        
        var clampRect = lastRect;
        lastRect.height = texture.height / DDD;
        clampRect.height = HHH;
        // lastRect.height = height ?? texture.height;
        var c = GUI.color;
        /* if (!EditorGUIUtility.isProSkin)
         {
             var cc = GUI.color;
             cc.a = 0.8f;
             GUI.color = cc;
         }*/
        if (Event.current.type.Equals( EventType.Repaint ))
        {   GUI.BeginClip( clampRect );
            lastRect.y = lastRect.x = 0;
            if (yOffset.HasValue) lastRect.y += yOffset.Value;
            GUI.DrawTexture( lastRect, texture, ScaleMode.ScaleToFit);
            //  if (shadow == null) shadow = A.InitializeStyle( "SHADOW", 0.25f, 0.25f, 0.25f, 0.25f );
            if (glow == null) glow = A.InitializeStyle( "HIPERUI_BUTTONGLOW", 0.25f, 0.25f, 0.25f, 0.25f );
            //  if (Event.current.type == EventType.Repaint) shadow.Draw(lastRect, "", false, false, false, false);
            if (Event.current.type == EventType.Repaint  )
            {   var c12 = GUI.color;
                if (EditorGUIUtility.isProSkin)GUI.color *= new Color32(50, 50, 50, 255);
                glow.Draw(lastRect, "", false, false, false, false);
                GUI.color = c12;
            }
            GUI.EndClip();
        }
        //Y += yOffset ?? 0;
        if (!GUI.enabled || !(enable ?? true)) Adapter.FadeRect( clampRect, 0.7f );
        GUI.color = c;
        
        clampRect.x += clampRect.width;
        clampRect.width = dw - clampRect.x + dx;
        
        
        return clampRect;
        // GUILayout.Space(lastRect.height);
        /*  var lastRect = GUILayoutUtility.GetLastRect();
          lastRect.x += 15 + xOffset;
          lastRect.y += lastRect.height;
          lastRect.width = 327;
          lastRect.height = width;
          if (Event.current.type.Equals(EventType.Repaint)) GUI.DrawTexture(lastRect, GetIcon(textureName));
          GUILayout.Space(lastRect.height);*/
    }
    
    
    internal void editor_updated()
    {   if ( A.videoRequest != null)
        {   if (!A.videoRequest.MoveNext())
            {   A.videoRequest = null;
            }
        }
    }
    
    
    
    internal void Draw_HIERERCHY_YouTube(Rect re)
    {   GUI.BeginClip( re );
        re.x = 0;
        re.y = 0;
        
        
        var B_normal = Adapter.GET_SKIN().button.normal.background;
        var B_active = Adapter.GET_SKIN().button.active.background;
        var B_focused = Adapter.GET_SKIN().button.focused.background;
        var B_hover = Adapter.GET_SKIN().button.hover.background;
        
        var YR = re;
        
#pragma warning disable
        //if (!Adapter.LITE && !A.IS_PROJECT()) YR.width /= 2;
#pragma warning restore
        
        var ar = 350 / 194f;
        var oldH = YR.height;
        YR.height = YR.width / ar;
        YR.y += (oldH - YR.height) / 2;
        
        
        var style = EditorStyles.miniButtonLeft;
        var tt = style.border.top;
        var bb = style.border.bottom;
        if (!EditorGUIUtility.isProSkin)
        {   style.border.top = 5;
            style.border.bottom = 5;
        }
        /* if (GUI.Button( YR, new GUIContent( "Video", null, "http://emem.store/wiki?=..." ),
                         EditorStyles.miniButtonRight ))
         {   Application.OpenURL( "https://www.youtube.com/watch?v=NrTQGNAkJZ4&t=7s" );
         }
         EditorGUIUtility.AddCursorRect( R, MouseCursor.Link );*/
        Adapter.GET_SKIN().button.normal.background = A.GetIcon( "YouTube" );
        Adapter.GET_SKIN().button.hover.background = Adapter.GET_SKIN().button.focused.background = Adapter.GET_SKIN().button.active.background = A.GetIcon( "YouTube_Hover" );
        
        if (GUI.Button( YR, new GUIContent( "", null, "https://www.youtube.com/watch?v=NrTQGNAkJZ4" ) ))
        {   // Application.OpenURL( "https://www.youtube.com/watch?v=NrTQGNAkJZ4" );
        
            // w.downloadHandler = new UnityEngine.Networking.DownloadHandler()
            // w.SendWebRequest();
            
            
            /*   form.AddField("username", "name");
               UnityWebRequest wwwSignin = UnityWebRequest.Post(URL, form);
                  Debug.Log("URL:" + URL);
            
            string jsonData = "";
            jsonData = "{Username:\"Pants\",Password:\"PantsLoveMe\"}";
            
            if (jsonData != null)
            {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);
            
            UploadHandlerRaw upHandler = new UploadHandlerRaw(data);
            upHandler.contentType = "application/json";
            wwwSignin.uploadHandler = upHandler;
            }*/
            
            A.VideoRequest();
            
            
        }
        
        EditorGUIUtility.AddCursorRect( YR, MouseCursor.Link );
        var cl = GUI.skin.label.normal.textColor;
        GUI.skin.label.normal.textColor = Color.white;
        LABEL(YR, "▶ Youtube Video", true, TextAnchor.MiddleCenter);
        GUI.skin.label.normal.textColor = cl;
        /* if (A.IS_HIERARCHY())
         {   Adapter.GET_SKIN().button.normal.background = A.GetIcon( "YouTube" );
             Adapter.GET_SKIN().button.hover.background = Adapter.GET_SKIN().button.focused.background = Adapter.GET_SKIN().button.active.background = A.GetIcon( "YouTube_Hover" );
             if (GUI.Button( YR, new GUIContent( "", null, "https://www.youtube.com/watch?v=NrTQGNAkJZ4&t=7s" ) )) Application.OpenURL( "https://www.youtube.com/watch?v=NrTQGNAkJZ4&t=7s" );
             EditorGUIUtility.AddCursorRect( YR, MouseCursor.Link );
         }
         if (A.IS_PROJECT())
         {   Adapter.GET_SKIN().button.normal.background = A.GetIcon( "YouTube PR" );
             Adapter.GET_SKIN().button.hover.background = Adapter.GET_SKIN().button.focused.background = Adapter.GET_SKIN().button.active.background = A.GetIcon( "YouTube PR Hover" );
             if (GUI.Button( YR, new GUIContent( "", null, "https://www.youtube.com/watch?v=GxdJTXErxME" ) )) Application.OpenURL( "https://www.youtube.com/watch?v=GxdJTXErxME" );
             EditorGUIUtility.AddCursorRect( YR, MouseCursor.Link );
        
         }
        
        
         if (!Adapter.LITE && A.IS_HIERARCHY())
         {   YR.x += YR.width;
             Adapter.GET_SKIN().button.normal.background = A.GetIcon( "YouTube2" );
             Adapter.GET_SKIN().button.hover.background = Adapter.GET_SKIN().button.focused.background = Adapter.GET_SKIN().button.active.background = A.GetIcon( "YouTube2_Hover" );
             if (GUI.Button( YR, new GUIContent( "", null, "https://www.youtube.com/watch?v=87joPsQ68MU" ) )) Application.OpenURL( "https://www.youtube.com/watch?v=87joPsQ68MU" );
             EditorGUIUtility.AddCursorRect( YR, MouseCursor.Link );
         }*/
        
        
        if (re.Contains( Event.current.mousePosition ))
        {   Repaint();
            //InternalEditorUtility.RepaintAllViews();
        }
        style.border.top = tt;
        style.border.bottom = bb ;
        Adapter.GET_SKIN().button.normal.background = B_normal;
        Adapter.GET_SKIN().button.active.background = B_active;
        Adapter.GET_SKIN().button.focused.background = B_focused;
        Adapter.GET_SKIN().button.hover.background = B_hover;
        
        GUI.EndClip();
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    internal void DrawItalic(string str)
    {   var f = Adapter.GET_SKIN().label.fontSize;
        var s = Adapter.GET_SKIN().label.fontStyle;
        Adapter.GET_SKIN().label.fontSize = 10;
        Adapter.GET_SKIN().label.fontStyle = FontStyle.Italic;
        GUILayout.Label( str );
        Adapter.GET_SKIN().label.fontSize = f;
        Adapter.GET_SKIN().label.fontStyle = s;
    }
    
    
    
    
    
    internal Color black = new Color(.2f, .2f, .2f, 1f);
    
    
    internal void DrawStroke(Rect R, string text, Color color, float outline = 1.4f)
    {   var c = Adapter.GET_SKIN().label.normal.textColor;
        //var cds = Adapter.GET_SKIN().label.fontStyle;
        Adapter.GET_SKIN().label.normal.textColor = color;
        // Adapter.GET_SKIN().label.fontStyle = FontStyle.Bold;
        R.x -= outline;
        R.y -= outline;
        LABEL( R, text );
        R.x += outline * 2;
        LABEL( R, text );
        R.y += outline * 2;
        LABEL( R, text );
        R.x -= outline * 2;
        LABEL( R, text );
        Adapter.GET_SKIN().label.normal.textColor = c;
        //Adapter.GET_SKIN().label.fontStyle = cds;
    }
    
    internal void DrawHeader(string str, bool simple = false)
    {   //  InitializeStyles();
        Space( 12 );
        var f = Adapter.GET_SKIN().label.fontSize;
        var s = Adapter.GET_SKIN().label.fontStyle;
        //Adapter.GET_SKIN().label.fontSize = 14;
        var c = Adapter.GET_SKIN().label.normal.textColor;
        
        BEGIN_PADDING(-10, 0);
        if (!simple)BEGIN_PADDING(5, 5);
        
        var R = GetControlRect(EditorGUIUtility.singleLineHeight);
        
        
        try
        {   if (Event.current.type == EventType.Repaint)
            {   var c2 = GUI.color;
                var c3 = c2;
                c3.a = 0.3f;
                // if (!EditorGUIUtility.isProSkin) GUI.color = c3;
                var RRR = new Rect(R.x - 10, R.y - 13, R.width + 10 +  PAD, R.height + 26);
                if (!simple)   A.SETUP_TITLEHOGLIGHT.Draw( RRR, "", false, false, false, false );
                GUI.color = c2;
                
                // if (!EditorGUIUtility.isProSkin)
                {   var PPP = 8;
                    RRR.x += PPP;
                    RRR.y += PPP;
                    RRR.width -= PPP * 2;
                    RRR.height -= PPP * 2;
                    Adapter.GET_SKIN().textArea.Draw( RRR, "", false, false, false, false );
                }
                
            }
        }
        catch
        {   Debug.LogError( "Error Loading Styles" );
        }
        var asdc = (Color)colors[EditorPrefs.GetInt( A.pluginname + "/" + "Plugin Menu Item", 0 )];
        //  asdc.a /= 1.5f;
        if (!simple)
        {   if (EditorGUIUtility.isProSkin)
            {   var rrr = R;
            
                rrr.height  = 2;
                rrr.y -= 3;
                Adapter.DrawRect(rrr, asdc);
                
            }
            else
                Adapter.DrawRect(R, asdc);
        }
        
        // R.x += 20;
        // Adapter.GET_SKIN().label.fontStyle = FontStyle.Bold;
        var boldStr = str.Split('(').FirstOrDefault();
        var italStr = str.Split('(').LastOrDefault();
        if (italStr == boldStr) italStr = null;
        str = "☢ " + (boldStr != null ? ("<b>" + boldStr + "</b>") : "") + (italStr != null ? ("<i>(" + italStr + "</i>") : "");
        if (!simple) DrawStroke( R, str, /*!EditorGUIUtility.isProSkin ? Color.white :*/ black );
        if (!simple)  if (!EditorGUIUtility.isProSkin) Adapter.GET_SKIN().label.normal.textColor = Color.white;
        LABEL(R, str);
        //GUI.Label( R, str );
        
        
        
        if (!simple)END_PADDING();
        END_PADDING();
        
        Adapter.GET_SKIN().label.fontSize = f;
        Adapter.GET_SKIN().label.fontStyle = s;
        Adapter.GET_SKIN().label.normal.textColor = c;
        Space( EditorGUIUtility.singleLineHeight * 1.5f );
        
    }
    
    
    
    internal void Space(float space)
    {   GetControlRect( space );
        // Y += space;
        // GUILayout.Space(space);
    }
    
    
    internal const int PAD = 20;
    
    internal static float X = 0;
    internal static float Y;
    internal static Rect tr = new Rect();
    internal static Rect pretr = new Rect();
    
    internal Rect GetLastRect()
    {   return tr;
    }
    internal static Rect GetPreLastRect()
    {   return pretr;
    }
    internal static Rect GetControlRect(float height, float divideWidth = 1, float width = -1)
    {   var padl = padding_list.Sum();
        var padr = paddingRIGHT_list.Sum();
        pretr = tr;
        tr.Set( padl + X, Y, (W - padr - padl) / divideWidth, height );
        if (height != 0) Y += height + 2;
        if (width != -1) tr.width = width / divideWidth;
        return tr;
    }
    internal Rect GetControlRectAndOffset(float height, int offset)
    {   var result = GetControlRect(height);
        result.x += offset;
        result.width -= offset;
        return result;
    }
    
    internal float[] padding_save = new float[0];
    internal float[] paddingRight_save = new float[0];
    internal void SAVE_ANDCLEAR_PADDING()
    {   if (padding_save.Length != padding_list.Count)
        {   Array.Resize( ref padding_save, padding_list.Count );
            Array.Resize( ref paddingRight_save, paddingRIGHT_list.Count );
        }
        for (int i = 0 ; i < padding_save.Length ; i++)
        {   padding_save[i] = padding_list[i];
            paddingRight_save[i] = paddingRIGHT_list[i];
        }
        
        padding_list.Clear();
        paddingRIGHT_list.Clear();
    }
    internal void RESTORE_PADDING()
    {   padding_list.Clear();
        paddingRIGHT_list.Clear();
        for (int i = 0 ; i < padding_save.Length ; i++)
        {   padding_list.Add( padding_save[i] );
            paddingRIGHT_list.Add( paddingRight_save[i] );
        }
    }
    
    internal static List<float> padding_list = new List<float>() { 0 };
    internal static List<float> paddingRIGHT_list = new List<float>() { 0 };
    internal static void BEGIN_PADDING(float padding, float rightPadding = 0)
    {   /*   GUILayout.BeginHorizontal();
           GUILayout.Space(padding);
           GUILayout.BeginVertical();*/
        padding_list.Add( padding );
        paddingRIGHT_list.Add( rightPadding );
    }
    internal static void END_PADDING()
    {   padding_list.RemoveAt( padding_list.Count - 1 );
        paddingRIGHT_list.RemoveAt( paddingRIGHT_list.Count - 1 );
        /*GUILayout.EndVertical();
        GUILayout.EndHorizontal();*/
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    internal void INTERNAL_BOX(Rect r, GUIContent c)
    {   Adapter.INTERNAL_BOX( r, c );
    }
    internal void INTERNAL_BOX(Rect r, string text)
    {   Adapter.INTERNAL_BOX( r, text );
    }
    internal void INTERNAL_BOX(Rect r)
    {   INTERNAL_BOX( r, "" );
    }
    
    
    
    internal void BEGIN_CATEGORY(ref float calW, bool drawnoise = true, Color? color = null, float? padding = null, float? NarrowBg = null)
    {
    
        padding = 5;
        
        if (NarrowBg.HasValue) BEGIN_PADDING(NarrowBg.Value, NarrowBg.Value);
        
        var R = GetControlRect((0));
        R.height = calW == 0 ? 500 : calW;
        R = new Rect(R.x, R.y + (NarrowBg ?? 0), R.width, R.height - (NarrowBg ?? 0));
        /*  if (Event.current.type == EventType.Repaint
                  && EditorGUIUtility.isProSkin ) A.SETUP_BOXCAT.Draw( R, "", false, false, false, false );*/
        // if (drawnoise && EditorGUIUtility.isProSkin) DrawTiled( new Rect( R.x + 3, R.y + 3, R.width - 6, R.height - 6 ), A.GetIcon( Adapter.ICONID.SETUP_NOISE ), 171 );
        // if (!EditorGUIUtility.isProSkin)
        {
        
        
        
        
            if (EditorGUIUtility.isProSkin)
            {
            
                if (Event.current.type == EventType.Repaint)   GUI.skin.box.Draw( SHRINK(R, -3), "", false, false, false, false );
                BOX( R );
            }
            else
            {   var oc = GUI.color;
                GUI.color *= new Color(1, 1, 1, EditorGUIUtility.isProSkin ? 1 : 0.2f);
                INTERNAL_BOX(  SHRINK(R, 0), "" );
                GUI.color = oc;
            }
            
            
            
            
            
            
            /* var asdasd = GUI.color;
             GUI.color *= new Color( 1, 1, 1, 0.2f );
             DrawTiled( new Rect( R.x + 3, R.y + 3, R.width - 6, R.height - 6 ), A.GetIcon( Adapter.ICONID.SETUP_NOISE ), 171 );
             //   DrawTiled(new Rect(R.x + 3, R.y + 3, R.width - 6, R.height - 6), GetSetubBGTexture(), 337);
             // DrawTiled(new Rect(R.x + 3, R.y + 3, R.width - 6, R.height - 6), GetIcon(ICONID.SET), 171);
             GUI.color = asdasd;*/
        }
        /*  if (drawnoise && !EditorGUIUtility.isProSkin)
          {   var asdasd = GUI.color;
              GUI.color *= new Color( 1, 1, 1, 0.2f );
              DrawTiled( new Rect( R.x + 3, R.y + 3, R.width - 6, R.height - 6 ), A.GetIcon( Adapter.ICONID.SETUP_NOISE ), 171 );
              GUI.color = asdasd;
          }*/
        if (color != null)
        {   /* if (EditorGUIUtility.isProSkin)
             {   var cc = GUI.color;
                 GUI.color *= color.Value;
                 GUI.DrawTexture( new Rect( R.x + 3, R.y + 3, R.width - 6, R.height - 6 ), Texture2D.whiteTexture );
                 GUI.color = cc;
             }*/
            
        }
        
        if (NarrowBg.HasValue) END_PADDING();
        
        if (Event.current.type == EventType.Repaint) calW = -(GetLastRect().y + GetLastRect().height);
        
        
        // BEGIN_PADDING( padding ?? PAD, padding ?? PAD );
        BEGIN_PADDING( PAD, PAD );
        
        
        /* if (Event.current.type == EventType.repaint)*/
        ///   BEGIN_PADDING( padding ?? PAD, 12 );
        /*   GUILayout.BeginHorizontal();
           GUILayout.Space(padding ?? PAD);
           GUILayout.BeginVertical();*/
    }
    internal void END_CATEGORY(ref float calW)
    {   if (Event.current.type == EventType.Repaint && calW < 0) calW = GetLastRect().y + GetLastRect().height - (-calW) + EditorGUIUtility.singleLineHeight;
        /* if (Event.current.type == EventType.repaint)*/
        END_PADDING();
        /*  GUILayout.EndVertical();
          GUILayout.Space(PAD);
          GUILayout.EndHorizontal();*/
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    internal void DrawTiled(Rect rect, Texture2D texture, int SIZE)
    {
    
        GUI.BeginClip( rect );
        float Y = -SIZE;
        do
        {   Y += SIZE;
            float X = -SIZE;
            do
            {   X += SIZE;
                GUI.DrawTexture( new Rect( X, Y, SIZE, SIZE ), texture );
            } while (X < rect.width);
        } while (Y < rect.height);
        GUI.EndClip();
    }
    internal float SLIDER(Rect rect, string title, float value, float left, float right)
    {
    
        var slid = new GUIStyle(Adapter.GET_SKIN().horizontalSliderThumb);
        slid.normal.background = slid.focused.background = slid.hover.background = A.SETUP_SLIDER.normal.background;
        slid.active.background = A.SETUP_SLIDER.active.background;
        // EditorStyles.numberField = SETUP_SLIDER;
        var oldS = Adapter.GET_SKIN().horizontalSliderThumb;
        Adapter.GET_SKIN().horizontalSliderThumb = slid;
        A.PREPARE_TEXTFIELD();
        // var result = EditorGUI.Slider(rect, title, value, left, right);
        
        var oldT = EditorStyles.label.richText;
        EditorStyles.label.richText = true;
        
        var result = title == null ? EditorGUI.Slider(rect, value, left, right) : EditorGUI.Slider(rect, title, value, left, right);
        
        EditorStyles.label.richText = oldT;
        
        A.RESTORE_TEXTFIELD();
        Adapter.GET_SKIN().horizontalSliderThumb = oldS;
        
        return result;
    }
    internal int SLIDER(string title, int value, int left, int right)
    {   var R = GET_OFFSETRECT(EditorGUIUtility.singleLineHeight);
        return SLIDER( R, title, value, left, right );
    }
    internal int SLIDER(Rect rect, string title, int value, int left, int right)
    {
    
        var slid = new GUIStyle(Adapter.GET_SKIN().horizontalSliderThumb);
        slid.normal.background = slid.focused.background = slid.hover.background = A.SETUP_SLIDER.normal.background;
        slid.active.background = A.SETUP_SLIDER.active.background;
        // EditorStyles.numberField = SETUP_SLIDER;
        var oldS = Adapter.GET_SKIN().horizontalSliderThumb;
        Adapter.GET_SKIN().horizontalSliderThumb = slid;
        A.PREPARE_TEXTFIELD();
        var result = title == null ? EditorGUI.IntSlider(rect, value, left, right) : EditorGUI.IntSlider(rect, title, value, left, right);
        A.RESTORE_TEXTFIELD();
        Adapter.GET_SKIN().horizontalSliderThumb = oldS;
        
        return result;
    }
    
    internal float FLOAT_FIELD(string title, float value)
    {   // var R = GetControlRect(false, GUILayout.Height(20));
        var R = GetControlRect(EditorGUIUtility.singleLineHeight);
        return FLOAT_FIELD( R, title, value );
    }
    internal float FLOAT_FIELD(Rect rect, string title, float value)
    {
    
        A.PREPARE_TEXTFIELD();
        var result = EditorGUI.FloatField(rect, title, value);
        A.RESTORE_TEXTFIELD();
        
        return result;
    }
    
    
    internal int INT_FIELD(string title, int value)
    {   var R = GetControlRect(EditorGUIUtility.singleLineHeight);
        // var R = GetControlRect(false, GUILayout.Height(20));
        return INT_FIELD( R, title, value );
    }
    internal int INT_FIELD(Rect rect, string title, int value)
    {
    
        A.PREPARE_TEXTFIELD();
        var result = EditorGUI.IntField(rect, title, value);
        A.RESTORE_TEXTFIELD();
        
        return result;
    }
    internal int INT_FIELD(Rect rect,  int value)
    {
    
        A.PREPARE_TEXTFIELD();
        GUI.BeginClip(rect);
        var result = EditorGUI.IntField(new Rect(0, 0, rect.width, rect.height), " ",  value);
        GUI.EndClip();
        if (Event.current.type == EventType.Repaint) Adapter.GET_SKIN().textField.Draw(rect, new GUIContent(value.ToString()), false, false, false, false);
        A.RESTORE_TEXTFIELD();
        
        return result;
    }
    
    /*   Rect GET_OFFSETRECT(float HEIGHT, GUILayoutOption[] p)
      {
          if (p == null) p = new GUILayoutOption[0];
          if (HEIGHT != -1)
          {
              Array.Resize(ref p, p.Length + 1);
              p[p.Length - 1] = GUILayout.Height(HEIGHT);
          }
    
          var R = GetControlRect(false, p);
          R.height = HEIGHT;
          return R;
      }*/
    internal Rect GET_OFFSETRECT(float HEIGHT = -1, float Yoffset = 0, float Xoffset = 0)
    {   var R = HEIGHT == -1 ? GetControlRect(EditorGUIUtility.singleLineHeight) : GetControlRect((HEIGHT + Yoffset));
        R.y += Yoffset;
        R.height = HEIGHT;
        R.x += Xoffset;
        R.width -= Xoffset;
        return R;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    GUIContent tempContent = new GUIContent();
    void HelpBox(string text, MessageType type)
    {   if (!DrawHelpImage) return;
        text = "☢ " + text;
        var rect = GetControlRect( 0 );
        tempContent.text = text;
        var h = EditorStyles.helpBox.CalcHeight( tempContent, rect.width - (type == MessageType.None ? 0 : 40) );
        if (type != MessageType.None && h < 32) h = 32;
        rect = GetControlRect( h );
        EditorGUI.HelpBox( rect, text, type );
    }
    
    void HelpBox(Rect rect, string text, MessageType type)
    {   if (!DrawHelpImage) return;
        text = "☢ " + text;
        tempContent.text = text;
        var h = EditorStyles.helpBox.CalcHeight( tempContent, rect.width - (type == MessageType.None ? 0 : 40) );
        if (type != MessageType.None && h < 32) h = 32;
        // rect = GetControlRect( h );
        EditorGUI.HelpBox( rect,  text, type );
    }
    
    void Label(string text)
    {   var rect = GetControlRect( 0 );
        tempContent.text = text;
        var h = EditorStyles.helpBox.CalcHeight( tempContent, rect.width );
        rect = GetControlRect( h );
        GUI.Label( rect, text );
    }
    
    /*   Texture2D GetNoiseIcon()
      {
          return EditorGUIUtility.isProSkin ? GetIcon(ICONID.SETUP_NOISE) : GetIcon("SETUP_NOISE_PERSONAL");
      }*/
}
}
