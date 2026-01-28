using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

public class EnemyStunState : PredictedStateNode<EnemyStunState.StunState>
{
    [SerializeField] private float _stunTimer = 5f;

    public delegate void TransitionFunc(ref EnemyStunState.StunState state);
    public TransitionFunc transitionFunc;

    protected override StunState GetInitialState()
    {
        return new StunState {
            stunTimer = _stunTimer,
        };
    }

    public struct StunState : IPredictedData<StunState>
    {
        public float stunTimer;

        public override string ToString()
        {
            return $"Stun timer: {stunTimer}";
        }
        public void Dispose() {}
    }

    public override void Enter()
    {
        currentState.stunTimer = _stunTimer;
        Debug.Log("STUNNNN!");
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
