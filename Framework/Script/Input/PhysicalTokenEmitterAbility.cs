using UnityEngine;

public class PhysicalTokenEmitterAbility : Ability, IPhysicalInputTokenRequestSource
{
    PhysicalInputBindingData inputBindingData;
    PhysicalInputStateData inputStateData;
    IPhysicalInputTokenRequester tokenRequester;

    public void SetInputBindingData(PhysicalInputBindingData inputBindingData)
    {
        this.inputBindingData = inputBindingData;
    }

    public void SetInputStateData(PhysicalInputStateData inputStateData)
    {
        this.inputStateData = inputStateData;
    }

    public void SetTokenRequester(IPhysicalInputTokenRequester tokenRequester)
    {
        this.tokenRequester = tokenRequester;
    }

    public override void Uninitialize()
    {
        inputBindingData = null;
        inputStateData = null;
        tokenRequester = null;

        base.Uninitialize();
    }

    void Update()
    {
        if (inputBindingData == null || inputStateData == null || tokenRequester == null)
        {
            return;
        }

        TryPublishAxisInput();
        inputStateData.SetMouseScreenPosition(Input.mousePosition);

        foreach (var keyCode in inputBindingData.GetBoundKeyCodes())
        {
            TryPublishTokenInput(keyCode);
        }
    }

    void TryPublishAxisInput()
    {
        var axisDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var isChanged = inputStateData.SetAxisDirection(axisDirection);
        if (!isChanged && axisDirection == Vector2.zero)
        {
            return;
        }

        tokenRequester.RequestTokenInput(new PhysicalInputTokenEvent
        {
            TokenType = PhysicalInputTokenType.AxisInputChanged
        });
    }

    void TryPublishTokenInput(KeyCode keyCode)
    {
        if (!Input.GetKeyDown(keyCode))
        {
            return;
        }

        if (!inputBindingData.TryGetTokenType(keyCode, out var tokenType))
        {
            return;
        }

        var tokenInput = new PhysicalInputTokenEvent
        {
            KeyCode = keyCode,
            TokenType = tokenType
        };

        tokenRequester.RequestTokenInput(tokenInput);
    }
}
