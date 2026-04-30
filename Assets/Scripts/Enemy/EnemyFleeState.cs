using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(EnemyController))]
public class EnemyFleeState : PredictedStateNode<EnemyFleeState.FleeState>
{
    public delegate void TransitionFunc(ref FleeState state);
    public TransitionFunc transitionFunc;

    [SerializeField] private float _fleeDistance = 10f;
    private EnemyController enemyController;

    public Vector3 fleeFrom { get => currentState.fleeFrom; set => currentState.fleeFrom = value; }

    public struct FleeState : IPredictedData<FleeState>
    {
        public Vector3 fleeFrom;
        public void Dispose() {}
    }

    protected override void LateAwake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    protected override void StateSimulate(ref FleeState state, float delta)
    {
        transitionFunc(ref state);

        Vector3 fleeDirection = (transform.position - state.fleeFrom).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * _fleeDistance;
        enemyController.destination = fleeTarget;
    }
}