partial class TIMELINE
{
    public partial class CODE
    {
        public BUFFER buffer = new BUFFER();

        public partial class BUFFER
        {
            private TIMELINE timeline;
            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                TIMELINE.Log("Init Buffer");
            }
        }
    }
}
