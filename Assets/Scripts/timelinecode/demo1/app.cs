public partial class TIMELINE
{
    public partial class SCENES
    {
        public DEMO1 demo1 = new DEMO1(2200);

        public partial class DEMO1 : SCENE
        {
            public DEMO1(int length) : base(length)
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
                            TIMELINE.timeline1,
                            obj.position, 800,
                            obj.rotation, 801,
                            "x", 100F,
                            "y", 50F, 100F,
                            801, 802,
                            false
                    }
                )
                };

                // TODO Make Queue
                timeline.code.binding.queue(
                    new object[]{
                        TIMELINE.timeline1,
                        element.position, 800,
                        'x', 100F,
                        'y', 50F,
                        801, 802,
                        false
                    }
                );
            }
        }
    }
}