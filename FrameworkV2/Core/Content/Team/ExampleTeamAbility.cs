using System;

// TeamAbility 사용 예시를 보여주는 참고 스크립트
public static class ExampleTeamAbility
{
    // Example: Realm 생성 -> TeamAbility 부착 -> 팀 배정/이동/제거
    public static void RunExample()
    {
        var realm = new Realm();
        var teamAbility = new TeamAbility();
        realm.AddAbility(teamAbility);

        var entity = new WorldEntity();
        var memberAbility = new TeamMemberAbility();
        entity.AddAbility(memberAbility);

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
