using System.Linq;

public class Processer
{
    public Actor Actor => actor;
    public ProcesserTrait ProcesserTrait => processerTrait;
    public Realm Realm => realm;
    public PanelTrait PanelTrait => panelTrait;
    
    Realm realm;
    PanelTrait panelTrait;
    Actor actor;
    ProcesserTrait processerTrait;

    public virtual void Initialize()
    {

    }

    public virtual void Uninitialize()
    {

    }

    public virtual void Ready()
    {
        
    }

    public static ProcesserType Create<ProcesserType>(Actor actor) where ProcesserType : Processer, new()
    {
        ProcesserType processer = new();
        processer.actor = actor;
        processer.realm = actor.GetRootParent<Realm>();
        processer.panelTrait = processer.realm.GetTrait<PanelTrait>();

        processer.Initialize();

        return processer;
    }

    public static Processer Create(System.Type type, Actor actor, ProcesserTrait processerTrait)
    {
        var processer = System.Activator.CreateInstance(type) as Processer;
        processer.actor = actor;
        processer.processerTrait = processerTrait;
        processer.realm = actor.GetRootParent<Realm>();
        processer.panelTrait = processer.realm.GetTrait<PanelTrait>();

        processer.Initialize();

        return processer;
    }

    public void SetProcesserTrait(ProcesserTrait processerTrait)
    {
        this.processerTrait = processerTrait;
    }
}
