using System.Collections.Generic;
using Assets.Services.ComponentConnection;
using Assets.Services.DependencyInjection;
using UnityEngine;

namespace Assets.Services.ComponentService
{
    public class IntersectionResults
    {
        public ComponentConnectionService Component { get; }

        public ConnectionDefinition ConnectionDefinition { get; }

        public IntersectionResults(ComponentConnectionService component, ConnectionDefinition connectionDefinition)
        {
            ConnectionDefinition = connectionDefinition;
            Component = component;
        }
    }

    public interface IComponentHolder
    {
        void PlaceComponent(ComponentConnectionService component);
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

            components = new List<ComponentConnectionService>();
        }

        private readonly List<ComponentConnectionService> components;

        public ComponentHolderService()
        {
            components = new List<ComponentConnectionService>();
        }

        public void PlaceComponent(ComponentConnectionService component)
        {
            components.Add(component);
        }

        public void OnDrawGizmos()
        {
            if (components != null)
            {
                foreach (ComponentConnectionService component in components)
                {
                    for (int i = 0; i < component.connectionDefinitionCollection.Count; i++)
                    {
                        ConnectionDefinition item = component.connectionDefinitionCollection.GetElementAt(i);

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

            foreach (ComponentConnectionService component in components)
            {

                //check if component is behind camera
                if (Vector3.Dot(ray.direction, (component.transform.position - lineStart).normalized) <= 0)
                    continue;

                for (int i = 0; i < component.connectionDefinitionCollection.Count; i++)
                {
                    ConnectionDefinition item = component.connectionDefinitionCollection.GetElementAt(i);

                    if (connectionInfo.Equals(item.ConnectionInfo).Equals(false))
                        continue;

                    Vector3 heading = item.CalculateHeading(component.transform.localRotation);
                    Vector3 wPos = item.CalculateWorldPosition(component.transform);

                    if (IntersectionHandler.CheckIntersection(lineStart, lineEnd, heading, wPos) == false)
                        continue;

                    connectionPoints.Add(new IntersectionResults(component, item));
                    result = true;
                }
            }

            return result;
        }
    }
}