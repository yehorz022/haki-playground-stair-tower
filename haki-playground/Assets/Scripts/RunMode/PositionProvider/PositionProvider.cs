using Assets.Scripts.RunMode.ComponentConnection;
using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.RunMode.DependencyInjection;
using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.InputService;
using Assets.Scripts.Shared.Containers.Collision;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.RunMode.PositionProvider
{
    public class PositionProvider : HakiComponent
    {
        public GameObject floor;
        private ObjectCacheManager ocm;

        [Inject]
        private IInputService InputService { get; set; }

        [Inject]
        private IComponentHolder ComponentHolder { get; set; }

        [Inject]
        private IComponentCollisionDetectionService collisionDetectionService { get; set; }

        void Start()
        {
            ocm = FindObjectOfType<ObjectCacheManager>();
            DependancyInjectionManager dis = FindObjectOfType<DependancyInjectionManager>();

            dis.InjectDependencies(this);

            floor.SetActive(true);
        }


        private bool run;
        private ScaffoldingComponent ccs;
        public void SetObject(ScaffoldingComponent replacement)
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
                ComponentHolder.OnDrawGizmos();
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
            }
        }

        public void PlaceComponent()
        {
            ComponentHolder.PlaceComponent(ccs);
            run = false;
            ccs = null;
        }

        public void RecycleComponent()
        {
            ocm.Cache(ccs);
            run = false;
            ccs = null;
        }

        private bool HandleCollisionDetection(out Vector3 result, out Quaternion euler)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            CollisionInfo ci = collisionDetectionService.Evaluate(ray, ccs.ConnectionDefinitionCollection, 0, ccs);

            if (ci.IsSuccess)
            {
                ConnectionDefinition cd = ci.Target.GetElementAt(ci.TargetConnectionIndex);
                result = cd.CalculateWorldPosition(ci.TargetScaffoldingComponent.GetTransform());
                euler = cd.CalculateRotation();

                return true;
            }

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                result = hit.point;
                euler = Quaternion.identity;
                return true;
            }

            result = Vector3.zero;
            euler = Quaternion.identity;
            return false;
        }


    }
}