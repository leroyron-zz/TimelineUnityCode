using System;
using System.Collections.Generic;

public partial class Timeline
{
    public partial class Core
    {
        public Timeframe timeframe = new Timeframe();

        public partial class Timeframe
        {
            Timeline _timeline;
            //private Timeline.GUI gui;
            public delegate void DelegateProcess(); // setup to invoke a function before each frame//new Streaming.addon.timeframe.Process() {}
            public delegate void DelegateInvoke(); // setup to invoke a function after after frame//new Streaming.addon.timeframe.Invoke() {}
            public DelegateProcess Process;
            public DelegateInvoke Invoke;
            public bool _init = false;
            public bool ready = false;
            public bool running = false;
            //public bool control = false;
            public int lapse = 10;
            public int duration, _duration = 0;
            public int read = 0;
            public int thrust = 0;
            public string mode = "3d";
            public void Init(Timeline timeline)
            {
                this._timeline = timeline;
                //this.gui = timeline.gui;
            }
            public void InitGUI(Timeline timeline)
            {
                timeline.gui.enabled = GUI.initialized = true;
                this.onrevert.CallBacks = RevertCallbacks;
                
                // Delegated
                TimelineCode.Log("Timeframe GUI Prepare for ("+timeline.name+")");
            }
            public GUI.Bind.Runtime onruntime = new GUI.Bind.Runtime();
            public GUI.Bind.Revert onrevert = new GUI.Bind.Revert();
            public void Runtime() {
                // GUI update calls (seek)
                if (this._timeline.gui.enabled) this.onruntime.CallBacks();
            }
            public void Revert(int revertPos) {
                if (this._timeline.gui.enabled) this.onrevert.CallBacks(revertPos); 
                //this.RevertCallbacks(revertPos);
            }

            public void RevertCallbacks(int revertPos) {
                for (int r = 0; r < this.onrevert.Length; r++) {
                    // GUI revert calls (seek)
                    this.onrevert.Calls[r](revertPos);
                }
            }
            // reverting start stream from start
            public void _Revert(int revertPos) {
                Revert(revertPos);
            }

            public void Update(int position = -1) {
                //Time.Process = this.Process;
                //Time.invoke = this.invoke;
                //Time.length = this.length;
                //Time.running = this.running;
                Time.lapse = this.lapse;

                // GUI update calls this
                //if (this.updateCallbacks) this.UpdateCallbacks();

                // Always 3D mode
                this._timeline.access.process.UtilizeThrustData = TimeframeThrustingStreamingUtilizationAsRuntimeSumingValues;
                this._timeline.access.process.UtilizeReadData = TimeframeReadingStreamingUtilizationAsRuntimeGettingValues;
                /*if (this.mode == "3d") {
                    TimeframeThrustingStreamingUtilizationAsRuntimeSumingValuesForTHREE();
                    TimeframeReadingStreamingUtilizationAsRuntimeGettingValuesForTHREE();
                } else {
                    TimeframeThrustingStreamingUtilizationAsRuntimeSumingValues();
                    TimeframeReadingStreamingUtilizationAsRuntimeGettingValues();
                }*/
                SyncInTimeframe(position);
                _timeline.access.Update();
                TimeframeRuntimeStreamRevertCallAndForwardingRevertPositionValue(/*_revert*/);
            }

            void SyncInTimeframe(int position) {
                this._timeline.access.process.option = this._timeline.access.defaults.timeframe;
                this._timeline.access.process.method = "all";
                this._timeline.access.readCount = this._timeline.access.thrustCount = 1;
                if (position != -1) Syncing(position);
            }
            public void Syncing(int? position = null) {
                int modDuration = position ?? this.duration;
                this._timeline.access.SyncOffsets(modDuration);
            }

            public void Start(/*canvas*/) {
                /*this.mode =
                canvas
                    ? canvas.context
                        ? canvas.context.constructor.name == 'CanvasRenderingContext2D'
                            ? '2d'
                            : '3d'
                    : window.canvases
                        ? window.canvases['0'].context.constructor.name == 'CanvasRenderingContext2D'
                            ? '2d'
                            : '3d'
                    : null
                : null*/
                //this.length = _runtime[_runtime.access.stream].length;
                this._init = true;
            }

