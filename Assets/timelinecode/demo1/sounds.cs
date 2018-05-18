partial class TIMELINE
{
    public partial class SCENES
    {
        public partial class DEMO1 : SCENE
        {
            public new int Sounds()
            {
                sound[200] = () =>
                {
                    timeline.Log("This is frame 200");
                    return 0;
                };

                sound[500] = () =>
                {
                    timeline.Log("This is frame 500");
                    return 0;
                };

                return 0;
            }
        }
    }
}