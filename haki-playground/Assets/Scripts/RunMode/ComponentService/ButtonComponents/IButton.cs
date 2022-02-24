using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Shared.Helpers
{

    //>>> This class is responsible to handle button features as we need more to do than simple unity button

    public class IButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        public bool interactable = true;
        public UnityEvent onClick = new UnityEvent();
        public UnityEvent onEnter = new UnityEvent();
        public UnityEvent onDown = new UnityEvent();
        public UnityEvent onExit = new UnityEvent();
        public UnityEvent onUp = new UnityEvent();
    //  public Component[] animations; // will work on later

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            
            if (!interactable)
                return;
            onEnter.Invoke();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable)
                return;
            Inputs.OnInputDown();
            onDown.Invoke();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!interactable)
                return;
            if (Inputs.InputType() == Click.Tap)
                onClick.Invoke();
#if UNITY_EDITOR
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
#endif
                onExit.Invoke();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable)
                return;
#if UNITY_EDITOR
            if (Inputs.InputType() == Click.Tap && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
                onClick.Invoke();
#endif

            onUp.Invoke();
        }
    }
}
