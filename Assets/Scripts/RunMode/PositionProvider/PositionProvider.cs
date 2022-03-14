using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Containers.Collision;
using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Helpers;
using Codice.Client.GameUI.Status;
using UnityEngine;

namespace Assets.Scripts.RunMode.PositionProvider
{
    public class PositionProvider : SceneMemberInjectDependencies
    {

        private bool run;
        private HakiComponent ccs;

        [Inject] public IObjectCacheManager ObjectCacheManager { get; set; }

        [SerializeField] private GameObject floor;

        public Transform origin;

        [Inject] public IComponentHolder ComponentHolder { get; set; }

        [Inject] private IComponentCollisionDetectionService CollisionDetectionService { get; set; }
        private ProjectLayout projectLayout;
        private bool isAssembly = false;
        void Start()
        {
            projectLayout = FindObjectOfType<ProjectLayout>();
            floor.SetActive(true);
        }


        public HakiComponent CreateAndPickComponent(HakiComponent replacement) // made create 
        {
            ccs = ObjectCacheManager.Instantiate(replacement, origin);
            isAssembly = ccs.GetType() == typeof(ScaffoldingAssembly);
            run = true;
            return ccs;
        }

        public void PickComponent(HakiComponent component)
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
        }

        public void RecycleComponent()
        {
            ObjectCacheManager.Cache(ccs);
            ccs = null;
            run = false;
        }

        public ScaffoldingComponent GetComponent()
        {
            return ComponentHolder?.GetComponentBehindRay() as ScaffoldingComponent;
        }


        private void Update()
        {
            if (run == false)
                return;

            if (ccs.gameObject.activeSelf is false)
                ccs.gameObject.SetActive(true);

            if (isAssembly)
            {


                if (HandleCollisionDetectionForScaffoldingAssembly(out Vector3 newPos, out Quaternion newEuler))
                {
                    ccs.Position = newPos;
                    ccs.transform.rotation = newEuler;
                }
            }
            else
            {
                if (HandleCollisionDetectionForScaffoldingComponent(out Vector3 newPos, out Quaternion newEuler))
                {
                    ccs.Position = newPos;
                    ccs.transform.rotation = newEuler;
                }
            }
        }

        private bool HandleCollisionDetectionForScaffoldingAssembly(out Vector3 position, out Quaternion quaternion)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] results = Physics.RaycastAll(ray);

            if (results.Length > 0)
            {
                foreach (RaycastHit hit in results)
                {
                    ScaffoldingAssembly assembly = hit.transform.gameObject.GetComponent<ScaffoldingAssembly>();
                    if (assembly == null)
                        continue;

                    if (assembly.GetInstanceID() == ccs.GetInstanceID())
                        continue;

                    Vector3 size = assembly.Size;

                    Vector3 n = hit.normal;
                    position = assembly.Position + new Vector3(n.x * size.x, n.y * size.y, n.z * size.z);


                    quaternion = Quaternion.identity;
                    return true;
                }
            }

            if (Intersections.RayPlaneIntersection(ray, Vector3.up, Vector3.zero, out Vector3 hitPoint))
            {
                position = hitPoint;
                quaternion = Quaternion.identity;
                return true;
            }

            //this should never happen
            position = Vector3.zero;
            quaternion = Quaternion.identity;
            return false;
        }

        private bool HandleCollisionDetectionForScaffoldingComponent(out Vector3 pos, out Quaternion euler)
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



            if (Intersections.RayPlaneIntersection(ray, Vector3.up, Vector3.zero, out Vector3 hitPoint))
            {
                pos = hitPoint;
                euler = Quaternion.identity;
                return true;
            }

            pos = Vector3.zero;
            euler = Quaternion.identity;
            return false;
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                projectLayout.SaveProject();
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                projectLayout.SaveProject();
            }
        }
    }
}
