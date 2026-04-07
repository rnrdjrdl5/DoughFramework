using UnityEngine;

public class CooldownDecorator : DecoratorNode
{
    float cooldownTime;
    float nextAvailableTime;

    public void SetCooldownTime(float cooldownTime)
    {
        this.cooldownTime = Mathf.Max(0.0f, cooldownTime);
    }

    public override BTNodeState OnUpdateNode()
    {
        if (Time.time < nextAvailableTime)
        {
            return BTNodeState.Fail;
        }

        var result = base.OnUpdateNode();
        if (result == BTNodeState.Success && cooldownTime > 0.0f)
        {
            nextAvailableTime = Time.time + cooldownTime;
        }

        return result;
    }
}
