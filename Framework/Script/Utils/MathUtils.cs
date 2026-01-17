using UnityEngine;

public static class MathUtils
{
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        
        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }
}
