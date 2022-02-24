using Assets.Scripts.RunMode.ComponentConnection;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class PopulateGridLayout : MonoBehaviour
    {

        [SerializeField] Color iconBGColor;
        [SerializeField] ScrollViewComponent scrollView;
        [SerializeField] ScaffoldingComponent[] elements;
        [SerializeField] Sprite[] elementsIcons;
        private ObjectCacheManager ocm;
        private PositionProvider.PositionProvider positionProvider;

        void Start()
        {
            positionProvider = FindObjectOfType<PositionProvider.PositionProvider>();
            ocm = FindObjectOfType<ObjectCacheManager>();
            ScrollState scrollState = new ScrollState(0);
            Routine.WaitAndCall (.01f, () => //wait for system to initialize first
            {
                PopulateItems();
                scrollView.Initialize(elements.Length, transform.GetComponent<RectTransform>(), LoadPanel, scrollState, ScrollViewComponent.OLD_STATE);
            }); 
        }

        void PopulateItems()
        {
            elementsIcons = new Sprite[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                ScaffoldingComponent element = ocm.Instantiate(elements[i]);
                elementsIcons[i] = Media.TextureToSprite(IconMaker.CreateIcon(element.gameObject, iconBGColor));
                ocm.Cache(element);
            }
        }

        public void LoadPanel(Transform panel, int reset = ScrollViewComponent.DEFAULT)
        {
            panel.GetComponent<PanelComponent>().Initialize(elements[int.Parse(panel.name)], elementsIcons[int.Parse(panel.name)]);
        }

        public void OnDeviceOrientationChange()
        {
            if (CanvasComponent.orientation == DeviceOrientation.Portrait || CanvasComponent.orientation == DeviceOrientation.PortraitUpsideDown)
                scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(120, scrollView.GetComponent<RectTransform>().sizeDelta.y);
            if (CanvasComponent.orientation == DeviceOrientation.LandscapeLeft || CanvasComponent.orientation == DeviceOrientation.LandscapeRight)
                scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(220, scrollView.GetComponent<RectTransform>().sizeDelta.y);
            scrollView.Initialize(elements.Length, transform.GetComponent<RectTransform>(), LoadPanel, new ScrollState(0));
        }
    }

}