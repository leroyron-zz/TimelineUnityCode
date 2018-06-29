using UnityEngine;
using UnityEngine.Events;

// Move all to new TIMELINE.gui - hide for optimization
public partial class timeline : MonoBehaviour
{
    public bool enableInGameGUI = true;

    public int leftPad = 5, topPad = 5, widthPad = 10;

    public static int _leftPad, _topPad, _widthPad;

    //Timeline
    public TIMELINE.GUI.ELEMENT timelineGUI = new TIMELINE.GUI.ELEMENT();
    //public Rect timeline_area = new Rect(5, 5, Screen.width - 10, 85);1
    public Rect timelineGUI_box = new Rect(_leftPad, _topPad, Screen.width-_widthPad, 30);
    public GUIStyle timelineGUI_style = new GUIStyle();
    public GUIContent timelineGUI_content = GUIContent.none;

    // info
    public TIMELINE.GUI.ELEMENT info = new TIMELINE.GUI.ELEMENT();
    public Rect info_area = new Rect(_leftPad/*script*/, 68/*script*/, 150, 15);
    public Rect info_box = new Rect(0, 0, 150, 15);
    public GUIStyle info_style = new GUIStyle();
    public GUIContent info_content = new GUIContent("Playback: press to play");
    public static GUIContent sample_content = new GUIContent("Playback: press to play");

    // dialog
    public TIMELINE.GUI.ELEMENT dialog = new TIMELINE.GUI.ELEMENT();
    public Rect dialog_area = new Rect((350 / 2) - (Screen.width/2), 115, 350, 350);
    public Rect dialog_box = new Rect(0, 0, 350, 350);
    public GUIStyle dialog_style = new GUIStyle();
    public GUIContent dialog_content = GUIContent.none;

    // span
    public TIMELINE.GUI.ELEMENT span = new TIMELINE.GUI.ELEMENT();
    public Rect span_area = new Rect(_leftPad, _topPad, Screen.width-_widthPad, 68);
    //private Rect span_box = new Rect(0, 0, 5, 30);
    //public GUIStyle span_style = new GUIStyle();
    //public GUIContent span_content = GUIContent.none;

    // action
    public TIMELINE.GUI.ELEMENT action = new TIMELINE.GUI.ELEMENT();
    public Rect action_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect action_box = new Rect(_leftPad, _topPad, 1, 21);
    public GUIStyle action_style = new GUIStyle();
    public GUIContent action_content = GUIContent.none;

    // comment
    public TIMELINE.GUI.ELEMENT comment = new TIMELINE.GUI.ELEMENT();
    public Rect comment_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect comment_box = new Rect(_leftPad, _topPad, 1, 21);
    public GUIStyle comment_style = new GUIStyle();
    public GUIContent comment_content = GUIContent.none;

    // segment
    public TIMELINE.GUI.ELEMENT segment = new TIMELINE.GUI.ELEMENT();
    public Rect segment_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect segment_box = new Rect(_leftPad, _topPad, 1, 21);
    public GUIStyle segment_style = new GUIStyle();
    public GUIContent segment_content = GUIContent.none;

    // sound
    public TIMELINE.GUI.ELEMENT sound = new TIMELINE.GUI.ELEMENT();
    public Rect sound_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect sound_box = new Rect(_leftPad, _topPad, 1, 21);
    public GUIStyle sound_style = new GUIStyle();
    public GUIContent sound_content = GUIContent.none;

    // scrubber
    public TIMELINE.GUI.ELEMENT scrubber = new TIMELINE.GUI.ELEMENT();
    public Rect scrubber_box = new Rect(0, 44, Screen.width-_widthPad, 23);
    public GUIStyle scrubber_style = new GUIStyle();
    public GUIContent scrubber_content = GUIContent.none;

    // seek
    public TIMELINE.GUI.ELEMENT seek = new TIMELINE.GUI.ELEMENT();
    public Rect seek_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect seek_box = new Rect(_leftPad, 5+_topPad, 1, 21);
    public GUIStyle seek_style = new GUIStyle();
    public GUIContent seek_content = GUIContent.none;

    // slider
    public TIMELINE.GUI.ELEMENT slider = new TIMELINE.GUI.ELEMENT();
    public Rect slider_box = new Rect(-20+_leftPad, 30+_topPad, 40, 36);
    public GUIStyle slider_style = new GUIStyle();
    public GUIContent slider_content = GUIContent.none;

