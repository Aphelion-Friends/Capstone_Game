using UnityEngine;

public class FreecamSpectator : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float sprintMultiplier = 3f;
    public float verticalSpeed = 8f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2.5f;
    public float smoothTime = 0.05f;

    private float rotationX;
    private float rotationY;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;

    [Header("References")]
    public Camera cam;
    public bool isSpectating = false;

    void Start()
    {
        if (cam == null)
            cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (!isSpectating) return;

        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        Vector2 targetDelta = new Vector2(mouseX, mouseY);
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetDelta, ref currentMouseDeltaVelocity, smoothTime);

        rotationX -= currentMouseDelta.y;
        rotationY += currentMouseDelta.x;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    void HandleMovement()
    {
        float speed = moveSpeed;
        if (InputManager.Instance.sprintAction.inProgress)
            speed *= sprintMultiplier;

        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        transform.Translate(move * speed * Time.deltaTime);

        // Vertical movement
        if (InputManager.Instance.interactAction.inProgress)
            transform.position += Vector3.up * verticalSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
            transform.position += Vector3.down * verticalSpeed * Time.deltaTime;
    }

    public void EnableSpectator()
    {
        isSpectating = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisableSpectator()
    {
        isSpectating = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
