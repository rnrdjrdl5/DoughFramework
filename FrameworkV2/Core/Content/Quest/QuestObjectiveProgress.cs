// 퀘스트 목표 진행 상태
public sealed class QuestObjectiveProgress
{
    public string Key { get; }
    public int Current { get; private set; }
    public int Target { get; }
    public bool IsCompleted => Current >= Target;

    // 목표 키/목표치를 바탕으로 상태를 초기화한다
    public QuestObjectiveProgress(string key, int target)
    {
        Key = key;
        Target = target;
    }

    // 진행치를 증가시키고, 실제 변화가 있으면 true를 반환한다
    public bool TryAddProgress(int amount)
    {
        if (amount <= 0 || IsCompleted)
        {
            return false;
        }

        var next = Current + amount;
        if (next > Target)
        {
            next = Target;
        }

        if (next == Current)
        {
            return false;
        }

        Current = next;
        return true;
    }
}