     // left
    public TIMELINE.GUI.ELEMENT left = new TIMELINE.GUI.ELEMENT();
    public Rect left_box = new Rect((_leftPad-3), _topPad, 3, 30);
    public GUIStyle left_style = new GUIStyle();
    public GUIContent left_content = GUIContent.none;

    // right
    public TIMELINE.GUI.ELEMENT right = new TIMELINE.GUI.ELEMENT();
    public Rect right_box = new Rect(Screen.width-(_leftPad-3), _topPad, 3, 30);
    public GUIStyle right_style = new GUIStyle();
    public GUIContent right_content = GUIContent.none;

    void Start () {
        widthPad = leftPad*2;
        this.timelineGUI.Elements(
            timelineGUI_box,
            timelineGUI_style,
            timelineGUI_content
        );
        this.info.Elements(
            info_area,
            info_box,
            info_style,
            info_content
        );
        this.dialog.Elements(
            dialog_area,
            dialog_box,
            dialog_style,
            dialog_content
        );
        this.span.Elements(
            span_area
        );
        this.action.Elements(
            action_area,
            action_box,
            action_style,
            action_content
        );
        this.comment.Elements(
            comment_area,
            comment_box,
            comment_style,
            comment_content
        );
        this.segment.Elements(
            segment_area,
            segment_box,
            segment_style,
            segment_content
        );
        this.sound.Elements(
            sound_area,
            sound_box,
            sound_style,
            sound_content
        );
        this.scrubber.Elements(
            scrubber_box,
            scrubber_style,
            scrubber_content
        );
        this.seek.Elements(
            seek_area,
            seek_box,
            seek_style,
            seek_content
        );
        this.slider.Elements(
            slider_box,
            slider_style,
            slider_content
        );
        this.left.Elements(
            left_box,
            left_style,
            left_content
        );
        this.right.Elements(
            right_box,
            right_style,
            right_content
        );
        
        timelineGUI.style.normal.background = // must be white to tint properly
        info.style.normal.background =
        dialog.style.normal.background =
        scrubber.style.normal.background =
        seek.style.normal.background =
        // span.style.normal.background =
        slider.style.normal.background =
        left.style.normal.background =
        right.style.normal.background = Texture2D.whiteTexture;

        info.style.normal.textColor = Color.white; // whatever you want
        info.style.fontSize = 10;
        info.style.alignment = TextAnchor.MiddleCenter;

        timelines[0].gui.timeframe.control.start(this);
    }

    // editor
    public void OnSceneGUI()
    {
        if (!TIMELINE.running) return;
        gui();
    }

    void OnGUI()
    {
        if(Event.current.type == EventType.MouseDrag) {
            if(!isDragging) { 
                mouseDragStart = Event.current.mousePosition;
                isDragging = true;
            }
        }
        if (!enableInGameGUI || !TIMELINE.running) return;
            gui();
    }

    private bool isDragging = false;
    Vector2 mouseDragStart;
    void gui () {
        widthPad = leftPad*2;

        // timelineGUI 
        //GUILayout.BeginArea(timelineGUI.area);
        // // GUILayout.BeginVertical();

        GUI.backgroundColor = Color.grey;
        timelineGUI.box.x = leftPad;
        timelineGUI.box.width = Screen.width-widthPad;
        GUI.Button(timelineGUI.box, timelineGUI.content, timelineGUI.style);

            // dialog 
            dialog.area.x = (Screen.width/2) - (350 / 2);
            GUILayout.BeginArea(dialog.area);
            // GUILayout.BeginVertical();
                // content -->
                    // bg
                    GUI.backgroundColor = Color.black;
                    //GUI.Box(dialog.box, dialog.content, dialog.style);
                    // title
                    // description
                    // option -- >
                        // text
                        // input
                        // input
                        // input
                        // input
            // info END
            // GUILayout.EndVertical();
            GUILayout.EndArea();

            // info
            GUILayout.BeginArea(info.area);
            // GUILayout.BeginVertical();
            // content -->
                // bg
                GUI.backgroundColor = Color.black; // change to image
                GUI.Box(info.box, sample_content, info.style);
            // info END
            // GUILayout.EndVertical();
            GUILayout.EndArea();

            // span
            span.area.x = leftPad;
            span.area.width = Screen.width-widthPad;
            GUILayout.BeginArea(span.area);
            // GUILayout.BeginVertical();
                // scrubber
                scrubber.box.width =  Screen.width-widthPad;
                GUI.backgroundColor = Color.white;
                GUI.Button(scrubber.box, scrubber.content, scrubber.style);
                // seek
                // GUILayout.BeginVertical();
                GUI.backgroundColor = Color.white;
                seek.box.x = slider.box.x = (slider.proxy[0] * span.area.width);
                GUI.Box(seek.box, seek.content, seek.style);
                    // slider
                    GUI.backgroundColor = Color.black;
                    slider.box.x -= 20;
                    GUI.Button(slider.box, slider.content, slider.style);
                // scrubber END
                // GUILayout.EndVertical();
            // span END
            // GUILayout.EndVertical();
            GUILayout.EndArea();

            // left
            GUI.backgroundColor = Color.blue;
            left.box.x = (leftPad-3);
            GUI.Box(left.box, left.content, left.style);

            // right
            GUI.backgroundColor = Color.blue;
            right.box.x = Screen.width-(leftPad+3);
            GUI.Box(right.box, right.content, right.style);

        // timelineGUI END 
        // GUILayout.EndVertical();
        //GUILayout.EndArea();
        /*
        GUILayout.BeginArea(new Rect(5, 5, Screen.width-10, 60));
        GUI.Box(new Rect(0, 0, Screen.width-10, 20), GUIContent.none);
        GUI.Box(new Rect(0, 20, Screen.width-10, 20), GUIContent.none, whiteBox);
        
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //GUIInserts.Begin();
        //GUIInserts.Update();
        GUILayout.Label("Rotate", whiteText);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.red;
        
        if (GUILayout.Button("Left")) {
            //RotateLeft();
        }
        
        if (GUILayout.Button("Right")) {
            //RotateRight();
        }
        
        GUILayout.EndHorizontal();
        
        
        
        GUILayout.EndArea();
        
    }
    */

        //Control control = new Control();

        //Dialog dialog = new Dialog();
    }
}

