using System.Collections.Generic;
using Assets.Services.ComponentConnection;
using Assets.Services.DependencyInjection;
using UnityEngine;

namespace Assets.Services.ComponentService
{
    public interface IComponentHolder
    {
        void PlaceComponent(ScaffoldingComponent scaffoldingComponent);
        bool TryGetIntersections(Ray ray, ConnectionInfo connectionInfo, out List<IntersectionResults> connectionPoints);
        void OnDrawGizmos();
    }

    [Service(typeof(IComponentHolder))]
    public class ComponentHolderService : IComponentHolder
    {

        [Inject] private IIntersectionService IntersectionHandler { get; set; }

        public ComponentHolderService(IIntersectionService intersectionService)
        {
            IntersectionHandler = intersectionService;

            components = new List<ScaffoldingComponent>();
        }

        private readonly List<ScaffoldingComponent> components;

        public ComponentHolderService()
        {
            components = new List<ScaffoldingComponent>();
        }

        public void PlaceComponent(ScaffoldingComponent scaffoldingComponent)
        {
            components.Add(scaffoldingComponent);
        }

        public void OnDrawGizmos()
        {
            if (components != null)
            {
                foreach (ScaffoldingComponent component in components)
                {
                    for (int i = 0; i < component.ConnectionDefinitionCollection.Count; i++)
                    {
                        ConnectionDefinition item = component.ConnectionDefinitionCollection.GetElementAt(i);

                        Vector3 connectorPos = item.CalculateWorldPosition(component.transform);

                        Gizmos.DrawSphere(connectorPos, Constants.VirtualSphereRadius);
                    }
                }
            }
        }

        public bool TryGetIntersections(Ray ray, ConnectionInfo connectionInfo, out List<IntersectionResults> connectionPoints)
        {

            connectionPoints = new List<IntersectionResults>();

            bool result = false;

            Vector3 lineStart = ray.origin;
            Vector3 lineEnd = ray.GetPoint(100);

            foreach (ScaffoldingComponent component in components)
            {

                //check if scaffoldingComponent is behind ray

                if(ray.IsVectorBehind(component.transform.position))
                    continue;

                for (int i = 0; i < component.ConnectionDefinitionCollection.Count; i++)
                {
                    ConnectionDefinition item = component.ConnectionDefinitionCollection.GetElementAt(i);

                    if (connectionInfo.Equals(item.ConnectionInfo).Equals(false))
                        continue;

                    Vector3 heading = item.CalculateHeading(component.transform.localRotation);
                    Vector3 wPos = item.CalculateWorldPosition(component.transform);

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