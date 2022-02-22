using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Services.ComponentService;

public class UI : MonoBehaviour {
    public const int MinResolution = 680;
    public const int MaxResolution = 1100;
    public static Vector2 CanvasSize;
    public static int ScreenResolutionChangeNo = -1; //-1 to negate 1st change // for updating change, if screen resolution changes
    public static Action OnUpdateMoney;
    public static Action<string> OnUpdateName;
    public static Action<string> OnUpdateRank;
    public static Action OnScreenResolutionChanged;

    [HideInInspector] public Transform panelsView;
    public Coroutine routineDrag;
    [HideInInspector] public CanvasScaler cs;
    [SerializeField] PopulateGridLayout populateGridLayout;
    public static DeviceOrientation orientation = DeviceOrientation.Unknown;
    float deltaDrag;

    public static UI instance;
    void Awake() {
        instance = this;
        cs = FindObjectOfType<CanvasScaler>();
        float ScreenWidthInch = Screen.width / Screen.dpi;
        //SetScreenResolution(PlayerPrefs.GetFloat(Prefs.ScreenResolution, 800 + (ScreenWidthInch - 3f) * 100)); //adjust canvas size according to screen width,, //-3 bcz 3 inches is standard size 

        // The next line is not valid syntax: what should it be?
    }

    void OnRectTransformDimensionsChange() {
        if (Input.deviceOrientation != orientation) {
            orientation = Input.deviceOrientation;
            if (orientation == DeviceOrientation.Portrait || orientation == DeviceOrientation.PortraitUpsideDown) {
                cs.matchWidthOrHeight = 0;
                Code.WaitAndCall(.1f, () => populateGridLayout.OnDeviceOrientationChange());
            }
            else if (orientation == DeviceOrientation.LandscapeLeft || orientation == DeviceOrientation.LandscapeRight) {
                cs.matchWidthOrHeight = 1;
                Code.WaitAndCall(.1f, () => populateGridLayout.OnDeviceOrientationChange());
            }
        }
        //print("OnRectTransformDimensionsChange");
    }

    public void OnBeginDrag () {
        deltaDrag = 0;
        Animate.Stop (routineDrag);
        InputUI.OnInputDown();
    }

    public void OnEndDrag() {
        InputUI.OnInputUp();
        Transform trans = panelsView.GetChild(GetFirstActiveChild()).transform;
        routineDrag = Animate.Lerp(trans.position.y, deltaDrag > Screen.height / 8f ? Screen.height * 1.1f : Screen.height / 2f, .18f, 
            (val) => {
                trans.position = new Vector3(trans.position.x, val, trans.position.z);
                //if (trans.position.y > Screen.height && trans.gameObject.activeSelf)
                    //trans.GetComponent<Panel>().Close();
            }); // moving animation
    }

    public void OnDrag() {
        if (!panelsView.GetComponent<IButton>().interactable)
            return;
        float drag = InputUI.Drag().y;
        deltaDrag += drag * Screen.width; // multiply with screen width bcz input drag func returns divided with screen width
        panelsView.GetChild(GetFirstActiveChild()).position += new Vector3 (0, drag * 1000f, 0);
    }

    public void SetScreenResolution(float value) {
        bool dirty = cs.referenceResolution.x != value;
        cs.referenceResolution = new Vector2(Mathf.Clamp(value, MinResolution, MaxResolution), cs.referenceResolution.y); // clamp canvas witdh btw min and max value
        CanvasSize = new Vector2((1f - cs.matchWidthOrHeight) * cs.referenceResolution.x + cs.matchWidthOrHeight * cs.referenceResolution.y * Screen.width / Screen.height,
            cs.matchWidthOrHeight * cs.referenceResolution.y + (1f - cs.matchWidthOrHeight) * cs.referenceResolution.x * Screen.height / Screen.width);
        if (dirty) { //if no change
            PlayerPrefs.SetFloat(Prefs.ScreenResolution, cs.referenceResolution.x);
            ScreenResolutionChangeNo++; // tells all scrollviews to update canvas size etc
            if (OnScreenResolutionChanged != null)
                OnScreenResolutionChanged();
        }
    }

    public static GameObject CreatePanel (GameObject panel, Transform parent) {
        panel = Instantiate(panel);
        panel.transform.SetParent(parent);
        panel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        panel.transform.localScale = Vector2.one;
        panel.gameObject.SetActive(false);
        return panel;
    }

    public static void PlayAnimation (Animator animator, string name) {
        animator.Rebind();
        animator.Play(name);
    }

    public static void SlightlyMoveDownLeft(RectTransform trans) {
        Animate.MoveAnchorPos(trans, trans.anchoredPosition + new Vector2(30, 30), trans.anchoredPosition, .3f); // moving animation
    }

    public static void SlightlyMoveUpLeft(RectTransform trans) {
        Animate.MoveAnchorPos(trans, trans.anchoredPosition + new Vector2(30, -30), trans.anchoredPosition, .3f); // moving animation
    }

