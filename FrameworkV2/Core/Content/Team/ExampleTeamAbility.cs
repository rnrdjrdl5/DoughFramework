using System;
using UnityEngine;

// TeamAbility 사용 예시를 보여주는 참고 스크립트
public static class ExampleTeamAbility
{
    // Realm/Entity 생성 및 TeamAbility 동작을 예시로 보여줍니다.
    public static void RunExample()
    {
        var realmObject = new GameObject("ExampleRealm");
        var realm = realmObject.AddComponent<Realm>();
        var teamAbility = realm.AddAbility<TeamAbility>();

        var entityObject = new GameObject("ExampleEntity");
        entityObject.transform.SetParent(realmObject.transform, false);
        var entity = entityObject.AddComponent<Entity>();
        var memberAbility = entity.AddAbility<TeamMemberAbility>();

        realm.Initialize();
        realm.Ready();
        entity.Initialize();
        entity.Ready();

        teamAbility.MemberChanged += payload =>
        {
            // 팀 변경 이벤트를 수신한다
        };

        var memberId = entity.Id;
        teamAbility.TryAddMember("red", memberId, memberAbility, reason: "spawn");
        teamAbility.TryMoveMember(memberId, "blue", memberAbility, reason: "switch");
        teamAbility.TryRemoveMember(memberId, memberAbility, reason: "leave");

        var members = teamAbility.GetMembers("blue");
        Console.WriteLine($"blue members: {members.Count}");
    }
}
