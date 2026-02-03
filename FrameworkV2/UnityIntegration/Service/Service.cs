using UnityEngine;

public abstract class Service : MonoBehaviour, IService
{
    bool isInitialized;
    bool isReady;
    IServiceResolver services;

    public bool IsInitialized => isInitialized;
    public bool IsReady => isReady;
    protected IServiceResolver Services => services;

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

    public void AttachService(IServiceResolver resolver)
    {
        services = resolver;
    }

    public void DetachService()
    {
        services = null;
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

    protected virtual void OnDestroy()
    {
        // 보장: 파괴 시 정리. 중복 호출은 가드됨.
        Uninitialize();
        services = null;
    }
}
