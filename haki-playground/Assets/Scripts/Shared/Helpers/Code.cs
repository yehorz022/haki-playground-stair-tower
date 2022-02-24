using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.Shared.Helpers
{
    //>>>This is a class containing generic functions

    public static class Code {

        public static bool IsInternetAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        static AsyncOperation async = null; // When assigned, load is in progress.
        public static IEnumerator LoadASyncLevel(int levelNo, Image logo = null)
        {
            yield return new WaitForSeconds(1.5f);
            async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelNo);
            while (async.progress < 1)
            {
                if (logo)
                    logo.color = new Color(255, 255, 255, async.progress);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        public static GameObject Instantiate(GameObject prefab, Transform parent)
        {
            Transform trans = GameObject.Instantiate(prefab).transform;
            Vector3 position = trans.GetComponent<RectTransform>().anchoredPosition;
            Vector3 localScale = trans.localScale;
            trans.SetParent(parent);
            trans.GetComponent<RectTransform>().anchoredPosition = position;
            trans.localScale = localScale;
            return trans.gameObject;
        }

        public static void ClearChilds(Transform content, int i = 0)
        {
            for (; i < content.childCount; i++)
                GameObject.Destroy(content.GetChild(i).gameObject);
        }

        public static Transform AddPanel(GameObject panelPrefab, RectTransform content, float startGap = 0, float gap = 0, float endGap = 0, bool vertical = true, bool fixSize = false)
        {
            RectTransform panel = GameObject.Instantiate(panelPrefab).GetComponent<RectTransform>();
            panel.SetParent(content);
            panel.name = panel.GetSiblingIndex().ToString();
            int index = content.childCount;
            float panelSize = vertical ? panel.sizeDelta.y : panel.sizeDelta.x;
            content.sizeDelta = new Vector2(!vertical ? index * (panelSize + gap) + startGap + endGap : content.sizeDelta.x, vertical ? index * (panelSize + gap) + startGap + endGap : content.sizeDelta.y);
            panel.anchoredPosition = ((panelSize + gap) * (index - 1) + startGap + (!vertical ? panel.pivot.x : 1 - panel.pivot.y) * panelSize) * (vertical ? new Vector2(0, -1) : new Vector2(1, 0));
            panel.localScale = Vector3.one;
            if (!fixSize)
                panel.sizeDelta = panelSize * (vertical ? new Vector2(0, 1) : new Vector2(1, 0));
            return panel;
        }

        public static void ShiftPanel(RectTransform content, Transform parent, bool vertical, bool next, int minPanels, int totalPanels, float panelSize, float startGap, float endGap, float gap, Action<RectTransform, int> action = null)
        { //dir true = shift right
            if (parent.childCount < minPanels)
                return;
            int panelNo = int.Parse(parent.GetChild(parent.childCount - 1).name) + 1; //plus panelSize for adding extents of start and bottom
            if (content.childCount > 0 && ((!next && panelNo >= minPanels + 1) || (next && panelNo > (minPanels - 1) && panelNo < totalPanels)))
            {
                RectTransform childToMove = parent.GetChild(next ? 0 : minPanels - 1).GetComponent<RectTransform>();
                float sizeDelta = (panelNo + (next ? 1 : -1)) * (panelSize + gap) + startGap + endGap;
                content.sizeDelta = vertical ? new Vector2(content.sizeDelta.x, sizeDelta) : new Vector2(sizeDelta, content.sizeDelta.y); //startPos = 135, endPos = -55
                childToMove.anchoredPosition = (vertical ? Vector2.down : Vector2.right) * ((panelSize + gap) * (next ? panelNo : panelNo - (minPanels + 1)) + startGap + (!vertical ? childToMove.pivot.x : 1 - childToMove.pivot.y) * panelSize);
                childToMove.gameObject.SetActive(true); // if hided by load category sub panel
                childToMove.SetSiblingIndex(next ? minPanels - 1 : 0); //set new child no
                childToMove.name = (next ? panelNo : panelNo - (minPanels + 1)).ToString();
                action(childToMove, next ? panelNo : panelNo - (minPanels + 1)); //load panel
            }
        }
    }
}
