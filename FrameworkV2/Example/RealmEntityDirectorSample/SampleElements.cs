using System;
using UnityEngine;

public sealed class MessageElement : Element
{
    readonly string message;

    public MessageElement(string message)
    {
        this.message = message;
    }

    public override void Ready()
    {
        if (!string.IsNullOrEmpty(message))
        {
            Debug.Log(message);
        }
    }
}

public sealed class EventElement : Element
{
    readonly int eventKey;
    readonly string payloadMessage;

    EventAbility eventAbility;
    IDisposable subscription;

    public EventElement(string eventName, string payloadMessage)
    {
        eventKey = HashKey.FromName(eventName);
        this.payloadMessage = payloadMessage;
    }

    public void Connect(EventAbility eventAbility)
    {
        this.eventAbility = eventAbility;
    }

    public override void Ready()
    {
        if (eventAbility == null)
        {
            Debug.LogWarning("EventAbility not found.");
            return;
        }

        subscription = eventAbility.Register(eventKey, OnEvent);
        eventAbility.Execute(eventKey);
    }

    public override void Uninitialize()
    {
        subscription?.Dispose();
        subscription = null;
    }

    void OnEvent()
    {
        if (!string.IsNullOrEmpty(payloadMessage))
        {
            Debug.Log(payloadMessage);
        }
    }
}
