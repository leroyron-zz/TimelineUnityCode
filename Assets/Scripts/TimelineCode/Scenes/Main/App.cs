public partial class Timeline
{
    public partial class Scenes
    {
        public MAIN Main = new MAIN(2200);

        public partial class MAIN : Scene
        {
            public MAIN(int length) : base(length)
            {

            }
            private int variable;
            public override void Start(Timeline[] timelines) {
                Init(timelines);
                var obj = new { position = new { type = "position" }, rotation = new { type = "rotation" } };
                var element = new
                {
                    position = new { x = 10, y = 0.1 },
                    variable = variable,
                    nodes = timeline.code.binding.Add(
                    new object[]{
                            TimelineCode.timeline1,
                            obj.position, 800,
                            obj.rotation, 801,
                            "x", 100f,
                            "y", 50f, 100f,
                            801, 802,
                            false
                    }
                )
                };

                // TODO Make Queue
                timeline.code.binding.Queue(
                    new object[]{
                        TimelineCode.timeline1,
                        element.position, 800,
                        'x', 100f,
                        'y', 50f,
                        801, 802,
                        false
                    }
                );

                TimelineCode.timeline1.timeframe.Process = () => {
                    //Log("TL1");
                };
                TimelineCode.timeline2.timeframe.Process = () => {
                    //Log("TL2");
                };
                TimelineCode.timeline1.timeframe.Invoke = () => {
                    //Log("TL1");
                };
                TimelineCode.timeline2.timeframe.Invoke = () => {
                    //Log("TL2");
                };
            }
        }
    }
}