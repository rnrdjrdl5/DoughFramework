using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelElement : MonoBehaviour
{
    public MessageBus TargetMessageBus => targetMessageBus;

    DataSet targetDataSet = new();
    MessageBus targetMessageBus;

    public virtual void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
    }

    public virtual void Uninitialize()
    {
        
    }

    public void SetTargetMessageBus(MessageBus messageBus)
    {
        this.targetMessageBus = messageBus;
    }

    public void UnsetTargetMessageBus()
    {
        targetMessageBus = null;
    }
    
    public void SetTargetPanelDatas(IEnumerable<IData> datas)
    {
        targetDataSet.TryUnsetTargetDatas();
        targetDataSet.SetTargetDatas(datas);

        OnSetPanelDatas();
    }

    public void UnsetTargetPanelDatas()
    {
        targetDataSet.TryUnsetTargetDatas();
        
        OnUnsetPanelDatas();
    }
    
    protected virtual void OnSetPanelDatas()
    {
        
    }

    protected virtual void OnUnsetPanelDatas()
    {
        
    }
    
    public TElement GetTargetPanelDatas<TElement>() where TElement : class, IData
    {
        return targetDataSet.GetTargetDatas<TElement>();
    }
    
    public virtual void RefreshUI()
    {
        
    }
}
