using UnityEngine;
using PurrNet.Prediction;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;


public class PlayerRespawnManager : PredictedIdentity<PlayerRespawnManager.RespawnState>
{
    public static PlayerRespawnManager Instance;

    [SerializeField] private float _respawnTimer = 5f;

    [SerializeField] private GameObject deathScreen;

    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private Transform[] spawnPoints;
    
    public PurrNet.PlayerID player;

    private UpdateRespawnCountdown updateRespawnCounter;

    private PredictedPlayerSpawner playerRespawn;

    void Awake()
    {
        Instance = this;

        updateRespawnCounter = deathScreen.GetComponent<UpdateRespawnCountdown>();

        playerRespawn = FindObjectsByType<PredictedPlayerSpawner>(FindObjectsSortMode.None)[0];
    }

    public struct RespawnState : IPredictedData<RespawnState>
    {
        public float respawnTimer;
        public bool isDead;

        public void Dispose() {}

        public override string ToString() {return $"timer {respawnTimer}";}
    }

    protected override RespawnState GetInitialState()
    {
        return new RespawnState
        {
            respawnTimer = _respawnTimer,
            isDead = false,
        };
    }

    public void PlayerDied()
    {
        Debug.Log("Dead");
        RestartTimer();
        currentState.isDead = true;
        // deathScreen.SetActive(true);
    }

    public void RestartTimer()
    {
        currentState.respawnTimer = _respawnTimer;
    }

    protected override void Simulate(ref RespawnState state, float delta)
    {

        if (!state.isDead) return;

        if(state.respawnTimer > 0) {
            state.respawnTimer -= delta;
        }
        else
        {
            state.respawnTimer = 0f;

            if (state.isDead)
            {
                state.isDead = false;
                TriggerRespawn();
            }

        }


        Debug.Log($"isOwner: {isOwner}, timer: {state.respawnTimer}");
        updateRespawnCounter.SetCounterValue((int) state.respawnTimer);
     
    }

    private void TriggerRespawn()
    {
        deathScreen.SetActive(false);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var newPlayer = hierarchy.Create(_playerPrefab, spawnPoint.position, spawnPoint.rotation, owner);
        predictionManager.SetOwnership(newPlayer, player);  

    }

}
