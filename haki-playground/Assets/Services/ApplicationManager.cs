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
        }

        void Update()
        {
            InputService.Update();

        }

    }
}