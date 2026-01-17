using System;
using System.Collections.Generic;

public class ListPool<T> : IDisposable, MemoryPoolModule.IMemoryPool
{
    public static ListPool<T> Create(MemoryPoolModule memoryPoolModule)
    {
        var listPool = new ListPool<T>();
        listPool.MemoryPoolModule = memoryPoolModule;

        return listPool;
    }
    
    public MemoryPoolModule MemoryPoolModule { get; set; }
    public void ReleasePool()
    {
        Dispose();
    }

    public List<T> list = new();
    
    public void Dispose()
    {
        foreach (var item in list)
        {
            if (item is MemoryPoolModule.IMemoryPool memoryPool)
            {
                memoryPool.ReleasePool();
            }
        }
        
        list.Clear();
        MemoryPoolModule.Release(this);
    }
}