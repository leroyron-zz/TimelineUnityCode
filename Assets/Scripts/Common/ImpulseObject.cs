using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseObject : ImpulseEngine {
	public int index;
	public bool started;
	public Vector2[] activePoints = new Vector2[4]{
		new Vector2(1f, -2.25f),
		new Vector2(1f, 4f),
		new Vector2(-1f, 4f),
		new Vector2(-1f, -2.25f)
	};

	private Vec2[] verts;
	public Body body;
	private Polygon shape;

	public ImpulseObject () {
		CommonMonoBehaviour.AddStart(StartImpulseObject, index + 3);
		CommonMonoBehaviour.AddUpdate(UpdateImpulseObject, index + 2);
	}

	public void StartImpulseObject()
	{
		if (started) return;
		verts = Vec2.ArrayOf(activePoints.Length);
		for (int v = 0; v < activePoints.Length; v++) {
			verts[v].x = activePoints[v].x;
			verts[v].y = activePoints[v].y;
		}

		body = impulse.Add(new Polygon(verts), transform.position.x, transform.position.y);
		body.SetOrient(0);
		body.restitution = 0.2f;
		body.dynamicFriction = 0.2f;
		body.staticFriction = 0.4f;
		shape = ((Polygon)body.shape);
		started = true;
	}

	public void UpdateImpulseObject()
	{
		if (!started) return;
		// falls through platform if not perfect topology
		for (int v = 0; v < activePoints.Length; v++) {
			shape.vertices[v].x = (float)activePoints[v].x;
			shape.vertices[v].y = (float)activePoints[v].y;
		}
		transform.position = new Vector3(body.position.x, body.position.y, 0);
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, body.orient * Mathf.Rad2Deg));
	}
}