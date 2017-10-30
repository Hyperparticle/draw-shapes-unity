using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawRectangle : MonoBehaviour
{
	public Color FillColor = Color.gray;
	
	private MeshFilter _meshFilter;
	
	private readonly List<Vector2> _vertices = new List<Vector2>();

	private void Awake()
	{
		_meshFilter = GetComponent<MeshFilter>();
	}

	public void StartVertex(Vector2 vertex)
	{
		transform.position = vertex;
		
		_vertices.Add(Vector2.zero);
		_vertices.Add(Vector2.zero);
	}

	public void UpdateShape(Vector2 newVertex)
	{
		var pos = (Vector2) transform.position;
		
		_vertices[_vertices.Count - 1] = newVertex - pos;

		_meshFilter.mesh = RectMesh(_vertices[0], _vertices[1], FillColor);
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
		var triangulator = new Triangulator(rectVertices);
		var triangles = triangulator.Triangulate();
		
		// Assign each vertex the fill color
		var colors = Enumerable.Repeat(fillColor, rectVertices.Length).ToArray();

		var mesh = new Mesh {
			name = "Rect",
			vertices = System.Array.ConvertAll<Vector2, Vector3>(rectVertices, v => v),
			triangles = triangles,
			colors = colors
		};
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		return mesh;
	}
}
