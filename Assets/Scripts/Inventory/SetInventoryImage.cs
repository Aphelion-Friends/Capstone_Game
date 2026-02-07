using UnityEngine;
using UnityEngine.UI;

public class SetInventoryImage : MonoBehaviour
{
    [SerializeField] private InventoryUIConfig config;

    void Awake()
    {
        GetComponent<Image>().sprite = config.backgroundImage;
        GetComponent<CanvasRenderer>().SetAlpha(0f);
    }
}
