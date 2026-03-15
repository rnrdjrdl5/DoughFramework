using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelElement : MonoBehaviour
{
    public MessageBus ExternalMessageBus => externalMessageBus;
    public Panel Panel => panel;

    Panel panel;
    DataSet targetDataSet = new();
    MessageBus externalMessageBus;

    public virtual void Initialize(Panel panel, IInitData initData = null)
    {
        this.panel = panel;
        initData ??= EmptyInitData.Instance;
    }

    public virtual void Uninitialize()
    {
        
    }

    public void SetExternalMessageBus(MessageBus messageBus)
    {
        this.externalMessageBus = messageBus;
    }

    public void UnsetExternalMessageBus()
    {
        externalMessageBus = null;
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
