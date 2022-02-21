using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler {
    public bool interactable = true;
    public UnityEvent onClick = new UnityEvent();
    public UnityEvent onLongTap = new UnityEvent();
    //  public Component[] animations; // will work on later

    public virtual void OnPointerDown(PointerEventData eventData) {
        if (!interactable)
            return;
        InputUI.OnInputDown();
        transform.GetComponent<PanelComponent>().Select();
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
#if !UNITY_EDITOR
        if (!interactable)
            return;

        if (InputUI.InputType() == Click.Tap)
            onClick.Invoke();
#endif
    }

    public virtual void OnPointerUp(PointerEventData eventData) {
#if UNITY_EDITOR
        if (!interactable)
                return;
        if (InputUI.InputType() == Click.Tap && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            onClick.Invoke();
#endif
    }
}
