using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class AbilityAttribute : Attribute
{
    public Type[] AbilityTypes { get; }
    public int Order { get; set; }

    public AbilityAttribute(Type abilityType)
    {
        if (abilityType == null)
        {
            throw new ArgumentNullException(nameof(abilityType));
        }
        AbilityTypes = new[] { abilityType };
    }

    public AbilityAttribute(params Type[] abilityTypes)
    {
        if (abilityTypes == null || abilityTypes.Length == 0)
        {
            throw new ArgumentException("At least one ability type is required.", nameof(abilityTypes));
        }
        AbilityTypes = abilityTypes;
    }
}

