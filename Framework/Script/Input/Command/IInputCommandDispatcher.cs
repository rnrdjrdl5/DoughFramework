public interface IInputCommandDispatcher<TCommand>
{
    LayerResult Dispatch(TCommand command);
}
