public class PhysicalTokenRouterProcessor : Processor, IPhysicalInputTokenRequester
{
    PhysicalTokenEmitterAbility physicalTokenEmitterAbility;
    FrameworkInputProcessorAbility frameworkInputProcessorAbility;

    public override void Ready()
    {
        base.Ready();

        physicalTokenEmitterAbility = Entity.GetAbility<PhysicalTokenEmitterAbility>();
        if (physicalTokenEmitterAbility == null)
        {
            return;
        }
        
        physicalTokenEmitterAbility.SetTokenRequester(this);

        var physicalInputBindingData = Entity.GetEntityData<PhysicalInputBindingData>();
        physicalTokenEmitterAbility.SetInputBindingData(physicalInputBindingData);
        physicalTokenEmitterAbility.SetInputStateData(Entity.GetEntityData<PhysicalInputStateData>());

        frameworkInputProcessorAbility = Entity.GetAbility<FrameworkInputProcessorAbility>();
    }

    public override void Uninitialize()
    {
        physicalTokenEmitterAbility?.SetTokenRequester(null);
        physicalTokenEmitterAbility?.SetInputBindingData(null);
        physicalTokenEmitterAbility?.SetInputStateData(null);

        physicalTokenEmitterAbility = null;
        frameworkInputProcessorAbility = null;

        base.Uninitialize();
    }

    public void RequestTokenInput(PhysicalInputTokenEvent tokenInput)
    {
        frameworkInputProcessorAbility?.ProcessInput(tokenInput);
    }
}
