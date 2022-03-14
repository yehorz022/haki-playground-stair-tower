using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class ComponentsLayout : SceneMemberInjectDependencies
    {

        [SerializeField] Color iconBGColor;
        [SerializeField] ScrollViewComponent scrollView;
        [SerializeField] public HakiComponent[] elements;
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
                scrollView.Initialize(elements.Length, scrollView.panelsParent, LoadPanel, scrollState, ScrollViewComponent.OLD_STATE);
            });
        }

        public void Show() => Routine.MovePivot(transform.GetComponent<RectTransform>(), transform.GetComponent<RectTransform>().pivot, new Vector2(1, 1), .18f); // opening animation

        public void Hide() => Routine.MovePivot(transform.GetComponent<RectTransform>(), transform.GetComponent<RectTransform>().pivot, new Vector2(0, 1), .18f); // closing animation

        void PopulateItems()
        {
            elementsIcons = new Sprite[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                var element = ObjectcacheManager.Instantiate(elements[i], positionProvider.origin);
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
            scrollView.Initialize(elements.Length, scrollView.panelsParent, LoadPanel, new ScrollState(0));
        }
    }

}