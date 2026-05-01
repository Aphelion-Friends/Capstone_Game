using UnityEngine;
using UnityEngine.InputSystem;

// A singleton class that gives a reference to the player input.
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    private PlayerInput _playerInput;
    public PlayerInput playerInput { get { return _playerInput; } }

    private Vector2 _moveDirection;
    private Vector2 _lookDirection;

    public Vector2 moveDirection { get { return _moveDirection; } }
    public Vector2 lookDirection { get { return _lookDirection; } }

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _fireAction;
    private InputAction _sprintAction;
    private InputAction _interactAction;
    private InputAction _inventoryAction;
    private InputAction _flashlightAction;
    private InputAction _reloadAction;
    private InputAction _healAction;
    private InputAction _pauseAction;
    private InputAction _resetAction;

    public InputAction moveAction { get { return _moveAction; } }
    public InputAction lookAction { get { return _lookAction; } }
    public InputAction jumpAction { get { return _jumpAction; } }
    public InputAction fireAction { get { return _fireAction; } }
    public InputAction sprintAction { get { return _sprintAction; } }
    public InputAction interactAction { get { return _interactAction; } }
    public InputAction inventoryAction { get { return _inventoryAction; } }
    public InputAction flashlightAction { get { return _flashlightAction; } }
    public InputAction reloadAction { get { return _reloadAction; } }
    public InputAction healAction { get { return _healAction; } }
    public InputAction pauseAction { get { return _pauseAction; } }
    public InputAction resetAction { get { return _resetAction; } }

    private bool _ignoreMouseMove = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;

        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.currentActionMap.FindAction("Move");
        _lookAction = _playerInput.currentActionMap.FindAction("Look");
        _jumpAction = _playerInput.currentActionMap.FindAction("Jump");
        _fireAction = _playerInput.currentActionMap.FindAction("Fire");
        _sprintAction = _playerInput.currentActionMap.FindAction("Sprint");
        _interactAction = _playerInput.currentActionMap.FindAction("Interact");
        _inventoryAction = _playerInput.currentActionMap.FindAction("Inventory");
        _flashlightAction = _playerInput.currentActionMap.FindAction("Flashlight");
        _reloadAction = _playerInput.currentActionMap.FindAction("Reload");
        _healAction = _playerInput.currentActionMap.FindAction("Heal");
        _pauseAction = _playerInput.currentActionMap.FindAction("Pause");
        _resetAction = _playerInput.currentActionMap.FindAction("Reset");

        _playerInput.onActionTriggered += OnAction;
    }

    private void OnAction(InputAction.CallbackContext context)
    {
        if (context.action == _moveAction)
        {
            _moveDirection = moveAction.ReadValue<Vector2>();
        }

        if (context.action == _lookAction)
        {
            // To prevent the player's camera from jumping when the cursor gets locked
            if (_ignoreMouseMove && context.performed)
            {
                _ignoreMouseMove = false;
                _lookDirection = Vector2.zero;
            }
            else if (!_ignoreMouseMove)
                _lookDirection = lookAction.ReadValue<Vector2>();
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _lookAction.Enable();
        _fireAction.Enable();
    }

    public void UnlockCursor()
    {
        _lookAction.Disable();
        _fireAction.Disable();
        Cursor.lockState = CursorLockMode.Confined;
        _ignoreMouseMove = true;
    }
}
