public interface IProcessorContext
{
    Entity Entity { get; }

    bool TryInitialize(Entity entity);
    void Reset();
}
