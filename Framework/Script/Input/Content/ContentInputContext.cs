public readonly struct ContentInputContext<TInputType>
    where TInputType : struct
{
    public readonly TInputType InputType;
    public readonly TokenInputContext TokenContext;

    public ContentInputContext(TInputType inputType, TokenInputContext tokenContext)
    {
        InputType = inputType;
        TokenContext = tokenContext;
    }
}
