using System.Collections.Generic;
using Assets.Scripts.RunMode.ComponentConnection;
using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Shared.Interfaces;

namespace Assets.Scripts.RunMode.ScaffoldingAssemblies
{
    public class ScaffoldingAssembly : ScaffoldingComponent
    {
        List<IScaffoldingComponent> scaffoldingComponents;

        // Start is called before the first frame update
        void Start()
        {

            //ObjectCacheManager ocm

            scaffoldingComponents = new List<IScaffoldingComponent>();
            IEnumerable<IScaffoldingComponent> components = GetComponentsInChildren<ScaffoldingComponent>();


            foreach (IScaffoldingComponent component in components)
            {
                scaffoldingComponents.Add(component);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}