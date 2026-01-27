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

    [SerializeField] float minDistance = 2f;

    public delegate void TransitionFunc(ref EnemyPatrolState.PatrolState state);
    public TransitionFunc transitionFunc;

    private EnemyController enemyController;

    protected override void LateAwake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    protected override PatrolState GetInitialState()
    {
        return new PatrolState
        {
            randomSeedSet = false,
            destPointSet = false,
        };
    }

    public struct PatrolState : IPredictedData<PatrolState>
    {
        public PredictedRandom random;
        public bool randomSeedSet;

        public Vector3 destPoint;
        public bool destPointSet;

        public float giveUpTimer;

        public override string ToString()
        {
            return $"Give up timer: {giveUpTimer}\nDest point: {destPoint}\nDest point set: {destPointSet}";
        }

        public void Dispose() {}
    }

    public override void Enter()
    {
        currentState.destPointSet = false;
    }

    protected override void StateSimulate(ref PatrolState state, float delta)
    {

        if (!state.randomSeedSet)
            currentState.random.seed = (uint) UnityEngine.Random.Range(0, 100000);

        transitionFunc(ref state);

        state.giveUpTimer -= delta;

        float distance = Vector3.Distance(state.destPoint, transform.position);

        if (state.giveUpTimer <= 0 || distance <= minDistance)
        {
            state.destPointSet = false;
        }

        if (state.giveUpTimer <= 0)
        {
            state.giveUpTimer = giveUpTime;
        }

        if (!state.destPointSet)
        {
            Vector3 newTarget = FindRandomDestPoint();
            state.destPoint = newTarget;
            enemyController.destination = state.destPoint;
            state.destPointSet = true;
            Debug.Log("Set a new dest point!");
        }
    }

    Vector3 FindRandomDestPoint()
    {
        float x = currentState.random.NextFloat(-walkRange, walkRange);
        float z = currentState.random.NextFloat(-walkRange, walkRange);

        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }
}
