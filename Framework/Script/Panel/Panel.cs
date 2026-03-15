using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Panel : Entity
{
    public MessageBus TargetMessageBus => targetMessageBus;
    public Canvas Canvas => canvas;
    public virtual int PanelCustomOffset { get; protected set; }
    public int PanelOrder => panelOrder + panelOrderOffset + PanelCustomOffset;
    public int PanelOrderOffset => panelOrderOffset;
    
    [SerializeField] List<PanelElement> panelElements;
    [SerializeField] Canvas canvas;

    DataSet targetDataSet = new();
    MessageBus targetMessageBus;
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
        UnsetTargetMessageBus();
        UninitPanelElements();
        
        base.Uninitialize();
    }
    
    void UnsetTargetPanelDatas()
    {
        targetDataSet.TryUnsetTargetDatas();
        
        foreach (var element in panelElements)
        {
            element.UnsetTargetPanelDatas();
        }
    }
    
    void UnsetTargetMessageBus()
    {
        foreach (var panelElement in panelElements)
        {
            panelElement.UnsetTargetMessageBus();
        }
        
        targetMessageBus = null;
    }

    void UninitPanelElements()
    {
        foreach (var element in panelElements)
        {
            element.Uninitialize();
        }
    }

    public void SetTargetData(Entity entity, MessageBus targetMessageBus)
    {
        UnsetTargetPanelDatas();
        SetTargetPanelDatas(entity.ToData());
        
        UnsetTargetMessageBus();
        SetTargetMessageBus(targetMessageBus);
    }
    
    public void SetTargetPanelDatas(IEnumerable<IData> elements)
    {
        targetDataSet.SetTargetDatas(elements);
        
        foreach (var element in panelElements)
        {
            element.SetTargetPanelDatas(elements);
        }
    }
    
    public void SetTargetMessageBus(MessageBus targetMessageBus)
    {
        this.targetMessageBus = targetMessageBus;

        foreach (var panelElement in panelElements)
        {
            panelElement.SetTargetMessageBus(targetMessageBus);
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
