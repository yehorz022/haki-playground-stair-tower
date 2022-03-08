using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.Shapes;
using UnityEngine;

namespace Assets.Scripts.Shared.Behaviours
{
    public abstract class HakiComponent : DebugBehaviour
    {
        public int id;
        public Vector3 Position => transform.position;
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

        public abstract void Read(int projectID, int no);

        public abstract void Write(int projectID, int no);
    }
}