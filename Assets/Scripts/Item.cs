using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Reference to the item ScriptableObject")]
    public ItemObject item;

    private void Start()
    {
        // If the item has a world model prefab, instantiate it as a child
        if (item != null && item.worldModelPrefab != null)
        {
            // GameObject model = Instantiate(item.worldModelPrefab, transform);
            // model.transform.localPosition = Vector3.zero;
            // model.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning($"Item '{name}' has no world model prefab assigned in {item}.");
        }
    }
}
