// Adapted from this video: https://www.youtube.com/watch?v=wd3mDnogxRk&list=PLF6lFlLzb6CRom_ItuhgGRTGNArFf23uw&index=1&t=296s
using UnityEngine;
using Cinemachine; //To use the cinemachine camera
using UnityEngine.InputSystem;
using UnityEngine.AI;


public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private float _lookSensitivity = 2f;
    [SerializeField] private float _maxLookAngle = 85f;
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera; //Camera I'm using for fps

    private Vector2 _currentRotation;
    private bool _initalized;

    public Vector3 forward => transform.forward;
    
    void Awake()
    {
        if (_cinemachineCamera != null)
            _cinemachineCamera.Priority = -1;
    }

    public void Init()
    {
        _initalized = true;
        if (_cinemachineCamera != null)
            _cinemachineCamera.Priority = 10;
    }

    void LateUpdate()
    {
        if (!_initalized) return;

        Vector2 mouseDelta = Vector2.zero;
        if (Mouse.current != null)
            mouseDelta = Mouse.current.delta.ReadValue() * _lookSensitivity;

        _currentRotation.x -= mouseDelta.y;
        _currentRotation.x = Mathf.Clamp(_currentRotation.x, -_maxLookAngle, _maxLookAngle);
        transform.localRotation = Quaternion.Euler(_currentRotation.x, 0f, 0f);

        _currentRotation.y += mouseDelta.x;
        if (transform.parent != null)
            transform.parent.rotation = Quaternion.Euler(0f, _currentRotation.y, 0f);
        
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.MoveRotation(Quaternion.Euler(0f, _currentRotation.y, 0f));
        }
    }
}
