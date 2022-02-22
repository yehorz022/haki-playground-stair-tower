using System.Collections.Generic;
using Assets.Services.ComponentConnection;
using UnityEngine;

namespace Assets.Services.ComponentService
{
    public class PopulateGridLayout : MonoBehaviour
    {

        [SerializeField] Color IconBGColor;
        [SerializeField] MyScrollView scrollView;
        [SerializeField] List<ComponentConnectionService> elements;
        [SerializeField] List<Texture2D> elementsIcons = new List<Texture2D>();
        ObjectCacheManager ocm;
        MeshIconMaker iconMaker = new MeshIconMaker();

        public static PopulateGridLayout instance;
        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            ocm = FindObjectOfType<ObjectCacheManager>();
            PopulateItems();
            ScrollState scrollState = new ScrollState(0);
            scrollView.Initialize(elements.Count, transform.GetComponent<RectTransform>(), LoadPanel, scrollState, MyScrollView.OLD_STATE);
        }

        void PopulateItems()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i] = ocm.Instantiate(elements[i], Quaternion.identity);
                elementsIcons.Add(iconMaker.CreateIcon(elements[i].gameObject, IconBGColor));
                ocm.Cache(elements[i]);
            }
        }

        public void LoadPanel(Transform panel, int reset = MyScrollView.DEFAULT)
        {
            panel.GetComponent<PanelComponent>().SetImage(elementsIcons[int.Parse(panel.name)], elements[int.Parse(panel.name)]);
        }

        public void OnDeviceOrientationChange()
        {
            if (UI.orientation == DeviceOrientation.Portrait || UI.orientation == DeviceOrientation.PortraitUpsideDown)
                scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(120, scrollView.GetComponent<RectTransform>().sizeDelta.y);
            if (UI.orientation == DeviceOrientation.LandscapeLeft || UI.orientation == DeviceOrientation.LandscapeRight)
                scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(220, scrollView.GetComponent<RectTransform>().sizeDelta.y);
            scrollView.Initialize(elements.Count, transform.GetComponent<RectTransform>(), LoadPanel, new ScrollState(0));
        }

    }
    
}