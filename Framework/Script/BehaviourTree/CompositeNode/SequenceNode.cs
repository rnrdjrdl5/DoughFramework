public class SequenceNode : CompositeNode
{
    public override BTNodeState OnUpdateNode()
    {
        foreach (var childNode in childrenNode)
        {
            var result = childNode.Update();
            switch (result)
            {
                case BTNodeState.Fail:
                    return BTNodeState.Fail;
                case BTNodeState.Running:
                    return BTNodeState.Running;
            }
        }

        return BTNodeState.Success;
    }
}