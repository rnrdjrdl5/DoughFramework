public class BehaviourTreeRunnerTrait : Trait
{
    bool activeBehaviour;
    
    public BehaviourTree BehaviourTree => behaviourTree;
    
    BehaviourTree behaviourTree;

    void Update()
    {
        if (behaviourTree != null && activeBehaviour)
        {
            behaviourTree.Update();
        }
    }

    public void SetBehaviourTree(BehaviourTree behaviourTree)
    {
        this.behaviourTree = behaviourTree;
    }

    public void SetActiveBehaviour(bool active)
    {
        activeBehaviour = active;
    }
}