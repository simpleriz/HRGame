using UnityEngine;
using UnityEngine.EventSystems;

public class WindowsController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform windowRect;
    private Vector2 originalMousePosition;
    private Vector2 originalWindowPosition;

    public RectTransform TitleBar;

    public void OnBeginDrag(PointerEventData eventData)
    {
        windowRect = GetComponent<RectTransform>();
        /*if (!RectTransformUtility.RectangleContainsScreenPoint(TitleBar, eventData.position, eventData.pressEventCamera))
        {
            
            return;
        }*/

        originalMousePosition = eventData.position;
        originalWindowPosition = windowRect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        windowRect = GetComponent<RectTransform>();
        if (!RectTransformUtility.RectangleContainsScreenPoint(TitleBar, eventData.position, eventData.pressEventCamera))
        {
            return;
        }

        Vector2 currentMousePosition = eventData.position;
        Vector2 delta = currentMousePosition - originalMousePosition;

        windowRect.anchoredPosition = originalWindowPosition + delta;
    }
}