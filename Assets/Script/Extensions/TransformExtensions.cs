using UnityEngine;

public static class TransformExtensions
{
    public static bool IsRectTransform(this Transform transform)
    {
        return transform is RectTransform;
    }

    public static RectTransform RectTransform(this Transform transform)
    {
        return transform as RectTransform;
    }
}