using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    [CreateAssetMenu(fileName = nameof(ConnectionDefinition), menuName = "Component/Connection/Connection Definition", order = 0)]
    public class ConnectionDefinition : ScriptableObject
    {


        //think about make different connection type classes, but on the other hand some extra settings can be applied too.
        [SerializeField]
        internal Vector3 localPosition;
        [SerializeField]
        internal Vector3 lookAt;

        [SerializeField]
        internal ConnectionType connectionType;


       

        public void DebugDraw(Transform parent)
        {
            Gizmos.color = Color.blue;
            Vector3 worldPosition = CalculateWorldPosition(parent);

            Gizmos.DrawLine(parent.position, worldPosition);

            Vector3 lookAtPoint = CalculateHeading(parent.localRotation);


            Gizmos.color = Color.red;
            Gizmos.DrawLine(worldPosition, worldPosition + lookAtPoint);

        }

        public Vector3 CalculateWorldPosition(Transform parent)
        {
            return parent.rotation * localPosition + parent.position;
        }

        public Vector3 CalculateHeading(Quaternion rotation)
        {
            return rotation * lookAt;
        }
    }
}