using TMPro;
using UnityEngine;
using PurrNet.Prediction;
using System.Collections.Generic;

public class PlayerInteraction : PredictedIdentity<PlayerInteraction.PlayerInteractionState>
{
    private Camera mainCam;

    // Stores state such as which item the player is looking at
    public struct PlayerInteractionState : IPredictedData<PlayerInteractionState>
    {
        // We use PredictedObjectID so PurrNet is able to keep
        // track of networked items
        public List<PredictedObjectID> itemsInRange;
        public PredictedObjectID lookedAtItem;

        public void Dispose() {}
    }

    // Input should be based on player input from other predicted identities
    // such as player camera, player movement
    // this only checks if the player is near enough to item to interact
    // as player camera movement is not predicted
    // this should work fine with purrdiction i think

    [Header("Inventory Reference")]
    // Scriptable object inventory, only stores data about inventory
    public InventoryObject inventory;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI pickupPrompt;

    [Header("Pickup Settings")]
    // Distance between player and item
    public float pickupRange = 3f;
    // The layer of items that can actually be collected
    // if we have interactable items that aren't items, i think we need more layers
    // like buttonLayer or somethink I don't know
    public LayerMask itemLayer;

    protected override void Simulate(ref PlayerInteractionState state, float delta)
    {
        // We check if any items are in range
        // We get a list of all items in range and save to state
        // We check if player is close enough to item
        // if he is close enough, we set lookedAtItem
        // item is not displayed until the player camera actually looks at the item
        // the server does not care about the player camera
        // maybe this is a bad design, I don't know

        // This is probably a terrible way to do this but whatever
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, pickupRange);
        List<PredictedObjectID> newItemList = new List<PredictedObjectID>();
        foreach (var hitCollider in hitColliders)
        {
            GameObject hitObject = hitCollider.gameObject;
            
            // Get predicted identity component for in-world object if exists
            InWorldItem inWorldItem = hitObject.GetComponent<InWorldItem>();

            PredictedObjectID itemID;
            bool isPredicted;
            isPredicted = predictionManager.hierarchy.TryGetId(hitObject, out itemID);

            // Only add the item to the list of items
            // if it is tracked by PurrNet's prediction system
            // and if it has the inWorldItem component
            if (inWorldItem && isPredicted)
            {
                newItemList.Add(itemID);
                Debug.Log(hitObject.name);
            }
        }

        state.itemsInRange = newItemList;
    }
}
