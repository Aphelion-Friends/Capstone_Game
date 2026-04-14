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
        public PredictedObjectID? lookedAtItem;

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

    protected override void UpdateInput(ref ItemDetectionInput input)
    {
        input.forward = firstPersonCamera.playerCamera.transform.forward;
        Debug.Log($"Forward: {input.forward}");
    }

    protected override void Simulate(ItemDetectionInput input, ref ItemDetectionState state, float delta)
    {
        if (!firstPersonCamera)
        {
            Debug.LogError("No camera script assigned!");
            return;
        }

        Ray ray = new Ray(firstPersonCamera.playerCamera.transform.position, input.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * playerInteraction.pickupRange, Color.blue);
        bool showPrompt = false;

        if (Physics.Raycast(ray, out hit, playerInteraction.pickupRange, itemLayer))
        {
            Debug.Log("NOW looking at item: " + hit.collider.gameObject.name);
            pickupPrompt.text = originalPrompt + hit.collider.gameObject.GetComponent<InWorldItem>().item.displayName;
            showPrompt = true;

            // We have to find the PurrDiction ID so it works nicely with PurrNet
            PredictedObjectID hitItem;
            if (predictionManager.hierarchy.TryGetId(hit.collider.gameObject, out hitItem))
                state.lookedAtItem = hitItem;
            else
                state.lookedAtItem = null;
        }
        else
            state.lookedAtItem = null;

        pickupPrompt.gameObject.SetActive(showPrompt);
    }
}
