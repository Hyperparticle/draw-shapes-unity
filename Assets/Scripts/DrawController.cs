using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
	public DrawMode DrawMode = DrawMode.Rect;
	
	public GameObject RectanglePrefab;
	public GameObject CirclePrefab;

	private Dictionary<DrawMode, GameObject> _drawModeToPrefab;

	private void Awake()
	{
		_drawModeToPrefab = new Dictionary<DrawMode, GameObject> {
			{DrawMode.Rect, RectanglePrefab},
			{DrawMode.Circle, CirclePrefab}
		};
	}

	private readonly List<GameObject> _shapes = new List<GameObject>();
	private GameObject _currentShape;

	private bool IsDrawingShape
	{
		get { return _currentShape != null; }
		set { _currentShape = value ? _currentShape : null; }
	}

	private void Update()
	{
		var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		if (Input.GetKeyUp(KeyCode.Mouse0)) {
			if (!IsDrawingShape) {
				var prefab = _drawModeToPrefab[DrawMode];
				_currentShape = Instantiate(prefab);
				_currentShape.name = "Shape " + _shapes.Count;
				_currentShape.SendMessage("StartVertex", mousePos);
				
				_shapes.Add(_currentShape);
			} else {
				_currentShape.SendMessage("Simulate", true);
				IsDrawingShape = false;
			}
		}

		if (IsDrawingShape) {
			_currentShape.SendMessage("UpdateShape", mousePos);
		}
	}
}

public enum DrawMode
{
    Rect, Circle
}
