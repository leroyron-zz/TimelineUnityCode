using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public partial class TimelineCode : CommonMonoBehaviour
{

    public TimelineCode() {
        // All Common
        CommonMonoBehaviour.AddAwake(AwakeTimelines, 0);
        CommonMonoBehaviour.AddAwake(AwakeTimelineCode, 1);
        CommonMonoBehaviour.AddStart(StartTimelineGUI, 0);
        CommonMonoBehaviour.AddStart(StartTimelineControlGUI, 1);
        CommonMonoBehaviour.AddUpdate(UpdateTimelineCode, 0);
        CommonMonoBehaviour.AddOnGUI(OnGUITimelineGUI, 0);
        CommonMonoBehaviour.AddOnSceneGUI(OnSceneGUITimelineGUI, 0);
    }
    
    [Tooltip("Duration of Timeline (Buffer Length)")]
    public int Length = 1000;

    internal static Timeline[] timelines;
	internal static Timeline timeline1;
    internal static Timeline timeline2;
    
    public Text CavButText;
    // Update is called once per frame
    void AwakeTimelineCode()
    {
        CavButText = GameObject.Find("Canvas/Button/Text").GetComponent<Text>();
        timeline1 = timelines[0];
        timeline2 = timelines[1];
        timeline1.access.defaults.timeframe = "read";
        timeline2.access.defaults.timeframe = "thrust";
    
        //// SETUP Timeline ------
        timeline1.access.AddUpdateCallback("var 1", (int key) => { Log(key); return 0; });
        timeline1.access.AddUpdateCallback("var 2", Test);
        timeline1.access.AddUpdateCallback("var 3", Test1);
        timeline1.access.AddDevertCallback(new {var1 = "var1",
        var2 = 9}, Test2);
        //timeline.Params(true, 3, 0, 0, true, 0, -999999, false);

        // All above variables are declared and assigned and can be used in Scenes App.cs script
        TimelineCode.scene.Start(timelines);

        //TimelineCode.Build();// This Is Default
        TimelineCode.Build = this.AutoBuild; // Reassigned, but Same function as default
        TimelineCode.Build();
    }

    // Custom build
    void CustomBuild () {
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

    void AutoBuild () {
        StartCoroutine(PerformCoroutine());
    }

    void PerformInit () {
        timelines[0].code.binding.Build(timelines, () => {
            timelines[0].code.buffer.Build(timelines, () => {
                for (int t = 0; t < timelines.Length; t++) {
                    timelines[t].code.timeframe.Start();// after script load
                    //timelines[t].code.timeframe.Ready();// after inserts and assets (images)
                    //timelines[t].code.timeframe.ForceInit();// after script load & after inserts and assets
                    //timelines[t].code.timeframe.Run();
                    //timelines[t].gui.timeframe.control.Start();
                    
                }
                return 0;
            });
            return 0;
        });
        TimelineCode.userReady = true;// Put this and set to true anywhere you'll think the end of your load will take place
    }

    void PerformReady () {
        TimelineCode.running = true;
    }

    public static bool userReady = false;// Put this and set to true anywhere you'll think the end of your load will take place
    IEnumerator PerformCoroutine() {
        yield return new WaitForSeconds(1f);
            if (TimelineCode.scene._init) {
                PerformInit();
                    yield return new WaitForSeconds(1f);
                    if (TimelineCode.userReady) {
                        PerformReady();
                    } else {
                        PerformCoroutine();
                    }
            } else {
                PerformCoroutine();
            }
    }

    void UpdateTimelineCode()
    {
        Timeline.Time.delta = Time.deltaTime;
        CavButText.text = 
        TimelineCode.sample_content.text = timelines[0].code.timeframe.duration.ToString();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            TimelineCode.running = false;
        }
    }

    public static void Log(object msg)
    {
        Debug.Log("Timeline: " + msg);
    }
}