using UnityEngine;
using PurrNet.Prediction;
using PurrNet;

public class EnemyHealth : PredictedIdentity<EnemyHealth.HealthState>
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private NetworkAnimator _animator;
    // private EnemyAIPatrol _enemyAIPatrol;

    private PredictedEvent _onDie;

    private void Awake()
    {
        // _enemyAIPatrol = GetComponent<EnemyAIPatrol>();
    }

    protected override void LateAwake()
    {
        _onDie = new PredictedEvent(predictionManager, this);
        _onDie.AddListener(Die);
    }

    public struct HealthState : IPredictedData<HealthState>
    {
        public float health;
        public bool alive;

        public override string ToString()
        {
            return $"Health: {health}. Alive: {alive}.";
        }

        public void Dispose() {}
    }

    protected override HealthState GetInitialState()
    {
        return new HealthState
        {
            health = _maxHealth,
            alive = true,
        };
    }
    
    public void ChangeHealth(float change)
    {
        currentState.health += change;

        if (currentState.health <= 0 && currentState.alive)
        {
            _onDie?.Invoke();
        }
    }

    private void Die()
    {
        Debug.Log("The enemy died!");
        currentState.alive = false;
        _animator.SetTrigger("Die");
        // _enemyAIPatrol.Stop();
    }
}
