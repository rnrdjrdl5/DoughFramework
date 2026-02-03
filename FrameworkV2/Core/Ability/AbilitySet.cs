using System;
using System.Collections.Generic;
using System.Linq;

public sealed class AbilitySet : IAbilityResolver
{
    public IReadOnlyList<Ability> Abilities => abilities;

    readonly Action<Ability> onAdded;
    readonly Action<Ability> onRemoved;
    readonly List<Ability> abilities = new();

    public AbilitySet(Action<Ability> onAdded = null, Action<Ability> onRemoved = null)
    {
        this.onAdded = onAdded;
        this.onRemoved = onRemoved;
    }

    public void Add(Ability ability)
    {
        if (ability == null)
        {
            throw new ArgumentNullException(nameof(ability));
        }

        abilities.Add(ability);
        ability.AttachResolver(this);

        ability.Initialize();
        ability.Ready();

        onAdded?.Invoke(ability);
    }

    public bool Remove(Ability ability)
    {
        if (ability == null)
        {
            return false;
        }

        var removed = abilities.Remove(ability);
        if (!removed)
        {
            return false;
        }

        onRemoved?.Invoke(ability);
        ability.Uninitialize();
        ability.DetachResolver();
        return true;
    }

    public bool HasAbility<T>() where T : Ability
    {
        return abilities.Any(a => a is T);
    }

    public T GetAbility<T>() where T : Ability
    {
        return abilities.OfType<T>().FirstOrDefault();
    }
}
