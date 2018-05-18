partial class TIMELINE
{
    public partial class SCENES
    {
        public DEMO1 demo1 = new DEMO1(1100);

        public partial class DEMO1 : SCENE
        {
            public DEMO1(int length) : base(length)
            {
                this.loadActions = Actions;
                this.loadComments = Comments;
                this.loadSegments = Segments;
                this.loadSounds = Sounds;
            }
        }
    }
}