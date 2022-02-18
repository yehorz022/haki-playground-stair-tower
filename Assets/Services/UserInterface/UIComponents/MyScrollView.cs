using UnityEngine;

[RequireComponent(typeof(MyScrollRect))]
public class MyScrollView : MonoBehaviour {
    public const int DEFAULT = 0;
    public const int OLD_STATE = 1;
    public const int GO_TO_TOP = 2;

    //if dynamically ad panels on runtime
    [SerializeField] bool dynamicCreation; //is scrolling dynamic
    [SerializeField] bool correctPosition; //move to correctPosition
    public Transform panelsParent;
    [SerializeField] GameObject panelPrefab; // prefab of panel in scroll list
    [HideInInspector] public int data; // for saving catNo or modeNo
    [HideInInspector] public RectTransform content;
    [SerializeField] int totalPanels;
    [SerializeField]public float startGap; // gap at the start in scroll list
    [SerializeField] float endGap; // gap at the end in scroll list
    [SerializeField] float gap; // gaps among panels in scroll list
    int minPanels;
    int myScreenResolutionChangeNo;
    float panelSize; // size of each panel in scroll list

    public delegate void LoadPanelDelegate(Transform panel, int reset);
    public LoadPanelDelegate LoadPanel;

    //common properties
    bool vertical; //is horizontal or verticle
    Vector2 lastScrollValue;
    static float scrollingTime; // check scrolling to stop laging or struckness
    [HideInInspector] public MyScrollRect scrollRect; //Get the current scroll rect so we can disable it if the other one is scrolling

    void Awake() {
        scrollRect = GetComponent<MyScrollRect>();
        vertical = scrollRect.vertical; //If the current scroll Rect has the vertical checked then the other one will be scrolling horizontally.
        content = scrollRect.content;
        if (dynamicCreation || correctPosition) { 
            scrollRect.onValueChanged.AddListener(OnValueChanged);
            panelSize = vertical ? panelPrefab.GetComponent<RectTransform>().sizeDelta.y : panelPrefab.GetComponent<RectTransform>().sizeDelta.x;
            minPanels = Mathf.CeilToInt((vertical ? 1f * Screen.height / Screen.width * UI.MaxResolution : UI.MaxResolution) / (panelSize + gap)) + 1;
        }
        OnScreenResolutionChanged(); // to set end gaps at start
    }

    void OnScreenResolutionChanged () { // call first time from UI class in set resolution func when ScreenResolutionChangeNo++
        panelSize = vertical ? panelPrefab.GetComponent<RectTransform>().sizeDelta.y : panelPrefab.GetComponent<RectTransform>().sizeDelta.x;
        if (correctPosition)
            endGap = (vertical ? UI.CanvasSize.y : UI.CanvasSize.x) % (panelSize + gap) - startGap;
        int childs = GetChildCountWRTNaming(); // in the last bcz after naming panels
        content.sizeDelta = new Vector2(!vertical ? childs * (panelSize + gap) + startGap + endGap : content.sizeDelta.x, vertical ? childs * (panelSize + gap) + startGap + endGap : content.sizeDelta.y);
        myScreenResolutionChangeNo = UI.ScreenResolutionChangeNo;
    }

    public ScrollState Initialize(int _totalPanels, Transform _panelsParent, LoadPanelDelegate _LoadPanel, ScrollState oldState, int reset = DEFAULT, MyScrollRect _otherScrollRect = null) {
        //print("Initialize//  " + (transform.parent.parent ? transform.parent.parent.name : "") + " / " + transform.parent.name + "  /  " + transform.name);
        ScrollState thisState = new ScrollState(this);
        data = oldState.data;
        LoadPanel = _LoadPanel;
        totalPanels = _totalPanels;
        panelsParent = _panelsParent;
        scrollRect.velocity = Vector2.zero;
        if (_otherScrollRect)
            scrollRect.otherScrollRect = _otherScrollRect;
        for (int i = panelsParent.childCount; i < minPanels && i < totalPanels; i++) // creating panels if not exist
            Code.AddPanel(panelPrefab, panelsParent.GetComponent<RectTransform>(), startGap, gap, endGap, vertical); //This is creating panels
        if (panelsParent.childCount > 0)
        for (int i = 0; i < panelsParent.childCount; i++) // set panel visible or not
            panelsParent.GetChild(i).gameObject.SetActive(i < totalPanels);
        if (reset == OLD_STATE || reset == GO_TO_TOP)
            content.anchoredPosition = reset == OLD_STATE ? oldState.anchoredPos : Vector2.zero;
        int activeChilds = GetActiveChildCount(); // 
        for (int i = 0; i < panelsParent.childCount; i++) {
            RectTransform panel = panelsParent.GetChild(i).GetComponent<RectTransform>();
            int child = reset == OLD_STATE ? (i < oldState.childs.Length ? oldState.childs[i] : i) : (i < thisState.childs.Length ? thisState.childs[i] : i); // totalPanels - oldState.childs.Length + i for last 6 panels
            if (child > totalPanels - activeChilds + i)
                child = totalPanels - activeChilds + i; // totalPanels - oldState.childs.Length + i for last 6 panels
            panel.name = child.ToString();
            Vector2 targetPos = (vertical ? new Vector2(0, -1) : new Vector2(1, 0)) * (child * (panelSize + gap) + startGap + (!vertical ? panel.pivot.x : 1 - panel.pivot.y) * panelSize);
            if (reset != OLD_STATE && panel.gameObject.activeInHierarchy)
                Animate.MoveAnchorPos(panel, panel.anchoredPosition, targetPos); // moving animation
            else
                panel.anchoredPosition = targetPos;
            //bool inView = vertical ? panel.position.y + (panelSize / 2) >= 0 && panel.position.y - (panelSize / 2) < Screen.height : panel.position.x + (panelSize / 2) >= 0 && panel.position.x - (panelSize / 2) < Screen.width;
            LoadPanel(panelsParent.GetChild(i), DEFAULT); // flag for is in view or not
        }
        int childs = GetChildCountWRTNaming (); // in the last bcz after naming panels
        content.sizeDelta = new Vector2(!vertical ? childs * (panelSize + gap) + startGap + endGap : content.sizeDelta.x, vertical ? childs * (panelSize + gap) + startGap + endGap : content.sizeDelta.y);
        return thisState;
    }

