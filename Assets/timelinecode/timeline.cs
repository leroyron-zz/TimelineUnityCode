using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeline : MonoBehaviour
{
    [Tooltip("Duration of Timeline")]
    public int bufferLength = 1000;
    
    [TIMELINEAttribute (new string[]{"FixedUpdate()", "Update()", "timeline"}, 100)]
    [Tooltip("Timelines (Default 2: Read and Thrust)")]
    public TIMELINEShelf[] timeLines;
    public timeline () {
        timeLines = new TIMELINEShelf[2]{new TIMELINEShelf("thursting", false), new TIMELINEShelf("reading", false)};
    }

    internal static TIMELINE timeline1;
    internal static TIMELINE timeline2;
    internal TIMELINE[] timelines;
    
    // Use this for initialization
    void Start()
    {
        timelines = new TIMELINE[timeLines.Length];
        for (int t = 0; t < timeLines.Length; t++) {
            timelines[t] = new TIMELINE(bufferLength, timeLines[t].name, timeLines[t].mute);
        }
        timeline1 = timelines[0];
        timeline2 = timelines[1];

        //// SETUP TIMELINE ------
        timeline1._access.addUpdateCallback("var 1", (int key) => { Log(key); return 0; });
        timeline1._access.addUpdateCallback("var 2", test);
        timeline1._access.addUpdateCallback("var 3", test1);
        //timeline.access(true, 3, 0, 0, true, 0, -999999, false);

        timeline1.scenes.demo1.init(timelines);

        timeline1.scenes.demo1.start();
        
        timeline1.code.binding.build(timelines, () => {
            timeline1.code.buffer.build(timelines, () => {
                // run the game logic
                return 0;
            });
            return 0;
        });
    }
    int test(int key)
    {
        Log(key);
        return 0;
    }

    int test1(int key)
    {
        Log(key);
        return 0;
    }
    // Update is called once per frame
    void Update() 
    {

    }
    public static void Log(object msg)
    {
        Debug.Log(msg);
    }
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

    //Control control = new Control();

    //Dialog dialog = new Dialog();
}



[System.Serializable]
public class TIMELINEShelf {
    public string name = "timeline";
    public bool mute = false;
    public TIMELINEShelf (string name, bool mute) {
        this.name = name;
        this.mute = mute;
    }
}

public partial class TIMELINE : timeline
{
    string stream = "timeline";
    public int length;
    public string name;
    public bool mute;
    public ACCESS _access;
    private CODE.BINDING binding;
    private CODE.BUFFER buffer;
    private CODE.BUFFER interpolation;
    private CODE.TIMEFRAME timeframe;
    private GUI.CONTROL guiControl;
    private GUI.INSERT guiInsert;
    private GUI.DIALOG guiDialog;
    public TIMELINE(int length = 1000, string name = "timeline", bool mute = false)
    {
        this.length = length;
        this.name = name;
        this.mute = mute;

        _access = new ACCESS(code);

        _access.defaults.timeframe = "read";
        access(true, 0, 0, 0, true, 0, -999999, false);

        ////TIMELINE code ----
        code.init(this);

        //setup binding
        binding = code.binding;
        binding.init(this);

        //setup buffer
        buffer = code.buffer;
        buffer.init(this);
        //setup interpolation
        interpolation = buffer.interpolation;

        //setup timeframe
        timeframe = code.timeframe;
        timeframe.init(this);

        ////TIMELINE gui ----
        gui.init(this);

        // gui control
        guiControl = this.gui.control;
        guiControl.init(this);

        // gui insert
        guiInsert = gui.insert;
        guiInsert.init(this);

        // gui dialog
        guiDialog = gui.dialog;
        guiDialog.init(this);

        scenes.init(this);
    }

    public partial class ACCESS
    {
        CODE code;
        CODE.BINDING binding;
        public ACCESS(CODE code)
        {
            this.code = code;
            this.binding = code.binding;
        }
    }

    // changing the access arguments
    public void access(bool setcontinuance, int setskip, int setrCount, int settCount, bool setrevert, int setmCount, int setleap, bool setreset)
    {
        _access.update(setcontinuance, setskip, setrCount, settCount, setrevert, setmCount, setleap, setreset);
    }
}

public class TIMELINEAttribute : PropertyAttribute
{
    public readonly string[] names;
	public string[] list;
    public TIMELINEAttribute(string[] names, int length = 0) {
		if (length == 0 || names.Length == 0) { this.names = names; return; }
        int namesLength = names.Length - 1;
		this.list = new string[namesLength + length];
		for (int n = 0; n < namesLength; n++) this.list[n] = names[n];
		for (int i = 0; i < length; i++) this.list[namesLength+i] = names[namesLength] + i;
		this.names = this.list;	
	}
}
// Makes this button go back in depth over the example1 class one.

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