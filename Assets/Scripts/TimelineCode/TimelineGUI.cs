using UnityEngine;
using UnityEngine.Events;

// Move all to new Timeline.gui - hide for optimization
public partial class TimelineCode
{
    public bool enableInGameGUI = true;

    public int leftPad = 5, topPad = 5, widthPad = 10;

    public static int _leftPad, _topPad, _widthPad;

    //Timeline
    public Timeline.GUI.TLUIElement timelineGUI = new Timeline.GUI.TLUIElement();
    //public Rect timeline_area = new Rect(5, 5, Screen.width - 10, 85);1
    public Rect timelineGUI_box = new Rect(_leftPad, _topPad, Screen.width-_widthPad, 30);
    public GUIStyle timelineGUI_style = new GUIStyle();
    public GUIContent timelineGUI_content = GUIContent.none;

    // info
    public Timeline.GUI.TLUIElement infoUI = new Timeline.GUI.TLUIElement();
    public Rect info_area = new Rect(_leftPad/*script*/, 68/*script*/, 150, 15);
    public Rect info_box = new Rect(0, 0, 150, 15);
    public GUIStyle info_style = new GUIStyle();
    public GUIContent info_content = new GUIContent("Playback: press to play");
    public static GUIContent sample_content = new GUIContent("Playback: press to play");

    // dialog
    public Timeline.GUI.TLUIElement dialogUI = new Timeline.GUI.TLUIElement();
    public Rect dialog_area = new Rect((350 / 2) - (Screen.width/2), 115, 350, 350);
    public Rect dialog_box = new Rect(0, 0, 350, 350);
    public GUIStyle dialog_style = new GUIStyle();
    public GUIContent dialog_content = GUIContent.none;

    // span
    public Timeline.GUI.TLUIElement spanUI = new Timeline.GUI.TLUIElement();
    public Rect span_area = new Rect(_leftPad, _topPad, Screen.width-_widthPad, 68);
    //private Rect span_box = new Rect(0, 0, 5, 30);
    //public GUIStyle span_style = new GUIStyle();
    //public GUIContent span_content = GUIContent.none;

    // action
    public Timeline.GUI.TLUIElement actionUI = new Timeline.GUI.TLUIElement();
    public Rect action_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect action_box = new Rect(_leftPad, _topPad, 1, 21);
    public GUIStyle action_style = new GUIStyle();
    public GUIContent action_content = GUIContent.none;

    // comment
    public Timeline.GUI.TLUIElement commentUI = new Timeline.GUI.TLUIElement();
    public Rect comment_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect comment_box = new Rect(_leftPad, _topPad, 1, 21);
    public GUIStyle comment_style = new GUIStyle();
    public GUIContent comment_content = GUIContent.none;

    // segment
    public Timeline.GUI.TLUIElement segmentUI = new Timeline.GUI.TLUIElement();
    public Rect segment_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect segment_box = new Rect(_leftPad, _topPad, 1, 21);
    public GUIStyle segment_style = new GUIStyle();
    public GUIContent segment_content = GUIContent.none;

    // sound
    public Timeline.GUI.TLUIElement soundUI = new Timeline.GUI.TLUIElement();
    public Rect sound_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect sound_box = new Rect(_leftPad, _topPad, 1, 21);
    public GUIStyle sound_style = new GUIStyle();
    public GUIContent sound_content = GUIContent.none;

    // scrubber
    public Timeline.GUI.TLUIElement scrubberUI = new Timeline.GUI.TLUIElement();
    public Rect scrubber_box = new Rect(0, 44, Screen.width-_widthPad, 23);
    public GUIStyle scrubber_style = new GUIStyle();
    public GUIContent scrubber_content = GUIContent.none;

    // seek
    public Timeline.GUI.TLUIElement seekUI = new Timeline.GUI.TLUIElement();
    public Rect seek_area = new Rect(_leftPad, _topPad, 1, 21);
    public Rect seek_box = new Rect(_leftPad, 5+_topPad, 1, 21);
    public GUIStyle seek_style = new GUIStyle();
    public GUIContent seek_content = GUIContent.none;

