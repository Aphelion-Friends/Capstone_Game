using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(PredictedStateMachine))]
[RequireComponent(typeof(EnemyPatrolState))]
public class Spider : MonoBehaviour
{
    EnemyPatrolState enemyPatrolState;
    void Awake()
    {
        enemyPatrolState = GetComponent<EnemyPatrolState>();
        enemyPatrolState.transitionFunc = PatrolTransitions;
    }

    void PatrolTransitions(ref EnemyPatrolState.PatrolState state)
    {
        // Debug.Log("Checking if I should transition....");
    }
}
