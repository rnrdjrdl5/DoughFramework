using System.Collections.Generic;

public class CompositeNode : BaseNode
{
    protected List<BaseNode> childrenNode = new();

    public void AddChildNode(BaseNode childNode)
    {
        childrenNode.Add(childNode);
    }
}