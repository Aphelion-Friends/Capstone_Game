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
    
    // private void SetLight(bool On)
    // {
    //     lightComp.enabled = On;
    // }

    protected override void Simulate(FlashlightInput input, ref FlashlightState state, float delta)
    {
        // if (input.light)
        // {
        //     isOn = !isOn;
        // }

        // lightComp = isOn;
    }

    protected override void UpdateInput(ref FlashlightInput input)
    {
        input.light |= InputManager.Instance.flashlightAction.inProgress;
    }
}

