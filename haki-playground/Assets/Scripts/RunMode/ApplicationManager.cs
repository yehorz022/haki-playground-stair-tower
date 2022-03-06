using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.DependencyInjection;
using Assets.Scripts.Services.InputService;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.RunMode
{
    public partial class ApplicationManager : SceneMemberInjectDependencies
    {
        private static ApplicationManager instance;
        private ServiceManager serviceManager;

        [Inject]
        private IInputService inputService { get; set; }
        [SerializeField] private GameObject emptyGameObject;
        public static ApplicationManager Instance => instance;


        void Awake()
        {
            instance = this;
            InitService();

            //inputService = serviceManager.GetDependency<IInputService>();

            Inputs.Initialize(); //intializing inputs to enable drag and drop features of UI
            Routine.Initialize(this); // needs to place in Awake , intializing routine to run animations and routines like wait routines and button clicking anims
        }

        private void InitService()
        {
            serviceManager = new ServiceManager();

            serviceManager.RegisterServicesFromAssembly(typeof(ServiceHook).Assembly);


            serviceManager.DefineAs<IDependencyInjection, DependencyInjection>(new DependencyInjection(serviceManager.InjectDependencies));
            serviceManager.DefineAs<IObjectCacheManager, ObjectCacheManager>(new ObjectCacheManager(this, emptyGameObject, serviceManager.GetDependency<IDependencyInjection>()));
        }


        void Start()
        {
            ResetTransform();

            InjectDependenciesToSceneObjects();
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


        public static void HandleDependencyInjection(HakiComponent hakicomponent)
        {
            instance.serviceManager.InjectDependencies(hakicomponent);
        }

        void Update()
        {
            inputService?.Update();
        }

    }
}