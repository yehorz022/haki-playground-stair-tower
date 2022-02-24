using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Shared.Interfaces;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentConnection
{
    public class ScaffoldingComponent : HakiComponent, IScaffoldingComponent
    {
        [SerializeField] public ConnectionDefinitionCollection ConnectionDefinitionCollection;
        public static ScaffoldingComponent selected;
        public InputHandler inputHandler;
        bool isDown;
        int touchCount;
        bool onRecycleBin;

        void Start()
        {
            inputHandler = FindObjectOfType<InputHandler>();
            AddMeshCollider(); // adding mesh collider to select the items again using raycast
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); // disable component raycast to detect floor easily
        }

        protected override void DebugDraw(bool isSelected)
        {
            if (ConnectionDefinitionCollection == null)
                return;

            for (int i = 0; i < ConnectionDefinitionCollection.Count; i++)
            {

                Color color = default;

                switch (i % 4)
                {
                    case 0:
                        color = Color.red;
                        break;
                    case 1:
                        color = Color.yellow;
                        break;
                    case 2:
                        color = Color.blue;
                        break;
                    case 3:
                        color = Color.green;
                        break;
                }

                ConnectionDefinitionCollection.GetElementAt(i).DebugDraw(transform, color);
            }
        }

        public void Select()
        {
            selected = this;
            SetMaterial(inputHandler.selectedMat);
        }

        public void Deselect()
        {
            selected = null;
            SetMaterial(inputHandler.defaultMat);
        }

        public void SetMaterial(Material mat)
        {
            SetMaterialRecursively(transform, mat);
        }

        public void AddMeshCollider()
        {
            AddMeshColliderRecursively(gameObject);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public ConnectionDefinitionCollection GetConnectionDefinitionCollection()
        {
            return ConnectionDefinitionCollection;
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
    }
}