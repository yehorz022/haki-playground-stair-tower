using Assets.Scripts.RunMode.ComponentConnection;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class PopulateGridLayout : SceneMemberInjectDependencies
    {

        [SerializeField] Color iconBGColor;
        [SerializeField] ScrollViewComponent scrollView;
        [SerializeField] ScaffoldingComponent[] elements;
        [SerializeField] Sprite[] elementsIcons;
        [Inject]
        private IObjectCacheManager ObjectcacheManager { get; set; }
        private PositionProvider.PositionProvider positionProvider;

        void Start()
        {
            positionProvider = FindObjectOfType<PositionProvider.PositionProvider>();

            ScrollState scrollState = new ScrollState(0);
            Routine.WaitAndCall(.01f, () => //wait for system to initialize first
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
                ScaffoldingComponent element = ObjectcacheManager.Instantiate(elements[i]);
                //IconMaker create camera on runtime because we only need camera once to create icons in first time opening the app and second time it pick from persistent path
                //otherwise it will become heavy call for creating 200 or more icon everytime and it will take 1-2 seconds on opening the app everytime
                elementsIcons[i] = Media.TextureToSprite(IconMaker.CreateIcon(element.gameObject, iconBGColor));
                ObjectcacheManager.Cache(element);
            }
        }

        public void LoadPanel(Transform panel, int reset = ScrollViewComponent.DEFAULT)
        {
            panel.GetComponent<PanelComponent>().Initialize(elements[int.Parse(panel.name)], elementsIcons[int.Parse(panel.name)]);
        }

        // will structurize the below funtion later 
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