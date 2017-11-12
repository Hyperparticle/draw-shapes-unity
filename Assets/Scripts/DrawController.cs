using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <inheritdoc />
/// <summary>
/// Listens for input from the mouse, where shapes are created and updated by 
/// the current cursor position.
/// </summary>
public class DrawController : MonoBehaviour
{
	public DrawRectangle RectanglePrefab;

	private readonly List<DrawRectangle> _allShapes = new List<DrawRectangle>();

	private DrawRectangle CurrentShapeToDraw { get; set; }
	private bool IsDrawingShape { get; set; }

	private void Update()
	{
		var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

		var click = Input.GetKeyUp(KeyCode.Mouse0);
		var canUpdateShape = CurrentShapeToDraw != null && IsDrawingShape;
		
		if (click) {
			AddShapeVertex(mousePos);
		} else if (canUpdateShape) {
			UpdateShapeVertex(mousePos);
		}
	}

	/// <summary>
	/// Adds a new vertex to the current shape at the given position, 
	/// or creates a new shape if it doesn't exist
	/// </summary>
	private void AddShapeVertex(Vector2 position)
	{
		if (CurrentShapeToDraw == null) {
			// No current shape -> instantiate a new shape and add two vertices:
			// one for the initial position, and the other for the current cursor
			var prefab = RectanglePrefab;
			CurrentShapeToDraw = Instantiate(prefab);
			CurrentShapeToDraw.name = "Shape " + _allShapes.Count;
				
			CurrentShapeToDraw.AddVertex(position);
			CurrentShapeToDraw.AddVertex(position);
			
			IsDrawingShape = true;
				
			_allShapes.Add(CurrentShapeToDraw);
		} else {
			// Current shape exists -> add vertex if finished, 
			// otherwise start physics simulation and reset reference
			IsDrawingShape = !CurrentShapeToDraw.ShapeFinished;

			if (IsDrawingShape) {
				CurrentShapeToDraw.AddVertex(position);
			} else {
				CurrentShapeToDraw = null;
			}
		}
	}

	/// <summary>
	/// Updates the current shape's latest vertex position to allow
	/// a shape to be updated with the mouse cursor and redrawn
	/// </summary>
	private void UpdateShapeVertex(Vector2 position)
	{
		if (CurrentShapeToDraw == null) {
			return;
		}
		
		CurrentShapeToDraw.UpdateShape(position);
	}
}


