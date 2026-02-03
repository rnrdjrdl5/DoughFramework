public readonly struct TimeSnapshot
{
    public float Time { get; }
    public float DeltaTime { get; }
    public float UnscaledTime { get; }
    public float UnscaledDeltaTime { get; }
    public int Frame { get; }

    public TimeSnapshot(float time, float deltaTime, float unscaledTime, float unscaledDeltaTime, int frame)
    {
        Time = time;
        DeltaTime = deltaTime;
        UnscaledTime = unscaledTime;
        UnscaledDeltaTime = unscaledDeltaTime;
        Frame = frame;
    }
}

