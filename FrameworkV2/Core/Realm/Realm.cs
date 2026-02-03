using System;
using System.Collections.Generic;
using System.Linq;

[Ability(typeof(BuildRealmAbility))]
[Ability(typeof(SpawnEntityAbility))]
[Ability(typeof(ClockAbility))]
public partial class Realm : ILifecycle
{
    public string Id => identity.Id;
    public IReadOnlyList<Realm> Children => children;
    public IReadOnlyList<Ability> Abilities => abilitySet.Abilities;
    public IReadOnlyList<string> Aliases => aliases.Aliases;
    public IAbilityResolver UpstreamAbilityResolver { get; private set; }
    public bool IsInitialized => isInitialized;
    public bool IsReady => isReady;
    
    readonly Identity identity;
    readonly List<Realm> children = new();
    readonly AbilitySet abilitySet;
    readonly AliasSet aliases = new();
    bool isInitialized;
    bool isReady;
    public void AddAlias(string alias)
    {
        aliases.Add(alias);
    }

    public bool RemoveAlias(string alias)
    {
        return aliases.Remove(alias);
    }

    public bool HasAlias(string alias)
    {
        return aliases.Has(alias);
    }

    public IEnumerable<Realm> FindByAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            yield break;
        }

        if (HasAlias(alias))
        {
            yield return this;
        }

        foreach (var child in children)
        {
            foreach (var match in child.FindByAlias(alias))
            {
                yield return match;
            }
        }
    }
    
    public Realm GetByAlias(string alias)
    {
        return FindByAlias(alias).FirstOrDefault();
    }
    public Realm()
    {
        identity = new Identity();
        abilitySet = new AbilitySet();
        AbilityAttributeInstaller.Apply(this, AddAbility);
    }

    // Explicit id injection removed: automatic-only policy

    public void AddChild(Realm realm)
    {
        if (realm == null)
        {
            throw new ArgumentNullException(nameof(realm));
        }

        children.Add(realm);
        realm.Initialize();
        realm.Ready();
    }

    public bool RemoveChild(Realm realm)
    {
        if (realm == null)
        {
            return false;
        }

        var removed = children.Remove(realm);
        if (removed)
        {
            realm.Uninitialize();
        }

        return removed;
    }

    public void AddAbility(Ability ability)
    {
        abilitySet.Add(ability);
        // 상위 Resolver를 신규 Ability에 전달
        ability.AttachUpstreamResolver(UpstreamAbilityResolver);
    }

    public bool RemoveAbility(Ability ability)
    {
        var removed = abilitySet.Remove(ability);
        if (removed)
        {
            ability.DetachUpstreamResolver();
        }
        return removed;
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

    protected virtual void OnInitialize()
    {
    }

    protected virtual void OnReady()
    {
    }

    protected virtual void OnUninitialize()
    {
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
