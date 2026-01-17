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
        if (repeatCount >= maxRepeatCount)
        {
            return BTNodeState.Fail;
        }
        
        repeatCount++;
        
        return base.OnUpdateNode();
    }

    public override void OnEnterNode()
    {
        base.OnEnterNode();
        
        repeatCount = 0;
    }
}