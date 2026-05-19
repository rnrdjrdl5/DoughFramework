public class BrainActionProcessor : Processor
{
    Brain brain;
    BrainActionContext actionContext;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        actionContext = new BrainActionContext();

        brain = Entity as Brain;
        if (brain == null)
        {
            return;
        }

        brain.OnAttachControll += RefreshControlledCache;
        brain.OnDetachControll += ClearControlledCache;
        RefreshControlledCache(brain.Controll);
    }

    public override void Ready()
    {
        base.Ready();
    }

    public override void Uninitialize()
    {
        if (brain != null)
        {
            brain.OnAttachControll -= RefreshControlledCache;
            brain.OnDetachControll -= ClearControlledCache;
        }

        ResetControlledCache();
        actionContext = null;

        base.Uninitialize();
    }

    public bool RequestAction<TAction>(TAction action)
        where TAction : struct, IBrainAction
    {
        return actionContext != null && action.Execute(actionContext);
    }

    void RefreshControlledCache(IControlled controlled)
    {
        actionContext?.TryInitialize(brain, controlled);
    }

    void ClearControlledCache(IControlled controlled)
    {
        if (actionContext == null || !actionContext.Matches(controlled))
        {
            return;
        }

        ResetControlledCache();
    }

    void ResetControlledCache()
    {
        actionContext?.Reset();
    }
}
