public partial class TIMELINE
{
    public partial class GUI
    {
        public partial class TIMEFRAME
        {
            public partial class CONTROL
            {
                public partial class SEEK
                {
                    public partial class INSERT 
                    {
                        public COMMENT comment = new COMMENT();
                        public class COMMENT
                        {
                            private TIMELINE timeline;
                            private TIMELINE.GUI gui;
                            public void init(TIMELINE timeline)
                            {
                                this.timeline = timeline;
                                this.gui = timeline.gui;

                                Log("(" + this.timeline.name + ") : Init Timeframe Control Seek Insert Comment");
                            }
                        }
                    }
                }
            }
        }
    }
}