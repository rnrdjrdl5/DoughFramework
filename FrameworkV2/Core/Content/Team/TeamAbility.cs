using System;
using System.Collections.Generic;

// 팀 로스터와 이벤트를 관리하는 Ability
public sealed class TeamAbility : Ability
{
    public event Action<TeamMemberChangedPayload> MemberChanged;

    readonly Dictionary<string, List<string>> membersByTeamId = new();
    readonly Dictionary<string, string> teamByMemberId = new();

    // 팀 멤버를 추가한다
    public bool TryAddMember(string teamId, string memberId, TeamMemberAbility memberAbility, string reason = null)
    {
        if (string.IsNullOrWhiteSpace(teamId) || string.IsNullOrWhiteSpace(memberId) || memberAbility == null)
        {
            return false;
        }

        if (teamByMemberId.TryGetValue(memberId, out var existingTeamId))
        {
            if (existingTeamId == teamId)
            {
                return true;
            }

            return TryMoveMember(memberId, teamId, memberAbility, reason);
        }

        if (!membersByTeamId.TryGetValue(teamId, out var list))
        {
            list = new List<string>();
            membersByTeamId[teamId] = list;
        }

        if (!list.Contains(memberId))
        {
            list.Add(memberId);
        }

        teamByMemberId[memberId] = teamId;
        memberAbility.ApplyFromTeamAbility(teamId);
        EmitChanged(teamId, memberId, null, reason);
        return true;
    }

    // 팀 멤버를 제거한다
    public bool TryRemoveMember(string memberId, TeamMemberAbility memberAbility = null, string reason = null)
    {
        if (string.IsNullOrWhiteSpace(memberId))
        {
            return false;
        }

        if (!teamByMemberId.TryGetValue(memberId, out var teamId))
        {
            return false;
        }

        if (membersByTeamId.TryGetValue(teamId, out var list))
        {
            list.Remove(memberId);
            if (list.Count == 0)
            {
                membersByTeamId.Remove(teamId);
            }
        }

        teamByMemberId.Remove(memberId);

        if (memberAbility != null)
        {
            memberAbility.ClearFromTeamAbility();
        }

        EmitChanged(null, memberId, teamId, reason);
        return true;
    }

    // 팀 멤버를 다른 팀으로 이동한다
    public bool TryMoveMember(string memberId, string newTeamId, TeamMemberAbility memberAbility, string reason = null)
    {
        if (string.IsNullOrWhiteSpace(memberId) || string.IsNullOrWhiteSpace(newTeamId))
        {
            return false;
        }

        if (!teamByMemberId.TryGetValue(memberId, out var previousTeamId))
        {
            return false;
        }

        if (previousTeamId == newTeamId)
        {
            return true;
        }

        if (memberAbility == null)
        {
            return false;
        }

        if (membersByTeamId.TryGetValue(previousTeamId, out var prevList))
        {
            prevList.Remove(memberId);
            if (prevList.Count == 0)
            {
                membersByTeamId.Remove(previousTeamId);
            }
        }

        if (!membersByTeamId.TryGetValue(newTeamId, out var newList))
        {
            newList = new List<string>();
            membersByTeamId[newTeamId] = newList;
        }

        if (!newList.Contains(memberId))
        {
            newList.Add(memberId);
        }

        teamByMemberId[memberId] = newTeamId;
        if (memberAbility != null)
        {
            memberAbility.ApplyFromTeamAbility(newTeamId);
        }
        EmitChanged(newTeamId, memberId, previousTeamId, reason);
        return true;
    }

    // 엔티티의 팀을 조회한다
    public bool TryGetTeamId(string memberId, out string teamId)
    {
        teamId = null;
        if (string.IsNullOrWhiteSpace(memberId))
        {
            return false;
        }

        return teamByMemberId.TryGetValue(memberId, out teamId);
    }

    // 팀 멤버 목록을 조회한다
    public IReadOnlyList<string> GetMembers(string teamId)
    {
        if (string.IsNullOrWhiteSpace(teamId))
        {
            return Array.Empty<string>();
        }

        return membersByTeamId.TryGetValue(teamId, out var list)
            ? list
            : Array.Empty<string>();
    }

    // 팀 멤버 변경 이벤트를 발행한다
    void EmitChanged(string teamId, string memberId, string previousTeamId, string reason)
    {
        try
        {
            MemberChanged?.Invoke(new TeamMemberChangedPayload(teamId, memberId, previousTeamId, reason));
        }
        catch
        {
        }
    }
}
