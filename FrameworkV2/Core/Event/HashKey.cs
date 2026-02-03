using System;

public static class HashKey
{
    // FNA-1a Hash 알고리즘
    public static int FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Event name must be non-empty.", nameof(name));
        }

        unchecked
        {
            uint hash = 2166136261;
            for (int i = 0; i < name.Length; i++)
            {
                hash ^= name[i];
                hash *= 16777619;
            }
            return (int)hash;
        }
    }
}
