using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawRectangle : MonoBehaviour
{
	public Color FillColor = Color.gray;
	
	private readonly List<Vector2> _vertices = new List<Vector2>();

//	private MeshRenderer _meshRenderer;
	private MeshFilter _meshFilter;

	private void Awake()
	{
//		_meshRenderer = GetComponent<MeshRenderer>();
		_meshFilter = GetComponent<MeshFilter>();
	}

	private void Start()
	{
		var mesh = RectMesh(Vector2.zero, Vector2.one);
		mesh.colors = Enumerable.Repeat(FillColor, mesh.vertexCount).ToArray();

		_meshFilter.mesh = mesh;
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

		var mesh = RectMesh(_vertices[0], _vertices[1]);
		mesh.colors = Enumerable.Repeat(FillColor, mesh.vertexCount).ToArray();
		
		_meshFilter.mesh = mesh;
	}

	private static Mesh RectMesh(Vector2 v0, Vector2 v1)
	{
//		var min = Vector2.Min(v0, v1);
//		var max = Vector2.Max(v0, v1);

		var rectVertices = new[] {
			v0,
			new Vector2(v0.x, v1.y),
			v1,
			new Vector2(v1.x, v0.y)
		};

		// Find all the triangles in the shape
		var triangulator = new Triangulator(rectVertices);
		var triangles = triangulator.Triangulate();

		var mesh = new Mesh {
			name = "Rect",
			vertices = System.Array.ConvertAll<Vector2, Vector3>(rectVertices, v => v),
			triangles = triangles
		};
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		return mesh;
	}
}
