using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyScrollRect : ScrollRect {
    static float SCROLL_SPEEDS = 2000f;
    const float SCROLL_SPEED = .7f;

    public MyScrollRect otherScrollRect; //for scrolling two ways at a time
    bool otherScrolling; //This tracks if the other one should be scrolling instead of the current one, if multiple scrollrects work at same time
    Vector2 deltaPositon;
    float time;
    bool limitScrolling;

    public override void OnBeginDrag(PointerEventData eventData) {
        Inpute.OnInputDown();
        if (time == Time.time)
            return;
        time = Time.time;
        if (otherScrollRect) {
            float horizontalSwipe = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
            float verticalSwipe = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
            if ((vertical && horizontalSwipe > verticalSwipe) || (!vertical && verticalSwipe > horizontalSwipe)) {
                otherScrolling = true;
                otherScrollRect.OnBeginDrag(eventData);
                return;
            }
            
        }
        float horizontalSwipe1 = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
        float verticalSwipe1 = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
        if (horizontalSwipe1 * 2 > verticalSwipe1) 
            PanelComponent.Spawn();
        limitScrolling = horizontalSwipe1 * 2 > verticalSwipe1;

        eventData.position = deltaPositon = Vector2.zero;
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData) {
        if (limitScrolling || time == Time.time)
            return;
        time = Time.time;
        if (otherScrolling) {
            otherScrollRect.OnDrag(eventData);
            return;
        }
        if (Input.touchCount <= 1 || (Input.touchCount > 1 && Inpute.GetTouchDirection(Input.touches[0]) == Inpute.GetTouchDirection(Input.touches[1])))
            deltaPositon += Inpute.Drag() * SCROLL_SPEEDS;
        else
            UI.instance.SetScreenResolution(UI.instance.cs.referenceResolution.x - Inpute.Zoom() * 300);
        eventData.position = deltaPositon;
        base.OnDrag(eventData); //the current scroll rect doesnt move in else condition.
    }

    public override void OnEndDrag(PointerEventData eventData) {
        if (limitScrolling)
            PositionProvider.instance.PlaceComponent();
        Inpute.OnInputUp();
        if (limitScrolling || time == Time.time)
            return;
        time = Time.time;
        if (otherScrolling) {
            otherScrolling = false;
            otherScrollRect.OnEndDrag(eventData);
            return;
        }
        eventData.position = deltaPositon;
        base.OnEndDrag(eventData);
    }
}
