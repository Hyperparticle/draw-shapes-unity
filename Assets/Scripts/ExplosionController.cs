using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Simulates physics 2D explosions at the mouse cursor position
/// </summary>
public class ExplosionController : MonoBehaviour
{
    public float Radius = 10f;
    public float Power = 2000f;
    public KeyCode ExplosionKey = KeyCode.Space;

    private void Update()
    {
        if (!Input.GetKeyDown(ExplosionKey)) {
            return;
        }
		
        var explosionPos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CreateExplosion(explosionPos, Radius, Power);
    }

    /// <summary>
    /// Creates an explosion at the given position by adding a force to all
    /// physics 2D colliders within the radius.
    /// </summary>
    private static void CreateExplosion(Vector2 explosionPos, float radius, float power)
    {
        var colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
		
        foreach (var hit in colliders)
        {
            var body = hit.GetComponent<Rigidbody2D>();
			
            if (body != null) {
                body.AddExplosionForce(power, explosionPos, radius);
            }
        }
    }
}

/// <summary>
/// An extension class to a 2D rigidbody, which defines how to add
/// an explosion force to the rigidbody.
/// </summary>
public static class Rigidbody2DExtension
{
    /// <summary>
    /// Adds an explosion force to the rigidbody from the given position. The 
    /// force drops off by the square of the distance between the explosion 
    /// position and the rigidbody's position. 
    /// Adapted from <see cref="https://forum.unity.com/threads/need-rigidbody2d-addexplosionforce.212173/#post-1426983"/>
    /// </summary>
    public static void AddExplosionForce(this Rigidbody2D body, 
        float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = body.transform.position - explosionPosition;
        var wearoff = Mathf.Pow(1 - dir.magnitude / explosionRadius, 2); // Wear off by the square of the distance
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }
}