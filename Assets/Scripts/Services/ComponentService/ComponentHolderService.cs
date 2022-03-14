using System.Collections.Generic;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Containers.Collision;
using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.UnityExtensions;
using UnityEngine;

namespace Assets.Scripts.Services.ComponentService
{
    public interface IComponentHolder
    {
        void PlaceComponent(HakiComponent scaffoldingComponent);
        void RemoveComponent(HakiComponent scaffoldingComponent);
        bool TryGetConnectionPoints(int id, Ray ray, ComponentConnectionInfo connectionInfo,
            out List<IntersectionResults> connectionPoints);

        List<HakiComponent> GetAllComponents();
        void RemoveAll();
        HakiComponent GetComponentBehindRay();

        HakiComponent[] GetComponentsIntersectingWith(Ray ray);
        IEnumerable<HakiComponent> Enumerate();
    }

    [Service(typeof(IComponentHolder))]
    public class ComponentHolderService : IComponentHolder
    {

        private readonly List<HakiComponent> components;
        private readonly IIntersectionService intersectionHandler;
        private readonly IObjectCacheManager objectCacheManager;
        public ComponentHolderService(IIntersectionService intersectionService, IObjectCacheManager objectCacheManager)
        {
            intersectionHandler = intersectionService;
            this.objectCacheManager = objectCacheManager;

            components = new List<HakiComponent>();
        }


        public ComponentHolderService(IObjectCacheManager objectCacheManager)
        {
            this.objectCacheManager = objectCacheManager;
            components = new List<HakiComponent>();
        }

        public void PlaceComponent(HakiComponent scaffoldingComponent)
        {
            components.Add(scaffoldingComponent);
        }

        public void RemoveComponent(HakiComponent scaffoldingComponent)
        {
            components.Remove(scaffoldingComponent);
        }

        public List<HakiComponent> GetAllComponents()
        {
            return components;
        }

        public void RemoveAll()
        {
            components.Clear();
        }

        /// <inheritdoc />
        public HakiComponent[] GetComponentsIntersectingWith(Ray ray)
        {
            List<HakiComponent> results = new List<HakiComponent>();
            foreach (HakiComponent component in components)
            {
                if (ray.IsVectorBehindRay(component.transform.position))
                    continue;

                if (intersectionHandler.RayCubeIntersection(ray, component, out Vector3 point) == false)
                    continue;

                results.Add(component);
            }

            return results.ToArray();
        }

        public IEnumerable<HakiComponent> Enumerate()
        {
            return components;
        }


        public bool TryGetConnectionPoints(int id, Ray ray, ComponentConnectionInfo connectionInfo, out List<IntersectionResults> connectionPoints)
        {

            connectionPoints = new List<IntersectionResults>();

            Vector3 lineEnd = ray.GetPoint(100);

            foreach (HakiComponent component in components)
            {
                if (component.GetInstanceID() == id)
                    continue;

                if (ray.IsVectorBehindRay(component.transform.position))
                    continue;

                if (component.TryGetCollectionDefinition(out ConnectionDefinitionCollection collection))
                {
                    for (int i = 0; i < collection.Count; i++)
                    {
                        ConnectionDefinition definition = collection.GetElementAt(i);

                        if (connectionInfo.Equals(definition.ComponentConnectionInfo).Equals(false))
                            continue;

                        Vector3 heading = definition.CalculateHeading(component.transform.rotation);
                        Vector3 wPos = definition.CalculateWorldPosition(component.transform);

                        if (intersectionHandler.LineSphereIntersection(ray.origin, lineEnd, heading, wPos) == false)
                            continue;

                        connectionPoints.Add(new IntersectionResults(component, i, collection));
                    }
                }
            }

            return connectionPoints.Count > 0;
        }

        public HakiComponent GetComponentBehindRay()
        {
            foreach (HakiComponent component in components)
                component.gameObject.layer = LayerMask.NameToLayer("Default"); // enable raycast so it can selectable
            HakiComponent componentSelected = null;
            bool gotHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
            if (gotHit)
            {
                if (hit.collider.GetComponent<HakiComponent>() != null) //print("gotHit  " + hit.collider.gameObject.name);
                    componentSelected = hit.collider.GetComponent<HakiComponent>();
            }

            //foreach (HakiComponent component in components)
            //    component.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); // disable objects raycast to detect floor easily
            return componentSelected;
        }
    }
}