public abstract class InputCommandLayerProcessor<TCommand> : BaseInputLayerProcessor
{
    protected abstract IInputCommandMapper<TCommand> Mapper { get; }
    protected abstract IInputCommandDispatcher<TCommand> Dispatcher { get; }

    public override LayerResult ProcessInput(InputContext input)
    {
        if (Mapper == null || Dispatcher == null)
        {
            return LayerResult.Pass;
        }

        return Mapper.TryMap(input, out var command)
            ? Dispatcher.Dispatch(command)
            : LayerResult.Pass;
    }
}
