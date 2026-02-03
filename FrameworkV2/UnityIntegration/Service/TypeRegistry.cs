using System;
using System.Collections.Generic;

public class TypeRegistry
{
    readonly Dictionary<Type, object> map = new();

    public void Add<T>(T service) where T : class
    {
        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        var key = typeof(T);
        if (!key.IsInterface)
        {
            throw new ArgumentException("Service key must be an interface type.", nameof(T));
        }

        map[key] = service;
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

    public bool Remove<T>() where T : class
    {
        var key = typeof(T);
        if (!key.IsInterface)
        {
            return false;
        }

        return map.Remove(key);
    }

    public void Clear()
    {
        map.Clear();
    }

    public void Add(Type interfaceType, object service)
    {
        if (interfaceType == null)
        {
            throw new ArgumentNullException(nameof(interfaceType));
        }

        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException("Key type must be an interface type.", nameof(interfaceType));
        }

        if (!interfaceType.IsAssignableFrom(service.GetType()))
        {
            throw new ArgumentException("Service instance does not implement the specified interface.", nameof(service));
        }

        map[interfaceType] = service;
    }
}
