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

    // index를 0 ~ count-1 사이로 리턴한다. 
    public static int WrapIndex(int index, int count)
    {
        return (index % count + count) % count;
    }
}
