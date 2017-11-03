using UnityEngine;

public class ExplosionController : MonoBehaviour
{
	public float Radius = 10f;
	public float Power = 2000f;

	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.Space)) {
			return;
		}
		
		var explosionPos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var colliders = Physics2D.OverlapCircleAll(explosionPos, Radius);
		
		foreach (var hit in colliders)
		{
			var body = hit.GetComponent<Rigidbody2D>();

			if (body != null) {
				body.AddExplosionForce(Power, explosionPos, Radius);
			}
		}
	}
}

/// <summary>
/// Adapted from <see cref="https://forum.unity.com/threads/need-rigidbody2d-addexplosionforce.212173/#post-1426983"/>
/// </summary>
public static class Rigidbody2DExtension
{
	public static void AddExplosionForce(this Rigidbody2D body, 
		float explosionForce, Vector3 explosionPosition, float explosionRadius)
	{
		var dir = body.transform.position - explosionPosition;
		var wearoff = Mathf.Pow(1 - dir.magnitude / explosionRadius, 2); // Wear off by the square of the distance
		body.AddForce(dir.normalized * explosionForce * wearoff);
	}
}
