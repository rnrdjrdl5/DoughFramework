// 엔티티의 팀 정보를 보관하는 Ability
public sealed class TeamMemberAbility : Ability
{
    public string TeamId => teamId;

    string teamId;

    // 팀을 지정한다
    public bool AssignTeam(string teamId, string memberId, string reason = null)
    {
        if (string.IsNullOrWhiteSpace(teamId))
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(memberId))
        {
            return false;
        }

        if (HasUpstreamAbility<TeamAbility>())
        {
            var teamAbility = GetUpstreamAbility<TeamAbility>();
            return teamAbility != null && teamAbility.TryAddMember(teamId, memberId, this, reason);
        }

        this.teamId = teamId;
        return true;
    }

    // 팀을 해제한다
    public bool ClearTeam(string memberId, string reason = null)
    {
        if (string.IsNullOrWhiteSpace(memberId))
        {
            return false;
        }
        if (HasUpstreamAbility<TeamAbility>())
        {
            var teamAbility = GetUpstreamAbility<TeamAbility>();
            return teamAbility != null && teamAbility.TryRemoveMember(memberId, this, reason);
        }

        teamId = null;
        return true;
    }

    // TeamAbility에서 팀 정보를 반영한다
    internal void ApplyFromTeamAbility(string teamId)
    {
        this.teamId = teamId;
    }

    // TeamAbility에서 팀 정보를 해제한다
    internal void ClearFromTeamAbility()
    {
        teamId = null;
    }
}
