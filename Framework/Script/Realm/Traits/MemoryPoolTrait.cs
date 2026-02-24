using System;
using System.Collections.Generic;

public class MemoryPoolTrait : Trait
{
    public interface IMemoryPool
    {
        public MemoryPoolTrait MemoryPoolTrait { get; set; }

        public void ReleasePool();
    }
    
    interface IMemoryStorage
    {
        public int Count { get; }
    }

    class MemoryStorage<T> : IMemoryStorage where T : class, IMemoryPool, new()
    {
        List<T> reservedMemory = new();
        MemoryPoolTrait memoryPoolModule;

        public MemoryStorage(MemoryPoolTrait memoryPoolModule)
        {
            this.memoryPoolModule = memoryPoolModule;
        }
        
        public T Pool()
        {
            if (reservedMemory.Count == 0)
            {
                var newMemory = new T();
                newMemory.MemoryPoolTrait = memoryPoolModule;
                
                reservedMemory.Add(newMemory);
            }

            var reservedData = reservedMemory[^1];
            reservedMemory.RemoveAt(reservedMemory.Count - 1);

            return reservedData;
        }

        public void Reserve(T reserveData)
        {
            reservedMemory.Add(reserveData);
        }
        
        public int Count => reservedMemory.Count;
    }

    Dictionary<Type, IMemoryStorage> memoryPools = new();

    public T Pool<T>() where T : class, IMemoryPool, new()
    {
        if (!memoryPools.TryGetValue(typeof(T), out var storage))
        {
            storage = new MemoryStorage<T>(this);
            memoryPools[typeof(T)] = storage;
        }

        var reservedData = (storage as MemoryStorage<T>).Pool();
        reservedData.MemoryPoolTrait = this;
        
        return reservedData;
    }

    public void Release<T>(T value) where T : class, IMemoryPool, new()
    {
        if (!memoryPools.TryGetValue(typeof(T), out var storage))
        {
            storage = new MemoryStorage<T>(this);
            memoryPools[typeof(T)] = storage;
        }
        
        (storage as MemoryStorage<T>).Reserve(value);
    }

    public int GetTotalCount()
    {
        var total = 0;
        foreach (var kv in memoryPools)
        {
            total += kv.Value.Count;
        }

        return total;
    }
}