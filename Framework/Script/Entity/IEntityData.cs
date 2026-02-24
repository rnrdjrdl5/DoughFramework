using System;

// Entity에서 관리하는 내부 Data
public interface IEntityData : IData
{
    void Initialize(Parameter parameter);
    void Uninitialize();
}

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class EntityDataAttribute : Attribute
{
    public Type Type;

    public EntityDataAttribute(Type type)
    {
        Type = type;
    }
}