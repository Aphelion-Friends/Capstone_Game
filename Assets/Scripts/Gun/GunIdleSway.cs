using UnityEngine;

public class GunIdleSway : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Idle Breathing")]
    [SerializeField] private float idleHorizontalAmount = 0.02f;
    [SerializeField] private float idleVerticalAmount = 0.015f;
    [SerializeField] private float idleRotationAmount = 2f;
    [SerializeField] private float idleSpeed = 1.2f;

    [Header("Movement Bob")]
    [SerializeField] private float moveHorizontalAmount = 0.03f;
    [SerializeField] private float moveVerticalAmount = 0.025f;
    [SerializeField] private float moveRotationAmount = 3f;
    [SerializeField] private float moveSpeed = 7f;

    [Header("Sprint Pose")]
    [SerializeField] private Vector3 sprintPositionOffset = new Vector3(-0.18f, -0.12f, -0.12f);
    [SerializeField] private Vector3 sprintRotationOffset = new Vector3(18f, -35f, 12f);
    [SerializeField] private float sprintSmooth = 10f;

    [Header("Smoothing")]
    [SerializeField] private float normalSmooth = 8f;

    private Vector3 baseLocalPosition;
    private Quaternion baseLocalRotation;

    private void Awake()
    {
        baseLocalPosition = transform.localPosition;
        baseLocalRotation = transform.localRotation;

        if (playerMovement == null)
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
        }
    }

    private void LateUpdate()
    {
        if (playerMovement == null)
        {
            return;
        }

        Vector2 moveInput = playerMovement.currentInput.moveDirection;
        bool isMoving = moveInput.sqrMagnitude > 0.001f;
        bool isSprinting = playerMovement.currentInput.sprint && isMoving;

        Vector3 targetPos = baseLocalPosition;
        Quaternion targetRot = baseLocalRotation;

        float t = Time.time;

        if (isSprinting)
        {
            Vector3 sprintBob = new Vector3(Mathf.Sin(t * moveSpeed) * 0.02f, Mathf.Cos(t * moveSpeed * 2f) * 0.015f, 0f);
            Vector3 sprintRotBob = new Vector3(Mathf.Cos(t * moveSpeed * 2f) * 2f, Mathf.Sin(t * moveSpeed) * 1.5f, Mathf.Sin(t * moveSpeed) * 1f);

            targetPos = baseLocalPosition + sprintPositionOffset + sprintBob;
            targetRot = baseLocalRotation * Quaternion.Euler(sprintRotationOffset + sprintRotBob);

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, sprintSmooth * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, sprintSmooth * Time.deltaTime);
            return;
        }

        if (isMoving)
        {
            Vector3 bobPos = new Vector3(Mathf.Sin(t * moveSpeed) * moveHorizontalAmount, Mathf.Cos(t * moveSpeed * 2f) * moveVerticalAmount,0f);
            Vector3 bobRot = new Vector3(Mathf.Cos(t * moveSpeed * 2f) * moveRotationAmount, Mathf.Sin(t * moveSpeed) * moveRotationAmount, Mathf.Sin(t * moveSpeed) * (moveRotationAmount * 0.5f));

            targetPos += bobPos;
            targetRot = baseLocalRotation * Quaternion.Euler(bobRot);
        }
        else
        {
            Vector3 idlePos = new Vector3(Mathf.Sin(t * idleSpeed) * idleHorizontalAmount, Mathf.Cos(t * idleSpeed * 2f) * idleVerticalAmount, 0f);

            Vector3 idleRot = new Vector3(Mathf.Cos(t * idleSpeed) * idleRotationAmount, Mathf.Sin(t * idleSpeed) * idleRotationAmount, Mathf.Sin(t * idleSpeed * 0.5f) * (idleRotationAmount * 0.35f));

            targetPos += idlePos;
            targetRot = baseLocalRotation * Quaternion.Euler(idleRot);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, normalSmooth * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, normalSmooth * Time.deltaTime);
    }
}