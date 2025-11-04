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

    void Awake()
    {
        slotList = new List<GameObject>();
        inventoryObject.Reset();
        InstantiateSlots(inventoryObject.numStorageSlots);
        SetVisibility();
        inventoryObject.Subscribe(onInventoryChange);
    }

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

    void onInventoryChange()
    {
        for (int x = 0; x < slotList.Count; x++)
        {
            if (slotList[x].transform.childCount > 0)
            {
                Destroy(slotList[x].transform.GetChild(0).gameObject);
            }

            if (!inventoryObject.Container[x].empty)
            {
                GameObject newItemGameObject = inventoryObject.Container[x].item.InstantiatePrefab();
                newItemGameObject.GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
                newItemGameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().enabled = inventoryOpen;
                newItemGameObject.GetComponent<DraggableItem>().inventory = inventoryObject;
                newItemGameObject.transform.SetParent(slotList[x].transform, false);
            }
        }
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

    public void SetVisibility()
    {
        Debug.Log("visible! toggle");
        GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
        for (int x = 0; x < slotList.Count; x++){
            slotList[x].GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
            Debug.Log(slotList[x].transform.childCount);
            if (slotList[x].transform.childCount > 0)
            {
                slotList[x].transform.GetChild(0).gameObject.GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
                slotList[x].transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().enabled = inventoryOpen;
            }
        }
    }

    private void OnToggleInventory()
    {
        Debug.Log("INVEN");
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
