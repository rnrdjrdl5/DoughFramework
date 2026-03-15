using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Panel : Entity
{
    public MessageBus ExternalMessageBus => externalMessageBus;
    public Canvas Canvas => canvas;
    public virtual int PanelCustomOffset { get; protected set; }
    public int PanelOrder => panelOrder + panelOrderOffset + PanelCustomOffset;
    public int PanelOrderOffset => panelOrderOffset;
    public event Action OnUnsetPanelDatas;
    public event Action OnSetPanelDatas;
    
    [SerializeField] List<PanelElement> panelElements;
    [SerializeField] Canvas canvas;
    
    DataSet targetDataSet = new();
    MessageBus externalMessageBus;
    PanelAbility parentPanelAbility;

    int panelOrder;
    int panelOrderOffset;

    public override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        base.Initialize(initData);
        
        InitPanelAbility();
        InitPanelElements(initData);
    }

    void InitPanelAbility()
    {
        parentPanelAbility = Parent.GetAbility<PanelAbility>();
    }

    void InitPanelElements(IInitData initData)
    {
        foreach (var element in panelElements)
        {
            element.Initialize(this, initData);
        }
    }

    public override void Uninitialize()
    {
        UnsetTargetPanelDatas();
        UnsetExternalMessageBus();
        UninitPanelElements();
        
        base.Uninitialize();
    }
    
    void UnsetTargetPanelDatas()
    {
        targetDataSet.TryUnsetTargetDatas();
        UnsetPanelDatas();
        
        foreach (var element in panelElements)
        {
            element.UnsetTargetPanelDatas();
        }
    }

    protected virtual void UnsetPanelDatas()
    {
        OnUnsetPanelDatas?.Invoke();
    }
    
    void UnsetExternalMessageBus()
    {
        foreach (var panelElement in panelElements)
        {
            panelElement.UnsetExternalMessageBus();
        }
        
        externalMessageBus = null;
    }

    void UninitPanelElements()
    {
        foreach (var element in panelElements)
        {
            element.Uninitialize();
        }
    }

    public void SetTargetData(Entity entity, MessageBus externalMessageBus)
    {
        UnsetExternalMessageBus();
        SetExternalMessageBus(externalMessageBus);
        
        UnsetTargetPanelDatas();
        SetTargetPanelDatas(entity.ToData());
    }
    
    public void SetTargetPanelDatas(IEnumerable<IData> elements)
    {
        targetDataSet.SetTargetDatas(elements);
        SetPanelDatas();
        
        foreach (var element in panelElements)
        {
            element.SetTargetPanelDatas(elements);
        }
    }

    protected virtual void SetPanelDatas()
    {
        OnSetPanelDatas?.Invoke();
    }
    
    public void SetExternalMessageBus(MessageBus externalMessageBus)
    {
        this.externalMessageBus = externalMessageBus;

        foreach (var panelElement in panelElements)
        {
            panelElement.SetExternalMessageBus(externalMessageBus);
        }
    }

    public void SetPanelOrder(int order)
    {
        panelOrder = order;
        
        RefreshCanvasOrder();
    }
    public void SetPanelOrderOffset(int offset)
    {
        panelOrderOffset = offset;
        
        RefreshCanvasOrder();
    }

    public void RefreshCanvasOrder()
    {
        canvas.sortingOrder = PanelOrder;
    }
    
    public TElement GetPanelElement<TElement>() where TElement : PanelElement
    {
        return panelElements.Where(element => typeof(TElement).IsAssignableFrom(element.GetType()))
            .Cast<TElement>()
            .FirstOrDefault();
    }
    
    public TElement GetTargetPanelDatas<TElement>() where TElement : class, IData
    {
        return targetDataSet.GetTargetDatas<TElement>();
    }
    
    public virtual void Close()
    {
        parentPanelAbility.RemovePanel(this);
    }
    
    public virtual void RefreshUI()
    {
        foreach (var element in panelElements)
        {
            element.RefreshUI();
        }
    }
}
