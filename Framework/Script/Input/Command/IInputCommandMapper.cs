public interface IInputCommandMapper<TCommand>
{
    bool TryMap(InputContext input, out TCommand command);
}
