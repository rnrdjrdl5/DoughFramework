using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class AbilityCommandCache
{
    public sealed class HandlerInfo
    {
        public Type CommandType;
        public MethodInfo Method;
    }

    static readonly Dictionary<Type, HandlerInfo[]> cache = new();

    public static HandlerInfo[] GetHandlers(Type abilityType)
    {
        if (cache.TryGetValue(abilityType, out var cached))
        {
            return cached;
        }

        var handlers = abilityType
            .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Select(m => (method: m, attr: m.GetCustomAttribute<HandlesCommandAttribute>()))
            .Where(x => x.attr != null)
            .Where(x => IsValidSignature(x.method, x.attr!.CommandType))
            .Select(x => new HandlerInfo
            {
                CommandType = x.attr!.CommandType,
                Method = x.method
            })
            .ToArray();

        cache[abilityType] = handlers;
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

        // 정확 일치 매칭
        return parameters[0].ParameterType == commandType;
    }

    // 부트스트랩 시 1회 호출용 클린 API(외부에서 호출)
    public static void Clear()
    {
        cache.Clear();
    }
}

