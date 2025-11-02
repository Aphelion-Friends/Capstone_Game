using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DraggableItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public Canvas canvas;
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        Debug.Log(canvas);
    }
    // public void OnBeginDrag(PointerEventData eventData)
    // {
    //     Debug.Log("Begin drag");
    //     parentAfterDrag = transform.parent;
    //     transform.SetParent(transform.root, false);
    //     transform.SetAsLastSibling();
    //     image.raycastTarget = false;
    // }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag, false);
        transform.position = parentAfterDrag.position;
    }

    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     Debug.Log("End drag");
    //     transform.SetParent(parentAfterDrag);
    //     image.raycastTarget = true;
    // }
}