    public static void SlightlyMoveRight(RectTransform trans) {
        Animate.MoveAnchorPos(trans, trans.anchoredPosition - new Vector2(30, 0), trans.anchoredPosition, .3f);
    }

    public static void SlightlyMoveUp(RectTransform trans) {
        Animate.MoveAnchorPos(trans, trans.anchoredPosition - new Vector2(0, 30), trans.anchoredPosition, .3f);
    }

    public static void SlightlyMoveDown(RectTransform trans) {
        Animate.MoveAnchorPos(trans, trans.anchoredPosition + new Vector2(0, 30), trans.anchoredPosition, .3f);
    }

    public static IEnumerator ContentsScaleAnim(Transform content, float seconds = 0) {
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < content.childCount; i++)
            Animate.Scale(content.GetChild(i), Vector3.one/1.3f, Vector3.one); //openeing scaling animation
    }

    public static IEnumerator ContentsOpenAnim(Transform content, float seconds = 0) {
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < content.childCount; i++) {
            Animate.Open(content.GetChild(i)); //openeing scaling animation
            yield return new WaitForFixedUpdate();
        }
    }

    public static IEnumerator ContentsFadeInAnim(MonoBehaviour reference, RectTransform content, float seconds = 0) {
        yield return new WaitForSeconds(0);
        for (int i = 0; i < content.childCount; i++) {
            reference.StartCoroutine(Animate.FadeImage(content.GetChild(i).GetComponent<Image>(), 0, 1));
            yield return new WaitForFixedUpdate();
        }
    }

    public static IEnumerator ContentsSlideInAnim(Transform content, float seconds = 0) {
        yield return new WaitForSeconds(0);
        for (int i = 0; i < content.childCount; i++) // set position first
            content.GetChild(i).GetComponent<RectTransform>().anchoredPosition = content.GetChild(i).GetComponent<RectTransform>().anchoredPosition - new Vector2(20, 0);
        for (int i = 0; i < content.childCount; i++) {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Animate.MoveAnchorPos(content.GetChild(i).GetComponent<RectTransform>(), content.GetChild(i).GetComponent<RectTransform>().anchoredPosition, content.GetChild(i).GetComponent<RectTransform>().anchoredPosition + new Vector2(20, 0), .3f);
        }
    }

    public static void ToggleSection (RectTransform section) {
        bool toggle = section.gameObject.activeSelf;
        section.gameObject.SetActive(!section.gameObject.activeSelf);
        int index = section.GetSiblingIndex();
        float height = section.sizeDelta.y;
        for (int i = index; i < section.parent.childCount; i++)
            section.parent.GetChild (i).GetComponent<RectTransform> ().anchoredPosition = new Vector2 (section.parent.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x, section.parent.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y + height * (toggle ? 1 : -1));
        float size = 0;
        for (int i = 0; i < section.parent.childCount; i++)
            size += section.parent.GetChild(i).gameObject.activeSelf ? section.parent.GetChild(i).GetComponent<RectTransform>().sizeDelta.y : 0;
        section.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(section.parent.GetComponent<RectTransform>().sizeDelta.x, size);
    }

    public static string GetMoney (int value, string minDigits ="0", int sigificantFigure = 10000) {
        if (value < sigificantFigure)
            return (value).ToString(minDigits);
        else if (value < 1000000)
            return (value / 1000) + "K";
        else if (value < 1000000000)
            return (value / 1000000) + "M";
        else
            return (value / 1000000000) + "B";
    }

    public static string GetNoun (string word, int amount) {
        return word + (amount > 1 && word[word.Length - 1] != 's' ? "s" : "");
    }

    public static string CopyRightSafe(string sentence) {
        string[] words = sentence.Split(' ');
        string result = "";
        for (int i = 0; i < words.Length; i++) {
            int oneThird = Mathf.FloorToInt (words[i].Length / 3f);
            result += words[i].Substring(0, oneThird);
            for (int j = 0; j < words[i].Length - oneThird * 2; j++)
                result += "%";
            result += words[i].Substring(words[i].Length - oneThird, oneThird) + (i < words.Length - 1? " " : "");
        }
        return result;
    }

    public static IEnumerator UpdateMoneyAnimation(int origAmount, int targetAmount, System.Action<bool, int> callback) {
        if (origAmount != targetAmount) {
            int i = 0;
            float speed = 1f / (targetAmount - origAmount);
            int skipCount = Mathf.Abs (targetAmount - origAmount) / 100;
            int sign = (int)Mathf.Sign(targetAmount - origAmount);
            while (origAmount != targetAmount) {
                callback(false, origAmount = origAmount + sign);
                if (--i < 0) {
                    yield return new WaitForSeconds(speed);
                    i = skipCount;
                }
            }
        }
        callback(true, origAmount);
    }

    int GetFirstActiveChild () {
        for (int i = 0; i < panelsView.childCount; i++)
            if (panelsView.GetChild(i).gameObject.activeSelf)
                return i; //bcz child count is 1 greator than child index 
        return 0; // default value is 0 bcz count can not be -ve
    }
}