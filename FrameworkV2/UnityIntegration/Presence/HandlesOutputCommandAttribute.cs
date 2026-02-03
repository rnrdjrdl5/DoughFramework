using System;

[AttributeUsage(AttributeTargets.Method)]
public sealed class HandlesOutputCommandAttribute : Attribute
{
    public Type CommandType { get; }

    public HandlesOutputCommandAttribute(Type commandType)
    {
        CommandType = commandType;
    }
}

