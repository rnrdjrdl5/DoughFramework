public partial class ProcessorAbility
{
    IProcessorContext processorContext;
    bool isProcessorContextInitialized;

    public TContext GetContext<TContext>()
        where TContext : class, IProcessorContext, new()
    {
        if (processorContext == null)
        {
            processorContext = new TContext();

            if (isProcessorContextInitialized)
            {
                processorContext.TryInitialize(Entity);
            }
        }

        return processorContext as TContext;
    }

    public void InitializeProcessorContext()
    {
        processorContext?.TryInitialize(Entity);
        isProcessorContextInitialized = true;
    }

    void ResetProcessorContext()
    {
        processorContext?.Reset();
        processorContext = null;
        isProcessorContextInitialized = false;
    }
}
