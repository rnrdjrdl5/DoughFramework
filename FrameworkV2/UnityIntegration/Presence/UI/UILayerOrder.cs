public static class UILayerOrder
{
    public const int CoreBase = 0;
    public const int PanelBase = 1000;
    public const int PanelStep = 50;
    public const int ModalBase = 5000;
    public const int ModalStep = 50;
    public const int SystemBase = 20000;

    // overlayIndex는 Overlay/Modal에서만 의미가 있으며, 각 레이어별 스택 인덱스입니다.
    public static int Compute(UILayer layer, int overlayIndex = 0, int extraOffset = 0)
    {
        int order = layer switch
        {
            UILayer.Core => CoreBase,
            UILayer.Panel => PanelBase + (overlayIndex * PanelStep),
            UILayer.Modal => ModalBase + (overlayIndex * ModalStep),
            UILayer.System => SystemBase,
            _ => 0
        };
        return order + extraOffset;
    }
}
