public partial class Timeline
{
    public partial class GUI
    {
        public partial class Timeframe
        {
            public partial class Control
            {
                public partial class Seek
                {
                    public Insert insert = new Insert();
                    public partial class Insert 
                    {
                        Timeline _timeline;
                        GUI _gui;
                        public void Init(Timeline timeline)
                        {
                            this._timeline = timeline;
                            this._gui = timeline.gui;

                            TimelineCode.Log("(" + this._timeline.name + ") : Init Timeframe Control Seek Insert");
                        }
                    }

                    public class Elements {
                        public TLUIElement[] elements;
                    }
                }
            }
        }
    }
}