using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.InputService;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.RunMode
{
    public partial class ApplicationManager : MonoBehaviour
    {
        private static ApplicationManager instance;
        private ServiceManager serviceManager;

        private IInputService inputService;

        public static ApplicationManager Instance => instance;

        void Awake()
        {
            instance = this;
            serviceManager = new ServiceManager();


            serviceManager.RegisterServicesFromAssembly(typeof(ServiceHook).Assembly);

            serviceManager.DefineAs<IObjectCacheManager, ObjectCacheManager>(new ObjectCacheManager(this));

            inputService = serviceManager.GetDependency<IInputService>();

            Inputs.Initialize(); //intializing inputs to enable drag and drop features of UI
            Routine.Initialize(this); // needs to place in Awake , intializing routine to run animations and routines like wait routines and button clicking anims
        }


        void Start()
        {
            ResetTransform();
        }


        /// <summary>
        /// This is solely because at some point, some, is going to drag this game object, knowingly or not. 
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
            inputService.Update();

        }

    }
}