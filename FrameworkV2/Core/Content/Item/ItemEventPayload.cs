// 아이템 수량 변경 이벤트 데이터
public readonly struct ItemEventPayload
{
    public int ItemId => itemId;
    public int Previous => previous;
    public int Delta => delta;
    public int Current => current;
    public string Reason => reason;

    readonly int itemId;
    readonly int previous;
    readonly int delta;
    readonly int current;
    readonly string reason;

    // 아이템 수량 변경 이벤트 데이터를 만든다
    public ItemEventPayload(int itemId, int previous, int delta, int current, string reason)
    {
        this.itemId = itemId;
        this.previous = previous;
        this.delta = delta;
        this.current = current;
        this.reason = reason;
    }
}
