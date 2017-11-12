using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// A 2D physics rectangle that is drawn by specifying the positions of 
/// its two opposite corners.
/// </summary>
public class DrawRectangle : MonoBehaviour 
{
    public Color FillColor = Color.white;
	
    private MeshFilter _meshFilter;
    private LineRenderer _lineRenderer;

    // Start and end vertices (in absolute coordinates)
    private readonly List<Vector2> _vertices = new List<Vector2>(2);
    
    public bool ShapeFinished { get { return _vertices.Count >= 2; } }
    
    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void AddVertex(Vector2 vertex)
    {
        if (ShapeFinished) {
            return;
        }
        
        _vertices.Add(vertex);
        UpdateShape(vertex);
    }

    public void UpdateShape(Vector2 newVertex)
    {
        if (_vertices.Count < 2) {
            return;
        }
        
        _vertices[_vertices.Count - 1] = newVertex;
        
        // Set the gameobject's position to be the center of mass
        var center = _vertices.Centroid();
        transform.position = center;

        // Update the mesh relative to the transform
        var relativeVertices = _vertices.Select(v => v - center).ToArray();
        _meshFilter.mesh = RectangleMesh(relativeVertices[0], relativeVertices[1], FillColor);
		
        // Update the shape's outline
        _lineRenderer.positionCount = _meshFilter.mesh.vertices.Length;
        _lineRenderer.SetPositions(_meshFilter.mesh.vertices);
    }

    /// <summary>
    /// Creates and returns a rectangle mesh given two vertices on its 
    /// opposite corners and fills it with the given color. 
    /// </summary>
    private static Mesh RectangleMesh(Vector2 v0, Vector2 v1, Color fillColor)
    {
        // Calculate implied verticies from corner vertices
        // Note: vertices must be adjacent to each other for Triangulator to work properly
        var v2 = new Vector2(v0.x, v1.y);
        var v3 = new Vector2(v1.x, v0.y);
        var rectangleVertices = new[] { v0, v2, v1, v3 };

        // Find all the triangles in the shape
        var triangles = new Triangulator(rectangleVertices).Triangulate();
		
        // Assign each vertex the fill color
        var colors = Enumerable.Repeat(fillColor, rectangleVertices.Length).ToArray();

        var mesh = new Mesh {
            name = "Rectangle",
            vertices = rectangleVertices.ToVector3(),
            triangles = triangles,
            colors = colors
        };
		
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        return mesh;
    }
}
