using System;

// Entity에서 관리하는 내부 Data
public interface IEntityData : IData
{
    void Initialize(IInitData initData = null);
    void Uninitialize();
}

public interface IMessageBus
{
    MessageBus MessageBus { get; set; }
    void OnSetMessageBus();
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
