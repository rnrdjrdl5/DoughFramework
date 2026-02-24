using System;
using System.Collections.Generic;

public class ListPool<T> : IDisposable, MemoryPoolAbility.IMemoryPool
{
    public static ListPool<T> Create(MemoryPoolAbility memoryPoolAbility)
    {
        var listPool = new ListPool<T>();
        listPool.MemoryPoolAbility = memoryPoolAbility;

        return listPool;
    }
    
    public MemoryPoolAbility MemoryPoolAbility { get; set; }
    public void ReleasePool()
    {
        Dispose();
    }

    public List<T> list = new();
    
    public void Dispose()
    {
        foreach (var item in list)
        {
            if (item is MemoryPoolAbility.IMemoryPool memoryPool)
            {
                memoryPool.ReleasePool();
            }
        }
        
        list.Clear();
        MemoryPoolAbility.Release(this);
    }
}