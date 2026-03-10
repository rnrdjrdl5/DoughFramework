using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 SetX(this Vector3 vector, float x)
    {
        var v3 = new Vector3(x, vector.y, vector.z);
        return v3;
    }

    public static Vector3 SetY(this Vector3 vector, float y)
    {
        var v3 =  new Vector3(vector.x, y, vector.z);
        return v3;
    }

    public static Vector3 SetZ(this Vector3 vector, float z)
    {
        var v3 =  new Vector3(vector.x, vector.y, z);
        return v3;
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}