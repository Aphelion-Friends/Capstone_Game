using UnityEngine;
using UnityEngine.EventSystems;
public class UIButtonPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform visualRoot;
    [SerializeField] private Vector2 pressedOffset = new Vector2(0f, -4f);

    private Vector2 _startPos;
    private bool _isHeldDown;

    private void Awake()
    {
        if (!visualRoot)
        {
            visualRoot = (RectTransform)transform;
            _startPos = visualRoot.anchoredPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHeldDown = true;
        visualRoot.anchoredPosition = _startPos + pressedOffset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHeldDown = false;
        visualRoot.anchoredPosition = _startPos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isHeldDown)
        {
            return;
        }
        visualRoot.anchoredPosition = _startPos;
    }

    private void OnDisable()
    {
        _isHeldDown = false;
        if (visualRoot)
        {
            visualRoot.anchoredPosition = _startPos;
        }
    }

}
