using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(PredictedStateMachine))]
[RequireComponent(typeof(EnemyPatrolState))]
[RequireComponent(typeof(EnemyChaseState))]
public class Spider : MonoBehaviour
{
    EnemyPatrolState enemyPatrolState;
    EnemyChaseState enemyChaseState;
    void Awake()
    {
        enemyPatrolState = GetComponent<EnemyPatrolState>();
        enemyChaseState = GetComponent<EnemyChaseState>();
        enemyPatrolState.transitionFunc = PatrolTransitions;
    }

    void PatrolTransitions(ref EnemyPatrolState.PatrolState state)
    {
        // Debug.Log("Checking if I should transition....");
    }
}
