using UnityEngine;
using PurrNet.Prediction;

public class PlayerShoot : PredictedIdentity<PlayerShoot.ShootInput, PlayerShoot.ShootState>
{
    [SerializeField] private LayerMask _shootLayerMask;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private int roundsPerMinute = 500;
    [SerializeField] private int maxAmmo = 30;



    protected override void Simulate(ShootInput input, ref ShootState state, float delta)
    {
        RaycastHit hit;
        state.shootCooldown -= delta;

        if (input.reload)
        {
            state.ammo = maxAmmo;
        }

        if (input.shoot && state.shootCooldown <= 0 && state.ammo > 0)
        {
            Debug.Log("Shoot!");
            // if (Physics.Raycast(_playerMovement, currentInput.cameraForward, out hit, Mathf.Infinity, _shootLayerMask))
            // {
            //     Debug.Log(hit.transform.name);
            // }
            float cooldownTime = 1 / (roundsPerMinute / 60);
            state.shootCooldown = cooldownTime;
            state.ammo--;
        }
    }

    protected override void ModifyExtrapolatedInput(ref ShootInput input)
    {
        input.shoot = false;
        input.reload = false;
    }

    protected override void UpdateInput(ref ShootInput input)
    {
        input.shoot |= InputManager.Instance.fireAction.inProgress;
        input.reload |= InputManager.Instance.reloadAction.inProgress;
    }

    protected override ShootState GetInitialState()
    {
        return new ShootState
        {
            ammo = maxAmmo,
            shootCooldown = 0,
        };
    }

    public struct ShootInput : IPredictedData
    {
        public bool shoot;
        public bool reload;

        public void Dispose() {}
    }

    public struct ShootState : IPredictedData<ShootState>
    {
        public int ammo;
        public float shootCooldown;

        public void Dispose() {}
    }
}
