using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    // [Header("References")]
    // public GameObject inventoryPanel;

    [SerializeField] StarterAssetsInputs input;
    public static bool inventoryOpen = false;
    [SerializeField] InventoryObject inventoryObject;
    [SerializeField] GameObject slotPrefab;
    private List<GameObject> slotList;

    // private void Awake()
    // {
    //     inputActions = new InputSystem_Actions();
    // }

    // private void OnEnable()
    // {
    //     inputActions.UI.ToggleInventory.performed += OnToggleInventory;
    //     inputActions.UI.Enable();
    // }

    // private void OnDisable()
    // {
    //     inputActions.UI.ToggleInventory.performed -= OnToggleInventory;
    //     inputActions.UI.Disable();
    // }
    

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
        Debug.Log("CHANGE");
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
        GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
        for (int x = 0; x < slotList.Count; x++){
            slotList[x].GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);
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
