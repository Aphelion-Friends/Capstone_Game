using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotDrop : MonoBehaviour, IDropHandler
{
    // After the item is dropped onto a slot, this function sets the parentAfterDrag variable in DraggableItem.cs.
    public void OnDrop(PointerEventData eventData)
    {
        // It doesn't allow you to drop the item if anything is in the slot.
        // We will have to change this eventually because you should be able to combine two stacks of the same type of item.
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
        }
    }
}
