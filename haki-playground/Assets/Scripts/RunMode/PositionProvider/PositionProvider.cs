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
        [SerializeField]
        private ObjectCacheManager ocm;

        [SerializeField]
        private GameObject floor;

        [Inject]
        private IInputService InputService { get; set; }

        [Inject]
        private IComponentHolder ComponentHolder { get; set; }

        [Inject]
        private IComponentCollisionDetectionService collisionDetectionService { get; set; }

        void Start()
        {
            DependancyInjectionManager dis = FindObjectOfType<DependancyInjectionManager>();

            dis.InjectDependencies(this);

            floor.SetActive(true);
        }


        private bool run;
        private ScaffoldingComponent ccs;
        public ScaffoldingComponent CreateObject(ScaffoldingComponent replacement)
        {
            ccs = ocm.Instantiate(replacement, transform);
            ComponentHolder.PlaceComponent(ccs);
            return ccs;
        }

        public void SetObjectActive(ScaffoldingComponent component)
        {
            ccs = component;
            run = true;
        }

        public void SetObjectInactive()
        {
            ccs = null;
            run = false;
        }

        public void RecycleComponent()
        {
            ComponentHolder.RemoveComponent(ccs);
            ocm.Cache(ccs);
            SetObjectInactive();
        }

        public ScaffoldingComponent PickComponent()
        {
            return (ScaffoldingComponent)ComponentHolder.GetComponentBehindRay();
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
