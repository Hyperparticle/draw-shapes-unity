using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DrawShape : MonoBehaviour
{
    public abstract bool AddVertex(Vector2 vertex);
    public abstract void UpdateShape(Vector2 newVertex);
    public abstract void Simulate(bool active);
}
