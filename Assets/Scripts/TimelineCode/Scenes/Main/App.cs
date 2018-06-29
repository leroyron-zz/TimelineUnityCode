public partial class TIMELINE
{
    public partial class SCENES
    {
        public MAIN Main = new MAIN(2200);

        public partial class MAIN : SCENE
        {
            public MAIN(int length) : base(length)
            {

            }
            private int variable;
            public override void start () {
                var obj = new { position = new { type = "position" }, rotation = new { type = "rotation" } };
                var element = new
                {
                    position = new { x = 10, y = 0.1 },
                    variable = variable,
                    nodes = timeline.code.binding.add(
                    new object[]{
                            timeline1,
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
                timeline.code.binding.queue(
                    new object[]{
                        timeline1,
                        element.position, 800,
                        'x', 100f,
                        'y', 50f,
                        801, 802,
                        false
                    }
                );

                timeline1.timeframe.process = () => {
                    //Log("TL1");
                };
                timeline2.timeframe.process = () => {
                    //Log("TL2");
                };
                timeline1.timeframe.invoke = () => {
                    //Log("TL1");
                };
                timeline2.timeframe.invoke = () => {
                    //Log("TL2");
                };
            }
        }
    }
}