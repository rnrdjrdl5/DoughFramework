using System;

public sealed class TimeBoundaryAbility : Ability
{
    public event Action Started;
    public event Action Ended;

    public float Start { get; private set; }
    public float End { get; private set; }
    public TimeDomain Domain { get; private set; } = TimeDomain.Unscaled;

    public bool IsPending => state == State.Pending;
    public bool IsStarted => state == State.Started;
    public bool IsEnded => state == State.Ended;

    public bool HasClock => clock != null;
    public float Now => GetNow(snapshot, Domain);
    public int Frame => snapshot.Frame;

    enum State { Idle, Pending, Started, Ended }

    ClockAbility clock;
    TimeSnapshot snapshot;
    State state = State.Idle;
    float prevNow;

    protected override void OnReady()
    {
        ResolveClock();
        Subscribe();
    }

    protected override void OnUninitialize()
    {
        Unsubscribe();
        clock = null;
        state = State.Idle;
        snapshot = default;
        prevNow = 0f;
    }

    public void Configure(float start, float end, TimeDomain domain = TimeDomain.Unscaled)
    {
        if (start > end)
        {
            throw new ArgumentException("start must be <= end");
        }

        Domain = domain;
        Start = start;
        End = end;

        if (clock == null)
        {
            ResolveClock();
        }

        // 현재 시각으로 즉시 평가
        var now = GetNow(CurrentSnapshot(), Domain);
        prevNow = now;

        if (now < Start)
        {
            state = State.Pending;
            return;
        }

        if (now == Start)
        {
            EmitStarted();
            if (now >= End)
            {
                // start == end == now 케이스: 같은 프레임에 Ended도 호출
                EmitEnded();
            }
            else
            {
                state = State.Started;
            }
            return;
        }

        if (now < End)
        {
            state = State.Started; // 시작은 호출하지 않음, 진행 상태로 간주
            return;
        }

        // now >= end: 아무 이벤트도 호출하지 않음
        state = State.Ended;
    }

    void OnTick(TimeSnapshot snap)
    {
        snapshot = snap;
        var now = GetNow(snap, Domain);
        var prev = prevNow;
        prevNow = now;

        if (state == State.Ended || state == State.Idle)
        {
            return;
        }

        bool startedFired = false;

        if (state != State.Started && prev < Start && now >= Start)
        {
            EmitStarted();
            startedFired = true;
        }

        if (state != State.Ended && prev < End && now >= End)
        {
            if (!startedFired && state != State.Started && now >= Start)
            {
                // 시작 미발화 상태에서 같은 틱에 end도 넘은 경우, 시작 먼저 보장
                EmitStarted();
            }
            EmitEnded();
        }
    }

    void EmitStarted()
    {
        state = State.Started;
        try { Started?.Invoke(); } catch { }
    }

    void EmitEnded()
    {
        state = State.Ended;
        try { Ended?.Invoke(); } catch { }
    }

    void ResolveClock()
    {
        clock = AbilityResolver?.GetAbility<ClockAbility>();
        if (clock == null)
        {
            clock = GetUpstreamAbility<ClockAbility>();
        }
        if (clock != null)
        {
            snapshot = clock.Snapshot;
            prevNow = GetNow(snapshot, Domain);
        }
    }

    void Subscribe()
    {
        if (clock != null)
        {
            clock.Tick += OnTick;
        }
    }

    void Unsubscribe()
    {
        if (clock != null)
        {
            clock.Tick -= OnTick;
        }
    }

    static float GetNow(in TimeSnapshot snap, TimeDomain domain)
    {
        return domain == TimeDomain.Scaled ? snap.Time : snap.UnscaledTime;
    }

    TimeSnapshot CurrentSnapshot()
    {
        return clock != null ? clock.Snapshot : snapshot;
    }
}

