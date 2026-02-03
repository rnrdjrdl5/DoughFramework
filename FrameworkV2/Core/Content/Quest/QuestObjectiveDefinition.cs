// 퀘스트 목표 정의(키 + 목표치)
public sealed class QuestObjectiveDefinition
{
    public string Key { get; }
    public int Target { get; }

    // 목표 키와 목표치를 설정한다
    public QuestObjectiveDefinition(string key, int target)
    {
        Key = key;
        Target = target;
    }
}
