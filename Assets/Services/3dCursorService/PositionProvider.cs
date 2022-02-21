using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Services.ComponentConnection;
using Assets.Services.ComponentService;
using Assets.Services.InputService;
using UnityEngine;
#if UNITY_EDITOR
using Codice.CM.SEIDInfo;
using UnityEngine.Windows.WebCam;
#endif

public class PositionProvider : HakiComponent
{
    public GameObject floor;
    private ObjectCacheManager ocm;

    [Inject]
    private IInputService InputService { get; set; }

    [Inject]
    private IComponentHolder componentHolder { get; set; }

    public static PositionProvider instance;
    void Awake() {
        instance = this;
    }

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

        run = true;
        ccs = ocm.Instantiate(replacement); ;

    }


    void OnDrawGizmos()
    {
        if (Application.isEditor == false)
            componentHolder.OnDrawGizmos();
    }

    public void PlaceComponent () {
        if (ccs != null) {
            componentHolder.PlaceComponent(ccs);
            run = false;
            ccs = null;
        }
    }

    public void DiscardComponent () {
        if (ccs != null) {
            ocm.Cache(ccs);
            run = false;
            ccs = null;
        }
    }

    private void Update()
    {
        if (run == false)
            return;

        if (ccs.gameObject.activeSelf is false)
            ccs.gameObject.SetActive(true);


        if (HandleCollisionDetection(out Vector3 newPos))
        {
            ccs.transform.position = newPos;
        }

        //if (InputService.IsLeftMouseButtonDown)
        //{
        //    componentHolder.PlaceComponent(ccs);
        //    run = false;
        //    ccs = null;
        //}
        //else if (InputService.IsRightMouseButtonDown)
        //{
        //    ocm.Cache(ccs);
        //    run = false;
        //    ccs = null;
        //}

    }

    private bool HandleCollisionDetection(out Vector3 result)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

#if UNITY_EDITOR
        if (ccs.connectionDefinitionCollection != null) { 
            if (ccs.connectionDefinitionCollection.Count == 0)
                throw new Exception(Constants.ConnectionDefinitionsIsEmpty);

            if (componentHolder.TryGetIntersections(ray, ccs.connectionDefinitionCollection.GetElementAt(0).connectionType, out List<InterSectionResults> intersections) && intersections.Count > 0)
            {
                result = intersections.First().WorldPosition;
                return true;
            }
        }
#endif
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            result = hit.point;
            return true;
        }

        result = default;
        return false;
    }
}

