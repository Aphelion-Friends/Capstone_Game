using PurrNet.Prediction;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerReset : PredictedIdentity<PlayerReset.ResetInput, PlayerReset.ResetState>
{
    public struct ResetInput : IPredictedData
    {
        public bool reset;
        public void Dispose() { }
    }

    public struct ResetState : IPredictedData<ResetState>
    {
        public bool pressed;
        public void Dispose() { }
    }

    protected override void UpdateInput(ref ResetInput input)
    {
        input.reset |= InputManager.Instance.resetAction.inProgress;
    }
    protected override void ModifyExtrapolatedInput(ref ResetInput input)
    {
        input.reset = false;
    }
    protected override void Simulate(ResetInput input, ref ResetState state, float delta)
    {
        if (input.reset && !state.pressed)
        {
            state.pressed = true;

            predictionManager.networkManager.sceneModule.LoadSceneAsync("SpaceStationMap");
            //SceneManager.LoadScene("SpaceStationScene");

        }
        else if (!input.reset)
        {
            state.pressed = false;
        }
    }
}
