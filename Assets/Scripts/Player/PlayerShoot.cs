using UnityEngine;
using PurrNet.Prediction;

public class PlayerShoot : PredictedIdentity<PlayerShoot.ShootInput, PlayerShoot.ShootState>
{
    [SerializeField] private LayerMask _shootLayerMask;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private float roundsPerMinute = 200;
    [SerializeField] private int maxAmmo = 30;


    protected override void Simulate(ShootInput input, ref ShootState state, float delta)
    {
        RaycastHit hit;
        state.shootCooldown -= delta;

        if (input.shoot && state.shootCooldown <= 0)
        {
            // if (Physics.Raycast(transform.position, _playerMovement))
            Debug.Log("Shoot!");
            float cooldownTime = 1 / (roundsPerMinute / 60);
            state.shootCooldown = cooldownTime;
        }
    }

    protected override void ModifyExtrapolatedInput(ref ShootInput input)
    {
        input.shoot = false;
    }

    protected override void UpdateInput(ref ShootInput input)
    {
        input.shoot |= InputManager.Instance.fireAction.inProgress;
    }

    public struct ShootInput : IPredictedData
    {
        public bool shoot;

        public void Dispose() {}
    }

    public struct ShootState : IPredictedData<ShootState>
    {
        public int ammo;
        public float shootCooldown;

        public void Dispose() {}
    }
}
