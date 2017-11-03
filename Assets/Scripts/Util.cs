using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Vector3[] ToVector3(this Vector2[] vectors)
    {
        return System.Array.ConvertAll<Vector2, Vector3>(vectors, v => v);
    }
}
