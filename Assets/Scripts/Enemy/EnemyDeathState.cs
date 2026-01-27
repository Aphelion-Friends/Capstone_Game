using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

public class EnemyDeathState : PredictedStateNode<EnemyDeathState.DeathState>
{
    private EnemyController enemyController;

    public struct DeathState : IPredictedData<DeathState>
    {
        public void Dispose() {}
    }

    protected override void LateAwake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public override void Enter()
    {
        enemyController.active = false;
    }
}
