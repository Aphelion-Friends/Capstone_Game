using PurrNet.Packing;
using PurrNet.Prediction;
using UnityEngine;

public class PredictedItemSpawner : PredictedIdentity<PredictedItemSpawner.ItemSpawnerState>
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float respawnTime = 20f;

    protected override ItemSpawnerState GetInitialState()
    {
        return new ItemSpawnerState
        {
            isOccupied = false,
            respawnTimer = 0f
        };
    }

    protected override void LateAwake()
    {
        SpawnItem(ref currentState);
    }

    protected override void Simulate(ref ItemSpawnerState state, float delta)
    {
        if (!state.isOccupied)
        {
            state.respawnTimer -= delta;

            if (state.respawnTimer <= 0f)
                SpawnItem(ref state);
        }
    }

    void SpawnItem(ref ItemSpawnerState state)
    {
        var obj = hierarchy.Create(itemPrefab, spawnPoint.position, spawnPoint.rotation);

        if (!obj.HasValue)
            return;

        state.spawnedObject = obj.Value;
        state.isOccupied = true;
    }

    public struct ItemSpawnerState : IPredictedData<ItemSpawnerState>, IDuplicate<ItemSpawnerState>
    {
        public bool isOccupied;
        public PredictedObjectID spawnedObject;
        public float respawnTimer;

        public void Dispose() { }

        public ItemSpawnerState Duplicate()
        {
            return new ItemSpawnerState
            {
                isOccupied = isOccupied,
                spawnedObject = spawnedObject,
                respawnTimer = respawnTimer
            };
        }
    }

}
