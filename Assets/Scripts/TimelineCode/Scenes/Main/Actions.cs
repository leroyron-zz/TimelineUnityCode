public partial class Timeline
{
    public partial class Scenes
    {
        public partial class MAIN : Scene
        {
            public override int Actions()
            {
                actions[200] = () =>
                {
                    TimelineCode.Log("This is frame 200");
                    return 0;
                };

                actions[500] = () =>
                {
                    TimelineCode.Log("This is frame 500");
                    return 0;
                };
                return 0;
            }
        }
    }
}