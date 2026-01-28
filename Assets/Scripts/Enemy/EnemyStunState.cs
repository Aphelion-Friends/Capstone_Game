using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

public class EnemyStunState : PredictedStateNode<EnemyStunState.StunState>
{
    [SerializeField] private float _stunTimer = 5f;

    public delegate void TransitionFunc(ref EnemyStunState.StunState state);
    public TransitionFunc transitionFunc;

    public struct StunState : IPredictedData<StunState>
    {
        public float stunTimer;

        public void Dispose() {}
    }

    public override void Enter()
    {
        currentState.stunTimer = _stunTimer;
    }

    protected override void StateSimulate(ref StunState state, float delta)
    {
        state.stunTimer -= delta;

        if (state.stunTimer <= 0)
        {
            transitionFunc(ref state);
        }
    }
}
