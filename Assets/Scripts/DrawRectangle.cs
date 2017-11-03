using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawRectangle : MonoBehaviour
{
    public Color FillColor = Color.white;
	
    private MeshFilter _meshFilter;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;

    // Start and end vertices (in absolute coordinates)
    private Vector2 _v0, _v1;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void StartVertex(Vector2 vertex)
    {
        transform.position = vertex;
        _v0 = _v1 = vertex;
    }

    public void UpdateShape(Vector2 newVertex)
    {
        // Set the gameobject's position to be the center of mass
        _v1 = newVertex;
        var center = (_v0 + _v1) / 2f;
        transform.position = center;

        // Update the mesh relative to the transform
        var v0Relative = _v0 - center;
        var v1Relative = _v1 - center;
        _meshFilter.mesh = RectMesh(v0Relative, v1Relative, FillColor);
		
        // Update the collider
        var width  = Mathf.Abs(_v1.x - _v0.x);
        var height = Mathf.Abs(_v1.y - _v0.y);
        _boxCollider2D.size = new Vector2(width, height);
    }

    public void Simulate(bool active)
    {
        _rigidbody2D.bodyType = active ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
    }

    private static Mesh RectMesh(Vector2 v0, Vector2 v1, Color fillColor)
    {
        var rectVertices = new[] {
            v0,
            new Vector2(v0.x, v1.y),
            v1,
            new Vector2(v1.x, v0.y)
        };

        // Find all the triangles in the shape
        var triangles = new Triangulator(rectVertices).Triangulate();
		
        // Assign each vertex the fill color
        var colors = Enumerable.Repeat(fillColor, rectVertices.Length).ToArray();

        var mesh = new Mesh {
            name = "Rect",
            vertices = rectVertices.ToVector3(),
            triangles = triangles,
            colors = colors
        };
		
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