    public void OnValueChanged(Vector2 value) {
        //print("OnValueChanged//  " + (transform.parent.parent ? transform.parent.parent.name : "") + " / " + transform.parent.name + "  /  " + transform.name);
        if (myScreenResolutionChangeNo != UI.ScreenResolutionChangeNo)
            OnScreenResolutionChanged(); // set appropriate spaces at the end of scroll panels and sub scroll panels
        if (dynamicCreation) { 
            Vector2 sizeDelta = content.sizeDelta - UI.CanvasSize;
            value = new Vector2(value.x * sizeDelta.x, value.y * sizeDelta.y);
            if ((vertical && (value.y < endGap || value.y + UI.CanvasSize.y > minPanels * (panelSize + gap) + endGap)) || (!vertical && (value.x > sizeDelta.x - endGap || value.x - UI.CanvasSize.x < sizeDelta.x - minPanels * (panelSize + gap) - endGap))) { //when slider reach at start or end
                bool next = (vertical && value.y < endGap) || (!vertical && value.x > sizeDelta.x - endGap);
                Code.ShiftPanel(content, panelsParent, vertical, next, minPanels, totalPanels, panelSize, startGap, endGap, gap, (child, childNo)=> {
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

    void MoveToCorrectPosition() {
#if !UNITY_EDITOR
        if (Input.touchCount == 0) {
#else
        if (!Input.GetMouseButton(0)) {
#endif
            if (Mathf.Abs(scrollRect.velocity.x) < 100f) {
                scrollRect.StopMovement();
                Vector2 correctPos = GetPanelCorrectPosition(content);
                if (Vector2.Distance(content.anchoredPosition, correctPos) > .01f)
                    content.anchoredPosition += (correctPos - content.anchoredPosition) / 10f; // 10 is speed
            }
        } // Move to Correct Place .... ends
    }

    public static bool IsScrolling() {
        return (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved) || scrollingTime >= Time.time;
    }

    Vector2 GetPanelCorrectPosition(RectTransform panel) {
        int indexPos = GetIndexPosition(panel);  //finding index of position of panel to follow,
        //print((((Mathf.Abs(panel.anchoredPosition.x) + UI.CanvasSize.x) - startGap - endGap + panelSize / 2) / (panelSize + gap)) + "   " + (Mathf.Abs(panel.anchoredPosition.x) + UI.CanvasSize.x - startGap - endGap + panelSize / 2) + " === " + indexPos + "  panelsInView  " + panelsInView);
        return new Vector2(-((indexPos > totalPanels ? totalPanels : indexPos)) * (panelSize + gap), 0); // if indexpos is grater than total panel then make it equal to total panels
    }

    int GetIndexPosition(RectTransform panel) {
        return Mathf.RoundToInt(((vertical ? Mathf.Abs(panel.anchoredPosition.y) : Mathf.Abs(panel.anchoredPosition.x)) - gap / 2f) / (panelSize + gap));  //finding index of position of panel to follow,
    }

    int GetChildCountWRTNaming () {
        for (int i = panelsParent.childCount - 1; i >= 0; i--)
            if (panelsParent.GetChild(i).gameObject.activeSelf)
                return int.Parse(panelsParent.GetChild(i).name) + 1; //bcz child count is 1 greator than child index 
        return 0; // default value is 0 bcz count can not be -ve
    }

    int GetActiveChildCount () {
        int count = 0;
        for (int i = 0; i < panelsParent.childCount; i++)
            if (panelsParent.GetChild(i).gameObject.activeSelf)
                count++; //bcz child count is 1 greator than child index 
        return count; // default value is 0 bcz count can not be -ve
    }
}

[System.Serializable]
public class ScrollState { //for saving state
    public int data; // for saving catNo or modeNo
    public Vector2 sizeDelta;
    public Vector2 anchoredPos;
    public int[] childs = new int[0];

    public ScrollState(int _data) {
        data = _data;
    }

    public ScrollState (MyScrollView scrollview) {
        data = scrollview.data;
        sizeDelta = scrollview.content.sizeDelta;
        anchoredPos = scrollview.content.anchoredPosition;
        childs = new int[scrollview.panelsParent.childCount];
        for (int i = 0; i < childs.Length; i++)
            childs[i] = int.Parse(scrollview.panelsParent.GetChild(i).name);
    }
}
