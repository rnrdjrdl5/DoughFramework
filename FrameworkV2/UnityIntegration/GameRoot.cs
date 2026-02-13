using UnityEngine;

// 서비스 Ability와 루트 Realm 초기화를 담당하는 진입점
[DefaultExecutionOrder(-10000)]
public class GameRoot : MonoBehaviour
{
    public Realm RootRealm { get; private set; }

    [Header("Root Abilities")]
    [SerializeField] bool attachDefaultServiceAbilities = true;

    // GameRoot의 초기화를 수행합니다.
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        AbilityAttributeCache.Clear();

        RootRealm = BuildRealmTree();
        if (RootRealm != null)
        {
            if (attachDefaultServiceAbilities)
            {
                AttachRootAbilities(RootRealm);
            }

            RootRealm.Initialize();
            RootRealm.Ready();
        }
    }

    // 루트 Realm 트리를 구성합니다.
    protected virtual Realm BuildRealmTree()
    {
        var rootObject = new GameObject("RootRealm");
        rootObject.transform.SetParent(transform, false);
        return rootObject.AddComponent<Realm>();
    }

    // RootRealm에 기본 Ability들을 부착합니다.
    void AttachRootAbilities(Realm root)
    {
        if (root == null) return;

        root.AddAbility<UIAbility>();
        root.AddAbility<ObjectPoolAbility>();
        root.AddAbility<SpawnAbility>();
        root.AddAbility<InputAbility>();
    }

    void Update()
    {
        RootRealm?.TickTree();
    }
}
