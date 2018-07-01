using UnityEngine;

public partial class Timeline
{
    public partial class GUI
    {
        public partial class Timeframe
        {
            public partial class Control
            {
                public Seek seek = new Seek();
                
                public partial class Seek
                {
                    Timeline _timeline;
                    GUI _gui;
                    public TLUIElement _element;
                    public TLUIElement _slider;
                    public TLUIElement _span;
                    public TLUIElement _scrubber;
                    public TLUIElement _info;
                    public float position = 0;
                    public float width = 0;
                    public void Init(Timeline timeline)
                    {
                        this._timeline = timeline;
                        this._gui = timeline.gui;
                        this._element = timeline.gui.element;
                        this._span = timeline.gui.span;
                        this._scrubber = timeline.gui.scrubber;
                        this._info = timeline.gui.info;
                        this._slider = timeline.gui.slider;   

                        TimelineCode.Log("(" + this._timeline.name + ") : Init Timeframe Control Seek");
                    }
                }
            }
        }
    }
}