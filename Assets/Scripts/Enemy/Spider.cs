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

    GameObject GetClosestPlayer(Collider[] colliderArray)
    {
        GameObject currentBest = colliderArray[0].gameObject;
        for (int x = 1; x < colliderArray.Length; x++)
        {
            float currentBestDistance = Vector3.Distance(transform.position, currentBest.transform.position);
            float currentDistance = Vector3.Distance(transform.position, colliderArray[x].gameObject.transform.position);

            if (currentDistance < currentBestDistance)
            {
                currentBest = colliderArray[x].gameObject;
            }
        }
        return currentBest;
    }
}
