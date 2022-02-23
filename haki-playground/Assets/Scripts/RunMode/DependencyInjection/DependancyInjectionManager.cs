using Assets.Scripts.Services;
using Assets.Scripts.Services.Core;
using UnityEngine;

namespace Assets.Scripts.RunMode.DependencyInjection
{
    public class DependancyInjectionManager : MonoBehaviour
    {

        private ServiceCollection services;

        void Awake()
        {
            services = new ServiceCollection();

            services.RegisterServicesFromAssembly(typeof(ServiceHook).Assembly);
        }



        public void InjectDependencies(object item)
        {
            services.InjectDependencies(item);
        }
    }
}