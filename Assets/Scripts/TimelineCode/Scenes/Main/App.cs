using UnityEngine;

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

                string tlname = timeline.name;
                timeline.access.defaults.timeframe = "read";
                timeline.timeframe.Update();

                Core.Binding bind = timeline.code.binding;
                Core.Buffer buffer = timeline.code.buffer;

                Core.TLGameObject earth = new Core.TLGameObject("Earth");
                Core.TLTransform earthTransform = earth.transform;
                bind.Add(
                    new object[]{
                        TimelineCode.timeline1,
                        earthTransform.rotation, 804,
                        earthTransform.position, 805,// give binding it's current position
                        'x', 0f, // if no value then give current
                        801,
                        false
                    }
                );

                Core.TLGameObject moon = new Core.TLGameObject("Moon");
                Core.TLTransform moonTransform = moon.transform;
                moon.nodes = timeline.code.binding.Add(
                    new object[]{
                        TimelineCode.timeline1,
                        moonTransform.rotation, 806,
                        'x', 0f,
                        //'y', 0f,
                        //'z', 0f,
                        801,
                        false
                    }
                );

                //timeline.code.binding.Test();

                buffer.Eval(
                    TimelineCode.timeline1,
                    new object[]{
                        earthTransform.rotation,
                        //earthTransform.position,
                        'x', 360f,
                        "linear", 2200,
                        earthTransform.position,
                        'x', 360f,
                        "inSine", 2200
                    }
                );
                
                buffer.Eval(
                    TimelineCode.timeline1,
                    new object[]{
                        moonTransform.rotation,
                        'x', 1080f + 360f,
                        "linear", 2198
                    }, false, false, (node) => {
                        TimelineCode.Log("This is the end.----------------------------------------------#######");
                        buffer.ZeroOut(TimelineCode.timeline1, 0, timeline.length, new Core.TLType[]{earthTransform.position}, new string[]{"x"});
                        return 0;
                    }
                );

                //buffer.ZeroOut(TimelineCode.timeline1, 0, timeline.length, new Core.TLType[]{earthTransform.position}, new string[]{"x"});

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