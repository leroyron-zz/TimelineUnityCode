using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public partial class ImpulseEngine : MonoBehaviour
{
    void Start()
    {
        impulse = new ImpulseScene(ImpulseMath.DT, 10);

        Body b = null;

        b = impulse.add(new Circle(30.0f), 0, 100);
        b.setStatic();

        b = impulse.add(new Polygon(200.0f, 10.0f), 0, 0);
        b.setStatic();
        b.setOrient(0);

        accumulator = 0f;
        playing = true;

        /*
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
		*/
        // Invisible Plane Click Refer
        m_DistanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
        m_Plane = new Plane(Vector3.forward, m_DistanceFromCamera);
    }
    /*
    public void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log("OnPointerDownDelegate called.");
    }
    */

    public Transform PointerTransform;
    float gridPlaneCenterZ = 0f;
    Vector3 m_DistanceFromCamera;
    Plane m_Plane;
    Ray ray;
    void rayInput(int? pointState = null, int? keyState = null)
    {
        m_DistanceFromCamera.z = gridPlaneCenterZ;
        gameInput.mouseUp = pointState ?? -1;
        gameInput.keyDown = keyState ?? gameInput.keyDown;
        ray = Camera.main.ScreenPointToRay(gameInput.position);
        //Initialize the enter variable
        float enter = 0.0f;
        if (m_Plane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);
            //Move your cube GameObject to the point where you clicked
            if (PointerTransform) PointerTransform.position = hitPoint;
            gameInput.position = hitPoint;

            input(gameInput);
        }
    }


    public void update()
    {
        accumulator += Time.deltaTime; //Time.time;
                                       //accumulator += Time.time;

        if (accumulator >= impulse.dt)
        {
            impulse.step();

            accumulator -= impulse.dt;
        }
    }

    public Material material;
    public ImpulseScene impulse;
    public bool playing;
    private float accumulator;
    Vector3 xyz = new Vector3();
    Vector3 rxyz = new Vector3();
    Vec2 vxy = new Vec2();
    Vec2 nxy = new Vec2();
    void OnPostRender() { RenderLines(); }
    void OnDrawGizmos() { RenderLines(); }
    void RenderLines()
    {
		if (impulse == null) return;
        if (impulse.bodies.Count > 0)
            foreach (Body b in impulse.bodies)
            {
                if (b.shape is Circle)
                {
                    Circle c = (Circle)b.shape;

                    // Circle
                    GL.Begin(GL.LINE_STRIP);
                    material.SetPass(0);
                    GL.Color(Color.red);
                    for (int i = 0; i < c.vertices.Length; i++)
                    {
                        //xyz = p.vertices[i];// Vec2 don't reference when modifying take x, y values only
                        xyz.x = (float)Mathf.Cos((i / (float)c.vertices.Length - 1) * 2 * Mathf.PI);
                        xyz.y = (float)Mathf.Sin((i / (float)c.vertices.Length - 1) * 2 * Mathf.PI);
                        xyz.x *= c.radius;
                        xyz.y *= c.radius;
                        xyz.x += b.position.x;
                        xyz.y += b.position.y;
                        GL.Vertex(xyz);
                    }
                    GL.End();

                    rxyz.x = (float)Mathf.Cos(b.orient) * c.radius;
                    rxyz.y = (float)Mathf.Sin(b.orient) * c.radius;

                    // Line
                    GL.Begin(GL.LINES);
                    material.SetPass(0);
                    GL.Color(Color.red);
                    xyz.x = b.position.x;
                    xyz.y = b.position.y;
                    GL.Vertex(xyz);
                    rxyz.x = b.position.x + rxyz.x;
                    rxyz.y = b.position.y + rxyz.y;
                    GL.Vertex(rxyz);
                    GL.End();
                }
                else if (b.shape is Polygon)
                {
                    Polygon p = (Polygon)b.shape;

                    // Polygon
                    GL.Begin(GL.LINE_STRIP);
                    material.SetPass(0);
                    GL.Color(Color.blue);
                    for (int i = 0; i < p.vertexCount; i++)
                    {
                        //vxy = p.vertices[i];// Vec2 don't reference when modifying take x, y values only
                        vxy.x = p.vertices[i].x;
                        vxy.y = p.vertices[i].y;
                        b.shape.u.muli(vxy);
                        vxy.addi(b.position);
                        xyz.x = vxy.x;
                        xyz.y = vxy.y;
                        GL.Vertex(xyz);
                    }
                    //vxy = p.vertices[i];// don't reference when modifying take x, y values only
                    vxy.x = p.vertices[0].x;
                    vxy.y = p.vertices[0].y;
                    b.shape.u.muli(vxy);
                    vxy.addi(b.position);
                    xyz.x = vxy.x;
                    xyz.y = vxy.y;
                    GL.Vertex(xyz);
                    GL.End();
                }
            }

        GL.Color(Color.black);
        if (impulse.contacts.Count > 0)
            foreach (Manifold m in impulse.contacts)
            {
                for (int i = 0; i < m.contactCount; i++)
                {
                    vxy.x = m.contacts[i].x;
                    vxy.y = m.contacts[i].y;
                    nxy.x = m.normal.x;
                    nxy.y = m.normal.y;

                    // Line
                    GL.Begin(GL.LINES);
                    material.SetPass(0);
                    GL.Color(Color.red);
                    xyz.x = vxy.x;
                    xyz.y = vxy.y;
                    GL.Vertex(xyz);
                    xyz.x = vxy.x + nxy.x * 4.0f;
                    xyz.y = vxy.y + nxy.y * 4.0f;
                    GL.Vertex(xyz);
                    GL.End();
                }
            }
    }
    public void destroy()
    {
        impulse.clear();
    }
    public bool isPlaying()
    {
        return playing;
    }
}
