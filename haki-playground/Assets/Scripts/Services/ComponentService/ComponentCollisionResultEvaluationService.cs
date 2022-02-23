using System.Collections.Generic;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Containers.Collision;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Services.ComponentService
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
                Vector3 newPos = intersection.CalculateWorldPosition(intersections[i].ScaffoldingComponent.GetTransform());

                float diff = (newPos - Camera.main.transform.position).sqrMagnitude;


                if (diff < min)
                    res = intersections[i];

                Debug.Log(diff);
            }

            return res;
        }
    }
}