using UnityEngine;
using System.Collections;

public static class Utils
{
    public static Vector2 RotateVec2(float degrees, Vector2 vector)
    {
        var rads = degrees * Mathf.Deg2Rad;
        var xNew = Mathf.Cos(rads) * vector.x - Mathf.Sin(rads) * vector.y;
        var yNew = Mathf.Sin(rads) * vector.x + Mathf.Cos(rads) * vector.y;

        return new Vector2(xNew, yNew);
    }

    public static Vector3 Project3D(Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }

    public static Vector2 Project2D(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 GetRandomDir()
    {
        var dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        return dir;
    }

    public static bool TransformEqualityCheck(Transform t1, Transform t2)
    {
        return t1.position == t2.position && t1.rotation == t2.rotation && t1.localScale == t2.localScale;
    }

}
