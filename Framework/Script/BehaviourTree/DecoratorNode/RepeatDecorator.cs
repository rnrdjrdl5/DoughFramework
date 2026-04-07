public class RepeatDecorator : DecoratorNode
{
    int maxRepeatCount;
    int repeatCount;

    public void SetRepeatCount(int maxRepeatCount)
    {
        this.repeatCount = 0;
        this.maxRepeatCount = maxRepeatCount;
    }
    
    public override BTNodeState OnUpdateNode()
    {
        if (maxRepeatCount <= 0)
        {
            return BTNodeState.Fail;
        }

        if (repeatCount >= maxRepeatCount)
        {
            return BTNodeState.Success;
        }

        var result = base.OnUpdateNode();
        if (result == BTNodeState.Success)
        {
            repeatCount++;
            return repeatCount >= maxRepeatCount ? BTNodeState.Success : BTNodeState.Running;
        }

        return result;
    }

    public override void OnEnterNode()
    {
        base.OnEnterNode();
        
        repeatCount = 0;
    }
}
