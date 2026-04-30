using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;

[RequireComponent(typeof(PredictedStateMachine))]
[RequireComponent(typeof(EnemyPatrolState))]
[RequireComponent(typeof(EnemyChaseState))]
[RequireComponent(typeof(EnemyAttackState))]
[RequireComponent(typeof(EnemyDeathState))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyFleeState))]
[RequireComponent(typeof(EnemyAttractedState))]
public abstract class GenericEnemy : StatelessPredictedIdentity
{
    protected PredictedStateMachine stateMachine;
    protected EnemyPatrolState enemyPatrolState;
    protected EnemyChaseState enemyChaseState;
    protected EnemyAttackState enemyAttackState;
    protected EnemyDeathState enemyDeathState;
    protected EnemyFleeState enemyFleeState;
    protected EnemyAttractedState enemyAttractedState;
    protected FlashlightDetector flashlightDetector;
    protected FlashlightSense flashlightSense;

    [SerializeField] protected float chaseRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected LayerMask playerLayer;

    virtual protected void Awake()
    {
        stateMachine = GetComponent<PredictedStateMachine>();
        enemyPatrolState = GetComponent<EnemyPatrolState>();
        enemyChaseState = GetComponent<EnemyChaseState>();
        enemyAttackState = GetComponent<EnemyAttackState>();
        enemyDeathState = GetComponent<EnemyDeathState>();
        enemyFleeState = GetComponent<EnemyFleeState>();
        enemyAttractedState = GetComponent<EnemyAttractedState>();
        flashlightDetector = GetComponent<FlashlightDetector>();
        flashlightSense = GetComponent<FlashlightSense>();
        enemyPatrolState.transitionFunc = PatrolTransitions;
        enemyChaseState.transitionFunc = ChaseTransitions;
        enemyAttackState.transitionFunc = AttackTransitions;
        enemyFleeState.transitionFunc = FleeTransitions;
        enemyAttractedState.transitionFunc = AttractedTransitions;
    }
    
    virtual protected void AttackTransitions(ref EnemyAttackState.AttackState state)
    {
        GameObject targetedPlayer = predictionManager.hierarchy.GetGameObject(state.targetedPlayer);

        if (targetedPlayer is null)
        {
            stateMachine.SetState(enemyChaseState);
            return;
        }

        if (Vector3.Distance(transform.position, targetedPlayer.transform.position) >= attackRange)
        {
            stateMachine.SetState(enemyChaseState);
        }
    }

    virtual protected void ChaseTransitions(ref EnemyChaseState.ChaseState state)
    {
        GameObject targetedPlayer = predictionManager.hierarchy.GetGameObject(state.targetedPlayer);

        if (Vector3.Distance(transform.position, targetedPlayer.transform.position) >= chaseRange)
        {
            Debug.Log("Transitioning to patrol state!");
            stateMachine.SetState(enemyPatrolState);
        }
        else if (Vector3.Distance(transform.position, targetedPlayer.transform.position) <= attackRange)
        {
            stateMachine.SetState(enemyAttackState);
        }
    }

    virtual protected void PatrolTransitions(ref EnemyPatrolState.PatrolState state)
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

    virtual protected void FleeTransitions(ref EnemyFleeState.FleeState state)
    {
        if (flashlightDetector == null || !flashlightDetector.isLit)
            stateMachine.SetState(enemyPatrolState);
    }

    virtual protected void AttractedTransitions(ref EnemyAttractedState.AttractedState state)
    {
        if (flashlightDetector == null || !flashlightDetector.isLit)
            stateMachine.SetState(enemyChaseState);
    }

    virtual protected GameObject GetClosestPlayer(Collider[] colliderArray)
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

    virtual public void OnDeath()
    {
        stateMachine.SetState(enemyDeathState);
    }
}
