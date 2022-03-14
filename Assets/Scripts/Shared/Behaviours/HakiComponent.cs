using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.Shapes;
using UnityEngine;

namespace Assets.Scripts.Shared.Behaviours
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class HakiComponent : DebugBehaviour
    {
        public int id;

        public Vector3 Position
        {
            get { return transform.position; }
            set
            {
                if (value == Vector3.zero)
                {

                }
                transform.position = value;
            }
        }
        public Quaternion Rotation => transform.rotation;

        protected void Activate() => gameObject.SetActive(true);
        protected void Deactivate() => gameObject.SetActive(false);

        public virtual void OnCache(Transform newParent)
        {
            transform.SetParent(newParent);
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            Deactivate();
        }

        public virtual void OnInitialize(Transform newParent)
        {
            transform.SetParent(newParent);
            Activate();
        }


        public abstract bool TryGetCollectionDefinition(out ConnectionDefinitionCollection collection);

        public abstract Box GetBounds();


        public abstract void Select();
        //{
        //    Selected.SetSelected(this);
        //    SetMaterial(inputHandler.selectedMat);
        //}

        public abstract void Deselect();
        //{
        //    Selected.SetSelected(null);
        //    SetMaterial(inputHandler.defaultMat);
        //}

        public void SetMaterial(Material mat)
        {
            SetMaterialRecursively(transform, mat);
        }

        public void AddMeshCollider()
        {
            AddMeshColliderRecursively(gameObject);
        }

        void AddMeshColliderRecursively(GameObject go)
        {
            if (go.GetComponent<MeshRenderer>() != null)
            {
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.center = go.GetComponent<MeshRenderer>().bounds.center - transform.position;
                boxCollider.size = go.GetComponent<MeshRenderer>().bounds.size;
            }
            for (int i = 0; i < go.transform.childCount; i++)
                AddMeshColliderRecursively(go.transform.GetChild(i).gameObject);
        }

        void SetMaterialRecursively(Transform trans, Material mat)
        {
            if (trans.GetComponent<MeshRenderer>() != null)
                trans.GetComponent<MeshRenderer>().material = mat;

            for (int i = 0; i < trans.transform.childCount; i++)
                SetMaterialRecursively(trans.GetChild(i), mat);
        }


        public abstract void Read(string projectID, int no);

        public abstract void Write(string projectID, int no);
    }
}