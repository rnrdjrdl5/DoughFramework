// 퀘스트 이벤트 전달용 페이로드
public sealed class QuestEventPayload
{
    public string QuestId { get; }
    public QuestStatus Status { get; }
    public string ObjectiveKey { get; }
    public int? Current { get; }
    public int? Target { get; }

    // 퀘스트 상태/목표 진행 정보로 페이로드를 구성한다
    public QuestEventPayload(string questId, QuestStatus status, string objectiveKey = null, int? current = null, int? target = null)
    {
        QuestId = questId;
        Status = status;
        ObjectiveKey = objectiveKey;
        Current = current;
        Target = target;
    }
}
