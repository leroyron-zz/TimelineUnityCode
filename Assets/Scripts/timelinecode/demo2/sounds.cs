public partial class TIMELINE
{
    public partial class SCENES
    {
        public partial class DEMO2 : SCENE
        {
            public new int Sounds()
            {
                sounds[200] = () =>
                {
                    TIMELINE.Log("This is frame 200");
                    return 0;
                };

                sounds[500] = () =>
                {
                    TIMELINE.Log("This is frame 500");
                    return 0;
                };

                return 0;
            }
        }
    }
}