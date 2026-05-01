using PurrNet.Packing;
using PurrNet.Prediction;
using UnityEngine;

public class PredictedEnemySpawner : PredictedIdentity<PredictedEnemySpawner.EnemySpawnerState>
{
    [Header("Setup")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform spawnPoint;

    [Header("Rules")]
    [SerializeField] float spawnInterval = 5f;
    [SerializeField] int maxAlive = 3;

    protected override EnemySpawnerState GetInitialState()
    {
        return new EnemySpawnerState
        {
            timer = spawnInterval,
            aliveCount = 0
        };
    }

    protected override void LateAwake()
    {
        TrySpawn(ref currentState);
    }
    protected override void Simulate(ref EnemySpawnerState state, float delta)
    {
        state.timer -= delta;

        if (state.timer <= 0f)
        {
            TrySpawn(ref state);
            state.timer = spawnInterval;
        }
    }

    void TrySpawn(ref EnemySpawnerState state)
    {
        if (state.aliveCount >= maxAlive)
            return;

        var enemy = hierarchy.Create(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        if (!enemy.HasValue)
            return;

        state.aliveCount++;
    }

    public void OnEnemyKilled()
    {
        currentState.aliveCount = Mathf.Max(0, currentState.aliveCount - 1);
    }

    public struct EnemySpawnerState : IPredictedData<EnemySpawnerState>, IDuplicate<EnemySpawnerState>
    {
        public float timer;
        public int aliveCount;

        public void Dispose() { }

        public EnemySpawnerState Duplicate()
        {
            return new EnemySpawnerState
            {
                timer = timer,
                aliveCount = aliveCount
            };
        }
    }

}
