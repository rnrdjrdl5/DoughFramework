using UnityEngine;

// Ability 수명과 Resolver 연결을 담당하는 기본 객체
public abstract class Ability : ILifecycle
{
    public bool IsInitialized => isInitialized;
    public bool IsReady => isReady;
    public GameObject Owner => owner;

    protected IAbilityResolver AbilityResolver { get; private set; }
    protected IAbilityResolver UpstreamAbilityResolver { get; private set; }

    GameObject owner;
    bool isInitialized;
    bool isReady;

    // Ability 초기화를 수행합니다.
    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }
        OnInitialize();
        isInitialized = true;
    }

    // Ability 준비 단계를 수행합니다.
    public void Ready()
    {
        if (!isInitialized || isReady)
        {
            return;
        }
        OnReady();
        isReady = true;
    }

    // Ability 종료 단계를 수행합니다.
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

    // Ability 초기화 훅을 제공합니다.
    protected virtual void OnInitialize() { }

    // Ability 준비 훅을 제공합니다.
    protected virtual void OnReady() { }

    // Ability 종료 훅을 제공합니다.
    protected virtual void OnUninitialize() { }

    // Ability 소유자 GameObject를 연결합니다.
    internal void AttachOwner(GameObject owner)
    {
        this.owner = owner;
    }

    // Ability 소유자 GameObject를 해제합니다.
    internal void DetachOwner()
    {
        owner = null;
    }

    // Ability의 로컬 Resolver를 연결합니다.
    internal void AttachResolver(IAbilityResolver resolver)
    {
        AbilityResolver = resolver;
    }

    // Ability의 로컬 Resolver 연결을 해제합니다.
    internal void DetachResolver()
    {
        AbilityResolver = null;
    }

    // Ability의 상위 Resolver를 연결합니다.
    internal void AttachUpstreamResolver(IAbilityResolver resolver)
    {
        UpstreamAbilityResolver = resolver;
    }

    // Ability의 상위 Resolver 연결을 해제합니다.
    internal void DetachUpstreamResolver()
    {
        UpstreamAbilityResolver = null;
    }

    // 상위 Ability 존재 여부를 조회합니다.
    protected bool HasUpstreamAbility<T>() where T : Ability
    {
        return UpstreamAbilityResolver != null && UpstreamAbilityResolver.HasAbility<T>();
    }

    // 상위 Ability를 조회합니다.
    protected T GetUpstreamAbility<T>() where T : Ability
    {
        return UpstreamAbilityResolver != null ? UpstreamAbilityResolver.GetAbility<T>() : null;
    }

    // 소유자 컴포넌트를 조회합니다.
    protected T GetOwnerComponent<T>() where T : Component
    {
        return owner != null ? owner.GetComponent<T>() : null;
    }
}
