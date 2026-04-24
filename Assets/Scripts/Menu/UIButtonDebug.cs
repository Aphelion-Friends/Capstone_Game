using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonDebug : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hovering Options Button");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked Options Button");
    }
}