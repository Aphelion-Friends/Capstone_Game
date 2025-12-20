// Adapted from this video: https://www.youtube.com/watch?v=wd3mDnogxRk&list=PLF6lFlLzb6CRom_ItuhgGRTGNArFf23uw&index=1&t=296s

using UnityEngine;
using PurrNet.Prediction;

public class PlayerMovement : PredictedIdentity<PlayerMovement.MoveInput, PlayerMovement.MoveState>
{
    [SerializeField] private float _moveSpeed = 7;
    [SerializeField] private float _acceleration = 20;
    [SerializeField] private float _planarDamping = 10f;
    [SerializeField] private float _jumpForce = 100f;
    [SerializeField] private float _groundCheckRadius = 0.5f;
    [SerializeField] private float _jumpCooldown = 0.2f;
    [SerializeField] private LayerMask _groundMask;

    [SerializeField] private FirstPersonCamera _camera;
    [SerializeField] private PredictedRigidbody _rigidbody;

    protected override void LateAwake()
    {
        if(isOwner)
            _camera.Init();
    }

    protected override void Simulate(MoveInput input, ref MoveState state, float delta)
    {
        state.jumpCooldown -= delta;

        Vector3 targetVel = (transform.forward * input.moveDirection.y + transform.right * input.moveDirection.x) * _moveSpeed;
        _rigidbody.AddForce(targetVel * _acceleration);
        Debug.Log(targetVel);

        var horizontal = new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z);
        _rigidbody.AddForce(-horizontal * _planarDamping);
        if (horizontal.magnitude > _moveSpeed)
            _rigidbody.velocity = new Vector3(targetVel.x, _rigidbody.velocity.y, targetVel.z);

        
        if(input.jump && isGrounded() && state.jumpCooldown <= 0)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce);
            state.jumpCooldown = _jumpCooldown;
        }

        var cameraForward = input.cameraForward;
        cameraForward.y = 0;
        if(cameraForward.sqrMagnitude > 0.0001f)
        {
            _rigidbody.MoveRotation((Quaternion.LookRotation(cameraForward.normalized)));
        }
    }

    protected override void UpdateInput(ref MoveInput input)
    {
        input.jump |= InputManager.Instance.jumpAction.inProgress;
    }

    protected override void GetFinalInput(ref MoveInput input)
    {
        Vector2 move = Vector2.zero;

        move = InputManager.Instance.moveDirection;

        input.moveDirection = Vector2.ClampMagnitude(move, 1f);
        input.cameraForward = _camera.forward;
    }

    protected override void SanitizeInput(ref MoveInput input)
    {
        if(input.moveDirection.magnitude > 1)
            input.moveDirection.Normalize();
    }

    public struct MoveInput : IPredictedData
    {
        public Vector2 moveDirection;
        public Vector3 cameraForward;
        public bool jump;

        public void Dispose() {}
    }

    public struct MoveState : IPredictedData<MoveState>
    {
        public float jumpCooldown;

        public void Dispose() {}
    }

    private bool isGrounded()
    {
        return Physics.CheckSphere(transform.position, _groundCheckRadius, _groundMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _groundCheckRadius);
    }
}
