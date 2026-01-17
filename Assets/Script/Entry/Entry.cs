using System;
using System.Collections.Generic;
using UnityEngine;

// Application 시작구간 , 첫 초기화 시점
public abstract class Entry : MonoBehaviour
{
    List<Universe> universes = new();
    
    void Awake()
    {
        Initialize();
        Ready();
    }

    protected virtual void Initialize()
    {
        Universe.universes.Clear();
    }

    protected virtual void Ready()
    {
        foreach (var universe in universes)
        {
            universe.Ready();
        }
    }

    protected void AddUniverse(Universe universe)
    {
        universes.Add(universe);
    }
}
