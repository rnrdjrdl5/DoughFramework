using UnityEngine;

// 입력 기반 이동 벡터를 계산하고 Transform에 반영하는 Ability
public sealed class MoveAbility : Ability, IAbilityTick
{
    public float InputX { get; private set; }
    public float InputY { get; private set; }
    public float Speed { get; private set; }
    public float VelocityX { get; private set; }
    public float VelocityY { get; private set; }

    Transform ownerTransform;

    protected override void OnInitialize()
    {
        ownerTransform = Owner != null ? Owner.transform : null;
    }

    protected override void OnReady()
    {
        if (AbilityResolver is AbilityHost host)
        {
            host.RegisterTick(this);
        }
    }

    protected override void OnUninitialize()
    {
        if (AbilityResolver is AbilityHost host)
        {
            host.UnregisterTick(this);
        }

        ClearInput();
        Speed = 0f;
    }

    public void SetInput(float x, float y)
    {
        InputX = x;
        InputY = y;
        RefreshVelocity();
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
        RefreshVelocity();
    }

    public void ClearInput()
    {
        InputX = 0f;
        InputY = 0f;
        VelocityX = 0f;
        VelocityY = 0f;
    }

    public void Tick()
    {
        if (ownerTransform == null)
        {
            return;
        }

        var delta = new Vector3(VelocityX, VelocityY, 0f) * Time.deltaTime;
        if (delta == Vector3.zero)
        {
            return;
        }

        ownerTransform.position += delta;
    }

    void RefreshVelocity()
    {
        VelocityX = InputX * Speed;
        VelocityY = InputY * Speed;
    }
}
