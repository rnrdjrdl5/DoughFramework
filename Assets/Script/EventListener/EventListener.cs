using System;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : IActorData
{
    Dictionary<int, List<Action<int, Values>>> listeners = new();

    public void Initialize(Parameter parameter)
    {
        listeners.Clear();
    }

    public void Uninitialize()
    {
        listeners.Clear();
    }
    
    public void AddListener(int key, Action<int, Values> listener)
    {
        if (listener == null) 
            return;
        
        if (!listeners.TryGetValue(key, out var listenerList))
        {
            listenerList = new List<Action<int, Values>>();
            listeners.Add(key, listenerList);
        }
        
        if (!listenerList.Contains(listener))
        {
            listenerList.Add(listener);
        }
    }
    
    public void RemoveListener(int key, Action<int, Values> listener)
    {
        if (listeners.TryGetValue(key, out var listenerList))
        {
            listenerList.Remove(listener);
            
            if (listenerList.Count == 0)
            {
                listeners.Remove(key);
            }
        }
    }
    
    public void ExecuteListeners(int key, Values param = null)
    {
        if (!listeners.TryGetValue(key, out var listenerList))
            return;
            
        foreach (var listener in listenerList)
        {
            listener.Invoke(key, param);
        }
    }
}
