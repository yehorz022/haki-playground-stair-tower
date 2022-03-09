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
        public static int projectId;
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
        private AssemblyFactory assemblyFactory;
        private PositionProvider.PositionProvider positionProvider;
        private PopulateGridLayout populateGridLayout;

        void Start()
        {
            assemblyFactory = FindObjectOfType<AssemblyFactory>();
            populateGridLayout = FindObjectOfType<PopulateGridLayout>();
            positionProvider = FindObjectOfType<PositionProvider.PositionProvider>();
            Routine.WaitAndCall(.01f, () => //wait for system to initialize first
            {
                Initialize();
            });
        }

        public void Initialize()
        {
            scrollView.Initialize(PlayerPrefs.GetInt("ProjectsCount", 1), scrollView.panelsParent, LoadPanel, scrollState, ScrollViewComponent.OLD_STATE);
        }

        public void LoadPanel(Transform panel, int reset = ScrollViewComponent.DEFAULT)
        {
            panel.GetComponent<ProjectComponent>().Initialize(int.Parse(panel.name), int.Parse(panel.name) == scrollView.totalPanels - 1 ? newProjectIcon : projectIcon);
        }

        public void LoadProject(int id)
        {
            projectId = id;
            Project = new Project(id, positionProvider.ComponentHolder, positionProvider.ObjectCacheManager);
            List<HakiComponent> components = new List<HakiComponent>();
            components.AddRange(populateGridLayout.elements);
            Project.Load(positionProvider.transform, components);
            assemblyFactory.LoadFactory();
        }

        public void SaveProject()
        {
            Project.Save(positionProvider.transform);
            assemblyFactory.SaveFactory();
        }
    }

}