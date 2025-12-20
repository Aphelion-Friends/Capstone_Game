// Adapted from this video: https://www.youtube.com/watch?v=wd3mDnogxRk&list=PLF6lFlLzb6CRom_ItuhgGRTGNArFf23uw&index=1&t=296s

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
