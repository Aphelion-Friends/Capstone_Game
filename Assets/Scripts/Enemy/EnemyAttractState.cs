using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(EnemyController))]
public class EnemyAttractedState : PredictedStateNode<EnemyAttractedState.AttractedState>
{
    public delegate void TransitionFunc(ref AttractedState state);
    public TransitionFunc transitionFunc;

    private EnemyController enemyController;

    public Vector3 attractedTo { get => currentState.attractedTo; set => currentState.attractedTo = value; }

    public struct AttractedState : IPredictedData<AttractedState>
    {
        public Vector3 attractedTo;
        public void Dispose() {}
    }

    protected override void LateAwake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    protected override void StateSimulate(ref AttractedState state, float delta)
    {
        transitionFunc(ref state);
        enemyController.destination = state.attractedTo;
    }
}