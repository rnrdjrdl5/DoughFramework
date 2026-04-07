using UnityEngine;

public class RandomDecorator : DecoratorNode
{
    float successProbability = 0.5f;
    bool canRun;

    public void SetSuccessProbability(float successProbability)
    {
        this.successProbability = Mathf.Clamp01(successProbability);
    }

    public override BTNodeState OnUpdateNode()
    {
        if (canRun)
        {
            return base.OnUpdateNode();
        }

        return BTNodeState.Fail;
    }

    public override void OnEnterNode()
    {
        base.OnEnterNode();

        canRun = Random.value <= successProbability;
    }
}
