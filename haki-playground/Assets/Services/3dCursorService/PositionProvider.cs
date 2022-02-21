using System;
using System.Collections.Generic;
using Assets.Services.ComponentConnection;
using Assets.Services.ComponentService;
using Assets.Services.InputService;
using UnityEngine;

public class PositionProvider : HakiComponent
{
    public GameObject floor;
    private ObjectCacheManager ocm;

    [Inject]
    private IInputService InputService { get; set; }

    [Inject]
    private IComponentHolder componentHolder { get; set; }

    void Start()
    {
        ocm = FindObjectOfType<ObjectCacheManager>();
        DependancyInjectionManager dis = FindObjectOfType<DependancyInjectionManager>();

        dis.InjectDependencies(this);

        floor.SetActive(true);
    }


    private bool run;
    private ComponentConnectionService ccs;
    public void SetObject(ComponentConnectionService replacement)
    {

        if (ccs != null)
        {
            RecycleComponent();
        }

        run = true;
        ccs = ocm.Instantiate(replacement, transform); ;

    }


    void OnDrawGizmos()
    {
        if (Application.isEditor == false)
            componentHolder.OnDrawGizmos();
    }

    private void Update()
    {
        if (run == false)
            return;

        if (ccs.gameObject.activeSelf is false)
            ccs.gameObject.SetActive(true);


        if (HandleCollisionDetection(out Vector3 newPos, out Quaternion euler))
        {
            ccs.transform.position = newPos;
            ccs.transform.rotation = euler;

            if (InputService.IsLeftMouseButtonDown)
            {
                PlaceComponent();
            }
            else if (InputService.IsRightMouseButtonDown)
            {
                RecycleComponent();
            }
        }
    }

    private void RecycleComponent()
    {
        ocm.Cache(ccs);
        run = false;
        ccs = null;
    }

    private void PlaceComponent()
    {
        componentHolder.PlaceComponent(ccs);
        run = false;
        ccs = null;
    }

    private Vector3 CalculateNewPosition(IntersectionResults intersection)
    {

        return intersection.ConnectionDefinition.CalculateWorldPosition(intersection.Component.transform);
    }

    IntersectionResults GetBestResult(List<IntersectionResults> intersections)
    {
        IntersectionResults res = null;
        float min = float.MaxValue;

        for (int i = 0; i < intersections.Count; i++)
        {
            var newPos = CalculateNewPosition(intersections[i]);

            float diff = (newPos - Camera.main.transform.position).sqrMagnitude;


            if (diff < min)
                res = intersections[i];

            Debug.Log(diff);
        }

        return res;
    }

    private bool HandleCollisionDetection(out Vector3 result, out Quaternion euler)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (ccs.connectionDefinitionCollection.Count == 0)
            throw new Exception(Constants.ConnectionDefinitionsIsEmpty);


        if (componentHolder.TryGetIntersections(ray, ccs.connectionDefinitionCollection.GetElementAt(0).ConnectionInfo, out List<IntersectionResults> intersections) && intersections.Count > 0)
        {

            IntersectionResults intersection = GetBestResult(intersections);

            result = CalculateNewPosition(intersection);

            switch (intersection.ConnectionDefinition.ConnectionInfo.rotationOrientation)
            {
                case ConnectionInfo.RotationOrientation.Horizontal:
                    euler = Quaternion.FromToRotation(Vector3.back, intersection.ConnectionDefinition.lookAt);
                    break;
                case ConnectionInfo.RotationOrientation.Vertical:
                    euler = Quaternion.FromToRotation(Vector3.up, intersection.ConnectionDefinition.lookAt);
                    break;
                default:
                    euler = Quaternion.identity;
                    break;
            }
            return true;
        }

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            result = hit.point;
            euler = Quaternion.identity;
            return true;
        }

        result = default;

        euler = default;
        return false;
    }


}

