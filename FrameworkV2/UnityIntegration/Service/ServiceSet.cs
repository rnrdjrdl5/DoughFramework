using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ServiceSet : IServiceResolver
{
    readonly Dictionary<Type, object> map = new();
    readonly List<IService> ordered = new();
    IServiceResolver resolver;

    public bool Has<T>() where T : class
    {
        var key = typeof(T);
        if (!key.IsInterface) return false;
        return map.ContainsKey(key);
    }

    public bool TryGet<T>(out T service) where T : class
    {
        var key = typeof(T);
        if (!key.IsInterface)
        {
            service = null;
            return false;
        }
        if (map.TryGetValue(key, out var obj))
        {
            service = obj as T;
            return service != null;
        }
        service = null;
        return false;
    }

    public T Get<T>() where T : class
    {
        return TryGet<T>(out var service) ? service : null;
    }

    public void RegisterAllFrom(Transform root)
    {
        Clear();
        if (root == null)
        {
            return;
        }

        var behaviours = root.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var behaviour in behaviours)
        {
            if (behaviour is not IService service)
            {
                continue;
            }

            var serviceInterfaces = behaviour.GetType().GetInterfaces()
                .Where(t => typeof(IService).IsAssignableFrom(t) && t != typeof(IService));

            bool anyRegistered = false;
            foreach (var itf in serviceInterfaces)
            {
                try
                {
                    Add(itf, service);
                    anyRegistered = true;
                }
                catch
                {
                }
            }

            if (anyRegistered)
            {
                ordered.Add(service);
                if (resolver != null && service is Service ss)
                {
                    ss.AttachService(resolver);
                }
            }
        }
    }

    public void InitializeAll()
    {
        for (int i = 0; i < ordered.Count; i++)
        {
            var s = ordered[i];
            if (s == null) continue;
            s.Initialize();
        }
    }

    public void ReadyAll()
    {
        for (int i = 0; i < ordered.Count; i++)
        {
            var s = ordered[i];
            if (s == null) continue;
            s.Ready();
        }
    }

    public void UninitializeAll()
    {
        for (int i = ordered.Count - 1; i >= 0; i--)
        {
            var s = ordered[i];
            if (s == null) continue;
            s.Uninitialize();
        }
    }

    public void AttachResolverToAll(IServiceResolver resolver)
    {
        for (int i = 0; i < ordered.Count; i++)
        {
            if (ordered[i] is Service ss)
            {
                ss.AttachService(resolver);
            }
        }
    }

    public void DetachResolverFromAll()
    {
        for (int i = 0; i < ordered.Count; i++)
        {
            if (ordered[i] is Service ss)
            {
                ss.DetachService();
            }
        }
    }

    void Add(Type interfaceType, object service)
    {
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));
        if (service == null) throw new ArgumentNullException(nameof(service));
        if (!interfaceType.IsInterface) throw new ArgumentException("Key type must be an interface type.", nameof(interfaceType));
        if (!interfaceType.IsAssignableFrom(service.GetType())) throw new ArgumentException("Instance does not implement the specified interface.");
        map[interfaceType] = service;
    }

    void Clear()
    {
        map.Clear();
        ordered.Clear();
    }

    public void ClearAll()
    {
        Clear();
    }

    public void SetResolver(IServiceResolver r)
    {
        resolver = r;
        for (int i = 0; i < ordered.Count; i++)
        {
            if (ordered[i] is Service ss)
            {
                if (resolver != null)
                {
                    ss.AttachService(resolver);
                }
                else
                {
                    ss.DetachService();
                }
            }
        }
    }
}
