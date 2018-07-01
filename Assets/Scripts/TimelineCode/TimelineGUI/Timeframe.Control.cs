using TLExtensions;

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
                Scenes _scenes;
                Core _code;
                public GUI _gui;
                Core.Timeframe _timeframe;
                public void Init(Timeline timeline)
                {
                    this._timeline = timeline;
                    this._scenes = timeline.scenes;
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
                public void Refresh() {

                }
                public void RemoveInsertsNear(string insert, int at, int near) {
                    int from = at - near;
                    int to = at + near;
                    for (int ni = from; ni < to; ni++) {
                        this.RemoveInsertAt(insert, ni);
                    }
                }

                public void RemoveInsertAt(string insert, int at) {
                    //TLUIElement[] elements = ((Seek.Elements)this.seek.insert.GetMember(insert)).elements;
                    //elements[at] = null;
                }
                public void Destroy(string[] inserts = null, bool refresh = true) {
                    inserts = inserts ?? new string[]{"action", "sound", "comment", "segment"};
                    Dest(inserts, refresh);
                }
                public void Destroy(string inserts, bool refresh = true) {
                    Dest(new string[]{inserts}, refresh);
                }

                void Dest(string[] inserts, bool refresh = true) {
                    for (int ii = 0; ii < inserts.Length; ii++) {
                        this._scenes.ClearRuntimeAuthority(inserts[ii]);
                        //RemoveInsertsNear(inserts[ii], 0, _timeline.length);
                    }
                    if (refresh) this.Refresh();
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