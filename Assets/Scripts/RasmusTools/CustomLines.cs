using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLines {

	public static void DrawLine(Vector2 from, Vector2 to, float width)
    {
        Vector2 direction = to - from;
        direction.Normalize();
        Vector2 cross = Vector3.Cross(direction, Vector3.forward);
        GL.Begin(GL.QUADS);
        Vector2 halfWidth = cross * width / 2;
        GL.Vertex(from + halfWidth);
        GL.Vertex(to + halfWidth);
        GL.Vertex(to - halfWidth);
        GL.Vertex(from - halfWidth);
        GL.End();
    }
}
