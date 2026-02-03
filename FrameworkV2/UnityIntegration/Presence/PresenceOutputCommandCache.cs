using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class PresenceOutputCommandCache
{
    public sealed class HandlerInfo
    {
        public Type CommandType;
        public MethodInfo Method;
    }

    static readonly Dictionary<Type, HandlerInfo[]> cache = new();

    public static HandlerInfo[] GetHandlers(Type presenceType)
    {
        if (cache.TryGetValue(presenceType, out var cached))
        {
            return cached;
        }

        var handlers = presenceType
            .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Select(m => (method: m, attr: m.GetCustomAttribute<HandlesOutputCommandAttribute>()))
            .Where(x => x.attr != null)
            .Where(x => IsValidSignature(x.method, x.attr!.CommandType))
            .Select(x => new HandlerInfo
            {
                CommandType = x.attr!.CommandType,
                Method = x.method
            })
            .ToArray();

        cache[presenceType] = handlers;
        return handlers;
    }

    static bool IsValidSignature(MethodInfo method, Type commandType)
    {
        if (method.ReturnType != typeof(void))
        {
            return false;
        }

        var parameters = method.GetParameters();
        if (parameters.Length != 1)
        {
            return false;
        }

        return parameters[0].ParameterType == commandType;
    }

    public static void Clear()
    {
        cache.Clear();
    }
}
