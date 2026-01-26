using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(PredictedStateMachine))]
[RequireComponent(typeof(EnemyPatrolState))]
[RequireComponent(typeof(EnemyChaseState))]
[RequireComponent(typeof(EnemyAttackState))]
public class Psychotron : StatelessPredictedIdentity
{
}
