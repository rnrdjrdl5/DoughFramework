using System.Collections.Generic;
using System.Linq;

public partial class Processor
{
    public Entity Entity => entity;
    public ProcessorAbility ProcessorAbility => processorAbility;
    public Realm Realm => realm;
    public PanelAbility PanelAbility => panelAbility;

    ProcessorSet<Processor> processorSet = new();
    Realm realm;
    PanelAbility panelAbility;
    Entity entity;
    ProcessorAbility processorAbility;
    bool isDynamic;

    public virtual void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
    }

    public virtual void Uninitialize()
    {

    }

    public virtual void Update()
    {
        foreach (var processor in processorSet.Processors)
        {
            processor.Update();
        }
    }

    public virtual void FixedUpdate()
    {
        foreach (var processor in processorSet.Processors)
        {
            processor.FixedUpdate();
        }
    }

    public static ProcessorType Create<ProcessorType>(Entity entity, ProcessorAbility processorAbility, bool isDynamic = false) where ProcessorType : Processor, new()
    {
        var processor = new ProcessorType();
        processor.entity = entity;
        processor.processorAbility = processorAbility;
        processor.realm = entity.GetRootParent<Realm>();
        processor.panelAbility = processor.realm.GetAbility<PanelAbility>();
        processor.isDynamic = isDynamic;

        return processor;
    }

    public static Processor Create(System.Type type, Entity entity, ProcessorAbility processorAbility, bool isDynamic = false)
    {
        var processor = System.Activator.CreateInstance(type) as Processor;
        processor.entity = entity;
        processor.processorAbility = processorAbility;
        processor.realm = entity.GetRootParent<Realm>();
        processor.panelAbility = processor.realm.GetAbility<PanelAbility>();
        processor.isDynamic = isDynamic;

        return processor;
    }

    public void SetProcessorAbility(ProcessorAbility processorAbility)
    {
        this.processorAbility = processorAbility;
    }
}
