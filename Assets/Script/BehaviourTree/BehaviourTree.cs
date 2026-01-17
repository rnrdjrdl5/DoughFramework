public class BehaviourTree
{
    BaseNode parentNode;
    
    public BehaviourTree(BaseNode parentNode)
    {
        this.parentNode = parentNode;
    }

    public void Update()
    {
        parentNode.OnUpdateNode();
    }
}