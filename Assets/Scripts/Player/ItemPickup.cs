using UnityEngine;
using PurrNet.Prediction;

public class ItemPickup : PredictedIdentity<ItemPickup.ItemPickupInput, ItemPickup.ItemPickupState>
{
    public struct ItemPickupState : IPredictedData<ItemPickupState>
    {
        public bool pickedUp;

        public void Dispose() {}
    }

    public struct ItemPickupInput : IPredictedData
    {
        public bool pickup;

        public void Dispose() {}
    }

    protected override void UpdateInput(ref ItemPickupInput input)
    {
        input.pickup |= InputManager.Instance.interactAction.inProgress;
    }

    protected override void ModifyExtrapolatedInput(ref ItemPickupInput input)
    {
        input.pickup = false;
    }

    private ItemDetection itemDetection;
    protected override void LateAwake()
    {
        itemDetection = GetComponent<ItemDetection>();
    }

    protected override void Simulate(ItemPickupInput input, ref ItemPickupState state, float delta)
    {
        if (input.pickup && !state.pickedUp)
        {
            state.pickedUp = true;
            PredictedObjectID? itemToPickUp = itemDetection.currentState.lookedAtItem;

            if (itemToPickUp.HasValue)
            {
                Debug.Log($"Item pickup: {itemToPickUp.Value.GetGameObject(predictionManager).name}");
                predictionManager.hierarchy.Delete(itemToPickUp);
            }
        }
        else if (!input.pickup)
            state.pickedUp = false;
    }
}
