using Assets.Scripts.RunMode.DependencyInjection;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.InputService;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.RunMode
{
    public class ApplicationManager : MonoBehaviour
    {

        private DependancyInjectionManager dim;

        [Inject]
        private IInputService InputService { get; set; }

        private void Awake()
        {   
            Inputs.Initialize(); //intializing inputs to enable drag and drop features of UI
            Routine.Initialize(this); // needs to place in Awake , intializing routine to run animations and routines like wait routines and button clicking anims
        }

        void Start()
        {
            dim = GetComponent<DependancyInjectionManager>();
            dim.InjectDependencies(this);
        }

        void Update()
        {
            InputService.Update();
        }

    }
}