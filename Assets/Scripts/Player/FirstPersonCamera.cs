// Adapted from this video: https://www.youtube.com/watch?v=wd3mDnogxRk&list=PLF6lFlLzb6CRom_ItuhgGRTGNArFf23uw&index=1&t=296s
using UnityEngine;


public class FirstPersonCamera : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float _lookSensitivity = 0.2f;
    [SerializeField] private float _maxLookAngle = 85f;
    private Camera _playerCamera; //Camera I'm using for fps
    private AudioListener _audioListener;

    public Camera playerCamera { get { return _playerCamera; } }

    private Vector2 _currentRotation;
    private bool _initalized;

    public Vector3 forward => transform.forward;
    
    void Awake()
    {
        _playerCamera = GetComponent<Camera>();
        _audioListener = GetComponent<AudioListener>();
    }

    public void Init()
    {
        _initalized = true;
        _playerCamera.enabled = true;
        _audioListener.enabled = true;
    }

    void LateUpdate()
    {
        if (!_initalized) return;

        Vector2 mouseDelta = Vector2.zero;
        mouseDelta = InputManager.Instance.lookDirection;
        mouseDelta *= _lookSensitivity;

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
