using UnityEngine;
using System;
using System.Collections.Generic;

public class MessageBus : IEntityData
{
    readonly Dictionary<Type, List<Delegate>> handlers = new();

    public void Initialize(Parameter parameter)
    {
        handlers.Clear();
    }

    public void Uninitialize()
    {
        handlers.Clear();
    }

    public void Subscribe<T>(Action<T> handler)
    {
        if (handler == null)
            return;

        var type = typeof(T);
        if (!handlers.TryGetValue(type, out var list))
        {
            list = new List<Delegate>();
            handlers.Add(type, list);
        }

        if (!list.Contains(handler))
            list.Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler)
    {
        if (handler == null)
            return;

        var type = typeof(T);
        if (!handlers.TryGetValue(type, out var list))
            return;

        list.Remove(handler);
        if (list.Count == 0)
            handlers.Remove(type);
    }

    public void Publish<T>(T message)
    {
        var type = typeof(T);
        if (!handlers.TryGetValue(type, out var list))
            return;

        // Snapshot to avoid modification during iteration.
        var snapshot = list.ToArray();
        for (int i = 0; i < snapshot.Length; i++)
        {
            if (snapshot[i] is Action<T> handler)
                handler.Invoke(message);
        }
    }
}
