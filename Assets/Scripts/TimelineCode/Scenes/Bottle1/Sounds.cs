public partial class Timeline
{
    public partial class Scenes
    {
        public partial class BOTTLE1 : Scene
        {
            public override int Sounds()
            {
                sounds[200] = () =>
                {
                    TimelineCode.Log("This is frame 200");
                    return 0;
                };

                sounds[500] = () =>
                {
                    TimelineCode.Log("This is frame 500");
                    return 0;
                };
                return 0;
            }
        }
    }
}