using UnityEngine;
using PurrNet.Prediction;

public class PlayerHeal : PredictedIdentity<PlayerHeal.HealInput, PlayerHeal.HealState>
{
    [Header("References")]
    [SerializeField] private NetworkInventory inventory;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private ItemObject medkitItem;

    [Header("Heal Settings")]
    [SerializeField] private float healAmount = 25f;

    private MultiAudioSource healAudio;

    public struct HealInput : IPredictedData
    {
        public bool heal;
        public void Dispose() { }
    }

    public struct HealState : IPredictedData<HealState>
    {
        public bool usedHeal;
        public void Dispose() { }
    }

    protected override void LateAwake()
    {
        if (inventory == null)
        {
            inventory = GetComponent<NetworkInventory>();
        }

        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        healAudio = MultiAudioSource.FromResource(this.gameObject, "heal");
    }

    protected override void UpdateInput(ref HealInput input)
    {
        input.heal |= InputManager.Instance.healAction.inProgress;
    }

    protected override void ModifyExtrapolatedInput(ref HealInput input)
    {
        input.heal = false;
    }

    protected override void Simulate(HealInput input, ref HealState state, float delta)
    {
        if (input.heal && !state.usedHeal)
        {
            state.usedHeal = true;

            if (playerHealth == null || inventory == null || medkitItem == null)
            {
                return;
            }

            if (playerHealth.IsDead)
            {
                return;
            }

            if (playerHealth.CurrentHealth >= playerHealth.MaxHealth)
            {
                return;
            }

            bool removedMedkit = inventory.TryRemoveItem(medkitItem.itemId, 1);

            if (removedMedkit)
            {
                playerHealth.ChangeHealth(healAmount);
                healAudio.PlayOnlyIfDone();
                Debug.Log($"Used medkit. Healed {healAmount} HP.");
            }
        }
        else if (!input.heal)
        {
            state.usedHeal = false;
        }
    }
}