using Assets.Services.ComponentConnection;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Services.ComponentService
{
    public class PopulateGridLayout : MonoBehaviour
    {

        [SerializeField] Color IconBGColor;
        [SerializeField] MyScrollView scrollView;
        [SerializeField] List<ComponentConnectionService> elements;
        [SerializeField] GameObject[] assemblies;  // This is just for testing purpose
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

            // vvvvv This code is for just testing purpose vvvvv
            for (int i = 0; i < assemblies.Length; i++)
            {
                elements.Add(Instantiate(assemblies[i]).AddComponent<ComponentConnectionService>());
                elementsIcons.Add(iconMaker.CreateIcon(elements[elements.Count - 1].gameObject, IconBGColor));
                ocm.Cache(elements[elements.Count - 1]);
            }
            // ^^^^^ This code is for just testing purpose ^^^^^
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