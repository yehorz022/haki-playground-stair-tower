using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorButton : IButton {
    [SerializeField] Color normal = Color.white;
    [SerializeField] Color pressed = Color.gray;
    [SerializeField] Color disabled = Color.red;

    void Awake() {
        if (GetComponent<Image>())
            GetComponent<Image>().color = interactable ? normal : disabled;
        else if (GetComponent<Text>())
            GetComponent<Text>().color = interactable ? normal : disabled;
    }

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        if (GetComponent<Image>())
            GetComponent<Image>().color = pressed;
        else if (GetComponent<Text>())
            GetComponent<Text>().color = pressed;
    }
    public override void OnPointerExit(PointerEventData eventData) {
#if !UNITY_EDITOR
        base.OnPointerExit(eventData);
            if (GetComponent<Image>())
                GetComponent<Image>().color = normal;
            else if (GetComponent<Text>())
                GetComponent<Text>().color = normal;
#endif
    }
    public override void OnPointerUp(PointerEventData eventData) {
#if UNITY_EDITOR
            base.OnPointerUp(eventData);
            if (GetComponent<Image>())
                GetComponent<Image>().color = normal;
            else if (GetComponent<Text>())
                GetComponent<Text>().color = normal;
#endif
    }
}
