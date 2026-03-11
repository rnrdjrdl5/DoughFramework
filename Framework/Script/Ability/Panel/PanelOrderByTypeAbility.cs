
using UnityEngine;

// PanelType 을 고려한 Order 재설정
public class PanelOrderByTypeAbility : Ability
{
    // TODO : Scriptable Object에서 관리되어야 한다.
    static int HUDOrder = 0;
    static int PanelOrder = 1000;
    static int PopupOrder = 2000;
    static int OverlayOrder = 5000;
    
    [SerializeField] PanelAbility panelAbility;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        InitializeEvent();
    }

    public override void Uninitialize()
    {
        UninitializeEvent();
        
        base.Uninitialize();
    }

    void InitializeEvent()
    {
        if (panelAbility == null)
        {
            return;
        }

        panelAbility.OnAddPanel += OnAddPanel;
    }

    void UninitializeEvent()
    {
        if (panelAbility == null)
        {
            return;
        }

        panelAbility.OnAddPanel -= OnAddPanel;
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
