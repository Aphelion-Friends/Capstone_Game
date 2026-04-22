using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    private FirstPersonCamera cameraScript;

    [Header("Camera Recoil")]
    [SerializeField] private float recoilX = 1f;
    [SerializeField] private float recoilY = 10f;

    [Header("Gun Kickback")]
    [SerializeField] private Vector3 kickPosition = new Vector3(0f, 0f, -0.08f);
    [SerializeField] private Vector3 kickRotation = new Vector3(-8f, 2f, 2f);
    [SerializeField] private float returnSpeed = 8f;
    [SerializeField] private float snappiness = 16f;

    private Vector3 currentPosition;
    private Vector3 targetPosition;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    void Awake()
    {
        cameraScript = GetComponentInParent<FirstPersonCamera>();

        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    void Update()
    {
        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.deltaTime);
        transform.localPosition = initialLocalPosition + currentPosition;

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = initialLocalRotation * Quaternion.Euler(currentRotation);
    }

    public void Recoil()
    {
        if (cameraScript != null)
        {
            cameraScript.AddRotation(new Vector2(-recoilY, Random.Range(-recoilX, recoilX)));
        }

        targetPosition += kickPosition;
        targetRotation += new Vector3(
            kickRotation.x,
            Random.Range(-kickRotation.y, kickRotation.y),
            Random.Range(-kickRotation.z, kickRotation.z)
        );
    }
}