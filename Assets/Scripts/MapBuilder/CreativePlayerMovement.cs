using UnityEngine;

namespace MapBuilder
{
    // This class moves the creative player in response to keyboard input.
    public class CreativePlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 7f;
        [SerializeField] private float _fastSpeed = 20f;

        private Vector3 _velocity;

        void Update()
        {
            // The speed changes to _fastSpeed if the player is holding the "fast" key.
            float speed = MapEditorInputManager.Instance.fastAction.inProgress ? _fastSpeed : _moveSpeed;

            _velocity = (transform.forward * MapEditorInputManager.Instance.moveDirection.y + transform.right * MapEditorInputManager.Instance.moveDirection.x) * speed;
            
            if (MapEditorInputManager.Instance.upAction.inProgress)
            {
                _velocity.y = speed;
            }
            else if (MapEditorInputManager.Instance.downAction.inProgress)
            {
                _velocity.y = -speed;
            }

            transform.position += _velocity * Time.deltaTime;
        }
    }
}
