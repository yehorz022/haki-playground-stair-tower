using System.Collections;
using System.Collections.Generic;
using Assets.Services.ComponentConnection;
using Assets.Services.ComponentService;
using UnityEngine;

namespace Assets.Services.ComponentService
{
    public class InterSectionResults
    {
        public ComponentConnectionService Component { get; }
        public Vector3 WorldPosition { get; }

        public InterSectionResults(ComponentConnectionService component, Vector3 worldPosition)
        {
            this.Component = component;
            this.WorldPosition = worldPosition;
        }
    }

    public interface IComponentHolder
    {
        void PlaceComponent(ComponentConnectionService component);
        bool TryGetIntersections(Ray ray, ConnectionType connectionType, out List<InterSectionResults> connectionPoints);
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

        public bool TryGetIntersections(Ray ray, ConnectionType connectionType, out List<InterSectionResults> connectionPoints)
        {

            connectionPoints = new List<InterSectionResults>();

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

                    if (connectionType.Equals(item.connectionType).Equals(false))
                        continue;

                    Vector3 heading = item.CalculateHeading(component.transform.localRotation);
                    Vector3 wPos = item.CalculateWorldPosition(component.transform);

                    if (IntersectionHandler.CheckIntersection(lineStart, lineEnd, heading, wPos) == false)
                        continue;

                    connectionPoints.Add(new InterSectionResults(component, wPos));
                    result = true;
                }
            }

            return result;
        }
    }
}