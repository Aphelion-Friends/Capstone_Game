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

    public bool AddItemAtIndex(ItemObject _item, int _amount, int _index)
    {
        if (!Container[_index].empty && Container[_index].item != _item)
        {
            return false;
        }
        else if (Container[_index].empty)
        {
            Container[_index] = new InventorySlot(_item, _amount);
            return true;
        }
        else if (!Container[_index].empty)
        {
            Container[_index].amount += _amount;
            return true;
        }
        return false;
    }

    public void Reset()
    {
        ClearInventory();
        onChange.Clear();
    }

    public void ClearInventory()
    {
        Container.Clear();
        for (int x = 0; x < numStorageSlots; x++)
        {
            Container.Add(new InventorySlot());
        }
        executeOnChange();
    }

    void executeOnChange()
    {
        for (int x = 0; x < onChange.Count; x++)
        {
            onChange[x]();
        }
    }

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
