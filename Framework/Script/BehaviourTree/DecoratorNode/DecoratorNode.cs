public class DecoratorNode : BaseNode
{
    BaseNode wrapperNode;

    public void SetWrapperNode(BaseNode wrapperNode)
    {
        this.wrapperNode = wrapperNode;
        this.wrapperNode?.SetDataSet(dataSet);
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

    protected internal override void SetDataSet(DataSet dataSet)
    {
        base.SetDataSet(dataSet);
        wrapperNode?.SetDataSet(this.dataSet);
    }

    public override void Reset()
    {
        base.Reset();
        wrapperNode?.Reset();
    }
}
