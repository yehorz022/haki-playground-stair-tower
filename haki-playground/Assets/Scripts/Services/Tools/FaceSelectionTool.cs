using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Scripts.Services.ComponentFilteredProviders;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.FilterOptions;
using Assets.Scripts.Shared.Helpers;
using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.Shapes;
using Assets.Scripts.Shared.UnityExtensions;
using UnityEngine;

namespace Assets.Scripts.Services.Tools
{
    public interface ISelectFaceTool
    {
        void DetectFace(Ray ray);
    }

    [Service(typeof(ISelectFaceTool))]
    public class SelectFaceTool : ISelectFaceTool
    {
        private readonly IFilteredComponentProvider<SpireFilteringOptions> filteredComponents;
        private readonly ISpireTopConnectionSelector top;
        private readonly ISpireBottomConnectionSelector bottom;

        private class PotencialFaceSelections
        {
            internal PotencialFaceSelections(Vector3 normal, Vector3 pointOfIntersection)
            {
                Components = new List<HakiComponent>();
                Normal = normal;
                PointOfIntersection = pointOfIntersection;
            }

            internal Vector3 Normal { get; }
            internal Vector3 PointOfIntersection { get; }
            internal ICollection<HakiComponent> Components { get; }
        }

        private static readonly SpireFilteringOptions FilterOptions = new SpireFilteringOptions()
        {
            Prefix = "7016"
        };

        public SelectFaceTool(IFilteredComponentProvider<SpireFilteringOptions> filteredComponents, ISpireTopConnectionSelector top, ISpireBottomConnectionSelector bottom)
        {
            this.filteredComponents = filteredComponents;
            this.top = top;
            this.bottom = bottom;
        }

        public void DetectFace(Ray ray)
        {
            if (CalculateBestMatch(ray, out IDictionary<Vector3, PotencialFaceSelections> potential))
            {
                BuildFromPotentialMatches(ray, potential);
            }
        }

        private void BuildFromPotentialMatches(Ray ray, IDictionary<Vector3, PotencialFaceSelections> potentials)
        {
            Vector3 best = Vector3Extensions.Max();
            bool gotIntersection = false;

            foreach (PotencialFaceSelections potential in potentials.Values)
            {
                if (potential.Components.Count <= 1)
                {
                    continue;
                }

                if (potential.Components.Count == 2)
                {
                    gotIntersection = HandleWhenOnlyTwoSpiresAreAwailible(ray, potential, gotIntersection, ref best);
                }
            }

            if (gotIntersection)
                Debug.DrawLine(ray.origin, best);
        }

        bool TryGetPoints(PotencialFaceSelections potential, out Vector3 a, out Vector3 b, out Vector3 c, out Vector3 d)
        {
            HakiComponent first = potential.Components.First();
            HakiComponent last = potential.Components.Last();

            a = Vector3.zero;
            b = Vector3.zero;
            c = Vector3.zero;
            d = Vector3.zero;

            if (top.GetConnection(first, out ConnectionDefinition fTop) == false)
                return false;
            if (bottom.GetConnection(first, out ConnectionDefinition fBot) == false)
                return false;
            if (top.GetConnection(last, out ConnectionDefinition lTop) == false)
                return false;
            if (bottom.GetConnection(last, out ConnectionDefinition lBot) == false)
                return false;

            a = CreateWorldPosition(first, fTop);
            b = CreateWorldPosition(last, lTop);
            c = CreateWorldPosition(first, lBot);
            d = CreateWorldPosition(last, fBot);

            return true;

        }

        private bool HandleWhenOnlyTwoSpiresAreAwailible(Ray ray, PotencialFaceSelections potential, bool gotIntersection,
            ref Vector3 best)
        {
            if (TryGetPoints(potential, out var a, out var b, out var c, out var d) == false)
            {
                return false;
            }

            if (Intersections.CheckIfPointIsOnQuad(potential.PointOfIntersection, a, b, c, d) == false)
            {
                return false;
            }

            best = ray.origin.GetCloserVector(best, potential.PointOfIntersection);

            return true;

        }

        private static Vector3 CreateWorldPosition(HakiComponent component, ConnectionDefinition definition)
        {
            return definition.CalculateWorldPosition(component.transform);
        }

        private bool CalculateBestMatch(Ray ray, out IDictionary<Vector3, PotencialFaceSelections> lookUp)
        {
            lookUp = new Dictionary<Vector3, PotencialFaceSelections>();

            foreach (HakiComponent component in filteredComponents.Filter(FilterOptions))
            {
                if (ValidateComponent(ray, component) == false)
                    continue;

                Box bound = component.GetBounds(); // bounds are needed to establish normal vector
                foreach (Vector3 normal in bound.Normals)
                {
                    if (ValidateNormal(ray, normal) == false)
                        continue;

                    if (Intersections.RayPlaneIntersection(ray, normal, component.Position, out Vector3 pointOfIntersection))
                    {
                        if (lookUp.ContainsKey(pointOfIntersection) == false)
                        {
                            lookUp.Add(pointOfIntersection, new PotencialFaceSelections(normal, pointOfIntersection));
                        }

                        lookUp[pointOfIntersection].Components.Add(component);
                    }
                }
            }

            return lookUp.Count > 0;
        }

        private static bool ValidateComponent(Ray ray, Component component)
        {
            if (ray.IsTransformBehindRay(component.transform))
                return false;
            return true;
        }

        private static bool ValidateNormal(Ray ray, Vector3 normal)
        {
            if (normal == Vector3.up || normal == Vector3.down)
                return false; // for now we can ignore top and bottom 
            if (ray.DotProduct(normal) >= 0)
                return false; // normal is facing away from ray 
            return true;
        }
    }
}