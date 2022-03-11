using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.RunMode.ComponentService.ButtonComponents
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
        public UnityEvent onLongPress = new UnityEvent();
        //  public Component[] animations; // will work on later

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
#if UNITY_EDITOR
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
                return;
#endif  
            if (!interactable)
                return;
            onEnter.Invoke();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
#if UNITY_EDITOR
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
                return;
#endif
            if (!interactable)
                return;
            Inputs.OnInputDown();
            Routine.WaitAndCall(1, () => {
#if UNITY_EDITOR
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
#endif
                    if (Inputs.InputType() == Click.LongTap)
                        onLongPress.Invoke();
            });
            onDown.Invoke();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
#if UNITY_EDITOR
            if (!Input.GetMouseButtonUp(0) && !Input.GetMouseButtonUp(1))
                return;
#endif
            if (!interactable)
                return;
            if (Inputs.InputType() == Click.Tap)
                onClick.Invoke();
            onExit.Invoke();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_EDITOR
            if (!Input.GetMouseButtonUp(0) && !Input.GetMouseButtonUp(1))
                return;
#endif
            if (!interactable)
                return;
            Click tap = Inputs.InputType();
            if (tap == Click.Tap)
                onClick.Invoke();
            onUp.Invoke();
        }
    }
}
