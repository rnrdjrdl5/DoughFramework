using System;
using System.Collections.Generic;

public class ListPool<T> : IDisposable, MemoryPoolTrait.IMemoryPool
{
    public static ListPool<T> Create(MemoryPoolTrait memoryPoolTrait)
    {
        var listPool = new ListPool<T>();
        listPool.MemoryPoolTrait = memoryPoolTrait;

        return listPool;
    }
    
    public MemoryPoolTrait MemoryPoolTrait { get; set; }
    public void ReleasePool()
    {
        Dispose();
    }

    public List<T> list = new();
    
    public void Dispose()
    {
        foreach (var item in list)
        {
            if (item is MemoryPoolTrait.IMemoryPool memoryPool)
            {
                memoryPool.ReleasePool();
            }
        }
        
        list.Clear();
        MemoryPoolTrait.Release(this);
    }
}