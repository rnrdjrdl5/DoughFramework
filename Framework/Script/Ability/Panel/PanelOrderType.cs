public interface IPanelOrderType
{
    public PanelOrderType PanelOrderType { get; set; }
}

public enum PanelOrderType
{
    Panel,
    Popup,
    HUD, 
    Overlay
}