using UnityEngine;

public partial class ImpulseEngine
{
    void InputImpulseEngine(CommonMonoBehaviour.GameInput input)
    {

        if (!playing) return;
        if (input.keyDown[0])
        {
            playing = false;
        }

        if (input.keyDown[1])
        {
            if (input.mouseUp[0] || input.mouseDown[0])
            {
                float hw = ImpulseMath.Random(1.0f, 3.0f);
                float hh = ImpulseMath.Random(1.0f, 3.0f);

                Body b = impulse.Add(new Polygon(hw, hh), input.rayPosition.x, input.rayPosition.y);
                b.SetOrient(0.0f);
            }
            if (input.mouseUp[1] || input.mouseDown[1])
            {
                float r = ImpulseMath.Random(1.0f, 5.0f);
                int vertCount = 3;

                Vec2[] verts = Vec2.ArrayOf(vertCount);
                for (int i = 0; i < vertCount; i++)
                {
                    verts[i].Set(ImpulseMath.Random(-r, r), ImpulseMath.Random(-r, r));
                }

                Body b = impulse.Add(new Polygon(verts), input.rayPosition.x, input.rayPosition.y);
                b.SetOrient(ImpulseMath.Random(-ImpulseMath.PI, ImpulseMath.PI));
                b.restitution = 0.2f;
                b.dynamicFriction = 0.2f;
                b.staticFriction = 0.4f;
            }
        }
        else
        {
            if (input.mouseUp[0] || input.mouseDown[0])
            {
                float r = ImpulseMath.Random(1.0f, 5.0f);
                int vertCount = ImpulseMath.Random(3, Polygon.MAX_POLY_VERTEX_COUNT);

                Vec2[] verts = Vec2.ArrayOf(vertCount);
                for (int i = 0; i < vertCount; i++)
                {
                    verts[i].Set(ImpulseMath.Random(-r, r), ImpulseMath.Random(-r, r));
                }

                Body b = impulse.Add(new Polygon(verts), input.rayPosition.x, input.rayPosition.y);
                b.SetOrient(ImpulseMath.Random(-ImpulseMath.PI, ImpulseMath.PI));
                b.restitution = 0.2f;
                b.dynamicFriction = 0.2f;
                b.staticFriction = 0.4f;
            }
            if (input.mouseUp[1] || input.mouseDown[1])
            {
                float r = ImpulseMath.Random(1.0f, 3.0f);

                impulse.Add(new Circle(r), input.rayPosition.x, input.rayPosition.y);
            }
            //Move your cube GameObject to the point where you clicked
            if (PointerTransform) PointerTransform.position = input.rayPosition;
        }
    }
}