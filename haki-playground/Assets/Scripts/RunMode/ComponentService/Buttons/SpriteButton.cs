using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Shared.Helpers
{
    public class SpriteButton : IButton
    {
        [SerializeField] Image targetImage;
        [SerializeField] Sprite normal;
        [SerializeField] Sprite pressed;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            targetImage.sprite = pressed;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
#if !UNITY_EDITOR
            base.OnPointerExit(eventData);
                targetImage.sprite = normal;
#endif
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_EDITOR
            base.OnPointerUp(eventData);
            targetImage.sprite = normal;
#endif
        }
    }
}
