partial class TIMELINE
{
    public partial class GUI
    {
        public DIALOG dialog = new DIALOG();

        public class DIALOG
        {
            private TIMELINE timeline;
            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                timeline.Log("Init Dialog");
            }
        }
    }
}