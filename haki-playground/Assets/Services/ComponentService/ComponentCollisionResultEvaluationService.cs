using System.Collections.Generic;
using Assets.Services.ComponentConnection;
using Assets.Services.DependencyInjection;
using UnityEngine;

namespace Assets.Services.ComponentService
{

    public interface IComponentCollisionResultEvaluationService
    {
        IntersectionResults Evaluate(List<IntersectionResults> intersections);
    }

    [Service(typeof(IComponentCollisionResultEvaluationService))]
    public class ComponentCollisionResultEvaluationService : IComponentCollisionResultEvaluationService
    {
        public IntersectionResults Evaluate(List<IntersectionResults> intersections)
        {
            IntersectionResults res = null;
            float min = float.MaxValue;

            for (int i = 0; i < intersections.Count; i++)
            {

                ConnectionDefinition intersection = intersections[i].ConnectionDefinitionCollection.GetElementAt(intersections[i].ConnectionIndex);
                Vector3 newPos = intersection.CalculateWorldPosition(intersections[i].ScaffoldingComponent.transform);

                float diff = (newPos - Camera.main.transform.position).sqrMagnitude;


                if (diff < min)
                    res = intersections[i];

                Debug.Log(diff);
            }

            return res;
        }
    }
}