public abstract class BasePopupInputLayerProcessor : BaseInputLayerProcessor
{
    public override LayerResult ProcessInput(PhysicalInputTokenEvent input)
    {
        if (CanConsume(input.TokenType))
        {
            ClosePopup();
            return LayerResult.Consume;
        }

        return LayerResult.Block;
    }

    protected abstract bool CanConsume(PhysicalInputTokenType tokenType);

    void ClosePopup()
    {
        (Entity as Panel)?.Close();
    }
}
