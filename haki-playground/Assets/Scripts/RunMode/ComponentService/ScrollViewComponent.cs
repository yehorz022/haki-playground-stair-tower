using System;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{

    //>>> This Scroll View class scroll much faster as compared to unity native scroller
    //>>> This class also create panels itself like unity gridLayout class
    //>>> This class also arrange the panels itself if resolution of your device changes
    //(((((((->>>>>>     The script is under working     <<<<<<-))))))))

    [RequireComponent(typeof(ScrollRectComponent))]
    public class ScrollViewComponent : MonoBehaviour
    {
        public const int DEFAULT = 0;
        public const int OLD_STATE = 1;
        public const int GO_TO_TOP = 2;

        //if dynamically ad panels on runtime
        [SerializeField] bool dynamicCreation; //is scrolling dynamic
        [SerializeField] bool correctPosition; //move to correctPosition
        public RectTransform panelsParent;
        public RectTransform panelPrefab; // prefab of panel in scroll list
        [SerializeField] int totalPanels;
        public Gap gapp;
        public float startGap; // gap at the start in scroll list
        public float endGap; // gap at the end in scroll list
        public float gap; // gaps among panels in scroll list
        [HideInInspector] public int data; // for saving catNo or modeNo
        [HideInInspector] public float panelSize; // size of each panel in scroll list
        [HideInInspector] public RectTransform content;
        public delegate void LoadPanelDelegate(Transform panel, int reset);
        public LoadPanelDelegate LoadPanel;
        int rows;
        int columns;
        int minPanels;
        int myScreenResolutionChangeNo;
        Vector2 contentSize; // size of each panel in scroll list
        Vector2 panelSizee; // size of each panel in scroll list
        Vector2 posFixDelta;
        float extraSpace;

        //common properties
        bool vertical; //is horizontal or verticle
        Vector2 lastScrollValue;
        static float scrollingTime; // check scrolling to stop laging or struckness
        [HideInInspector] public ScrollRectComponent scrollRect; //Get the current scroll rect so we can disable it if the other one is scrolling

        void Awake()
        {
            scrollRect = GetComponent<ScrollRectComponent>();
            vertical = scrollRect.vertical; //If the current scroll Rect has the vertical checked then the other one will be scrolling horizontally.
            content = scrollRect.content;
            if (dynamicCreation || correctPosition)
            {
                scrollRect.onValueChanged.AddListener(OnValueChanged);
                panelSize = vertical ? panelPrefab.GetComponent<RectTransform>().sizeDelta.y : panelPrefab.GetComponent<RectTransform>().sizeDelta.x;
                minPanels = Mathf.CeilToInt((vertical ? 1f * Screen.height / Screen.width * CanvasComponent.MaxResolution : CanvasComponent.MaxResolution) / (panelSize + gap)) + 1;
            }
            //OnScreenResolutionChanged(); // to set end gaps at start
        }

        void OnScreenResolutionChanged()
        { // call first time from UI class in set resolution func when ScreenResolutionChangeNo++
            panelSize = vertical ? panelPrefab.GetComponent<RectTransform>().sizeDelta.y : panelPrefab.GetComponent<RectTransform>().sizeDelta.x;
            if (correctPosition)
                endGap = (vertical ? CanvasComponent.CanvasSize.y : CanvasComponent.CanvasSize.x) % (panelSize + gap) - startGap;
            int childs = GetChildCountWRTNaming(); // in the last bcz after naming panels
            if (content != null)
                content.sizeDelta = new Vector2(!vertical ? childs * (panelSize + gap) + startGap + endGap : content.sizeDelta.x, vertical ? childs * (panelSize + gap) + startGap + endGap : content.sizeDelta.y);
            myScreenResolutionChangeNo = CanvasComponent.ScreenResolutionChangeNo;
        }

        public ScrollState Initialize(int _totalPanels, RectTransform _panelsParent, LoadPanelDelegate _LoadPanel, ScrollState oldState, int reset = DEFAULT, ScrollRectComponent _otherScrollRect = null)
        {
            //print("Initialize//  " + (transform.parent.parent ? transform.parent.parent.name : "") + " / " + transform.parent.name + "  /  " + transform.name);
            ScrollState thisState = new ScrollState(this);
            data = oldState.data;
            LoadPanel = _LoadPanel;
            totalPanels = _totalPanels;
            panelsParent = _panelsParent;
            if (_otherScrollRect)
                scrollRect.otherScrollRect = _otherScrollRect;
            scrollRect.velocity = Vector2.zero;
            CalculateRowsColumns();
            for (int i = panelsParent.childCount; i < totalPanels; i++) // creating panels if not exist
                Code.AddPanel(panelPrefab.gameObject, panelsParent.GetComponent<RectTransform>(), startGap, gap, endGap, vertical, true); //This is creating panels
            if (panelsParent.childCount > 0)
                for (int i = 0; i < panelsParent.childCount; i++) // set panel visible or not
                    panelsParent.GetChild(i).gameObject.SetActive(i < totalPanels);
            if (reset == OLD_STATE || reset == GO_TO_TOP)
                content.anchoredPosition = reset == OLD_STATE ? oldState.anchoredPos : Vector2.zero;
            int activeChilds = GetActiveChildCount(); // 
            int n = 0; // to terverse index of childs linearly
            if (vertical)
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columns; j++)
                        ResetPanels(i, j, n++, activeChilds, oldState, thisState, reset);
            else
                for (int j = 0; j < columns; j++)
                    for (int i = 0; i < rows; i++)
                        ResetPanels(i, j, n++, activeChilds, oldState, thisState, reset);

            //int childs = GetChildCountWRTNaming (); // in the last bcz after naming panels
            //content.sizeDelta = new Vector2(!vertical ? childs * (panelSize + gap) + startGap + endGap : content.sizeDelta.x, vertical ? childs * (panelSize + gap) + startGap + endGap : content.sizeDelta.y);
            if (vertical)
                content.sizeDelta = new Vector2(content.sizeDelta.x, panelSize * rows + gapp.middleY * (rows - 1) + extraSpace * (rows + 1) + gapp.top + gapp.bottom);
            else
                content.sizeDelta = new Vector2(panelSize * columns + gapp.middleX * (columns - 1) + extraSpace * (columns + 1) + gapp.left + gapp.right, content.sizeDelta.y);
            return thisState;
        }

        void ResetPanels(int i, int j, int n, int activeChilds, ScrollState oldState, ScrollState thisState, int reset)
        {
            if (n < totalPanels)
            {
                RectTransform panel = panelsParent.GetChild(n).GetComponent<RectTransform>();
                int child = reset == OLD_STATE ? (n < oldState.childs.Length ? oldState.childs[n] : n) : (n < thisState.childs.Length ? thisState.childs[n] : n); // totalPanels - oldState.childs.Length + i for last 6 panels
                if (child > totalPanels - activeChilds + n)
                    child = totalPanels - activeChilds + n; // totalPanels - oldState.childs.Length + i for last 6 panels
                panel.name = child.ToString();
                Vector2 targetPos = new Vector2((panelSizee.x + gapp.middleX + extraSpace) * j + gapp.left + extraSpace + posFixDelta.x, -((panelSizee.y + gapp.middleY + extraSpace) * i + gapp.top + extraSpace + posFixDelta.y));
                if (reset != OLD_STATE && panel.gameObject.activeInHierarchy)
                    Routine.MoveAnchorPos(panel, panel.anchoredPosition, targetPos); // moving animation
                else
                    panel.anchoredPosition = targetPos;

                //bool inView = vertical ? panel.position.y + (panelSize / 2) >= 0 && panel.position.y - (panelSize / 2) < Screen.height : panel.position.x + (panelSize / 2) >= 0 && panel.position.x - (panelSize / 2) < Screen.width;
                if (LoadPanel != null)
                    LoadPanel(panelsParent.GetChild(n), DEFAULT); // flag for is in view or not
            }
        }

        public void OnValueChanged(Vector2 value)
        {
            //print("OnValueChanged//  " + (transform.parent.parent ? transform.parent.parent.name : "") + " / " + transform.parent.name + "  /  " + transform.name);
            if (myScreenResolutionChangeNo != CanvasComponent.ScreenResolutionChangeNo)
                OnScreenResolutionChanged(); // set appropriate spaces at the end of scroll panels and sub scroll panels
            if (dynamicCreation)
            {
                Vector2 sizeDelta = content.sizeDelta - CanvasComponent.CanvasSize;
                value = new Vector2(value.x * sizeDelta.x, value.y * sizeDelta.y);
                if ((vertical && (value.y < endGap || value.y + CanvasComponent.CanvasSize.y > minPanels * (panelSize + gap) + endGap)) || (!vertical && (value.x > sizeDelta.x - endGap || value.x - CanvasComponent.CanvasSize.x < sizeDelta.x - minPanels * (panelSize + gap) - endGap)))
                { //when slider reach at start or end
                    bool next = (vertical && value.y < endGap) || (!vertical && value.x > sizeDelta.x - endGap);
                    Code.ShiftPanel(content, panelsParent, vertical, next, minPanels, totalPanels, panelSize, startGap, endGap, gap, (child, childNo) =>
                    {
                        //AudioManager.o.PlaySound (SoundID.Scroll); // play scrolling sound
                        LoadPanel(child, OLD_STATE);
                    }); // true or false to check which panel to create ie next or back
                }
                if ((lastScrollValue - value).SqrMagnitude() > .01f)
                    scrollingTime = Time.time + .1f;
                lastScrollValue = value;
            }
            if (correctPosition)
                MoveToCorrectPosition(); // Move to Correct Place
        }

        void MoveToCorrectPosition()
        {
#if !UNITY_EDITOR
            if (Input.touchCount == 0) {
#else
            if (!Input.GetMouseButton(0))
            {
#endif
                if (Mathf.Abs(scrollRect.velocity.x) < 100f)
                {
                    scrollRect.StopMovement();
                    Vector2 correctPos = GetPanelCorrectPosition(content);
                    if (Vector2.Distance(content.anchoredPosition, correctPos) > .01f)
                        content.anchoredPosition += (correctPos - content.anchoredPosition) / 10f; // 10 is speed
                }
            } // Move to Correct Place .... ends
        }

        public static bool IsScrolling()
        {
            return (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved) || scrollingTime >= Time.time;
        }

        Vector2 GetPanelCorrectPosition(RectTransform panel)
        {
            int indexPos = GetIndexPosition(panel);  //finding index of position of panel to follow,
            //print((((Mathf.Abs(panel.anchoredPosition.x) + UI.CanvasSize.x) - startGap - endGap + panelSize / 2) / (panelSize + gap)) + "   " + (Mathf.Abs(panel.anchoredPosition.x) + UI.CanvasSize.x - startGap - endGap + panelSize / 2) + " === " + indexPos + "  panelsInView  " + panelsInView);
            return new Vector2(-((indexPos > totalPanels ? totalPanels : indexPos)) * (panelSize + gap), 0); // if indexpos is grater than total panel then make it equal to total panels
        }

        void CalculateRowsColumns()
        { // below formula works like for 100 space, with 5 gap, 10 panelSIze, 7 panels can sit, //arrange like 10 + 5 + 10 + 5 + 10 + 5 + 10 + 5 + 10 + 5 + 10 + 5 + 10  = 100 (7 panels)
          // to get value you need to add (space + gap) / (panels + gap) i.e. 105 / 15, answer = 7
            contentSize = transform.GetComponent<RectTransform>().sizeDelta;
            panelSizee = panelPrefab.sizeDelta;
            posFixDelta = vertical ? new Vector2(-(contentSize.x * (1 - panelsParent.pivot.x)) + panelSizee.x * .5f, panelSizee.y * .5f) //if pivot x disturbs then adjust pos accordingly
                                   : new Vector2(panelSizee.x * .5f, -(contentSize.y * panelsParent.pivot.y - panelSizee.y * .5f)); //if pivot x disturbs then adjust pos accordingly
            if (vertical)
            {
                columns = Mathf.FloorToInt(1f * (contentSize.x + gapp.middleX - gapp.left - gapp.right) / (panelSizee.x + gapp.middleX));
                rows = Mathf.CeilToInt(1f * totalPanels / columns);
                extraSpace = 1f * (contentSize.x + gapp.middleX - gapp.left - gapp.right) % (panelSizee.x + gapp.middleX) / (columns + 1);
            }
            else
            {
                rows = Mathf.FloorToInt(1f * (contentSize.y + gapp.middleY - gapp.top - gapp.bottom) / (panelSizee.y + gapp.middleY));
                columns = Mathf.CeilToInt(1f * totalPanels / rows);
                extraSpace = 1f * (contentSize.y + gapp.middleY - gapp.top - gapp.bottom) % (panelSizee.y + gapp.middleY) / (rows + 1);
            }
        }

        int GetIndexPosition(RectTransform panel)
        {
            return Mathf.RoundToInt(((vertical ? Mathf.Abs(panel.anchoredPosition.y) : Mathf.Abs(panel.anchoredPosition.x)) - gap / 2f) / (panelSize + gap));  //finding index of position of panel to follow,
        }

        int GetChildCountWRTNaming()
        {
            for (int i = panelsParent.childCount - 1; i >= 0; i--)
                if (panelsParent.GetChild(i).gameObject.activeSelf)
                {
                    if (int.TryParse(panelsParent.GetChild(i).name, out int res))
                        return res;


                    //was unable to parse, returning default value.
                    return 0;
                }
            return 0; // default value is 0 bcz count can not be -ve
        }

        int GetActiveChildCount()
        {
            int count = 0;
            for (int i = 0; i < panelsParent.childCount; i++)
                if (panelsParent.GetChild(i).gameObject.activeSelf)
                    count++; //bcz child count is 1 greator than child index 
            return count; // default value is 0 bcz count can not be -ve
        }
    }

    [System.Serializable]
    public class ScrollState
    { //for saving state
        public int data; // for saving catNo or modeNo
        public Vector2 sizeDelta;
        public Vector2 anchoredPos;
        public int[] childs = new int[0];

        public ScrollState(int _data)
        {
            data = _data;
        }

        public ScrollState(ScrollViewComponent scrollview)
        {
            data = scrollview.data;
            sizeDelta = scrollview.content.sizeDelta;
            anchoredPos = scrollview.content.anchoredPosition;
            childs = new int[scrollview.panelsParent.childCount];
            for (int i = 0; i < childs.Length; i++)
                childs[i] = int.Parse(scrollview.panelsParent.GetChild(i).name);
        }
    }

    [System.Serializable]
    public class Gap
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
        public float middleX;
        public float middleY;

        public Gap(float _left, float _right, float _top, float _bottom, float _middleX, float _middleY)
        {
            left = _left;
            right = _right;
            top = _top;
            bottom = _bottom;
            middleX = _middleX;
            middleY = _middleY;
        }
    }
}