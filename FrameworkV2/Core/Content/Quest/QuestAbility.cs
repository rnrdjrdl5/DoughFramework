using System;
using System.Collections.Generic;

// 퀘스트 진행/완료를 관리하는 핵심 Ability
public sealed class QuestAbility : Ability
{
    readonly Dictionary<string, QuestState> questStates = new();
    readonly HashSet<string> completed = new();

    IQuestDefinitionProvider provider;

    // 퀘스트 정의 프로바이더를 주입한다
    public void Configure(IQuestDefinitionProvider provider)
    {
        this.provider = provider;
    }

    // 퀘스트 시작을 시도한다(선행 조건 검사 포함)
    public bool TryStartQuest(string questId)
    {
        if (string.IsNullOrWhiteSpace(questId))
        {
            return false;
        }

        if (IsQuestCompleted(questId))
        {
            return false;
        }

        if (questStates.TryGetValue(questId, out var existing))
        {
            if (existing.Status == QuestStatus.Active)
            {
                return false;
            }
            if (existing.Status == QuestStatus.Completed)
            {
                return false;
            }
        }

        if (provider == null || !provider.TryGetDefinition(questId, out var definition))
        {
            return false;
        }

        if (!ArePrerequisitesCompleted(definition))
        {
            return false;
        }

        var state = new QuestState(definition);
        state.Activate();
        questStates[questId] = state;

        EmitEvent(QuestEvents.QuestStarted, new QuestEventPayload(questId, state.Status));
        EmitEvent(QuestEvents.QuestUpdated, new QuestEventPayload(questId, state.Status));

        return true;
    }

    // 퀘스트가 진행 중인지 확인한다
    public bool IsQuestActive(string questId)
    {
        if (string.IsNullOrWhiteSpace(questId))
        {
            return false;
        }

        return questStates.TryGetValue(questId, out var state) && state.Status == QuestStatus.Active;
    }

    // 퀘스트가 완료 상태인지 확인한다
    public bool IsQuestCompleted(string questId)
    {
        if (string.IsNullOrWhiteSpace(questId))
        {
            return false;
        }

        if (completed.Contains(questId))
        {
            return true;
        }

        return questStates.TryGetValue(questId, out var state) && state.Status == QuestStatus.Completed;
    }

    // 퀘스트 상태를 반환한다
    public QuestStatus GetQuestStatus(string questId)
    {
        if (IsQuestCompleted(questId))
        {
            return QuestStatus.Completed;
        }

        if (IsQuestActive(questId))
        {
            return QuestStatus.Active;
        }

        return QuestStatus.Locked;
    }

    // 활성화된 퀘스트 목록을 반환한다
    public IReadOnlyList<QuestState> GetActiveQuests()
    {
        var list = new List<QuestState>();
        foreach (var state in questStates.Values)
        {
            if (state.Status == QuestStatus.Active)
            {
                list.Add(state);
            }
        }
        return list;
    }

    // 목표 키에 대한 진행도를 보고한다
    public void ReportProgress(string objectiveKey, int amount = 1)
    {
        if (string.IsNullOrWhiteSpace(objectiveKey) || amount <= 0)
        {
            return;
        }

        foreach (var pair in questStates)
        {
            var questId = pair.Key;
            var state = pair.Value;

            if (state.Status != QuestStatus.Active)
            {
                continue;
            }

            var definition = state.Definition;
            if (definition == null)
            {
                continue;
            }

            if (definition.IsSequential)
            {
                var currentObjective = state.GetFirstIncompleteObjective();
                if (currentObjective == null || currentObjective.Key != objectiveKey)
                {
                    continue;
                }

                if (currentObjective.TryAddProgress(amount))
                {
                    EmitEvent(QuestEvents.ObjectiveProgressed,
                        new QuestEventPayload(questId, state.Status, currentObjective.Key, currentObjective.Current, currentObjective.Target));

                    TryCompleteQuestIfReady(questId, state);
                }
                continue;
            }

            var progressed = false;
            foreach (var obj in state.GetMatchingObjectives(objectiveKey))
            {
                if (obj.TryAddProgress(amount))
                {
                    progressed = true;
                    EmitEvent(QuestEvents.ObjectiveProgressed,
                        new QuestEventPayload(questId, state.Status, obj.Key, obj.Current, obj.Target));
                }
            }

            if (progressed)
            {
                TryCompleteQuestIfReady(questId, state);
            }
        }
    }

    // 완료 조건 충족 시 퀘스트를 완료 처리한다
    void TryCompleteQuestIfReady(string questId, QuestState state)
    {
        if (state.IsCompleted())
        {
            state.Complete();
            completed.Add(questId);
            EmitEvent(QuestEvents.QuestCompleted, new QuestEventPayload(questId, state.Status));
        }
        EmitEvent(QuestEvents.QuestUpdated, new QuestEventPayload(questId, state.Status));
    }

    // 선행 퀘스트 완료 여부를 확인한다
    bool ArePrerequisitesCompleted(QuestDefinition definition)
    {
        if (definition == null || definition.PrerequisiteQuestIds == null || definition.PrerequisiteQuestIds.Count == 0)
        {
            return true;
        }

        for (int i = 0; i < definition.PrerequisiteQuestIds.Count; i++)
        {
            var prereq = definition.PrerequisiteQuestIds[i];
            if (!IsQuestCompleted(prereq))
            {
                return false;
            }
        }

        return true;
    }

    // EventAbility가 있을 때만 이벤트를 발행한다
    void EmitEvent(int key, QuestEventPayload payload)
    {
        var events = AbilityResolver != null ? AbilityResolver.GetAbility<EventAbility>() : null;
        events?.Execute(key, payload);
    }
}
