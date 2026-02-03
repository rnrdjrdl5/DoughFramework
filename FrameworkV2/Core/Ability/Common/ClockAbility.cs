public sealed class ClockAbility : Ability
{
    TimeSnapshot snapshot;

    public TimeSnapshot Snapshot => snapshot;
    public int Frame => snapshot.Frame;
    public event System.Action<TimeSnapshot> Tick;

    public float Now(TimeDomain domain = TimeDomain.Scaled)
    {
        return domain == TimeDomain.Scaled ? snapshot.Time : snapshot.UnscaledTime;
    }

    public float Delta(TimeDomain domain = TimeDomain.Scaled)
    {
        return domain == TimeDomain.Scaled ? snapshot.DeltaTime : snapshot.UnscaledDeltaTime;
    }

    public void UpdateSnapshot(in TimeSnapshot s)
    {
        snapshot = s;
        try { Tick?.Invoke(snapshot); } catch { }
    }
}
