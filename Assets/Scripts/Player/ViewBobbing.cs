using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody targetRigidbody;

    [Header("Bobbing")]
    [SerializeField] private float bobFrequency = 8f;
    [SerializeField] private float bobHorizontalAmplitude = 0.03f;
    [SerializeField] private float bobVerticalAmplitude = 0.05f;
    [SerializeField] private float sprintMultiplier = 1.35f;
    [SerializeField] private float movementThreshold = 0.1f;

    [Header("Smoothing")]
    [SerializeField] private float positionSmoothness = 10f;

    [Header("Toggle")]
    [SerializeField] private bool enableBobbing = true;

    private Vector3 _initialLocalPosition;
    private float _bobTimer;
    private bool _initialized;

    public void Init()
    {
        _initialized = true;
        _initialLocalPosition = transform.localPosition;
    }

    public void SetBobbingEnabled(bool enabled)
    {
        enableBobbing = enabled;
    }

    public void SetIntensity(float horizontal, float vertical)
    {
        bobHorizontalAmplitude = horizontal;
        bobVerticalAmplitude = vertical;
    }

    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (!_initialized)
            return;

        if (!enableBobbing || targetRigidbody == null)
        {
            ReturnToRest();
            return;
        }

        Vector3 planarVelocity = targetRigidbody.linearVelocity;
        planarVelocity.y = 0f;

        float speed = planarVelocity.magnitude;

        bool isMoving = speed > movementThreshold;
        bool isGroundedEnough = Mathf.Abs(targetRigidbody.linearVelocity.y) < 0.15f;

        if (!isMoving || !isGroundedEnough)
        {
            ReturnToRest();
            return;
        }

        float speedFactor = InputManager.Instance != null && InputManager.Instance.sprintAction.inProgress
            ? sprintMultiplier
            : 1f;

        _bobTimer += Time.deltaTime * bobFrequency * speedFactor;

        float xOffset = Mathf.Cos(_bobTimer * 0.5f) * bobHorizontalAmplitude * speedFactor;
        float yOffset = Mathf.Sin(_bobTimer) * bobVerticalAmplitude * speedFactor;

        Vector3 targetPosition = _initialLocalPosition + new Vector3(xOffset, yOffset, 0f);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            positionSmoothness * Time.deltaTime
        );
    }

    private void ReturnToRest()
    {
        _bobTimer = 0f;
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            _initialLocalPosition,
            positionSmoothness * Time.deltaTime
        );
    }
}