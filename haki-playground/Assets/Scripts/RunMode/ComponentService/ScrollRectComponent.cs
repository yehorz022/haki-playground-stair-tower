using Assets.Scripts.Shared.Behaviours;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.Shared.Helpers;

namespace Assets.Scripts.RunMode.ComponentService
{
    //>>> This Scroll Rect class enable verticle swipe to drag scroller while horizontal swipe to create items
    //(((((((->>>>>>     The script is under working     <<<<<<-))))))))

    public class ScrollRectComponent : ScrollRect
    {
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
            if (horizontalSwipe1 * 2 > verticalSwipe1)
            {   
                //This script is under working...
                InputHandler.instance.OnCreateItem(); 
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
                //This script is under working...
                if (InputHandler.instance.onRecycleBin)
                    InputHandler.instance.OnClickRecycleButton(); 
                else
                    InputHandler.instance.OnDropItem();
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