using UnityEngine;

public class InputCollectorAbility : Ability, IRawInputRequestSource
{
    TokenInputBindingData inputBindingData;
    RawInputStateData inputStateData;
    IRawInputRequester rawInputRequester;

    public void SetInputBindingData(TokenInputBindingData inputBindingData)
    {
        this.inputBindingData = inputBindingData;
    }

    public void SetInputStateData(RawInputStateData inputStateData)
    {
        this.inputStateData = inputStateData;
    }

    public void SetRawInputRequester(IRawInputRequester rawInputRequester)
    {
        this.rawInputRequester = rawInputRequester;
    }

    public override void Uninitialize()
    {
        inputBindingData = null;
        inputStateData = null;
        rawInputRequester = null;

        base.Uninitialize();
    }

    void Update()
    {
        if (inputBindingData == null || inputStateData == null || rawInputRequester == null)
        {
            return;
        }

        inputStateData.SetMouseScreenPosition(Input.mousePosition);
        TryPublishAxisInput();

        foreach (var keyCode in inputBindingData.GetBoundKeyCodes())
        {
            TryPublishKeyInput(keyCode);
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

        var inputType = axisDirection == Vector2.zero ? RawInputType.Canceled : RawInputType.Performed;
        rawInputRequester.RequestRawInput(new RawInputContext(KeyCode.None, inputType, axisDirection, inputStateData.MouseScreenPosition));
    }

    void TryPublishKeyInput(KeyCode keyCode)
    {
        var inputType = RawInputType.None;
        if (Input.GetKeyDown(keyCode))
        {
            inputType = RawInputType.Started;
        }
        else if (Input.GetKeyUp(keyCode))
        {
            inputType = RawInputType.Canceled;
        }
        else if (Input.GetKey(keyCode))
        {
            inputType = RawInputType.Performed;
        }

        if (inputType == RawInputType.None)
        {
            return;
        }

        rawInputRequester.RequestRawInput(new RawInputContext(keyCode, inputType, inputStateData.AxisDirection, inputStateData.MouseScreenPosition));
    }
}
