using System.Collections.Generic;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class PopulateAssemblyGrid : SceneMemberInjectDependencies
    {
        [Inject]
        private IObjectCacheManager ObjectcacheManager { get; set; }
        [SerializeField] List<Sprite> elementsIcons;
        [SerializeField] Color iconBGColor;
        [SerializeField] ScrollViewComponent scrollView;
        [SerializeField] private List<ScaffoldingAssembly> elements = new List<ScaffoldingAssembly>();
        void Start()
        {
            elementsIcons = new List<Sprite>();
        }

        public void AddItem(ScaffoldingAssembly assembly)
        {
            var item = ObjectcacheManager.Instantiate(assembly);
            elements.Add(item);

            ScrollState scrollState = new ScrollState(0);
            elementsIcons.Add(Media.TextureToSprite(IconMaker.CreateIcon(assembly.gameObject, iconBGColor)));
            scrollView.Initialize(elements.Count, transform.GetComponent<RectTransform>(), LoadPanel, scrollState, ScrollViewComponent.OLD_STATE);
            item.gameObject.SetActive(false);
        }

        void LoadPanel(Transform panel, int reset = ScrollViewComponent.DEFAULT)
        {
            panel.GetComponent<PanelComponent>().Initialize(elements[int.Parse(panel.name)], elementsIcons[int.Parse(panel.name)]);
        }
    }
}