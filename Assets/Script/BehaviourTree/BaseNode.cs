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
        nodeState = BTNodeState.Success;
    }

    public virtual void OnExitNode()
    {
        nodeState = BTNodeState.None;
    }

    public virtual BTNodeState OnUpdateNode()
    {
        return BTNodeState.Running;
    }
    
    public void SetTargetData(IEnumerable<IData> datas)
    {
        dataSet.SetTargetDatas(datas);
    }
}