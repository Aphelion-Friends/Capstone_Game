using UnityEngine;
using UnityEngine.UI;

public class SetInventoryImage : MonoBehaviour
{
    [SerializeField] InventoryObject inventoryObject;

    void Awake()
    {
        GetComponent<Image>().sprite = inventoryObject.backgroundImage;
        GetComponent<CanvasRenderer>().SetAlpha(0f);
    }
}
