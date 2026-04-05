public interface IPhysicalInputTokenRequester
{
    void RequestTokenInput(PhysicalInputTokenEvent tokenInput);
}

public interface IPhysicalInputTokenRequestSource
{
    void SetTokenRequester(IPhysicalInputTokenRequester tokenRequester);
}
