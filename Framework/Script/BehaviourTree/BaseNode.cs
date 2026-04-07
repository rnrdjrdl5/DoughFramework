using System.Collections.Generic;

public class BaseNode
{
    protected DataSet dataSet = new DataSet();

    BTNodeState nodeState = BTNodeState.None;
    
    public BTNodeState Update()
    {
        if (nodeState == BTNodeState.None)
        {
            nodeState = BTNodeState.Running;
            OnEnterNode();
        }

        var result = OnUpdateNode();
        if (result != BTNodeState.Running)
        {
            OnExitNode();
            nodeState = BTNodeState.None;
        }

        return result;
    }
    
    public virtual void OnEnterNode()
    {
    }

    public virtual void OnExitNode()
    {
    }

    public virtual BTNodeState OnUpdateNode()
    {
        return BTNodeState.Running;
    }

    public virtual void Reset()
    {
        if (nodeState == BTNodeState.Running)
        {
            OnExitNode();
        }

        nodeState = BTNodeState.None;
    }
    
    public void SetTargetData(IEnumerable<IData> datas)
    {
        dataSet.SetTargetDatas(datas);
    }

    protected internal virtual void SetDataSet(DataSet dataSet)
    {
        this.dataSet = dataSet ?? new DataSet();
    }
}
