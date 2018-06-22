using UnityEngine;

// Recommended to attach this script to the Main Camera instance.
// Will not render in play mode if not attached to Camera.
[ExecuteInEditMode]
public class DrawLines : MonoBehaviour {

	public Color baseColor;
    public Material material;

    public Transform origin;
    public Vector3[] points;
    public Color[] colors;

    void OnPostRender()
    {
        RenderLines(points, colors);
    }

    void OnDrawGizmos()
    {
        RenderLines(points, colors);
    }

    void RenderLines(Vector3[] points, Color[] colors)
    {
        if (!ValidateInput(points, colors))
        {
            return;
        }

        GL.Begin(GL.LINES);
        material.SetPass(0);
        for (int i = 0; i < points.Length; i++)
        {
            GL.Color(Color.blue);
            GL.Vertex(origin.position);
            GL.Color(Color.red);
            GL.Vertex(points[i]);
        }
        GL.End();
    }

    private bool ValidateInput(Vector3[] points, Color[] colors)
    {
        return points != null && colors != null && points.Length == colors.Length;
    }
}
