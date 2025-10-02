using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    public GameObject inventoryPanel;

    private InputSystem_Actions inputActions;
    private bool isOpen = false;

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
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }
}
