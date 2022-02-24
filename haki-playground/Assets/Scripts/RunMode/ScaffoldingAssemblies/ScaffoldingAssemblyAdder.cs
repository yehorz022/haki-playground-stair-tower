using System.Collections.Generic;
using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using UnityEngine;

namespace Assets.Scripts.RunMode.ScaffoldingAssemblies
{
    public class ScaffoldingAssemblyAdder : HakiComponent
    {
        public ScaffoldingAssembly Assembly;


        [Inject]
        private IObjectCacheManager ObjectCacheManager { get; set; }

        private List<ScaffoldingAssembly> assemblies;
        [ExecuteInEditMode]
        void Start()
        {
            ApplicationManager.HandleDependencyInjection(this);
            assemblies = new List<ScaffoldingAssembly>();
        }

        public void Add()
        {
            ScaffoldingAssembly assembly = ObjectCacheManager != null
                ? ObjectCacheManager.Instantiate(Assembly, transform)
                : Instantiate(Assembly, transform);
            if (assemblies == null)
                assemblies = new List<ScaffoldingAssembly>();

            assembly.transform.Translate(0, assemblies.Count * 2, 0);
            assemblies.Add(assembly);
        }

        public void Remove()
        {
            if (assemblies.Count > 0)
            {
                int maxIndex = assemblies.Count - 1;
                var item = assemblies[maxIndex];
                if (ObjectCacheManager == null)
                    DestroyImmediate(item.gameObject);
                else ObjectCacheManager.Cache(item);
                assemblies.RemoveAt(maxIndex);
            }
        }
    }
}