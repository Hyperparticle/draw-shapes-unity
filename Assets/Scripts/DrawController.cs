using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
	public DrawRectangle RectanglePrefab;
	
	private GameMode _gameMode = GameMode.Draw;

	private readonly List<DrawRectangle> _shapes = new List<DrawRectangle>();
	private DrawRectangle _currentShape;

	private bool IsDrawingShape
	{
		get { return _currentShape != null; }
		set { _currentShape = value ? _currentShape : null; }
	}

	private void Update()
	{
		if (_gameMode != GameMode.Draw) {
			return;
		}
		
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		if (Input.GetKeyUp(KeyCode.Mouse0)) {
			if (!IsDrawingShape) {
				_currentShape = Instantiate(RectanglePrefab);
				_currentShape.StartVertex(mousePos);
				_currentShape.name = "Shape " + _shapes.Count;
				
				_shapes.Add(_currentShape);
			} else {
				IsDrawingShape = false;
			}
		}

		if (IsDrawingShape) {
			_currentShape.UpdateShape(mousePos);
		}
	}
}

public enum GameMode
{
	Draw, Simulate
}
