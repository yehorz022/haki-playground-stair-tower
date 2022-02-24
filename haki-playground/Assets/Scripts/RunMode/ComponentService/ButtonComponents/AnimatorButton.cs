using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Shared.Helpers
{
    //>>> This is child class is of button responsible to handle animation features on button

    public class AnimatorButton : IButton
    {
        public string normal = "Normal";
        public string pressed = "Pressed";
        public string disabled = "Disabled";
        public string highlighted = "Highlighted";

        void OnEnable()
        {
            GetComponent<Animator>().SetBool(interactable ? normal : disabled, true);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            GetComponent<Animator>().SetBool(interactable ? pressed : disabled, true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
#if !UNITY_EDITOR
                base.OnPointerExit(eventData);
                GetComponent<Animator>().Rebind();
                GetComponent<Animator>().SetBool(interactable ? highlighted : disabled, true);
#endif
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_EDITOR
            base.OnPointerUp(eventData);
            GetComponent<Animator>().Rebind();
            GetComponent<Animator>().SetBool(interactable ? highlighted : disabled, true);
#endif
        }
    }
}
