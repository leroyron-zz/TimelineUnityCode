public partial class Timeline
{
    public partial class GUI
    {
        public partial class Timeframe
        {
            public Control control = new Control();
            public partial class Control
            {
                Timeline _timeline;
                Core _code;
                public GUI _gui;
                Core.Timeframe _timeframe;
                public void Init(Timeline timeline)
                {
                    this._timeline = timeline;
                    this._code = timeline.code;
                    this._gui = timeline.gui;
                    this._timeframe = timeline.timeframe;
                    //timeline.timeframe.AddRevertCallback(timeline.slider_box, testRevert);
                    TimelineCode.Log("(" + this._timeline.name + ") : Init Timeframe Control");
                }
                int OnRuntime(TLUIElement controller) {
                    if (this._timeframe.duration > this._timeline.length) {
                        this._timeframe.duration += this._timeline.length - this._timeframe.duration;
                    }

                    //_time.setValue(_timeframe.duration + "sec/ms")

                    controller.proxy[0] = this.seek.position = (float)((float)this._timeframe.duration / (float)this._timeline.length)/* * 100*/;
                    //controller.proxy[0] = -20 + (this._timeline.gui.span.area.width * this.seek.position) /*+ "%"*/;
                    // assignment moved to timeline.gui//this.gui.timeline_box.width

                    // TO-DO
                    /*
                    if ((this.seek.position - that.prevSegmentPos) > (that.segmentPos - that.prevSegmentPos)) {
                        that.checkPassSegment()
                    }
                    */

                    //that.Update();

                    return 0;
                }

                int OnRevert(TLUIElement controller, int revertPos) {
                    _timeframe.duration = revertPos;
                    OnRuntime(controller);
                    //that.CheckPassSegment();
                    return 0;
                }

                public void Start(TimelineCode global) {
                    if (!this._gui.enabled) return;
                    this._gui.Start(global, this._gui);
                    this.seek.Init(_timeline);
                    this.seek.insert.Init(_timeline);
                    this.seek.insert.action.Init(_timeline);
                    this.seek.insert.comment.Init(_timeline);
                    this.seek.insert.segment.Init(_timeline);
                    this.seek.insert.sound.Init(_timeline);
                    this.seek.insert.dialog.Init(_timeline);
                    this.seek._slider.proxy = new float[1];
                    Bind.OnRevert(this.seek._slider/* this.gui.slider_box*/, this._code.timeframe, OnRevert);
                    Bind.OnRuntime(this.seek._slider/* this.gui.slider_box*/, this._code.timeframe, OnRuntime);
                }
            }
        }
    }
}