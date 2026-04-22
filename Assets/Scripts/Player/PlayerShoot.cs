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
    [SerializeField] private GunRecoil gunRecoil;

    [SerializeField] private Transform shootOrigin;

    [Header("Inventory + Items Stuff")]
    [SerializeField] private NetworkInventory inventory;
    [SerializeField] private ItemObject ammoItem;

    [Header("Reload Settings")]
    [SerializeField] private float reloadDuration = 1.5f;

    private PredictedEvent _onShoot;
    private MultiAudioSource reloadAudio;

    public int CurrentAmmo => currentState.ammo;
    public int ReserveAmmo => inventory != null ? inventory.GetTotalAmount(ammoItem.itemId) : 0;
    public int MaxAmmo => _maxAmmo;

    protected override void LateAwake()
    {
        base.LateAwake();

        _onShoot = new PredictedEvent(predictionManager, this);
        _onShoot.AddListener(OnShootEvent);

        reloadAudio = MultiAudioSource.FromResource(this.gameObject, "Reload");

        RegisterToAmmoUI();
    }

    private void Start()
    {
        RegisterToAmmoUI();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _onShoot.RemoveListener(OnShootEvent);
    }

    private void RegisterToAmmoUI()
    {
        if (!isOwner)
            return;

        AmmoUI ammoUI = FindFirstObjectByType<AmmoUI>();

        if (ammoUI != null)
        {
            ammoUI.SetPlayer(this);
        }
    }

    private void OnShootEvent()
    {
        gunEffects.PlayEffects();
        if (gunRecoil != null)
        {
            gunRecoil.Recoil();
        }
    }

    protected override void Simulate(ShootInput input, ref ShootState state, float delta)
    {
        state.shootCooldown -= delta;
        state.reloadTimer -= delta;

        if (input.reload && state.reloadTimer <= 0f)
        {
            int neededAmmo = _maxAmmo - state.ammo;

            if (neededAmmo > 0)
            {
                int availableAmmo = inventory.GetTotalAmount(ammoItem.itemId);

                if (availableAmmo > 0)
                {
                    state.reloadTimer = reloadDuration;

                    reloadAudio.PlayOnlyIfDone();
                }
            }
        }

        if (state.reloadTimer <= 0f && state.wasReloading)
        {
            int neededAmmo = _maxAmmo - state.ammo;

            if (neededAmmo > 0)
            {
                int availableAmmo = inventory.GetTotalAmount(ammoItem.itemId);
                int ammoToLoad = Mathf.Min(neededAmmo, availableAmmo);

                if (ammoToLoad > 0)
                {
                    bool removedAmmo = inventory.TryRemoveItem(ammoItem.itemId, ammoToLoad);

                    if (removedAmmo)
                        state.ammo += ammoToLoad;
                }
            }
        }

        if (input.shoot && state.shootCooldown <= 0 && state.ammo > 0 && state.reloadTimer <= 0f && !input.sprint)
        {
            Shoot();

            float cooldownTime = 1 / (roundsPerMinute / 60f);
            state.shootCooldown = cooldownTime;
            state.ammo--;
        }

        state.wasReloading = state.reloadTimer > 0f;
    }

    private void Shoot()
    {
        _onShoot?.Invoke();

        RaycastHit hit;

        if (Physics.Raycast(
            shootOrigin.position,
            _playerMovement.currentInput.cameraForward,
            out hit,
            Mathf.Infinity,
            _shootLayerMask))
        {
            if (hit.transform.TryGetComponent(out PlayerHealth playerHealth))
                playerHealth.ChangeHealth(-_damage);
            else if (hit.transform.TryGetComponent(out EnemyHealth enemyHealth))
                enemyHealth.ChangeHealth(-_damage);
        }
    }

    protected override void ModifyExtrapolatedInput(ref ShootInput input)
    {
        input.shoot = false;
        input.reload = false;
        input.sprint = false;
    }

    protected override void UpdateInput(ref ShootInput input)
    {
        input.shoot |= InputManager.Instance.fireAction.inProgress;
        input.reload |= InputManager.Instance.reloadAction.inProgress;
        input.sprint |= InputManager.Instance.sprintAction.inProgress;
    }

    protected override ShootState GetInitialState()
    {
        return new ShootState
        {
            ammo = 0,
            shootCooldown = 0,
            reloadTimer = 0,
            wasReloading = false
        };
    }

    public struct ShootInput : IPredictedData
    {
        public bool shoot;
        public bool reload;
        public bool sprint;
        public void Dispose() { }
    }

    public struct ShootState : IPredictedData<ShootState>
    {
        public int ammo;
        public float shootCooldown;

        public float reloadTimer;
        public bool wasReloading;

        public override string ToString()
        {
            return $"Ammo: {ammo}\nCooldown: {shootCooldown}\nReloadTimer: {reloadTimer}";
        }

        public void Dispose() { }
    }
}