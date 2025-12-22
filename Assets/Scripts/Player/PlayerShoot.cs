using UnityEngine;
using PurrNet.Prediction;

public class PlayerShoot : PredictedIdentity<PlayerShoot.ShootInput, PlayerShoot.ShootState>
{
    [SerializeField] private LayerMask _shootLayerMask;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private float roundsPerMinute = 300;
    [SerializeField] private int _maxAmmo = 30;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private GunEffects gunEffects;

    [SerializeField] private Transform shootOrigin;

    private PredictedEvent _onShoot;

    protected override void LateAwake()
    {
        base.LateAwake();
        _onShoot  = new PredictedEvent(predictionManager, this);
        _onShoot.AddListener(OnShootEvent);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _onShoot.RemoveListener(OnShootEvent);
    }

    private void OnShootEvent()
    {
        gunEffects.PlayEffects();
    }

    protected override void Simulate(ShootInput input, ref ShootState state, float delta)
    {
        state.shootCooldown -= delta;

        if (input.reload)
        {
            state.ammo = _maxAmmo;
        }

        if (input.shoot && state.shootCooldown <= 0 && state.ammo > 0)
        {
            
            Shoot();
            float cooldownTime = 1 / (roundsPerMinute / 60);
            state.shootCooldown = cooldownTime;
            state.ammo--;
        }
    }

    private void Shoot()
    {
        _onShoot?.Invoke();

        RaycastHit hit;

        if (Physics.Raycast(shootOrigin.position, _playerMovement.currentInput.cameraForward, out hit, Mathf.Infinity, _shootLayerMask))
        {
            if(hit.transform.TryGetComponent(out PlayerHealth playerHealth))
                playerHealth.ChangeHealth(-_damage);
            else if(hit.transform.TryGetComponent(out EnemyHealth enemyHealth))
                enemyHealth.ChangeHealth(-_damage);
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
            ammo = _maxAmmo,
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

        public override string ToString()
        {
            return $"Ammo: {ammo}\nCooldown: {shootCooldown}";
        }

        public void Dispose() {}
    }
}
