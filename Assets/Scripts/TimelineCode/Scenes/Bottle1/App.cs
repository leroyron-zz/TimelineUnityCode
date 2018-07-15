using UnityEngine;

public partial class Timeline
{
    public partial class Scenes
    {
        public BOTTLE1 Bottle1 = new BOTTLE1(2200);

        public partial class BOTTLE1 : Scene
        {
            public BOTTLE1(int length) : base(length)
            {

            }
            class BottleObject {
                public Core.TLImpulseObject parent = new Core.TLImpulseObject("bottle");
                public Core.TLGameObject pop = new Core.TLGameObject("bottle/bottlePop");
                public Core.TLGameObject spin = new Core.TLGameObject("bottle/bottleSpin");
            }

            public override void Start(Timeline[] timelines) {
                Init(timelines);
                Core.Binding bind = timeline.code.binding;
                Core.Buffer buffer = timeline.code.buffer;

                BottleObject bottle = new BottleObject();

                Core.TLImpulseTransform bottleTransform = bottle.parent.transform;
                bind.Add(
                    new object[]{
                        TimelineCode.timeline1,
                        bottleTransform.rotation, 806,
                        bottleTransform.position, 805,// give binding it's current position
                        'x', // if no value then give current
                        'y', // if no value then give current
                        'z', // if no value then give current
                        801, 802, 803,
                        false
                    }
                );

                buffer.Eval(
                    TimelineCode.timeline1,
                    new object[]{
                        bottleTransform.rotation,
                        'z', 90f,
                        "inSine", 500,
                    }, false, false, (node) => {
                        node.parameter.mute = true;// Let Impulse Take Control
                        return 0;
                    }
                );

                buffer.Eval(
                    TimelineCode.timeline1,
                    new object[]{
                        bottleTransform.position,
                        'y', 15f,
                        "inSine", 500,
                        'x', 15f,
                        "outSine", 500,
                    }, false, false, (node) => {
                        node.parameter.mute = true;// Let Impulse Take Control
                        bottle.parent.impulseObject.body.velocity.Set(0, 0);
                        bottle.parent.impulseObject.body.force.Set(0, 0);
                        bottle.parent.impulseObject.body.shape.Initialize();
                        return 0;
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