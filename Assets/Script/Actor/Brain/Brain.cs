using System.Collections.Generic;
using UnityEngine;

public class Brain : Actor
{
    public EventListener InteractionEvent => interactionEvent;
    public event System.Action<Actor> OnSetControlActor;
    public event System.Action<Actor> OnUnsetControlActor;
    
    protected int[] eventIds;
    
    EventListener interactionEvent = new();
    Actor controlledActor; // TODO : 두 개 이상의 Actor를 조종해야한다면, 수정 필요
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

        InitializeEventId();
    }
    
    protected virtual void InitializeEventId()
    {
        foreach (var eventId in eventIds)
        {
            interactionEvent.AddListener(eventId, RelayToActor);
        }
    }

    public override void Uninitialize()
    {
        UninitializeEvents();
        
        base.Uninitialize();
    }

    void UninitializeEvents()
    {
        foreach (var eventId in eventIds)
        {
            interactionEvent.RemoveListener(eventId, RelayToActor);
        }
    }

    public void SetControlledActor(Actor actor)
    {
        if (controlledActor != null)
        {
            UnsetControlledActor();
        }

        controlledActor = actor;
        OnSetControlActor?.Invoke(controlledActor);
    }

    public void UnsetControlledActor()
    {
        if (controlledActor == null)
        {
            return;
        }

        OnUnsetControlActor?.Invoke(controlledActor);
        controlledActor = null;
    }
    
    public void RelayToActor(int eventId, Values values)
    {
        if (controlledActor == null)
        {
            return;
        }
        
        var eventListener = controlledActor.GetActorData<EventListener>();
        if (eventListener != null)
        {
            eventListener.ExecuteListeners(eventId, values);
        }
    }
}