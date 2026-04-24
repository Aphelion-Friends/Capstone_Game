using UnityEngine;
using PurrNet.Prediction;

public class PlayerRespawnManager : PredictedIdentity<PlayerRespawnManager.RespawnState>
{
    public static PlayerRespawnManager Instance;

    [SerializeField] private float _respawnTimer = 5f;

    [SerializeField] private GameObject deathScreen;
    

    private UpdateRespawnCountdown updateRespawnCounter;

    void Awake()
    {
        Instance = this;

        updateRespawnCounter = deathScreen.GetComponent<UpdateRespawnCountdown>();
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

        if(state.respawnTimer > 0)
            state.respawnTimer -= delta;
        else
            state.respawnTimer = 0f;


        Debug.Log($"isOwner: {isOwner}, timer: {state.respawnTimer}");
        updateRespawnCounter.SetCounterValue((int) state.respawnTimer);
     
    }

}
