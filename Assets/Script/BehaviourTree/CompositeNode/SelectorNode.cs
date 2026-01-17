public class SelectorNode : CompositeNode
{
    public override BTNodeState OnUpdateNode()
    {
        foreach (var childNode in childrenNode)
        {
            var result = childNode.Update();
            switch (result)
            {
                case BTNodeState.Success:
                    return BTNodeState.Success;
                case BTNodeState.Running:
                    return BTNodeState.Running;
            }
        }

        return BTNodeState.Fail;
    }
}