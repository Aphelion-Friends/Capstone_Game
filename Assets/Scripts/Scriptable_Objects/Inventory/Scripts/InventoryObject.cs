using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public Sprite backgroundImage;
    public int numStorageSlots;
    List<Action> onChange = new List<Action>();
    
    // Adds an item to the fist availible slot.
    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        int firstEmptySlot = 0;
        bool hasSpace = false;

        for(int x = 0; x < Container.Count; x++)
        {
            if (Container[x].empty && !hasSpace)
            {
                firstEmptySlot = x;
                hasSpace = true;
            }

            if (!Container[x].empty && Container[x].item == _item)
            {
                Container[x].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem && hasSpace)
        {
            Container[firstEmptySlot] = new InventorySlot(_item, _amount);
        }

        executeOnChange();
    }

    // Adds an item at the specified index. Return true if successful, false if not.
    public bool AddItemAtIndex(ItemObject _item, int _amount, int _index)
    {
        if (!Container[_index].empty && Container[_index].item != _item)
        {
            return false;
        }
        else if (Container[_index].empty)
        {
            Container[_index] = new InventorySlot(_item, _amount);
            executeOnChange();
            return true;
        }
        else if (!Container[_index].empty)
        {
            Container[_index].amount += _amount;
            executeOnChange();
            return true;
        }
        return false;
    }

    // Removes one or more items at the specified index.
    public bool RemoveItemAtIndex(int _amount, int _index)
    {
        if (Container[_index].empty)
        {
            return false;
        }
        else if (Container[_index].amount - _amount < 0)
        {
            return false;
        }
        else
        {
            Container[_index].amount -= _amount;

            if (Container[_index].amount == 0)
            {
                Container[_index].empty = true;
                Container[_index].item = null;
            }
            
            executeOnChange();
            return true;
        }
    }

    // Returns the number of items at the specified index
    public int GetItemAmountAtIndex(int _index)
    {
        return Container[_index].amount;
    }

    public ItemObject GetItemAtIndex(int _index)
    {
        return Container[_index].item;
    }

    // Completely resets the inventory and the onChange list.
    // You can call this function at the very start of the game when you want the inventory to be completely refreshed.
    public void Reset()
    {
        ClearInventory();
        onChange.Clear();
    }

    // Removes all the items from the inventory. Unlike Reset(), this does not clear the onChange list.
    public void ClearInventory()
    {
        Container.Clear();
        for (int x = 0; x < numStorageSlots; x++)
        {
            Container.Add(new InventorySlot());
        }
        executeOnChange();
    }

    // Calls all the functions in the onChange list.
    void executeOnChange()
    {
        for (int x = 0; x < onChange.Count; x++)
        {
            onChange[x]();
        }
    }

    // Adds a new function to the onChange list.
    public void Subscribe(Action func)
    {
        onChange.Add(func);
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public bool empty;

    public InventorySlot()
    {
        empty = true;
    }

    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
        empty = false;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
