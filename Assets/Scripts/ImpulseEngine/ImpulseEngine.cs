using UnityEngine;

[ExecuteInEditMode]
public partial class ImpulseEngine : CommonMonoBehaviour
{
    public ImpulseEngine () {
        // All Common
        CommonMonoBehaviour.AddStart(StartImpulseEngine, 2);
        CommonMonoBehaviour.AddUpdate(UpdateImpulseEngine, 1);
        CommonMonoBehaviour.AddInput(InputImpulseEngine, 0);
        CommonMonoBehaviour.AddOnPostRender(OnPostRenderImpulseEngine, 0);
        CommonMonoBehaviour.AddOnDrawGizmos(OnDrawGizmosImpulseEngine, 0);
    }
    public ImpulseScene impulse = new ImpulseScene(1.0f / 60.0f, 5);
    public bool playing;
    public bool showInEditor;
    private float accumulator;
    void StartImpulseEngine()
    {
        impulse = new ImpulseScene(1.0f / 60.0f, 5);

        Body b = null;

        b = impulse.Add(new Circle(3.0f), 0, 10);
        b.SetStatic();
        b.SetOrient(0);

        b = impulse.Add(new Polygon(20.0f, 1.0f), 0, 0);
        b.SetStatic();
        b.SetOrient(0);

        accumulator = 0f;
        playing = true;
    }

    public void UpdateImpulseEngine()
    {
        accumulator += Time.deltaTime; //Time.deltaTime;
                                       //accumulator += Time.deltaTime;
        if (playing && accumulator >= impulse.dt)
        {
            impulse.Step();

            accumulator -= impulse.dt;
        }
    }

    Vector3 xyz = new Vector3();
    Vector3 rxyz = new Vector3();
    Vec2 vxy = new Vec2();
    Vec2 nxy = new Vec2();

    void OnPostRenderImpulseEngine() { RenderGLImpulseEngine(); }

    void OnDrawGizmosImpulseEngine() { if (showInEditor) RenderGLImpulseEngine(); }
    
    void RenderGLImpulseEngine()
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
                    for (int i = 0; i < 45; i++)
                    {
                        //xyz = p.vertices[i];// Vec2 don't reference when modifying take x, y values only
                        xyz.x = (float)Mathf.Cos((i / 45f) * 2 * Mathf.PI);
                        xyz.y = (float)Mathf.Sin((i / 45f) * 2 * Mathf.PI);
                        xyz.x *= c.radius;
                        xyz.y *= c.radius;
                        xyz.x += b.position.x;
                        xyz.y += b.position.y;
                        GL.Vertex(xyz);
                    }
                    GL.End();

                    rxyz.x = Mathf.Cos(b.orient) * c.radius;
                    rxyz.y = Mathf.Sin(b.orient) * c.radius;

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
                        b.shape.u.Muli(vxy);
                        vxy.Addi(b.position);
                        xyz.x = vxy.x;
                        xyz.y = vxy.y;
                        GL.Vertex(xyz);
                    }
                    //vxy = p.vertices[i];// don't reference when modifying take x, y values only
                    vxy.x = p.vertices[0].x;
                    vxy.y = p.vertices[0].y;
                    b.shape.u.Muli(vxy);
                    vxy.Addi(b.position);
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
                    xyz.x = vxy.x + nxy.x * .4f;
                    xyz.y = vxy.y + nxy.y * .4f;
                    GL.Vertex(xyz);
                    GL.End();
                }
            }
    }

    public void Destroy()
    {
        impulse.Clear();
    }

    public bool IsPlaying()
    {
        return playing;
    }

    public static void Log(object msg)
    {
        Debug.Log("ImpulseEngine: " + msg);
    }
}
