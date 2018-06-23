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
            public bool control = false;
            public int lapse { get{ return TIMELINE.time.lapse; } set { TIMELINE.time.lapse = value; }}
            public int duration, _duration = 0;
            public int read = 0;
            public int thrust = 0;
            public string mode = "3d";
            private TIMELINE timeline;
            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                TIMELINE.Log("Init Timeframe");
            }
            public void runtime() {
                // GUI update calls (SEEK)
                // stream this.runtimeCallbacks();
            }
            public void revert(int revertPos) {
                this.revertCallbacks(revertPos);
            }
            public void addRevertCallback(object objBoxed, Func<object, int, int> func)
            {
                revertCalls[revertCallCount] = (int revertPos) => { return func(objBoxed, revertPos); };
                revertCallCount++;
            }
            public Func<int, int>[] revertCalls = new Func<int, int>[10];
            public int revertCallCount = 0;
            public void revertCallbacks(int revertPos) {
                for (int r = 0; r < this.revertCallCount; r++) {
                    // GUI revert calls (SEEK)
                    revertCalls[r](revertPos);
                }
            }
            // reverting start stream from start
            public void _revert(int revertPos) {
                revert(revertPos);
            }

            public void update(int position = -1) {
                //time.process = this.process;
                //time.invoke = this.invoke;
                //time.length = this.length;
                //time.running = this.running;
                time.lapse = this.lapse;

                // GUI update calls this
                //if (this.updateCallbacks) this.updateCallbacks();

                // Always 3D mode
                timeline._access.process.utilizeThrustData = timeframeThrustingStreamingUtilizationAsRuntimeSumingValues;
                timeline._access.process.utilizeReadData = timeframeReadingStreamingUtilizationAsRuntimeGettingValues;
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
                timeline._access.process.option = timeline._access.defaults.timeframe;
                timeline._access.process.method = "all";
                timeline._access.readCount = timeline._access.thrustCount = 1;
                if (position != -1) syncing(position);
            }
            public void syncing(int? position = null) {
                int modDuration = position ?? this.duration;
                timeline._access._syncOffsets(modDuration);
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
                    //time.now = time._then = Date.now();
                    //this.length = _runtime[_runtime.access.stream] ? _runtime[_runtime.access.stream].length : this.length;
                    this.running = true;
                    this.update();
                    frame();
                }
            }
            public void stop(int at = -1) {
                if (this.running) {
                    this.running = false;
                    this.update();
                    //window.cancelAnimationFrame(time.animationFrameLoop);
                }
                if (at != -1) {
                    this.duration = at;
                    this.syncing();
                }
            }
            public void tick(bool exact, int number = 1) {
                time.byFrame.exact = exact || time.byFrame.exact;
                time.byFrame.number = number;
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
                if (!running) {
                    return;
                }
                //window.stats.begin();
                //time.now = Date.now();
                //time._delta = time.now - time._then + time._remain;
                //this[time.access] = time._timeFrame = time._delta / 10 << 0;
                //this._duration = that.duration + time._timeFrame;
                this.process();
                //time._remain = time._delta - (time._timeFrame * 10);// get remainder for precision
                //time._then = time.now;
                if (time._timeFrame > 0 && time._timeFrame < time.lapse) {
                    this.runtime();
                    if (!time.byFrame.exact) {
                        // values summed up overtime and passed to node properties
                        timeline._access.process.invokeCall(time._timeFrame);
                        if (time.byFrame.number > 0) {
                            if (time.byFrame.number == 1) {
                                this.stop();
                            }
                            time.byFrame.number--;
                        }
                        //that.duration += time._timeFrame;
                    } else if (time.byFrame.exact) {
                        timeline._access.process.invokeCall(1);
                        time.byFrame.exact = false;
                        time.byFrame.number = 0;
                        this.stop();
                    }
                    this.invoke();
                } else if (time._timeFrame > time.lapse) {
                    // time.lapse; if CPU halts the streams for too long then stop process
                }
                //time.animationFrameLoop = window.requestAnimationFrame(frame);
                //window.stats.end();
            }
            public void switchToTimeFrameThrusting (int position) {
                //time.access = "thrust";
                timeline._access.defaults.timeframe = "thrust";
                timeline._access.block = true;
                this.update(position);
            }
            public void switchToTimeFrameReading(int position) {
                //time.access = "read";
                timeline._access.defaults.timeframe = "read";
                timeline._access.block = true;
                this.update(position);
            }
            
            void timeframeThrustingStreamingUtilizationAsRuntimeSumingValues(float value, int node, int property) {
                    if (value == 0) return;
                    IDictionary<int, object> setBind = timeline.binding.ids[node];
                    TLType setNode = (TLType)timeline.binding.ids[0];
                    BINDPROPERTY setBindProperty = (BINDPROPERTY)setBind[property];
                    setBindProperty.value += value;
                    // TO-DO redo all demos to utilize property binding value and remove if else statement, uniform scheme
                    // Generate HashTable for optimization ex. idHash(node + property)
                    //if (setBindProperty.property != null) setBind.node[setBindProperty.property][setBindProperty.binding] = setBindProperty.value; else setBind.node[setBindProperty.binding] = setBindProperty.value;
            }
            void timeframeReadingStreamingUtilizationAsRuntimeGettingValues(float value, int node, int property) {
                IDictionary<int, object> setBind = timeline.binding.ids[node];
                BINDPROPERTY setBindProperty = (BINDPROPERTY)setBind[property];
                setBindProperty.value = value;
                // TO-DO redo all demos to utilize property binding value and remove if else statement, uniform scheme
                // Generate HashTable for optimization ex. idHash(node + property)
                //if (setBindProperty.property != null) setBind.node[setBindProperty.property][setBindProperty.binding] = setBindProperty.value; else setBind.node[setBindProperty.binding] = setBindProperty.value;
            }
            void timeframeRuntimeStreamRevertCallAndForwardingRevertPositionValue() {
                timeline._access.process.outputRevertCall = _revert;
            }
            public void resetStreamProperties() {
                timeline._access.revertFromTo(timeline.length, 0);
            }
        }
    }

    // Shared Time for Sync Timelines
    public static class time {
        public static int now = 0;
        public static int lapse = 0; 
        public static int _then = 0;
        public static int _remain = 0; 
        public static int _timeFrame = 0;
        public static class byFrame {
            public static int number = 0; 
            public static bool exact = false;
            
        }; 
    }
}

