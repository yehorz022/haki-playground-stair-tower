using Assets.Scripts.RunMode.UserInterface;
using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Services.Storage;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class ProjectLayout : MonoBehaviour
    {
        public static string projectId;
        [Inject]
        private IProject Project { get; set; }
        [Inject]
        public IComponentHolder ComponentHolder { get; set; }
        [Inject]
        public IObjectCacheManager ObjectCacheManager { get; set; }

        public static int ProjectNo = 0;
        [SerializeField] Sprite projectIcon;
        [SerializeField] Sprite newProjectIcon;
        public ScrollViewComponent scrollView;
        ScrollState scrollState = new ScrollState(0);
        private ToolManager toolManager;
        private AssemblyFactory assemblyFactory;
        private PositionProvider.PositionProvider positionProvider;
        private ComponentsLayout componentsLayout;

        void Start()
        {
            toolManager = FindObjectOfType<ToolManager>();
            assemblyFactory = FindObjectOfType<AssemblyFactory>();
            componentsLayout = FindObjectOfType<ComponentsLayout>();
            positionProvider = FindObjectOfType<PositionProvider.PositionProvider>();
            Routine.WaitAndCall(.01f, () => //wait for system to initialize first
            {
                Initialize();
            });
        }
        public void Show() {
            gameObject.SetActive(true);
            Routine.MovePivot(transform.GetComponent<RectTransform>(), new Vector2(2, .5f), new Vector2(.5f, .5f), .3f); // opening animation
        }
        public void Hide() => gameObject.SetActive(false); // closing animation


        public void Initialize()
        {
            scrollView.Initialize(PlayerPrefs.GetInt("ProjectsCount") + 1, scrollView.panelsParent, LoadPanel, scrollState, ScrollViewComponent.OLD_STATE);
        }

        public void LoadPanel(Transform panel, int reset = ScrollViewComponent.DEFAULT)
        {
            panel.GetComponent<ProjectComponent>().Initialize(int.Parse(panel.name), int.Parse(panel.name) == scrollView.totalPanels - 1 ? newProjectIcon : projectIcon);
        }

        public void LoadProject(string id)
        {
            projectId = id;
            Project = new Project(id, positionProvider.ComponentHolder, positionProvider.ObjectCacheManager);
            List<HakiComponent> components = new List<HakiComponent>();
            components.AddRange(componentsLayout.elements);
            Project.Load(positionProvider.transform, components);
            assemblyFactory.LoadFactory();
            assemblyFactory.Show();
            toolManager.Show();
            Hide();
        }

        public void SaveProject()
        {
            Project.Save(positionProvider.transform);
            assemblyFactory.SaveFactory();
        }
    }

}