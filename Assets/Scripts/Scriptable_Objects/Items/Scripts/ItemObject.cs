using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Gun,
    QuestItem,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    [Header("Networking")]
    [Tooltip("Stable ID for this Item. Must be unique for all ItemObjects")]
    public int itemId;

    [Header("UI Settings")]
    public GameObject prefab;
    public Sprite texture;

    [Header("3D Model Settings")]
    public GameObject worldModelPrefab;

    public ItemType type;

    [TextArea(15, 20)]
    public string description;
    public Name itemName;
    public string displayName;

    public enum Name
    {
        SpiderAss,
        Gun
    }

    public GameObject InstantiatePrefab()
    {
        GameObject newGameObj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        newGameObj.GetComponent<Image>().sprite = texture;
        newGameObj.GetComponent<ItemScript>().itemObject = this;

        return newGameObj;
    }

    public GameObject InstantiateWorldModel(Vector3 position, Quaternion rotation)
    {
        if (worldModelPrefab == null)
        {
            Debug.LogWarning($"No world model prefab assigned for {name}!");
            return null;
        }

        GameObject newWorldObj = Instantiate(worldModelPrefab, position, rotation);
        return newWorldObj;
    }
}
