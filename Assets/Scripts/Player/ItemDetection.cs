using TMPro;
using UnityEngine;
using PurrNet.Prediction;

[RequireComponent(typeof(PlayerInteraction))]
public class ItemDetection : PredictedIdentity<ItemDetection.ItemDetectionInput, ItemDetection.ItemDetectionState>
{
    public struct ItemDetectionInput : IPredictedData
    {
        public Vector2 forward;

        public void Dispose() {}
    }

    public struct ItemDetectionState : IPredictedData<ItemDetectionState>
    {
        PredictedObjectID? lookedAtItem;

        public void Dispose() {}
    }

    PlayerInteraction playerInteraction;
    [SerializeField] FirstPersonCamera firstPersonCamera;

    [SerializeField] LayerMask itemLayer;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI pickupPrompt;
    private string originalPrompt;

    protected override void LateAwake()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        firstPersonCamera = GetComponentInChildren<FirstPersonCamera>();
        originalPrompt = pickupPrompt.text;
    }

    protected override void Simulate(ItemDetectionInput input, ref ItemDetectionState state, float delta)
    {
        if (!firstPersonCamera)
        {
            Debug.LogError("No camera script assigned!");
            return;
        }

        Ray ray = new Ray(firstPersonCamera.playerCamera.transform.position, firstPersonCamera.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * playerInteraction.pickupRange, Color.blue);
        bool showPrompt = false;

        if (Physics.Raycast(ray, out hit, playerInteraction.pickupRange, itemLayer))
        {
            // Debug.Log("NOW looking at item: " + hit.collider.gameObject.name);
            pickupPrompt.text = originalPrompt + hit.collider.gameObject.GetComponent<InWorldItem>().item.displayName;
            showPrompt = true;
        }

        pickupPrompt.gameObject.SetActive(showPrompt);
    }
}
