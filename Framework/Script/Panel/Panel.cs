using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Panel : Actor
{
    public EventListener InteractionEvent => interactionEvent;
    public Canvas Canvas => canvas;
    public virtual int PanelCustomOffset { get; protected set; }
    public int PanelOrder => panelOrder + panelOrderOffset + PanelCustomOffset;
    public int PanelOrderOffset => panelOrderOffset;
    
    [SerializeField] List<PanelElement> panelElements;
    [SerializeField] Canvas canvas;
    
    EventListener interactionEvent;
    PanelAbility parentPanelAbility;

    int panelOrder;
    int panelOrderOffset;

    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        
        InitPanelAbility();
        InitPanelElements(parameter);
    }

    void InitPanelAbility()
    {
        parentPanelAbility = Parent.GetAbility<PanelAbility>();
    }

    void InitPanelElements(Parameter parameter)
    {
        foreach (var element in panelElements)
        {
            element.Initialize(parameter);
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

    public void SetPanelData(Actor actor, EventListener interactionEvent)
    {
        SetTargetPanelDatas(actor.ToData());
        SetInteractionEvent(interactionEvent);
    }
    
    public void SetInteractionEvent(EventListener interactionEvent)
    {
        this.interactionEvent = interactionEvent;

        foreach (var panelElement in panelElements)
        {
            panelElement.SetInteractionEvent(interactionEvent);
        }
    }

    public void UnsetInteractionEvent()
    {
        foreach (var panelElement in panelElements)
        {
            panelElement.UnsetInteractionEvent();
        }
        
        interactionEvent = null;
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
