using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Panel : Entity
{
    public MessageBus MessageBus => messageBus;
    public Canvas Canvas => canvas;
    public virtual int PanelCustomOffset { get; protected set; }
    public int PanelOrder => panelOrder + panelOrderOffset + PanelCustomOffset;
    public int PanelOrderOffset => panelOrderOffset;
    
    [SerializeField] List<PanelElement> panelElements;
    [SerializeField] Canvas canvas;
    
    MessageBus messageBus;
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
            element.Initialize(initData);
        }
    }

    public override void Uninitialize()
    {
        base.Uninitialize();

        UninitPanelElements();
    }

    void UninitPanelElements()
    {
        foreach (var element in panelElements)
        {
            element.Uninitialize();
        }
    }

    public void SetPanelData(Entity entity, MessageBus messageBus)
    {
        SetTargetPanelDatas(entity.ToData());
        SetInteractionEvent(messageBus);
    }
    
    public void SetInteractionEvent(MessageBus messageBus)
    {
        this.messageBus = messageBus;

        foreach (var panelElement in panelElements)
        {
            panelElement.SetInteractionEvent(messageBus);
        }
    }

    public void UnsetInteractionEvent()
    {
        foreach (var panelElement in panelElements)
        {
            panelElement.UnsetInteractionEvent();
        }
        
        messageBus = null;
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
    
    public void SetTargetPanelDatas(IEnumerable<IData> elements)
    {
        foreach (var element in panelElements)
        {
            element.SetTargetPanelDatas(elements);
        }
    }
    
    public void UnsetTargetPanelDatas()
    {
        foreach (var element in panelElements)
        {
            element.UnsetTargetPanelDatas();
        }
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