public partial class TIMELINE
{
    public GUI gui = new GUI();
    
    public partial class GUI
    {
        public static bool initialized = false;
        public bool enabled = false;
        private TIMELINE timeline;
        public void init(TIMELINE timeline)
        {
            this.timeline = timeline;
            TIMELINE.Log("Gui Started");
            timeframe.control.init(timeline);
        }

        public void start (timeline global, GUI gui) {
            gui.element = global.seek;
            gui.span = global.span;
            gui.scrubber = global.scrubber;
            gui.info = global.info;
            gui.slider = global.slider;
        }
        public class ELEMENT {
            public Rect area;
            public Rect box;
            public GUIStyle style;
            public GUIContent content;
            public float[] proxy = new float[1];
            public ELEMENT (Rect box, GUIStyle style, GUIContent content) {
                this.area = this.box = box;
                this.style = style;
                this.content = content;
            }
            public ELEMENT (Rect area, Rect box, GUIStyle style, GUIContent content) {
                this.area = area;
                this.box = box;
                this.style = style;
                this.content = content;
            }
            public ELEMENT (Rect box) {
                this.area = this.box = box;
            }
            public ELEMENT () {
            }
            public ELEMENT Elements (Rect box, GUIStyle style, GUIContent content) {
                this.area = this.box = box;
                this.style = style;
                this.content = content;
                return this;
            }
            public ELEMENT Elements (Rect area, Rect box, GUIStyle style, GUIContent content) {
                this.area = area;
                this.box = box;
                this.style = style;
                this.content = content;
                return this;
            }
            public ELEMENT Elements (Rect box) {
                this.area = this.box = box;
                return this;
            }
            public ELEMENT Area (Rect area) {
                this.area = area;
                return this;
            }
            public ELEMENT Box (Rect box) {
                this.box = box;
                return this;
            }
            public ELEMENT Style (GUIStyle style) {
                this.style = style;
                return this;
            }
            public ELEMENT Content (GUIContent content) {
                this.content = content;
                return this;
            }
    }
    public ELEMENT element;
    public ELEMENT slider;
    public ELEMENT span;
    public ELEMENT scrubber;
    public ELEMENT info;
    }
}


// Makes this button go back in depth over the example1 class one.
/*
public class Control : MonoBehaviour
{
    public int guiDepth = 1;
    public Dialog dialog;

    void OnGUI()
    {
        GUI.depth = guiDepth;

        if (GUI.RepeatButton(new Rect(0, 0, 100, 100), "Bring Forward"))
        {
            guiDepth = 0;
            dialog.guiDepth = 1;
        }
    }
}

public class Dialog : MonoBehaviour
{
    public int guiDepth = 1;
    public Control control;

    void OnGUI()
    {
        GUI.depth = guiDepth;

        if (GUI.RepeatButton(new Rect(0, 0, 100, 100), "Bring Forward"))
        {
            guiDepth = 0;
            control.guiDepth = 1;
        }
    }
}
*/