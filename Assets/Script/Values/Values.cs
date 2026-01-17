using System;
using System.Collections.Generic;

public class Values : IDisposable, MemoryPoolModule.IMemoryPool
{
    interface IValue
    {
        int Count { get; }
        void Clear();
    }
    class ValueData<T> : IValue
    {
        public ListPool<T> values;
        public int Count => values.list.Count;

        public void Add(T value)
        {
            values.list.Add(value);
        }

        public bool TryGet(int index, out T result)
        {
            if (index >= 0 && index < values.list.Count)
            {
                result = values.list[index];
                return true;
            }

            result = default;
            return false;
        }

        public void Clear()
        {
            values.Dispose();
            values = null;
        }
    }
    
    public static Values Create(MemoryPoolModule memoryPoolModule)
    {
        var values = memoryPoolModule.Pool<Values>();
        
        return values;
    }
    
    public MemoryPoolModule MemoryPoolModule { get; set; }
    
    Dictionary<Type, IValue> values = new();
    
    public void ReleasePool()
    {
        Dispose();
    }

    public void Add<T>(T value)
    {
        if (!values.TryGetValue(typeof(T), out var iValue))
        {
            var createdValueData = new ValueData<T>();
            iValue = createdValueData;
            values[typeof(T)] = iValue;
        }

        var valueData = iValue as ValueData<T>;
        if (valueData.values == null)
        {
            var listPool = MemoryPoolModule.Pool<ListPool<T>>();
            valueData.values = listPool;
        }
        
        ((ValueData<T>)iValue).Add(value);
    }

    public bool TryGet<T>(out T result,int index = 0)
    {
        result = default;
        
        return values.TryGetValue(typeof(T), out var iValue) &&
               ((ValueData<T>)iValue).TryGet(index, out result);
    }

    public int GetCount<T>() => values.TryGetValue(typeof(T), out var iValue) ? iValue.Count : 0;

    public void Dispose()
    {
        foreach (var iValue in values.Values)
        {
            iValue.Clear();
        }
        
        values.Clear();
        
        MemoryPoolModule.Release(this);
    }
}