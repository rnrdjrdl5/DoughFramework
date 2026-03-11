using System.Collections.Generic;
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

    public static bool Roll(float minRange, float maxRange, float threshold)
    {
        if (maxRange <= minRange)
        {
            return true;
        }

        var clampedThreshold = Mathf.Clamp(threshold, minRange, maxRange);
        var roll = Random.Range(minRange, maxRange);
        return roll < clampedThreshold;
    }

    public static int SelectRandomIndexByWeight(List<float> weights)
    {
        if (weights == null || weights.Count == 0)
        {
            return -1;
        }

        var total = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            total += Mathf.Max(0f, weights[i]);
        }

        if (total <= 0f)
        {
            return -1;
        }

        var roll = Random.Range(0f, total);
        var cumulative = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            cumulative += Mathf.Max(0f, weights[i]);
            if (roll < cumulative)
            {
                return i;
            }
        }

        return weights.Count - 1;
    }

    public static int SelectRandomIndexByWeight(List<int> weights)
    {
        if (weights == null || weights.Count == 0)
        {
            return -1;
        }

        int total = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            total += Mathf.Max(0, weights[i]);
        }

        if (total <= 0)
        {
            return -1;
        }

        int roll = Random.Range(0, total);
        int cumulative = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            cumulative += Mathf.Max(0, weights[i]);
            if (roll < cumulative)
            {
                return i;
            }
        }

        return weights.Count - 1;
    }
}
