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
                    public Scrubber scrubber = new Scrubber();
                    public class Scrubber 
                    {
                        Timeline _timeline;
                        GUI _gui;
                        public void Init(Timeline timeline)
                        {
                            this._timeline = timeline;
                            this._gui = timeline.gui;
                        }
                    }
                }
            }
        }
    }
}