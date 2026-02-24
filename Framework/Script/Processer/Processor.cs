using System.Linq;

public class Processor
{
    public Actor Actor => actor;
    public ProcessorAbility ProcessorAbility => processorAbility;
    public Realm Realm => realm;
    public PanelAbility PanelAbility => panelAbility;
    
    Realm realm;
    PanelAbility panelAbility;
    Actor actor;
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

    public static ProcessorType Create<ProcessorType>(Actor actor) where ProcessorType : Processor, new()
    {
        ProcessorType processor = new();
        processor.actor = actor;
        processor.realm = actor.GetRootParent<Realm>();
        processor.panelAbility = processor.realm.GetAbility<PanelAbility>();

        processor.Initialize();

        return processor;
    }

    public static Processor Create(System.Type type, Actor actor, ProcessorAbility processorAbility)
    {
        var processor = System.Activator.CreateInstance(type) as Processor;
        processor.actor = actor;
        processor.processorAbility = processorAbility;
        processor.realm = actor.GetRootParent<Realm>();
        processor.panelAbility = processor.realm.GetAbility<PanelAbility>();

        processor.Initialize();

        return processor;
    }

    public void SetProcessorAbility(ProcessorAbility processorAbility)
    {
        this.processorAbility = processorAbility;
    }
}
