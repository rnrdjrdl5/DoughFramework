public static class ProcessorContextExtensions
{
    public static TContext GetProcessorContext<TContext>(this Entity entity)
        where TContext : class, IProcessorContext, new()
    {
        return entity
            ?.GetAbility<ProcessorAbility>()
            ?.GetContext<TContext>();
    }
}
