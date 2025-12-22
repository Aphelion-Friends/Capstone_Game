using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using PurrNet;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Inventory Reference")]
    public InventoryObject inventory;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public LayerMask itemLayer;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI pickupPrompt;

    private Camera mainCam;
    private Item lookedAtItem;

    public Objective playerObjective;

    private void OnEnable()
    {
        // StartCoroutine(FindCameraWhenReady());
    }

    private IEnumerator FindCameraWhenReady()
    {
        while (mainCam == null)
        {
            mainCam = Camera.main;
            yield return null;
        }
        Debug.Log($"Player camera found: {mainCam.name}");
    }

    private void Update()
    {
        if (mainCam == null) return;
        DetectItemInFront();
        UpdatePromptUI();
    }

    private void DetectItemInFront()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.red);

        if (Physics.Raycast(ray, out hit, pickupRange, itemLayer))
        {
            Item item = hit.collider.GetComponent<Item>();
            lookedAtItem = item;
        }
        else
        {
            lookedAtItem = null;
        }
    }
    private void UpdatePromptUI()
    {
        if (pickupPrompt == null) return;

        if (lookedAtItem != null)
        {
            pickupPrompt.gameObject.SetActive(true);
            pickupPrompt.text = $"Press E to pick up <color=#FFD700>{lookedAtItem.item.displayName}</color>";
        }
        else
        {
            pickupPrompt.gameObject.SetActive(false);
        }
    }
    private void OnInteract()
    {
        Debug.Log("E pressed!");

        if (lookedAtItem == null)
        {
            Debug.Log("No item to pick up.");
            return;
        }

        inventory.AddItem(lookedAtItem.item, 1);
        Debug.Log("Picked up: " + lookedAtItem.name);
        ObjectiveManager.Instance.objective.ItemCollected(lookedAtItem.item.itemName);

        // DestroyItemForAll(lookedAtItem.gameObject);
        lookedAtItem = null;
    }

    // [ObserversRpc(bufferLast:true)]
    // void DestroyItemForAll(GameObject itemGameObject)
    // {
    //     Destroy(itemGameObject);
    //     Debug.Log("DESTROY");
    // }
}
