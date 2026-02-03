using System;
using System.Collections.Generic;

public sealed class SubscriptionSet : IDisposable
{
    readonly List<IDisposable> items = new();
    bool disposed;

    public int Count => items.Count;

    public void Add(IDisposable disposable)
    {
        if (disposed || disposable == null)
        {
            return;
        }
        items.Add(disposable);
    }

    public T Add<T>(T disposable) where T : IDisposable
    {
        Add((IDisposable)disposable);
        return disposable;
    }

    public void Clear()
    {
        if (disposed)
        {
            return;
        }
        for (int i = items.Count - 1; i >= 0; i--)
        {
            try { items[i]?.Dispose(); }
            catch { /* ignore */ }
        }
        items.Clear();
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }
        disposed = true;
        Clear();
    }
}

