public interface IRawInputRequester
{
    void RequestRawInput(RawInputContext rawInput);
}

public interface IRawInputRequestSource
{
    void SetRawInputRequester(IRawInputRequester rawInputRequester);
}
