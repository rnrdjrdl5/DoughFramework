using System.Linq;

public class Processor
{
    public Entity Entity => entity;
    public ProcessorAbility ProcessorAbility => processorAbility;
    public Realm Realm => realm;
    public PanelAbility PanelAbility => panelAbility;
    
    Realm realm;
    PanelAbility panelAbility;
    Entity entity;
    ProcessorAbility processorAbility;

    public virtual void Initialize()
    {

    }

    public virtual void Uninitialize()
    {

    }

    public virtual void Ready()
    {
        
    }

    public static ProcessorType Create<ProcessorType>(Entity entity) where ProcessorType : Processor, new()
    {
        ProcessorType processor = new();
        processor.entity = entity;
        processor.realm = entity.GetRootParent<Realm>();
        processor.panelAbility = processor.realm.GetAbility<PanelAbility>();

        processor.Initialize();

        return processor;
    }

    public static Processor Create(System.Type type, Entity entity, ProcessorAbility processorAbility)
    {
        var processor = System.Activator.CreateInstance(type) as Processor;
        processor.entity = entity;
        processor.processorAbility = processorAbility;
        processor.realm = entity.GetRootParent<Realm>();
        processor.panelAbility = processor.realm.GetAbility<PanelAbility>();

        processor.Initialize();

        return processor;
    }

    public void SetProcessorAbility(ProcessorAbility processorAbility)
    {
        this.processorAbility = processorAbility;
    }
}
