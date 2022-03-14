using System.Collections.Generic;
using Assets.Scripts.Shared.Behaviours;
using UnityEngine;

namespace Assets.Scripts.Services.Tools.Selector.Face
{

    internal class PotentialFaceSelections
    {
        internal Vector3 Normal { get; }
        internal Vector3 PointOfIntersection { get; }
        internal ICollection<HakiComponent> Components { get; }
        internal int ComponentCount => Components.Count;

        internal PotentialFaceSelections(Vector3 normal, Vector3 pointOfIntersection)
        {
            Components = new List<HakiComponent>();
            Normal = normal;
            PointOfIntersection = pointOfIntersection;
        }
    }
}


