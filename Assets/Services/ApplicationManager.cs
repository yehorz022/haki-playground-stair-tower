using Assets.Services.InputService;
using UnityEngine;

namespace Assets
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