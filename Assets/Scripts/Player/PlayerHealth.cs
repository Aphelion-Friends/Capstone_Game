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
    }

    public struct HealthState : IPredictedData<HealthState>
    {
        public float health;

        public void Dispose() {}
    }
}


