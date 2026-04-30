using UnityEngine;

public class FlashlightDetector : MonoBehaviour
{
    [SerializeField] private float _detectionAngle = 45f;
    [SerializeField] private float _detectionRange = 15f;

    public bool isLit { get; private set; }
    public Vector3 lightSource { get; private set; }

    void Update()
    {
        isLit = false;

        FlashlightToggle[] flashlights = FindObjectsByType<FlashlightToggle>(FindObjectsSortMode.None);
        Debug.Log($"Flashlights found: {flashlights.Length}");

        foreach (var flashlight in flashlights)
        {

            if (!flashlight.isOwner)
            {
                Debug.Log("Skipping non-owner flashlight");
                continue;
            }

            Light light = flashlight.GetComponentInChildren<Light>();
            Debug.Log($"Light found: {light != null}, Light enabled: {light?.enabled}");

            if (light == null || !light.enabled) 
                continue;
                
            float distanceToLight = Vector3.Distance(transform.position, flashlight.transform.position);
            Debug.Log($"Distance to light: {distanceToLight}, Detection range: {_detectionRange}");
            if (distanceToLight > _detectionRange)
                continue;

            Vector3 directToEnemy = (transform.position - flashlight.transform.position).normalized;
            float angle = Vector3.Angle(flashlight.transform.forward, directToEnemy);
            Debug.Log($"Angle to enemy: {angle}, Detection angle: {_detectionAngle}");

            if (angle < _detectionAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(flashlight.transform.position, directToEnemy, out hit))
                {
                    Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");
                    if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
                    {
                        isLit = true;
                        lightSource = flashlight.transform.position;
                        break;
                    }
                }
            }
        }
    }
}
