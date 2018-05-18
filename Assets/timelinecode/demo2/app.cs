partial class TIMELINE
{
    public partial class SCENES
    {
        public DEMO2 demo2 = new DEMO2(1200);

        public partial class DEMO2 : SCENE // This class derives from the previous class.
        {
            public DEMO2(int length) : base(length)
            {
                this.loadActions = Actions;
                this.loadComments = Comments;
                this.loadSegments = Segments;
                this.loadSounds = Sounds;
            }
        }
    } 
}