using UnityEngine;
using UnityEngine.SceneManagement;
using TLExtensions;

public partial class TimelineCode
{
	public static bool running {
    get {
            return timelines == null ? false : timelines[0].code.timeframe.running;
        } 
    set {
            if (value) Timeline.Time.Start();
            else Timeline.Time.Stop();
            timelines[0].code.timeframe.running = timelines == null ? false : value;
        }
    }

    public static Timeline.Scenes.Scene scene;

    [TimelineCodeAttribute (new string[]{"Update()", "FixedUpdate()", "timeline"}, 100)]
    [Tooltip("Timelines (Default 2: Read and Thrust)")]
    public TimelineCodeShelf[] Timelines = new TimelineCodeShelf[2]{new TimelineCodeShelf("reading", false), new TimelineCodeShelf("thrusting", false)};
    
    // Use this for initialization
    void AwakeTimelines()
    {
        //Timeline.GUI.initialized = false;
        timelines = new Timeline[Timelines.Length];
        for (int t = 0; t < Timelines.Length; t++) {
            timelines[t] = new Timeline(Length, Timelines[t].name, Timelines[t].mute);
        }
    }
    
    int Test(int key)
    {
        Log(key);
        return 0;
    }
    int Test1(int key)
    {
        Log(key);
        return 0;
    }
    int Test2(object obj, int key)
    {
        Log(key);
        return 0;
    }

    public delegate void DelegateBuild();

    public static DelegateBuild Build = DefaultBuild; 

    public static void DefaultBuild () {
        timelines[0].code.binding.Build(timelines, () => {
            timelines[0].code.buffer.Build(timelines, () => {
                for (int t = 0; t < timelines.Length; t++) {
                    timelines[t].code.timeframe.Start();// after script load
                    timelines[t].code.timeframe.Ready();// after inserts and assets (images)
                    timelines[t].code.timeframe.ForceInit();// after script load & after inserts and assets
                    //timelines[t].code.timeframe.Run();
                    //timelines[t].gui.timeframe.control.Start();
                }
                TimelineCode.running = true;
                return 0;
            });
            return 0;
        });
    }
}

public partial class Timeline
{
    string stream = "timeline";
    public int length;
    public string name;
    public bool mute;
    public Access access;
    private Core.Binding binding;
    private Core.Buffer buffer;
    private Core.Buffer.Interpolation interpolation;
    private Core.Timeframe timeframe;

    public Timeline(int length = 1000, string name = "timeline", bool mute = false)
    {
        this.length = length;
        this.name = name;
        this.mute = mute;

        access = new Access(code);

        access.defaults.timeframe = "read";
        Params(true, 0, 0, 0, true, 0, -999999, false);

        ////Timeline code ----
        code.Init(this);

        //setup binding per timeline
        binding = code.binding;
        binding.Init(this);

        //setup buffer
        buffer = code.buffer;
        buffer.Init(this);

        //setup interpolation
        interpolation = buffer.interpolation;

        //setup timeframe
        timeframe = code.timeframe;
        timeframe.Init(this);

        if (!GUI.initialized) {
            string sceneName = SceneManager.GetActiveScene().name;
            ////
            TimelineCode.scene = scenes.Init(this, (Timeline.Scenes.Scene)this.scenes.GetMember(sceneName));

            ////Timeline gui ----
            gui.Init(this); // moved to timline gui Awake()
            timeframe.InitGUI(this);
        }
    }

    public partial class Access
    {
        Core code;
        Core.Binding binding;
        public Access(Core code)
        {
            this.code = code;
            this.binding = code.binding;
        }
    }

    // changing the access arguments
    public void Params(bool setcontinuance, int setskip, int setrCount, int settCount, bool setrevert, int setmCount, int setleap, bool setreset)
    {
        access.Update(setcontinuance, setskip, setrCount, settCount, setrevert, setmCount, setleap, setreset);
    }
}


[System.Serializable]
public class TimelineCodeShelf {
    public string name = "timeline";
    public bool mute = false;
    public TimelineCodeShelf(string name, bool mute) {
        this.name = name;
        this.mute = mute;
    }
}

public class TimelineCodeAttribute : PropertyAttribute
{
    public readonly string[] names;
	public string[] list;
    public TimelineCodeAttribute(string[] names, int length = 0) {
		if (length == 0 || names.Length == 0) { this.names = names; return; }
        int namesLength = names.Length - 1;
		this.list = new string[namesLength + length];
		for (int n = 0; n < namesLength; n++) this.list[n] = names[n];
		for (int i = 0; i < length; i++) this.list[namesLength+i] = names[namesLength] + i;
		this.names = this.list;	
	}
}