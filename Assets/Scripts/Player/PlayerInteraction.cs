using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera mainCam;
    private Item lookedAtItem;

    [Header("Inventory Reference")]
    public InventoryObject inventory;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI pickupPrompt;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public LayerMask itemLayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
