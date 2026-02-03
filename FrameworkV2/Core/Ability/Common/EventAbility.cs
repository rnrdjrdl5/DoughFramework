using System;
using System.Collections.Generic;

public sealed class EventAbility : Ability
{
    readonly Dictionary<int, List<Action>> handlers = new();
    readonly Dictionary<int, Dictionary<Type, List<Delegate>>> typedHandlers = new();

    public IDisposable Register(int key, Action handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        if (!handlers.TryGetValue(key, out var list))
        {
            list = new();
            handlers[key] = list;
        }

        list.Add(handler);
        return new Subscription(() => Unregister(key, handler));
    }

    public IDisposable Register<T>(int key, Action<T> handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        if (!typedHandlers.TryGetValue(key, out var byType))
        {
            byType = new();
            typedHandlers[key] = byType;
        }

        var type = typeof(T);
        if (!byType.TryGetValue(type, out var list))
        {
            list = new();
            byType[type] = list;
        }

        list.Add(handler);
        return new Subscription(() => Unregister(key, handler));
    }

    public void Unregister(int key, Action handler)
    {
        if (!handlers.TryGetValue(key, out var list))
        {
            return;
        }

        list.Remove(handler);
        if (list.Count == 0)
        {
            handlers.Remove(key);
        }
    }

    public void Unregister<T>(int key, Action<T> handler)
    {
        if (!typedHandlers.TryGetValue(key, out var byType))
        {
            return;
        }

        var type = typeof(T);
        if (!byType.TryGetValue(type, out var list))
        {
            return;
        }

        list.Remove(handler);
        if (list.Count == 0)
        {
            byType.Remove(type);
            if (byType.Count == 0)
            {
                typedHandlers.Remove(key);
            }
        }
    }

    public void Execute(int key)
    {
        if (!handlers.TryGetValue(key, out var list) || list.Count == 0)
        {
            return;
        }

        var snapshot = list.ToArray();
        for (int i = 0; i < snapshot.Length; i++)
        {
            try { snapshot[i](); }
            catch { }
        }
    }

    public void Execute<T>(int key, T payload)
    {
        if (!typedHandlers.TryGetValue(key, out var byType))
        {
            return;
        }

        var type = typeof(T);
        if (!byType.TryGetValue(type, out var list) || list.Count == 0)
        {
            return;
        }

        var snapshot = list.ToArray();
        for (int i = 0; i < snapshot.Length; i++)
        {
            try { ((Action<T>)snapshot[i]).Invoke(payload); }
            catch { }
        }
    }

    class Subscription : IDisposable
    {
        Action dispose;

        public Subscription(Action dispose)
        {
            this.dispose = dispose ?? throw new ArgumentNullException(nameof(dispose));
        }

        public void Dispose()
        {
            var d = dispose;
            if (d != null)
            {
                dispose = null;
                d();
            }
        }
    }
}
