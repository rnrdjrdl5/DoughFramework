using System.Collections.Generic;

// 퀘스트 정의(목표 목록/순차 여부/선행 퀘스트)
public sealed class QuestDefinition
{
    public string Id { get; }
    public IReadOnlyList<QuestObjectiveDefinition> Objectives { get; }
    public bool IsSequential { get; }
    public IReadOnlyList<string> PrerequisiteQuestIds { get; }

    // 퀘스트 정의를 구성한다
    public QuestDefinition(
        string id,
        IReadOnlyList<QuestObjectiveDefinition> objectives,
        bool isSequential = false,
        IReadOnlyList<string> prerequisiteQuestIds = null)
    {
        Id = id;
        Objectives = objectives ?? new List<QuestObjectiveDefinition>();
        IsSequential = isSequential;
        PrerequisiteQuestIds = prerequisiteQuestIds ?? new List<string>();
    }
}
