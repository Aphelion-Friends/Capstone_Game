using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DraggableItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Image image;
    public InventoryObject inventory;
    private Transform parentBeforeDrag;
    [HideInInspector] public Transform parentAfterDrag;
    public Canvas canvas;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    // Called when the item has started to be dragged. It sets the before and after parents, which are the slots that the item is
    // being dragged to and from.
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        parentBeforeDrag = transform.parent;
        // You need to disable raycastTarget during the drag or you get a weird bug
        image.raycastTarget = false;
    }

    // Sets the item position to the cursor position during the drag
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    // After the drag ends, the item needs to be put in its new slot.
    // That's why we set the parent of the item to parentAfterDrag.
    // By the way, parentAfterDrag gets set in the InventorySlotDrop.cs script.
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag, false);
        transform.position = parentAfterDrag.position;
        // Enable raycastTarget or else we can't drag the item anymore
        image.raycastTarget = true;

        // Sorry this code is not very clear. It updates the actual inventory scriptable object.
        // It finds the index of the slot the item was originally in and the index of the slot the item was dragged to.
        int startIndex = parentBeforeDrag.gameObject.GetComponent<SlotScript>().index;
        int endIndex = parentAfterDrag.gameObject.GetComponent<SlotScript>().index;
        int itemAmount = inventory.GetItemAmountAtIndex(startIndex);
        ItemObject itemToMove = inventory.GetItemAtIndex(startIndex);

        // Then, it moves all of the items from startIndex to endIndex in the ItemObject.
        inventory.RemoveItemAtIndex(itemAmount, startIndex);
        inventory.AddItemAtIndex(itemToMove, itemAmount, endIndex);
    }
}
