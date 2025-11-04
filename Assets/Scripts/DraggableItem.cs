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
        Debug.Log(canvas);
    }

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

        int startIndex = parentBeforeDrag.gameObject.GetComponent<SlotScript>().index;
        int endIndex = parentAfterDrag.gameObject.GetComponent<SlotScript>().index;
        int itemAmount = inventory.GetItemAmountAtIndex(startIndex);
        ItemObject itemToMove = inventory.GetItemAtIndex(startIndex);

        inventory.RemoveItemAtIndex(itemAmount, startIndex);
        inventory.AddItemAtIndex(itemToMove, itemAmount, endIndex);
    }
}
