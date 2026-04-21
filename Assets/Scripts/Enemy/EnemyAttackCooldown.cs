using UnityEngine;
using PurrNet.Prediction;

public class EnemyAttackCooldown : PredictedIdentity<EnemyAttackCooldown.CooldownState>
{
    [SerializeField] private float cooldown = 2f;

    public struct CooldownState : IPredictedData<CooldownState>
    {
        public float timer;

        public void Dispose() {}
    }

    protected override CooldownState GetInitialState()
    {
        return new CooldownState
        {
            timer = cooldown,
        };
    }

    protected override void Simulate(ref CooldownState state, float delta)
    {
        state.timer -= delta;
    }

    public void ResetTimer()
    {
        currentState.timer = cooldown;
    }
}
