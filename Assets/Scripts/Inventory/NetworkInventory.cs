using System;
using UnityEngine;
using PurrNet.Prediction;

public class NetworkInventory : PredictedIdentity<NetworkInventory.InvInput, NetworkInventory.InvState>
{
    [SerializeField] private int _slotCount = 24;
    private ItemDetection itemDetection;

    protected override void LateAwake()
    {
        itemDetection = GetComponent<ItemDetection>();
    }

    public event Action OnInventoryChanged;

    // private bool _dirty;

    public struct InvState : IPredictedData<InvState>
    {
        public int slotCount;

        public int[] itemIds;
        public int[] amounts;

        public override string ToString()
        {
            string displayString = "Inventory State:\n";
            for (int i = 0; i < slotCount; i++)
            {
                displayString += $"{i}: [${itemIds[i]}](${amounts[i]})\n";
            }
            return displayString;
        }

        public void Dispose() { }
    }

    public struct InvInput : IPredictedData
    {
        public bool hasItemPickup;
        public int pickupItemID;
        public int pickupItemAmount;

        public bool hasAction;
        public int fromIndex;
        public int toIndex;

        public void Dispose() { }
    }

    protected override InvState GetInitialState()
    {
        InvState newInv = new InvState
        {
            slotCount = _slotCount,
            itemIds = new int[_slotCount],
            amounts = new int[_slotCount],
        };

        for (int i = 0; i < _slotCount; i++)
        {
            newInv.itemIds[i] = -1;
            newInv.amounts[i] = 0;
        }

        return newInv;
    }

    public int SlotCount => currentState.slotCount;

    public bool IsEmpty(int index)
    {
        if (index < 0 || index >= currentState.slotCount) return true;
        return currentState.itemIds[index] < 0 || currentState.amounts[index] <= 0;
    }

    public int GetItemId(int index)
    {
        if (index < 0 || index >= currentState.slotCount) return -1;
        return currentState.itemIds[index];
    }

    public int GetAmount(int index)
    {
        if (index < 0 || index >= currentState.slotCount) return -1;
        return currentState.amounts[index];
    }
    public int GetTotalAmount(int itemId)
    {
        int total = 0;

        for (int i = 0; i < currentState.slotCount; i++)
        {
            if (currentState.itemIds[i] == itemId && currentState.amounts[i] > 0)
            {
                total += currentState.amounts[i];
            }
        }

        return total;
    }
    // I don't know if this is the best solution
    // Seems like it's working though
    private bool _hasPending;
    private int _pendingFrom;
    private int _pendingTo;

    public void RequestMoveOrSwap(int from, int to)
    {
        if (from == to) return;
        Debug.Log("Requesting move!");
        _pendingFrom = from;
        _pendingTo = to;
        _hasPending = true;

        Debug.Log("MOVING!");
    }

    // I don't know why this is here keeping it just in case
    protected override void GetFinalInput(ref InvInput input)
    {
        // if (!isOwner)
        // {
        //     input.hasAction = false;
        //     return;
        // }

        // if (_hasPending)
        // {
        //     input.hasAction = true;
        //     input.fromIndex = _pendingFrom;
        //     input.toIndex = _pendingTo;
        //     _hasPending = false;
        // }
        // else
        // {
        //     input.hasAction = false;
        // }
        input.hasItemPickup = InputManager.Instance.interactAction.inProgress;
        // Re-resolve the item from ItemDetection's state here
        PredictedObjectID? lookedAt = itemDetection.currentState.lookedAtItem;
        if (lookedAt.HasValue)
        {
            GameObject obj = lookedAt.Value.GetGameObject(predictionManager);
            ItemObject item = obj?.GetComponent<InWorldItem>()?.item;
            if (item != null)
            {
                Debug.Log("ITEM PICKPPEIPIPIP");
                input.pickupItemID = item.itemId;
                input.pickupItemAmount = item.pickupAmount;
            }
            else
            {
                input.hasItemPickup = false;
            }
        }
        else
        {
            input.hasItemPickup = false;
        }
        // if (_hasItemPickup && !input.hasItemPickup)
        // {
        //     input.hasItemPickup = true;
        //     _hasItemPickup = false;

        //     input.pickupItemID = _itemPickupID;
        //     input.pickupItemAmount = _itemPickupAmount;
        // }
    }

    protected override void UpdateInput(ref InvInput input)
    {
        if (_hasPending && !input.hasAction)
        {
            Debug.Log($"Now UPDATING input! From: {_pendingFrom}. To: {_pendingTo}");
            input.fromIndex = _pendingFrom;
            input.toIndex = _pendingTo;
            input.hasAction = true;
            _hasPending = false;
        }

    }

    protected override void Simulate(InvInput input, ref InvState state, float delta)
    {
        // We move the item now if the user is trying to move an item
        if (input.hasAction)
        {
            int from = input.fromIndex;
            int to = input.toIndex;

            Debug.Log($"MOVING in Simulate From: {from}, To: {to}");

            // Swap the ID and amount of each slot
            int tempFromID = state.itemIds[from];
            int tempFromAmount = state.amounts[from];

            state.itemIds[from] = state.itemIds[to];
            state.amounts[from] = state.amounts[to];
            state.itemIds[to] = tempFromID;
            state.amounts[to] = tempFromAmount;

            input.hasAction = false;
        }

        if (input.hasItemPickup)
        {
            Debug.Log("Item pickup SIMULATE");
            AddItem(input.pickupItemID, input.pickupItemAmount, ref state);
            input.hasItemPickup = false;
        }
    }

    private bool IsValid(InvState s, int i) => i >= 0 && i < s.slotCount;

    private bool _hasItemPickup;
    private int _itemPickupID;
    private int _itemPickupAmount;
    public void AddItemWithInput(int itemId, int amount)
    {
        _itemPickupID = itemId;
        _itemPickupAmount = amount;
        _hasItemPickup = true;
    }

    public bool AddItem(int itemId, int amount)
    {
        return AddItem(itemId, amount, ref currentState);
    }

    public bool AddItem(int itemId, int amount, ref InvState state)
    {
        if (itemId < 0 || amount <= 0) return false;

        for (int i = 0; i < state.slotCount; i++)
        {
            if (state.itemIds[i] == itemId && state.amounts[i] > 0)
            {
                state.amounts[i] += amount;
                // _dirty = true;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        for (int i = 0; i < state.slotCount; i++)
        {
            // less than 1 means no item
            if (state.itemIds[i] == -1 || state.amounts[i] <= 0)
            {
                state.itemIds[i] = itemId;
                state.amounts[i] = amount;
                // _dirty = true;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool TryRemoveItem(int itemId, int amount)
    {
        for (int i = 0; i < currentState.slotCount; i++)
        {
            if (currentState.itemIds[i] == itemId && currentState.amounts[i] >= amount)
            {
                currentState.amounts[i] -= amount;
                if (currentState.amounts[i] <= 0)
                    currentState.itemIds[i] = -1;

                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public void EditorInitForTests(int slotCount = 24)
    {
        currentState = new InvState
        {
            slotCount = slotCount,
            itemIds = new int[slotCount],
            amounts = new int[slotCount]
        };
    }
}
