partial class TIMELINE
{
    public partial class SCENES
    {
        public partial class DEMO2 : SCENE
        {
            public new int Comments()
            {
                comment[200] = () =>
                {
                    TIMELINE.Log("This is frame 200");
                    return 0;
                };

                comment[500] = () =>
                {
                    TIMELINE.Log("This is frame 500");
                    return 0;
                };

                return 0;
            }
        }
    }
}