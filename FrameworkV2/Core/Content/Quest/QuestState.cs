using System.Collections.Generic;

// 퀘스트 진행 상태(목표 진행 포함)
public sealed class QuestState
{
    public QuestDefinition Definition { get; }
    public QuestStatus Status { get; private set; }
    public IReadOnlyList<QuestObjectiveProgress> Objectives => objectives;

    readonly List<QuestObjectiveProgress> objectives = new();

    // 정의를 기준으로 목표 상태를 생성한다
    public QuestState(QuestDefinition definition)
    {
        Definition = definition;
        Status = QuestStatus.Locked;
        if (definition?.Objectives != null)
        {
            for (int i = 0; i < definition.Objectives.Count; i++)
            {
                var obj = definition.Objectives[i];
                objectives.Add(new QuestObjectiveProgress(obj.Key, obj.Target));
            }
        }
    }

    // 퀘스트를 활성화 상태로 전환한다
    public void Activate()
    {
        Status = QuestStatus.Active;
    }

    // 퀘스트를 완료 상태로 전환한다
    public void Complete()
    {
        Status = QuestStatus.Completed;
    }

    // 모든 목표가 완료되었는지 확인한다
    public bool IsCompleted()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (!objectives[i].IsCompleted)
            {
                return false;
            }
        }
        return true;
    }

    // 순차 진행용: 첫 번째 미완료 목표를 반환한다
    public QuestObjectiveProgress GetFirstIncompleteObjective()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            var obj = objectives[i];
            if (!obj.IsCompleted)
            {
                return obj;
            }
        }
        return null;
    }

    // 목표 키가 일치하는 목표 상태를 순회한다
    public IEnumerable<QuestObjectiveProgress> GetMatchingObjectives(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            yield break;
        }

        for (int i = 0; i < objectives.Count; i++)
        {
            var obj = objectives[i];
            if (obj.Key == key)
            {
                yield return obj;
            }
        }
    }
}
