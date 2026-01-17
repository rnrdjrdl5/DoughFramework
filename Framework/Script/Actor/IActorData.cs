using System;

// Actor에서 관리하는 내부 Data
public interface IActorData : IData
{
    void Initialize(Parameter parameter);
    void Uninitialize();
}

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class ActorDataAttribute : Attribute
{
    public Type Type;

    public ActorDataAttribute(Type type)
    {
        Type = type;
    }
}