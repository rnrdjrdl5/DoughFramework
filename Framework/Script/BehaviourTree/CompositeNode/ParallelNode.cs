
//병렬실행처리, 필요 시 구현
public class ParallelNode : CompositeNode
{
    public override BTNodeState OnUpdateNode()
    {
        var resultState = BTNodeState.Success;
        foreach (var childNode in childrenNode)
        {
            var result = childNode.Update();
            if (result == BTNodeState.Fail)
            {
                ResetChildren();
                return BTNodeState.Fail;
            }

            if (result == BTNodeState.Running)
            {
                resultState = BTNodeState.Running;
            }
        }

        return resultState;
    }

    void ResetChildren()
    {
        foreach (var childNode in childrenNode)
        {
            childNode.Reset();
        }
    }
}
