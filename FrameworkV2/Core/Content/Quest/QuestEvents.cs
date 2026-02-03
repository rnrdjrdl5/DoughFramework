// 퀘스트 이벤트 키 정의(해시 기반)
public static class QuestEvents
{
    // 퀘스트 시작
    public static readonly int QuestStarted = HashKey.FromName("Quest.Started");
    // 퀘스트 상태 갱신
    public static readonly int QuestUpdated = HashKey.FromName("Quest.Updated");
    // 퀘스트 완료
    public static readonly int QuestCompleted = HashKey.FromName("Quest.Completed");
    // 목표 진행
    public static readonly int ObjectiveProgressed = HashKey.FromName("Quest.ObjectiveProgressed");
}
