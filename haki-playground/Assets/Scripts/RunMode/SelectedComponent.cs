using System.Collections.Generic;
using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.Cameras;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Tools.Selector.Face;
using Assets.Scripts.Shared.Behaviours;
using UnityEngine;

namespace Assets.Scripts.RunMode
{
    [Service(typeof(ISelected<ScaffoldingComponent>))]
    public class SelectedComponent : ISelected<ScaffoldingComponent>
    {
        private ScaffoldingComponent component;
        public void SetSelected(ScaffoldingComponent item)
        {
            component = item;
        }

        /// <inheritdoc />
        public bool TryGet(out ScaffoldingComponent item)
        {
            item = component;
            return item != null;
        }

        /// <inheritdoc />
        public void Release()
        {
            component.Deselect();
            component = null;
        }
    }

    [Service(typeof(IBoxFinder<ScaffoldingComponent>))]
    public class BoxFinder : IBoxFinder<ScaffoldingComponent>
    {
        private Vector3 position;
        private Vector3 size;

        public bool TryFind(out IList<ScaffoldingComponent> item)
        {
            item = new List<ScaffoldingComponent>();
            
            RaycastHit[] data = Physics.BoxCastAll(position, size, Vector3.forward);

            foreach (RaycastHit hit in data)
            {
                HakiComponent component = hit.transform.GetComponent<ScaffoldingComponent>();

                if (component is ScaffoldingComponent scaffoldingComponent)
                {
                    item.Add(scaffoldingComponent);
                }
            }

            return item.Count > 0;
        }

        public void SetBox(Vector3 position, Vector3 size)
        {
            this.position = position;
            this.size = size;
        }
    }
    [Service(typeof(IFinder<ScaffoldingAssembly>))]
    public class AssemblyFinder : IFinder<ScaffoldingAssembly>
    {
        private readonly ICameraService cameraService;

        public AssemblyFinder(ICameraService cameraService)
        {
            this.cameraService = cameraService;
        }

        public bool TryFind(out ScaffoldingAssembly item)
        {
            Ray ray = cameraService.CreateMouseRay();

            RaycastHit[] allhits = Physics.RaycastAll(ray);

            item = default;
            float distance = float.MaxValue;

            foreach (RaycastHit hit in allhits)
            {
                ScaffoldingAssembly ass = hit.transform.GetComponent<ScaffoldingAssembly>();

                if (ass == null)
                {
                    continue;
                }

                if (hit.distance < distance)
                    item = ass;
            }

            if (item == null)
            {
                return false;
            }

            return true;
        }
    }
}