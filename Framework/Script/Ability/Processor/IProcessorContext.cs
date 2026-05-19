public interface IProcessorContext
{
    Entity Entity { get; }
    bool IsValid { get; }

    bool TryInitialize(Entity entity);
    void Reset();
}
