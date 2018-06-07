using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeline : MonoBehaviour
{
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
    // Use this for initialization
    public static TIMELINE timeline1;
    public static TIMELINE timeline2;
    void Start()
    {
        //// SETUP TIMELINE ------
        timeline1 = new TIMELINE();
        timeline2 = new TIMELINE();
        timeline1.name = "reading";
        timeline2.name = "thrusting";
        TIMELINE[] timelines = new TIMELINE[]{timeline1, timeline2};
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
    // Update is called once per frame

    void Update()
    {

    }

    public static void Log(object msg)
    {
        Debug.Log(msg);
    }
}

public partial class TIMELINE : timeline
{
    string stream = "timeline";
    public string name = "timeline";
    public int length = 1000;
    public ACCESS _access;
    private CODE.BINDING binding;
    private CODE.BUFFER buffer;
    private CODE.BUFFER interpolation;
    private CODE.TIMEFRAME timeframe;
    private GUI.CONTROL guiControl;
    private GUI.INSERT guiInsert;
    private GUI.DIALOG guiDialog;
    
    public TIMELINE()
    {
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