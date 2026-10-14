using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    private Vector2 _inputVector;

    public Vector2 Direction => _inputVector;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out Vector2 position))
        {
            Vector2 sizeDelta = background.sizeDelta;
            position.x = (position.x / sizeDelta.x) * 2;
            position.y = (position.y / sizeDelta.y) * 2;

            _inputVector = position.magnitude > 1.0f ? position.normalized : position;
            handle.anchoredPosition = new Vector2(_inputVector.x * (sizeDelta.x / 2f), _inputVector.y * (sizeDelta.y / 2f));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}