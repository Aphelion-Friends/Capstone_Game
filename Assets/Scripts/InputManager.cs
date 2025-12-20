using UnityEngine;
using UnityEngine.InputSystem;

// A singleton class that gives a reference to the player input.
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    private PlayerInput _playerInput;
    public PlayerInput playerInput { get { return _playerInput; } }

    private Vector2 _move;
    private Vector2 _lookDirection;
    private Vector3 _forward;

    public Vector2 move { get { return _move; } }
    public Vector2 lookDirection { get { return _lookDirection; } }
    public Vector3 forward { get { return _forward; } }

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _fireAction;
    private InputAction _sprintAction;
    private InputAction _interactAction;
    private InputAction _inventoryAction;
    private InputAction _flashlightAction;

    public InputAction moveAction { get { return _moveAction; } }
    public InputAction lookAction { get { return _lookAction; } }
    public InputAction fireAction { get { return _fireAction; } }
    public InputAction sprintAction { get { return _sprintAction; } }
    public InputAction interactAction { get { return _interactAction; } }
    public InputAction inventoryAction { get { return _inventoryAction; } }
    public InputAction flashlightAction { get { return _flashlightAction; } }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;

        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.currentActionMap.FindAction("Move");
        _lookAction = _playerInput.currentActionMap.FindAction("Look");
        _fireAction = _playerInput.currentActionMap.FindAction("Fire");
        _sprintAction = _playerInput.currentActionMap.FindAction("Sprint");
        _interactAction = _playerInput.currentActionMap.FindAction("Interact");
        _inventoryAction = _playerInput.currentActionMap.FindAction("Inventory");
        _flashlightAction = _playerInput.currentActionMap.FindAction("Flashlight");
    }
}
