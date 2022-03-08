using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Containers.Collision;
using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.Behaviours;
using UnityEngine;

namespace Assets.Scripts.RunMode.PositionProvider
{
    public class PositionProvider : SceneMemberInjectDependencies
    {

        private bool run;
        private ScaffoldingComponent ccs;

        [Inject]
        public IObjectCacheManager ObjectCacheManager { get; set; }

        [SerializeField]
        private GameObject floor;


        [Inject]
        public IComponentHolder ComponentHolder { get; set; }

        [Inject]
        private IComponentCollisionDetectionService CollisionDetectionService { get; set; }
        private ProjectLayout projectLayout;

        void Start()
        {
            projectLayout = FindObjectOfType<ProjectLayout>();
            floor.SetActive(true);
        }


        public ScaffoldingComponent CreateComponent(ScaffoldingComponent replacement) // made create 
        {
            ccs = ObjectCacheManager.Instantiate(replacement, transform);
            run = true;
            return ccs;
        }

        public void PickComponent(ScaffoldingComponent component)
        {
            ComponentHolder.RemoveComponent(component);
            ccs = component;
            run = true;
        }

        public void PlaceComponent()
        {
            ComponentHolder.PlaceComponent(ccs);
            ccs = null;
            run = false;
            projectLayout.SaveProject();
        }

        public void RecycleComponent()
        {
            ObjectCacheManager.Cache(ccs);
            ccs = null;
            run = false;
            projectLayout.SaveProject();
        }

        public ScaffoldingComponent GetComponent()
        {
            return ComponentHolder.GetComponentBehindRay() as ScaffoldingComponent;
        }


        private void Update()
        {
            if (run == false)
                return;

            if (ccs.gameObject.activeSelf is false)
                ccs.gameObject.SetActive(true);


            if (HandleCollisionDetection(out Vector3 newPos, out Quaternion newEuler))
            {
                ccs.transform.position = newPos;
                ccs.transform.rotation = newEuler;
            }
        }



        private bool HandleCollisionDetection(out Vector3 pos, out Quaternion euler)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            CollisionInfo ci = CollisionDetectionService.Evaluate(ray, 0, ccs);

            if (ci.IsSuccess)
            {
                ConnectionDefinition cd = ci.Target.GetElementAt(ci.TargetConnectionIndex);
                pos = cd.CalculateWorldPosition(ci.TargetScaffoldingComponent.transform);
                euler = cd.CalculateRotation();

                if (ccs.transform.position != pos && ccs.transform.rotation != euler)
                {
                    AudioManager.instance.PlaySound(SoundID.Join); //playing joining sound
                }

                return true;
            }

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                pos = hit.point;
                euler = Quaternion.identity;
                return true;
            }

            pos = Vector3.zero;
            euler = Quaternion.identity;
            return false;
        }
    }
}
