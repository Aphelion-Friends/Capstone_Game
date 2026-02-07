using System;
using UnityEngine;
using PurrNet.Prediction;

public class NetworkInventory : PredictedIdentity<NetworkInventory.InvInput, NetworkInventory.InvState>
{
    [SerializeField] private int _slotCount = 24;

    public event Action OnInventoryChanged;

    private bool _dirty;

    [Serializable]
    public struct Slot
    {
        public int itemId;
        public int amount;

        public bool IsEmpty => itemId == 0 || amount <= 0;
    }

    public struct InvState : IPredictedData<InvState>
    {
        public int slotCount;
        public Slot[] slots;

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
        return new InvState
        {
            slotCount = _slotCount,
            slots = new Slot[_slotCount]
        };
    }

    public int SlotCount => currentState.slotCount;

    public Slot GetSlot(int index)
    {
        if (index < 0 || index >= currentState.slotCount) return default;
        return currentState.slots[index];
    }

    public void RequestMoveOrSwap(int from, int to)
    {
        if (from == to) return;
        _pendingFrom = from;
        _pendingTo = to;
        _hasPending = true;
    }

    private bool _hasPending;
    private int _pendingFrom;
    private int _pendingTo;

    protected override void GetFinalInput(ref InvInput input)
    {
        if (!isOwner)
        {
            input.hasAction = false;
            return;
        }

        if (_hasPending)
        {
            input.hasAction = true;
            input.fromIndex = _pendingFrom;
            input.toIndex = _pendingTo;

            _hasPending = false;
        }
        else
        {
            input.hasAction = false;
        }
    }

    protected override void Simulate(InvInput input, ref InvState state, float delta)
    {
        if (!input.hasAction) return;

        if (!IsValid(state, input.fromIndex) || !IsValid(state, input.toIndex) || input.fromIndex == input.toIndex)
            return;

        var a = state.slots[input.fromIndex];
        var b = state.slots[input.toIndex];

        if (a.IsEmpty) return;

        if (!b.IsEmpty && b.itemId == a.itemId)
        {
            b.amount += a.amount;
            state.slots[input.toIndex] = b;
            state.slots[input.fromIndex] = default;
        }
        else
        {
            state.slots[input.fromIndex] = b;
            state.slots[input.toIndex] = a;
        }

        _dirty = true;
    }

    private bool IsValid(InvState s, int i) => i >= 0 && i < s.slotCount;

    private void LateUpdate()
    {
        if (_dirty)
        {
            _dirty = false;
            OnInventoryChanged?.Invoke();
        }
    }

    public void ServerAddItem(int itemId, int amount)
    {
 
        AddItem_Internal(ref currentState, itemId, amount);
        _dirty = true;
    }

    private void AddItem_Internal(ref InvState state, int itemId, int amount)
    {
        if (itemId == 0 || amount <= 0) return;

        for (int i = 0; i < state.slotCount; i++)
        {
            if (!state.slots[i].IsEmpty && state.slots[i].itemId == itemId)
            {
                var s = state.slots[i];
                s.amount += amount;
                state.slots[i] = s;
                return;
            }
        }

        for (int i = 0; i < state.slotCount; i++)
        {
            if (state.slots[i].IsEmpty)
            {
                state.slots[i] = new Slot { itemId = itemId, amount = amount };
                return;
            }
        }
    }
}
