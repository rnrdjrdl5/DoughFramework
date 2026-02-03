using System;

[AttributeUsage(AttributeTargets.Method)]
public sealed class HandlesCommandAttribute : Attribute
{
    public Type CommandType { get; }

    public HandlesCommandAttribute(Type commandType)
    {
        CommandType = commandType;
    }
}

