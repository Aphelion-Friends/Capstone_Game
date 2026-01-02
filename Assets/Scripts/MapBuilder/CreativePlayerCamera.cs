using UnityEngine;

namespace MapBuilder
{
    public class CreativePlayerCamera : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] private float _lookSensitivity = 0.2f;
        [SerializeField] private float _maxLookAngle = 85f;

        private Camera _playerCamera;
        private Vector3 _currentRotation;

        public Vector3 currentRotation { get { return _currentRotation; } }
        public Camera playerCamera { get { return _playerCamera; } }

        void Awake()
        {
            _playerCamera = GetComponent<Camera>();
        }

        void LateUpdate()
        {
            if (MapEditorInputManager.Instance.menuOpen)
                return;

            Vector2 mouseDelta = Vector2.zero;
            mouseDelta = MapEditorInputManager.Instance.lookDirection;
            mouseDelta *= _lookSensitivity;

            _currentRotation.x -= mouseDelta.y;
            _currentRotation.x = Mathf.Clamp(_currentRotation.x, -_maxLookAngle, _maxLookAngle);
            transform.localRotation = Quaternion.Euler(_currentRotation.x, 0f, 0f);

            _currentRotation.y += mouseDelta.x;
            if (transform.parent != null)
                transform.parent.rotation = Quaternion.Euler(0f, _currentRotation.y, 0f);
            
        }
    }
}
