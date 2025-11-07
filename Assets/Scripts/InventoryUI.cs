using UnityEngine;
using StarterAssets;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] StarterAssetsInputs input;
    public static bool inventoryOpen = false;
    [SerializeField] InventoryObject inventoryObject;
    [SerializeField] GameObject slotPrefab;
    private List<GameObject> slotList;
    [SerializeField] GameObject itemPrefab;

    void Start()
    {
        //Debug.Log("WOKE");
        slotList = new List<GameObject>();
        inventoryObject.Reset();
        InstantiateSlots(inventoryObject.numStorageSlots);
        SetVisibility();
        inventoryObject.Subscribe(onInventoryChange);
    }

    // Creates the slot GameObjects for the inventory.
    void InstantiateSlots(int _numSlots)
    {
        for (int x = 0; x < _numSlots; x++)
        {
            GameObject newSlotObject = Instantiate(slotPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newSlotObject.GetComponent<SlotScript>().index = x;
            newSlotObject.transform.SetParent(transform, false);
            slotList.Add(newSlotObject);
        }
    }

    // This function is called every time the inventory has a change. For example, if the player picks up an item, or drags an item,
    // or drops an item. This function updates the actual item GameObjects after the inventory scriptable object has a change.
    void onInventoryChange()
    {
        for (int x = 0; x < slotList.Count; x++)
        {
            // First delete all of the item GameObjects.
            if (slotList[x].transform.childCount > 0)
            {
                for (int c = 0; c < slotList[x].transform.childCount; c++)
                {
                    Destroy(slotList[x].transform.GetChild(c).gameObject);
                }
            }

            // Then, recreate the item GameObjects if needed. Sorry this code looks bad. I should rewrite it later.
            if (!inventoryObject.Container[x].empty)
            {
                GameObject newItemGameObject = inventoryObject.Container[x].item.InstantiatePrefab();
                TMPro.TMP_Text amountText = newItemGameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>();
                newItemGameObject.GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
                amountText.enabled = inventoryOpen;
                amountText.text = inventoryObject.Container[x].amount.ToString();
                newItemGameObject.GetComponent<DraggableItem>().inventory = inventoryObject;
                newItemGameObject.transform.SetParent(slotList[x].transform, false);
            }
        }
        // Then set the visibility so if the inventory is closed, the inventory GUI and the items are invisible.
        SetVisibility();
    }

    public void Update()
    {
        if (input.inventoryOpen)
        {
            OnToggleInventory();
            input.inventoryOpen = false;
        }
    }

    // Sets the visibility of the inventory GUI based on the inventoryOpen bool. It sets the opacity of the objects instead of disabling them.
    // The reason it sets the opacity to 0 instead of disabling them is because I don't want the scripts attached to the GameObjects to be disabled.
    public void SetVisibility()
    {
        // Set the opacity of the inventory background and then loop through the slots
        GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
        for (int x = 0; x < slotList.Count; x++){
            slotList[x].GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
            if (slotList[x].transform.childCount > 0)
            {
                slotList[x].transform.GetChild(0).gameObject.GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
                slotList[x].transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().enabled = inventoryOpen;
            }
        }
    }

    // Called when the inventory key is pressed. Toggles inventory visibility and locks/unlocks cursor
    private void OnToggleInventory()
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
