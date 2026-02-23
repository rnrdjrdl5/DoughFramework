using System.Collections.Generic;
using UnityEngine;

// Element들을 조립하고 Ability를 연결하는 컨텐츠 전용 조정자
public abstract class Director : MonoBehaviour, ILifecycle
{
    public Entity Host => host;
    public bool IsInitialized => isInitialized;
    public bool IsReady => isReady;

    protected IReadOnlyList<Element> Elements => elements;

    [SerializeField] bool autoLifecycle = true;
    [SerializeField] Entity hostOverride;

    readonly List<Element> elements = new();
    Entity host;
    bool isInitialized;
    bool isReady;

    protected virtual void Awake()
    {
        ResolveHost();
        BuildElements();
        WireElements();

        if (autoLifecycle)
        {
            Initialize();
        }
    }

    protected virtual void Start()
    {
        if (autoLifecycle)
        {
            Ready();
        }
    }

    protected virtual void OnDestroy()
    {
        if (autoLifecycle)
        {
            Uninitialize();
        }
    }

    // Element 구성을 정의합니다.
    protected abstract void BuildElements();

    // Element와 Ability 연결을 정의합니다.
    protected abstract void WireElements();

    // Element를 Director에 등록합니다.
    protected void AddElement(Element element)
    {
        if (element == null)
        {
            return;
        }

        elements.Add(element);
        element.Attach(this);
    }

    // Ability를 조회합니다.
    public T GetAbility<T>() where T : Ability
    {
        return host != null ? host.GetAbility<T>() : null;
    }

    // Ability 보유 여부를 조회합니다.
    public bool HasAbility<T>() where T : Ability
    {
        return host != null && host.HasAbility<T>();
    }

    // Director 수명 초기화를 수행합니다.
    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            elements[i]?.Initialize();
        }

        OnInitialize();
        isInitialized = true;
    }

    // Director 준비 단계를 수행합니다.
    public void Ready()
    {
        if (!isInitialized || isReady)
        {
            return;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            elements[i]?.Ready();
        }

        OnReady();
        isReady = true;
    }

    // Director 종료 단계를 수행합니다.
    public void Uninitialize()
    {
        if (!isInitialized)
        {
            return;
        }

        OnUninitialize();

        for (int i = 0; i < elements.Count; i++)
        {
            elements[i]?.Uninitialize();
        }

        isReady = false;
        isInitialized = false;
    }

    // Director 초기화 훅을 제공합니다.
    protected virtual void OnInitialize()
    {
    }

    // Director 준비 훅을 제공합니다.
    protected virtual void OnReady()
    {
    }

    // Director 종료 훅을 제공합니다.
    protected virtual void OnUninitialize()
    {
    }

    // Host(Entity/Realm)를 찾습니다.
    void ResolveHost()
    {
        host = hostOverride != null ? hostOverride : GetComponent<Entity>();
        if (host == null)
        {
            host = GetComponentInParent<Entity>();
        }

        if (host == null)
        {
            Debug.LogWarning("Director host(Entity) not found.", this);
        }
    }
}
