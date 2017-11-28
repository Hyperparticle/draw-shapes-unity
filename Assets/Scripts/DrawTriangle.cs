using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// A 2D physics triangle that is drawn by specifying the positions of 
/// its three vertices.
/// </summary>
public class DrawTriangle : DrawShape 
{
    public Color FillColor = Color.white;
	
    private MeshFilter _meshFilter;
    private Rigidbody2D _rigidbody2D;
    private PolygonCollider2D _polygonCollider2D;
    private LineRenderer _lineRenderer;

    // Triangle vertices (in absolute coordinates)
    private readonly List<Vector2> _vertices = new List<Vector2>(3);
    
    public override bool ShapeFinished { get { return _vertices.Count >= 3; } }

    private bool _simulating;
    public override bool SimulatingPhysics
    {
        get { return _simulating; }
        set {
            _simulating = value;
            _rigidbody2D.bodyType = value ? 
                RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        }
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        
        _rigidbody2D.useAutoMass = true;
    }

    public override void AddVertex(Vector2 vertex)
    {
        if (ShapeFinished) {
            return;
        }
        
        _vertices.Add(vertex);
        UpdateShape(vertex);
    }

    public override void UpdateShape(Vector2 newVertex)
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
        _meshFilter.mesh = PolygonMesh(relativeVertices, FillColor);
		
        // Update the collider
        _polygonCollider2D.points = relativeVertices;

        // Update the shape's outline
        _lineRenderer.positionCount = _meshFilter.mesh.vertices.Length;
        _lineRenderer.SetPositions(_meshFilter.mesh.vertices);
    }

    
    /// <summary>
    /// Creates and returns a polygon mesh given a list of its vertices.
    /// </summary>
    private static Mesh PolygonMesh(Vector2[] vertices, Color fillColor)
    {
        // Find all the triangles in the shape
        var triangles = new Triangulator(vertices).Triangulate();
		
        // Assign each vertex the fill color
        var colors = Enumerable.Repeat(fillColor, vertices.Length).ToArray();

        var mesh = new Mesh {
            name = "Triangle",
            vertices = vertices.ToVector3(),
            triangles = triangles,
            colors = colors
        };
		
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        return mesh;
    }
}