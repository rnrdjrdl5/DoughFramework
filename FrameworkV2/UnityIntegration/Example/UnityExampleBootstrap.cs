using UnityEngine;

public sealed class UnityExampleBootstrap : MonoBehaviour
{
    [SerializeField] ExamplePlayerAvatar playerAvatar;

    WorldEntity entity;
    Realm realm;

    void Awake()
    {
        var root = FindObjectOfType<GameRoot>();
        if (root == null)
        {
            Debug.LogError("GameRoot not found in scene.");
            return;
        }
        realm = root.RootRealm;
        if (realm == null)
        {
            // 방어적 처리: 실행 순서 문제로 아직 초기화 전이면 새 Realm로 대체
            realm = new Realm();
            Debug.LogWarning("RootRealm is null on GameRoot. Created a temporary Realm.");
        }

        entity = new WorldEntity();
        entity.AddAbility(new OutputMovementAbility());
        entity.AddAbility(new OutputHealthAbility());
        var reg = realm.GetAbility<SpawnEntityAbility>();
        if (reg == null)
        {
            reg = new SpawnEntityAbility();
            realm.AddAbility(reg);
        }
        reg.AddWorldEntity(entity);

        if (playerAvatar == null)
        {
            playerAvatar = GetComponentInChildren<ExamplePlayerAvatar>();
        }
        if (playerAvatar != null)
        {
            playerAvatar.AttachService(root.Services);
            playerAvatar.BindEntity(entity);
            playerAvatar.Initialize();
            playerAvatar.Ready();
        }
        else
        {
            Debug.LogWarning("ExamplePlayerAvatar is not assigned.");
        }
    }

    void OnDestroy()
    {
        if (realm != null && entity != null)
        {
            var reg = realm.GetAbility<SpawnEntityAbility>();
            reg?.RemoveWorldEntity(entity);
        }
        entity = null;
        realm = null;
    }
}
