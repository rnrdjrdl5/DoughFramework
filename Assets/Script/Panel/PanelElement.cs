using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelElement : MonoBehaviour
{
    public EventListener InteractionEvent => interactionEvent;

    DataSet targetDataSet = new();
    EventListener interactionEvent;

    public virtual void Initialize(Parameter parameter)
    {
        
    }

    public virtual void Uninitialize()
    {
        
    }

    public void SetInteractionEvent(EventListener interactionEvent)
    {
        this.interactionEvent = interactionEvent;
    }

    public void UnsetInteractionEvent()
    {
        interactionEvent = null;
    }
    
    public void SetTargetPanelDatas(IEnumerable<IData> datas)
    {
        targetDataSet.SetTargetDatas(datas);

        OnSetPanelDatas();
    }

    public void UnsetTargetPanelDatas()
    {
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
