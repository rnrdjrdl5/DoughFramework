
using UnityEngine;

// PanelType 을 고려한 Order 재설정
public class PanelOrderByTypeTrait : Trait
{
    // TODO : Scriptable Object에서 관리되어야 한다.
    static int HUDOrder = 0;
    static int PanelOrder = 1000;
    static int PopupOrder = 2000;
    static int OverlayOrder = 5000;
    
    [SerializeField] PanelTrait panelTrait;

    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

        InitializeEvent();
    }

    public override void Uninitialize()
    {
        UninitializeEvent();
        
        base.Uninitialize();
    }

    void InitializeEvent()
    {
        if (panelTrait == null)
        {
            return;
        }

        panelTrait.OnAddPanel += OnAddPanel;
    }

    void UninitializeEvent()
    {
        if (panelTrait == null)
        {
            return;
        }

        panelTrait.OnAddPanel -= OnAddPanel;
    }

    void OnAddPanel(Panel panel)
    {
        if (panel is not IPanelOrderType panelOrderType)
        {
            return;
        }
        
        var offset = GetPanelOrderByType(panelOrderType);
        panel.SetPanelOrderOffset(offset);
    }

    int GetPanelOrderByType(IPanelOrderType panelOrderType)
    {
        return panelOrderType.PanelOrderType switch
        {
            PanelOrderType.HUD => HUDOrder,
            PanelOrderType.Popup => PopupOrder,
            PanelOrderType.Panel => PanelOrder,
            PanelOrderType.Overlay => OverlayOrder,
            _ => 0
        };
    }
}