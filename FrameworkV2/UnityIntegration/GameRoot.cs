using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class GameRoot : MonoBehaviour
{
    public Transform ServicesRoot => servicesRoot; 
    public Realm RootRealm { get; private set; }
    public IServiceResolver Services { get; private set; }
    
    [SerializeField] Transform servicesRoot;
    ServiceSet serviceSet;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        AbilityCommandCache.Clear();
        PresenceOutputCommandCache.Clear();
        AbilityAttributeCache.Clear();

        serviceSet = new ServiceSet();
        Services = serviceSet;
        var root = ServicesRoot != null ? ServicesRoot : transform;
        serviceSet.SetResolver(Services);
        serviceSet.RegisterAllFrom(root);
        InitializeServices();

        RootRealm = BuildRealmTree();

        // 초기 Realm 트리를 스캔하여 RealmPresence를 생성
        if (RootRealm != null)
        {
            CreatePresenceTree(RootRealm);
        }
    }

    protected virtual Realm BuildRealmTree()
    {
        return new Realm();
    }

    void InitializeServices()
    {
        serviceSet.InitializeAll();
        serviceSet.ReadyAll();
    }

    void OnDestroy()
    {
        serviceSet?.UninitializeAll();
        serviceSet?.SetResolver(null);
        serviceSet?.ClearAll();
    }

    void CreatePresenceTree(Realm realm)
    {
        if (realm == null) return;
        RealmPresence.GetOrCreate(realm, Services);
        var children = realm.Children;
        if (children == null) return;
        for (int i = 0; i < children.Count; i++)
        {
            CreatePresenceTree(children[i]);
        }
    }

    void Update()
    {
        // 프레임마다 시간 스냅샷을 생성해 Realm 트리에 푸시
        var root = RootRealm;
        if (root == null) return;

        var snap = new TimeSnapshot(
            time: Time.time,
            deltaTime: Time.deltaTime,
            unscaledTime: Time.unscaledTime,
            unscaledDeltaTime: Time.unscaledDeltaTime,
            frame: Time.frameCount);

        PushClockRecursive(root, snap);
    }

    void PushClockRecursive(Realm realm, in TimeSnapshot snapshot)
    {
        if (realm == null) return;
        var clock = realm.GetAbility<ClockAbility>();
        if (clock != null)
        {
            clock.UpdateSnapshot(snapshot);
        }

        var children = realm.Children;
        if (children == null || children.Count == 0) return;
        for (int i = 0; i < children.Count; i++)
        {
            PushClockRecursive(children[i], snapshot);
        }
    }
}
