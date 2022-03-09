using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Services.Cameras;
using Assets.Scripts.Services.ComponentFilteredProviders;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Providers.ExtrudeOptions;
using Assets.Scripts.Services.Tools.Selector.ConnectionPoint;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.FilterOptions;
using Assets.Scripts.Shared.Helpers;
using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.Shapes;
using Assets.Scripts.Shared.UnityExtensions;
using UnityEngine;

namespace Assets.Scripts.Services.Tools.Selector.Face
{



    [Service(typeof(ISelectFaceTool))]
    public class SelectFaceTool : ISelectFaceTool
    {
        public ToolType ToolType => ToolType.ExtrudeTool;


        private readonly IFilteredComponentProvider<SpireFilteringOptions> filteredComponents;
        private readonly ICameraService cameraService;
        private readonly ISpireTopConnectionSelector top;
        private readonly ISpireBottomConnectionSelector bottom;
        private readonly IExtrudeOptionsProvider extrudeOptionsProvider;

        private static readonly SpireFilteringOptions FilterOptions = new SpireFilteringOptions()
        {
            Prefix = "7016" // this is hardcoded liek this for now since there is no need to make it smarter, probably in the long run it should be changed to something more generic

        };

        public SelectFaceTool(IFilteredComponentProvider<SpireFilteringOptions> filteredComponents, ICameraService cameraService, ISpireTopConnectionSelector top, ISpireBottomConnectionSelector bottom, IExtrudeOptionsProvider extrudeOptionsProvider)
        {
            this.filteredComponents = filteredComponents;
            this.cameraService = cameraService;
            this.top = top;
            this.bottom = bottom;
            this.extrudeOptionsProvider = extrudeOptionsProvider;
        }

        public void Update()
        {

        }

        private bool DetectFace(Ray ray, out HakiComponent obj)
        {
            obj = default;
            if (CalculateBestMatch(ray, out IDictionary<Vector3, PotentialFaceSelections> potential))
            {
                BuildFromPotentialMatches(ray, potential, out obj);
            }

            return obj != null;
        }

        private void BuildFromPotentialMatches(Ray ray, IDictionary<Vector3, PotentialFaceSelections> potentials, out HakiComponent obj)
        {
            obj = default;

            if (TrySelectBestFace(ray, potentials, out PotentialFaceSelections bestPotential))
            {
                HandleIntersection(bestPotential, out obj);
            }
        }

        private bool TrySelectBestFace(Ray ray, IDictionary<Vector3, PotentialFaceSelections> potentials, out PotentialFaceSelections bestPotential)
        {
            Vector3 closestPointToRayOrigin = Vector3Extensions.Max();
            bool result = false;
            bestPotential = null;
            foreach (PotentialFaceSelections potential in potentials.Values)
            {
                if (potential.Components.Count <= 1)
                {
                    //maybe we should give the user ability to extrude single spire.
                    //this would be useful for curves.
                    continue;
                }

                if (HandleWhenOnlyTwoSpiresAreAvailable(potential) == false)
                    continue;

                if (ray.TryGetVectorClosestToRay(closestPointToRayOrigin, potential.PointOfIntersection, out closestPointToRayOrigin) == false)
                    continue;

                bestPotential = potential;
                result = true;
            }

            return result;
        }

        private static void HandleIntersection(PotentialFaceSelections potential, out HakiComponent o)
        {
            o = null;
            Vector3 current = potential.Components.First().Position;
            foreach (HakiComponent component in potential.Components)
            {
                if (potential.PointOfIntersection.IsVectorCloserThan(component.transform.position, current))
                {
                    o = component;
                }
            }
        }

        private bool TryGetQuad(PotentialFaceSelections potential, out Quad quad)
        {
            quad = null;

            HakiComponent first = potential.Components.First();
            HakiComponent last = potential.Components.Last();


            if (top.GetConnection(first, out ConnectionDefinition fTop) == false)
                return false;
            if (bottom.GetConnection(first, out ConnectionDefinition fBot) == false)
                return false;
            if (top.GetConnection(last, out ConnectionDefinition lTop) == false)
                return false;
            if (bottom.GetConnection(last, out ConnectionDefinition lBot) == false)
                return false;

            quad = new Quad(
                a: fTop.CalculateWorldPosition(first.transform),
                b: lTop.CalculateWorldPosition(last.transform),
                c: fBot.CalculateWorldPosition(last.transform),
                d: lBot.CalculateWorldPosition(first.transform)
            );

            return true;


            return false;
        }

        private bool HandleWhenOnlyTwoSpiresAreAvailable(PotentialFaceSelections potential)
        {

            if (TryGetQuad(potential, out Quad quad) == false)
            {
                return false;
            }

            if (Intersections.CheckIfPointIsOnQuad(potential.PointOfIntersection, quad) == false)
            {
                return false;
            }

            return true;
        }

        private bool CalculateBestMatch(Ray ray, out IDictionary<Vector3, PotentialFaceSelections> lookUp)
        {
            lookUp = new Dictionary<Vector3, PotentialFaceSelections>();

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
                            lookUp.Add(pointOfIntersection, new PotentialFaceSelections(normal, pointOfIntersection));
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

        /// <inheritdoc />
        public bool TryFind(out HakiComponent item)
        {
            Ray ray = cameraService.CreateMouseRay();
            return DetectFace(ray, out item);
        }
    }
}