    // slider
    public Timeline.GUI.TLUIElement sliderUI = new Timeline.GUI.TLUIElement();
    public Rect slider_box = new Rect(-20+_leftPad, 30+_topPad, 40, 36);
    public GUIStyle slider_style = new GUIStyle();
    public GUIContent slider_content = GUIContent.none;

     // left
    public Timeline.GUI.TLUIElement leftUI = new Timeline.GUI.TLUIElement();
    public Rect left_box = new Rect((_leftPad-3), _topPad, 3, 30);
    public GUIStyle left_style = new GUIStyle();
    public GUIContent left_content = GUIContent.none;

    // right
    public Timeline.GUI.TLUIElement rightUI = new Timeline.GUI.TLUIElement();
    public Rect right_box = new Rect(Screen.width-(_leftPad-3), _topPad, 3, 30);
    public GUIStyle right_style = new GUIStyle();
    public GUIContent right_content = GUIContent.none;

    void StartTimelineGUI() 
    {
        widthPad = leftPad*2;
        this.timelineGUI.Elements(
            timelineGUI_box,
            timelineGUI_style,
            timelineGUI_content
        );
        this.infoUI.Elements(
            info_area,
            info_box,
            info_style,
            info_content
        );
        this.dialogUI.Elements(
            dialog_area,
            dialog_box,
            dialog_style,
            dialog_content
        );
        this.spanUI.Elements(
            span_area
        );
        this.actionUI.Elements(
            action_area,
            action_box,
            action_style,
            action_content
        );
        this.commentUI.Elements(
            comment_area,
            comment_box,
            comment_style,
            comment_content
        );
        this.segmentUI.Elements(
            segment_area,
            segment_box,
            segment_style,
            segment_content
        );
        this.soundUI.Elements(
            sound_area,
            sound_box,
            sound_style,
            sound_content
        );
        this.scrubberUI.Elements(
            scrubber_box,
            scrubber_style,
            scrubber_content
        );
        this.seekUI.Elements(
            seek_area,
            seek_box,
            seek_style,
            seek_content
        );
        this.sliderUI.Elements(
            slider_box,
            slider_style,
            slider_content
        );
        this.leftUI.Elements(
            left_box,
            left_style,
            left_content
        );
        this.rightUI.Elements(
            right_box,
            right_style,
            right_content
        );
        
        timelineGUI.style.normal.background = // must be white to tint properly
        infoUI.style.normal.background =
        dialogUI.style.normal.background =
        scrubberUI.style.normal.background =
        seekUI.style.normal.background =
        // spanUI.style.normal.background =
        sliderUI.style.normal.background =
        leftUI.style.normal.background =
        rightUI.style.normal.background = Texture2D.whiteTexture;

        infoUI.style.normal.textColor = Color.white; // whatever you want
        infoUI.style.fontSize = 10;
        infoUI.style.alignment = TextAnchor.MiddleCenter;
        //StartTimelineControlGUI();
    }

    void StartTimelineControlGUI() {
        timelines[0].gui.timeframe.control.Start(this);
    }

    // editor
    public void OnSceneGUI()
    {
        if (!TimelineCode.running) return;
        RenderGUI();
    }

    void OnGUI()
    {
        /*if(Event.current.type == EventType.MouseDrag) {
            if(!isDragging) { 
                mouseDragStart = Event.current.mousePosition;
                isDragging = true;
            }
        }*/
        if (!enableInGameGUI || !TimelineCode.running) return;
            RenderGUI();
    }

