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
    public GameObject prefab;
    public Sprite texture;
    public ItemType type;

    [TextArea(15, 20)]
    public string description;

    public GameObject InstantiatePrefab()
    {
        GameObject newGameObj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        newGameObj.GetComponent<Image>().sprite = texture;

        return newGameObj;
    }
}
