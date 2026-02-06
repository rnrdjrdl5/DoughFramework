// 팀 멤버 변경 이벤트 데이터
public readonly struct TeamMemberChangedPayload
{
    public string TeamId => teamId;
    public string MemberId => memberId;
    public string PreviousTeamId => previousTeamId;
    public string Reason => reason;

    readonly string teamId;
    readonly string memberId;
    readonly string previousTeamId;
    readonly string reason;

    // 팀 멤버 변경 이벤트 데이터를 만든다
    public TeamMemberChangedPayload(string teamId, string memberId, string previousTeamId, string reason)
    {
        this.teamId = teamId;
        this.memberId = memberId;
        this.previousTeamId = previousTeamId;
        this.reason = reason;
    }
}
