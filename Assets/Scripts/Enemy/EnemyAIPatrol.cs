// Adapted from this tutorial: https://www.youtube.com/watch?v=-Iwsz4gdgyQ

using UnityEngine;
using PurrNet.Prediction;
using PurrNet.Prediction.StateMachine;
using System;

[RequireComponent(typeof(EnemyController))]
public class EnemyPatrolState : PredictedStateNode<EnemyPatrolState.PatrolState>
{
    [SerializeField] LayerMask groundLayer, playerLayer;

    [SerializeField] private float giveUpTime;

    [SerializeField] float walkRange;

    [SerializeField] float sightRange;

    [SerializeField] float minDistance = 2f;

    public delegate void TransitionFunc(ref EnemyPatrolState.PatrolState state);
    public TransitionFunc transitionFunc;

    private EnemyController enemyController;

    protected override void SimulationStart()
    {
        enemyController = GetComponent<EnemyController>();
    }

    protected override PatrolState GetInitialState()
    {
        return new PatrolState
        {
            stopped = false,
            randomSeedSet = false
        };
    }

    public void Stop()
    {
        currentState.stopped = true;
        enemyController.active = false;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.constraints &= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public struct PatrolState : IPredictedData<PatrolState>
    {
        public PredictedRandom random;
        public bool randomSeedSet;

        public bool stopped;

        public Vector3 destPoint;
        public bool destPointSet;

        public bool playerInSight;
        public bool playerInAttackRange;

        public float attackCooldownTimer;
        public float giveUpTimer;

        public override string ToString()
        {
            return $"Give up timer: {giveUpTimer}\nDest point: {destPoint}\nDest point set: {destPointSet}\nPlayer in sight: {playerInSight}\nPlayer in attack range: {playerInAttackRange}";
        }

        public void Dispose() {}
    }

    protected override void StateSimulate(ref PatrolState state, float delta)
    {
        if (state.stopped)
            return;

        if (!state.randomSeedSet)
            currentState.random.seed = (uint) Random.Range(0, 100000);

        state.attackCooldownTimer -= delta;

        state.giveUpTimer -= delta;

        float distance = Vector3.Distance(state.destPoint, transform.position);

        // Destination reached
        if (distance <= minDistance)
        {
            Debug.Log("Dest reached!");
            state.destPointSet = false;
            state.giveUpTimer = giveUpTime;
        }

        if (state.destPointSet && state.giveUpTimer <= 0)
        {
            state.giveUpTimer = giveUpTime;
            state.destPointSet = false;
            Debug.Log("Could not reach dest point! I give up!");
        }

        GameObject targetedPlayer = null;
        Collider[] playerInSightColliders = Physics.OverlapSphere(transform.position, sightRange, playerLayer);

        state.playerInSight = playerInSightColliders.Length > 0;

        if (state.playerInSight)
        {
            targetedPlayer = GetClosestPlayer(playerInSightColliders);
        }

        if (!state.playerInSight && !state.playerInAttackRange) 
        {
            // Patrol(ref PatrolState state);
            state.destPointSet = true;
        }

        if (targetedPlayer is null)
            return;

        if (state.playerInSight && !state.playerInAttackRange) Chase(targetedPlayer);
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

    void Chase(GameObject targetedPlayer)
    {
        enemyController.destination = targetedPlayer.transform.position;
    }

    void Patrol(ref PatrolState state)
    {
        if (!currentState.destPointSet)
        {
            Debug.Log("No dest point set! Searching!");
            SearchForDest();
            state.destPointSet = true;
            state.giveUpTimer = giveUpTime;
        }
    }

    void SearchForDest()
    {
        float x = currentState.random.NextFloat(-walkRange, walkRange);
        float z = currentState.random.NextFloat(-walkRange, walkRange);

        currentState.destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        Debug.Log("New dest point!: " + currentState.destPoint);

        enemyController.destination = currentState.destPoint;
        Debug.Log($"Dest point set? {currentState.destPointSet}");
    }
}
