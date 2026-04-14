using System;
using UnityEngine;
using PurrNet.Prediction;

public class NetworkInventory : PredictedIdentity<NetworkInventory.InvInput, NetworkInventory.InvState>
{
    [SerializeField] private int _slotCount = 24;

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

    // I don't know if this is the best solution
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

    // I don't know why this is here
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
    }

    protected override void UpdateInput(ref InvInput input)
    {
        if (_hasPending && !input.hasAction)
        {
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
    //     if (!input.hasAction) return;

    //     int from = input.fromIndex;
    //     int to = input.toIndex;

    //     if (!IsValid(state, from) || !IsValid(state, to) || from == to)
    //         return;

    //     int aId = state.itemIds[from];
    //     int aAmt = state.amounts[from];

    //     if (aId == 0 || aAmt <= 0)
    //         return;

    //     int bId = state.itemIds[to];
    //     int bAmt = state.amounts[to];

    //     if (bId != 0 && bAmt > 0 && bId == aId)
    //     {
    //         state.amounts[to] = bAmt + aAmt;
    //         state.itemIds[from] = 0;
    //         state.amounts[from] = 0;
    //     }
    //     else
    //     {
    //         state.itemIds[from] = bId;
    //         state.amounts[from] = bAmt;

    //         state.itemIds[to] = aId;
    //         state.amounts[to] = aAmt;
    //     }

    //     _dirty = true;
    }

    private bool IsValid(InvState s, int i) => i >= 0 && i < s.slotCount;

    // private void LateUpdate()
    // {
    //     if (_dirty)
    //     {
    //         _dirty = false;
    //         OnInventoryChanged?.Invoke();
    //     }
    // }

    // You suppoedsded to use this to add an item to the first inventory slot
    // public bool AddItem(int itemId)
    // {
    //     bool ableToAddItem = false;

    //     for(int i = 0; i < currentState.slotCount && !ableToAddItem; i++)
    //     {
    //         // -1 means no item
    //         if (currentState.itemIds[i] == -1)
    //         {
    //             currentState.itemIds[i] = itemId;
    //         }
    //     }
    // }

    public bool AddItem(int itemId, int amount)
    {
        if (itemId < 0 || amount <= 0) return false;

        for (int i = 0; i < currentState.slotCount; i++)
        {
            if (currentState.itemIds[i] == itemId && currentState.amounts[i] > 0)
            {
                currentState.amounts[i] += amount;
                // _dirty = true;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        for (int i = 0; i < currentState.slotCount; i++)
        {
            // less than 1 means no item
            if (currentState.itemIds[i] == -1 || currentState.amounts[i] <= 0)
            {
                currentState.itemIds[i] = itemId;
                currentState.amounts[i] = amount;
                // _dirty = true;
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
