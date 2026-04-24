using UnityEngine;
using PurrNet.Prediction;

public class PlayerRespawnManager : PredictedIdentity<PlayerRespawnManager.RespawnState>
{
    public static PlayerRespawnManager Instance;

    [SerializeField] private float _respawnTimer = 5f;
    

    private UpdateRespawnCountdown updateRespawnCounter;

    void Awake()
    {
        Instance = this;

        // updateRespawnCounter = deathScreen.GetComponent<UpdateRespawnCountdown>();
    }

    public struct RespawnState : IPredictedData<RespawnState>
    {
        public float respawnTimer;
        public bool isDead;

        public void Dispose() {}
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

        if(state.respawnTimer > 0)
            state.respawnTimer -= delta;
        else
            state.respawnTimer = 0f;

        updateRespawnCounter.SetCounterValue((int) state.respawnTimer);
    }
}
