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
    private NetworkInventory inventory;
    protected override void LateAwake()
    {
        itemDetection = GetComponent<ItemDetection>();
        inventory = GetComponent<NetworkInventory>();
    }

    protected override void Simulate(ItemPickupInput input, ref ItemPickupState state, float delta)
    {
        if (input.pickup && !state.pickedUp)
        {
            state.pickedUp = true;
            PredictedObjectID? itemToPickUp = itemDetection.currentState.lookedAtItem;

            if (itemToPickUp.HasValue)
            {
                GameObject pickedUpObject = itemToPickUp.Value.GetGameObject(predictionManager);

                if (pickedUpObject == null)
                    return;

                InWorldItem inWorldItem = pickedUpObject.GetComponent<InWorldItem>();

                if (inWorldItem == null || inWorldItem.item == null)
                    return;

                ItemObject item = inWorldItem.item;

                Debug.Log($"Item pickup: {pickedUpObject.name}");
                Debug.Log($"Adding {item.pickupAmount} of item ID {item.itemId}");

                bool canCollect = inventory.AddItem(item.itemId, item.pickupAmount);

                if (canCollect)
                    predictionManager.hierarchy.Delete(itemToPickUp);
            }
        }
        else if (!input.pickup)
        {
            state.pickedUp = false;
        }
    }
}
