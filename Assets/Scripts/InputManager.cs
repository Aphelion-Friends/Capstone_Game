using UnityEngine;
using UnityEngine.InputSystem;

// A singleton class that gives a reference to the player input.
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    private PlayerInput _playerInput;
    public PlayerInput playerInput { get { return _playerInput; } }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;

        _playerInput = GetComponent<PlayerInput>();
    }
}
