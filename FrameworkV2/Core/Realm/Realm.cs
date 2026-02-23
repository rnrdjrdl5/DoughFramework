using System;
using System.Collections.Generic;
using System.Linq;

// Realm 트리와 Ability를 보유하는 최상위 컨텍스트 컴포넌트
[Ability(typeof(BuildRealmAbility))]
[Ability(typeof(SpawnEntityAbility))]
public partial class Realm : Entity
{
    public IReadOnlyList<Realm> Children => children;

    readonly List<Realm> children = new();

    // 별칭으로 Realm을 검색합니다.
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

    // 별칭으로 단일 Realm을 조회합니다.
    public Realm GetByAlias(string alias)
    {
        return FindByAlias(alias).FirstOrDefault();
    }

    // 자식 Realm을 추가하고 부모-자식 관계를 설정합니다.
    public void AddChild(Realm realm)
    {
        if (realm == null)
        {
            throw new ArgumentNullException(nameof(realm));
        }

        if (realm.transform.parent != transform)
        {
            realm.transform.SetParent(transform, false);
        }

        if (!children.Contains(realm))
        {
            children.Add(realm);
        }

        realm.Initialize();
        realm.Ready();
        RefreshEntitiesIfNeeded();
    }

    // 자식 Realm을 제거하고 수명을 종료합니다.
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

        RefreshEntitiesIfNeeded();
        return removed;
    }

    // Realm 트리 전체를 Tick 처리합니다.
    public void TickTree()
    {
        Tick();

        var spawn = GetAbility<SpawnEntityAbility>();
        if (spawn != null)
        {
            var entities = spawn.Entities;
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i]?.Tick();
            }
        }

        for (int i = 0; i < children.Count; i++)
        {
            children[i]?.TickTree();
        }
    }

    // Realm 초기화 전 작업을 수행합니다.
    protected override void OnInitialize()
    {
        RefreshChildrenCache();
        RefreshEntitiesIfNeeded();
    }

    // 자식 Realm 캐시를 갱신합니다.
    void RefreshChildrenCache()
    {
        children.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var childRealm = child.GetComponent<Realm>();
            if (childRealm != null)
            {
                children.Add(childRealm);
            }
        }
    }

    // SpawnEntityAbility가 존재하면 Entity 목록을 갱신합니다.
    void RefreshEntitiesIfNeeded()
    {
        var spawn = GetAbility<SpawnEntityAbility>();
        if (spawn != null)
        {
            spawn.RefreshEntities();
        }
    }
}
