public partial class Timeline
{
    public partial class Scenes
    {
        public partial class MAIN : Scene
        {
            public override int Comments()
            {
                comments[200] = () =>
                {
                    TimelineCode.Log("This is frame 200");
                    return 0;
                };

                comments[500] = () =>
                {
                    TimelineCode.Log("This is frame 500");
                    return 0;
                };
                return 0;
            }
        }
    }
}