using System;
using System.Collections.Generic;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Constants;
using Assets.Scripts.Shared.Containers.Collision;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Services.ComponentService
{
    public interface IComponentCollisionDetectionService
    {
        CollisionInfo Evaluate(Ray ray, int sourceConnectionIndex,
            HakiComponent component);
    }

    [Service(typeof(IComponentCollisionDetectionService))]
    public class DefaultComponentCollisionDetectionService : IComponentCollisionDetectionService
    {
        private readonly IComponentCollisionResultEvaluationService intersectionEvaluationService;
        private readonly IComponentHolder componentHolder;

        public DefaultComponentCollisionDetectionService(IComponentHolder componentHolder, IComponentCollisionResultEvaluationService intersectionEvaluationService)
        {
            this.intersectionEvaluationService = intersectionEvaluationService;
            this.componentHolder = componentHolder;
        }


        public CollisionInfo Evaluate(Ray ray,  int sourceConnectionIndex, HakiComponent component)
        {
            CollisionInfo ci = new CollisionInfo();
            if (component.TryGetCollectionDefinition(out ConnectionDefinitionCollection source) is false)
                return ci;

            ci.SourceScaffoldingComponent = component;
            ci.Source = source;
            ci.SourceConnectionIndex = sourceConnectionIndex;


            if (source.Count == 0 || sourceConnectionIndex >= source.Count)
                throw new Exception(Constants.ConnectionDefinitionsIsEmpty);

            

            if (ValidateIntersection(component.GetInstanceID(),ray, source, sourceConnectionIndex, out List<IntersectionResults> intersections))
            {
                IntersectionResults intersection = intersectionEvaluationService.Evaluate(intersections);

                ci.TargetScaffoldingComponent = intersection.ScaffoldingComponent;
                ci.Target = intersection.ConnectionDefinitionCollection;
                ci.TargetConnectionIndex = intersection.ConnectionIndex;
                return ci;
            }

            return ci;
        }

        private bool ValidateIntersection(int id, Ray ray, ConnectionDefinitionCollection source,
            int sourceConnectionIndex, out List<IntersectionResults> intersections)
        {
            return componentHolder.TryGetIntersections(id,ray, source.GetElementAt(sourceConnectionIndex).ComponentConnectionInfo, out intersections);
        }
    }
}