partial class TIMELINE
{
    public partial class GUI
    {
        public INSERT insert = new INSERT();

        public class INSERT
        {
            private TIMELINE timeline;
            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                timeline.Log("Init Insert");
            }
        }
    }
}