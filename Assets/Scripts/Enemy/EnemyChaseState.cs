using UnityEngine;
using PurrNet;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(EnemyController))]
public class EnemyChaseState : PredictedStateNode<EnemyChaseState.ChaseState>
{
    public delegate void TransitionFunc(ref EnemyChaseState.ChaseState state);
    public TransitionFunc transitionFunc;

    private EnemyController enemyController;

    public struct ChaseState : IPredictedData<ChaseState>
    {
        public PredictedObjectID? targetedPlayer;

        public void Dispose() {}
    }

    protected override void LateAwake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    protected override void StateSimulate(ref ChaseState state, float delta)
    {
        transitionFunc(ref state);

        if (!state.targetedPlayer.HasValue)
            return;

        GameObject targetedPlayerGameObject = predictionManager.hierarchy.GetGameObject(state.targetedPlayer.Value);
        enemyController.destination = targetedPlayerGameObject.transform.position;
    }
}
