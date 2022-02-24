using Assets.Scripts.RunMode.DependencyInjection;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.InputService;
using UnityEngine;

namespace Assets.Scripts.RunMode
{
    public class ApplicationManager : MonoBehaviour
    {

        private DependancyInjectionManager dim;

        [Inject]
        private IInputService InputService { get; set; }

        void Start()
        {
            dim = GetComponent<DependancyInjectionManager>();
            dim.InjectDependencies(this);
            InputUI.Initialize();
        }

        void Update()
        {
            InputService.Update();

        }

    }
}