using System;
using System.Collections.Generic;
using Assets.Scripts.Services.Cameras;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Services.Tools.Selector.Face;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.Shapes;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class ScaffoldingAssembly : HakiComponent
    {
        private BoxCollider boxCollider;
        [Inject] private IObjectCacheManager ObjectCacheManager { get; set; }
        [Inject] private IBoxFinder<ScaffoldingComponent> ComponentBoxFinder { get; set; }
        [Inject] private ICameraService CameraService { get; set; }
        [Inject] private ISelected<ScaffoldingAssembly> Selected { get; set; }
        [Inject] private IMaterialService MaterialService { get; set; }
        public InputHandler inputHandler;
        private MeshRenderer indicatorCube;

        public Vector3 Center => transform.position + boxCollider.center;
        private IList<ScaffoldingComponent> scaffoldingComponents;
        private float x, y, z;

        void Awake()
        {
            SetCollider();

            if (indicatorCube == null)
                indicatorCube = GetComponentInChildren<MeshRenderer>(true);
            indicatorCube.gameObject.SetActive(false);
        }

        public void RemoveHighlight()
        {
            if (Selected.TryGet(out ScaffoldingAssembly item) && item == this)
                return;
            indicatorCube.gameObject.SetActive(false);
        }

        public void Highlight()
        {
            Material m;
            if (Selected.TryGet(out ScaffoldingAssembly item) && item == this)
            {
                m = MaterialService.Selected;
            }
            else m = MaterialService.MouseOver;

            indicatorCube.sharedMaterial = m;
            indicatorCube.gameObject.SetActive(true);
        }

        void Start()
        {
            if (boxCollider == null)
            {

                var comp = GetComponent<BoxCollider>();
                if (comp == null)
                {
                    comp = this.gameObject.AddComponent<BoxCollider>();
                }

                boxCollider = comp;
            }
            if (indicatorCube == null)
                indicatorCube = GetComponentInChildren<MeshRenderer>();

            SetCollider();
        }

        void Update() => SetCollider();

        private void SetCollider()
        {
            if (boxCollider == null)
                return;

            if (Math.Abs(x) + Math.Abs(z) + Math.Abs(y) == 0)
                return;


            boxCollider.center = new Vector3(x / 2, y / 2, z / 2);
            boxCollider.size = new Vector3(x, y, z);
            indicatorCube.transform.localPosition = boxCollider.center;
            indicatorCube.transform.localScale = boxCollider.size;
        }


        public void SetDimensions(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override void OnCache(Transform newParent)
        {
            base.OnCache(newParent);

            ScaffoldingComponent[] childrens = GetComponentsInChildren<ScaffoldingComponent>();

            foreach (ScaffoldingComponent element in childrens)
            {
                ObjectCacheManager.Cache(element);
            }
        }

        public override bool TryGetCollectionDefinition(out ConnectionDefinitionCollection collection)
        {
            collection = null;

            return false;
        }

        public override Box GetBounds()
        {
            throw new NotImplementedException("This feature has not yet been implemented!");
        }


        public override void Select()
        {
            Selected.SetSelected(this);
            SetMaterial(inputHandler.selectedMat);
        }

        public override void Deselect()
        {
            Selected.SetSelected(null);
            SetMaterial(inputHandler.defaultMat);
        }
        
        public override void Write(int projectID, int no)
        {
            //will write it later
        }

        public override void Read(int projectID, int no)
        {
            //will write it later
        }
    }
}