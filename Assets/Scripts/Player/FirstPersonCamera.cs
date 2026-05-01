// Adapted from this video: https://www.youtube.com/watch?v=wd3mDnogxRk&list=PLF6lFlLzb6CRom_ItuhgGRTGNArFf23uw&index=1&t=296s
using UnityEngine;


public class FirstPersonCamera : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float _lookSensitivity = 0.2f;
    [SerializeField] private float _maxLookAngle = 85f;
    [SerializeField] private float _smoothness = 0.2f;
    [SerializeField] private Camera _playerCamera; //Camera I'm using for fps
    [SerializeField] private AudioListener _audioListener;

    public Camera playerCamera { get { return _playerCamera; } }

    private Vector2 _targetRotation;
    private Vector2 _currentRotation;
    private bool _initalized;

    public Vector3 forward => transform.forward;
    
    // void Start()
    // {
    //     _playerCamera = GetComponent<Camera>();
    //     _audioListener = GetComponent<AudioListener>();
    // }

    public void Init()
    {
        _initalized = true;
        _playerCamera.enabled = true;
        _audioListener.enabled = true;
    }

    public void AddRotation(Vector2 rotationToAdd)
    {
        _targetRotation += rotationToAdd;
    }

    // From the internets: https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
    private static float Damp(float a, float b, float lambda, float dt)
    {
        return Mathf.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    void LateUpdate()
    {
        if (!_initalized) return;

        Vector2 mouseDelta = Vector2.zero;
        mouseDelta = InputManager.Instance.lookDirection;
        mouseDelta *= _lookSensitivity;

        _targetRotation.x -= mouseDelta.y;
        _targetRotation.x = Mathf.Clamp(_targetRotation.x, -_maxLookAngle, _maxLookAngle);

        _targetRotation.y += mouseDelta.x;

        _currentRotation.x = Damp(_currentRotation.x, _targetRotation.x, 1 / _smoothness, Time.deltaTime);
        _currentRotation.y = Damp(_currentRotation.y, _targetRotation.y, 1 / _smoothness, Time.deltaTime);

        // Debug.Log($"Current rotation: {_currentRotation}, Target rotation: {_targetRotation}, Smoothness: {_smoothness}");

        transform.localRotation = Quaternion.Euler(_currentRotation.x, 0f, 0f);
        if (transform.parent != null)
            transform.parent.rotation = Quaternion.Euler(0f, _currentRotation.y, 0f);
        
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.MoveRotation(Quaternion.Euler(0f, _currentRotation.y, 0f));
        }
    }
}
