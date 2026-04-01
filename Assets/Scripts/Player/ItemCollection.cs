using TMPro;
using UnityEngine;
using PurrNet.Prediction;

[RequireComponent(typeof(PlayerInteraction))]
public class ItemCollection : PredictedIdentity<ItemCollection.ItemCollectionInput, ItemCollection.ItemCollectionState>
{
    public struct ItemCollectionInput : IPredictedData
    {
        public Vector2 forward;

        public void Dispose() {}
    }

    public struct ItemCollectionState : IPredictedData<ItemCollectionState>
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

    protected override void Simulate(ItemCollectionInput input, ref ItemCollectionState state, float delta)
    {
        if (!firstPersonCamera)
        {
            Debug.LogError("No camera script assigned!");
            return;
        }

        Ray ray = new Ray(firstPersonCamera.playerCamera.transform.position, firstPersonCamera.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * playerInteraction.pickupRange, Color.red);
        bool showPrompt = false;

        if (Physics.Raycast(ray, out hit, playerInteraction.pickupRange, itemLayer))
        {
            // Debug.Log("NOW looking at item: " + hit.collider.gameObject.name);
            pickupPrompt.text = originalPrompt + hit.collider.gameObject.name;
            showPrompt = true;
        }

        pickupPrompt.gameObject.SetActive(showPrompt);
    }
}
