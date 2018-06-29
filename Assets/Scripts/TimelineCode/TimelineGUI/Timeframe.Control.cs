public partial class TIMELINE
{
    public partial class GUI
    {
        public partial class TIMEFRAME
        {
            public CONTROL control = new CONTROL();
            public partial class CONTROL
            {
                private TIMELINE timeline;
                private TIMELINE.CODE code;
                private TIMELINE.GUI gui;
                private CODE.TIMEFRAME _timeframe;
                public void init(TIMELINE timeline)
                {
                    this.timeline = timeline;
                    this.code = timeline.code;
                    this.gui = timeline.gui;
                    this._timeframe = timeline.timeframe;
                    //timeline.timeframe.addRevertCallback(timeline.slider_box, testRevert);
                    Log("(" + this.timeline.name + ") : Init Timeframe Control");
                }
                int _onruntime (ELEMENT controller) {
                    if (this._timeframe.duration > this.timeline.length) {
                        this._timeframe.duration += this.timeline.length - this._timeframe.duration;
                    }

                    //_time.setValue(_timeframe.duration + "sec/ms")

                    controller.proxy[0] = this.seek.position = (float)((float)this._timeframe.duration / (float)this.timeline.length)/* * 100*/;
                    //controller.proxy[0] = -20 + (this.timeline.gui.span.area.width * this.seek.position) /*+ "%"*/;
                    // assignment moved to timeline.gui//this.gui.timeline_box.width

                    // TO-DO
                    /*
                    if ((this.seek.position - that.prevSegmentPos) > (that.segmentPos - that.prevSegmentPos)) {
                        that.checkPassSegment()
                    }
                    */

                    //that.update();

                    return 0;
                }

                int _onrevert (ELEMENT controller, int revertPos) {
                    _timeframe.duration = revertPos;
                    _onruntime(controller);
                    //that.checkPassSegment();
                    return 0;
                }

                public void start (timeline global) {
                    if (!this.gui.enabled) return;
                    this.gui.start(global, this.gui);
                    this.seek.init(timeline);
                    this.seek.insert.init(timeline);
                    this.seek.insert.action.init(timeline);
                    this.seek.insert.comment.init(timeline);
                    this.seek.insert.segment.init(timeline);
                    this.seek.insert.sound.init(timeline);
                    this.seek.insert.dialog.init(timeline);
                    this.seek.slider.proxy = new float[1];
                    BIND.onrevert(this.seek.slider/* this.gui.slider_box*/, this.code.timeframe, _onrevert);
                    BIND.onruntime(this.seek.slider/* this.gui.slider_box*/, this.code.timeframe, _onruntime);
                }
            }
        }
    }
}