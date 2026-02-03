using System;

// Minimal Vector3-like struct without UnityEngine dependency
public readonly struct Vec3
{
    public readonly float X;
    public readonly float Y;
    public readonly float Z;

    public Vec3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public float SqrMagnitude => X * X + Y * Y + Z * Z;
    public float Magnitude => (float)Math.Sqrt(SqrMagnitude);
    public Vec3 Normalized => Magnitude > 1e-6f ? this * (1.0f / Magnitude) : new Vec3(0, 0, 0);

    public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator *(Vec3 a, float s) => new Vec3(a.X * s, a.Y * s, a.Z * s);
    public static Vec3 operator *(float s, Vec3 a) => a * s;

    public override string ToString() => $"({X:0.###}, {Y:0.###}, {Z:0.###})";
}

