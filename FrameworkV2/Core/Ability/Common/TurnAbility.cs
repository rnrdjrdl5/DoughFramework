using System;
using System.Collections.Generic;

public readonly struct TurnContext
{
    public string ActorId { get; }
    public int TurnIndex { get; }
    public int Round { get; }

    public TurnContext(string actorId, int turnIndex, int round)
    {
        ActorId = actorId;
        TurnIndex = turnIndex;
        Round = round;
    }
}

public sealed class TurnAbility : Ability
{
    public event Action<TurnContext> TurnStarted;
    public event Action<TurnContext> TurnEnded;
    public event Action<int> RoundStarted;
    public event Action<int> RoundEnded;

    public string CurrentActorId => IsRunning ? currentActorId : null;
    public int TurnIndex { get; private set; } = -1;
    public int Round { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsPaused => isPaused;
    public int ActionCost { get; set; }

    Dictionary<string, int> points = new();
    string currentActorId;
    bool isPaused;

    public void SetPoint(string actorId, int point)
    {
        if (string.IsNullOrWhiteSpace(actorId))
        {
            throw new ArgumentException("actorId must be non-empty.", nameof(actorId));
        }

        points[actorId] = point;
    }

    public bool TryGetPoint(string actorId, out int point)
    {
        if (string.IsNullOrWhiteSpace(actorId))
        {
            point = 0;
            return false;
        }

        return points.TryGetValue(actorId, out point);
    }

    public void AddPoint(string actorId, int delta)
    {
        if (string.IsNullOrWhiteSpace(actorId))
        {
            throw new ArgumentException("actorId must be non-empty.", nameof(actorId));
        }

        points.TryGetValue(actorId, out var current);
        points[actorId] = current + delta;
    }

    public bool RemoveActor(string actorId)
    {
        if (string.IsNullOrWhiteSpace(actorId))
        {
            return false;
        }

        if (!points.Remove(actorId))
        {
            return false;
        }

        if (!IsRunning)
        {
            return true;
        }

        if (!string.Equals(currentActorId, actorId, StringComparison.Ordinal))
        {
            return true;
        }

        EmitTurnEnded(BuildContext());

        if (points.Count == 0)
        {
            Stop();
            return true;
        }

        TurnIndex++;
        if (TurnIndex % points.Count == 0)
        {
            EmitRoundEnded(Round);
            Round++;
            EmitRoundStarted(Round);
        }

        currentActorId = SelectNextActor();
        if (string.IsNullOrWhiteSpace(currentActorId))
        {
            Stop();
            return true;
        }

        EmitTurnStarted(BuildContext());
        return true;
    }

    public void Start()
    {
        if (points.Count == 0)
        {
            throw new InvalidOperationException("At least one participant is required.");
        }

        if (IsRunning)
        {
            return;
        }

        IsRunning = true;
        isPaused = false;
        Round = 1;
        TurnIndex = 0;
        currentActorId = SelectNextActor();
        if (string.IsNullOrWhiteSpace(currentActorId))
        {
            Stop();
            return;
        }

        EmitRoundStarted(Round);
        EmitTurnStarted(BuildContext());
    }

    public void EndTurn()
    {
        if (!IsRunning)
        {
            return;
        }

        if (isPaused)
        {
            return;
        }

        if (points.Count == 0)
        {
            Stop();
            return;
        }

        EmitTurnEnded(BuildContext());

        AddPoint(currentActorId, ActionCost);

        TurnIndex++;
        if (TurnIndex % points.Count == 0)
        {
            EmitRoundEnded(Round);
            Round++;
            EmitRoundStarted(Round);
        }

        currentActorId = SelectNextActor();
        if (string.IsNullOrWhiteSpace(currentActorId))
        {
            Stop();
            return;
        }
        EmitTurnStarted(BuildContext());
    }

    public void Stop()
    {
        IsRunning = false;
        TurnIndex = -1;
        Round = 0;
        isPaused = false;
        currentActorId = null;
    }

    public void Pause()
    {
        if (!IsRunning || isPaused)
        {
            return;
        }

        isPaused = true;
    }

    public void Resume()
    {
        if (!IsRunning || !isPaused)
        {
            return;
        }

        isPaused = false;
    }

    public bool TryGetCurrentActorId(out string actorId)
    {
        actorId = null;
        if (!IsRunning || string.IsNullOrWhiteSpace(currentActorId))
        {
            return false;
        }
        actorId = currentActorId;
        return true;
    }

    protected override void OnUninitialize()
    {
        points.Clear();
        IsRunning = false;
        TurnIndex = -1;
        Round = 0;
        isPaused = false;
        currentActorId = null;
    }

    TurnContext BuildContext()
    {
        var actorId = currentActorId;
        return new TurnContext(actorId, TurnIndex, Round);
    }

    void EmitTurnStarted(TurnContext ctx)
    {
        try { TurnStarted?.Invoke(ctx); } catch { }
    }

    void EmitTurnEnded(TurnContext ctx)
    {
        try { TurnEnded?.Invoke(ctx); } catch { }
    }

    void EmitRoundStarted(int round)
    {
        try { RoundStarted?.Invoke(round); } catch { }
    }

    void EmitRoundEnded(int round)
    {
        try { RoundEnded?.Invoke(round); } catch { }
    }

    string SelectNextActor()
    {
        if (points.Count == 0)
        {
            return null;
        }

        var min = int.MaxValue;
        foreach (var entry in points)
        {
            if (entry.Value < min)
            {
                min = entry.Value;
            }
        }

        if (min != 0)
        {
            var keys = new List<string>(points.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                points[key] -= min;
            }
        }

        foreach (var entry in points)
        {
            if (entry.Value == 0)
            {
                return entry.Key;
            }
        }

        return null;
    }

}
