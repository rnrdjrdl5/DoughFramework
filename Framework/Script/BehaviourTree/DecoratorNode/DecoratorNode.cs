public class DecoratorNode : BaseNode
{
    BaseNode wrapperNode;

    public void SetWrapperNode(BaseNode wrapperNode)
    {
        this.wrapperNode = wrapperNode;
    }

    public override BTNodeState OnUpdateNode()
    {
        if (wrapperNode == null)
        {
            return BTNodeState.Fail;
        }
        
        var result = wrapperNode.Update();
        return result; 
    }
}