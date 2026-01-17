using UnityEngine;

public class RandomDecorator : DecoratorNode
{
    public override BTNodeState OnUpdateNode()
    {
        // 랜덤 정의하기
        var result = true;
        if (result)
        {
            return base.OnUpdateNode();
        }
        
        return BTNodeState.Fail;
    }
}