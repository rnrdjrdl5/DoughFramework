// 통화 잔액 변경 이벤트 데이터
public readonly struct EconomyEventPayload
{
    public int CurrencyId => currencyId;
    public int Previous => previous;
    public int Delta => delta;
    public int Current => current;
    public string Reason => reason;

    readonly int currencyId;
    readonly int previous;
    readonly int delta;
    readonly int current;
    readonly string reason;

    // 통화 잔액 변경 이벤트 데이터를 만든다
    public EconomyEventPayload(int currencyId, int previous, int delta, int current, string reason)
    {
        this.currencyId = currencyId;
        this.previous = previous;
        this.delta = delta;
        this.current = current;
        this.reason = reason;
    }
}
