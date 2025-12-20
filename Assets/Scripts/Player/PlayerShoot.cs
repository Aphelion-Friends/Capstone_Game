using UnityEngine;
using PurrNet.Prediction;

public class PlayerShoot : PredictedIdentity<PlayerShoot.ShootInput, PlayerShoot.ShootState>
{
    [SerializeField] private LayerMask _shootLayerMask;
    [SerializeField] private PlayerMovement _playerMovement;

    protected override void Simulate(ShootInput input, ref ShootState state, float delta)
    {
        RaycastHit hit;
        if (input.shoot)
        {
            // if (Physics.Raycast(transform.position, _playerMovement))
            Debug.Log("Shoot!");
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

        public void Dispose() {}
    }
}
