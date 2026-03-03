using UnityEngine;
using UnityEngine.InputSystem;
using PurrNet.Prediction;

public class FlashlightToggle : PredictedIdentity<FlashlightToggle.FlashlightInput, FlashlightToggle.FlashlightState>
{
    private Light lightComp;

    public struct FlashlightInput : IPredictedData
    {
        public bool light;

        public void Dispose() {}
    }

    public struct FlashlightState : IPredictedData<FlashlightState>
    {
        public bool isOn;

        public override string ToString() { return $"Is on? {isOn}"; }

        public void Dispose() {}
    }

    protected override void LateAwake()
    {
        lightComp = GetComponent<Light>();
    }

    protected override void Simulate(FlashlightInput input, ref FlashlightState state, float delta)
    {
        if (input.light)
        {
            state.isOn = !state.isOn;
        }

        lightComp.enabled = state.isOn;
    }

    protected override void UpdateInput(ref FlashlightInput input)
    {
        input.light |= InputManager.Instance.flashlightAction.inProgress;
    }

    protected override void ModifyExtrapolatedInput(ref FlashlightInput input)
    {
        input.light = false;
    }
}

