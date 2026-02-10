using UnityEngine;
using UnityEngine.UI;

public class SetInventoryImage : MonoBehaviour
{
    [SerializeField] private InventoryUIConfig config;

    void Awake()
    {
        var img = GetComponent<Image>();
        img.sprite = config.backgroundImage;
    }
}
