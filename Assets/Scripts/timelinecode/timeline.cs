using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TLExtensions;

public partial class timeline : MonoBehaviour
{
    public static bool running = false;
    [Tooltip("Duration of Timeline (Buffer Length)")]
    public int Length = 1000;
    
    [TIMELINEAttribute (new string[]{"Update()", "FixedUpdate()", "timeline"}, 100)]
    [Tooltip("Timelines (Default 2: Read and Thrust)")]
    public TIMELINEShelf[] Timelines;
    public timeline () {
        Timelines = new TIMELINEShelf[2]{new TIMELINEShelf("reading", false), new TIMELINEShelf("thrusting", false)};
    }

    internal static TIMELINE timeline1;
    internal static TIMELINE timeline2;
    internal static TIMELINE[] timelines;
    
    // Use this for initialization
    void Awake()
    {
        timelines = new TIMELINE[Timelines.Length];
        for (int t = 0; t < Timelines.Length; t++) {
            timelines[t] = new TIMELINE(Length, Timelines[t].name, Timelines[t].mute);
        }
        timeline1 = timelines[0];
        timeline2 = timelines[1];
        timeline1._access.defaults.timeframe = "read";
        timeline2._access.defaults.timeframe = "thrust";

        //// SETUP TIMELINE ------
        timeline1._access.addUpdateCallback("var 1", (int key) => { Log(key); return 0; });
        timeline1._access.addUpdateCallback("var 2", test);
        timeline1._access.addUpdateCallback("var 3", test1);
        timeline1._access.addDevertCallback(new {var1 = "var1",
        var2 = 9}, test2);
        //timeline.access(true, 3, 0, 0, true, 0, -999999, false);

        // Open TimelineCode For Scene
        string sceneName = SceneManager.GetActiveScene().name;
        TIMELINE.SCENES.SCENE scene = (TIMELINE.SCENES.SCENE)timeline1.scenes.GetMember(sceneName);
        scene.init(timelines);
        scene.start();
        
        timeline1.code.binding.build(timelines, () => {
            timeline1.code.buffer.build(timelines, () => {
                for (int t = 0; t < timelines.Length; t++) {
                    timelines[t].code.timeframe.start();// after script load
                    timelines[t].code.timeframe._ready();// after inserts and assets (images)
                    timelines[t].code.timeframe._forceInit();// after script load & after inserts and assets
                    timelines[t].code.timeframe.run();
                    //timelines[t].gui.timeframe.control.start();
                }
                return 0;
            });
            return 0;
        });
        CavButText = GameObject.Find("Canvas/Button/Text").GetComponent<Text>();
    }
    public Text CavButText;
    // Update is called once per frame
    void Update()
    {
        TIMELINE.TIME.delta = Time.deltaTime;
        CavButText.text = 
        timeline.sample_content.text = timelines[0].code.timeframe.duration.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
            TIMELINE.running = false;
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
    int test2(object obj, int key)
    {
        Log(key);
        return 0;
    }
    public static void Log(object msg)
    {
        Debug.Log("TIMELINE: " + msg);
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

        //setup binding per timeline
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

        if (!GUI.initialized) {
            timeframe.initGUI(this);
            ////
            scenes.init(this);

            ////TIMELINE gui ----
            gui.init(this); // moved to timline gui Awake()
        }
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


[System.Serializable]
public class TIMELINEShelf {
    public string name = "timeline";
    public bool mute = false;
    public TIMELINEShelf (string name, bool mute) {
        this.name = name;
        this.mute = mute;
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