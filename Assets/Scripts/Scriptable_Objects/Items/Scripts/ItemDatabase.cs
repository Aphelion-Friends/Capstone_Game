using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory System/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public ItemObject[] items;

    public ItemObject GetById(int id)
    {
        if (id == 0) return null;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemId == id)
                return items[i];
        }
        return null;
    }
}
