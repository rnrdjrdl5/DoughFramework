using System.Collections.Generic;

public class CompositeNode : BaseNode
{
    protected List<BaseNode> childrenNode = new();

    public void AddChildNode(BaseNode childNode)
    {
        if (childNode == null)
        {
            return;
        }

        childNode.SetDataSet(dataSet);
        childrenNode.Add(childNode);
    }

    protected internal override void SetDataSet(DataSet dataSet)
    {
        base.SetDataSet(dataSet);
        foreach (var childNode in childrenNode)
        {
            childNode.SetDataSet(this.dataSet);
        }
    }

    public override void Reset()
    {
        base.Reset();
        foreach (var childNode in childrenNode)
        {
            childNode.Reset();
        }
    }
}
