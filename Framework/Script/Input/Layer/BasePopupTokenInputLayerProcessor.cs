public abstract class BasePopupTokenInputLayerProcessor : BaseTokenInputLayerProcessor
{
    public override LayerResult ProcessInput(TokenInputContext input)
    {
        if (input.RawContext.InputType == RawInputType.Started && CanConsume(input.InputType))
        {
            ClosePopup();
            return LayerResult.Consume;
        }

        return LayerResult.Block;
    }

    protected abstract bool CanConsume(TokenInputType inputType);

    void ClosePopup()
    {
        (Entity as Panel)?.Close();
    }
}
