using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public static bool inventoryOpen = false;

    [SerializeField] private NetworkInventory inventory;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private CanvasGroup canvasGroup;

    private readonly List<GameObject> slotList = new List<GameObject>();

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (inventory == null)
            inventory = GetComponentInParent<NetworkInventory>();
    }

    private void OnEnable()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private IEnumerator Start()
    {
        if (itemDatabase == null || slotPrefab == null)
            yield break;

        float timeout = 10f;
        while (InputManager.Instance == null && timeout > 0f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        if (InputManager.Instance == null)
            yield break;

        var action = InputManager.Instance.inventoryAction;
        if (action == null)
            yield break;

        if (!action.enabled)
            action.Enable();

        timeout = 10f;
        while ((inventory == null || !inventory.isOwner) && timeout > 0f)
        {
            if (inventory == null)
                inventory = GetComponentInParent<NetworkInventory>();

            if (inventory == null)
            {
                var all = FindObjectsByType<NetworkInventory>(FindObjectsSortMode.None);
                foreach (var inv in all)
                {
                    if (inv != null && inv.isOwner)
                    {
                        inventory = inv;
                        break;
                    }
                }
            }

            timeout -= Time.deltaTime;
            yield return null;
        }

        if (inventory == null || !inventory.isOwner)
        {
            gameObject.SetActive(false);
            yield break;
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
        if (action == null)
            return;

        if (action.WasPressedThisFrame())
            ToggleInventory();
    }

    private void InstantiateSlots(int numSlots)
    {
        for (int i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i] != null)
                Destroy(slotList[i]);
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
        if (inventory == null)
            return;

        for (int x = 0; x < slotList.Count; x++)
        {
            GameObject slotGO = slotList[x];
            if (slotGO == null)
                continue;

            for (int c = slotGO.transform.childCount - 1; c >= 0; c--)
                Destroy(slotGO.transform.GetChild(c).gameObject);

            if (inventory.IsEmpty(x))
                continue;

            int itemId = inventory.GetItemId(x);
            int amount = inventory.GetAmount(x);

            ItemObject itemObj = itemDatabase.GetById(itemId);
            if (itemObj == null)
                continue;

            GameObject itemUI = itemObj.InstantiatePrefab();

            var amountText = itemUI.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            amountText.text = amount.ToString();
            amountText.enabled = inventoryOpen;

            var drag = itemUI.GetComponent<DraggableItem>();
            drag.inventory = inventory;

            itemUI.transform.SetParent(slotGO.transform, false);
        }

        SetVisibility();
    }

    private void SetVisibility()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = inventoryOpen ? 1f : 0f;
            canvasGroup.interactable = inventoryOpen;
            canvasGroup.blocksRaycasts = inventoryOpen;
        }

        for (int x = 0; x < slotList.Count; x++)
        {
            var slotGO = slotList[x];
            if (slotGO == null)
                continue;

            if (slotGO.transform.childCount > 0)
            {
                var itemGO = slotGO.transform.GetChild(0).gameObject;
                var tmp = itemGO.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                tmp.enabled = inventoryOpen;
            }
        }
    }

    private void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        SetVisibility();

        Cursor.lockState = inventoryOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = inventoryOpen;
    }
}
