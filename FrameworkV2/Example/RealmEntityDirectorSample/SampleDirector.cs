using UnityEngine;

public sealed class SampleDirector : Director
{
    MessageElement messageElement;
    EventElement eventElement;

    protected override void BuildElements()
    {
        messageElement = new MessageElement("SampleDirector Ready");
        eventElement = new EventElement("SampleEvent", "SampleEvent received.");

        AddElement(messageElement);
        AddElement(eventElement);
    }

    protected override void WireElements()
    {
        var eventAbility = GetAbility<EventAbility>();
        eventElement?.Connect(eventAbility);
    }

    protected override void OnReady()
    {
        Debug.Log("SampleDirector Ready flow complete.", this);
    }
}
