using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using PurrNet;

public class Player : NetworkIdentity
{
    [Header("Inventory Reference")]
    public InventoryObject inventory;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public LayerMask itemLayer;

    private Camera mainCam;
    private Item lookedAtItem;

    private void OnEnable()
    {
        StartCoroutine(FindCameraWhenReady());
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

        DestroyItemForAll(lookedAtItem.gameObject);
        lookedAtItem = null;
    }

    [ObserversRpc(bufferLast:true)]
    void DestroyItemForAll(GameObject itemGameObject)
    {
        Destroy(itemGameObject);
        Debug.Log("DESTROY");
    }
}
