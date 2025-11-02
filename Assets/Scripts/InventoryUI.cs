using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class InventoryUI : MonoBehaviour
{
    // [Header("References")]
    // public GameObject inventoryPanel;

    [SerializeField] StarterAssetsInputs input;
    public static bool inventoryOpen = false;

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
    

    public void Update()
    {
        if (input.inventoryOpen)
        {
            OnToggleInventory();
            input.inventoryOpen = false;
        }
    }

    private void OnToggleInventory()
    {
        Debug.Log("INVEN");
        inventoryOpen = !inventoryOpen;
        GetComponent<CanvasRenderer>().SetAlpha(inventoryOpen ? 1f : 0f);

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
