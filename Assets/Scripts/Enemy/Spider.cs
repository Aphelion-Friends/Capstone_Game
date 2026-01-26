using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(PredictedStateMachine))]
[RequireComponent(typeof(EnemyPatrolState))]
[RequireComponent(typeof(EnemyChaseState))]
public class Spider : StatelessPredictedIdentity
{
    PredictedStateMachine stateMachine;
    EnemyPatrolState enemyPatrolState;
    EnemyChaseState enemyChaseState;

    [SerializeField] private float chaseRange;
    [SerializeField] private LayerMask playerLayer;

    void Awake()
    {
        stateMachine = GetComponent<PredictedStateMachine>();
        enemyPatrolState = GetComponent<EnemyPatrolState>();
        enemyChaseState = GetComponent<EnemyChaseState>();
        enemyPatrolState.transitionFunc = PatrolTransitions;
        enemyChaseState.transitionFunc = ChaseTransitions;
    }

    void ChaseTransitions(ref EnemyChaseState.ChaseState state)
    {
        GameObject targetedPlayer = predictionManager.hierarchy.GetGameObject(state.targetedPlayer);

        if (Vector3.Distance(transform.position, targetedPlayer.transform.position) >= chaseRange)
        {
            Debug.Log("Transitioning to patrol state!");
            stateMachine.SetState(enemyPatrolState);
        }
    }

    void PatrolTransitions(ref EnemyPatrolState.PatrolState state)
    {
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, chaseRange, playerLayer);

        if (playerColliders.Length == 0)
            return;

        GameObject playerCollider = GetClosestPlayer(playerColliders);
        PredictedObjectID playerID;

        if (!predictionManager.hierarchy.TryGetId(playerCollider, out playerID))
            return;

        enemyChaseState.targetedPlayer = playerID;
        Debug.Log("Transitioning to chase state!");
        stateMachine.SetState(enemyChaseState);
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
