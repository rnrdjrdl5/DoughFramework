using System;

public static class RefreshTimingTypeExtensions
{
    public static void RunOnEnable(this RefreshTimingType timingType, Action action)
    {
        if (timingType != RefreshTimingType.Enable)
        {
            return;
        }

        action?.Invoke();
    }

    public static void RunOnUpdate(this RefreshTimingType timingType, Action action)
    {
        if (timingType != RefreshTimingType.Update)
        {
            return;
        }

        action?.Invoke();
    }

    public static void RunOnLateUpdate(this RefreshTimingType timingType, Action action)
    {
        if (timingType != RefreshTimingType.LateUpdate)
        {
            return;
        }

        action?.Invoke();
    }
}
