using System.Collections.Generic;
using UnityEngine;

public class Brain : Entity
{
    public EventListener InteractionEvent => interactionEvent;
    public event System.Action<Entity> OnSetControlEntity;
    public event System.Action<Entity> OnUnsetControlEntity;
    
    protected int[] eventIds;
    
    EventListener interactionEvent = new();
    Entity controlledEntity; // TODO : 두 개 이상의 Entity를 조종해야한다면, 수정 필요
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

        InitializeEventId();
    }
    
    protected virtual void InitializeEventId()
    {
        foreach (var eventId in eventIds)
        {
            interactionEvent.AddListener(eventId, RelayToEntity);
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
            interactionEvent.RemoveListener(eventId, RelayToEntity);
        }
    }

    public void SetControlledEntity(Entity entity)
    {
        if (controlledEntity != null)
        {
            UnsetControlledEntity();
        }

        controlledEntity = entity;
        OnSetControlEntity?.Invoke(controlledEntity);
    }

    public void UnsetControlledEntity()
    {
        if (controlledEntity == null)
        {
            return;
        }

        OnUnsetControlEntity?.Invoke(controlledEntity);
        controlledEntity = null;
    }
    
    public void RelayToEntity(int eventId, Values values)
    {
        if (controlledEntity == null)
        {
            return;
        }
        
        var eventListener = controlledEntity.GetEntityData<EventListener>();
        if (eventListener != null)
        {
            eventListener.ExecuteListeners(eventId, values);
        }
    }
}