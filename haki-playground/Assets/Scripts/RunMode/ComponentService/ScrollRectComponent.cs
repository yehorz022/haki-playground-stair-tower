using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.Shared.Helpers;
using UnityEngine.Events;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class ScrollRectComponent : ScrollRect
    {
        [SerializeField] public UnityEvent<int> onBeginDrags = new UnityEvent<int>();
        [SerializeField] public UnityEvent onDrags = new UnityEvent();
        [SerializeField] public UnityEvent onEndDrags = new UnityEvent();

        static float SCROLL_SPEEDS = 2000f;
        const float SCROLL_SPEED = .7f;
        public ScrollRectComponent otherScrollRect; //for scrolling two ways at a time
        bool otherScrolling; //This tracks if the other one should be scrolling instead of the current one, if multiple scrollrects work at same time
        Vector2 deltaPositon;
        float time;
        bool limitScrolling;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            Inputs.OnInputDown();
            if (time == Time.time)
                return;
            time = Time.time;
            if (otherScrollRect)
            {
                float horizontalSwipe = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
                float verticalSwipe = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
                if ((vertical && horizontalSwipe > verticalSwipe) || (!vertical && verticalSwipe > horizontalSwipe))
                {
                    otherScrolling = true;
                    otherScrollRect.OnBeginDrag(eventData);
                    return;
                }
            }
            float horizontalSwipe1 = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
            float verticalSwipe1 = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
            onBeginDrags.Invoke(1);
            if (horizontalSwipe1 * 2 > verticalSwipe1)
            {
                InputHandler.instance.picked = InputHandler.instance.positionProvider.CreateObject(PanelComponent.selectedComponentPrefab);
                InputHandler.instance.positionProvider.SetObjectActive(InputHandler.instance.picked);
            }
            limitScrolling = horizontalSwipe1 * 2 > verticalSwipe1;

            eventData.position = deltaPositon = Vector2.zero;
            base.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (limitScrolling || time == Time.time)
                return;
            time = Time.time;
            if (otherScrolling)
            {
                otherScrollRect.OnDrag(eventData);
                return;
            }
            if (Input.touchCount <= 1 || (Input.touchCount > 1 && Inputs.GetTouchDirection(Input.touches[0]) == Inputs.GetTouchDirection(Input.touches[1])))
                deltaPositon += Inputs.Drag() * SCROLL_SPEEDS;
            //else
            //    CanvasComponent.instance.SetScreenResolution(CanvasComponent.instance.cs.referenceResolution.x - Inputs.Zoom() * 300);
            eventData.position = deltaPositon;
            base.OnDrag(eventData); //the current scroll rect doesnt move in else condition.
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (limitScrolling)
            {
                if (InputHandler.instance.onRecycleBin)
                    InputHandler.instance.RecycleComponent();
                else
                    InputHandler.instance.positionProvider.SetObjectInactive();
            }
            Inputs.OnInputUp();
            if (limitScrolling || time == Time.time)
                return;
            time = Time.time;
            if (otherScrolling)
            {
                otherScrolling = false;
                otherScrollRect.OnEndDrag(eventData);
                return;
            }
            eventData.position = deltaPositon;
            base.OnEndDrag(eventData);
        }
    }
}