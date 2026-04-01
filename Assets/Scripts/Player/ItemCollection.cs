using UnityEngine;
using PurrNet.Prediction;

[RequireComponent(typeof(PlayerInteraction))]
[RequireComponent(typeof(FirstPersonCamera))]
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
    FirstPersonCamera firstPersonCamera;

    [SerializeField] LayerMask itemLayer;

    protected override void LateAwake()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        firstPersonCamera = GetComponent<FirstPersonCamera>();
    }

    protected override void Simulate(ItemCollectionInput input, ref ItemCollectionState state, float delta)
    {
        Ray ray = new Ray(firstPersonCamera.playerCamera.transform.position, firstPersonCamera.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * playerInteraction.pickupRange, Color.red);

        if (Physics.Raycast(ray, out hit, playerInteraction.pickupRange, itemLayer))
        {
            Debug.Log("NOW looking at item: " + hit.collider.gameObject.name);
        }
    }
}
