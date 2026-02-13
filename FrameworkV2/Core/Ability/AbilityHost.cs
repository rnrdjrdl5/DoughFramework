using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Ability를 보유하고 수명 주기를 제어하는 호스트 베이스
public abstract class AbilityHost : MonoBehaviour, ILifecycle, IAbilityResolver
{
    public IReadOnlyList<Ability> Abilities => abilities;
    public IAbilityResolver UpstreamAbilityResolver => upstreamAbilityResolver;
    public bool IsInitialized => isInitialized;
    public bool IsReady => isReady;

    readonly List<Ability> abilities = new();
    readonly List<IAbilityTick> tickables = new();
    IAbilityResolver upstreamAbilityResolver;
    bool isInitialized;
    bool isReady;

    // Host에 Ability를 추가하고 등록합니다.
    public void AddAbility(Ability ability)
    {
        if (ability == null)
        {
            throw new ArgumentNullException(nameof(ability));
        }

        if (abilities.Contains(ability))
        {
            return;
        }

        abilities.Add(ability);
        ability.AttachOwner(gameObject);
        ability.AttachResolver(this);
        ability.AttachUpstreamResolver(upstreamAbilityResolver);
        OnAbilityAdded(ability);

        if (isInitialized)
        {
            ability.Initialize();
            if (isReady)
            {
                ability.Ready();
            }
        }
    }

    // Host에 Ability를 생성하여 추가합니다.
    public T AddAbility<T>() where T : Ability, new()
    {
        var existing = abilities.OfType<T>().FirstOrDefault();
        if (existing != null)
        {
            return existing;
        }

        var created = new T();
        AddAbility(created);
        return created;
    }

    // Host에서 Ability를 제거하고 수명을 해제합니다.
    public bool RemoveAbility(Ability ability)
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

        if (ability is IAbilityTick tickable)
        {
            UnregisterTick(tickable);
        }

        OnAbilityRemoved(ability);
        ability.Uninitialize();
        ability.DetachResolver();
        ability.DetachUpstreamResolver();
        ability.DetachOwner();
        return true;
    }

    // Host가 보유한 Ability 여부를 조회합니다.
    public bool HasAbility<T>() where T : Ability
    {
        return abilities.Any(a => a is T);
    }

    // Host가 보유한 Ability를 반환합니다.
    public T GetAbility<T>() where T : Ability
    {
        return abilities.OfType<T>().FirstOrDefault();
    }

    // Tick 대상 Ability를 등록합니다.
    public void RegisterTick(IAbilityTick ability)
    {
        if (ability == null)
        {
            return;
        }

        if (tickables.Contains(ability))
        {
            return;
        }

        tickables.Add(ability);
    }

    // Tick 대상 Ability를 해제합니다.
    public void UnregisterTick(IAbilityTick ability)
    {
        if (ability == null)
        {
            return;
        }

        tickables.Remove(ability);
    }

    // Host 수명 초기화를 시작하고 Ability를 초기화합니다.
    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        RegisterAttributeAbilities();
        InitializeAbilities();
        OnInitialize();
        isInitialized = true;
    }

    // Host 수명 준비 단계를 호출하고 Ability를 준비합니다.
    public void Ready()
    {
        if (!isInitialized || isReady)
        {
            return;
        }

        ReadyAbilities();
        OnReady();
        isReady = true;
    }

    // Host 수명 종료를 처리하고 Ability를 정리합니다.
    public void Uninitialize()
    {
        if (!isInitialized)
        {
            return;
        }

        OnUninitialize();
        UninitializeAbilities();
        tickables.Clear();
        isReady = false;
        isInitialized = false;
    }

    // 매 프레임 호출이 필요한 Ability를 업데이트합니다.
    public void Tick()
    {
        for (int i = 0; i < tickables.Count; i++)
        {
            tickables[i]?.Tick();
        }
    }

    // 상위 Resolver를 전달합니다.
    internal void AttachUpstreamResolver(IAbilityResolver resolver)
    {
        upstreamAbilityResolver = resolver;
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i]?.AttachUpstreamResolver(resolver);
        }
    }

    // 상위 Resolver 연결을 해제합니다.
    internal void DetachUpstreamResolver()
    {
        upstreamAbilityResolver = null;
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i]?.DetachUpstreamResolver();
        }
    }

    // Ability가 추가될 때 확장 포인트를 제공합니다.
    protected virtual void OnAbilityAdded(Ability ability)
    {
    }

    // Ability가 제거될 때 확장 포인트를 제공합니다.
    protected virtual void OnAbilityRemoved(Ability ability)
    {
    }

    // Host 초기화 전 처리할 작업을 정의합니다.
    protected virtual void OnInitialize()
    {
    }

    // Host 준비 단계의 작업을 정의합니다.
    protected virtual void OnReady()
    {
    }

    // Host 종료 단계의 작업을 정의합니다.
    protected virtual void OnUninitialize()
    {
    }

    // AbilityAttribute 기반 Ability를 추가합니다.
    void RegisterAttributeAbilities()
    {
        AbilityAttributeInstaller.Apply(this, AddAbility);
    }

    // Ability들의 Initialize를 호출합니다.
    void InitializeAbilities()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i]?.Initialize();
        }
    }

    // Ability들의 Ready를 호출합니다.
    void ReadyAbilities()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i]?.Ready();
        }
    }

    // Ability들의 Uninitialize를 호출합니다.
    void UninitializeAbilities()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i]?.Uninitialize();
        }
    }
}
