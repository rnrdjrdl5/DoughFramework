using System;

public static class SubscriptionExtensions
{
    // Adds the disposable to the set and returns it for chaining.
    public static T AddTo<T>(this T disposable, SubscriptionSet set) where T : IDisposable
    {
        if (set != null && disposable != null)
        {
            set.Add(disposable);
        }
        return disposable;
    }
}

