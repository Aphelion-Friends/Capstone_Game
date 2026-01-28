using UnityEngine;
using PurrNet.Prediction;
using PurrNet;

public class EnemyHealth : PredictedIdentity<EnemyHealth.HealthState>
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private NetworkAnimator _animator;
    // private EnemyAIPatrol _enemyAIPatrol;

    private PredictedEvent _onDie;
    [SerializeField] private GenericEnemy enemy;

    private void Awake()
    {
        // _enemyAIPatrol = GetComponent<EnemyAIPatrol>();
        enemy = GetComponent<GenericEnemy>();
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

    public void Reset()
    {
        currentState.health = _maxHealth;
        currentState.alive = true;
    }

    private void Die()
    {
        Debug.Log("The enemy died!");
        currentState.alive = false;
        // _animator.SetTrigger("Die");
        enemy.OnDeath();
        // _enemyAIPatrol.Stop();
    }
}
