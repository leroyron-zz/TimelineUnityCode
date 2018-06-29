using UnityEngine;

public partial class TIMELINE
{
    public partial class GUI
    {
        public partial class TIMEFRAME
        {
            public partial class CONTROL
            {
                public SEEK seek = new SEEK();
                
                public partial class SEEK
                {
                    public ELEMENT element;
                    public ELEMENT slider;
                    public ELEMENT span;
                    public ELEMENT scrubber;
                    public ELEMENT info;
                    private TIMELINE timeline;
                    public float position = 0;
                    public float width = 0;
                    private TIMELINE.GUI gui;
                    public void init(TIMELINE timeline)
                    {
                        this.element = timeline.gui.element;
                        this.span = timeline.gui.span;
                        this.scrubber = timeline.gui.scrubber;
                        this.info = timeline.gui.info;
                        this.slider = timeline.gui.slider;
                        
                        this.timeline = timeline;
                        this.gui = timeline.gui;

                        Log("(" + this.timeline.name + ") : Init Timeframe Control Seek");
                    }
                }
            }
        }
    }
}