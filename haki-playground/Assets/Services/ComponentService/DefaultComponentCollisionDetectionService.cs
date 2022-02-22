using System;
using System.Collections.Generic;
using Assets.Services.ComponentConnection;
using Assets.Services.DependencyInjection;
using UnityEngine;

namespace Assets.Services.ComponentService
{
    public interface IComponentCollisionDetectionService
    {
        CollisionInfo Evaluate(Ray ray, ConnectionDefinitionCollection source, int sourceConnectionIndex);
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


        public CollisionInfo Evaluate(Ray ray, ConnectionDefinitionCollection source, int sourceConnectionIndex)
        {
            CollisionInfo ci = new CollisionInfo();
            ci.Source = source;
            ci.SourceConnectionIndex = sourceConnectionIndex;


            if (source.Count == 0 || sourceConnectionIndex >= source.Count)
                throw new Exception(Constants.ConnectionDefinitionsIsEmpty);


            if (componentHolder.TryGetIntersections(ray, source.GetElementAt(sourceConnectionIndex).ConnectionInfo, out List<IntersectionResults> intersections) && intersections.Count > 0)
            {

                IntersectionResults intersection = intersectionEvaluationService.Evaluate(intersections);

                ci.TargetScaffoldingComponent = intersection.ScaffoldingComponent;
                ci.Target = intersection.ConnectionDefinitionCollection;
                ci.TargetConnectionIndex = intersection.ConnectionIndex;
                return ci;
            }

            return ci;
        }
    }
}