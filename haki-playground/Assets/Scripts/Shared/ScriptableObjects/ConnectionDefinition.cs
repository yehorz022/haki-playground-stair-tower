using Assets.Scripts.Shared.Helpers;
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

            //Gizmos.color = color;
            //Gizmos.DrawLine(worldPosition, worldPosition + lookAtPoint);
            Gizmo.DrawArrow(worldPosition, worldPosition + lookAtPoint, color);
            Gizmo.DrawWireCube(worldPosition + lookAtPoint * 1f / 4f, lookAtPoint, new Vector3(.05f, .05f, .1f), color, Direction.center);
            Gizmo.DrawWireCone(worldPosition + lookAtPoint * 2f / 4f, lookAtPoint, .1f, color, Direction.center);
            Gizmo.DrawWireCircle(worldPosition + lookAtPoint * 3f / 4f, lookAtPoint, .04f, color, Direction.center);
            Gizmo.DrawText("Hello", worldPosition + lookAtPoint, color, 12);

            Gizmo.DrawWireCube(new Vector3(2, 3, 0), Quaternion.Euler(250, 224, 10), new Vector3(.05f, .05f, .1f), Color.white, Direction.down);
            Gizmo.DrawWireCone(new Vector3(3, 2, 0), Quaternion.Euler(150, 24, 0), .1f, Color.magenta, Direction.right);
            Gizmo.DrawWireCircle(new Vector3(2, 2, 0), Quaternion.Euler(50, 124, 20), .1f, Color.cyan, Direction.up);
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