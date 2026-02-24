using System.Linq;

public class Processor
{
    public Actor Actor => actor;
    public ProcessorTrait ProcessorTrait => processorTrait;
    public Realm Realm => realm;
    public PanelTrait PanelTrait => panelTrait;
    
    Realm realm;
    PanelTrait panelTrait;
    Actor actor;
    ProcessorTrait processorTrait;

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
        processor.panelTrait = processor.realm.GetTrait<PanelTrait>();

        processor.Initialize();

        return processor;
    }

    public static Processor Create(System.Type type, Actor actor, ProcessorTrait processorTrait)
    {
        var processor = System.Activator.CreateInstance(type) as Processor;
        processor.actor = actor;
        processor.processorTrait = processorTrait;
        processor.realm = actor.GetRootParent<Realm>();
        processor.panelTrait = processor.realm.GetTrait<PanelTrait>();

        processor.Initialize();

        return processor;
    }

    public void SetProcessorTrait(ProcessorTrait processorTrait)
    {
        this.processorTrait = processorTrait;
    }
}
