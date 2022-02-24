using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    public class Inputs
    {
        const float DOUBLE_TAP_WAIT = 0.25f;
        const float LONG_TAP_WAIT = 1f;
        // General
        public static Vector2 SCREEN_SIZE; // screen size (width, height) in world
        static int skip; //use to avoid jerk
        static bool rePosition; // flag to reposition image
        static float MIN_SWIPE_DISTANCE;
        static Vector2 firstPosition;
        static Vector2 lastPosition;
        static Vector2 INPUT_AREA; // for maintain pic in canvas

        //Click
        static Coroutine doubleClickWait;
        //Zoom
        public static float minZoom = 1;
        public static float maxZoom = 20;
        public static float zoomSpeed = 4; // The rate of change of zoom.
        static float lastZoom;
        static float lastTapTime; // for zoom on double tap
        static float startTapTime; // for zoom on double tap

        public static void Initialize()
        {
            SCREEN_SIZE = INPUT_AREA = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) - Camera.main.ScreenToWorldPoint(Vector2.zero);
            MIN_SWIPE_DISTANCE = Screen.dpi * .05f;
        }

        public static void OnInputDown()
        {
            startTapTime = Time.time;
            firstPosition = Input.mousePosition;
            skip = 0;
        }

        public static void OnInputUp()
        {
            skip = 1;
        }

        public static Vector2 Drag()
        {
            Vector2 deltaPos = Vector2.zero;
#if !UNITY_EDITOR
            if (Input.touchCount > 0)
                deltaPos = Input.touches[0].deltaPosition;
#else
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                deltaPos = (Vector2)Input.mousePosition - lastPosition;
            lastPosition = Input.mousePosition;
            if (skip >= 0)
            { deltaPos = Vector2.zero; skip--;
            } //skip once or twice to avoid jerk
#endif
            return new Vector2(deltaPos.x / Screen.width, deltaPos.y / Screen.width);
        }

        public static float Zoom(Transform target = null, Transform content = null)
        {
#if !UNITY_EDITOR
            if (Input.touchCount < 2)
                return 0; //new Vector2(Input.touches [0].position.x / Screen.width, Input.touches [0].position.y / Screen.height);
		    float currZoom = (Camera.main.ScreenToWorldPoint (Input.touches [0].position) - Camera.main.ScreenToWorldPoint (Input.touches [1].position)).magnitude;
            float zoom = currZoom - lastZoom;
		    lastZoom = currZoom;
            if (skip >= 0) { skip--; return 0; }//skip once or twice to avoid jerk
		    return zoom;
#else
            return Input.GetAxis("Mouse ScrollWheel");
#endif
        }

        public static Click InputType()
        {
            int upsCount = 0;
            foreach (Touch t in Input.touches)
                if (t.phase == TouchPhase.Ended)
                    upsCount++;
            if (Input.touches.Length == upsCount && Vector2.Distance(Input.mousePosition, firstPosition) < MIN_SWIPE_DISTANCE)
            {
                if (Time.time - lastTapTime <= DOUBLE_TAP_WAIT && Vector2.Distance(Input.mousePosition, lastPosition) < MIN_SWIPE_DISTANCE)
                {// checking double tap
                    return Click.DoubleTap;
                }
                else
                {
                    lastPosition = Input.mousePosition;
                    lastTapTime = Time.time;
                    return Time.time - startTapTime > LONG_TAP_WAIT ? Click.LongTap : Click.Tap;
                }
            }
            return Click.Drag;
        }

        public static void WaitInputType(System.Action<Click> action)
        {
            Click click = InputType();
            if (click == Click.DoubleTap)
            {
                Routine.Stop(doubleClickWait);
                action(click);
            }
            else if (click == Click.Tap)
                doubleClickWait = Routine.WaitAndCall(DOUBLE_TAP_WAIT, () => action(click));
        }

        public static void ZoomingBackToBoundaries(Transform target, Transform content)
        {
            if (target.localScale.x < minZoom)
                SetTargetOnZoom(target, content, Mathf.Lerp(target.localScale.x, minZoom, Time.deltaTime * 10), Camera.main.ScreenToWorldPoint(firstPosition));
            else if (target.localScale.x > maxZoom)
                SetTargetOnZoom(target, content, Mathf.Lerp(target.localScale.x, maxZoom, Time.deltaTime * 10), Camera.main.ScreenToWorldPoint(firstPosition));
            RePosition(target);
        }

        public static void SetTargetOnZoom(Transform target, Transform content, float value, Vector2 position)
        {
            Vector2 delta = (Vector2)target.localPosition - position;
            target.localPosition -= (Vector3)delta;
            content.localPosition += (Vector3)delta / target.localScale.x;
            target.localScale = Vector3.one * value;
            if (target.localScale.x < .03f)
                target.localScale = new Vector3(.03f, .03f, 1);
            else if (target.localScale.x > 100f)
                target.localScale = new Vector3(100f, 100f, 1);
            delta = content.localPosition;
            content.localPosition -= (Vector3)delta;
            target.localPosition += (Vector3)delta * target.localScale.x;
        }

        static void RePosition(Transform target)
        {
            if (rePosition)
            { // keep the Picture in middle using lerp
                if (target.position.x < 0 || target.position.x > 0)
                    target.position = new Vector3(Mathf.Lerp(target.position.x, 0, Time.deltaTime * 20), target.position.y, target.position.z);
                if (target.position.y < 0 || target.position.y > 0)
                    target.position = new Vector3(target.position.x, Mathf.Lerp(target.position.y, 0, Time.deltaTime * 20), target.position.z);
                if (rePosition && Mathf.Abs(target.position.x) < .01f) // set reposition flag false 
                    rePosition = false;
            }
            if (target.localScale.x < minZoom - .01f && !rePosition) // if Zoom Less Than MinZoom set reposition flag true 
                rePosition = true;
        }

        public static int GetTouchDirection(Touch touch)
        {
            if (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
                return touch.deltaPosition.x >= 0 ? 1 : 2;
            else
                return touch.deltaPosition.y >= 0 ? 3 : 4;
        }

        public static void SetBoundaries(Transform target)
        { // when input not zooming and lerping will change zoom // keep the target with in boundaries
          //Vector2 picSize = new Vector2 (targetExtent < SCREEN_EXTENT.x ? targetExtent : SCREEN_EXTENT.x, targetExtent < SCREEN_EXTENT.y ? targetExtent : SCREEN_EXTENT.y); // detect whether pic within screen or not. if in screen then limit min and max Drag wrt to target cordinates other wise screen corinates
            Vector2 targetExtent = SCREEN_SIZE / 2 * target.localScale; // as target.position is center point so adding extent to make comparison of target edges with screen edges
            Vector2 minDrag = new Vector2(-INPUT_AREA.x - targetExtent.x, -INPUT_AREA.y - targetExtent.y);
            Vector2 maxDrag = new Vector2(+INPUT_AREA.x + targetExtent.x, +INPUT_AREA.y + targetExtent.y);
            if (target.position.x < minDrag.x)
                target.position = new Vector3(minDrag.x, target.position.y, target.position.z);
            else if (target.position.x > maxDrag.x)
                target.position = new Vector3(maxDrag.x, target.position.y, target.position.z);
            if (target.position.y < minDrag.y)
                target.position = new Vector3(target.position.x, minDrag.y, target.position.z);
            else if (target.position.y > maxDrag.y)
                target.position = new Vector3(target.position.x, maxDrag.y, target.position.z);
        }
    }
    public enum Click
    {
        None = -1,
        Tap = 0,
        Drag = 1,
        Zoom = 2,
        DoubleTap = 3,
        LongTap = 4
    };
}

