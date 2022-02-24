using System.Collections.Generic;
using Assets.Scripts.Extensions.UnityExtensions;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Constants;
using Assets.Scripts.Shared.Containers.Collision;
using Assets.Scripts.Shared.Interfaces;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Services.ComponentService
{
    public interface IComponentHolder
    {
        void PlaceComponent(IScaffoldingComponent scaffoldingComponent);
        bool TryGetIntersections(Ray ray, ComponentConnectionInfo connectionInfo, out List<IntersectionResults> connectionPoints);
        void OnDrawGizmos();
    }

    [Service(typeof(IComponentHolder))]
    public class ComponentHolderService : IComponentHolder
    {

        [Inject] private IIntersectionService IntersectionHandler { get; set; }

        public ComponentHolderService(IIntersectionService intersectionService)
        {
            IntersectionHandler = intersectionService;

            components = new List<IScaffoldingComponent>();
        }

        private readonly List<IScaffoldingComponent> components;

        public ComponentHolderService()
        {
            components = new List<IScaffoldingComponent>();
        }

        public void PlaceComponent(IScaffoldingComponent scaffoldingComponent)
        {
            components.Add(scaffoldingComponent);
        }

        public void OnDrawGizmos()
        {
            if (components != null)
            {
                foreach (IScaffoldingComponent component in components)
                {
                    for (int i = 0; i < component.GetConnectionDefinitionCollection().Count; i++)
                    {
                        ConnectionDefinition item = component.GetConnectionDefinitionCollection().GetElementAt(i);

                        Vector3 connectorPos = item.CalculateWorldPosition(component.GetTransform());

                        Gizmos.DrawSphere(connectorPos, Constants.VirtualSphereRadius);
                    }
                }
            }
        }

        public bool TryGetIntersections(Ray ray, ComponentConnectionInfo connectionInfo, out List<IntersectionResults> connectionPoints)
        {

            connectionPoints = new List<IntersectionResults>();

            bool result = false;

            Vector3 lineStart = ray.origin;
            Vector3 lineEnd = ray.GetPoint(100);

            foreach (IScaffoldingComponent component in components)
            {

                //check if scaffoldingComponent is behind ray

                if(ray.IsVectorBehind(component.GetTransform().position))
                    continue;

                for (int i = 0; i < component.GetConnectionDefinitionCollection().Count; i++)
                {
                    ConnectionDefinition item = component.GetConnectionDefinitionCollection().GetElementAt(i);

                    if (connectionInfo.Equals(item.ComponentConnectionInfo).Equals(false))
                        continue;

                    Vector3 heading = item.CalculateHeading(component.GetTransform().localRotation);
                    Vector3 wPos = item.CalculateWorldPosition(component.GetTransform());

                    if (IntersectionHandler.CheckIntersection(lineStart, lineEnd, heading, wPos) == false)
                        continue;

                    connectionPoints.Add(new IntersectionResults(component, i));
                    result = true;
                }
            }

            return result;
        }
    }
}