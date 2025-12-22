using UnityEngine;
using PurrNet.Prediction;

public class EnemyHealth : PredictedIdentity<EnemyHealth.HealthState>
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private Animator _animator;
    private EnemyAIPatrol _enemyAIPatrol;

    public struct HealthState : IPredictedData<HealthState>
    {
        public float health;
        public bool alive;

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
    }

    private void Die()
    {
        Debug.Log("The enemy died!");
        _animator.SetTrigger("Die");
        _enemyAIPatrol.enabled = false;
    }
}
