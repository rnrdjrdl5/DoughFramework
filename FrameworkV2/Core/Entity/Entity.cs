using System;
using System.Collections.Generic;
using System.Linq;


public abstract partial class Entity : ILifecycle
{
    public string Id => identity.Id;
    public IReadOnlyList<string> Aliases => aliases.Aliases;
    public IReadOnlyList<Ability> Abilities => abilitySet.Abilities;
    public IAbilityResolver UpstreamAbilityResolver { get; private set; }
    public bool IsInitialized => isInitialized;
    public bool IsReady => isReady;
    
    readonly Identity identity;
    readonly AliasSet aliases = new();
    readonly AbilitySet abilitySet;
    bool isInitialized;
    bool isReady;

    protected Entity()
    {
        identity = new Identity();
        abilitySet = new AbilitySet(
            onAdded: OnAbilityAdded,
            onRemoved: OnAbilityRemoved);
        AbilityAttributeInstaller.Apply(this, AddAbility);
    }

    public void AddAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException("Alias must be non-empty.", nameof(alias));
        }

        aliases.Add(alias);
    }

    public bool RemoveAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return false;
        }

        return aliases.Remove(alias);
    }

    public bool HasAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return false;
        }

        return aliases.Has(alias);
    }

    public void AddAbility(Ability ability)
    {
        abilitySet.Add(ability);
    }

    public bool RemoveAbility(Ability ability)
    {
        return abilitySet.Remove(ability);
    }

    public bool HasAbility<T>() where T : Ability
    {
        return abilitySet.HasAbility<T>();
    }

    public T GetAbility<T>() where T : Ability
    {
        return abilitySet.GetAbility<T>();
    }

    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }
        OnInitialize();
        isInitialized = true;
    }

    public void Ready()
    {
        if (!isInitialized || isReady)
        {
            return;
        }
        OnReady();
        isReady = true;
    }

    public void Uninitialize()
    {
        if (!isInitialized)
        {
            return;
        }
        OnUninitialize();
        isReady = false;
        isInitialized = false;
    }

    protected virtual void OnInitialize() { }
    protected virtual void OnReady() { }
    protected virtual void OnUninitialize() { }
    protected virtual void OnAbilityAdded(Ability ability)
    {
        // 상위 Resolver를 신규 Ability에 전달
        if (ability != null)
        {
            ability.AttachUpstreamResolver(UpstreamAbilityResolver);
        }
    }
    protected virtual void OnAbilityRemoved(Ability ability)
    {
        if (ability != null)
        {
            ability.DetachUpstreamResolver();
        }
    }

    internal void AttachUpstreamResolver(IAbilityResolver resolver)
    {
        UpstreamAbilityResolver = resolver;
        // 보유 Ability에도 전파
        var list = Abilities;
        for (int i = 0; i < list.Count; i++)
        {
            list[i]?.AttachUpstreamResolver(resolver);
        }
    }

    internal void DetachUpstreamResolver()
    {
        UpstreamAbilityResolver = null;
        // 보유 Ability에도 전파
        var list = Abilities;
        for (int i = 0; i < list.Count; i++)
        {
            list[i]?.DetachUpstreamResolver();
        }
    }
}
