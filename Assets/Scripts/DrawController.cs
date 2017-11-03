using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
	public DrawMode DrawMode = DrawMode.Rectangle;
	
	public DrawShape RectanglePrefab;
	public DrawShape CirclePrefab;
	public DrawShape TrianglePrefab;

	private Dictionary<DrawMode, DrawShape> _drawModeToPrefab;

	private void Awake()
	{
		_drawModeToPrefab = new Dictionary<DrawMode, DrawShape> {
			{DrawMode.Rectangle, RectanglePrefab},
			{DrawMode.Circle, CirclePrefab},
			{DrawMode.Triangle, TrianglePrefab}
		};
	}

	private readonly List<DrawShape> _shapes = new List<DrawShape>();

	private DrawShape CurrentShape { get; set; }
	private bool IsDrawingShape { get; set; }

	private void Update()
	{
		var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		// TODO: need to know when shape starts and when it finishes
		if (Input.GetKeyUp(KeyCode.Mouse0)) {
			if (CurrentShape == null) {
				var prefab = _drawModeToPrefab[DrawMode];
				CurrentShape = Instantiate(prefab);
				CurrentShape.name = "Shape " + _shapes.Count;
				
				CurrentShape.AddVertex(mousePos);
				CurrentShape.AddVertex(mousePos);
				
				IsDrawingShape = true;
				
				_shapes.Add(CurrentShape);
			} else {
				var shapeFinished = CurrentShape.AddVertex(mousePos);
				
				if (shapeFinished) {
					CurrentShape.Simulate(true);
				}
				
				IsDrawingShape = !shapeFinished;
				CurrentShape = IsDrawingShape ? CurrentShape : null;
			}
		} else if (CurrentShape != null && IsDrawingShape) {
			CurrentShape.UpdateShape(mousePos);
		}
	}
}

public enum DrawMode
{
    Rectangle, Circle, Triangle
}
