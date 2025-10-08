using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    public GameObject inventoryPanel;

    private InputSystem_Actions inputActions;
    public static bool inventoryOpen = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.UI.ToggleInventory.performed += OnToggleInventory;
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.ToggleInventory.performed -= OnToggleInventory;
        inputActions.UI.Disable();
    }

    private void OnToggleInventory(InputAction.CallbackContext context)
    {
        inventoryOpen = !inventoryOpen;
        inventoryPanel.SetActive(inventoryOpen);

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