            public void ForceInit(/*canvas*/) {
                // FORCE resizing fix
                /*window.app.force = true
                window.onresize()
                this.mode =
                canvas
                    ? canvas.context
                        ? canvas.context.constructor.name == 'CanvasRenderingContext2D'
                            ? '2d'
                            : '3d'
                    : window.canvases
                        ? window.canvases['0'].context.constructor.name == 'CanvasRenderingContext2D'
                            ? '2d'
                            : '3d'
                    : null
                : null
                this.length = _runtime.access.access.prototype.length*/
                this._init = true;
                this.Ready();
            }
            public void Ready() {
                this.ready = true;
            }
            public void Run() {
                if (!this.running && this.ready) {
                    //Time.now = Time._then = Date.now();
                    //this.length = _runtime[_runtime.access.stream] ? _runtime[_runtime.access.stream].length : this.length;
                    this.running = true;
                    this.Update();
                    this.Frame();
                }
            }
            public void Stop(int at = -1) {
                if (this.running) {
                    this.running = false;
                    this.Update();
                    //window.cancelAnimationFrame(Time.animationFrameLoop);
                }
                if (at != -1) {
                    this.duration = at;
                    this.Syncing();
                }
            }
            public void Tick(bool exact, int number = 1) {
                Time.ByFrame.exact = exact || Time.ByFrame.exact;
                Time.ByFrame.number = number;
                this.running = false;
                this.Run();
            }
            /*public void keyPauseToggle(e) {
                if (e) {
                    if (e.keyCode != 32) {
                        return
                    }
                    if (this.running) {
                        this.Stop()
                    } else {
                        this.Run()
                    }
                }
            }*/
            public void Frame() {
                if (!this.running) {
                    return;
                }
                //window.stats.begin();
                //Time.now = Date.now();
                //Time._delta = Time.now - Time._then + Time._remain;
                //this[Time.access] = Time._timeFrame = Time._delta / 10 << 0;
                //this._duration = that.duration + Time._timeFrame;
                this.Process();
                //Time._remain = Time._delta - (Time.timeFrame / 100);// get remainder for precision
                //Time._then = Time.now;
                if (Time.timeFrame > 0 && Time.timeFrame < Time.lapse) {
                    this.Runtime();
                    if (!Time.ByFrame.exact) {
                        // values summed up overtime and passed to node properties
                        this._timeline.access.process.InvokeCall(Time.timeFrame);
                        if (Time.ByFrame.number > 0) {
                            if (Time.ByFrame.number == 1) {
                                this.Stop();
                            }
                            Time.ByFrame.number--;
                        }
                        // duration per successful frame
                        this.duration += Time.timeFrame;
                    } else if (Time.ByFrame.exact) {
                        this._timeline.access.process.InvokeCall(1);
                        Time.ByFrame.exact = false;
                        Time.ByFrame.number = 0;
                        this.Stop();
                    }
                    this.Invoke();
                } else if (Time.timeFrame > Time.lapse) {
                    // Time.lapse; if CPU halts the streams for too long then stop process
                }
                //Time.animationFrameLoop = window.requestAnimationFrame(frame);
                //window.stats.end();
            }
            public void SwitchToTimeFrameThrusting(int position) {
                //Time.access = "thrust";
                this._timeline.access.defaults.timeframe = "thrust";
                this._timeline.access.block = true;
                this.Update(position);
            }
            public void SwitchToTimeFrameReading(int position) {
                //Time.access = "read";
                this._timeline.access.defaults.timeframe = "read";
                this._timeline.access.block = true;
                this.Update(position);
            }
            void TimeframeThrustingStreamingUtilizationAsRuntimeSumingValues(float value, Bind.Property setBindProperty, int node, int property) {
                    if (value == 0) return;
                    //IDictionary<int, object> setBind = this._timeline.binding.ids[node];
                    //TLType setNode = (TLType)setBind[0];
                    //Bind.Property setBindProperty = (Bind.Property)this._timeline.binding.ids[node][property];
                    if (!setBindProperty.parameter.mute) setBindProperty.assign += value;
                    // TO-DO redo all demos to utilize property binding value and remove if else statement, uniform scheme
                    // Generate HashTable for optimization ex. idHash(node + property)
                    //if (setBindProperty.property != null) setBind.node[setBindProperty.property][setBindProperty.binding] = setBindProperty.value; else setBind.node[setBindProperty.binding] = setBindProperty.value;
            }
            void TimeframeReadingStreamingUtilizationAsRuntimeGettingValues(float value, Bind.Property setBindProperty, int node, int property) {
                //IDictionary<int, object> setBind = this._timeline.binding.ids[node];
                //Bind.Property setBindProperty = (Bind.Property)this._timeline.binding.ids[node][property];
                if (!setBindProperty.parameter.mute) setBindProperty.assign = value;
                // TO-DO redo all demos to utilize property binding value and remove if else statement, uniform scheme
                // Generate HashTable for optimization ex. idHash(node + property)
                //if (setBindProperty.property != null) setBind.node[setBindProperty.property][setBindProperty.binding] = setBindProperty.value; else setBind.node[setBindProperty.binding] = setBindProperty.value;
                //TLType param = setBind[0] as TLType;
                    //TLElement 
                //if (setBindProperty.property != null && setBindProperty.property == "x") 
                //param.x /*[setBindProperty.property][setBindProperty.binding]*/ = setBindProperty.value; 
                //else param.value/*[setBindProperty.binding]*/ = setBindProperty.value;
            }
            void TimeframeRuntimeStreamRevertCallAndForwardingRevertPositionValue() {
                this._timeline.access.process.OutputRevertCall = Revert;
            }
            public void ResetStreamProperties() {
                this._timeline.access.RevertFromTo(this._timeline.length, 0);
            }
        }
    }

    // Shared Time for Sync Timelines
    public static class Time {
        public static int now = 0;
        public static bool running = false;
        public static int lapse = 10; 
        public static int _then = 0;
        public static float _remain = 0; 
        public static float _timeFrame = 0;
        public static int timeFrame = 0;
        public static int _duration = 0;
        public static float _delta = 0;
        public static float delta {
            set {
                _delta = value + _remain;
                _timeFrame = _delta * 100f;
                // (int)timeFrame for array data assignments and insert invocation
                timeFrame = (int)_timeFrame;
                _duration += timeFrame;
                // needs remainder decimals to keep up on frame precision
                _remain = (float)timeFrame / 100f;
                _remain = _delta - _remain;
                Step();
            }
        }
        public static class ByFrame {
            public static int number = 0; 
            public static bool exact = false;
        };

        public static void Start()
        {
            for (int t = 0; t < TimelineCode.timelines.Length; t++) {
                TimelineCode.timelines[t].timeframe.Ready();
                TimelineCode.timelines[t].timeframe.Run();
            }
        }
        public static void Stop()
        {
            for (int t = 0; t < TimelineCode.timelines.Length; t++) {
                TimelineCode.timelines[t].timeframe.Stop();
            }
        }
        public static void Step()
        {
            for (int t = 0; t < TimelineCode.timelines.Length; t++) {
                TimelineCode.timelines[t].timeframe.Frame();
            }
        }
    }
}

