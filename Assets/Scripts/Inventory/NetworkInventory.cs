using System;
using UnityEngine;
using PurrNet.Prediction;

public class NetworkInventory : PredictedIdentity<NetworkInventory.InvInput, NetworkInventory.InvState>
{
    [SerializeField] private int _slotCount = 24;

    public event Action OnInventoryChanged;

    private bool _dirty;

    public struct InvState : IPredictedData<InvState>
    {
        public int slotCount;

        public int[] itemIds;
        public int[] amounts;

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
            itemIds = new int[_slotCount],
            amounts = new int[_slotCount],
        };
    }

    public int SlotCount => currentState.slotCount;

    public bool IsEmpty(int index)
    {
        if (index < 0 || index >= currentState.slotCount) return true;
        return currentState.itemIds[index] == 0 || currentState.amounts[index] <= 0;
    }

    public int GetItemId(int index)
    {
        if (index < 0 || index >= currentState.slotCount) return 0;
        return currentState.itemIds[index];
    }

    public int GetAmount(int index)
    {
        if (index < 0 || index >= currentState.slotCount) return 0;
        return currentState.amounts[index];
    }

    private bool _hasPending;
    private int _pendingFrom;
    private int _pendingTo;

    public void RequestMoveOrSwap(int from, int to)
    {
        if (from == to) return;
        _pendingFrom = from;
        _pendingTo = to;
        _hasPending = true;
    }

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

        int from = input.fromIndex;
        int to = input.toIndex;

        if (!IsValid(state, from) || !IsValid(state, to) || from == to)
            return;

        int aId = state.itemIds[from];
        int aAmt = state.amounts[from];

        if (aId == 0 || aAmt <= 0)
            return;

        int bId = state.itemIds[to];
        int bAmt = state.amounts[to];

        if (bId != 0 && bAmt > 0 && bId == aId)
        {
            state.amounts[to] = bAmt + aAmt;
            state.itemIds[from] = 0;
            state.amounts[from] = 0;
        }
        else
        {
            state.itemIds[from] = bId;
            state.amounts[from] = bAmt;

            state.itemIds[to] = aId;
            state.amounts[to] = aAmt;
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
        if (itemId == 0 || amount <= 0) return;
        for (int i = 0; i < currentState.slotCount; i++)
        {
            if (currentState.itemIds[i] == itemId && currentState.amounts[i] > 0)
            {
                currentState.amounts[i] += amount;
                _dirty = true;
                return;
            }
        }

        for (int i = 0; i < currentState.slotCount; i++)
        {
            if (currentState.itemIds[i] == 0 || currentState.amounts[i] <= 0)
            {
                currentState.itemIds[i] = itemId;
                currentState.amounts[i] = amount;
                _dirty = true;
                return;
            }
        }
    }
}
