using System;
using System.Collections.Generic;

public partial class TIMELINE
{
    public partial class CODE
    {
        public TIMEFRAME timeframe = new TIMEFRAME();

        public partial class TIMEFRAME
        {
            public delegate void delegateProcess(); // setup to invoke a function before each frame//new Streaming.addon.timeframe.process() {}
            public delegate void delegateInvoke(); // setup to invoke a function after after frame//new Streaming.addon.timeframe.invoke() {}
            public delegateProcess process;
            public delegateInvoke invoke;
            public bool _init = false;
            public bool ready = false;
            public bool running = false;
            //public bool control = false;
            public int lapse = 10;
            public int duration, _duration = 0;
            public int read = 0;
            public int thrust = 0;
            public string mode = "3d";
            private TIMELINE timeline;
            //private TIMELINE.GUI gui;
            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                //this.gui = timeline.gui;
            }

            public void initGUI(TIMELINE timeline)
            {
                timeline.gui.enabled = GUI.initialized = true;
                this.onrevert.CallBacks = revertCallbacks;
                
                // delegated
                TIMELINE.Log("Timeframe GUI Prepare for ("+timeline.name+")");
            }
            public GUI.BIND.RUNTIME onruntime = new GUI.BIND.RUNTIME();
            public GUI.BIND.REVERT onrevert = new GUI.BIND.REVERT();
            public void runtime() {
                // GUI update calls (SEEK)
                if (this.timeline.gui.enabled) this.onruntime.CallBacks();
            }
            public void revert(int revertPos) {
                if (this.timeline.gui.enabled) this.onrevert.CallBacks(revertPos); 
                //this.revertCallbacks(revertPos);
            }

            public void revertCallbacks(int revertPos) {
                for (int r = 0; r < this.onrevert.Length; r++) {
                    // GUI revert calls (SEEK)
                    this.onrevert.Calls[r](revertPos);
                }
            }
            // reverting start stream from start
            public void _revert(int revertPos) {
                revert(revertPos);
            }

            public void update(int position = -1) {
                //TIME.process = this.process;
                //TIME.invoke = this.invoke;
                //TIME.length = this.length;
                //TIME.running = this.running;
                TIME.lapse = this.lapse;

                // GUI update calls this
                //if (this.updateCallbacks) this.updateCallbacks();

                // Always 3D mode
                this.timeline._access.process.utilizeThrustData = timeframeThrustingStreamingUtilizationAsRuntimeSumingValues;
                this.timeline._access.process.utilizeReadData = timeframeReadingStreamingUtilizationAsRuntimeGettingValues;
                /*if (this.mode == "3d") {
                    timeframeThrustingStreamingUtilizationAsRuntimeSumingValuesForTHREE();
                    timeframeReadingStreamingUtilizationAsRuntimeGettingValuesForTHREE();
                } else {
                    timeframeThrustingStreamingUtilizationAsRuntimeSumingValues();
                    timeframeReadingStreamingUtilizationAsRuntimeGettingValues();
                }*/
                syncInTimeframe(position);
                timeframeRuntimeStreamRevertCallAndForwardingRevertPositionValue(/*_revert*/);
            }

            void syncInTimeframe(int position) {
                this.timeline._access.process.option = this.timeline._access.defaults.timeframe;
                this.timeline._access.process.method = "all";
                this.timeline._access.readCount = this.timeline._access.thrustCount = 1;
                if (position != -1) syncing(position);
            }
            public void syncing(int? position = null) {
                int modDuration = position ?? this.duration;
                this.timeline._access._syncOffsets(modDuration);
            }