    private bool isDragging = false;
    Vector2 mouseDragStart;
    void RenderGUI() {
        widthPad = leftPad*2;

        // timelineGUI 
        //GUILayout.BeginArea(timelineGUI.area);
        // // GUILayout.BeginVertical();

        GUI.backgroundColor = Color.grey;
        timelineGUI.box.x = leftPad;
        timelineGUI.box.width = Screen.width-widthPad;
        GUI.Button(timelineGUI.box, timelineGUI.content, timelineGUI.style);

            // dialog 
            dialogUI.area.x = (Screen.width/2) - (350 / 2);
            GUILayout.BeginArea(dialogUI.area);
            // GUILayout.BeginVertical();
                // content -->
                    // bg
                    GUI.backgroundColor = Color.black;
                    //GUI.Box(dialogUI.box, dialogUI.content, dialogUI.style);
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
            GUILayout.BeginArea(infoUI.area);
            // GUILayout.BeginVertical();
            // content -->
                // bg
                GUI.backgroundColor = Color.black; // change to image
                GUI.Box(infoUI.box, sample_content, infoUI.style);
            // info END
            // GUILayout.EndVertical();
            GUILayout.EndArea();

            // span
            spanUI.area.x = leftPad;
            spanUI.area.width = Screen.width-widthPad;
            GUILayout.BeginArea(spanUI.area);
            // GUILayout.BeginVertical();
                // scrubber
                scrubberUI.box.width =  Screen.width-widthPad;
                GUI.backgroundColor = Color.white;
                GUI.Button(scrubberUI.box, scrubberUI.content, scrubberUI.style);
                // seek
                // GUILayout.BeginVertical();
                GUI.backgroundColor = Color.white;
                seekUI.box.x = sliderUI.box.x = (sliderUI.proxy[0] * spanUI.area.width);
                GUI.Box(seekUI.box, seekUI.content, seekUI.style);
                    // slider
                    GUI.backgroundColor = Color.black;
                    sliderUI.box.x -= 20;
                    GUI.Button(sliderUI.box, sliderUI.content, sliderUI.style);
                // scrubber END
                // GUILayout.EndVertical();
            // span END
            // GUILayout.EndVertical();
            GUILayout.EndArea();

            // left
            GUI.backgroundColor = Color.blue;
            leftUI.box.x = (leftPad-3);
            GUI.Box(leftUI.box, leftUI.content, leftUI.style);

            // right
            GUI.backgroundColor = Color.blue;
            rightUI.box.x = Screen.width-(leftPad+3);
            GUI.Box(rightUI.box, rightUI.content, rightUI.style);

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

public partial class Timeline
{
    public GUI gui = new GUI();
    
    public partial class GUI
    {
        public static bool initialized = false;
        public bool enabled = false;
        Timeline _timeline;
        public void Init(Timeline timeline)
        {
            this._timeline = timeline;
            TimelineCode.Log("Gui Started");
            timeframe.control.Init(timeline);
        }

        public void Start(TimelineCode global, GUI gui) {
            gui.element = global.seekUI;
            gui.span = global.spanUI;
            gui.scrubber = global.scrubberUI;
            gui.info = global.infoUI;
            gui.slider = global.sliderUI;
        }
        public class TLUIElement {
            public Rect area;
            public Rect box;
            public GUIStyle style;
            public GUIContent content;
            public float[] proxy = new float[1];
            public TLUIElement(Rect box, GUIStyle style, GUIContent content) {
                this.area = this.box = box;
                this.style = style;
                this.content = content;
            }
            public TLUIElement(Rect area, Rect box, GUIStyle style, GUIContent content) {
                this.area = area;
                this.box = box;
                this.style = style;
                this.content = content;
            }
            public TLUIElement(Rect box) {
                this.area = this.box = box;
            }
            public TLUIElement() {
            }
            public TLUIElement Elements(Rect box, GUIStyle style, GUIContent content) {
                this.area = this.box = box;
                this.style = style;
                this.content = content;
                return this;
            }
            public TLUIElement Elements(Rect area, Rect box, GUIStyle style, GUIContent content) {
                this.area = area;
                this.box = box;
                this.style = style;
                this.content = content;
                return this;
            }
            public TLUIElement Elements(Rect box) {
                this.area = this.box = box;
                return this;
            }
            public TLUIElement Area(Rect area) {
                this.area = area;
                return this;
            }
            public TLUIElement Box(Rect box) {
                this.box = box;
                return this;
            }
            public TLUIElement Style(GUIStyle style) {
                this.style = style;
                return this;
            }
            public TLUIElement Content(GUIContent content) {
                this.content = content;
                return this;
            }
    }
    public TLUIElement element;
    public TLUIElement slider;
    public TLUIElement span;
    public TLUIElement scrubber;
    public TLUIElement info;
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
            dialogUI.guiDepth = 1;
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