using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public static bool inventoryOpen = false;

    [Header("Runtime Inventory (Networked)")]
    [SerializeField] private NetworkInventory inventory;

    [Header("Scriptable Object Database")]
    [SerializeField] private ItemDatabase itemDatabase;

    [Header("UI Prefabs")]
    [SerializeField] private GameObject slotPrefab;

    private readonly List<GameObject> slotList = new List<GameObject>();

    private void Awake()
    {
        Debug.Log("InventoryUI Awake");
    }

    private void Start()
    {
        if (inventory == null)
        {
            var all = FindObjectsByType<NetworkInventory>(FindObjectsSortMode.None);
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] != null && all[i].isOwner)
                {
                    inventory = all[i];
                    break;
                }
            }
        }

        if (inventory == null || !inventory.isOwner)
        {
            Debug.LogWarning("InventoryUI: Could not find owned NetworkInventory. Disabling UI.");
            gameObject.SetActive(false);
            return;
        }

        if (itemDatabase == null)
        {
            Debug.LogError("InventoryUI: ItemDatabase is not assigned.");
            gameObject.SetActive(false);
            return;
        }

        InstantiateSlots(inventory.SlotCount);

        inventory.OnInventoryChanged += OnInventoryChange;
        OnInventoryChange();
        SetVisibility();
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= OnInventoryChange;
    }

    private void Update()
    {
        if (InputManager.Instance == null)
            return;

        var action = InputManager.Instance.inventoryAction;
        if (action != null && action.WasPressedThisFrame())
        {
            ToggleInventory();
        }
    }

    private void InstantiateSlots(int numSlots)
    {
        for (int i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i] != null) Destroy(slotList[i]);
        }
        slotList.Clear();

        for (int x = 0; x < numSlots; x++)
        {
            GameObject newSlotObject = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            newSlotObject.GetComponent<SlotScript>().index = x;
            newSlotObject.transform.SetParent(transform, false);
            slotList.Add(newSlotObject);
        }
    }

    private void OnInventoryChange()
    {
        if (inventory == null) return;

        for (int x = 0; x < slotList.Count; x++)
        {
            GameObject slotGO = slotList[x];
            if (slotGO == null) continue;

            for (int c = slotGO.transform.childCount - 1; c >= 0; c--)
            {
                Destroy(slotGO.transform.GetChild(c).gameObject);
            }

            if (inventory.IsEmpty(x))
                continue;

            int itemId = inventory.GetItemId(x);
            int amount = inventory.GetAmount(x);

            ItemObject itemObj = itemDatabase.GetById(itemId);
            if (itemObj == null)
            {
                Debug.LogWarning($"InventoryUI: ItemDatabase has no item with id {itemId}.");
                continue;
            }

            GameObject itemUI = itemObj.InstantiatePrefab();

            var amountText = itemUI.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            amountText.text = amount.ToString();
            amountText.enabled = inventoryOpen;

            var drag = itemUI.GetComponent<DraggableItem>();
            drag.inventory = inventory;

            itemUI.transform.SetParent(slotGO.transform, false);

            itemUI.GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
        }

        SetVisibility();
    }

    private void SetVisibility()
    {
        GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);

        for (int x = 0; x < slotList.Count; x++)
        {
            var slotGO = slotList[x];
            if (slotGO == null) continue;

            slotGO.GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);

            if (slotGO.transform.childCount > 0)
            {
                var itemGO = slotGO.transform.GetChild(0).gameObject;
                itemGO.GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);

                var tmp = itemGO.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                tmp.enabled = inventoryOpen;
            }
        }
    }

    private void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        SetVisibility();

        if (inventoryOpen)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
