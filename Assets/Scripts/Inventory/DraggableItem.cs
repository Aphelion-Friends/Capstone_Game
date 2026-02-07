using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Image image;

    [HideInInspector] public NetworkInventory inventory;

    private Transform parentBeforeDrag;
    [HideInInspector] public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        parentBeforeDrag = transform.parent;
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag, false);
        transform.position = parentAfterDrag.position;
        image.raycastTarget = true;

        int startIndex = parentBeforeDrag.GetComponent<SlotScript>().index;
        int endIndex = parentAfterDrag.GetComponent<SlotScript>().index;

        inventory.RequestMoveOrSwap(startIndex, endIndex);
    }
}
