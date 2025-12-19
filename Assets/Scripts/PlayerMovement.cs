using UnityEngine;
using PurrNet.Prediction;

public class PlayerMovement : PredictedIdentity<PlayerMovement.MoveInput, PlayerMovement.MoveState>
{
    protected override void Simulate(MoveInput input, ref MoveState state, float delta)
    {

    }

    public struct MoveInput : IPredictedData
    {
        public Vector2 moveDirection;
        public Vector3 cameraForward;

        public void Dispose() {}
    }

    public struct MoveState : IPredictedData<MoveState>
    {
        public void Dispose() {}
    }
}
