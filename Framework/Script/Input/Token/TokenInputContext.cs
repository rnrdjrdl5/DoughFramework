using UnityEngine;

public readonly struct TokenInputContext
{
    public readonly TokenInputType InputType;
    public readonly RawInputContext RawContext;

    public TokenInputContext(TokenInputType inputType, RawInputContext rawContext)
    {
        InputType = inputType;
        RawContext = rawContext;
    }
}