            public void start(/*canvas*/) {
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

            public void _forceInit(/*canvas*/) {
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
                this._ready();
            }
            public void _ready() {
                this.ready = true;
            }
            public void run() {
                if (!this.running && this.ready) {
                    //TIME.now = TIME._then = Date.now();
                    //this.length = _runtime[_runtime.access.stream] ? _runtime[_runtime.access.stream].length : this.length;
                    this.running = true;
                    this.update();
                    this.frame();
                }
            }
            public void stop(int at = -1) {
                if (this.running) {
                    this.running = false;
                    this.update();
                    //window.cancelAnimationFrame(TIME.animationFrameLoop);
                }
                if (at != -1) {
                    this.duration = at;
                    this.syncing();
                }
            }
            public void tick(bool exact, int number = 1) {
                TIME.byFrame.exact = exact || TIME.byFrame.exact;
                TIME.byFrame.number = number;
                this.running = false;
                this.run();
            }
            /*public void keyPauseToggle(e) {
                if (e) {
                    if (e.keyCode != 32) {
                        return
                    }
                    if (this.running) {
                        this.stop()
                    } else {
                        this.run()
                    }
                }
            }*/
            public void frame() {
                if (!this.running) {
                    return;
                }
                //window.stats.begin();
                //TIME.now = Date.now();
                //TIME._delta = TIME.now - TIME._then + TIME._remain;
                //this[TIME.access] = TIME._timeFrame = TIME._delta / 10 << 0;
                //this._duration = that.duration + TIME._timeFrame;
                this.process();
                //TIME._remain = TIME._delta - (TIME.timeFrame / 100);// get remainder for precision
                //TIME._then = TIME.now;
                if (TIME.timeFrame > 0 && TIME.timeFrame < TIME.lapse) {
                    this.runtime();
                    if (!TIME.byFrame.exact) {
                        // values summed up overtime and passed to node properties
                        this.timeline._access.process.invokeCall(TIME.timeFrame);
                        if (TIME.byFrame.number > 0) {
                            if (TIME.byFrame.number == 1) {
                                this.stop();
                            }
                            TIME.byFrame.number--;
                        }
                        // duration per successful frame
                        this.duration += TIME.timeFrame;
                    } else if (TIME.byFrame.exact) {
                        this.timeline._access.process.invokeCall(1);
                        TIME.byFrame.exact = false;
                        TIME.byFrame.number = 0;
                        this.stop();
                    }
                    this.invoke();
                } else if (TIME.timeFrame > TIME.lapse) {
                    // TIME.lapse; if CPU halts the streams for too long then stop process
                }
                //TIME.animationFrameLoop = window.requestAnimationFrame(frame);
                //window.stats.end();
            }
            public void switchToTimeFrameThrusting (int position) {
                //TIME.access = "thrust";
                this.timeline._access.defaults.timeframe = "thrust";
                this.timeline._access.block = true;
                this.update(position);
            }
            public void switchToTimeFrameReading(int position) {
                //TIME.access = "read";
                this.timeline._access.defaults.timeframe = "read";
                this.timeline._access.block = true;
                this.update(position);
            }
            void timeframeThrustingStreamingUtilizationAsRuntimeSumingValues(float value, int node, int property) {
                    if (value == 0) return;
                    IDictionary<int, object> setBind = this.timeline.binding.ids[node];
                    TLType setNode = (TLType)setBind[0];
                    BIND setBindProperty = (BIND)setBind[property];
                    setBindProperty.value += value;
                    // TO-DO redo all demos to utilize property binding value and remove if else statement, uniform scheme
                    // Generate HashTable for optimization ex. idHash(node + property)
                    //if (setBindProperty.property != null) setBind.node[setBindProperty.property][setBindProperty.binding] = setBindProperty.value; else setBind.node[setBindProperty.binding] = setBindProperty.value;
            }
            void timeframeReadingStreamingUtilizationAsRuntimeGettingValues(float value, int node, int property) {
                IDictionary<int, object> setBind = this.timeline.binding.ids[node];
                BIND setBindProperty = (BIND)setBind[property];
                setBindProperty.value = value;
                // TO-DO redo all demos to utilize property binding value and remove if else statement, uniform scheme
                // Generate HashTable for optimization ex. idHash(node + property)
                //if (setBindProperty.property != null) setBind.node[setBindProperty.property][setBindProperty.binding] = setBindProperty.value; else setBind.node[setBindProperty.binding] = setBindProperty.value;
            }
            void timeframeRuntimeStreamRevertCallAndForwardingRevertPositionValue() {
                this.timeline._access.process.outputRevertCall = _revert;
            }
            public void resetStreamProperties() {
                this.timeline._access.revertFromTo(this.timeline.length, 0);
            }
        }
    }

    // Shared Time for Sync Timelines
    public static class TIME {
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
                TIMELINE.step();
            }
        }
        public static class byFrame {
            public static int number = 0; 
            public static bool exact = false;
        }; 
    }

    static void step()
    {
        for (int t = 0; t < TIMELINE.timelines.Length; t++) {
            TIMELINE.timelines[t].timeframe.frame();
        }
    }
}

