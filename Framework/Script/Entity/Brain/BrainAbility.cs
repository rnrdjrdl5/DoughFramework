using System.Collections.Generic;
using System.Linq;

public class BrainAbility : Ability
{
    public Brain MainPlayerBrain => mainPlayerBrain;
    public IReadOnlyCollection<Brain> Brains => brains;

    readonly HashSet<Brain> brains = new();
    Brain mainPlayerBrain;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        brains.Clear();
        mainPlayerBrain = null;

        if (Entity == null)
        {
            return;
        }

        Entity.OnChildAdded += OnChildAdded;
        Entity.OnChildRemoved += OnChildRemoved;
    }

    public override void Uninitialize()
    {
        if (Entity != null)
        {
            Entity.OnChildAdded -= OnChildAdded;
            Entity.OnChildRemoved -= OnChildRemoved;
        }

        brains.Clear();
        mainPlayerBrain = null;

        base.Uninitialize();
    }

    public bool Register(Brain brain)
    {
        if (brain == null)
        {
            return false;
        }

        return brains.Add(brain);
    }

    public bool Unregister(Brain brain)
    {
        if (brain == null)
        {
            return false;
        }

        if (mainPlayerBrain == brain)
        {
            mainPlayerBrain = null;
        }

        return brains.Remove(brain);
    }

    public void SetMainPlayerBrain(Brain brain)
    {
        if (brain == null)
        {
            mainPlayerBrain = null;
            return;
        }

        Register(brain);
        mainPlayerBrain = brain;
    }

    public (Brain brain, TControlled controlled) CreateBrainAndControlled<TControlled>(
        string brainPath,
        string controlledPath,
        IInitData brainInitData = null,
        IInitData controlledInitData = null) where TControlled : Entity, new()
    {
        var realm = Entity as Realm;
        if (realm == null)
        {
            return default;
        }

        var brain = realm.AddEntity<Brain>(brainPath, brainInitData);
        var controlled = realm.AddEntity<TControlled>(controlledPath, controlledInitData);
        brain?.AttachControll(controlled);

        return (brain, controlled);
    }

    public Brain CreateBrainAndEntity(
        string brainPath,
        string entityPath,
        IInitData brainInitData = null,
        IInitData entityInitData = null)
    {
        var result = CreateBrainAndControlled<Entity>(brainPath, entityPath, brainInitData, entityInitData);
        return result.brain;
    }

    public IEnumerable<Brain> GetBrains(BrainControlMode controlMode)
    {
        return brains.Where(brain => brain != null && brain.ControlMode == controlMode);
    }

    void OnChildAdded(Entity entity)
    {
        if (entity is not Brain brain)
        {
            return;
        }

        Register(brain);
    }

    void OnChildRemoved(Entity entity)
    {
        if (entity is Brain brain)
        {
            Unregister(brain);
            return;
        }

        var controlledBrains = Entity.GetChildren<Brain>()
            .Where(brain => brain != null && brain.Controll == entity)
            .ToList();

        foreach (var controlledBrain in controlledBrains)
        {
            Entity.RemoveChild(controlledBrain);
        }
    }
}
