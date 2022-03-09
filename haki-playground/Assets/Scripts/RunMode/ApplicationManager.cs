using System;
using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.Cameras;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.DependencyInjection;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Services.Tools;
using Assets.Scripts.Services.Tools.Selector.Face;
using Assets.Scripts.Services.Utility.InputService;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.RunMode
{
    public partial class ApplicationManager : SceneMemberInjectDependencies
    {
        private static ApplicationManager instance;
        private ServiceManager serviceManager;

        [SerializeField] private Material mouseover;
        [SerializeField] private Material selected;

        [SerializeField] private GameObject emptyGameObject;
        [SerializeField] private GameObject uiElement;

        [Inject] private IInputService InputService { get; set; }
        [Inject] private IToolHandlerService ToolManager { get; set; }
        [Inject] private ICameraService CameraService { get; set; }
        [Inject] private IBoxFinder<ScaffoldingComponent> ScaffoldingAssemblyBoxFinder { get; set; }
        [Inject] private IFinder<ScaffoldingAssembly> AssemblyFinder { get; set; }
        [Inject] private ISelected<ScaffoldingAssembly> SelectedAssembly { get; set; }




        public static ApplicationManager Instance => instance;


        void Awake()
        {
            instance = this;
            InitService();


            Inputs.Initialize(); //intializing inputs to enable drag and drop features of UI
            Routine.Initialize(this); // needs to place in Awake , intializing routine to run animations and routines like wait routines and button clicking anims
        }

        private void InitService()
        {
            serviceManager = new ServiceManager();

            serviceManager.RegisterServicesFromAssembly(typeof(ServiceHook).Assembly);
            serviceManager.RegisterServicesFromAssembly(GetType().Assembly);


            serviceManager.DefineAs<IDependencyInjection, DependencyInjection>(new DependencyInjection(serviceManager.InjectDependencies));
            serviceManager.DefineAs<IObjectCacheManager, ObjectCacheManager>(new ObjectCacheManager(this, emptyGameObject, serviceManager.GetDependency<IDependencyInjection>()));

            //serviceManager.DefineAs<ISelected<ScaffoldingAssembly>, SelectedAssembly>(new SelectedAssembly());
            serviceManager.DefineAs<ISelected<ScaffoldingComponent>, SelectedComponent>(new SelectedComponent());
            serviceManager.DefineAs<IOnSelected, OnSelectedService>(new OnSelectedService(uiElement));
            serviceManager.GetDependency<IMaterialService>()?.SetMaterials(mouseover, selected);
        }


        void Start()
        {
            ResetTransform();

            InjectDependenciesToSceneObjects();
            ToolManager.SelectToolByType(ToolType.ExtrudeTool);
        }

        private void InjectDependenciesToSceneObjects()
        {
            SceneMemberInjectDependencies[] hakiComponents = GameObject.FindObjectsOfType<SceneMemberInjectDependencies>();

            foreach (SceneMemberInjectDependencies component in hakiComponents)
            {
                serviceManager.InjectDependencies(component);
            }
        }


        /// <summary>
        /// This is solely because at some point, someone, is going to drag this game object, knowingly or not. 
        /// </summary>
        private void ResetTransform()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        void Update()
        {
            InputService?.Update();
            ToolManager?.Update();

            foreach (ScaffoldingAssembly assembly in GameObject.FindObjectsOfType<ScaffoldingAssembly>())
            {
                assembly.RemoveHighlight();
            }

            if (AssemblyFinder.TryFind(out ScaffoldingAssembly item))
            {
                if (item == null)
                {
                    return;
                }

                if (InputService.IsLeftMouseButtonDown)
                {
                    SelectedAssembly.SetSelected(item);

                    //activate UI
                }
                item.Highlight();

            }
        }
    }
}