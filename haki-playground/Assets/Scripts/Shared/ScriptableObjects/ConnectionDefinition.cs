using UnityEngine;

namespace Assets.Scripts.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(ConnectionDefinition), menuName = "ScaffoldingComponent/Connection/Connection Definition", order = 0)]
    public class ConnectionDefinition : ScriptableObject
    {


        //think about make different connection type classes, but on the other hand some extra settings can be applied too.
        [SerializeField] public Vector3 localPosition;
        [SerializeField] public Vector3 lookAt;

        [SerializeField] public ComponentConnectionInfo ComponentConnectionInfo;





        public void DebugDraw(Transform parent)
        {
            DebugDraw(parent, Color.red);
        }

        public Vector3 CalculateWorldPosition(Transform parent)
        {
            return parent.rotation * localPosition + parent.position;
        }

        public Vector3 CalculateHeading(Quaternion rotation)
        {
            return rotation * lookAt;
        }

        public void DebugDraw(Transform parent, Color color)
        {
            Vector3 worldPosition = CalculateWorldPosition(parent);

            Vector3 lookAtPoint = CalculateHeading(parent.localRotation);

            Gizmos.color = color;
            Gizmos.DrawLine(worldPosition, worldPosition + lookAtPoint);
        }

        public Quaternion CalculateRotation()
        {
            switch (ComponentConnectionInfo.rotationOrientation)
            {
                case ComponentConnectionInfo.RotationOrientation.Horizontal:
                    return Quaternion.FromToRotation(Vector3.back, lookAt);
                case ComponentConnectionInfo.RotationOrientation.Vertical:
                    return Quaternion.FromToRotation(Vector3.up, lookAt);
                default:
                    return Quaternion.identity;
            }
        }
    }
}