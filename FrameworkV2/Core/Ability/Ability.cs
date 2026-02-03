using System;

public abstract class Ability : ILifecycle
{
    public bool IsInitialized => isInitialized;
    public bool IsReady => isReady;

    protected IAbilityResolver AbilityResolver { get; private set; }
    protected IAbilityResolver UpstreamAbilityResolver { get; private set; }

    bool isInitialized;
    bool isReady;

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

    internal void AttachResolver(IAbilityResolver resolver)
    {
        AbilityResolver = resolver;
    }

    internal void DetachResolver()
    {
        AbilityResolver = null;
    }

    internal void AttachUpstreamResolver(IAbilityResolver resolver)
    {
        UpstreamAbilityResolver = resolver;
    }

    internal void DetachUpstreamResolver()
    {
        UpstreamAbilityResolver = null;
    }

    protected bool HasUpstreamAbility<T>() where T : Ability
    {
        return UpstreamAbilityResolver != null && UpstreamAbilityResolver.HasAbility<T>();
    }

    protected T GetUpstreamAbility<T>() where T : Ability
    {
        return UpstreamAbilityResolver != null ? UpstreamAbilityResolver.GetAbility<T>() : null;
    }
}
