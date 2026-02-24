using System;
using System.Collections.Generic;
using UnityEngine;

public class Parameter : IDisposable, MemoryPoolAbility.IMemoryPool
{
    interface IParameter
    {
        public void Clear();
    }

    class ParamData<T> : IParameter
    {
        public T Data;

        public void Clear()
        {
            if (Data is MemoryPoolAbility.IMemoryPool memoryPool)
            {
                memoryPool.ReleasePool();
            }
        }
    }
    
    public static Parameter Create(MemoryPoolAbility memoryPoolAbility)
    {
        var parameter = memoryPoolAbility.Pool<Parameter>();
        
        return parameter;
    }
    
    public MemoryPoolAbility MemoryPoolAbility { get; set; }
    public int Count => parameters.Count;
    
    Dictionary<string, IParameter> parameters = new();
    
    public void ReleasePool()
    {
        Dispose();
    }

    public void Set<T1>(string key1, T1 value1)
    {
        parameters[key1] = CreateParam(value1);
    }

    public void Set<T1, T2>(string key1, T1 value1, string key2, T2 value2)
    {
        parameters[key1] = CreateParam(value1);
        parameters[key2] = CreateParam(value2);
    }

    public void Set<T1, T2, T3>(string key1, T1 value1, string key2, T2 value2, string key3, T3 value3)
    {
        parameters[key1] = CreateParam(value1);
        parameters[key2] = CreateParam(value2);
        parameters[key3] = CreateParam(value3);
    }

    public void Set<T1, T2, T3, T4>(string key1, T1 value1, string key2, T2 value2, string key3, T3 value3, string key4, T4 value4)
    {
        parameters[key1] = CreateParam(value1);
        parameters[key2] = CreateParam(value2);
        parameters[key3] = CreateParam(value3);
        parameters[key4] = CreateParam(value4);
    }

    public void Set<T1, T2, T3, T4, T5>(string key1, T1 value1, string key2, T2 value2, string key3, T3 value3, string key4, T4 value4, string key5, T5 value5)
    {
        parameters[key1] = CreateParam(value1);
        parameters[key2] = CreateParam(value2);
        parameters[key3] = CreateParam(value3);
        parameters[key4] = CreateParam(value4);
        parameters[key5] = CreateParam(value5);
    }
    
    ParamData<T> CreateParam<T>(T data)
    {
        var paramData = new ParamData<T>();
        paramData.Data = data;
        
        return paramData;
    }
    
    public void Add<T>(string key, T value)
    {
        if (!parameters.TryGetValue(key, out var paramData))
        {
            paramData = new ParamData<T>();
            (paramData as ParamData<T>).Data = value;
        }
        
        parameters[key] = paramData;
    }

    public T Get<T>(string key)
    {
        return parameters.TryGetValue(key, out var paramData) ? (paramData as ParamData<T>).Data : default;
    }

    public void Dispose()
    {
        foreach (var parameter in parameters.Values)
        {
            parameter.Clear();
        }
        
        parameters.Clear();
        MemoryPoolAbility.Release(this);
    }
}