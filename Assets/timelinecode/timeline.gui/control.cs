partial class TIMELINE
{
    public partial class GUI
    {
        public CONTROL control = new CONTROL();

        public class CONTROL
        {
            private TIMELINE timeline;
            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                timeline.Log("Init Control");
            }
        }
    }
}