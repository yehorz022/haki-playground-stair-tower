using Assets.Scripts.RunMode.ComponentConnection;
using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.RunMode.DependencyInjection;
using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.InputService;
using Assets.Scripts.Shared.Containers.Collision;
using Assets.Scripts.Shared.Interfaces;
using Assets.Scripts.Shared.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.RunMode.PositionProvider
{
    public class PositionProvider : HakiComponent
    {
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
            ocm = FindObjectOfType<ObjectCacheManager>();

            DependancyInjectionManager dis = FindObjectOfType<DependancyInjectionManager>();

            dis.InjectDependencies(this);

            floor.SetActive(true);
        }


        private bool run;
        private ScaffoldingComponent ccs;
        public ScaffoldingComponent CreateAndPickComponent(ScaffoldingComponent replacement) // made create 
        {
            ccs = ocm.Instantiate(replacement, transform);
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
        }

        public void RecycleComponent()
        {
            ocm.Cache(ccs);
            ccs = null;
            run = false;
        }

        public ScaffoldingComponent GetComponent()
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


            if (HandleCollisionDetection(out Vector3 newPos, out Quaternion newEuler))
            {
                ccs.transform.position = newPos;
                ccs.transform.rotation = newEuler;
            }
        }

        private bool HandleCollisionDetection(out Vector3 pos, out Quaternion euler)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            CollisionInfo ci = collisionDetectionService.Evaluate(ray, ccs.ConnectionDefinitionCollection, 0, ccs);

            if (ci.IsSuccess)
            {
                ConnectionDefinition cd = ci.Target.GetElementAt(ci.TargetConnectionIndex);
                Vector3 newPos = cd.CalculateWorldPosition(ci.TargetScaffoldingComponent.GetTransform());
                Quaternion newEuler = cd.CalculateRotation();

                if (ccs.transform.position != newPos && ccs.transform.rotation != newEuler)
                {
                    AudioManager.instance.PlaySound(SoundID.Join); //playing joining sound
                }
                pos = newPos;
                euler = newEuler;
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
