// Adapted from this video: https://www.youtube.com/watch?v=wd3mDnogxRk&list=PLF6lFlLzb6CRom_ItuhgGRTGNArFf23uw&index=1&t=296s

using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private float _lookSensitivity = 2f;
    [SerializeField] private float _maxLookAngle = 85f;
    [SerializeField] private Camera _mainCamera;

    private Vector2 _currentRotation;
    private bool _initalized;

    public Vector3 forward => Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0) * Vector3.forward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
