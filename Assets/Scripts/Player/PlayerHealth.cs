using UnityEngine;
using PurrNet.Prediction;

public class PlayerHealth : PredictedIdentity<PlayerHealth.HealthState>
{
    [SerializeField] private float _maxHealth = 100f;


    protected override HealthState GetInitialState()
    {
        return new HealthState
        {
            health = _maxHealth,
        };
    }

    public void ChangeHealth(float change)
    {
        currentState.health += change;

        if (currentState.health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("The player died!");
    }

    public struct HealthState : IPredictedData<HealthState>
    {
        public float health;

        public override string ToString()
        {
            return $"Health: {health}";
        }

        public void Dispose() {}
    }
}


