public partial class TIMELINE
{
    public GUI gui = new GUI();

    public partial class GUI
    {
        private TIMELINE timeline;
        public void init(TIMELINE timeline)
        {
            this.timeline = timeline;
            TIMELINE.Log("Gui Started");
        }
    }
}