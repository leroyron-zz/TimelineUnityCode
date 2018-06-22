using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ImpulseEngine
{

    struct GameInput
    {
        public static Vector3 _position = new Vector3();
        public Vector3 position { get { return _position; } set { _position = value; } }
        public int? mouseUp;
        public int? keyDown;
        public Input input;
    }
    GameInput gameInput = new GameInput();
    void Update()
    {
        if (!playing) return;
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Pressed primary button.");
            gameInput.position = Input.mousePosition;
            rayInput(0, null);
        }
        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("Pressed secondary button.");
            gameInput.position = Input.mousePosition;
            rayInput(1, null);
        }
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Pressed middle click.");
            gameInput.position = Input.mousePosition;
            rayInput(2, null);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esc Pressed.");
            rayInput(null, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("LShift Pressed.");
            rayInput(null, 1);
        }
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("LShift Pressed.");
            rayInput(null, -1);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            gameInput.position = touchDeltaPosition;
            // Move object across XY plane
            PointerTransform.Translate(-touchDeltaPosition.x * 0.1F, -touchDeltaPosition.y * 0.1F, 0);

            Debug.Log("Touch called.");

            rayInput(0);
        }

        update();
    }

    void input(GameInput input)
    {
        if (input.keyDown == 0)
        {
            playing = false;
        }

        if (input.keyDown == 1)
        {
            if (input.mouseUp == 0)
            {
                float hw = ImpulseMath.random(10.0f, 30.0f);
                float hh = ImpulseMath.random(10.0f, 30.0f);

                Body b = impulse.add(new Polygon(hw, hh), input.position.x, input.position.y);
                b.setOrient(0.0f);
            }
            if (input.mouseUp == 1)
            {
                float r = ImpulseMath.random(10.0f, 50.0f);
                int vertCount = 3;

                Vec2[] verts = Vec2.arrayOf(vertCount);
                for (int i = 0; i < vertCount; i++)
                {
                    verts[i].set(ImpulseMath.random(-r, r), ImpulseMath.random(-r, r));
                }

                Body b = impulse.add(new Polygon(verts), input.position.x, input.position.y);
                b.setOrient(ImpulseMath.random(-ImpulseMath.PI, ImpulseMath.PI));
                b.restitution = 0.2f;
                b.dynamicFriction = 0.2f;
                b.staticFriction = 0.4f;
            }
        }
        else
        {
            if (input.mouseUp == 0)
            {
                float r = ImpulseMath.random(10.0f, 50.0f);
                int vertCount = ImpulseMath.random(3, Polygon.MAX_POLY_VERTEX_COUNT);

                Vec2[] verts = Vec2.arrayOf(vertCount);
                for (int i = 0; i < vertCount; i++)
                {
                    verts[i].set(ImpulseMath.random(-r, r), ImpulseMath.random(-r, r));
                }

                Body b = impulse.add(new Polygon(verts), input.position.x, input.position.y);
                b.setOrient(ImpulseMath.random(-ImpulseMath.PI, ImpulseMath.PI));
                b.restitution = 0.2f;
                b.dynamicFriction = 0.2f;
                b.staticFriction = 0.4f;
            }
            if (input.mouseUp == 1)
            {
                float r = ImpulseMath.random(10.0f, 30.0f);

                impulse.add(new Circle(r), input.position.x, input.position.y);
            }
        }

    }
}